using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSDG.CBI.A.ETBConfig
{
    /// <summary>
    /// 内部常量
    /// </summary>
   
        /// <summary>
        /// Excel
        /// </summary>
    public enum ExcelModelPropertyType : int
    {
        /// <summary>
        /// 字符串类型的值
        /// </summary>
        StringType,

        /// <summary>
        /// 32位无符号整数值
        /// </summary>
        Value32Type,

        /// <summary>
        /// 索引类型数据，通过计算得出
        /// </summary>
        IndexType,

        /// <summary>
        /// 信号机类型，可以包含多个信号机
        /// </summary>
        SignalType

    }

    /// <summary>
    /// 子表类型
    /// </summary>
    public enum SHEET_TYPE : uint
    {
        /// <summary>  
        ///     TCC-XXX_TCC码位表   
        /// </summary> 
        CommCbiSheet,

        /// <summary>  
        ///     TCC-CBI码位表   
        /// </summary> 
        CbiSheet

    }

    /// <summary>
    /// 二进制数据字段数据类型
    /// </summary>
    public enum BINARY_MODEL_VALUE_TYPE
    {
        /// <summary>
        /// 通用字符类型
        /// </summary>
        Normal,

        /// <summary>
        /// 设备ID列表
        /// </summary>
        DeviceIDList,

        /// <summary>
        /// 特殊条件
        /// </summary>
        Condition,

        /// <summary>
        /// 总的数量
        /// </summary>
        TotalCount
    };
}


    

