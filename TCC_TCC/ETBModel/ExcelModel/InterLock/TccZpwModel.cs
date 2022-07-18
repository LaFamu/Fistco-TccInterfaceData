using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC_TCC.A.ETBAttribute;
using TCC_TCC.A.ETBConfig;

namespace TCC_TCC.A.ETBModel.ExcelModel.InterLock
{
    public class TccZpwModel
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
        public List<TccZpwDataBlockModel> dataBlockList;

        /// <summary>
        /// 节点序号表
        /// </summary>
        public List<CabinetModel> cabinetList;
    }

    [MetroExcelModelClass(SHEET_TYPE.TccZpwDataBlockSheet)]
    public class TccZpwDataBlockModel:DataBlockModel
    {
        /// <summary>
        /// 数据块序号
        /// </summary>
        [MetroExcelModelProperty("数据块序号", ExcelModelPropertyType.StringType, false, true)]
        public String BlockId { get; set; }

        /// <summary>
        /// 协议类型
        /// </summary>
        [MetroExcelModelProperty("协议类型", ExcelModelPropertyType.StringType, false, true)]
        public String Type { get; set; }
    }

    /// <summary>
    /// 节点序号表
    /// </summary>
    [MetroExcelModelClass(SHEET_TYPE.CabinetSheet)]
    public class CabinetModel : BitMapBase
    {
        /// <summary>
        /// 接收or发送
        /// </summary>
        [MetroExcelModelProperty("接收or发送", ExcelModelPropertyType.StringType, false, true)]
        public String Direction { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [MetroExcelModelProperty("名称", ExcelModelPropertyType.StringType, false, false)]
        public override string Name { get; set; }

        /// <summary>
        /// 通信通道
        /// </summary>
        [MetroExcelModelProperty("通信通道", ExcelModelPropertyType.StringType, false, true)]
        public string ComCan { get; set; }

        /// <summary>
        /// 节点序号
        /// </summary>
        [MetroExcelModelProperty("节点序号", ExcelModelPropertyType.StringType, false, true)]
        public string CabinetArc { get; set; }
    }
}
