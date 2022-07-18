using System;
using System.Configuration;
using System.IO;

namespace JSDG.Platform.Util
{
    /// <summary>
    /// 文件帮助类
    /// </summary>
    public class FileNamesHelper
    {
        public static string[] GetFileNames(string inputPath, ESubSystemType subSystemType)
        {
            //var filenames = new String[2];
            var i = 0;
            var j = 0;
            var xlsfilesNames = String.Empty;
            var xlsxfileNames = String.Empty;
            //var isReadStatus = true;
            var xlsFiles = Directory.GetFiles(inputPath);
            //if (null != xlsFiles)
            //{
            //    foreach (string item in xlsFiles)
            //    {
            //        //item = item.ToUpper();
            //    }
            //}
            //string[] xlsFilesdic = null;
            //switch (subSystemType)
            //{
            //    case ESubSystemType.JSDG_CBI_A:
            //    case ESubSystemType.JSDG_CBI_B:
            //        filenames = new String[2];
            //        xlsFilesdic = new[]
            //        {
            //            String.Format("{0}", ConfigurationManager.AppSettings["CommC2TFileName"]),
            //        };
            //        break;
            //    case ESubSystemType.TCC_TCC_A:
            //        filenames = new String[2];
            //        xlsFilesdic = new[]
            //        {
            //            String.Format("{0}", ConfigurationManager.AppSettings["CommT2TFileName"]),
            //        };
            //        break;
            //    case ESubSystemType.TccBit:
            //        filenames = new string[xlsFiles.Length];
            //        //xlsFilesdic = new[]
            //        //{
            //        //    String.Format("{0}", ConfigurationManager.AppSettings["CommT2TFileName"]),
            //        //    String.Format("{0}", ConfigurationManager.AppSettings["CommT2CFileName"]),
            //        //    String.Format("{0}", ConfigurationManager.AppSettings["CommT2CtcFileName"]),
            //        //    String.Format("{0}", ConfigurationManager.AppSettings["CommT2TsrsFileName"])
            //        //};
            //        break;
            //}

            //foreach (var filenameend in xlsFilesdic)
            //for(int index = 0; index < filenames.Length;index++)
            //{
            //    j = Array.FindIndex<String>(xlsFiles,
            //        a =>
            //            //a.Substring(a.LastIndexOf("\\") + 1, a.Length - a.LastIndexOf("\\") - 1)
            //            //    .Contains(string.Format("{0}", filenameend)) &&
            //            (a.Substring(a.LastIndexOf("\\") + 1, a.Length - a.LastIndexOf("\\") - 1).EndsWith(".xls") ||
            //             a.Substring(a.LastIndexOf("\\") + 1, a.Length - a.LastIndexOf("\\") - 1).EndsWith(".xlsx") ||
            //             a.Substring(a.LastIndexOf("\\") + 1, a.Length - a.LastIndexOf("\\") - 1).EndsWith(".XLS") ||
            //             a.Substring(a.LastIndexOf("\\") + 1, a.Length - a.LastIndexOf("\\") - 1).EndsWith(".XLSX") ||
            //             a.Substring(a.LastIndexOf("\\") + 1, a.Length - a.LastIndexOf("\\") - 1).EndsWith(".xlsm")));
            //    if (j > -1)
            //    {
            //        filenames[i] = xlsFiles[j];
            //        i++;
            //    }
            //    //else if (Array.FindIndex<String>(xlsFiles, a => a.Contains(string.Format("{0}", filenameend))) > -1)
            //    //{
            //    //    j = Array.FindIndex<String>(xlsFiles, a => a.Contains(string.Format("{0}", filenameend)));
            //    //    filenames[i] = xlsFiles[j];
            //    //    i++;
            //    //}
            //    else
            //    {
            //       // xlsfilesNames += (filenameend + ",");
            //        isReadStatus = false;
            //    }
            //}

            //if (!isReadStatus)
            //{
            //    throw new Exception("请检查输入文件是否缺失或者文件名错误");
            //}

            return xlsFiles;
        }
    }
}
