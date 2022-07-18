using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using JSDG.CBI.A.ETBModel;
using JSDG.CBI.A.ETBUtil;
using NPOI.SS.UserModel;

namespace JSDG.CBI.A.ETBProcess
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

        [Export("JSDG.CBI.A")]
        public void ExcelToData(String outputPath, String[] fileNames, Action<Int32, String> act)
        {
            LogHelper.Inst.Init(VA_LogType.LOG_ETB, outputPath);
            MsgHelper.Init(act);

            try
            {
                DataModel dataModel = new DataModel(fileNames.ToList().FirstOrDefault());//读取数据

                CheckAllDataProcess.CheckAllData(dataModel);//检查数据

                SBForConflictSignal.Clear();
                IOMapSB.Clear();

                var datStruct = ConvertToBinary.ReturnDATStruct(dataModel, ref SBForConflictSignal);//生成二进制结构

                List<byte> byteList = ConvertToByte.ToBytes(datStruct).ToList();//转成字节

                DataOutput.CreateDatFile(byteList, outputPath, dataModel);//输出文件

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
