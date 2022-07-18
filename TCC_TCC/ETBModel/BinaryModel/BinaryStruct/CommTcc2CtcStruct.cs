using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TCC_TCC.A.ETBAttribute;
using TCC_TCC.A.ETBAttribute.BinaryModelAttribute;
using TCC_TCC.A.ETBModel.ExcelModel.InterLock;
using TCC_TCC.A.ETBProcess;
using TCC_TCC.A.ETBUtil;

namespace TCC_TCC.A.ETBModel.BinaryModel.BinaryStruct
{
    public class CommTcc2CtcStruct:BaseStruct
    {
        #region 私有变量
        //private List<CommModel> sourceList;
        private List<InterfaceConfigModel> interfaceConfigsourceList;
        private List<InterfaceBitModel> interfaceBitsourceList;
        private List<DataPacketModel> dataPacketsourceList;
        private List<DataBlockModel> dataBlocksourceList;
        #endregion

        public InterfaceInfo InterfaceInfo;
        public Tcc2CtcInfCfgData Tcc2CtcInfCfgData;
        public Tcc2CtcReceiveData Tcc2CtcReceiveData;
        public Tcc2CtcSendData Tcc2CtcSendData;
        public StringBuilder Tcc2CtcSB;

        public CommTcc2CtcStruct(DataModel dataModel)
        {
            MsgHelper.Instance(1,string.Format("正在生成结构体《Tcc2Ctc》"));
            interfaceConfigsourceList = dataModel.BitData.Tcc2CtcInterface.interfaceConfigList;
            interfaceBitsourceList = dataModel.BitData.Tcc2CtcInterface.interfaceBitList;
            dataPacketsourceList = dataModel.BitData.Tcc2CtcInterface.dataPacketList;
            dataBlocksourceList = dataModel.BitData.Tcc2CtcInterface.dataBlockList;
            this.InterfaceInfo.InterfaceName = "Tcc2Ctc";
            this.InterfaceInfo.InterfaceByteNum = 7704U;
            this.InterfaceInfo.InterfaceNum = 1;

            Transform();
        }

        public override void Transform()
        {
            Tcc2CtcInfCfgData = new Tcc2CtcInfCfgData();
            Tcc2CtcReceiveData = new Tcc2CtcReceiveData();
            Tcc2CtcSendData = new Tcc2CtcSendData();

            Tcc2CtcSB = new StringBuilder();
            Tcc2CtcSB.AppendFormat("\r\n\r\n====================Tcc2Ctc====================\r\n");

            try
            {
                Tcc2CtcInfCfgData.IsConnExist = interfaceConfigsourceList.First(x => x.Name.Equals("是否有通信连接")).Value.Equals("是") ? CONST_TRUE : CONST_FALSE;
                Tcc2CtcInfCfgData.SrcDeviceId = GetDEVICE_ID(interfaceConfigsourceList.First(x => x.Name.Equals("源设备Id")).Value) ;
                Tcc2CtcInfCfgData.DstDeviceId = GetDEVICE_ID(interfaceConfigsourceList.First(x => x.Name.Equals("目的设备Id")).Value);
                Tcc2CtcInfCfgData.MyCtcMsgMasterDeviceCtrlBs = GetAreaStruct(interfaceConfigsourceList.First(x => x.Name.Equals("主站的闭塞分区Std75编号范围")).Value);
                Tcc2CtcInfCfgData.MyCtcMsgMasterDeviceCtrlLiaison = GetAreaStruct(interfaceConfigsourceList.First(x => x.Name.Equals("主站的联系口Std75编号范围")).Value);
                Tcc2CtcInfCfgData.MyCtcMsgSlaveDeviceId = GetDEVICE_IDArray(interfaceConfigsourceList.First(x => x.Name.Equals("我的Ctc报文从站设备Id")).Value,8);
                Tcc2CtcInfCfgData.MyCtcMsgSlaveDeviceNum = Convert.ToUInt32(interfaceConfigsourceList.First(x => x.Name.Equals("我的Ctc报文从站设备Id")).Value.SplitToList().Count());
                Tcc2CtcInfCfgData.MyCtcMsgSlaveDeviceCtrlBs = GetAreaStructArray(interfaceConfigsourceList.First(x => x.Name.Equals("从站的闭塞分区Std75编号范围")).Value,8);
                Tcc2CtcInfCfgData.MyCtcMsgSlaveDeviceCtrlLiaison = GetAreaStructArray(interfaceConfigsourceList.First(x => x.Name.Equals("从站的联系口Std75编号范围")).Value, 8);
                Tcc2CtcInfCfgData.MyMasterDeviceId = GetDEVICE_ID(interfaceConfigsourceList.First(x => x.Name.Equals("我的Ctc报文主站设备Id")).Value);
                Tcc2CtcInfCfgData.Tcc2CtcPacketNum = Tcc2CtcInfCfgData.MyCtcMsgSlaveDeviceNum / 2;
                Tcc2CtcInfCfgData.TCC_INF_VER = GetTccInfVer(interfaceConfigsourceList.First(x => x.Name.Equals("接口规范版本")).Value,
                                                interfaceConfigsourceList.First(x => x.Name.Equals("接口数据版本")).Value);
                Tcc2CtcInfCfgData.MaxSilentCycle = Convert.ToUInt32(interfaceConfigsourceList.FirstOrDefault(x => x.Name.Equals("超时等待周期数")).Value);
                Tcc2CtcInfCfgData.CommInterval = Convert.ToUInt32(interfaceConfigsourceList.FirstOrDefault(x => x.Name.Equals("通信间隔周期数")).Value,16);
                #region ReceiveStruct
                //“数据包描述信息-Ctc发送”信息地址
                Tcc2CtcReceiveData.PktDescMsgAddr.PktNum = GetPropertyValue<PKT_DESC_MSG_ADDR_CTC_ST>("数据包描述信息-Ctc发送", "PktNum");
                Tcc2CtcReceiveData.PktDescMsgAddr.PktIndex = GetPropertyValue<PKT_DESC_MSG_ADDR_CTC_ST>("数据包描述信息-Ctc发送", "PktIndex");
                //“区间闭塞分区状态确认命令”信息地址
                Tcc2CtcReceiveData.BsCfmCmdMsgAddr.TccCode = GetPropertyValue<BS_CFM_CMD_MSG_ADDR_ST>("区间闭塞分区状态确认命令", "TccCode");
                Tcc2CtcReceiveData.BsCfmCmdMsgAddr.CmdValue = GetPropertyValue<BS_CFM_CMD_MSG_ADDR_ST>("区间闭塞分区状态确认命令", "CmdValue");
                Tcc2CtcReceiveData.BsCfmCmdMsgAddr.CmdType = GetPropertyValue<BS_CFM_CMD_MSG_ADDR_ST>("区间闭塞分区状态确认命令", "CmdType");
                Tcc2CtcReceiveData.BsCfmCmdMsgAddr.BsOrLiaisonCode = GetPropertyValue<BS_CFM_CMD_MSG_ADDR_ST>("区间闭塞分区状态确认命令", "BsOrLiaisonCode");
                Tcc2CtcReceiveData.BsCfmCmdMsgAddr.CtcCode = GetPropertyValue<BS_CFM_CMD_MSG_ADDR_ST>("区间闭塞分区状态确认命令", "CtcCode");
                Tcc2CtcReceiveData.BsCfmCmdMsgAddr.Crc1 = GetPropertyValue<BS_CFM_CMD_MSG_ADDR_ST>("区间闭塞分区状态确认命令", "Crc1");
                Tcc2CtcReceiveData.BsCfmCmdMsgAddr.Crc2 = GetPropertyValue<BS_CFM_CMD_MSG_ADDR_ST>("区间闭塞分区状态确认命令", "Crc2");
                //“接口版本校验信息-Ctc发送”信息地址
                Tcc2CtcReceiveData.InfVerMsgAddr.InfSpecVer = GetPropertyValue<INF_VER_MSG_ADDR_ST>("接口版本校验信息-Ctc发送", "InfSpecVer");
                Tcc2CtcReceiveData.InfVerMsgAddr.InfDataVer = GetPropertyValue<INF_VER_MSG_ADDR_ST>("接口版本校验信息-Ctc发送", "InfDataVer");
                #endregion

                #region SendStruct
                //“数据包描述信息-Tcc发送”信息地址
                Tcc2CtcSendData.PktDescMsgAddr.PktNum = GetPropertyValue<PKT_DESC_MSG_ADDR_TCC_ST>("数据包描述信息块-Tcc发送","PktNum");
                Tcc2CtcSendData.PktDescMsgAddr.PktIndex = GetPropertyValue<PKT_DESC_MSG_ADDR_TCC_ST>("数据包描述信息块-Tcc发送", "PktIndex");
                //“区间运行方向表示”信息地址
                Tcc2CtcSendData.LiaisonDirMsgAddr = GetLiaisonDirMsgAddr<LIAISON_DIR_MSG_ADDR_ST>("区间运行方向表示", 16);
                //区间口数量
                Tcc2CtcSendData.LiaisonNum = Convert.ToUInt32(interfaceBitsourceList.Select(x => (x.datablockName.Equals("区间运行方向表示") && !string.IsNullOrEmpty(x.Id.ToString()))).Count());
                //“站1综合状态-系统连接”信息地址
                Tcc2CtcSendData.Sta1SysConnStatMsgAddr = GetStaSysConnStatMsgAddr<SYS_CONN_STAT_MSG_ADDR_ST>("站1综合状态-系统连接",9);
                //“站1综合状态-站台门状态”信息地址
                Tcc2CtcSendData.Sta1PsdMsgAddr = GetStaPsdMsgAddr<STA_PSD_MSG_ADDR_ST>("站1综合状态-闭塞分区状态", 48);
                //站1站台门数量
                Tcc2CtcSendData.Sta1PsdNum = Convert.ToUInt32(interfaceBitsourceList.Select(x => (x.datablockName.Equals("站1综合状态-站台门状态") && !string.IsNullOrEmpty(x.Id.ToString()))).Count());
                //“站1综合状态-信号机状态”信息地址
                Tcc2CtcSendData.Sta1SigMsgAddr = GetStaSigMsgAddr<STA_SIG_MSG_ADDR_ST>("站1综合状态-信号机状态", 60);
                //站1信号机数量
                Tcc2CtcSendData.Sta1SigNum = Convert.ToUInt32(interfaceBitsourceList.Select(x => (x.datablockName.Equals("站1综合状态-信号机状态") && !string.IsNullOrEmpty(x.Id.ToString()))).Count());
                //“站1综合状态-闭塞分区状态”信息地址
                Tcc2CtcSendData.Sta1BsMsgAddr = GetStaBsMsgAddr<STA_BS_MSG_ADDR_ST>("站1综合状态-闭塞分区状态", 60);
                //站1闭塞分区数量
                Tcc2CtcSendData.Sta1BsNum = Convert.ToUInt32(interfaceBitsourceList.Select(x => (x.datablockName.Equals("站1综合状态-闭塞分区状态") && !string.IsNullOrEmpty(x.Id.ToString()))).Count());
                //“区间闭塞分区状态确认回执”信息地址
                Tcc2CtcSendData.BsCfmCmdRespMsgAddr.TccCode = GetPropertyValue<BS_CFM_CMD_RESP_MSG_ADDR_ST>("区间闭塞分区状态确认回执","TccCode");
                Tcc2CtcSendData.BsCfmCmdRespMsgAddr.CmdValue = GetPropertyValue<BS_CFM_CMD_RESP_MSG_ADDR_ST>("区间闭塞分区状态确认回执", "CmdValue");
                Tcc2CtcSendData.BsCfmCmdRespMsgAddr.CmdType = GetPropertyValue<BS_CFM_CMD_RESP_MSG_ADDR_ST>("区间闭塞分区状态确认回执", "CmdType");
                Tcc2CtcSendData.BsCfmCmdRespMsgAddr.BsOrLiaisonCode = GetPropertyValue<BS_CFM_CMD_RESP_MSG_ADDR_ST>("区间闭塞分区状态确认回执", "BsOrLiaisonCode");
                Tcc2CtcSendData.BsCfmCmdRespMsgAddr.CtcCode = GetPropertyValue<BS_CFM_CMD_RESP_MSG_ADDR_ST>("区间闭塞分区状态确认回执", "CtcCode");
                Tcc2CtcSendData.BsCfmCmdRespMsgAddr.ExeclResult = GetPropertyValue<BS_CFM_CMD_RESP_MSG_ADDR_ST>("区间闭塞分区状态确认回执", "ExeclResult");
                Tcc2CtcSendData.BsCfmCmdRespMsgAddr.FltCause = GetPropertyValue<BS_CFM_CMD_RESP_MSG_ADDR_ST>("区间闭塞分区状态确认回执", "FltCause");
                Tcc2CtcSendData.BsCfmCmdRespMsgAddr.Crc1 = GetPropertyValue<BS_CFM_CMD_RESP_MSG_ADDR_ST>("区间闭塞分区状态确认回执", "Crc1");
                Tcc2CtcSendData.BsCfmCmdRespMsgAddr.Crc2 = GetPropertyValue<BS_CFM_CMD_RESP_MSG_ADDR_ST>("区间闭塞分区状态确认回执", "Crc2");
                //“接口版本校验信息-Tcc发送”信息地址
                Tcc2CtcSendData.InfVerMsgAddr.InfSpecVer = GetPropertyValue<INF_VER_MSG_ADDR_ST>("接口版本校验信息-Tcc发送", "InfSpecVer");
                Tcc2CtcSendData.InfVerMsgAddr.InfDataVer = GetPropertyValue<INF_VER_MSG_ADDR_ST>("接口版本校验信息-Tcc发送", "InfDataVer");
                #endregion

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        
        private 

        private STA_BS_MSG_ADDR_ST[] GetStaBsMsgAddr<T>(string blockName,UInt32 length)
        {
            STA_BS_MSG_ADDR_ST[] result = new STA_BS_MSG_ADDR_ST[length];
            for (int i = 0;i < length;i++)
            {
                result[i].Id = new DEVICE_ID();
                result[i].BsLowFreq = new CommAddress();
                result[i].BsStat = new CommAddress();
            }

            var blockList = interfaceBitsourceList.Where(x => x.datablockName.Equals(blockName)).ToList();
            foreach (var item in blockList)
            {
                result[blockList.IndexOf(item)].Id = GetBitMapDEVICE_ID<InterfaceBitModel>(item.datablockId,blockList);
                result[blockList.IndexOf(item)].BsLowFreq = GetPropertyValue<T>(item.datablockName,item.datablockId, "BsLowFreq");
                result[blockList.IndexOf(item)].BsStat = GetPropertyValue<T>(item.datablockName, item.datablockId, "BsStat");
            }

            return result;
        }


        private STA_PSD_MSG_ADDR_ST[] GetStaPsdMsgAddr<T>(string blockName,UInt32 length)
        {
            STA_PSD_MSG_ADDR_ST[] result = new STA_PSD_MSG_ADDR_ST[length];

            var blockList = interfaceBitsourceList.Where(x => x.datablockName.Equals(blockName)).ToList();
            foreach (var item in blockList)
            { 
                result[blockList.IndexOf(item)].Id = GetBitMapDEVICE_ID<InterfaceBitModel>(item.datablockId, blockList);
                result[blockList.IndexOf(item)].DoorLockStat = GetPropertyValue<T>(item.datablockName,item.datablockId,"DoorLockStat");
                result[blockList.IndexOf(item)].DoorAlarmRelay = GetPropertyValue<T>(item.datablockName,item.datablockId,"DoorAlarmRelay");
                result[blockList.IndexOf(item)].DoorBypassRelay = GetPropertyValue<T>(item.datablockName,item.datablockId,"DoorBypassRelay");
                result[blockList.IndexOf(item)].DoorProtectRelay = GetPropertyValue<T>(item.datablockName,item.datablockId,"DoorProtectRelay");
            }

            return result;
        }

        private SYS_CONN_STAT_MSG_ADDR_ST[] GetStaSysConnStatMsgAddr<T>(string blockName,UInt32 length)
        {
            SYS_CONN_STAT_MSG_ADDR_ST[] result = new SYS_CONN_STAT_MSG_ADDR_ST[length];

            var blockList = interfaceBitsourceList.Where(x => x.datablockName.Equals(blockName)).ToList();
            foreach (var item in blockList)
            {
                result[blockList.IndexOf(item)].TccCode = GetPropertyValue<T>(item.datablockName,item.datablockId,"TccCode");
                result[blockList.IndexOf(item)].Tcc2CbiConnStat = GetPropertyValue<T>(item.datablockName,item.datablockId,"Tcc2CbiConnStat");
                result[blockList.IndexOf(item)].Tcc2CtcConnStat = GetPropertyValue<T>(item.datablockName,item.datablockId,"Tcc2CtcConnStat");
                result[blockList.IndexOf(item)].MasterOrSlave = GetPropertyValue<T>(item.datablockName,item.datablockId,"MasterOrSlave");
                result[blockList.IndexOf(item)].SingleOrDualSys = GetPropertyValue<T>(item.datablockName,item.datablockId,"SingleOrDuaSys");
                result[blockList.IndexOf(item)].Tcc2TccConnStat = GetPropertyValue<T>(item.datablockName,item.datablockId,"Tcc2TccConnStat");
                result[blockList.IndexOf(item)].Tcc2TsrsConnStat = GetPropertyValue<T>(item.datablockName,item.datablockId,"Tcc2TsrsConnStat");
                result[blockList.IndexOf(item)].MainStaConnStat = GetPropertyValue<T>(item.datablockName,item.datablockId,"MainStaConnStat");
                result[blockList.IndexOf(item)].BlkSectInitStat = GetPropertyValue<T>(item.datablockName,item.datablockId,"BlkSectInitStat");
            }

            return result;
        }

        private STA_SIG_MSG_ADDR_ST[] GetStaSigMsgAddr<T>(string blockName, UInt32 length)
        {
            STA_SIG_MSG_ADDR_ST[] result = new STA_SIG_MSG_ADDR_ST[length];
            for (int i = 0;i < length;i++)
            {
                result[i].Id = new DEVICE_ID();
                result[i].SigStat = new CommAddress();
            }

            var blockList = interfaceBitsourceList.Where(x => x.datablockName.Equals(blockName)).ToList();
            foreach (var item in blockList)
            {
                result[blockList.IndexOf(item)].Id = GetBitMapDEVICE_ID<InterfaceBitModel>(item.datablockId, blockList);
                result[blockList.IndexOf(item)].SigStat = GetPropertyValue<T>(item.datablockName, item.datablockId, "SigStat");
            }

            return result;
        }

        private LIAISON_DIR_MSG_ADDR_ST[] GetLiaisonDirMsgAddr<T>(string blockName,UInt32 length)
        {
            LIAISON_DIR_MSG_ADDR_ST[] result = new LIAISON_DIR_MSG_ADDR_ST[length];

            var blockList = interfaceBitsourceList.Where(x => x.datablockName.Equals(blockName)).ToList();
            foreach (var item in blockList)
            {
                result[blockList.IndexOf(item)].Id = GetBitMapDEVICE_ID<InterfaceBitModel>(item.datablockId,blockList);
                result[blockList.IndexOf(item)].LogicDetStat = GetPropertyValue<T>(item.datablockName,item.datablockId, "LogicDetStat");
                result[blockList.IndexOf(item)].Qjzt = GetPropertyValue<T>(item.datablockName, item.datablockId, "Qjzt");
                result[blockList.IndexOf(item)].Jqj = GetPropertyValue<T>(item.datablockName, item.datablockId, "Jqj");
                result[blockList.IndexOf(item)].Fj = GetPropertyValue<T>(item.datablockName, item.datablockId, "Fj");
            }

            return result;
        }

        private CommAddress GetPropertyValue<T>(string blockName, UInt32 blockId,string propertyName)
        {
            var block = interfaceBitsourceList.First(x => (x.datablockName.Equals(blockName)&&x.datablockId.Equals(blockId)));
            var blockid = blockId;
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

        private CommAddress GetPropertyValue<T>(string blockName,string propertyName)
        {
            var block = interfaceBitsourceList.First(x => x.datablockName.Equals(blockName));
            var blockid = block.datablockId;
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
            var infoList = dataBlocksourceList.Where(x => x.Name.Equals(block.datablockName)).ToList();
            foreach (var item in infoList)
            {
                blockWidth += item.BitWidth;
                //if (!item.InfoName.Equals(infoName))
                //{
                //    blockWidthOffset += item.BitWidth;
                //}
                if (item.InfoName.Equals(infoName))
                {
                    for (int i = 0; i < infoList.IndexOf(item); i++)
                    {
                        blockWidthOffset += infoList[i].BitWidth;
                    }
                    infoWidth = item.BitWidth;
                }
            }

           

            return GetCommAddress(blockid,packoffset,blockWidth,blockWidthOffset,infoWidth);
        }

        private TCC_INF_VER GetTccInfVer(string standVersion,string dataVersion)
        {
            TCC_INF_VER result = new TCC_INF_VER();
            result.SpecVer = new Byte[4];
            result.DataVer = new Byte[4];

            var standVersionList = standVersion.Split('.');
            var dataVersionList = dataVersion.Split('.');

            result.SpecVer[0] = Convert.ToByte(standVersionList[0]);
            result.SpecVer[1] = Convert.ToByte(standVersionList[1]);
            result.SpecVer[2] = Convert.ToByte(standVersionList[2]);
            result.SpecVer[3] = Convert.ToByte(standVersionList[3]);

            result.DataVer[0] = Convert.ToByte(dataVersionList[0]);
            result.DataVer[1] = Convert.ToByte(dataVersionList[1]);
            result.DataVer[2] = Convert.ToByte(dataVersionList[2]);
            result.DataVer[3] = Convert.ToByte(dataVersionList[3]);

            return result;
        }

        private MyCtcMsgDeviceCtrlAreaStruct[] GetAreaStructArray(string values,uint length)
        {
            MyCtcMsgDeviceCtrlAreaStruct[] result = new MyCtcMsgDeviceCtrlAreaStruct[length];
            for (int index = 0;index < length;index++)
            {
                result[index].StartStd75Code = UInt32.MaxValue;
                result[index].EndStd75Code = UInt32.MaxValue;
            }

            var valueList = values.SplitToList();
            foreach (var item in valueList)
            {
                var value = item.Split('-');
                result[valueList.IndexOf(item)].StartStd75Code = Convert.ToUInt32(value[0]);
                result[valueList.IndexOf(item)].EndStd75Code = Convert.ToUInt32(value[1]);
            }

            return result;
        }

        private MyCtcMsgDeviceCtrlAreaStruct GetAreaStruct(string value)
        {
            MyCtcMsgDeviceCtrlAreaStruct result = new MyCtcMsgDeviceCtrlAreaStruct();
            result.StartStd75Code = UInt32.MaxValue;
            result.EndStd75Code = UInt32.MinValue;

            string[] data = value.Split('-');

            result.StartStd75Code = Convert.ToUInt32(data[0]);
            result.EndStd75Code = Convert.ToUInt32(data[1]);

            return result;
        }

        private DEVICE_ID[] GetDEVICE_IDArray(string ids,uint length)
        {
            DEVICE_ID[] result = new DEVICE_ID[length];

            for (int index = 0; index < length;index++)
            {
                result[index] = CONST_DEVICE_ID;
            }

            var idList = ids.SplitToList();
            foreach (var item in idList)
            {
                result[idList.IndexOf(item)] = GetDEVICE_ID(item);
            }

            return result;
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

    public struct MyCtcMsgDeviceCtrlAreaStruct
    {
        public UInt32 StartStd75Code;
        public UInt32 EndStd75Code;

        
    }

   


    public class PKT_DESC_MSG_ADDR_TCC_ST
    {
        [CommAddressAttribute("数据包总数")]
        public CommAddress PktNum { get; set; }

        [CommAddressAttribute("当前包序号")]
        public CommAddress PktIndex { get; set; }
    }

    public class PKT_DESC_MSG_ADDR_CTC_ST
    {
        [CommAddressAttribute("数据包总数")]
        public CommAddress PktNum { get; set; }

        [CommAddressAttribute("当前数据包序号")]
        public CommAddress PktIndex { get; set; }
    }

    public class BS_CFM_CMD_MSG_ADDR_ST
    {
        [CommAddressAttribute("TCC编号")]
        public CommAddress TccCode { get; set; }

        [CommAddressAttribute("确认状态属性")]
        public CommAddress CmdValue { get; set; }

        [CommAddressAttribute("状态确认命令类型")]
        public CommAddress CmdType { get; set; }

        [CommAddressAttribute("确认闭塞分区编号/区间方向口编号")]
        public CommAddress BsOrLiaisonCode { get; set; }

        [CommAddressAttribute("确认操作源CTC编号")]
        public CommAddress CtcCode { get; set; }

        [CommAddressAttribute("CRC校验值1")]
        public CommAddress Crc1 { get; set; }

        [CommAddressAttribute("CRC校验值2")]
        public CommAddress Crc2 { get; set; }
    }

    public class INF_VER_MSG_ADDR_ST
    {
        [CommAddressAttribute("接口规范版本校验信息")]
        public CommAddress InfSpecVer { get; set; }

        [CommAddressAttribute("接口数据版本校验信息")]
        public CommAddress InfDataVer { get; set; }
    }

    public class STA_PSD_MSG_ADDR_ST
    {
        public DEVICE_ID Id { get; set; }

        [CommAddressAttribute("门锁闭状态")]
        public CommAddress DoorLockStat { get; set; }

        [CommAddressAttribute("门报警继电器")]
        public CommAddress DoorAlarmRelay { get; set; }

        [CommAddressAttribute("门旁路继电器")]
        public CommAddress DoorBypassRelay { get; set; }

        [CommAddressAttribute("门防护继电器")]
        public CommAddress DoorProtectRelay { get; set; }
    }

    public class SYS_CONN_STAT_MSG_ADDR_ST
    {
        [CommAddressAttribute("TCC编号")]
        public CommAddress TccCode { get; set; }

        [CommAddressAttribute("与CBI连接")]
        public CommAddress Tcc2CbiConnStat { get; set; }

        [CommAddressAttribute("与CTC连接")]
        public CommAddress Tcc2CtcConnStat { get; set; }

        [CommAddressAttribute("主控机")]
        public CommAddress MasterOrSlave { get; set; }

        [CommAddressAttribute("单双机")]
        public CommAddress SingleOrDualSys { get; set; }

        [CommAddressAttribute("列控站间连接")]
        public CommAddress Tcc2TccConnStat { get; set; }

        [CommAddressAttribute("与TSRS连接")]
        public CommAddress Tcc2TsrsConnStat { get; set; }

        [CommAddressAttribute("与主站通信状态")]
        public CommAddress MainStaConnStat { get; set; }

        [CommAddressAttribute("闭塞分区上电初始化状态")]
        public CommAddress BlkSectInitStat { get; set; }
    }

    public class LIAISON_DIR_MSG_ADDR_ST
    {
        public DEVICE_ID Id { get; set; }

        [CommAddressAttribute("区间占用逻辑检查功能状态")]
        public CommAddress LogicDetStat { get; set; }

        [CommAddressAttribute("发车口Qjzt信息")]
        public CommAddress Qjzt { get; set; }

        [CommAddressAttribute("发车口Jqj信息")]
        public CommAddress Jqj { get; set; }

        [CommAddressAttribute("发车口Fj")]
        public CommAddress Fj { get; set; }
    }

    public class STA_SIG_MSG_ADDR_ST
    {
        public DEVICE_ID Id { get; set; }

        [CommAddressAttribute("信号机显示状态")]
        public CommAddress SigStat { get; set; }

      
    }

    public class STA_BS_MSG_ADDR_ST
    {
        public DEVICE_ID Id { get; set; }

        [CommAddressAttribute("闭塞分区轨道电路低频吗")]
        public CommAddress BsLowFreq { get; set; }

        [CommAddressAttribute("闭塞分区状态")]
        public CommAddress BsStat { get; set; }
       
    }

    public class BS_CFM_CMD_RESP_MSG_ADDR_ST
    {
        [CommAddressAttribute("TCC编号")]
        public CommAddress TccCode { get; set; }

        [CommAddressAttribute("确认状态属性")]
        public CommAddress CmdValue { get; set; }

        [CommAddressAttribute("状态确认命令类型")]
        public CommAddress CmdType { get; set; }

        [CommAddressAttribute("确认闭塞分区编号/区间方向口编号")]
        public CommAddress BsOrLiaisonCode { get; set; }

        [CommAddressAttribute("确认操作源CTC编号")]
        public CommAddress CtcCode { get; set; }

        [CommAddressAttribute("命令执行结果")]
        public CommAddress ExeclResult { get; set; }

        [CommAddressAttribute("命令执行失败原因")]
        public CommAddress FltCause { get; set; }

        [CommAddressAttribute("CRC校验值1")]
        public CommAddress Crc1 { get; set; }

        [CommAddressAttribute("CRC校验值2")]
        public CommAddress Crc2 { get; set; }
    }

   

    public class Tcc2CtcInfCfgData 
    {
        public UInt32 IsConnExist { get; set; }
        public DEVICE_ID SrcDeviceId { get; set; }
        public DEVICE_ID DstDeviceId { get; set; }
        public MyCtcMsgDeviceCtrlAreaStruct MyCtcMsgMasterDeviceCtrlBs { get; set; }
        public MyCtcMsgDeviceCtrlAreaStruct MyCtcMsgMasterDeviceCtrlLiaison { get; set; }
        public DEVICE_ID[] MyCtcMsgSlaveDeviceId { get; set; }
        public UInt32 MyCtcMsgSlaveDeviceNum { get; set; }
        public MyCtcMsgDeviceCtrlAreaStruct[] MyCtcMsgSlaveDeviceCtrlBs { get; set; }
        public MyCtcMsgDeviceCtrlAreaStruct[] MyCtcMsgSlaveDeviceCtrlLiaison { get; set; }
        public DEVICE_ID MyMasterDeviceId { get; set; }
        public UInt32 Tcc2CtcPacketNum { get; set; }
        public TCC_INF_VER TCC_INF_VER { get; set; }
        public UInt32 MaxSilentCycle { get; set; }
        public UInt32 CommInterval { get; set; }
    }

    public class Tcc2CtcReceiveData
    {
        public PKT_DESC_MSG_ADDR_CTC_ST PktDescMsgAddr { get; set; }
        public BS_CFM_CMD_MSG_ADDR_ST BsCfmCmdMsgAddr { get; set; }
        public INF_VER_MSG_ADDR_ST InfVerMsgAddr { get; set; }
    }

    public class Tcc2CtcSendData
    {
        public PKT_DESC_MSG_ADDR_TCC_ST PktDescMsgAddr { get; set; }
        public LIAISON_DIR_MSG_ADDR_ST[] LiaisonDirMsgAddr { get; set; }
        public UInt32 LiaisonNum { get; set; }
        public SYS_CONN_STAT_MSG_ADDR_ST[] Sta1SysConnStatMsgAddr { get; set; }
        public STA_PSD_MSG_ADDR_ST[] Sta1PsdMsgAddr { get; set; }
        public UInt32 Sta1PsdNum { get; set; }
        public STA_SIG_MSG_ADDR_ST[] Sta1SigMsgAddr { get; set; }
        public UInt32 Sta1SigNum { get; set; }
        public STA_BS_MSG_ADDR_ST[] Sta1BsMsgAddr { get; set; }
        public UInt32 Sta1BsNum { get; set; }
        public BS_CFM_CMD_RESP_MSG_ADDR_ST BsCfmCmdRespMsgAddr { get; set; }
        public INF_VER_MSG_ADDR_ST InfVerMsgAddr { get; set; }
    }
}
