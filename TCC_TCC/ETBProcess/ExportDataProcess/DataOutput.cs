using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC_TCC.A.ETBModel;

namespace TCC_TCC.A.ETBProcess
{
    class DataOutput
    {
        public static void CreateDatFile(List<byte> byteList, string outpath)
        {
            try
            {
                ListToDat(byteList, outpath, ExcelToDatProcess.IsBigEndian);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 字节输出
        /// </summary>
        /// <param name="bytes">要输出的字节</param>
        /// <param name="directory">存放文件的路径</param>
        /// <param name="zoneName">要输出的控区名</param>
        /// <param name="isBigEndian">是否是大端</param>
        private static void ListToDat(List<Byte> bytes, String directory, bool isBigEndian)
        {
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            var datName = String.Format("{0}\\TccAppInf_Tjz.dat", directory);
            try
            {
                using (var fs = new FileStream(datName, FileMode.Create))
                {
                    using (var bw = new BinaryWriter(fs))
                    {
                        foreach (var b in bytes)
                        {
                            bw.Write(b);
                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
