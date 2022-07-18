using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC_TCC.A.ETBModel;
using TCC_TCC.A.ETBModel.BinaryModel.BinaryStruct;
using TCC_TCC.A.ETBProcess;

namespace TCC_TCC.A.ETBUtil
{
    /// <summary>
    ///      ToBytes扩展方法
    /// </summary>
    public static class ETBExtensionToBytes
    {
        public static IEnumerable<Byte> ToBytes(this Byte input)
        {
            return new byte[] { input };//应直接返回原值
        }
        public static IEnumerable<Byte> ToBytes(this Byte[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this UInt16 input)
        {
            if (!ExcelToDatProcess.IsBigEndian)
            {
                return BitConverter.GetBytes(input);
            }

            return BitConverter.GetBytes(input).ConvertToBigEndian();
        }

        public static IEnumerable<Byte> ToBytes(this UInt16[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }
        public static IEnumerable<Byte> ToBytes(this UInt32 input)
        {
            if (!ExcelToDatProcess.IsBigEndian)
            {
                return BitConverter.GetBytes(input);
            }

            return BitConverter.GetBytes(input).ConvertToBigEndian();
        }

        public static IEnumerable<Byte> ToBytes(this TCC_INF_VER input)
        {
            return ToBytes(input.SpecVer).AddRangeList(ToBytes(input.DataVer));
        }

        public static IEnumerable<Byte> ToBytes(this UInt32[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this UInt64 input)
        {
            if (!ExcelToDatProcess.IsBigEndian)
            {
                return BitConverter.GetBytes(input);
            }

            return BitConverter.GetBytes(input).ConvertToBigEndian();
        }

        public static IEnumerable<Byte> ToBytes(this CIOAddress input)
        {
            return ToBytes(input.ByteOffset).AddRangeList(ToBytes(input.BitOffset));
        }
        public static IEnumerable<Byte> ToBytes(this CIOAddress[] inputs)
        {
            return ConcatCollections(inputs.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this DataBlockAddress input)
        {
            return ToBytes(input.ByteOffset).AddRangeList(ToBytes(input.ByteLen)).AddRangeList(ToBytes(input.DevNum));
        }
        public static IEnumerable<Byte> ToBytes(this CommAddress input)
        {
            return ToBytes(input.ByteOffset).AddRangeList(ToBytes(input.BitOffset)).AddRangeList(ToBytes(input.Width));
        }
        public static IEnumerable<Byte> ToBytes(this CommAddress[] inputs)
        {
            return ConcatCollections(inputs.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this STA_PSD_MSG_ADDR_ST[] inputs)
        {
            return ConcatCollections(inputs.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this STA_PSD_MSG_ADDR_ST input)
        {
            return ToBytes(input.Id).AddRangeList(ToBytes(input.DoorLockStat)).AddRangeList(ToBytes(input.DoorAlarmRelay)).AddRangeList(ToBytes(input.DoorBypassRelay)).AddRangeList(ToBytes(input.DoorProtectRelay));
        }

        public static IEnumerable<Byte> ToBytes(this SYS_CONN_STAT_MSG_ADDR_ST[] inputs)
        {
            return ConcatCollections(inputs.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this SYS_CONN_STAT_MSG_ADDR_ST input)
        {
            return ToBytes(input.TccCode).AddRangeList(ToBytes(input.Tcc2CbiConnStat)).AddRangeList(ToBytes(input.Tcc2CtcConnStat)).AddRangeList(ToBytes(input.MasterOrSlave)).
                    AddRangeList(ToBytes(input.SingleOrDualSys)).AddRangeList(ToBytes(input.Tcc2TccConnStat)).AddRangeList(ToBytes(input.Tcc2TsrsConnStat)).
                    AddRangeList(ToBytes(input.MainStaConnStat)).AddRangeList(ToBytes(input.BlkSectInitStat));
        }

        public static IEnumerable<Byte> ToBytes(this PKT_DESC_MSG_ADDR_TCC_ST input)
        {
            return ToBytes(input.PktNum).AddRangeList(ToBytes(input.PktIndex));
        }

        public static IEnumerable<Byte> ToBytes(this PKT_DESC_MSG_ADDR_CTC_ST input)
        {
            return ToBytes(input.PktNum).AddRangeList(ToBytes(input.PktIndex));
        }

        public static IEnumerable<Byte> ToBytes(this BS_CFM_CMD_MSG_ADDR_ST input)
        {
            return ToBytes(input.TccCode).AddRangeList(input.CmdValue.ToBytes()).AddRangeList(input.CmdType.ToBytes()).AddRangeList(
                input.BsOrLiaisonCode.ToBytes()).AddRangeList(input.CtcCode.ToBytes()).AddRangeList(input.Crc1.ToBytes()).AddRangeList(input.Crc2.ToBytes());
        }

        public static IEnumerable<Byte> ToBytes(this INF_VER_MSG_ADDR_ST input)
        {
            return ToBytes(input.InfSpecVer).AddRangeList(input.InfDataVer.ToBytes());
        }

        public static IEnumerable<Byte> ToBytes(this MyCtcMsgDeviceCtrlAreaStruct input)
        {
            return ToBytes(input.StartStd75Code).AddRangeList(ToBytes(input.EndStd75Code));
        }

        public static IEnumerable<Byte> ToBytes(this HW_STAT_MSG_ADDR_ST input)
        {
            return ToBytes(input.Std75Code);
        }

        public static IEnumerable<Byte> ToBytes(this RT_FROM_CBI_MSG_ADDR_ST input)
        {
            return ToBytes(input.RtStd75Code).AddRangeList(ToBytes(input.BlkNum)).AddRangeList(ToBytes(input.SigStat)).AddRangeList(ToBytes(input.BlkLockStat));
        }

        public static IEnumerable<Byte> ToBytes(this CHG_DIR_CMD_FROM_CBI_MSG_ADDR_ST input)
        {
            return ToBytes(input.LiaisonStd75Code).AddRangeList(ToBytes(input.Fsj)).AddRangeList(ToBytes(input.Fqj)).AddRangeList(ToBytes(input.Jfaj)).
                AddRangeList(ToBytes(input.Ffaj));
        }

        public static IEnumerable<Byte> ToBytes(this CHG_DIR_RESP2CBI_MSG_ADDR_ST input)
        {
            return ToBytes(input.LiaisonStd75Code).AddRangeList(ToBytes(input.Yfj)).AddRangeList(ToBytes(input.SectStat)).AddRangeList(ToBytes(input.Jqj)).
                AddRangeList(ToBytes(input.Fj)).AddRangeList(ToBytes(input.stAuxlLamp));
        }

        public static IEnumerable<Byte> ToBytes(this CHG_DIR_CMD_FROM_TCC_MSG_ADDR_ST input)
        {
            return ToBytes(input.LiaisonStd75Code).AddRangeList(ToBytes(input.SectStat)).AddRangeList(ToBytes(input.Fsj)).AddRangeList(ToBytes(input.DptOrRcvDir)).
                AddRangeList(ToBytes(input.ChgDirCmd)).AddRangeList(ToBytes(input.ChgDirType));
        }

        public static IEnumerable<Byte> ToBytes(this CHG_DIR_RESP2TCC_MSG_ADDR_ST input)
        {
            return ToBytes(input.LiaisonStd75Code).AddRangeList(ToBytes(input.SectStat)).AddRangeList(ToBytes(input.Fsj)).AddRangeList(ToBytes(input.DptOrRcvDir)).
                AddRangeList(ToBytes(input.ChgDirCmd)).AddRangeList(ToBytes(input.ChgDirType));
        }

        public static IEnumerable<Byte> ToBytes(this BRD_MSG_ADDR_ST input)
        {
            return ToBytes(input.BrdStd75Code).AddRangeList(ToBytes(input.Reserve1)).AddRangeList(ToBytes(input.LowFreq)).AddRangeList(ToBytes(input.LowFreqFlag)).
                AddRangeList(ToBytes(input.OccupyStat)).AddRangeList(ToBytes(input.Reserve2));
        }

        public static IEnumerable<Byte> ToBytes(this SUBLIAISON_DIR2CBI_MSG_ADDR_ST input)
        {
            return ToBytes(input.SubLiaisonStd75Code).AddRangeList(ToBytes(input.Yfj)).AddRangeList(ToBytes(input.SectStat)).AddRangeList(ToBytes(input.Reserve));
        }

        public static IEnumerable<Byte> ToBytes(this TSR_MSG_ADDR_ST input)
        {
            return ToBytes(input.TsrSpeed).AddRangeList(ToBytes(input.CtcCmdCodePart1)).AddRangeList(ToBytes(input.CtcCmdCodePart2)).AddRangeList(ToBytes(input.CtcCmdCodePart3)).
                AddRangeList(ToBytes(input.CtcCmdCodePart4)).AddRangeList(ToBytes(input.CtcOrTccCode)).AddRangeList(ToBytes(input.UserId)).AddRangeList(ToBytes(input.TsrCode)).
                AddRangeList(ToBytes(input.LineCode)).AddRangeList(ToBytes(input.TsrCause)).AddRangeList(ToBytes(input.StartChaingeSys)).AddRangeList(ToBytes(input.StartChainge)).
                AddRangeList(ToBytes(input.EndChaingeSys)).AddRangeList(ToBytes(input.EndChainge)).AddRangeList(ToBytes(input.Reserve1)).AddRangeList(ToBytes(input.Reserve2));
        }

        public static IEnumerable<Byte> ToBytes(this BLK_TC_CODE_MSG_ADDR_ST input)
        {
            return ToBytes(input.XgCarrFreq).AddRangeList(ToBytes(input.ZgCarrFreq)).AddRangeList(ToBytes(input.ZgLowFreq)).AddRangeList(ToBytes(input.Reserve1)).AddRangeList(ToBytes(input.XgLowFreq)).AddRangeList(ToBytes(input.Reserve2));
        }

        public static IEnumerable<Byte> ToBytes(this STAT_INFO_PIO_BLK_STAT_MSG_ADDR_ST input)
        {
            return ToBytes(input.BlkOccupyStat_FromRelay).AddRangeList(ToBytes(input.BlkOccupyStat_FromCan));
        }

        public static IEnumerable<Byte> ToBytes(this STAT_INFO_PIO_LIAISON_DIR_MSG_ADDR_ST input)
        {
            return ToBytes(input.LiaisonDirOutput).AddRangeList(ToBytes(input.LiaisonDirInput)).AddRangeList(ToBytes(input.LiaisonDirAlarm)).AddRangeList(ToBytes(input.Reserve));
        }

        public static IEnumerable<Byte> ToBytes(this STAT_INFO_PIO_IN_STA_DIR_MSG_ADDR_ST input)
        {
            return ToBytes(input.BlkDirOutput).AddRangeList(ToBytes(input.BlkDirIntput)).AddRangeList(ToBytes(input.Reserve));
        }

        public static IEnumerable<Byte> ToBytes(this STAT_INFO_DISASTER_INFO_MSG_ADDR_ST input)
        {
            return ToBytes(input.DisasterStat);
        }

        public static IEnumerable<Byte> ToBytes(this STAT_INFO_PIO_SIG_STAT_MSG_ADDR_ST input)
        {
            return ToBytes(input.SigDisp);
        }

        public static IEnumerable<Byte> ToBytes(this STAT_INFO_SIG_DEGRATE_MSG_ADDR_ST input)
        {
            return ToBytes(input.SigDegrate);
        }

        public static IEnumerable<Byte> ToBytes(this STAT_INFO_LIAISON_LOGIC_DET_STAT_MSG_ADDR_ST input)
        {
            return ToBytes(input.LiaisonLogicDetStat).AddRangeList(ToBytes(input.Reserve));
        }

        public static IEnumerable<Byte> ToBytes(this DEVICE_ALARM_MSG_ADDR_ST input)
        {
            return ToBytes(input.DeviceType).AddRangeList(ToBytes(input.AlarmCode)).AddRangeList(ToBytes(input.Reserve));
        }

        public static IEnumerable<Byte> ToBytes(this BS_LOGIC_DET_STAT_MSG_ADDR_ST input)
        {
            return ToBytes(input.BsLogicStat);
        }

        public static IEnumerable<Byte> ToBytes(this SA_MSG_ADDR_ST input)
        {
            return ToBytes(input.SaCode).AddRangeList(ToBytes(input.Reserve));
        }

        public static IEnumerable<Byte> ToBytes(this TCC_CABINET_ACRI_ST input)
        {
            return ToBytes(input.Acr1_CanACpu1).AddRangeList(input.Acr1_CanACpu2.ToBytes()).AddRangeList(input.Acr1_CanBCpu1.ToBytes()).AddRangeList(input.Acr1_CanBCpu2.ToBytes());
        }

        public static IEnumerable<Byte> ToBytes(this TCC_CABINET_ACRI_ST[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this BLK_CODE_MSG_ADDR_ST input)
        {
            return ToBytes(input.Id).AddRangeList(input.XgCarrFreq.ToBytes()).AddRangeList(input.ZgCarrFreq.ToBytes()).
                AddRangeList(input.ZgLowFreq.ToBytes()).AddRangeList(input.Dir.ToBytes()).AddRangeList(input.XgLowFreq.ToBytes());
        }

        public static IEnumerable<Byte> ToBytes(this BLK_CODE_MSG_ADDR_ST[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this BLK_STAT_MSG_ADDR_ST input)
        {
            return ToBytes(input.Id).AddRangeList(input.ZgStatMain.ToBytes()).AddRangeList(input.XgStatMain.ToBytes()).AddRangeList(input.Dir.ToBytes()).
                AddRangeList(input.ZgStatParallel.ToBytes()).AddRangeList(input.XgStatParallel.ToBytes());
        }

        public static IEnumerable<Byte> ToBytes(this BLK_STAT_MSG_ADDR_ST[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this LIAISON_DIR_MSG_ADDR_ST input)
        {
            return ToBytes(input.Id).AddRangeList(input.LogicDetStat.ToBytes()).AddRangeList(input.Qjzt.ToBytes()).AddRangeList(input.Jqj.ToBytes()).AddRangeList(input.Fj.ToBytes());
        }

        public static IEnumerable<Byte> ToBytes(this LIAISON_DIR_MSG_ADDR_ST[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this STA_SIG_MSG_ADDR_ST input)
        {
            return ToBytes(input.Id).AddRangeList(input.SigStat.ToBytes());
        }

        public static IEnumerable<Byte> ToBytes(this STA_SIG_MSG_ADDR_ST[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this STA_BS_MSG_ADDR_ST input)
        {
            return ToBytes(input.Id).AddRangeList(input.BsLowFreq.ToBytes()).AddRangeList(input.BsStat.ToBytes());
        }

        public static IEnumerable<Byte> ToBytes(this STA_BS_MSG_ADDR_ST[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this BS_CFM_CMD_RESP_MSG_ADDR_ST input)
        {
            return ToBytes(input.TccCode).AddRangeList(input.CmdValue.ToBytes()).AddRangeList(input.CmdType.ToBytes()).AddRangeList(input.BsOrLiaisonCode.ToBytes()).AddRangeList(
                input.CtcCode.ToBytes()).AddRangeList(input.ExeclResult.ToBytes()).AddRangeList(input.FltCause.ToBytes()).AddRangeList(input.Crc1.ToBytes()).AddRangeList(input.Crc2.ToBytes());
        }

        public static IEnumerable<Byte> ToBytes(this MyCtcMsgDeviceCtrlAreaStruct[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }



        public static IEnumerable<Byte> ToBytes(this PSD_CMD_MSG_ADDR_ST input)
        {
            return ToBytes(input.PsdCmd);
        }

        public static IEnumerable<Byte> ToBytes(this PSD_CMD_MSG_ADDR_ST[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this BS_STAT_MSG_ADDR_ST input)
        {
            return ToBytes(input.Id).AddRangeList(input.LowFreq.ToBytes()).AddRangeList(input.LoseDetStat.ToBytes()).AddRangeList(input.OccupyStat.ToBytes());
        }

        public static IEnumerable<Byte> ToBytes(this BS_STAT_MSG_ADDR_ST[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this LOSE_DET_MSG_ADDR_ST input)
        {
            return ToBytes(input.Id).AddRangeList(input.LoseDetStat.ToBytes());
        }

        public static IEnumerable<Byte> ToBytes(this LOSE_DET_MSG_ADDR_ST[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this BALISE_DIR_MSG_ADDR_ST input)
        {
            return ToBytes(input.Id).AddRangeList(ToBytes(input.BaliseDir));
        }

        public static IEnumerable<Byte> ToBytes(this BALISE_DIR_MSG_ADDR_ST[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this PSD_STAT_MSG_ADDR_ST input)
        {
            return ToBytes(input.Id).AddRangeList(ToBytes(input.PsdStat));
        }

        public static IEnumerable<Byte> ToBytes(this PSD_STAT_MSG_ADDR_ST[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this DEVICE_ID input)
        {
            return ToBytes(input.Id).AddRangeList(ToBytes(input.Id));
        }
        public static IEnumerable<Byte> ToBytes(this DEVICE_ID[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this RouteSwitch input)
        {
            return ToBytes(input.Switch_ID).AddRangeList(ToBytes(input.Positon));
        }
        public static IEnumerable<Byte> ToBytes(this RouteSwitch[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this FoulingBlock input)
        {
            return input.Block_ID.ToBytes().AddRangeList( ToBytes(input.RouteSwitches));
        }
        public static IEnumerable<Byte> ToBytes(this FoulingBlock[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this ConflictSignal input)
        {
            return ToBytes(input.Conflictsignal).AddRangeList(input.ConflictType.ToBytes()).AddRangeList(ToBytes(input.ConditionSwitch));
        }
        public static IEnumerable<Byte> ToBytes(this ConflictSignal[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this Path input)
        {
            return input.Block_ID.ToBytes().AddRangeList(input.CondSwitch.ToBytes());
        }
        public static IEnumerable<Byte> ToBytes(this Path[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this LOGIC_FACTOR input)
        {
            var bytes = input.bAvailable.ToBytes().AddRangeList(input.Type.ToBytes()).AddRangeList(input.DeviceID.ToBytes());
            bytes = bytes.AddRangeList(input.Condition.ToBytes()).AddRangeList(input.LogicalOperationType.ToBytes());
            bytes = bytes.AddRangeList(input.LeftLeaf.ToBytes()).AddRangeList(input.RightLeaf.ToBytes());
            return bytes;
        }
        public static IEnumerable<Byte> ToBytes(this LOGIC_FACTOR[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this OTHER_LOGIC input)
        {
            return input.Type.ToBytes().AddRangeList(input.Id.ToBytes());
        }
        public static IEnumerable<Byte> ToBytes(this OTHER_LOGIC[] input)
        {
            return ConcatCollections(input.Select(ToBytes));
        }

        public static IEnumerable<Byte> ToBytes(this string input)
        {
            return StringToList(input);
        }
        public static IEnumerable<Byte> ToBytes(this WaysideInfo input)
        {
            return StringToList(input.SegmentName).AddRangeList(input.StructWidth.ToBytes()).AddRangeList(input.TotalStruct.ToBytes());
        }
        public static IEnumerable<Byte> ConcatCollections(IEnumerable<IEnumerable<Byte>> souce)
        {
            var b = new List<Byte>();
            souce.Foreach(b.AddRange);
            return b;
        }

      

        /// <summary>
        /// 字符串转换为字节数据，以gb2312编码方式转换
        /// </summary>
        /// <param name="segmentName">要转换的字符串</param>
        /// <returns>字节流数据</returns>
        public static List<Byte> StringToList( String segmentName)
        {
            if (segmentName == null)
            {
                throw new ArgumentNullException("参数segmentName不能为空。");
            }
            var result = new List<Byte>(32);
            var b = Encoding.GetEncoding("gb2312").GetBytes(segmentName.Trim()).ToList();
           
            var arrayCount = 32 - b.Count;
            if (b.Count < 32)
            {
                for (var i = 0; i < arrayCount; i++)
                {
                    b.Add(0);
                }
                result = b;
            }
            else if (b.Count >= 32)
            {
                for (var i = 0; i < 32; i++)
                {
                    result.Add(b[i]);
                }
            }
            return result;
        }
    }
}
