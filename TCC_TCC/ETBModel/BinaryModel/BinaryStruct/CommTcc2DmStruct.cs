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
    public class CommTcc2DmStruct:BaseStruct
    {
        #region 私有变量
        private List<InterfaceConfigModel> interfaceConfigsourceList;
        private List<InterfaceBitModel> interfaceBitsourceList;
        private List<DataPacketModel> dataPacketsourceList;
        private List<DataBlockModel> dataBlocksourceList;
        #endregion

        public InterfaceInfo InterfaceInfo;
        public Tcc2DmInfCfgData Tcc2DmInfCfgData;
        public Tcc2DmSendData Tcc2DmSendData;
        public StringBuilder Tcc2DmSB;

        public CommTcc2DmStruct(DataModel dataModel)
        {
            MsgHelper.Instance(1, string.Format("正在生成结构体《Tcc2Dm》"));
            interfaceConfigsourceList = dataModel.BitData.Tcc2DmInterface.interfaceConfigList;
            interfaceBitsourceList = dataModel.BitData.Tcc2DmInterface.interfaceBitList;
            dataPacketsourceList = dataModel.BitData.Tcc2DmInterface.dataPacketList;
            dataBlocksourceList = dataModel.BitData.Tcc2DmInterface.dataBlockList;
            this.InterfaceInfo.InterfaceName = "Tcc2Dm";
            this.InterfaceInfo.InterfaceByteNum = 1224;
            this.InterfaceInfo.InterfaceNum = 1;

            Transform();
        }

        public override void Transform()
        {
            Tcc2DmInfCfgData = new Tcc2DmInfCfgData();
            Tcc2DmSendData = new Tcc2DmSendData();

            Tcc2DmSB = new StringBuilder();
            Tcc2DmSB.AppendFormat("\r\n\r\n============================Tcc2Dm============================\r\n");
            try
            {
                Tcc2DmInfCfgData.IsConnExist = interfaceConfigsourceList.First(x => x.Name.Equals("是否有通信连接")).Value.Equals("是")?CONST_TRUE:CONST_FALSE;
                Tcc2DmInfCfgData.SrcDeviceId = GetDEVICE_ID(interfaceConfigsourceList.First(x => x.Name.Equals("源设备Id")).Value);
                Tcc2DmInfCfgData.DstDeviceId = GetDEVICE_ID(interfaceConfigsourceList.First(x => x.Name.Equals("目的设备Id")).Value);
                Tcc2DmInfCfgData.MaxSilentCycle = Convert.ToUInt32(interfaceConfigsourceList.First(x => x.Name.Equals("超时等待周期数")).Value);
                Tcc2DmInfCfgData.CommInterval = Convert.ToUInt32(interfaceConfigsourceList.First(x => x.Name.Equals("通信间隔周期数")).Value);
                Tcc2DmInfCfgData.TCC_INF_VER = GetTccInfVer(interfaceConfigsourceList.First(x => x.Name.Equals("接口规范版本")).Value,
                                                interfaceConfigsourceList.First(x => x.Name.Equals("接口数据版本")).Value);

                #region SendStruct
                //"硬件状态信息"信息地址
                Tcc2DmSendData.HwStatMsgAddr.Std75Code = GetPropertyValue<HW_STAT_MSG_ADDR_ST>("Tcc硬件平台状态数据","Std75Code");
                //"Tcc与联锁接口进路信息"
                Tcc2DmSendData.RtFromCbiMsgAddr.RtStd75Code = GetPropertyValue<RT_FROM_CBI_MSG_ADDR_ST>("Tcc与联锁接口进路信息","RtStd75Code");
                Tcc2DmSendData.RtFromCbiMsgAddr.BlkNum = GetPropertyValue<RT_FROM_CBI_MSG_ADDR_ST>("Tcc与联锁接口进路信息","BlkNum");
                Tcc2DmSendData.RtFromCbiMsgAddr.SigStat = GetPropertyValue<RT_FROM_CBI_MSG_ADDR_ST>("Tcc与联锁接口进路信息","SigStat");
                Tcc2DmSendData.RtFromCbiMsgAddr.BlkLockStat = new CommAddress[20];
                for (int i = 0;i < 20;i++)
                {
                    Tcc2DmSendData.RtFromCbiMsgAddr.BlkLockStat[i] = new CommAddress();
                }
                //"Tcc与联锁改方命令信息"
                Tcc2DmSendData.ChgDirCmdFromCbiMsgAddr.LiaisonStd75Code = GetPropertyValue<CHG_DIR_CMD_FROM_CBI_MSG_ADDR_ST>("Tcc与联锁改方命令信息","LiaisonStd75Code");
                Tcc2DmSendData.ChgDirCmdFromCbiMsgAddr.Fsj = GetPropertyValue<CHG_DIR_CMD_FROM_CBI_MSG_ADDR_ST>("Tcc与联锁改方命令信息","Fsj");
                Tcc2DmSendData.ChgDirCmdFromCbiMsgAddr.Fqj = GetPropertyValue<CHG_DIR_CMD_FROM_CBI_MSG_ADDR_ST>("Tcc与联锁改方命令信息","Fqj");
                Tcc2DmSendData.ChgDirCmdFromCbiMsgAddr.Jfaj = GetPropertyValue<CHG_DIR_CMD_FROM_CBI_MSG_ADDR_ST>("Tcc与联锁改方命令信息","Jfaj");
                Tcc2DmSendData.ChgDirCmdFromCbiMsgAddr.Ffaj = GetPropertyValue<CHG_DIR_CMD_FROM_CBI_MSG_ADDR_ST>("Tcc与联锁改方命令信息","Ffaj");
                //"Tcc与联锁改方回执信息"
                Tcc2DmSendData.ChgDirResp2CbiMsgAddr.LiaisonStd75Code = GetPropertyValue<CHG_DIR_RESP2CBI_MSG_ADDR_ST>("Tcc与联锁改方回执信息","LiaisonStd75Code");
                Tcc2DmSendData.ChgDirResp2CbiMsgAddr.Yfj = GetPropertyValue<CHG_DIR_RESP2CBI_MSG_ADDR_ST>("Tcc与联锁改方回执信息","Yfj");
                Tcc2DmSendData.ChgDirResp2CbiMsgAddr.SectStat = GetPropertyValue<CHG_DIR_RESP2CBI_MSG_ADDR_ST>("Tcc与联锁改方回执信息","SectStat");
                Tcc2DmSendData.ChgDirResp2CbiMsgAddr.Jqj = GetPropertyValue<CHG_DIR_RESP2CBI_MSG_ADDR_ST>("Tcc与联锁改方回执信息","Jqj");
                Tcc2DmSendData.ChgDirResp2CbiMsgAddr.Fj = GetPropertyValue<CHG_DIR_RESP2CBI_MSG_ADDR_ST>("Tcc与联锁改方回执信息","Fj");
                Tcc2DmSendData.ChgDirResp2CbiMsgAddr.stAuxlLamp = GetPropertyValue<CHG_DIR_RESP2CBI_MSG_ADDR_ST>("Tcc与联锁改方回执信息", "stAuxlLamp");
                //"Tcc站间改方命令信息"
                Tcc2DmSendData.ChgDirCmdFromTccMsgAddr.LiaisonStd75Code = GetPropertyValue<CHG_DIR_RESP2TCC_MSG_ADDR_ST>("Tcc站间改方命令信息","LiaisonStd75Code");
                Tcc2DmSendData.ChgDirCmdFromTccMsgAddr.SectStat = GetPropertyValue<CHG_DIR_RESP2TCC_MSG_ADDR_ST>("Tcc站间改方命令信息","SectStat");
                Tcc2DmSendData.ChgDirCmdFromTccMsgAddr.Fsj = GetPropertyValue<CHG_DIR_RESP2TCC_MSG_ADDR_ST>("Tcc站间改方命令信息","Fsj");
                Tcc2DmSendData.ChgDirCmdFromTccMsgAddr.DptOrRcvDir = GetPropertyValue<CHG_DIR_RESP2TCC_MSG_ADDR_ST>("Tcc站间改方命令信息","DptOrRcvDir");
                Tcc2DmSendData.ChgDirCmdFromTccMsgAddr.ChgDirCmd = GetPropertyValue<CHG_DIR_RESP2TCC_MSG_ADDR_ST>("Tcc站间改方命令信息","ChgDirCmd");
                Tcc2DmSendData.ChgDirCmdFromTccMsgAddr.ChgDirType = GetPropertyValue<CHG_DIR_RESP2TCC_MSG_ADDR_ST>("Tcc站间改方命令信息","ChgDirType");
                //"Tcc站间改方回执信息"
                Tcc2DmSendData.ChgDirResp2TccMsgAddr.LiaisonStd75Code = GetPropertyValue<CHG_DIR_RESP2TCC_MSG_ADDR_ST>("Tcc站间改方回执信息","LiaisonStd75Code");
                Tcc2DmSendData.ChgDirResp2TccMsgAddr.SectStat = GetPropertyValue<CHG_DIR_RESP2TCC_MSG_ADDR_ST>("Tcc站间改方回执信息","SectStat");
                Tcc2DmSendData.ChgDirResp2TccMsgAddr.Fsj = GetPropertyValue<CHG_DIR_RESP2TCC_MSG_ADDR_ST>("Tcc站间改方回执信息","Fsj");
                Tcc2DmSendData.ChgDirResp2TccMsgAddr.DptOrRcvDir = GetPropertyValue<CHG_DIR_RESP2TCC_MSG_ADDR_ST>("Tcc站间改方回执信息","DptOrRcvDir");
                Tcc2DmSendData.ChgDirResp2TccMsgAddr.ChgDirCmd = GetPropertyValue<CHG_DIR_RESP2TCC_MSG_ADDR_ST>("Tcc站间改方回执信息","ChgDirCmd");
                Tcc2DmSendData.ChgDirResp2TccMsgAddr.ChgDirType = GetPropertyValue<CHG_DIR_RESP2TCC_MSG_ADDR_ST>("Tcc站间改方回执信息","ChgDirType");
                //"Tcc站间边界信息"
                Tcc2DmSendData.BrdMsgAddr.BrdStd75Code = GetPropertyValue<BRD_MSG_ADDR_ST>("Tcc站间边界信息","BrdStd75Code");
                Tcc2DmSendData.BrdMsgAddr.Reserve1 = GetPropertyValue<BRD_MSG_ADDR_ST>("Tcc站间边界信息","Reserve1");
                Tcc2DmSendData.BrdMsgAddr.LowFreq = GetPropertyValue<BRD_MSG_ADDR_ST>("Tcc站间边界信息","LowFreq");
                Tcc2DmSendData.BrdMsgAddr.LowFreqFlag = GetPropertyValue<BRD_MSG_ADDR_ST>("Tcc站间边界信息", "LowFreqFlag");
                Tcc2DmSendData.BrdMsgAddr.OccupyStat = GetPropertyValue<BRD_MSG_ADDR_ST>("Tcc站间边界信息","OccupyStat");
                Tcc2DmSendData.BrdMsgAddr.Reserve2 = new CommAddress[5];
                for (int i = 0; i < 5; i++)
                {
                    Tcc2DmSendData.BrdMsgAddr.Reserve2[i] = new CommAddress();
                }
                //"发送给联锁的无配线站区间方向口信息"
                Tcc2DmSendData.SubLiaisonDir2CbiMsgAddr.SubLiaisonStd75Code = GetPropertyValue<SUBLIAISON_DIR2CBI_MSG_ADDR_ST>("发送给联锁的无配线站区间方向口信息","SubLiaisonStd75Code");
                Tcc2DmSendData.SubLiaisonDir2CbiMsgAddr.Yfj = GetPropertyValue<SUBLIAISON_DIR2CBI_MSG_ADDR_ST>("发送给联锁的无配线站区间方向口信息", "Yfj");
                Tcc2DmSendData.SubLiaisonDir2CbiMsgAddr.SectStat = GetPropertyValue<SUBLIAISON_DIR2CBI_MSG_ADDR_ST>("发送给联锁的无配线站区间方向口信息","SectStat");
                Tcc2DmSendData.SubLiaisonDir2CbiMsgAddr.Reserve = GetPropertyValue<SUBLIAISON_DIR2CBI_MSG_ADDR_ST>("发送给联锁的无配线站区间方向口信息","Reserve");
                //"Tcc临时限速状态"
                Tcc2DmSendData.TsrMsgAddr.TsrSpeed = GetPropertyValue<TSR_MSG_ADDR_ST>("Tcc临时限速状态","TsrSpeed");
                Tcc2DmSendData.TsrMsgAddr.CtcCmdCodePart1 = GetPropertyValue<TSR_MSG_ADDR_ST>("Tcc临时限速状态","CtcCmdCodePart1");
                Tcc2DmSendData.TsrMsgAddr.CtcCmdCodePart2 = GetPropertyValue<TSR_MSG_ADDR_ST>("Tcc临时限速状态","CtcCmdCodePart2");
                Tcc2DmSendData.TsrMsgAddr.CtcCmdCodePart3 = GetPropertyValue<TSR_MSG_ADDR_ST>("Tcc临时限速状态", "CtcCmdCodePart3");
                Tcc2DmSendData.TsrMsgAddr.CtcCmdCodePart4 = GetPropertyValue<TSR_MSG_ADDR_ST>("Tcc临时限速状态", "CtcCmdCodePart4");
                Tcc2DmSendData.TsrMsgAddr.CtcOrTccCode = GetPropertyValue<TSR_MSG_ADDR_ST>("Tcc临时限速状态", "CtcOrTccCode");
                Tcc2DmSendData.TsrMsgAddr.UserId = GetPropertyValue<TSR_MSG_ADDR_ST>("Tcc临时限速状态", "UserId");
                Tcc2DmSendData.TsrMsgAddr.TsrCode = GetPropertyValue<TSR_MSG_ADDR_ST>("Tcc临时限速状态", "TsrCode");
                Tcc2DmSendData.TsrMsgAddr.LineCode = GetPropertyValue<TSR_MSG_ADDR_ST>("Tcc临时限速状态", "LineCode");
                Tcc2DmSendData.TsrMsgAddr.TsrCause = GetPropertyValue<TSR_MSG_ADDR_ST>("Tcc临时限速状态", "TsrCause");
                Tcc2DmSendData.TsrMsgAddr.StartChaingeSys = GetPropertyValue<TSR_MSG_ADDR_ST>("Tcc临时限速状态", "StartChaingeSys");
                Tcc2DmSendData.TsrMsgAddr.StartChainge = GetPropertyValue<TSR_MSG_ADDR_ST>("Tcc临时限速状态", "StartChainge");
                Tcc2DmSendData.TsrMsgAddr.EndChaingeSys = GetPropertyValue<TSR_MSG_ADDR_ST>("Tcc临时限速状态", "EndChaingeSys");
                Tcc2DmSendData.TsrMsgAddr.EndChainge = GetPropertyValue<TSR_MSG_ADDR_ST>("Tcc临时限速状态", "EndChainge");
                Tcc2DmSendData.TsrMsgAddr.Reserve1 = GetPropertyValue<TSR_MSG_ADDR_ST>("Tcc临时限速状态","Reserve1");
                Tcc2DmSendData.TsrMsgAddr.Reserve2 = GetPropertyValue<TSR_MSG_ADDR_ST>("Tcc临时限速状态", "Reserve2");
                //"轨道区段编码信息"
                Tcc2DmSendData.BlkTcCodeMsgAddr.XgCarrFreq = GetPropertyValue<BLK_TC_CODE_MSG_ADDR_ST>("轨道区段编码信息","XgCarrFreq");
                Tcc2DmSendData.BlkTcCodeMsgAddr.ZgCarrFreq = GetPropertyValue<BLK_TC_CODE_MSG_ADDR_ST>("轨道区段编码信息","ZgCarrFreq");
                Tcc2DmSendData.BlkTcCodeMsgAddr.ZgLowFreq = GetPropertyValue<BLK_TC_CODE_MSG_ADDR_ST>("轨道区段编码信息","ZgLowFreq");
                Tcc2DmSendData.BlkTcCodeMsgAddr.Reserve1 = GetPropertyValue<BLK_TC_CODE_MSG_ADDR_ST>("轨道区段编码信息","Reserve1");
                Tcc2DmSendData.BlkTcCodeMsgAddr.XgLowFreq = GetPropertyValue<BLK_TC_CODE_MSG_ADDR_ST>("轨道区段编码信息","XgLowFreq");
                Tcc2DmSendData.BlkTcCodeMsgAddr.Reserve2 = GetPropertyValue<BLK_TC_CODE_MSG_ADDR_ST>("轨道区段编码信息","Reserve2");
                //"状态信息-区段空间占用状态"
                Tcc2DmSendData.StatInfo_PioBlkStatMsgAddr.BlkOccupyStat_FromRelay = GetPropertyValue<STAT_INFO_PIO_BLK_STAT_MSG_ADDR_ST>("状态信息-区段空闲占用状态", "BlkOccupyStat_FromRelay");
                Tcc2DmSendData.StatInfo_PioBlkStatMsgAddr.BlkOccupyStat_FromCan = GetPropertyValue<STAT_INFO_PIO_BLK_STAT_MSG_ADDR_ST>("状态信息-区段空闲占用状态", "BlkOccupyStat_FromCan");
                //"状态信息-区间方向"
                Tcc2DmSendData.StatInfo_PioLiaisonDirMsgAddr.LiaisonDirOutput = GetPropertyValue<STAT_INFO_PIO_LIAISON_DIR_MSG_ADDR_ST>("状态信息-区间方向","LiaisonDirOutput");
                Tcc2DmSendData.StatInfo_PioLiaisonDirMsgAddr.LiaisonDirInput = GetPropertyValue<STAT_INFO_PIO_LIAISON_DIR_MSG_ADDR_ST>("状态信息-区间方向","LiaisonDirInput");
                Tcc2DmSendData.StatInfo_PioLiaisonDirMsgAddr.LiaisonDirAlarm = GetPropertyValue<STAT_INFO_PIO_LIAISON_DIR_MSG_ADDR_ST>("状态信息-区间方向","LiaisonDirAlarm");
                Tcc2DmSendData.StatInfo_PioLiaisonDirMsgAddr.Reserve = GetPropertyValue<STAT_INFO_PIO_LIAISON_DIR_MSG_ADDR_ST>("状态信息-区间方向","Reserve");
                //"状态信息-站内区段发码方向"
                Tcc2DmSendData.StatInfo_PioInStaDirMsgAddr.BlkDirOutput = GetPropertyValue<STAT_INFO_PIO_IN_STA_DIR_MSG_ADDR_ST>("状态信息-站内区段发码方向","BlkDirOutput");
                Tcc2DmSendData.StatInfo_PioInStaDirMsgAddr.BlkDirIntput = GetPropertyValue<STAT_INFO_PIO_IN_STA_DIR_MSG_ADDR_ST>("状态信息-站内区段发码方向", "BlkDirIntput");
                Tcc2DmSendData.StatInfo_PioInStaDirMsgAddr.Reserve = GetPropertyValue<STAT_INFO_PIO_IN_STA_DIR_MSG_ADDR_ST>("状态信息-站内区段发码方向", "Reserve");
                //"状态信息-灾害状态"
                Tcc2DmSendData.StatInfo_DisasterInfoMsgAddr.DisasterStat = GetPropertyValue<STAT_INFO_DISASTER_INFO_MSG_ADDR_ST>("状态信息-灾害状态","DisasterStat");
                //"状态信息-区间信号点灯状态"
                Tcc2DmSendData.StatInfo_PioSigStatMsgAddr.SigDisp = GetPropertyValue<STAT_INFO_PIO_SIG_STAT_MSG_ADDR_ST>("状态信息-区间信号点灯状态","SigDisp");
                //"状态信息-站内信号降级"
                Tcc2DmSendData.StatInfo_SigDegrateMsgAddr.SigDegrate = GetPropertyValue<STAT_INFO_SIG_DEGRATE_MSG_ADDR_ST>("状态信息-站内信号降级","SigDegrate");
                //"状态信息-区间方向口占用检查状态"
                Tcc2DmSendData.StatInfo_LiaisonLogicDetStatMsgAddr.LiaisonLogicDetStat = GetPropertyValue<STAT_INFO_LIAISON_LOGIC_DET_STAT_MSG_ADDR_ST>("状态信息-区间方向口占用检查状态", "LiaisonLogicDetStat");
                Tcc2DmSendData.StatInfo_LiaisonLogicDetStatMsgAddr.Reserve = GetPropertyValue<STAT_INFO_LIAISON_LOGIC_DET_STAT_MSG_ADDR_ST>("状态信息-区间方向口占用检查状态", "Reserve");
                //"Tcc设备维护信息"
                Tcc2DmSendData.DeviceAlarmMsgAddr.DeviceType = GetPropertyValue<DEVICE_ALARM_MSG_ADDR_ST>("Tcc设备维护信息","DeviceType");
                Tcc2DmSendData.DeviceAlarmMsgAddr.AlarmCode = GetPropertyValue<DEVICE_ALARM_MSG_ADDR_ST>("Tcc设备维护信息", "AlarmCode");
                Tcc2DmSendData.DeviceAlarmMsgAddr.Reserve = GetPropertyValue<DEVICE_ALARM_MSG_ADDR_ST>("Tcc设备维护信息", "Reserve");
                //"闭塞分区逻辑状态信息"
                Tcc2DmSendData.BsLogicDetStatMsgAddr.BsLogicStat = GetPropertyValue<BS_LOGIC_DET_STAT_MSG_ADDR_ST>("闭塞分区逻辑状态信息","BsLogicStat");
                //"区段SA状态信息"
                Tcc2DmSendData.SaMsgAddr.SaCode = GetPropertyValue<SA_MSG_ADDR_ST>("区段SA状态信息","SaCode");
                Tcc2DmSendData.SaMsgAddr.Reserve = GetPropertyValue<SA_MSG_ADDR_ST>("区段SA状态信息","Reserve");

                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// 获取对象的实例属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="blockName">数据块名称</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
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

            return GetCommAddress(blockid, packoffset, blockWidth, blockWidthOffset, infoWidth);
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


    public struct HW_STAT_MSG_ADDR_ST
    {
        [CommAddressAttribute("硬件状态")]
        public CommAddress Std75Code { get; set; }
    }

    public struct RT_FROM_CBI_MSG_ADDR_ST
    {
        [CommAddressAttribute("进路编号")]
        public CommAddress RtStd75Code { get; set; }

        [CommAddressAttribute("区段数目")]
        public CommAddress BlkNum { get; set; }

        [CommAddressAttribute("信号机状态")]
        public CommAddress SigStat { get; set; }

        [CommAddressAttribute("区段x状态")]
        public CommAddress[] BlkLockStat { get; set; }
    }

    public struct CHG_DIR_CMD_FROM_CBI_MSG_ADDR_ST
    {
        [CommAddressAttribute("线路编号")]
        public CommAddress LiaisonStd75Code { get; set; }

        [CommAddressAttribute("联锁Fsj信息")]
        public CommAddress Fsj { get; set; }

        [CommAddressAttribute("联锁Fqj信息")]
        public CommAddress Fqj { get; set; }

        [CommAddressAttribute("联锁Jfaj信息")]
        public CommAddress Jfaj { get; set; }

        [CommAddressAttribute("联锁Ffaj信息")]
        public CommAddress Ffaj { get; set; }
    }

    public struct CHG_DIR_RESP2CBI_MSG_ADDR_ST
    {
        [CommAddressAttribute("线路编号")]
        public CommAddress LiaisonStd75Code { get; set; }

        [CommAddressAttribute("联锁Yfj信息")]
        public CommAddress Yfj { get; set; }

        [CommAddressAttribute("区间占用状态")]
        public CommAddress SectStat { get; set; }

        [CommAddressAttribute("联锁Jqj信息")]
        public CommAddress Jqj { get; set; }

        [CommAddressAttribute("联锁Fj")]
        public CommAddress Fj { get; set; }

        [CommAddressAttribute("联锁辅助办理指示灯")]
        public CommAddress stAuxlLamp { get; set; }
    }

    public struct CHG_DIR_CMD_FROM_TCC_MSG_ADDR_ST
    {
        [CommAddressAttribute("线路编号")]
        public CommAddress LiaisonStd75Code { get; set; }

        [CommAddressAttribute("全区间状态")]
        public CommAddress SectStat { get; set; }

        [CommAddressAttribute("发车进路锁闭")]
        public CommAddress Fsj { get; set; }

        [CommAddressAttribute("当前方向状态")]
        public CommAddress DptOrRcvDir { get; set; }

        [CommAddressAttribute("请求和允许信息（改方命令）")]
        public CommAddress ChgDirCmd { get; set; }

        [CommAddressAttribute("改方操作（改方类型）")]
        public CommAddress ChgDirType { get; set; }
    }

    public struct CHG_DIR_RESP2TCC_MSG_ADDR_ST
    {
        [CommAddressAttribute("线路编号")]
        public CommAddress LiaisonStd75Code { get; set; }

        [CommAddressAttribute("全区间状态")]
        public CommAddress SectStat { get; set; }

        [CommAddressAttribute("发车进路锁闭")]
        public CommAddress Fsj { get; set; }

        [CommAddressAttribute("当前方向状态")]
        public CommAddress DptOrRcvDir { get; set; }

        [CommAddressAttribute("请求和允许信息（改方命令）")]
        public CommAddress ChgDirCmd { get; set; }

        [CommAddressAttribute("改方操作（改方类型）")]
        public CommAddress ChgDirType { get; set; }
    }

    public struct BRD_MSG_ADDR_ST
    {
        [CommAddressAttribute("线路边界编号")]
        public CommAddress BrdStd75Code { get; set; }

        [CommAddressAttribute("预留")]
        public CommAddress Reserve1 { get; set; }

        [CommAddressAttribute("边界区段低频")]
        public CommAddress LowFreq { get; set; }

        [CommAddressAttribute("边界区段预告标识")]
        public CommAddress LowFreqFlag { get; set; }

        [CommAddressAttribute("边界区段状态")]
        public CommAddress OccupyStat { get; set; }

        [CommAddressAttribute("预留")]
        public CommAddress[] Reserve2 { get; set; }
    }

    public struct SUBLIAISON_DIR2CBI_MSG_ADDR_ST
    {
        [CommAddressAttribute("小发车口编号")]
        public CommAddress SubLiaisonStd75Code { get; set; }

        [CommAddressAttribute("小发车口Yfj信息")]
        public CommAddress Yfj { get; set; }

        [CommAddressAttribute("小发车口区间占用状态")]
        public CommAddress SectStat { get; set; }

        [CommAddressAttribute("预留")]
        public CommAddress Reserve { get; set; }
    }

    public struct TSR_MSG_ADDR_ST
    {
        [CommAddressAttribute("限速速度值")]
        public CommAddress TsrSpeed { get; set; }

        [CommAddressAttribute("调度命令号-Part1")]
        public CommAddress CtcCmdCodePart1 { get; set; }

        [CommAddressAttribute("调度命令号-Part2")]
        public CommAddress CtcCmdCodePart2 { get; set; }

        [CommAddressAttribute("调度命令号-Part3")]
        public CommAddress CtcCmdCodePart3 { get; set; }

        [CommAddressAttribute("调度命令号-Part4")]
        public CommAddress CtcCmdCodePart4 { get; set; }

        [CommAddressAttribute("Tsrs/Ctc/Tcc编号")]
        public CommAddress CtcOrTccCode { get; set; }

        [CommAddressAttribute("用户编号")]
        public CommAddress UserId { get; set; }

        [CommAddressAttribute("限速编号")]
        public CommAddress TsrCode { get; set; }

        [CommAddressAttribute("线路号")]
        public CommAddress LineCode { get; set; }

        [CommAddressAttribute("限速原因")]
        public CommAddress TsrCause { get; set; }

        [CommAddressAttribute("起点里程系")]
        public CommAddress StartChaingeSys { get; set; }

        [CommAddressAttribute("起点里程")]
        public CommAddress StartChainge { get; set; }

        [CommAddressAttribute("终点里程系")]
        public CommAddress EndChaingeSys { get; set; }

        [CommAddressAttribute("终点里程")]
        public CommAddress EndChainge { get; set; }

        [CommAddressAttribute("预留")]
        public CommAddress Reserve1 { get; set; }

        [CommAddressAttribute("预留")]
        public CommAddress Reserve2 { get; set; }
    }

    public struct BLK_TC_CODE_MSG_ADDR_ST
    {
        [CommAddressAttribute("区段小轨道载频")]
        public CommAddress XgCarrFreq { get; set; }

        [CommAddressAttribute("区段主轨道载频")]
        public CommAddress ZgCarrFreq { get; set; }

        [CommAddressAttribute("区段主轨道低频")]
        public CommAddress ZgLowFreq { get; set; }

        [CommAddressAttribute("预留")]
        public CommAddress Reserve1 { get; set; }

        [CommAddressAttribute("区段小轨道低频")]
        public CommAddress XgLowFreq { get; set; }

        [CommAddressAttribute("预留")]
        public CommAddress Reserve2 { get; set; }
    }

    public struct STAT_INFO_PIO_BLK_STAT_MSG_ADDR_ST
    {
        [CommAddressAttribute("区段占用状态-继电器")]
        public CommAddress BlkOccupyStat_FromRelay { get; set; }

        [CommAddressAttribute("区段占用状态-通信")]
        public CommAddress BlkOccupyStat_FromCan { get; set; }
    }

    public struct STAT_INFO_PIO_LIAISON_DIR_MSG_ADDR_ST
    {
        [CommAddressAttribute("线路方向驱动")]
        public CommAddress LiaisonDirOutput { get; set; }

        [CommAddressAttribute("线路方向采集")]
        public CommAddress LiaisonDirInput { get; set; }

        [CommAddressAttribute("线路方向报警")]
        public CommAddress LiaisonDirAlarm { get; set; }

        [CommAddressAttribute("预留")]
        public CommAddress Reserve { get; set; }
    }

    public struct STAT_INFO_PIO_IN_STA_DIR_MSG_ADDR_ST
    {
        [CommAddressAttribute("站内区段方向驱动")]
        public CommAddress BlkDirOutput { get; set; }

        [CommAddressAttribute("站内区段方向采集")]
        public CommAddress BlkDirIntput { get; set; }

        [CommAddressAttribute("BlkDirOutput")]
        public CommAddress Reserve { get; set; }
    }

    public struct STAT_INFO_DISASTER_INFO_MSG_ADDR_ST
    {
        [CommAddressAttribute("灾害状态")]
        public CommAddress DisasterStat { get; set; }
    }

    public struct STAT_INFO_PIO_SIG_STAT_MSG_ADDR_ST
    {
        [CommAddressAttribute("区间信号机显示状态")]
        public CommAddress SigDisp { get; set; }
    }

    public struct STAT_INFO_SIG_DEGRATE_MSG_ADDR_ST
    {
        [CommAddressAttribute("区间信号机显示状态")]
        public CommAddress SigDegrate { get; set; }
    }

    public struct STAT_INFO_LIAISON_LOGIC_DET_STAT_MSG_ADDR_ST
    {
        [CommAddressAttribute("黄闪黄信号降级")]
        public CommAddress LiaisonLogicDetStat { get; set; }

        [CommAddressAttribute("预留")]
        public CommAddress Reserve { get; set; }
    }

    public struct DEVICE_ALARM_MSG_ADDR_ST
    {
        [CommAddressAttribute("报警设备类型标识")]
        public CommAddress DeviceType { get; set; }

        [CommAddressAttribute("故障描述编码")]
        public CommAddress AlarmCode { get; set; }

        [CommAddressAttribute("预留")]
        public CommAddress Reserve { get; set; }
    }

    public struct BS_LOGIC_DET_STAT_MSG_ADDR_ST
    {
        [CommAddressAttribute("闭塞分区逻辑状态")]
        public CommAddress BsLogicStat { get; set; }
    }

    public struct SA_MSG_ADDR_ST
    {
        [CommAddressAttribute("Sa编号")]
        public CommAddress SaCode { get; set; }

        [CommAddressAttribute("预留")]
        public CommAddress Reserve { get; set; }
    }

    public class Tcc2DmInfCfgData
    {
        public UInt32 IsConnExist;
        public DEVICE_ID SrcDeviceId;
        public DEVICE_ID DstDeviceId;
        public UInt32 MaxSilentCycle;
        public UInt32 CommInterval;
        public TCC_INF_VER TCC_INF_VER;
    }

    public class Tcc2DmSendData
    {
        public HW_STAT_MSG_ADDR_ST HwStatMsgAddr;
        public RT_FROM_CBI_MSG_ADDR_ST RtFromCbiMsgAddr;
        public CHG_DIR_CMD_FROM_CBI_MSG_ADDR_ST ChgDirCmdFromCbiMsgAddr;
        public CHG_DIR_RESP2CBI_MSG_ADDR_ST ChgDirResp2CbiMsgAddr;
        public CHG_DIR_CMD_FROM_TCC_MSG_ADDR_ST ChgDirCmdFromTccMsgAddr;
        public CHG_DIR_RESP2TCC_MSG_ADDR_ST ChgDirResp2TccMsgAddr;
        public BRD_MSG_ADDR_ST BrdMsgAddr;
        public SUBLIAISON_DIR2CBI_MSG_ADDR_ST SubLiaisonDir2CbiMsgAddr;
        public TSR_MSG_ADDR_ST TsrMsgAddr;
        public BLK_TC_CODE_MSG_ADDR_ST BlkTcCodeMsgAddr;
        public STAT_INFO_PIO_BLK_STAT_MSG_ADDR_ST StatInfo_PioBlkStatMsgAddr;
        public STAT_INFO_PIO_LIAISON_DIR_MSG_ADDR_ST StatInfo_PioLiaisonDirMsgAddr;
        public STAT_INFO_PIO_IN_STA_DIR_MSG_ADDR_ST StatInfo_PioInStaDirMsgAddr;
        public STAT_INFO_DISASTER_INFO_MSG_ADDR_ST StatInfo_DisasterInfoMsgAddr;
        public STAT_INFO_PIO_SIG_STAT_MSG_ADDR_ST StatInfo_PioSigStatMsgAddr;
        public STAT_INFO_SIG_DEGRATE_MSG_ADDR_ST StatInfo_SigDegrateMsgAddr;
        public STAT_INFO_LIAISON_LOGIC_DET_STAT_MSG_ADDR_ST StatInfo_LiaisonLogicDetStatMsgAddr;
        public DEVICE_ALARM_MSG_ADDR_ST DeviceAlarmMsgAddr;
        public BS_LOGIC_DET_STAT_MSG_ADDR_ST BsLogicDetStatMsgAddr;
        public SA_MSG_ADDR_ST SaMsgAddr;
    }
}
