using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TCC_TCC.A.ETBModel;
using TCC_TCC.A.ETBUtil;
using NPOI.SS.UserModel;

namespace TCC_TCC.A.ETBProcess
{
    public class ExcelToDatProcess
    {



        #region 构造函数

        /// <summary>
        /// 构造器
        /// </summary>
        public ExcelToDatProcess()
        {


        }

        #endregion

        /// <summary>
        /// 记录敌对信号,用以生成txt
        /// </summary>
        public static StringBuilder SBForConflictSignal = new StringBuilder();

        /// <summary>
        /// 用来记录输出异常
        /// </summary>
        public static StringBuilder IOMapSB = new StringBuilder();

        /// <summary>
        /// 用来记录大小端，true为大端
        /// </summary>
        public static bool IsBigEndian = true;

        [Export("TCC_TCC.A")]
        public void ExcelToData(String outputPath, String[] fileNames, Action<Int32, String> act)
        {
            LogHelper.Inst.Init(VA_LogType.LOG_ETB, outputPath);
            MsgHelper.Init(act);

            try
            { 
                SBForConflictSignal.Clear();
                IOMapSB.Clear();          
                //foreach (var fileName in fileNames.ToList())
                //{
                //    DataModel dataModel = new DataModel(fileName);
                //    var dataStruct = ConvertToBinary.ReturnDATStruct(dataModel, ref SBForConflictSignal);
                //    byteList.AddRange(ConvertToByte.ToBytes(dataStruct).ToList());
                //}
                DataModel dataModel = new DataModel(fileNames.ToList());

                var dataStruct = ConvertToBinary.ReturnDATStruct(dataModel,ref SBForConflictSignal);

                List<byte> byteList = ConvertToByte.ToBytes(dataStruct).ToList();

                DataOutput.CreateDatFile(byteList,outputPath);

            }
            catch (Exception ex)
            {
                MsgHelper.Instance(-1, ex.Message);
            }

        }

        private bool CraeteTxt(string fileName, string txt)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                var fs1 = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                var sw = new StreamWriter(fs1);
                sw.WriteLine(txt); //开始写入值
                sw.Close();
                fs1.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("文本创建失败", ex);
            }

        }

    }

}
