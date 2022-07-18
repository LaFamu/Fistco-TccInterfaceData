using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media;
using System.ComponentModel;
using System.Diagnostics;
using FirstFloor.ModernUI.Windows.Controls;
using JSDG.Platform.Util;

namespace JSDG.Platform.Content
{
    /// <summary>
    /// ETBDataGeneratorUC.xaml 的交互逻辑
    /// </summary>
    public partial class ETBDataGeneratorUC : System.Windows.Controls.UserControl
    {
        private BackgroundWorker _bgw;
        private bool _isSuccess;
        private string _outputDirectory;
        private string[] fileNamesA;
        private string[] fileNamesB;
        private string _outputPathA;
        private string _outputPathB;
        private Dictionary<int, bool> _abCompletedDic;
        
        #region 文本框取值
        private String _inputPath
        {
            get { return this.InputPathTextBox.Dispatcher.Invoke(() => InputPathTextBox.Text); }
        }

        private String _outputPath
        {
            get { return this.OutputPathTextBox.Dispatcher.Invoke(() => OutputPathTextBox.Text); }
        }

        private Int32 _index
        {
            get { return this.ComboxState.Dispatcher.Invoke(() => ComboxState.SelectedIndex); }
        }

        private String _inputPathA
        {
            get { return this.ADataPathTextBox.Dispatcher.Invoke(() => ADataPathTextBox.Text); }
        }

        private String _inputPathB
        {
            get { return this.BDataPathTextBox.Dispatcher.Invoke(() => BDataPathTextBox.Text); }
        }

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public ETBDataGeneratorUC()
        {
            InitializeComponent();
            InitBackgroundWorker();
            this.AlarmRichTextBox.TextChanged += (sender, args) =>
            {
                //滚动到控件内容末尾
                this.AlarmRichTextBox.ScrollToEnd();
            };
            _abCompletedDic = new Dictionary<int, bool>();
            _abCompletedDic.Add(0, false);
            _abCompletedDic.Add(1, false);
            Debug();
        }

        [Conditional("DEBUG")]
        public void Debug()
        {
            this.InputPathTextBox.Text = @"C:\Users\gongzhiqiao\Desktop\Qcr新增功能的数据更新_0518 release\Tcc接口数据表_同济站";
            this.OutputPathTextBox.Text = @"C:\Users\gongzhiqiao\Desktop\Qcr新增功能的数据更新_0518 release\Tcc接口数据表_同济站";
        }

        /// <summary>
        /// 初始化BackgroundWorker
        /// </summary>
        private void InitBackgroundWorker()
        {
            _bgw = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            _bgw.DoWork += bgw_DoWork;
            _bgw.RunWorkerCompleted += bgw_RunWorkerCompleted;
            _bgw.ProgressChanged += bgw_ProgressChanged;
        }

        /// <summary>
        /// 写入报警界面和日志中
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
        private void WriteToAlarmAndLog(String message, SolidColorBrush color)
        {
            this.AlarmRichTextBox.Dispatcher.Invoke(
                () =>
                    AlarmRichTextBox.Document.Blocks.Add(
                    new Paragraph(
                        new Run(message) { Foreground = color })));
            LogHelper.Instance.Log(message);
        }

        /// <summary>
        /// 在RichTextBox上显示报警信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var message = e.UserState.ToString();
            if (e.ProgressPercentage == 1)
            {
                LogHelper.Instance.Log(message);
                WriteToAlarmAndLog(message, Brushes.Gray);
            }
            else if (e.ProgressPercentage == -1)
            {
                _isSuccess = false;
                WriteToAlarmAndLog(message, Brushes.Red);
            }
            else
            {
                WriteToAlarmAndLog(message, Brushes.Gray);
            }
        }

        /// <summary>
        /// 当任务执行结束时，显示完成情况信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                String message = String.Format("C2T通讯数据转换失败，发生以下错误：\r\n{0}", e.Error.Message);
                LogHelper.Instance.Log(String.Format("{0}\r\nStack Trace:{1}", message, e.Error.StackTrace));
                ModernDialog.ShowMessage(message, "温馨提示", MessageBoxButton.OK);
            }
            else if (!_isSuccess)
            {
                ModernDialog.ShowMessage("C2T通讯数据转换失败！              ", "温馨提示", MessageBoxButton.OK);
            }
            else
            {
                WriteToAlarmAndLog(string.Format("{0}: 数据生成结束。", DateTime.Now.ToString()), Brushes.Gray);
                WriteToAlarmAndLog(string.Format("数据存放路径为：{0}", _outputPath), Brushes.Gray);
                ModernDialog.ShowMessage("C2T通讯数据转换成功！              ", "温馨提示", MessageBoxButton.OK);
                ADataPathTextBox.Text = _outputPathA;
                BDataPathTextBox.Text = _outputPathB;
                _abCompletedDic[_index] = true;
            }
            ComboxState.IsEnabled = true;
        }

        /// <summary>
        /// 开始执行任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            //_outputDirectory = string.Format("CBI_Data_{0}", DateTime.Now.ToString("yyyyMMdd"));
            _outputDirectory = string.Format("C2T_{0}", DateTime.Now.ToString("yyyyMMdd"));
            if (_index == 0)
            {
                if (App.Container.Act_JSDG_CBI_A == null) throw new Exception("缺少DLL文件");
                fileNamesA = FileNamesHelper.GetFileNames(_inputPath, ESubSystemType.JSDG_CBI_A);
                _outputPathA = String.Format("{0}\\{1}\\A\\", _outputPath, _outputDirectory);
                LogHelper.Instance.Init(ESubSystemType.JSDG_CBI_A, _outputPathA);
             
                WriteToAlarmAndLog(string.Format("{0}: 数据生成开始：", DateTime.Now.ToString()), Brushes.Gray);
                App.Container.Act_JSDG_CBI_A(_outputPathA, fileNamesA, _bgw.ReportProgress);
            }
            else
            {
                if (App.Container.Act_JSDG_CBI_B == null) throw new Exception("缺少DLL文件");
                fileNamesB = FileNamesHelper.GetFileNames(_inputPath, ESubSystemType.JSDG_CBI_B);
                _outputPathB = String.Format("{0}\\{1}\\B\\", _outputPath, _outputDirectory);
                LogHelper.Instance.Init(ESubSystemType.JSDG_CBI_B, _outputPathB);
               
                WriteToAlarmAndLog(string.Format("{0}: 数据生成开始：", DateTime.Now.ToString()), Brushes.Gray);
                App.Container.Act_JSDG_CBI_B(_outputPathB, fileNamesB, _bgw.ReportProgress);
            }
        }


        /// <summary>
        /// 选择输入路径按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputPathButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "请选择源数据文件夹";
            fbd.SelectedPath = InputPathTextBox.Text;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                InputPathTextBox.Text = fbd.SelectedPath;
            }
        }

        /// <summary>
        /// 选择输出路径按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutputPathButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "请选择输出路径文件夹";
            fbd.SelectedPath = OutputPathTextBox.Text;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                OutputPathTextBox.Text = fbd.SelectedPath;
            }
        }

        /// <summary>
        /// 生成数据按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGenerateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _isSuccess = true;
                _abCompletedDic[_index] = false;
                ComboxState.IsEnabled = false;
                this.AlarmRichTextBox.Document.Blocks.Clear();
                if (string.IsNullOrEmpty(InputPathTextBox.Text))
                {
                    ModernDialog.ShowMessage("请选择源数据文件夹！              ", "温馨提示", MessageBoxButton.OK);
                    return;
                }
                if (string.IsNullOrEmpty(OutputPathTextBox.Text))
                {
                    ModernDialog.ShowMessage("请选择输出路径文件夹！                ", "温馨提示", MessageBoxButton.OK);
                    return;
                }
                if (!_bgw.IsBusy)
                {
                    this._bgw.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                ComboxState.IsEnabled = true;
                ModernDialog.ShowMessage(ex.Message, "温馨提示", MessageBoxButton.OK);
            }
        }

        private void ADataPathButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "请选择A链数据文件夹";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                ADataPathTextBox.Text = fbd.SelectedPath;
            }
        }

        private void BDataPathButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "请选择B链数据文件夹";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                BDataPathTextBox.Text = fbd.SelectedPath;
            }
        }

        private void DataCompareButton_Click(object sender, RoutedEventArgs e)
        {
            var comp = new BinaryCompare(AlarmRichTextBox);
            if (!CBICompareCheckBox.IsChecked.HasValue) //check for a value
            {
                CBICompareCheckBox.IsChecked = false;
            }
            BinaryCompare._CBICheckBoxChecked = (bool)CBICompareCheckBox.IsChecked;
            comp.ReadyToCompare(
                _inputPathA,
                _inputPathB,
                ESubSystemType.JSDG_CBI_A,
                ESubSystemType.JSDG_CBI_B,
                fileNamesA,
                fileNamesB,
                _outputDirectory,
                _outputPath,
                _abCompletedDic);
        }

        private void InputPathTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
