using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC_TCC.A.ETBAttribute.BinaryModelAttribute
{
    /// <summary>
    /// 信息地址特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CommAddressAttribute:Attribute
    {
        public String Name { get; set; }

        public CommAddressAttribute(string name)
        {
            this.Name = name;
        }
    }
}
