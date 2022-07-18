using System;
using JSDG.CBI.A.ETBConfig;

namespace JSDG.CBI.A.ETBAttribute
{
    /// <summary>
    /// 地铁Excel实体类属性描述特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MetroExcelModelPropertyAttribute : ExcelModelPropertyBaseAttribute
    {
        /// <summary>
        /// 是否可以为空
        /// </summary>
        public bool IsCheckNull;

         /// <summary>
         /// 实际EXCEL列名
         /// </summary>
        public string ColName;

        /// <summary>
        /// 描述地铁Excel实体类属性
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyType">属性类型</param>
        /// <param name="isCheckRepeat">是否查重,默认不查</param>
        /// <param name="isCheckNull">是否可以为空,默认可以为空</param>
        public MetroExcelModelPropertyAttribute(String propertyName, ExcelModelPropertyType propertyType = ExcelModelPropertyType.StringType,
            Boolean isCheckRepeat = false,bool isCheckNull=false)
            : base(propertyName, propertyType, isCheckRepeat)
        {
            IsCheckNull = isCheckNull;
            ColName = propertyName;
        }
    }
}
