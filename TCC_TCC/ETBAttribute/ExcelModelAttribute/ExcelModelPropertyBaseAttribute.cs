using System;
using TCC_TCC.A.ETBConfig;
using TCC_TCC.A.ETBProcess;

namespace TCC_TCC.A.ETBAttribute
{
    /// <summary>
    /// Excel实体类属性描述特性
    /// </summary>
    public abstract class ExcelModelPropertyBaseAttribute : Attribute
    {
        /// <summary>
        /// 属性在Excel中的列的索引号
        /// </summary>
        public Int32 CellNum { get;  set; }

        /// <summary>
        /// 属性的自定义类型
        /// </summary>
        public ExcelModelPropertyType PropertyType { get; private set; }

        /// <summary>
        /// 是否检查excel中相应字段可以重复
        /// </summary>
        public Boolean IsCheckRepeat { get; private set; }

        /// <summary>
        /// 描述Excel实体类属性
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyType">属性类型</param>
        /// <param name="IsRepeat">是否检查重复</param>
        protected ExcelModelPropertyBaseAttribute(
            String propertyName,
            ExcelModelPropertyType propertyType = ExcelModelPropertyType.StringType,
            Boolean IsRepeat = false)
            : base()
        {
            this.PropertyType = propertyType;
            this.IsCheckRepeat = IsRepeat;
        }

    }
}
