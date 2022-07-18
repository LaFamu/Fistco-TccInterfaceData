using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TCC_TCC.A.ETBAttribute;

namespace TCC_TCC.A.ETBUtil
{
    public class PropertyValueItem<T>
    {
       
        /// <summary>
        /// 属性的描述
        /// </summary>
        public MetroExcelModelPropertyAttribute Attribute { get;  set; }

        /// <summary>
        /// 属性对应的值的集合
        /// </summary>
        public IEnumerable<object> Value { get;  set; }
       
      
    }

    public class PropertyValueItemHelper<T>
    {
        public static IEnumerable<PropertyValueItem<T>> GetPropertyValueItems(List<T> list)
        {
           return typeof (T).GetProperties().Select(p => new PropertyValueItem<T>
            {
                Attribute = p.GetAttribute(), Value = list.Select(x => p.GetValue(x))
            });
           
        }
    }
}
