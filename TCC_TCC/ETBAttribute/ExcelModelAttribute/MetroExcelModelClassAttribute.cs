using System;
using System.Collections.Generic;
using TCC_TCC.A.ETBConfig;

namespace TCC_TCC.A.ETBAttribute
{
    /// <summary>
    /// Excel实体类描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MetroExcelModelClassAttribute : ExcelModelClassBaseAttribute
    {
        /// <summary>
        /// 描述Excel实体类
        /// </summary>
        /// <param name="cbidataType">设备类型</param>
        /// <param name="relaykey">继电器在config的配置</param>
        public MetroExcelModelClassAttribute(SHEET_TYPE cbidataType)
            : base(cbidataType)
        {

            //IOPointDictionary = ETB_APPConfig.GetConfigDic(relaykey);
        }
        /// <summary>
        /// 轨旁设备的输入输出继电器集合
        /// </summary>
        public Dictionary<String, UInt32> IOPointDictionary { get; private set; }
    }
}
