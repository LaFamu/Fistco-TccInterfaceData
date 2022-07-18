using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC_TCC.A.ETBAttribute;
using TCC_TCC.A.ETBConfig;

namespace TCC_TCC.A.ETBModel.ExcelModel.InterLock
{
  
    public class TccCommModel
    {
        /// <summary>
        /// 接口配置表
        /// </summary>
        public List<InterfaceConfigModel> interfaceConfigList;

        /// <summary>
        /// 接口码位表
        /// </summary>
        public List<InterfaceBitModel> interfaceBitList;

        /// <summary>
        /// 数据包格式表
        /// </summary>
        public List<DataPacketModel> dataPacketList;

        /// <summary>
        /// 数据块格式表
        /// </summary>
        public List<DataBlockModel> dataBlockList;
    }

    /// <summary>
    /// 接口配置表
    /// </summary>
    [MetroExcelModelClass(SHEET_TYPE.InterfaceConfigSheet)]
    public class InterfaceConfigModel:BitMapBase
    { 
        /// <summary>
        /// 参数ID
        /// </summary>
        [MetroExcelModelProperty("参数ID",ExcelModelPropertyType.Value32Type,false,false)]
        public override UInt32 Id { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        [MetroExcelModelProperty("参数名称",ExcelModelPropertyType.StringType,false,true)]
        public override String Name { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        [MetroExcelModelProperty("参数值",ExcelModelPropertyType.StringType,false,true)]
        public String Value { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [MetroExcelModelProperty("说明",ExcelModelPropertyType.StringType,false,true)]
        public String Explaination { get; set; }
    }

    /// <summary>
    /// 接口码位表
    /// </summary>
    [MetroExcelModelClass(SHEET_TYPE.InterfaceBitSheet)]
    public class InterfaceBitModel:BitMapBase
    {
        /// <summary>
        /// 接收or发送
        /// </summary>
        [MetroExcelModelProperty("接收or发送",ExcelModelPropertyType.StringType,false,true)]
        public String Direction { get; set; }

        /// <summary>
        /// 码位id
        /// </summary>
        [MetroExcelModelProperty("码位id",ExcelModelPropertyType.Value32Type,false,false)]
        public override UInt32 Id { get; set; }

        /// <summary>
        /// 码位名称
        /// </summary>
        [MetroExcelModelProperty("码位名称",ExcelModelPropertyType.StringType,false,false)]
        public override string Name { get ; set ; }

        /// <summary>
        /// 数据块名称
        /// </summary>
        [MetroExcelModelProperty("数据块名称",ExcelModelPropertyType.StringType,false,false)]
        public override string datablockName { get; set; }

        /// <summary>
        /// 数据块中序号
        /// </summary>
        [MetroExcelModelProperty("数据块中序号",ExcelModelPropertyType.StringType,false,false)]
        public override UInt32 datablockId { get; set; }
    }

    /// <summary>
    /// 数据包格式表
    /// </summary>
    [MetroExcelModelClass(SHEET_TYPE.DataPacketSheet)]
    public class DataPacketModel:BitMapBase
    {
        /// <summary>
        /// 序号
        /// </summary>
        [MetroExcelModelProperty("序号",ExcelModelPropertyType.Value32Type,false,false)]
        public override UInt32 Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [MetroExcelModelProperty("数据块名称",ExcelModelPropertyType.StringType,false,false)]
        public override string Name { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        [MetroExcelModelProperty("长度",ExcelModelPropertyType.StringType,false,false)]
        public UInt32 Length { get; set; }

        /// <summary>
        /// 偏移地址
        /// </summary>
        [MetroExcelModelProperty("偏移地址",ExcelModelPropertyType.StringType,false,false)]
        public UInt32 AddressOffset { get; set; }
    }

    /// <summary>
    /// 数据块格式表
    /// </summary>
    [MetroExcelModelClass(SHEET_TYPE.DataBlockSheet)]
    public class DataBlockModel:BitMapBase
    {
        /// <summary>
        /// 数据块名称
        /// </summary>
        [MetroExcelModelProperty("数据块名称",ExcelModelPropertyType.StringType,false,false)]
        public override string Name { get; set; }

        /// <summary>
        /// 信息名称
        /// </summary>
        [MetroExcelModelProperty("信息名称",ExcelModelPropertyType.StringType,false,false)]
        public String InfoName { get; set; }

        /// <summary>
        /// 位宽度
        /// </summary>
        [MetroExcelModelProperty("位宽度",ExcelModelPropertyType.StringType,false,false)]
        public UInt32 BitWidth { get; set; }
    }

   
}
