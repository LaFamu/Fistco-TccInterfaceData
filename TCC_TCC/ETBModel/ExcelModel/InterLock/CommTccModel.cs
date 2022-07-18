using System;
using TCC_TCC.A.ETBAttribute;
using TCC_TCC.A.ETBConfig;

namespace TCC_TCC.A.ETBModel
{
    /// <summary>
    /// TCC-XXX码位表
    /// </summary>
    [MetroExcelModelClass(SHEET_TYPE.TccBitSheet)]
    public class CommModel : InterLockBase
    {
        /// <summary>
        /// 接口名称
        /// </summary>
        [MetroExcelModelProperty("接口名称", ExcelModelPropertyType.StringType, false, true)]
        public override String Name { get; set; }

        /// <summary>
        /// 接口ID/连接站点ID
        /// </summary>
        [MetroExcelModelProperty("接口ID/连接站点ID", ExcelModelPropertyType.Value32Type, false, false)]
        public override UInt32 Id { get; set; }

        /// <summary>
        /// 输入输出
        /// </summary>
        [MetroExcelModelProperty("输入输出", ExcelModelPropertyType.StringType, false, true)]
        public String Direction { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        [MetroExcelModelProperty("设备id", ExcelModelPropertyType.Value32Type, false, true)]
        public UInt32 DeviceID { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [MetroExcelModelProperty("设备名称", ExcelModelPropertyType.StringType, false, true)]
        public String DeviceName { get; set; }

        /// <summary>
        /// 数据块类型
        /// </summary>
        [MetroExcelModelProperty("数据块类型", ExcelModelPropertyType.StringType, false, false)]
        public String MsgBlockType { get; set; }

        /// <summary>
        /// 字节编号
        /// </summary>
        [MetroExcelModelProperty("字节编号", ExcelModelPropertyType.Value32Type, false, false)]
        public UInt32 ByteOffSet { get; set; }

        /// <summary>
        /// 起始位
        /// </summary>
        [MetroExcelModelProperty("起始位", ExcelModelPropertyType.Value32Type, false, false)]
        public UInt32 BitOffSet { get; set; }

        /// <summary>
        /// 位宽度
        /// </summary>
        [MetroExcelModelProperty("位宽度", ExcelModelPropertyType.Value32Type, false, true)]
        public UInt32 Width { get; set; }

        /// <summary>
        /// 信息名称
        /// </summary>
        [MetroExcelModelProperty("信息名称", ExcelModelPropertyType.StringType, false, true)]
        public String InformationName { get; set; }

        [MetroExcelModelProperty("")]
        public String AimAreaProtect { get; set; }
    }

    
}
