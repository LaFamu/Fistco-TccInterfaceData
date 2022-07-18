using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JSDG.CBI.A.ETBModel;

namespace JSDG.CBI.A.ETBUtil
{
    /// <summary>
    /// 字节转换帮助类
    /// </summary>
    public class BytesConverter
    {
        /// <summary>
        /// Expand UINT16 Code Distance
        /// </summary>
        /// <param name="temp">Data</param>
        /// <returns>Expanded result</returns>
        public static UInt32 ExpandUint16CodeDis(UInt32 temp)
        {
            return (temp & 0x0000ffff) == ((temp & 0xffff0000) >> 16) ? temp : temp << 16 ^ temp;
        }

        /// <summary>
        /// 32位无符号整数转换为字节数据
        /// </summary>
        /// <param name="input">要转换数据</param>
        /// <param name="bigOrLittleEndian">大端还是小端</param>
        /// <returns>字节数据</returns>
        public static Byte[] Uint32ToByte(UInt32 input, bool bigOrLittleEndian)
        {
            var a = new Byte[] { 0, 0, 0, 0 };
            if (!bigOrLittleEndian)
            {
                return BitConverter.GetBytes(input);
            }
            var b = BitConverter.GetBytes(input);
            for (var i = 4; i > 0; i--)
            {
                a[i - 1] = b[4 - i];
            }
            return a;
        }

        /// <summary>
        /// 字符串转换为字节数据，以gb2312编码方式转换
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <param name="bigOrLittleEndian">大端还是小端</param>
        /// <returns>字节流数据</returns>
        public static List<Byte> StringToList(String s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("参数s不能为空。");
            }
            var result = new List<Byte>(32);
            var b = Encoding.GetEncoding("gb2312").GetBytes(s.Trim()).ToList();
            var arrayCount = 32 - b.Count;
            if (b.Count < 32)
            {
                for (var i = 0; i < arrayCount; i++)
                {
                    b.Add(0);
                }
                result = b;
            }
            else if (b.Count >= 32)
            {
                for (var i = 0; i < 32; i++)
                {
                    result.Add(b[i]);
                }
            }
            return result;
        }

        ///// <summary>
        ///// 文件头转换为字节数据
        ///// </summary>
        ///// <param name="fileBytes">文件头</param>
        ///// <param name="bigOrLittleEndian">大端还是小端</param>
        ///// <returns>字节流数据</returns>
        //public static List<Byte> FileBytesToList(FileBytes fileBytes, bool bigOrLittleEndian)
        //{
        //    if (fileBytes == null)
        //    {
        //        throw new ArgumentNullException("参数fileBytes不能为空。");
        //    }
        //    var listTemp = new List<Byte>();
        //    listTemp.AddRange(BytesConverter.Uint32ToByte(fileBytes.VOPLength, bigOrLittleEndian).ToList());
        //    listTemp.AddRange(BytesConverter.Uint32ToByte(fileBytes.GALength1, bigOrLittleEndian).ToList());
        //    listTemp.AddRange(BytesConverter.Uint32ToByte(fileBytes.GALength2, bigOrLittleEndian).ToList());
        //    listTemp.AddRange(BytesConverter.Uint32ToByte(fileBytes.GALength3, bigOrLittleEndian).ToList());
        //    listTemp.AddRange(BytesConverter.Uint32ToByte(fileBytes.GALength4, bigOrLittleEndian).ToList());
        //    listTemp.AddRange(BytesConverter.Uint32ToByte(fileBytes.RandomNum, bigOrLittleEndian).ToList());
        //    return listTemp;
        //}

        ///// <summary>
        ///// 二进制文件头信息转换为字节数据
        ///// </summary>
        ///// <param name="binaryFileHead">二进制文件头信息</param>
        ///// <param name="bigOrLittleEndian">大端还是小端</param>
        ///// <returns>字节流数据</returns>
        //public static List<Byte> BinaryFileToList(BinaryFileHead binaryFileHead, bool bigOrLittleEndian)
        //{
        //    if (binaryFileHead == null)
        //    {
        //        throw new ArgumentNullException("参数binaryFileHead不能为空。");
        //    }
        //    var listTemp = new List<Byte>();
        //    listTemp.AddRange(BytesConverter.Uint32ToByte(binaryFileHead.GenerateDate, bigOrLittleEndian).ToList());
            
        //    listTemp.AddRange(BytesConverter.Uint32ToByte(binaryFileHead.MajorNumber, bigOrLittleEndian).ToList());
        //    listTemp.AddRange(BytesConverter.Uint32ToByte(binaryFileHead.MinorNumber, bigOrLittleEndian).ToList());
        //    listTemp.AddRange(BytesConverter.Uint32ToByte(binaryFileHead.DataVersion, bigOrLittleEndian).ToList());
        //    listTemp.AddRange(BytesConverter.StringToList(binaryFileHead.StationName));
        //    return listTemp;
        //}

        ///// <summary>
        ///// 头信息转换为字节数据
        ///// </summary>
        ///// <param name="segmentName">头数据</param>
        ///// <param name="bigOrLittleEndian">大端还是小端</param>
        ///// <returns>字节流数据</returns>
        //public static List<Byte> DeviceInfoToList(SegmentName segmentName, bool bigOrLittleEndian)
        //{
        //    if (segmentName == null)
        //    {
        //        throw new ArgumentNullException("参数segmentName不能为空。");
        //    }
        //    var listTemp = new List<Byte>();
        //    listTemp.AddRange(BytesConverter.StringToList(segmentName.TypeName));
        //    listTemp.AddRange(BytesConverter.Uint32ToByte(segmentName.DeviceLength, bigOrLittleEndian));
        //    listTemp.AddRange(BytesConverter.Uint32ToByte(segmentName.TotalDeviceNum, bigOrLittleEndian));
        //    return listTemp;
        //}

        ///// <summary>
        ///// 转换msio数据为字节数据
        ///// </summary>
        ///// <param name="msio">msio数据</param>
        ///// <param name="bigOrLittleEndian">大端还是小端</param>
        ///// <returns>字节流数据</returns>
        //public static List<Byte> MSIOToBinaryList(MSIOModel msio, bool bigOrLittleEndian)
        //{
        //    if (msio == null)
        //    {
        //        throw new ArgumentNullException("参数msio不能为空。");
        //    }
        //    var result = new List<Byte>();
        //    foreach (var sc in msio.MSList)
        //    {
        //        result.AddRange(BytesConverter.Uint32ToByte(sc.Byte_F, bigOrLittleEndian));
        //        result.AddRange(BytesConverter.Uint32ToByte(sc.Bit_F, bigOrLittleEndian));
        //        result.AddRange(BytesConverter.Uint32ToByte(sc.Byte_R, bigOrLittleEndian));
        //        result.AddRange(BytesConverter.Uint32ToByte(sc.Bit_R, bigOrLittleEndian));
        //    }
        //    return result;
        //}
    }
}