using System;
using System.Collections.Generic;
using System.Linq;

using JSDG.CBI.A.ETBProcess;

namespace JSDG.CBI.A.ETBUtil
{
    /// <summary>
    /// 字符串处理帮助类
    /// </summary>
    public class StringOperation
    {
        /// <summary>
        /// 解析IOMap中的数据，将数据分拆为设备名和继电器名
        /// </summary>
        /// <param name="s">要解析的字符串</param>
        /// <returns>分拆后的设备名和继电器名</returns>
        public static String[] ObtainDeviceNameAndRelay(String s)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException("参数s不能为空。");
            }
            var arr = new String[2];

            #region 处理特殊设备

            //去掉结尾"_F"和"_R"然后判断是否有“_”来判断是否是特殊设备
            if (s.EndsWith("_F") || s.EndsWith("_R"))
            {
                if (!s.Remove(s.Length - 2).Contains("_"))
                {
                    s = s.Insert(s.Length - 2, "_J");
                }
            }
            else
            {
                    s =s.Contains("_")?s: s + "_J_F";
            }
           
            #endregion 处理特殊设备

            var underLinePosition = s.IndexOf("_", System.StringComparison.Ordinal);
            arr[0] = s.Substring(0, underLinePosition).Trim();
            arr[1] = s.Substring(underLinePosition + 1).Trim();
            return arr;
        }

        /// <summary>
        /// 通过信号机显示获取信号机类型
        /// </summary>
        /// <param name="s">要解析的字符串</param>
        /// <returns>信号机类型</returns>
        public static UInt32 ObtainSignalType(String s)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException("参数s不能为空。");
            }
            const string findBString = "B";
            const string findHString = "H";
            const string findLString = "L";
            const string findUString = "U";
            const string findUUString = "UU";
            var result = 0xFFFFFFFF;
            if (s == findHString)
            {
            }
            else if (s.Contains(findBString) && (s.Contains(findLString) || s.Contains(findUString) || s.Contains(findUUString)))
            {
            }
            else if (s.Contains(findLString) || !s.Contains(findBString))
            {
            }
            else if (s.Contains(findBString) || !s.Contains(findLString))
            {
            }
            else
            {
                throw new ArgumentException("参数s的值不正确。");
            }
            return result;
        }

        /// <summary>
        /// 分解道岔字符，返回带位置的道岔，例如（P1_N)
        /// </summary>
        /// <param name="sourceString">待分解的道岔字符</param>
        public static List<string> GetSwitch(string sourceString)
        {
            var resultList = new List<string>();
            var stringList = sourceString.Split(new char[] { ',', '/' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach(string switchString in stringList)
            {
                if(switchString.Contains("(") || switchString.Contains(")"))
                {
                    resultList.Add(switchString.Replace("(", "").Replace(")", "") + "_R");
                }
                else
                {
                    resultList.Add(switchString + "_N");
                }
            }

            return resultList;
        }
    }
}