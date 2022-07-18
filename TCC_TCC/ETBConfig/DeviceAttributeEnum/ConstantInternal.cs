using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC_TCC.A.ETBConfig
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
        /// TCC-XXX码位表   
        /// </summary> 
        TccBitSheet,

        /// <summary>
        /// TCC-XXX设备映射表
        /// </summary>
        TccDMSheet,

        /// <summary>
        /// 接口配置表
        /// </summary>
        InterfaceConfigSheet,

        /// <summary>
        /// 接口码位表
        /// </summary>
        InterfaceBitSheet,

        /// <summary>
        /// 数据包格式表
        /// </summary>
        DataPacketSheet,

        /// <summary>
        /// Zpw数据块格式表
        /// </summary>
        TccZpwDataBlockSheet,

        /// <summary>
        /// 数据块格式表
        /// </summary>
        DataBlockSheet,

        /// <summary>
        /// 节点序号表
        /// </summary>
        CabinetSheet,

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


    

