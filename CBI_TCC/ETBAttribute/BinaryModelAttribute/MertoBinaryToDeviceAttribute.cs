using System;
using JSDG.CBI.A.ETBConfig;

namespace JSDG.CBI.A.ETBAttribute
{
    /// <summary>
    /// 二进制数据到设备数据的映射关系特性,只适用于二进制类属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MertoBinaryToDeviceAttribute : BinaryToDeviceBaseAttribute
    {
        /// <summary>
        /// 二进制数据到设备数据的映射关系特性
        /// </summary>
        /// <param name="name">映射到设备数据类中属性的名称</param>
        /// <param name="valuetype">二进制数据字段数据类型</param>
        /// <param name="deviceTypeOfValue">该属性的值如果是设备名称，则这个需要说明这个设备是什么类型的设备</param>
        /// <param name="canEmpty">是否允许excel中相应字段为空,默认为不允许为空</param>
        public MertoBinaryToDeviceAttribute(
            String name,
            BINARY_MODEL_VALUE_TYPE valuetype = BINARY_MODEL_VALUE_TYPE.Normal,
            String deviceTypeOfValue = null,
            Boolean canEmpty = false)
            : base(name, valuetype, deviceTypeOfValue, canEmpty)
        {
        }
    }
}
