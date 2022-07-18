using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC_TCC.A.ETBAttribute;
using TCC_TCC.A.ETBConfig;

namespace TCC_TCC.A.ETBModel
{
    /// <summary>
    /// 联锁表sheet页基类
    /// </summary>
    public class InterLockBase
    {
        /// <summary>
        /// 编号
        /// </summary>
        [MetroExcelModelProperty("编号/号码/ID", ExcelModelPropertyType.IndexType, true, true)]
        public virtual UInt32 Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [MetroExcelModelProperty("名称", ExcelModelPropertyType.StringType, true, true)]
        public virtual String Name { get; set; }
    }
}
