using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSDG.CBI.A.ETBModel;
using JSDG.CBI.A.ETBProcess;

namespace JSDG.CBI.A.ETBUtil
{
    /// <summary>
    ///      ToBytes扩展方法
    /// </summary>
    public static class ETBExtensionToBytes
    {
        public static IEnumerable<Byte> ToBytes(this Byte input)
        {
            return new byte[] { input};//应直接返回原值
        }
        public static IEnumerable<Byte> ToBytes(this Byte[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }
       
        public static IEnumerable<Byte> ToBytes(this UInt16 input)
        {
            if (!ExcelToDatProcess.IsBigEndian)
            {
                return BitConverter.GetBytes(input);
            }

            return BitConverter.GetBytes(input).ConvertToBigEndian();
        }

        public static IEnumerable<Byte> ToBytes(this UInt16[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }
        public static IEnumerable<Byte> ToBytes(this UInt32 input)
        {
            if (!ExcelToDatProcess.IsBigEndian)
            {
                return BitConverter.GetBytes(input);
            }

            return BitConverter.GetBytes(input).ConvertToBigEndian();
        }
        public static IEnumerable<Byte> ToBytes(this UInt32[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this UInt64 input)
        {
            if (!ExcelToDatProcess.IsBigEndian)
            {
                return BitConverter.GetBytes(input);
            }

            return BitConverter.GetBytes(input).ConvertToBigEndian();
        }

        public static IEnumerable<Byte> ToBytes(this CIOAddress input)
        {
            return ToBytes(input.ByteOffset).AddRangeList( ToBytes(input.BitOffset));
        }
        public static IEnumerable<Byte> ToBytes(this CIOAddress[] inputs)
        {
            return ConcatCollections(inputs.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this DataBlockAddress input)
        {
            return ToBytes(input.ByteOffset).AddRangeList(ToBytes(input.ByteLen)).AddRangeList(ToBytes(input.DevNum));
        }
        public static IEnumerable<Byte> ToBytes(this CommAddress input)
        {
            return ToBytes(input.ByteOffset).AddRangeList(ToBytes(input.BitOffset)).AddRangeList(ToBytes(input.Width));
        }
        public static IEnumerable<Byte> ToBytes(this CommAddress[] inputs)
        {
            return ConcatCollections(inputs.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this DEVICE_ID input)
        {
            return ToBytes(input.Id).AddRangeList(ToBytes(input.Id)).AddRangeList(ToBytes(input.Index));
        }
        public static IEnumerable<Byte> ToBytes(this DEVICE_ID[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this RouteSwitch input)
        {
            return ToBytes(input.Switch_ID).AddRangeList(ToBytes(input.Positon));
        }
        public static IEnumerable<Byte> ToBytes(this RouteSwitch[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this FoulingBlock input)
        {
            return input.Block_ID.ToBytes().AddRangeList( ToBytes(input.RouteSwitches));
        }
        public static IEnumerable<Byte> ToBytes(this FoulingBlock[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this ConflictSignal input)
        {
            return ToBytes(input.Conflictsignal).AddRangeList(input.ConflictType.ToBytes()).AddRangeList(ToBytes(input.ConditionSwitch));
        }
        public static IEnumerable<Byte> ToBytes(this ConflictSignal[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this Path input)
        {
            return input.Block_ID.ToBytes().AddRangeList(input.CondSwitch.ToBytes());
        }
        public static IEnumerable<Byte> ToBytes(this Path[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this LOGIC_FACTOR input)
        {
            var bytes = input.bAvailable.ToBytes().AddRangeList(input.Type.ToBytes()).AddRangeList(input.DeviceID.ToBytes());
            bytes = bytes.AddRangeList(input.Condition.ToBytes()).AddRangeList(input.LogicalOperationType.ToBytes());
            bytes = bytes.AddRangeList(input.LeftLeaf.ToBytes()).AddRangeList(input.RightLeaf.ToBytes());
            return bytes;
        }
        public static IEnumerable<Byte> ToBytes(this LOGIC_FACTOR[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this OTHER_LOGIC input)
        {
            return input.Type.ToBytes().AddRangeList(input.Id.ToBytes());
        }
        public static IEnumerable<Byte> ToBytes(this OTHER_LOGIC[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this string input)
        {
            return StringToList(input);
        }
        public static IEnumerable<Byte> ToBytes(this WaysideInfo input)
        {
            return StringToList(input.SegmentName).AddRangeList(input.StructWidth.ToBytes()).AddRangeList(input.TotalStruct.ToBytes());
        }
        public static IEnumerable<Byte> ConcatCollections(IEnumerable<IEnumerable<Byte>> souce)
        {
            var b = new List<Byte>();
            souce.Foreach(b.AddRange);
            return b;
        }

        /// <summary>
        /// 字符串转换为字节数据，以gb2312编码方式转换
        /// </summary>
        /// <param name="segmentName">要转换的字符串</param>
        /// <returns>字节流数据</returns>
        public static List<Byte> StringToList( String segmentName)
        {
            if (segmentName == null)
            {
                throw new ArgumentNullException("参数segmentName不能为空。");
            }
            var result = new List<Byte>(32);
            var b = Encoding.GetEncoding("gb2312").GetBytes(segmentName.Trim()).ToList();
           
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
    }
}
