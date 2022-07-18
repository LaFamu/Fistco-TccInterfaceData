using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TCC_TCC.A.ETBAttribute.BinaryModelAttribute;
using TCC_TCC.A.ETBModel.ExcelModel.InterLock;
using TCC_TCC.A.ETBProcess;
using TCC_TCC.A.ETBUtil;

namespace TCC_TCC.A.ETBModel.BinaryModel.BinaryStruct
{
    public class CommTcc2ZpwStruct : BaseStruct
    {
        #region 私有变量
        private List<InterfaceConfigModel> interfaceConfigsourceList;
        private List<InterfaceBitModel> interfaceBitsourceList;
        private List<DataPacketModel> dataPacketsourceList;
        private List<TccZpwDataBlockModel> dataBlocksourceList;
        private List<CabinetModel> cabinetsourceList;
        #endregion

        public InterfaceInfo InterfaceInfo;
        public Tcc2ZpwInfCfgData Tcc2ZpwInfCfgData;
        public Tcc2ZpwRcvData Tcc2ZpwReceiveData;
        public Tcc2ZpwSendData Tcc2ZpwSendData;
        public StringBuilder Tcc2ZpwSB;

        public CommTcc2ZpwStruct(DataModel dataModel)
        {
            MsgHelper.Instance(1,string.Format("正在生成结构体《Tcc2Zpw》"));
            interfaceConfigsourceList = dataModel.BitData.Tcc2ZpwInterface.interfaceConfigList;
            interfaceBitsourceList = dataModel.BitData.Tcc2ZpwInterface.interfaceBitList;
            dataPacketsourceList = dataModel.BitData.Tcc2ZpwInterface.dataPacketList;
            dataBlocksourceList = dataModel.BitData.Tcc2ZpwInterface.dataBlockList;
            cabinetsourceList = dataModel.BitData.Tcc2ZpwInterface.cabinetList;

            this.InterfaceInfo.InterfaceName = "Tcc2Zpw";
            this.InterfaceInfo.InterfaceByteNum = 13840U;
            this.InterfaceInfo.InterfaceNum = 1;

            Transform();
        }

        public override void Transform()
        {
            Tcc2ZpwInfCfgData = new Tcc2ZpwInfCfgData();
            Tcc2ZpwReceiveData = new Tcc2ZpwRcvData();
            Tcc2ZpwSendData = new Tcc2ZpwSendData();

            Tcc2ZpwSB = new StringBuilder();
            Tcc2ZpwSB.AppendFormat("\r\n\r\n====================Tcc2Zpw====================\r\n");

            try
            {
                Tcc2ZpwInfCfgData.IsConnExist = interfaceConfigsourceList.First(x => x.Name.Equals("是否有通信连接")).Value.Equals("是") ? CONST_TRUE : CONST_FALSE;
                Tcc2ZpwInfCfgData.SrcDeviceId = GetDEVICE_ID(interfaceConfigsourceList.First(x => x.Name.Equals("源设备Id")).Value);
                Tcc2ZpwInfCfgData.DstDeviceId = GetDEVICE_ID(interfaceConfigsourceList.First(x => x.Name.Equals("目的设备Id")).Value);
                Tcc2ZpwInfCfgData.MaxSilentCycle = Convert.ToUInt32(interfaceConfigsourceList.First(x => x.Name.Equals("超时等待周期数")).Value);
                Tcc2ZpwInfCfgData.CommInterval = Convert.ToUInt32(interfaceConfigsourceList.First(x => x.Name.Equals("通信间隔周期数")).Value);
                Tcc2ZpwInfCfgData.ProtVer = Convert.ToUInt32(interfaceConfigsourceList.First(x => x.Name.Equals("通信协议版本")).Value,16);

                #region ReceiveData
                Tcc2ZpwReceiveData.BlkStatMsgAddr = GetBlkStatMsgAddr<BLK_STAT_MSG_ADDR_ST>("轨道区段状态", interfaceConfigsourceList.First(x => x.Name.Equals("通信协议版本")).Value, 100);
                Tcc2ZpwReceiveData.BlkNumFromZpwConsiderBlankItem = interfaceBitsourceList.FindLast(x => (x.datablockName.Equals("轨道区段状态") && !string.IsNullOrWhiteSpace(x.datablockId.ToString()))).datablockId;
                Tcc2ZpwReceiveData.CabinetAcr1 = GetCabinetArc1(10);
                #endregion

                #region SendData
                Tcc2ZpwSendData.BlkCodeMsgAddr = GetBlkCodeMsgAddr<BLK_CODE_MSG_ADDR_ST>("轨道区段编码",100);
                Tcc2ZpwSendData.BlkNum2ZpwConsiderBlankItem = interfaceBitsourceList.FindLast(x => (x.datablockName.Equals("轨道区段编码") && !string.IsNullOrWhiteSpace(x.datablockId.ToString()))).datablockId;
                Tcc2ZpwSendData.CabinetId = GetCabinetId(10);
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private UInt32[] GetCabinetId(UInt32 length)
        {
            UInt32[] result = new UInt32[length];

            for (int i = 0;i < length;i++)
            {
                result[i] = Convert.ToUInt32(cabinetsourceList.First(x => (x.Direction.Equals("发送") && x.Name.Contains((i + 1).ToString()))).CabinetArc);   
            }

            return result;
        }

        private TCC_CABINET_ACRI_ST[] GetCabinetArc1(UInt32 length)
        {
            TCC_CABINET_ACRI_ST[] result = new TCC_CABINET_ACRI_ST[length];

            for (int i = 0;i < length;i++)
            {
                var temp = cabinetsourceList.Where(x => (x.Direction.Equals("接收") && x.Name.Contains((i + 1).ToString()))).ToList();
                result[i].Acr1_CanACpu1 = Convert.ToUInt32(temp.First(x => x.ComCan.Equals("CANA的CPU1")).CabinetArc);
                result[i].Acr1_CanACpu2 = Convert.ToUInt32(temp.First(x => x.ComCan.Equals("CANA的CPU2")).CabinetArc);
                result[i].Acr1_CanBCpu1 = Convert.ToUInt32(temp.First(x => x.ComCan.Equals("CANB的CPU1")).CabinetArc);
                result[i].Acr1_CanBCpu2 = Convert.ToUInt32(temp.First(x => x.ComCan.Equals("CANB的CPU2")).CabinetArc);
            }

            return result;

        }

        private BLK_CODE_MSG_ADDR_ST[] GetBlkCodeMsgAddr<T>(string blockName,int length)
        {
            BLK_CODE_MSG_ADDR_ST[] result = new BLK_CODE_MSG_ADDR_ST[length];
            var blockList = interfaceBitsourceList.Where(x => x.datablockName.Equals(blockName)).ToList();

            foreach (var item in blockList)
            {
                result[blockList.IndexOf(item)].Id = GetBitMapDEVICE_ID<InterfaceBitModel>(item.datablockId,blockList);
                result[blockList.IndexOf(item)].XgCarrFreq = GetPropertyValue<T>(item.datablockName,item.datablockId,"XgCarrFreq");
                result[blockList.IndexOf(item)].ZgCarrFreq = GetPropertyValue<T>(item.datablockName,item.datablockId, "ZgCarrFreq");
                result[blockList.IndexOf(item)].ZgLowFreq = GetPropertyValue<T>(item.datablockName,item.datablockId, "ZgLowFreq");
                result[blockList.IndexOf(item)].Dir = GetPropertyValue<T>(item.datablockName, item.datablockId, "Dir");
                result[blockList.IndexOf(item)].XgLowFreq = GetPropertyValue<T>(item.datablockName,item.datablockId,"XgLowFreq");
            }

            return result;
        }

        private BLK_STAT_MSG_ADDR_ST[] GetBlkStatMsgAddr<T>(string blockName,string portType,int length)
        {
            BLK_STAT_MSG_ADDR_ST[] result = new BLK_STAT_MSG_ADDR_ST[length];
            var blockList = interfaceBitsourceList.Where(x => x.datablockName.Equals(blockName)).ToList();

            foreach (var item in blockList)
            {
                if (portType.Equals("0x00") || portType.Equals("0xff"))
                {
                    result[blockList.IndexOf(item)].Id = GetBitMapDEVICE_ID<InterfaceBitModel>(item.datablockId,blockList);
                    result[blockList.IndexOf(item)].ZgStatMain = GetPropertyValue<T>(item.datablockName,item.datablockId,"协议I","ZgStatMain");
                    result[blockList.IndexOf(item)].XgStatMain = GetPropertyValue<T>(item.datablockName,item.datablockId,"协议I","XgStatMain");
                    result[blockList.IndexOf(item)].Dir = new CommAddress();
                    result[blockList.IndexOf(item)].ZgStatParallel = GetPropertyValue<T>(item.datablockName, item.datablockId, "协议I", "ZgStatParallel");
                    result[blockList.IndexOf(item)].XgStatParallel = GetPropertyValue<T>(item.datablockName,item.datablockId,"协议I","XgStatParallel");
                }
                else
                {
                    result[blockList.IndexOf(item)].Id = GetBitMapDEVICE_ID<InterfaceBitModel>(item.datablockId,blockList);
                    result[blockList.IndexOf(item)].ZgStatMain = GetPropertyValue<T>(item.datablockName,item.datablockId,"协议II","ZgStatMain");
                    result[blockList.IndexOf(item)].XgStatMain = GetPropertyValue<T>(item.datablockName,item.datablockId,"协议II","XgStatMain");
                    result[blockList.IndexOf(item)].Dir = GetPropertyValue<T>(item.datablockName,item.datablockId,"协议II","Dir");
                    result[blockList.IndexOf(item)].ZgStatParallel = GetPropertyValue<T>(item.datablockName,item.datablockId,"协议II","ZgStatParallel");
                    result[blockList.IndexOf(item)].XgStatParallel = GetPropertyValue<T>(item.datablockName,item.datablockId,"协议II","XgStatParallel");
                }
            }


            return result;
        }

        private CommAddress GetPropertyValue<T>(string blockName, UInt32 blockId, string propertyName)
        {
            var block = interfaceBitsourceList.First(x => (x.datablockName.Equals(blockName) && x.datablockId.Equals(blockId)));
            var blockid = blockId;
            var packoffset = dataPacketsourceList.First(x => x.Name.Equals(block.datablockName)).AddressOffset;
            uint blockWidth = 0;
            uint blockWidthOffset = 0;
            uint infoWidth = 0;
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            var t = typeof(T);
            foreach (var p in t.GetProperties())
            {
                if (p.GetCustomAttribute<CommAddressAttribute>(true) != null)
                {
                    keyValuePairs.Add(p.Name, p.GetCustomAttribute<CommAddressAttribute>(true).Name);
                }
            }
            //var nowt = pt;
            var infoName = keyValuePairs[propertyName];
            var infoList = dataBlocksourceList.Where(x => x.InfoName.Equals(block.datablockName)).ToList();
            foreach (var item in infoList)
            {
                blockWidth += item.BitWidth;
                if (item.InfoName.Equals(infoName))
                {
                    for (int i = 0; i < infoList.IndexOf(item); i++)
                    {
                        blockWidthOffset += infoList[i].BitWidth;
                    }
                    infoWidth = item.BitWidth;
                }
            }

            return GetCommAddress(blockid, packoffset, blockWidth, blockWidthOffset, infoWidth);
        }

        private CommAddress GetPropertyValue<T>(string blockName, UInt32 blockId, string portType, string propertyName)
        {
            var block = interfaceBitsourceList.First(x => (x.datablockName.Equals(blockName) && x.datablockId.Equals(blockId)));
            var blockid = blockId;
            var packoffset = dataPacketsourceList.First(x => x.Name.Equals(block.datablockName)).AddressOffset;
            uint blockWidth = 0;
            uint blockWidthOffset = 0;
            uint infoWidth = 0;
            string blockIdType = (blockId % 2 == 1) ? "奇数" : "偶数";
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            var t = typeof(T);
            foreach (var p in t.GetProperties())
            {
                if (p.GetCustomAttribute<CommAddressAttribute>(true) != null)
                {
                    keyValuePairs.Add(p.Name, p.GetCustomAttribute<CommAddressAttribute>(true).Name);
                }
            }
            var infoName = keyValuePairs[propertyName];
            var infoList = dataBlocksourceList.Where(x => (x.InfoName.Equals(block.datablockName) && x.BlockId.Equals(blockIdType) && x.Type.Equals(portType))).ToList();
            foreach (var item in infoList)
            {
                blockWidth += item.BitWidth;
                if (item.InfoName.Equals(infoName))
                {
                    for (int i = 0; i < infoList.IndexOf(item); i++)
                    {
                        blockWidthOffset += infoList[i].BitWidth;
                    }
                    infoWidth = item.BitWidth;
                }
            }

            return GetCommAddress(blockid, packoffset, blockWidth, blockWidthOffset, infoWidth);
        }


        private DEVICE_ID GetDEVICE_ID(string id)
        {
            var result = new DEVICE_ID();
            if (id.Equals(""))
            {
                result = CONST_DEVICE_ID;
            }
            else
            {
                var temp = Convert.ToUInt32(id);

                result = CONST_DEVICE_ID;

                result.Id = temp << 16 ^ temp;
                result.Index = UInt32.MaxValue;
            }
            return result;
        }

    }




    public struct BLK_STAT_MSG_ADDR_ST
    { 
        public DEVICE_ID Id { get; set; }

        [CommAddressAttribute("轨道区段主轨道状态(主)")]
        public CommAddress ZgStatMain { get; set; }

        [CommAddressAttribute("轨道区段小轨道状态(主)")]
        public CommAddress XgStatMain { get; set; }

        [CommAddressAttribute("轨道区段发码方向状态")]
        public CommAddress Dir { get; set; }

        [CommAddressAttribute("轨道区段主轨道状态(并)")]
        public CommAddress ZgStatParallel { get; set; }

        [CommAddressAttribute("轨道区段小轨道状态(并)")]
        public CommAddress XgStatParallel { get; set; }
    }

    public struct TCC_CABINET_ACRI_ST
    {
        public UInt32 Acr1_CanACpu1;
        public UInt32 Acr1_CanACpu2;
        public UInt32 Acr1_CanBCpu1;
        public UInt32 Acr1_CanBCpu2;
    }

    public struct BLK_CODE_MSG_ADDR_ST
    { 
        public DEVICE_ID Id { get; set; }

        [CommAddressAttribute("小轨道载频编码")]
        public CommAddress XgCarrFreq { get; set; }

        [CommAddressAttribute("主轨道载频编码")]
        public CommAddress ZgCarrFreq { get; set; }

        [CommAddressAttribute("主轨道低频编码")]
        public CommAddress ZgLowFreq { get; set; }

        [CommAddressAttribute("发码方向信息")]
        public CommAddress Dir { get; set; }

        [CommAddressAttribute("小轨道低频编码")]
        public CommAddress XgLowFreq { get; set; }
    }

    public class Tcc2ZpwInfCfgData
    {
        public UInt32 IsConnExist;
        public DEVICE_ID SrcDeviceId;
        public DEVICE_ID DstDeviceId;
        public UInt32 MaxSilentCycle;
        public UInt32 CommInterval;
        public UInt32 ProtVer;
    }

    public class Tcc2ZpwRcvData
    {
        public BLK_STAT_MSG_ADDR_ST[] BlkStatMsgAddr;

        public UInt32 BlkNumFromZpwConsiderBlankItem;

        public TCC_CABINET_ACRI_ST[] CabinetAcr1;
    }

    public class Tcc2ZpwSendData
    {
        public BLK_CODE_MSG_ADDR_ST[] BlkCodeMsgAddr;

        public UInt32 BlkNum2ZpwConsiderBlankItem;

        public UInt32[] CabinetId;
    }

    
}
