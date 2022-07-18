using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSDG.CBI.A.ETBUtil
{
    public static class ObjectHelper
    {
       /// <summary>
       /// 非大于零的自然数
       /// </summary>
       /// <param name="obj"></param>
       /// <returns></returns>
        public static bool IsPositiveInteger(object obj)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.ToString()))
            {
                return false;
            }

            try
            {
                return Int32.Parse(obj.ToString())>0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsEmpty(this object obj)
        {
            return obj == null || string.IsNullOrWhiteSpace(obj.ToString());
        }

        public static string ToStr(this object obj)
        {
            return obj == null?"":obj.ToString();
        }

        public static List<string> SplitToList(this string zoneNames)
        {
            return zoneNames.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        }
    }
}
