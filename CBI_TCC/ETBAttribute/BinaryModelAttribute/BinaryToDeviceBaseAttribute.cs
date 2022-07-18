using System;
using System.Collections.Generic;
using System.Linq;
using JSDG.CBI.A.ETBConfig;

namespace JSDG.CBI.A.ETBAttribute
{
    /// <summary>
    /// 二进制数据描述基类特性
    /// </summary>
    public abstract class BinaryToDeviceBaseAttribute : Attribute
    {
        /// <summary>
        /// 映射到设备数据类中属性的名称
        /// </summary>
        public String DevicePropertyName { get; private set; }

        /// <summary>
        /// 字段的数据类型
        /// </summary>
        public BINARY_MODEL_VALUE_TYPE ValueType { get; private set; }

        /// <summary>
        /// 值的设备类型，便于验证填写的设备是否为指定的类型
        /// </summary>
        public String DeviceTypeOfValue { get; private set; }

        /// <summary>
        /// 是否允许excel中相应字段为空
        /// </summary>
        public Boolean CanEmpty { get; private set; }

        /// <summary>
        /// 二进制数据到设备数据的映射关系特性
        /// </summary>
        /// <param name="name">映射到设备数据类中属性的名称</param>
        /// <param name="valuetype">二进制数据字段数据类型</param>
        /// <param name="deviceTypeOfValue">该属性的值如果是设备名称，则这个需要说明这个设备是什么类型的设备</param>
        /// <param name="canEmpty">是否允许excel中相应字段为空,默认为不允许为空</param>
        protected BinaryToDeviceBaseAttribute(
            String name,
            BINARY_MODEL_VALUE_TYPE valuetype,
            String deviceTypeOfValue,
            Boolean canEmpty)
        {
            this.DevicePropertyName = name;
            this.ValueType = valuetype;
            this.DeviceTypeOfValue = deviceTypeOfValue;
            this.CanEmpty = canEmpty;
        }
    }
}
