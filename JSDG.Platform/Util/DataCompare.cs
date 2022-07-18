using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using FirstFloor.ModernUI.Windows.Controls;

namespace JSDG.Platform.Util
{
    public class BinaryCompare
    {
        private BackgroundWorker _bgw;
        private RichTextBox _richTextBox;
        private String _aPath;
        private String _bPath;

        private GenerateCompareResult GenerateCompareResult;
        private ESubSystemType _subSystemTypeA;
        private ESubSystemType _subSystemTypeB;
        private string[] _fileNamesA;
        private string[] _fileNamesB;
        private string _outputDirectory;
        private string _outputPath;
        private readonly List<String> _outputFilesNamesA;
        private readonly List<String> _outputFilesNamesB;
        private Dictionary<int, bool> _abCompletedDic;
        public static bool _CBICheckBoxChecked;
        private const int CBI_NoAB_StartCompareLen = 32;
        public bool CompareResult { get; private set; }

        public BinaryCompare(RichTextBox richTextBox)
        {
            _richTextBox = richTextBox;
            _bgw = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            _bgw.DoWork += bgw_DoWork;
            _bgw.RunWorkerCompleted += bgw_RunWorkerCompleted;
            GenerateCompareResult = new GenerateCompareResult();
            _outputFilesNamesA = new List<String>();
            _outputFilesNamesB = new List<String>();
            CompareResult = false;
        }

        private void WriteToAlarm(String message, SolidColorBrush color)
        {
            this._richTextBox.Dispatcher.Invoke(
                () =>
                    _richTextBox.Document.Blocks.Add(
                    new Paragraph(
                        new Run(message) { Foreground = color })));
        }

        private string GetLastDoubleName(String fileName)
        {

            int index = 0;
            string lastDoubleName = "";
            if (fileName.Contains("\\A\\"))
            {
                index = fileName.IndexOf("\\A\\");
            }
            else
            {
                index = fileName.IndexOf("\\B\\");
            }
            lastDoubleName = fileName.Substring(index + 3);
            return lastDoubleName;
        }

        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            Stack<string> stackFileNameA = new Stack<string>();
            Stack<string> stackFileNameB = new Stack<string>();
            Queue<string> queueFilePathA = new Queue<string>();
            Queue<string> queueFilePathB = new Queue<string>();

            Dictionary<string, string> poctNameDicA = new Dictionary<string,string>();//键为最后一级文件夹和文件名，值为完整文件名
            Dictionary<string, string> poctNameDicB = new Dictionary<string,string>();

            //添加A链数据
            stackFileNameA.Push(_aPath);
            do
            {
                string strFileNameA = stackFileNameA.Pop();
                if (Directory.Exists(strFileNameA))
                {
                    DirectoryInfo folderA = new DirectoryInfo(strFileNameA);
                    //判断folderA是不是底层目录
                    if (folderA.GetDirectories().Length > 0)
                    {
                        foreach (DirectoryInfo nextFolder in folderA.GetDirectories())
                        {
                            stackFileNameA.Push(nextFolder.FullName);
                        }
                    }
                    else
                    {
                        GetFiles(folderA, ref queueFilePathA, ref poctNameDicA);
                    }
                }
                else if (File.Exists(strFileNameA))
                {
                    queueFilePathA.Enqueue(strFileNameA);
                }

            } while (stackFileNameA.Any());

            //添加B链数据
            stackFileNameB.Push(_bPath);
            do
            {
                string strFileNameB = stackFileNameB.Pop();
                if (Directory.Exists(strFileNameB))
                {
                    DirectoryInfo folderB = new DirectoryInfo(strFileNameB);
                    if (folderB.GetDirectories().Length > 0)
                    {
                        foreach (DirectoryInfo nextFolder in folderB.GetDirectories())
                        {
                            stackFileNameB.Push(nextFolder.FullName);
                        }
                    }
                    else
                    {
                        GetFiles(folderB, ref queueFilePathB, ref poctNameDicB);
                    }
                }
                else if (File.Exists(strFileNameB))
                {
                    queueFilePathB.Enqueue(strFileNameB);
                }
            } while (stackFileNameB.Any());

            _outputFilesNamesA.AddRange(queueFilePathA.Select(m => m.Split('\\').Last()));
            _outputFilesNamesB.AddRange(queueFilePathB.Select(m => m.Split('\\').Last()));
            var exceptA = _outputFilesNamesA.Except(_outputFilesNamesB).ToList();
            var exceptB = _outputFilesNamesB.Except(_outputFilesNamesA).ToList();
            if (queueFilePathA.Count != queueFilePathB.Count || poctNameDicA.Count != poctNameDicB.Count)
            {
                WriteToAlarm("两链数据文件个数不同", Brushes.Red);
            }
            else if (exceptA.Any() || exceptB.Any())
            {
                exceptA.ForEach(m => WriteToAlarm(string.Format("A链数据文件中有不匹配的文件: {0}", m), Brushes.Red));
                exceptB.ForEach(m => WriteToAlarm(string.Format("B链数据文件中有不匹配的文件: {0}", m), Brushes.Red));
            }
            else
            {
                List<bool> resultList = new List<bool>();
                bool result = true;
                while (queueFilePathA.Count != 0 && queueFilePathB.Count != 0)
                {
                    resultList.Add(CompareBin(queueFilePathA.Dequeue(), queueFilePathB.Dequeue()));
                }
                if (poctNameDicA.Count != 0 && poctNameDicB.Count != 0)
                {
                    foreach (var nameKey in poctNameDicA.Keys.ToList())
                    {
                        string nameValue = poctNameDicB.Values.ToList().FirstOrDefault(x => x.Contains(nameKey));
                        if (nameValue == null)
                        {
                            WriteToAlarm(string.Format("POCT生成文件A链中的{0}在B链不存在", nameKey), Brushes.Red);
                        }
                        else
                        {
                            result = TextCompare(poctNameDicA[nameKey], poctNameDicB[nameKey]);
                        }

                        resultList.Add(result);
                    }  
                }
                if (resultList.Count == 0)
                {
                    WriteToAlarm("两链数据文件均为空目录！！！", Brushes.Red);
                }
                else
                {
                    if (resultList.All(m => m == true))
                    {
                        WriteToAlarm("两链数据最终比较结果：一致", Brushes.Gray);
                        CompareResult = true;
                    }
                    else
                    {
                        WriteToAlarm("两链数据最终比较结果：不同", Brushes.Red);
                    }
                }
            }
            if (_abCompletedDic.All(m => m.Value))
            {
                var boxResult =
                    System.Windows.Application.Current.Dispatcher.Invoke(
                        () =>
                        ModernDialog.ShowMessage("是否生成子系统数据生成比较结果表格？              ", "温馨提示", MessageBoxButton.OKCancel));
                if (boxResult == MessageBoxResult.OK)
                {
                    GenerateCompareResult.StartGenerateCompareResult(_subSystemTypeA, _subSystemTypeB, _fileNamesA,
                                                                     _fileNamesB,
                                                                     _outputFilesNamesA, _outputFilesNamesB,
                                                                     _outputDirectory,
                                                                     CompareResult, _outputPath);
                    WriteToAlarm(string.Format("\r\n 子系统数据生成比较结果表格存放至: {0}\\{1}", _outputPath, _outputDirectory),
                                 Brushes.Gray);
                }
            }
        }

        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                WriteToAlarm(String.Format("数据比较过程中发生异常\r\nException:\r\n{0}", e.Error.Message), Brushes.Red);
            }
        }

        public void ReadyToCompare(
            String APath, 
            String BPath,
            ESubSystemType a, 
            ESubSystemType b, 
            string[] fileNamesA,
            string[] fileNamesB,
            string outputDirectory,
            string outputPath,
            Dictionary<int, bool> abCompletedDic)
        {
            _aPath = APath;
            _bPath = BPath;
            _subSystemTypeA = a;
            _subSystemTypeB = b;
            _fileNamesA = fileNamesA;
            _fileNamesB = fileNamesB;
            _outputDirectory = outputDirectory;
            _outputPath = outputPath;
            _abCompletedDic = abCompletedDic;

            this._richTextBox.Document.Blocks.Clear();
            if (String.IsNullOrEmpty(APath))
            {
                ModernDialog.ShowMessage("请选择A链数据文件夹              ", "温馨提示", MessageBoxButton.OK);
                return;
            }
            if (String.IsNullOrEmpty(BPath))
            {
                ModernDialog.ShowMessage("请选择B链数据文件夹                ", "温馨提示", MessageBoxButton.OK);
                return;
            }

            WriteToAlarm("两链数据比较开始:\r\n", Brushes.Gray);
            if (!this._bgw.IsBusy)
                this._bgw.RunWorkerAsync();
        }

        private void GetFiles(DirectoryInfo dir, ref Queue<string> queueFilePath, ref Dictionary<string, string> doubleName)
        {
            var files = dir.GetFiles();
            foreach (FileInfo nextFile in files)
            {
                switch (_subSystemTypeA)
                {
                    case  ESubSystemType.JSDG_POCT_A:
                    case  ESubSystemType.JSDG_POCT_B:
                        if (nextFile.Extension == ".cds")
                        {
                            queueFilePath.Enqueue(nextFile.FullName);
                        }
                        else if (nextFile.Extension == ".ini" || nextFile.Extension == "" || nextFile.Extension == ".cfg")
                        {
                            doubleName.Add(GetLastDoubleName(nextFile.FullName), nextFile.FullName);
                        }
                        break;
                    case ESubSystemType.JSDG_CBI_A:
                    case ESubSystemType.JSDG_CBI_B:
                        if (nextFile.Extension == ".dat")
                            queueFilePath.Enqueue(nextFile.FullName);
                        break;
                    case ESubSystemType.JSDG_OBCU_A:
                    case ESubSystemType.JSDG_OBCU_B:
                        if (nextFile.Extension == "")
                            queueFilePath.Enqueue(nextFile.FullName);
                        break;
                    default:
                        break;
                }
            }
        }

        private Boolean CompareBin(String str1, String str2)
        {
            //逐字节比较
            WriteToAlarm("文件A：" + str1 + "\r\n文件B：" + str2 + "\r\n", Brushes.Gray);
            if (str1.Split('\\').Last() != str2.Split('\\').Last())
            {
                WriteToAlarm("文件A与文件B文件名称不一致！",Brushes.Red);
                return false;
            }
            using (FileStream fsA = new FileStream(str1, FileMode.Open, FileAccess.Read))
            {
                using (FileStream fsB = new FileStream(str2, FileMode.Open, FileAccess.Read))
                {
                    Int64 fsaLen = fsA.Length;
                    Int64 fsbLen = fsB.Length;
                    if (fsaLen != fsbLen)
                    {
                        WriteToAlarm("两链数据有差异！", Brushes.Red);
                        return false;
                    }
                    else
                    {
                        int pos = 0;
                        bool CBICompareWithNoAB = false;
                        switch (_subSystemTypeA)
                        {
                            case ESubSystemType.JSDG_CBI_A:
                            case ESubSystemType.JSDG_CBI_B:
                                CBICompareWithNoAB = _CBICheckBoxChecked;
                                break;
                            default:
                                break;
                        }
                        while (pos < fsaLen)
                        {
                            int na = fsA.ReadByte();
                            int nb = fsB.ReadByte();
                            pos++;

                            // Break when the end of the file is reached.
                            if (na == -1 || nb == -1)
                                break;
                            else if (na != nb)
                            {
                                if (CBICompareWithNoAB)
                                {
                                    if (CBI_NoAB_StartCompareLen < pos && pos < fsaLen - 16)
                                    {
                                        WriteToAlarm(String.Format("两个文件数据内容有差异！第{0}个字节不同", pos), Brushes.Red);
                                        return false;
                                    }
                                }
                                else
                                {
                                    WriteToAlarm(String.Format("两个文件数据内容有差异！第{0}个字节不同", pos), Brushes.Red);
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            WriteToAlarm("两个文件数据内容相同！", Brushes.Gray);
            return true;
        }

        /// <summary>
        /// 比较文本文件
        /// </summary>
        private Boolean TextCompare(String stra, String strb)
        {
            WriteToAlarm("文件A：" + stra + "\r\n文件B：" + strb + "\r\n", Brushes.Gray);
            if (stra.Split('\\').Last() != strb.Split('\\').Last())
            {
                WriteToAlarm("文件A与文件B文件名称不一致！", Brushes.Red);
                return false;
            }

            var textA = ReadText(stra);
            textA.Values.ToList().ForEach( x => x.ForEach(y => y.Replace(" ","")));
            var textB = ReadText(strb);
            textB.Values.ToList().ForEach(x => x.ForEach(y => y.Replace(" ", "")));

            foreach (var module in textA)
            {
                string nameB = textB.Keys.ToList().FirstOrDefault(x => x == module.Key);
                if (nameB == null)
                {
                    WriteToAlarm(string.Format("文件A中的模块{0}在文件B中不存在", module.Key), Brushes.Red);
                    return false;
                }
                var moduleB = textB[nameB];
                if (module.Value != null)
                {
                    foreach (var line in module.Value)
                    {
                        if (line.Contains("Version:"))
                        {
                            var doubleAString = line.Split(':').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                            var valueB = moduleB.FirstOrDefault(x => x.Contains(doubleAString[0]));
                            if (valueB == null)
                            {
                                WriteToAlarm(string.Format("文件A中{0}的{1}在文件B中不存在", module.Key, doubleAString[0]), Brushes.Red);
                                return false;
                            }
                            else
                            {
                                var doubleBString = valueB.Split(':').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                                if (doubleAString.Count > 1 && (doubleBString[1] != doubleAString[1]))
                                {
                                    WriteToAlarm(string.Format("文件A中{0}的{1}与文件B中不相等", module.Key, doubleAString[0]), Brushes.Red);
                                    return false;
                                }
                            }
                        }
                        else if (line.Contains("="))
                        {
                            var doubleAString = line.Split('=', '/').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                            var valueB = moduleB.FirstOrDefault(x => x.Contains(doubleAString[0]));
                            if (valueB == null)
                            {
                                WriteToAlarm(string.Format("文件A中{0}的{1}在文件B中不存在", module.Key, doubleAString[0]), Brushes.Red);
                                return false;
                            }
                            else
                            {
                                var doubleBString = valueB.Split('=', '/').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                                if (doubleAString.Count > 1 && (doubleBString[1] != doubleAString[1]))
                                {
                                    WriteToAlarm(string.Format("文件A中{0}的{1}与文件B中不相等", module.Key, doubleAString[0]), Brushes.Red);
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
                
            WriteToAlarm("两个文件数据内容相同！", Brushes.Gray);
            return true;
        }

        /// <summary>
        /// 读取文本文件，并按模块区分
        /// </summary>
        private Dictionary<string, List<string>> ReadText(String filePath)
        {

            Dictionary<string, List<string>> allText = new Dictionary<string, List<string>>();
            //List<string> lineString = new List<string>();//记录一个模块下面的多行
            HashSet<string> lineString = new HashSet<string>();//记录一个模块下面的多行

            using (StreamReader sr = new StreamReader(filePath, new UTF8Encoding(false)))
            {
                string line = "";
                string textTitle = "Tile";
                string rackTitle = "";//记录槽道

                while ((line = sr.ReadLine()) != null)
                {
                    if (line.ToString() != "")
                    {
                        if (line.ToString().Contains("[") || line.ToString().Contains("[")
                            || (line.ToString().Contains("#") && !line.ToString().Contains("==") && !line.ToString().Contains("-")
                            && !line.ToString().Contains("*") && filePath.Contains(".cfg")))//为了保证模块名准确性
                        {
                            if (!allText.Keys.Contains(textTitle))
                            {
                                allText.Add(textTitle, lineString.ToList());
                                lineString.Clear();
                            }
                            
                            if (allText.Count() != 0)
                            {
                                if (line.Contains("]"))
                                {
                                    textTitle = line.ToString().Substring(0, line.IndexOf(']') + 1);
                                    if (textTitle.Contains("RACK"))
                                    {
                                        rackTitle = textTitle.Replace("[", string.Empty).Replace("]", string.Empty);
                                    }
                                    if (textTitle.Contains("SLOT"))
                                    {
                                        textTitle = textTitle.Insert(1, rackTitle + "-");//给槽道附上层
                                    }
                                }
                                else
                                {
                                    textTitle = line.ToString();
                                }
                            }
                        }
                        else if(!line.ToString().Contains("==="))
                        {
                            string changeString = line.ToString().Replace(" ", string.Empty).Replace("\t", string.Empty).Replace("*", string.Empty);
                            if(!lineString.Contains(changeString))
                            {
                                lineString.Add(changeString);
                            } 
                        }
                    }
                }
            }

            return allText;
        }

    }
}
