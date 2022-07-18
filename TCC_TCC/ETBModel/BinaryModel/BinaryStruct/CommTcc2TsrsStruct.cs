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
    public class CommTcc2TsrsStruct : BaseStruct
    {
        #region 私有变量
        private List<InterfaceConfigModel> interfaceConfigsourceList;
        private List<InterfaceBitModel> interfaceBitsourceList;
        private List<DataPacketModel> dataPacketsourceList;
        private List<DataBlockModel> dataBlocksourceList;
        #endregion

        public InterfaceInfo InterfaceInfo;
        public Tcc2TsrsInfCfgData Tcc2TsrsInfCfgData;
        public Tcc2TsrsReceiveData Tcc2TsrsReceiveData;
        public Tcc2TsrsSendData Tcc2TsrsSendData;
        public StringBuilder Tcc2TsrsSB;

        public CommTcc2TsrsStruct(DataModel dataModel)
        {
            MsgHelper.Instance(1,string.Format("正在生成结构体《Tcc2Tsrs》"));
            interfaceConfigsourceList = dataModel.BitData.Tcc2TsrsInterface.interfaceConfigList;
            interfaceBitsourceList = dataModel.BitData.Tcc2TsrsInterface.interfaceBitList;
            dataPacketsourceList = dataModel.BitData.Tcc2TsrsInterface.dataPacketList;
            dataBlocksourceList = dataModel.BitData.Tcc2TsrsInterface.dataBlockList;

            this.InterfaceInfo.InterfaceName = "Tcc2Tsrs";
            this.InterfaceInfo.InterfaceByteNum = 7804U;
            this.InterfaceInfo.InterfaceNum = 1;

            Transform();
        }

        public override void Transform()
        {
            Tcc2TsrsInfCfgData = new Tcc2TsrsInfCfgData();
            Tcc2TsrsReceiveData = new Tcc2TsrsReceiveData();
            Tcc2TsrsSendData = new Tcc2TsrsSendData();

            Tcc2TsrsSB = new StringBuilder();
            Tcc2TsrsSB.AppendFormat("\r\n\r\n====================Tcc2Tsrs====================\r\n");

            try
            {
                Tcc2TsrsInfCfgData.IsConnExist = interfaceConfigsourceList.First(x => x.Name.Equals("是否有通信连接")).Value.Equals("是") ? CONST_TRUE : CONST_FALSE;
                Tcc2TsrsInfCfgData.SrcDeviceId = GetDEVICE_ID(interfaceConfigsourceList.First(x => x.Name.Equals("源设备Id")).Value);
                Tcc2TsrsInfCfgData.DstDeviceId = GetDEVICE_ID(interfaceConfigsourceList.First(x => x.Name.Equals("目的设备Id")).Value);
                Tcc2TsrsInfCfgData.MaxSilentCycle = Convert.ToUInt32((interfaceConfigsourceList.First(x => x.Name.Equals("超时等待周期数")).Value));
                Tcc2TsrsInfCfgData.CommInterval = Convert.ToUInt32((interfaceConfigsourceList.First(x => x.Name.Equals("通信间隔周期数")).Value));

                #region ReceiveStruct
                Tcc2TsrsReceiveData.PsdCmdMsgAddr = GetPsdCmdMsgAddr<PSD_CMD_MSG_ADDR_ST>("站台门控制命令",48);
                Tcc2TsrsReceiveData.PsdNum = Convert.ToUInt32(interfaceBitsourceList.Select(x => (x.datablockName.Equals("站台门控制命令") && !string.IsNullOrEmpty(x.Id.ToString()))).Count());
                Tcc2TsrsReceiveData.LoseDetMsgAddr = GetLoseDetMsgAddr<LOSE_DET_MSG_ADDR_ST>("TCC失去分路状态信息",100);
                Tcc2TsrsReceiveData.BlkSectNum = Convert.ToUInt32(interfaceBitsourceList.Select(x => (x.datablockName.Equals("TCC失去分路状态信息") && !string.IsNullOrEmpty(x.Id.ToString()))).Count());
                #endregion

                #region SendStruct
                Tcc2TsrsSendData.BsStatMsgAddr = GetBsStatMsgAddr<BS_STAT_MSG_ADDR_ST>("TCC闭塞分区状态信息",100);
                Tcc2TsrsSendData.BlkSectNum = Convert.ToUInt32(interfaceBitsourceList.Select(x => (x.datablockName.Equals("TCC闭塞分区状态信息") && !string.IsNullOrEmpty(x.Id.ToString()))).Count());
                Tcc2TsrsSendData.BaliseDirMsgAddr = GetBaliseDirMsgAddr<BALISE_DIR_MSG_ADDR_ST>("进路口应答器处的区间方向信息",16);
                Tcc2TsrsSendData.LiaisonBaliseNum = Convert.ToUInt32(interfaceBitsourceList.Select(x => (x.datablockName.Equals("进站口应答器处的区间方向信息") && !string.IsNullOrEmpty(x.Id.ToString()))).Count());
                Tcc2TsrsSendData.PsdStatMsgAddr = GetPsdStatMsgAddr<PSD_STAT_MSG_ADDR_ST>("站台门状态",48);
                Tcc2TsrsSendData.PsdNum = Convert.ToUInt32(interfaceBitsourceList.Select(x => (x.datablockName.Equals("站台门状态") && !string.IsNullOrEmpty(x.Id.ToString()))).Count());
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private PSD_STAT_MSG_ADDR_ST[] GetPsdStatMsgAddr<T>(string blockName, UInt32 length)
        {
            PSD_STAT_MSG_ADDR_ST[] result = new PSD_STAT_MSG_ADDR_ST[length];
            for (int i = 0;i < length;i++)
            {
                result[i].PsdStat = new CommAddress();
            }

            var blockList = interfaceBitsourceList.Where(x => x.datablockName.Equals(blockName)).ToList();
            foreach (var item in blockList)
            {
                result[blockList.IndexOf(item)].Id = GetBitMapDEVICE_ID<InterfaceBitModel>(item.datablockId,blockList);
                result[blockList.IndexOf(item)].PsdStat = GetPropertyValue<T>(item.datablockName,item.datablockId,"PsdStat");
            }

            return result;
        }

        private BALISE_DIR_MSG_ADDR_ST[] GetBaliseDirMsgAddr<T>(string blockName,UInt32 length)
        {
            BALISE_DIR_MSG_ADDR_ST[] result = new BALISE_DIR_MSG_ADDR_ST[length];
            for (int i = 0;i < length;i++)
            {
                result[i].BaliseDir = new CommAddress();
            }

            var blockList = interfaceBitsourceList.Where(x => x.datablockName.Equals(blockName)).ToList();
            foreach (var item in blockList)
            {
                result[blockList.IndexOf(item)].Id = GetBitMapDEVICE_ID<InterfaceBitModel>(item.datablockId,blockList);
                result[blockList.IndexOf(item)].BaliseDir = GetPropertyValue<T>(item.datablockName,item.datablockId,"BaliseDir");
            }

            return result;
        }

        private LOSE_DET_MSG_ADDR_ST[] GetLoseDetMsgAddr<T>(string blockName,UInt32 length)
        {
            LOSE_DET_MSG_ADDR_ST[] result = new LOSE_DET_MSG_ADDR_ST[length];

            var blockList = interfaceBitsourceList.Where(x => x.datablockName.Equals(blockName)).ToList();
            foreach (var item in blockList)
            {
                result[blockList.IndexOf(item)].Id = GetBitMapDEVICE_ID<InterfaceBitModel>(item.datablockId,blockList);
                result[blockList.IndexOf(item)].LoseDetStat = GetPropertyValue<T>(item.datablockName,item.datablockId,"LoseDetStat");
            }
            return result;
        }

        private BS_STAT_MSG_ADDR_ST[] GetBsStatMsgAddr<T>(string blockName,UInt32 length)
        {
            BS_STAT_MSG_ADDR_ST[] result = new BS_STAT_MSG_ADDR_ST[length];
            for (int i = 0; i < length;i++)
            {
                result[i].LoseDetStat = new CommAddress();
                result[i].LowFreq = new CommAddress();
                result[i].OccupyStat = new CommAddress();
            }

            var blockList = interfaceBitsourceList.Where(x => x.datablockName.Equals(blockName)).ToList();
            foreach (var item in blockList)
            {
                result[blockList.IndexOf(item)].Id = GetBitMapDEVICE_ID<InterfaceBitModel>(item.datablockId,blockList);
                result[blockList.IndexOf(item)].LowFreq = GetPropertyValue<T>(item.datablockName,item.datablockId,"LowFreq");
                result[blockList.IndexOf(item)].LoseDetStat = GetPropertyValue<T>(item.datablockName,item.datablockId,"LoseDetStat");
                result[blockList.IndexOf(item)].OccupyStat = GetPropertyValue<T>(item.datablockName,item.datablockId,"OccupyStat");
            }

            return result;
        }

        private PSD_CMD_MSG_ADDR_ST[] GetPsdCmdMsgAddr<T>(string blockName, UInt32 length)
        {
            PSD_CMD_MSG_ADDR_ST[] result = new PSD_CMD_MSG_ADDR_ST[length];

            var blockList = interfaceBitsourceList.Where(x => x.datablockName.Equals(blockName)).ToList();
            foreach (var item in blockList)
            {
                result[blockList.IndexOf(item)].PsdCmd = GetPropertyValue<T>(item.datablockName,item.datablockId,"PsdCmd");
            }

            return result;
        }

        private CommAddress GetPropertyValue<T>(string blockName, string propertyName)
        {
            var block = interfaceBitsourceList.First(x => x.datablockName.Equals(blockName));
            var blockid = block.datablockId;
            var packoffset = dataPacketsourceList.First(x => x.Name.Equals(block.datablockName)).AddressOffset;
            uint blockWidth = 0;
            uint blockWidthOffset = 0;
            uint infoWidth = 0;
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            var t = typeof(T);
            foreach (var p in t.GetFields())
            {
                keyValuePairs.Add(p.Name, p.GetCustomAttribute<CommAddressAttribute>(true).Name);
            }
            //var nowt = pt;
            var infoName = keyValuePairs[propertyName];
            var infoList = dataBlocksourceList.Where(x => x.Name.Equals(block.datablockName)).ToList();
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

    public struct PSD_CMD_MSG_ADDR_ST
    {
        [CommAddressAttribute("站台门控制命令")]
        public CommAddress PsdCmd { get; set; }
    }

    public struct LOSE_DET_MSG_ADDR_ST
    {
        public DEVICE_ID Id { get; set; }

        [CommAddressAttribute("闭塞分区分路状态")]
        public CommAddress LoseDetStat { get; set; }
    }

    public struct BS_STAT_MSG_ADDR_ST
    {
        public DEVICE_ID Id { get; set; }

        [CommAddressAttribute("低频码")]
        public CommAddress LowFreq { get; set; }

        [CommAddressAttribute("分路状态")]
        public CommAddress LoseDetStat { get; set; }

        [CommAddressAttribute("占用状态")]
        public CommAddress OccupyStat { get; set; }
    }

    public struct BALISE_DIR_MSG_ADDR_ST
    { 
        public DEVICE_ID Id { get; set; }

        [CommAddressAttribute("进站口应答器处的区间方向信息")]
        public CommAddress BaliseDir { get; set; }
    }

    public struct PSD_STAT_MSG_ADDR_ST
    {
        public DEVICE_ID Id { get; set; }

        [CommAddressAttribute("站台门状态")]
        public CommAddress PsdStat { get; set; }
    }

    public class Tcc2TsrsInfCfgData
    {
        public UInt32 IsConnExist;
        public DEVICE_ID SrcDeviceId;
        public DEVICE_ID DstDeviceId;
        public UInt32 MaxSilentCycle;
        public UInt32 CommInterval;
    }

    public class Tcc2TsrsReceiveData
    {
        public PSD_CMD_MSG_ADDR_ST[] PsdCmdMsgAddr;
        public UInt32 PsdNum;
        public LOSE_DET_MSG_ADDR_ST[] LoseDetMsgAddr;
        public UInt32 BlkSectNum;
    }

    public class Tcc2TsrsSendData
    {
        public BS_STAT_MSG_ADDR_ST[] BsStatMsgAddr;
        public UInt32 BlkSectNum;
        public BALISE_DIR_MSG_ADDR_ST[] BaliseDirMsgAddr;
        public UInt32 LiaisonBaliseNum;
        public PSD_STAT_MSG_ADDR_ST[] PsdStatMsgAddr;
        public UInt32 PsdNum;
    }
}
