using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCC_TCC.A.ETBModel;
using TCC_TCC.A.ETBModel.BinaryModel.BinaryStruct;
using TCC_TCC.A.ETBUtil;

namespace TCC_TCC.A.ETBProcess
{
    class ConvertToByte
    {
        public static UInt32 BOM = 0x12345678;
        public static UInt32 RandomNumberForPLT = 0xFFFFFFFF;

        public static IEnumerable<byte> ToBytes(DATStruct datStruct)
        {
            var bytes = new List<byte>();
            bytes.AddRange(BOM.ToBytes());

            bytes.AddRange(ReturnBytes(datStruct));

            var by = ETBExtensionMethods.PeekBytes(bytes, bytes.Count());
            var CRC1ILC = ETBExtensionMethods.CalWtpCRC(by.ToArray(), 0, by.Count());
            bytes.AddRange(CRC1ILC.ToBytes());

            by = ETBExtensionMethods.PeekBytes(bytes, bytes.Count());
            var CRC2ILC = ETBExtensionMethods.CalEthCRC(by.ToArray(), 0, by.Count());
            bytes.AddRange(CRC2ILC.ToBytes());
            bytes.AddRange(ToBytes(datStruct.Head_Tail_Struct));

            var lenForIlc = (UInt32)bytes.Count();
            var lenForVop = lenForIlc + 4 * 4;

            var ByteBuffer = new List<byte>();
            ByteBuffer.AddRange(lenForVop.ToBytes());
            for (int i = 0; i < 4; i++)
            {
                ByteBuffer.AddRange(lenForIlc.ToBytes());
            }
            ByteBuffer.AddRange(bytes);
            ByteBuffer.AddRange(RandomNumberForPLT.ToBytes());
            by = ETBExtensionMethods.PeekBytes(ByteBuffer, ByteBuffer.Count());
            var CRCVOP = ETBExtensionMethods.CalPlatCRC(by.ToArray(), by.Count());
            ByteBuffer.AddRange(CRCVOP.ToBytes());

            return ByteBuffer;
        }

        private static IEnumerable<byte> ReturnBytes(DATStruct datStruct)
        {
            MsgHelper.Instance(1, string.Format("==============================正在输出字节============================"));
            var returnBytes = new List<byte>();

            returnBytes.AddRange(ToBytes(datStruct.Head_Tail_Struct));
            returnBytes.AddRange(ToBytes(datStruct.CommTccStruct.InterfaceInfo));
            foreach (var data in datStruct.CommTccStruct.TccStrcutDatas)
            {
                returnBytes.AddRange(ToBytes(data.Parameter));
                returnBytes.AddRange(ToBytes(data.ReceiveData));
                returnBytes.AddRange(ToBytes(data.SendData));
            }

            returnBytes.AddRange(ToBytes(datStruct.CommTcc2CbiStruct.Parameter));
            returnBytes.AddRange(ToBytes(datStruct.CommTcc2CbiStruct.CbiTccData));
            returnBytes.AddRange(ToBytes(datStruct.CommTcc2CbiStruct.TccCbiData));

            returnBytes.AddRange(ToBytes(datStruct.CommTcc2ZpwStruct.InterfaceInfo));
            returnBytes.AddRange(ToBytes(datStruct.CommTcc2ZpwStruct.Tcc2ZpwInfCfgData));
            returnBytes.AddRange(ToBytes(datStruct.CommTcc2ZpwStruct.Tcc2ZpwReceiveData));
            returnBytes.AddRange(ToBytes(datStruct.CommTcc2ZpwStruct.Tcc2ZpwSendData));

            returnBytes.AddRange(ToBytes(datStruct.CommTcc2CtcStruct.InterfaceInfo));
            returnBytes.AddRange(ToBytes(datStruct.CommTcc2CtcStruct.Tcc2CtcInfCfgData));
            returnBytes.AddRange(ToBytes(datStruct.CommTcc2CtcStruct.Tcc2CtcReceiveData));
            returnBytes.AddRange(ToBytes(datStruct.CommTcc2CtcStruct.Tcc2CtcSendData));

            returnBytes.AddRange(ToBytes(datStruct.CommTcc2DmStruct.InterfaceInfo));
            returnBytes.AddRange(ToBytes(datStruct.CommTcc2DmStruct.Tcc2DmInfCfgData));
            returnBytes.AddRange(ToBytes(datStruct.CommTcc2DmStruct.Tcc2DmSendData));

            returnBytes.AddRange(ToBytes(datStruct.CommTcc2TsrsStruct.InterfaceInfo));
            returnBytes.AddRange(ToBytes(datStruct.CommTcc2TsrsStruct.Tcc2TsrsInfCfgData));
            returnBytes.AddRange(ToBytes(datStruct.CommTcc2TsrsStruct.Tcc2TsrsReceiveData));
            returnBytes.AddRange(ToBytes(datStruct.CommTcc2TsrsStruct.Tcc2TsrsSendData));

            return returnBytes;
        }

        private static IEnumerable<byte> ToBytes(Head_Tail_Struct head_Tail_Struct)
        {
            var bytes = new List<byte>();

            bytes.AddRange(head_Tail_Struct.GenerateDate.ToBytes());
            bytes.AddRange(head_Tail_Struct.MajorVersion.ToBytes());
            bytes.AddRange(head_Tail_Struct.MinorVersion.ToBytes());
            bytes.AddRange(head_Tail_Struct.DataVersion.ToBytes());
            bytes.AddRange(head_Tail_Struct.StationName.ToBytes());

            return bytes;
        }

        #region CommTccStruct
        private static IEnumerable<byte> ToBytes(CommTccParameter Parameter)
        {
            var bytes = new List<byte>();

            bytes.AddRange(Parameter.ullID.ToBytes());
            bytes.AddRange(Parameter.usMaxSilentCyclesTcc.ToBytes());
            bytes.AddRange(Parameter.usMaxTsnSkip.ToBytes());
            bytes.AddRange(Parameter.usMaxRsnSkip.ToBytes());
            bytes.AddRange(Parameter.usMaxSyncInteval.ToBytes());
            bytes.AddRange(Parameter.usMinSyncInteval.ToBytes());
            bytes.AddRange(Parameter.usTccCycleTime.ToBytes());
            bytes.AddRange(Parameter.usInitializedTime.ToBytes());
            bytes.AddRange(Parameter.usReceiveXLBJNum.ToBytes());
            bytes.AddRange(Parameter.usReceiveXLGFNum.ToBytes());
            bytes.AddRange(Parameter.usReceiveZJZXHNum.ToBytes());
            bytes.AddRange(Parameter.usReceiveZJZBSFQNum.ToBytes());
            bytes.AddRange(Parameter.usReceiveZJZYWQXNum.ToBytes());
            bytes.AddRange(Parameter.usReceiveXTLEUNum.ToBytes());
            bytes.AddRange(Parameter.usReceiveWPXXQJNum.ToBytes());
            bytes.AddRange(Parameter.usReceiveXHXKNum.ToBytes());
            bytes.AddRange(Parameter.usReceiveQJBSNum.ToBytes());
            bytes.AddRange(Parameter.usReceiveOtherNum.ToBytes());
            bytes.AddRange(Parameter.usReceiveRMNum.ToBytes());
            bytes.AddRange(Parameter.usReceiveInterfaceNum.ToBytes());
            bytes.AddRange(Parameter.usSendXLBJNum.ToBytes());
            bytes.AddRange(Parameter.usSendXLGFNum.ToBytes());
            bytes.AddRange(Parameter.usSendZJZXHNum.ToBytes());
            bytes.AddRange(Parameter.usSendZJZBSFQNum.ToBytes());
            bytes.AddRange(Parameter.usSendZJZYWQXNum.ToBytes());
            bytes.AddRange(Parameter.usSendXTLEUNum.ToBytes());
            bytes.AddRange(Parameter.usSendWPXXQJNum.ToBytes());
            bytes.AddRange(Parameter.usSendXHXKNum.ToBytes());
            bytes.AddRange(Parameter.usSendQJBSNum.ToBytes());
            bytes.AddRange(Parameter.usSendOtherNum.ToBytes());
            bytes.AddRange(Parameter.usSendRMNum.ToBytes());
            bytes.AddRange(Parameter.usSendInterfaceNum.ToBytes());

            return bytes;
        }

        private static IEnumerable<byte> ToBytes(CommTccStructData Data)
        {
            var bytes = new List<byte>();

            bytes.AddRange(Data.XLBJMsgDB.ToBytes());
            for (int i = 0; i < 16U; i++)
            {
                bytes.AddRange(Data.XLBJMsg[i].ullId.ToBytes());
                bytes.AddRange(Data.XLBJMsg[i].BSFQ_YL0.ToBytes());
                bytes.AddRange(Data.XLBJMsg[i].BSFQ_stu.ToBytes());
                bytes.AddRange(Data.XLBJMsg[i].BSFQDP_flag.ToBytes());
                bytes.AddRange(Data.XLBJMsg[i].BSFQDP_code.ToBytes());
                bytes.AddRange(Data.XLBJMsg[i].BSFQDP_attribute.ToBytes());
                bytes.AddRange(Data.XLBJMsg[i].BSFQ_YL1.ToBytes());
                bytes.AddRange(Data.XLBJMsg[i].FHXH1.ToBytes());
                bytes.AddRange(Data.XLBJMsg[i].FHXH2.ToBytes());
                bytes.AddRange(Data.XLBJMsg[i].FHXH3.ToBytes());
                bytes.AddRange(Data.XLBJMsg[i].BSFQ_YL2.ToBytes());
                bytes.AddRange(Data.XLBJMsg[i].BSFQ_YL3.ToBytes());
            }

            bytes.AddRange(Data.XLGFMsgDB.ToBytes());
            for (int i = 0; i < 16; i++)
            {
                bytes.AddRange(Data.XLGFMsg[i].ullId.ToBytes());
                bytes.AddRange(Data.XLGFMsg[i].BZQJ_stu.ToBytes());
                bytes.AddRange(Data.XLGFMsg[i].XLJL_flag.ToBytes());
                bytes.AddRange(Data.XLGFMsg[i].XLJLSB.ToBytes());
                bytes.AddRange(Data.XLGFMsg[i].XLFX_msg.ToBytes());
                bytes.AddRange(Data.XLGFMsg[i].XLGF_cmd.ToBytes());
                bytes.AddRange(Data.XLGFMsg[i].XLGF_type.ToBytes());
            }

            bytes.AddRange(Data.ZJZ_XHDB.ToBytes());
            for (int i = 0; i < 60; i++)
            {
                bytes.AddRange(Data.ZJZ_XH[i].ullId.ToBytes());
                bytes.AddRange(Data.ZJZ_XH[i].ZJXH.ToBytes());
            }

            bytes.AddRange(Data.ZJZ_BSFQDB.ToBytes());
            for (int i = 0; i < 60; i++)
            {
                bytes.AddRange(Data.ZJZ_BSFQ[i].ullId.ToBytes());
                bytes.AddRange(Data.ZJZ_BSFQ[i].BSFQ_DP.ToBytes());
                bytes.AddRange(Data.ZJZ_BSFQ[i].BSFQ_ZT.ToBytes());
            }

            bytes.AddRange(Data.ZJZ_YWQXDB.ToBytes());
            for (int i = 0; i < 40; i++)
            {
                bytes.AddRange(Data.ZJZ_YWQX[i].ullId.ToBytes());
                bytes.AddRange(Data.ZJZ_YWQX[i].ZJZ_YWQX.ToBytes());
            }

            bytes.AddRange(Data.XTLeuMsgDB.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].ullId.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].COMtoCBI.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].COMtoCTC.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].ZJwork_stu.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].DSJwork_stu.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].COMtoTCC.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].COMtoTSRS.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].COMtoMain.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].BSFQ_initial.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU1_TP1.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU1_TP2.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU1_TP3.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU1_TP4.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU1_TD1.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU1_TD2.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU1CFG_avlble.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU2_TP1.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU2_TP2.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU2_TP3.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU2_TP4.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU2_TD1.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU2_TD2.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU2CFG_avlble.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU3_TP1.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU3_TP2.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU3_TP3.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU3_TP4.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU3_TD1.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU3_TD2.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU3CFG_avlble.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU4_TP1.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU4_TP2.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU4_TP3.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU4_TP4.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU4_TD1.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU4_TD2.ToBytes());
            bytes.AddRange(Data.XTLeuMsg[0].LEU4CFG_avlble.ToBytes());

            bytes.AddRange(Data.WPX_XQJDB.ToBytes());
            for (int i = 0; i < 16; i++)
            {
                bytes.AddRange(Data.WPX_XQJ[i].ullId.ToBytes());
                bytes.AddRange(Data.WPX_XQJ[i].XQJ_stu.ToBytes());
            }

            bytes.AddRange(Data.XHXKDB.ToBytes());
            for (int i = 0; i < 16; i++)
            {
                bytes.AddRange(Data.XHXK[i].ullId.ToBytes());
                bytes.AddRange(Data.XHXK[i].XHXK_ID.ToBytes());
                bytes.AddRange(Data.XHXK[i].XHXK_ZT.ToBytes());
                bytes.AddRange(Data.XHXK[i].XHXK_LX.ToBytes());
                bytes.AddRange(Data.XHXK[i].XHXK_ZY.ToBytes());
                bytes.AddRange(Data.XHXK[i].XHXK_YL.ToBytes());
            }

            bytes.AddRange(Data.QJBSDB.ToBytes());
            bytes.AddRange(Data.QJBS[0].ullId.ToBytes());
            bytes.AddRange(Data.QJBS[0].TCC_ID.ToBytes());
            bytes.AddRange(Data.QJBS[0].QRZT.ToBytes());
            bytes.AddRange(Data.QJBS[0].CMD_type.ToBytes());
            bytes.AddRange(Data.QJBS[0].QJK_ID.ToBytes());
            bytes.AddRange(Data.QJBS[0].CTC_ID.ToBytes());
            bytes.AddRange(Data.QJBS[0].ZJZ_CMDJG.ToBytes());
            bytes.AddRange(Data.QJBS[0].ZJZ_CMDYY.ToBytes());

            bytes.AddRange(Data.OtherMsgDB.ToBytes());
            for (int i = 0; i < 8; i++)
            {
                bytes.AddRange(Data.OtherMsg[i].ullId.ToBytes());
                bytes.AddRange(Data.OtherMsg[i].XH_KDJ.ToBytes());
                bytes.AddRange(Data.OtherMsg[i].XH_LXJ.ToBytes());
                bytes.AddRange(Data.OtherMsg[i].XH_YXJ.ToBytes());
                bytes.AddRange(Data.OtherMsg[i].XH_TXJ.ToBytes());
            }
            bytes.AddRange(Data.RMDB.ToBytes());
            for (int i = 0; i < 4; i++)
            {
                bytes.AddRange(Data.RM[i].ullId.ToBytes());
                bytes.AddRange(Data.RM[i].RM_ID.ToBytes());
                bytes.AddRange(Data.RM[i].RM_SIGS.ToBytes());
                bytes.AddRange(Data.RM[i].RM_SIGDS.ToBytes());
                bytes.AddRange(Data.RM[i].RM_BLKS1.ToBytes());
                bytes.AddRange(Data.RM[i].RM_BLKS2.ToBytes());
                bytes.AddRange(Data.RM[i].RM_BLKS3.ToBytes());
                bytes.AddRange(Data.RM[i].RM_BLKS4.ToBytes());
            }

            bytes.AddRange(Data.InterfaceVersionDB.ToBytes());
            bytes.AddRange(Data.InterfaceVersion[0].ullId.ToBytes());
            bytes.AddRange(Data.InterfaceVersion[0].ITFspecification_version.ToBytes());
            bytes.AddRange(Data.InterfaceVersion[0].ITFDatas_version.ToBytes());

            return bytes;
        }
        #endregion

        #region CommTcc2CbiStruct

        private static IEnumerable<byte> ToBytes(CommCbiTccParameter Parameter)
        {
            var bytes = new List<byte>();
            bytes.AddRange(Parameter.SegmentName.ToBytes());
            bytes.AddRange(Parameter.StructWidth.ToBytes());
            bytes.AddRange(Parameter.TotalStruct.ToBytes());
            bytes.AddRange(Parameter.ullID.ToBytes());
            bytes.AddRange(Parameter.usMaxSilentCyclesTcc.ToBytes());
            bytes.AddRange(Parameter.usMaxTsnSkip.ToBytes());
            bytes.AddRange(Parameter.usMaxRsnSkip.ToBytes());
            bytes.AddRange(Parameter.usMaxSyncInteval.ToBytes());
            bytes.AddRange(Parameter.usMinSyncInteval.ToBytes());
            bytes.AddRange(Parameter.usTccCycleTime.ToBytes());
            bytes.AddRange(Parameter.usInitializedTime.ToBytes());
            bytes.AddRange(Parameter.ulCbiToTccQJGFNum.ToBytes());
            bytes.AddRange(Parameter.ulCbiToTccJLuNum.ToBytes());
            bytes.AddRange(Parameter.ulCbiToTccXHHDDSNum.ToBytes());
            bytes.AddRange(Parameter.ulCbiToTccDiaoCXHNum.ToBytes());
            bytes.AddRange(Parameter.ulCbiToTccXHKZNum.ToBytes());
            bytes.AddRange(Parameter.ulCbiToTccResevedNum.ToBytes());
            bytes.AddRange(Parameter.ulCbiToTccInterfaceNum.ToBytes());
            bytes.AddRange(Parameter.ulTccToCbiQJFXNum.ToBytes());
            bytes.AddRange(Parameter.ulTccToCbiBSFQNum.ToBytes());
            bytes.AddRange(Parameter.ulTccToCbiXHXSNum.ToBytes());
            bytes.AddRange(Parameter.ulTccToCbiQJXHHDNum.ToBytes());
            bytes.AddRange(Parameter.ulTccToCbiYWQXNum.ToBytes());
            bytes.AddRange(Parameter.ulTccToCbiWPXQJFXNum.ToBytes());
            bytes.AddRange(Parameter.ulTccToCbiWPXGDNum.ToBytes());
            bytes.AddRange(Parameter.ulTccToCbiWPXXHDSNum.ToBytes());
            bytes.AddRange(Parameter.ulTccToCbiXHCJNum.ToBytes());
            bytes.AddRange(Parameter.ulTccToCbiResevedNum.ToBytes());
            bytes.AddRange(Parameter.ulTccToCbiInterfaceNum.ToBytes());
            return bytes;
        }

        private static IEnumerable<byte> ToBytes(CommTccCbiStructData Data)
        {
            var bytes = new List<byte>();
            bytes.AddRange(Data.QJFXMsgDB.ToBytes());
            for (int i = 0; i < 16; i++)
            {
                bytes.AddRange(Data.QJFXMsg[i].ullId.ToBytes());
                bytes.AddRange(Data.QJFXMsg[i].MsgAdderss.ToBytes());
            }
            bytes.AddRange(Data.QJBSFQMsgDB.ToBytes());
            for (int i = 0; i < 240; i++)
            {
                bytes.AddRange(Data.QJBSFQMsg[i].ullId.ToBytes());
                bytes.AddRange(Data.QJBSFQMsg[i].MsgAdderss.ToBytes());
            }
            bytes.AddRange(Data.XHXSMsgDB.ToBytes());
            for (int i = 0; i < 40; i++)
            {
                bytes.AddRange(Data.XHXSMsg[i].ullId.ToBytes());
                bytes.AddRange(Data.XHXSMsg[i].MsgAdderss.ToBytes());
            }
            bytes.AddRange(Data.QJXHHDDSMsgDB.ToBytes());
            for (int i = 0; i < 16; i++)
            {
                bytes.AddRange(Data.QJXHHDDS[i].ullId.ToBytes());
                bytes.AddRange(Data.QJXHHDDS[i].MsgAdderss.ToBytes());
            }
            bytes.AddRange(Data.YWQXMsgDB.ToBytes());
            for (int i = 0; i < 120; i++)
            {
                bytes.AddRange(Data.YWQXMsg[i].ullId.ToBytes());
                bytes.AddRange(Data.YWQXMsg[i].MsgAdderss.ToBytes());
            }
            bytes.AddRange(Data.WPXQJFXMsgDB.ToBytes());
            for (int i = 0; i < 16; i++)
            {
                bytes.AddRange(Data.WPXQJFXMsg[i].ullId.ToBytes());
                bytes.AddRange(Data.WPXQJFXMsg[i].MsgAdderss.ToBytes());
            }
            bytes.AddRange(Data.WPXGDZTMsgDB.ToBytes());
            for (int i = 0; i < 128; i++)
            {
                bytes.AddRange(Data.WPXGDZTMsg[i].ullId.ToBytes());
                bytes.AddRange(Data.WPXGDZTMsg[i].MsgAdderss.ToBytes());
            }
            bytes.AddRange(Data.WPXXHHDDSMsgDB.ToBytes());
            for (int i = 0; i < 16; i++)
            {
                bytes.AddRange(Data.WPXXHHDDSMsg[i].ullId.ToBytes());
                bytes.AddRange(Data.WPXXHHDDSMsg[i].MsgAdderss.ToBytes());
            }
            bytes.AddRange(Data.XHCJMsgDB.ToBytes());
            for (int i = 0; i < 20; i++)
            {
                bytes.AddRange(Data.XHCJMsg[i].ullId.ToBytes());
                bytes.AddRange(Data.XHCJMsg[i].MsgAdderss.ToBytes());
            }
            bytes.AddRange(Data.TCReseivedMsgDB.ToBytes());
            bytes.AddRange(Data.TCReseivedMsg[0].ullId.ToBytes());
            bytes.AddRange(Data.TCReseivedMsg[0].MsgAdderss.ToBytes());

            bytes.AddRange(Data.InterfaceVersionDB.ToBytes());
            bytes.AddRange(Data.InterfaceVersion[0].ullId.ToBytes());
            bytes.AddRange(Data.InterfaceVersion[0].MsgAdderss.ToBytes());

            return bytes;
        }

        private static IEnumerable<byte> ToBytes(CommCbiTccStructData Data)
        {
            var bytes = new List<byte>();
            bytes.AddRange(Data.QJFXCmdMsgDB.ToBytes());
            for (int i = 0; i < 16; i++)
            {
                bytes.AddRange(Data.QJFXCmdMsg[i].ullId.ToBytes());
                bytes.AddRange(Data.QJFXCmdMsg[i].MsgAdderss.ToBytes());
            }

            bytes.AddRange(Data.RouteMsgDB.ToBytes());
            for (int i = 0; i < 40; i++)
            {
                bytes.AddRange(Data.RouteMsg[i].ullId.ToBytes());
                bytes.AddRange(Data.RouteMsg[i].MsgAdderss.ToBytes());
            }

            bytes.AddRange(Data.XHHDDSMsgDB.ToBytes());
            for (int i = 0; i < 32; i++)
            {
                bytes.AddRange(Data.XHHDDSMsg[i].ullId.ToBytes());
                bytes.AddRange(Data.XHHDDSMsg[i].MsgAdderss.ToBytes());
            }

            bytes.AddRange(Data.XHDCMsgDB.ToBytes());
            for (int i = 0; i < 64; i++)
            {
                bytes.AddRange(Data.XHDCMsg[i].ullId.ToBytes());
                bytes.AddRange(Data.XHDCMsg[i].MsgAdderss.ToBytes());
            }

            bytes.AddRange(Data.XHKZMsgDB.ToBytes());
            for (int i = 0; i < 20; i++)
            {
                bytes.AddRange(Data.XHKZMsg[i].ullId.ToBytes());
                bytes.AddRange(Data.XHKZMsg[i].MsgAdderss.ToBytes());
            }
            bytes.AddRange(Data.CTReseivedMsgDB.ToBytes());
            bytes.AddRange(Data.CTReseivedMsg[0].ullId.ToBytes());
            bytes.AddRange(Data.CTReseivedMsg[0].MsgAdderss.ToBytes());

            bytes.AddRange(Data.InterfaceVerMsgDB.ToBytes());
            bytes.AddRange(Data.InterfaceVerMsg[0].ullId.ToBytes());
            bytes.AddRange(Data.InterfaceVerMsg[0].MsgAdderss.ToBytes());

            return bytes;
        }

        #endregion

        private static IEnumerable<byte> ToBytes(InterfaceInfo Data)
        {
            var bytes = new List<byte>();

            bytes.AddRange(Data.InterfaceName.ToBytes());
            bytes.AddRange(Data.InterfaceByteNum.ToBytes());
            bytes.AddRange(Data.InterfaceNum.ToBytes());

            return bytes;
        }

        #region CommTcc2ZpwStruct

        private static IEnumerable<byte> ToBytes(Tcc2ZpwInfCfgData Data)
        {
            var bytes = new List<byte>();

            bytes.AddRange(Data.IsConnExist.ToBytes());
            bytes.AddRange(Data.SrcDeviceId.ToBytes());
            bytes.AddRange(Data.DstDeviceId.ToBytes());
            bytes.AddRange(Data.MaxSilentCycle.ToBytes());
            bytes.AddRange(Data.CommInterval.ToBytes());
            bytes.AddRange(Data.ProtVer.ToBytes());

            return bytes;
        }

        private static IEnumerable<byte> ToBytes(Tcc2ZpwRcvData Data)
        {
            var bytes = new List<byte>();

            bytes.AddRange(Data.BlkStatMsgAddr.ToBytes());
            bytes.AddRange(Data.BlkNumFromZpwConsiderBlankItem.ToBytes());
            bytes.AddRange(Data.CabinetAcr1.ToBytes());

            return bytes;
        }

        private static IEnumerable<byte> ToBytes(Tcc2ZpwSendData Data)
        {
            var bytes = new List<byte>();

            bytes.AddRange(Data.BlkCodeMsgAddr.ToBytes());
            bytes.AddRange(Data.BlkNum2ZpwConsiderBlankItem.ToBytes());
            bytes.AddRange(Data.CabinetId.ToBytes());

            return bytes;
        }

        #endregion

        #region CommTcc2CtcStruct

        private static IEnumerable<byte> ToBytes(Tcc2CtcInfCfgData Data)
        {
            var bytes = new List<byte>();

            bytes.AddRange(Data.IsConnExist.ToBytes());
            bytes.AddRange(Data.SrcDeviceId.ToBytes());
            bytes.AddRange(Data.DstDeviceId.ToBytes());
            bytes.AddRange(Data.MyCtcMsgMasterDeviceCtrlBs.ToBytes());
            bytes.AddRange(Data.MyCtcMsgMasterDeviceCtrlLiaison.ToBytes());
            bytes.AddRange(Data.MyCtcMsgSlaveDeviceId.ToBytes());
            bytes.AddRange(Data.MyCtcMsgSlaveDeviceNum.ToBytes());
            bytes.AddRange(Data.MyCtcMsgSlaveDeviceCtrlBs.ToBytes());
            bytes.AddRange(Data.MyCtcMsgSlaveDeviceCtrlLiaison.ToBytes());
            bytes.AddRange(Data.MyMasterDeviceId.ToBytes());
            bytes.AddRange(Data.Tcc2CtcPacketNum.ToBytes());
            bytes.AddRange(Data.TCC_INF_VER.ToBytes());
            bytes.AddRange(Data.MaxSilentCycle.ToBytes());
            bytes.AddRange(Data.CommInterval.ToBytes());
            return bytes;
        }

        private static IEnumerable<byte> ToBytes(Tcc2CtcReceiveData Data)
        {
            var bytes = new List<byte>();

            bytes.AddRange(Data.PktDescMsgAddr.ToBytes());
            bytes.AddRange(Data.BsCfmCmdMsgAddr.ToBytes());
            bytes.AddRange(Data.InfVerMsgAddr.ToBytes());

            return bytes;
        }

        private static IEnumerable<byte> ToBytes(Tcc2CtcSendData Data)
        {
            var bytes = new List<byte>();

            bytes.AddRange(Data.PktDescMsgAddr.ToBytes());
            bytes.AddRange(Data.LiaisonDirMsgAddr.ToBytes());
            bytes.AddRange(Data.LiaisonNum.ToBytes());
            bytes.AddRange(Data.Sta1SysConnStatMsgAddr.ToBytes());
            bytes.AddRange(Data.Sta1PsdMsgAddr.ToBytes());
            bytes.AddRange(Data.Sta1PsdNum.ToBytes());
            bytes.AddRange(Data.Sta1SigMsgAddr.ToBytes());
            bytes.AddRange(Data.Sta1SigNum.ToBytes());
            bytes.AddRange(Data.Sta1BsMsgAddr.ToBytes());
            bytes.AddRange(Data.Sta1BsNum.ToBytes());
            bytes.AddRange(Data.BsCfmCmdRespMsgAddr.ToBytes());
            bytes.AddRange(Data.InfVerMsgAddr.ToBytes());

            return bytes;
        }

        #endregion

        #region CommTcc2DmStruct
        private static IEnumerable<byte> ToBytes(Tcc2DmInfCfgData Data)
        {
            var bytes = new List<byte>();

            bytes.AddRange(Data.IsConnExist.ToBytes());
            bytes.AddRange(Data.SrcDeviceId.ToBytes());
            bytes.AddRange(Data.DstDeviceId.ToBytes());
            bytes.AddRange(Data.MaxSilentCycle.ToBytes());
            bytes.AddRange(Data.CommInterval.ToBytes());
            bytes.AddRange(Data.TCC_INF_VER.SpecVer);
            bytes.AddRange(Data.TCC_INF_VER.DataVer);

            return bytes;
        }

        private static IEnumerable<byte> ToBytes(Tcc2DmSendData Data)
        {
            var bytes = new List<byte>();

            bytes.AddRange(Data.HwStatMsgAddr.ToBytes());
            bytes.AddRange(Data.RtFromCbiMsgAddr.ToBytes());
            bytes.AddRange(Data.ChgDirCmdFromCbiMsgAddr.ToBytes());
            bytes.AddRange(Data.ChgDirResp2CbiMsgAddr.ToBytes());
            bytes.AddRange(Data.ChgDirCmdFromTccMsgAddr.ToBytes());
            bytes.AddRange(Data.ChgDirResp2TccMsgAddr.ToBytes());
            bytes.AddRange(Data.BrdMsgAddr.ToBytes());
            bytes.AddRange(Data.SubLiaisonDir2CbiMsgAddr.ToBytes());
            bytes.AddRange(Data.TsrMsgAddr.ToBytes());
            bytes.AddRange(Data.BlkTcCodeMsgAddr.ToBytes());
            bytes.AddRange(Data.StatInfo_PioBlkStatMsgAddr.ToBytes());
            bytes.AddRange(Data.StatInfo_PioLiaisonDirMsgAddr.ToBytes());
            bytes.AddRange(Data.StatInfo_PioInStaDirMsgAddr.ToBytes());
            bytes.AddRange(Data.StatInfo_DisasterInfoMsgAddr.ToBytes());
            bytes.AddRange(Data.StatInfo_PioSigStatMsgAddr.ToBytes());
            bytes.AddRange(Data.StatInfo_SigDegrateMsgAddr.ToBytes());
            bytes.AddRange(Data.StatInfo_LiaisonLogicDetStatMsgAddr.ToBytes());
            bytes.AddRange(Data.DeviceAlarmMsgAddr.ToBytes());
            bytes.AddRange(Data.BsLogicDetStatMsgAddr.ToBytes());
            bytes.AddRange(Data.SaMsgAddr.ToBytes());

            return bytes;
        }

        #endregion

        #region CommTcc2TsrsStruct

        private static IEnumerable<byte> ToBytes(Tcc2TsrsInfCfgData Data)
        {
            var bytes = new List<byte>();

            bytes.AddRange(Data.IsConnExist.ToBytes());
            bytes.AddRange(Data.SrcDeviceId.ToBytes());
            bytes.AddRange(Data.DstDeviceId.ToBytes());
            bytes.AddRange(Data.MaxSilentCycle.ToBytes());
            bytes.AddRange(Data.CommInterval.ToBytes());

            return bytes;
        }

        private static IEnumerable<byte> ToBytes(Tcc2TsrsReceiveData Data)
        {
            var bytes = new List<byte>();

            bytes.AddRange(Data.PsdCmdMsgAddr.ToBytes());
            bytes.AddRange(Data.PsdNum.ToBytes());
            bytes.AddRange(Data.LoseDetMsgAddr.ToBytes());
            bytes.AddRange(Data.BlkSectNum.ToBytes());


            return bytes;
        }

        private static IEnumerable<byte> ToBytes(Tcc2TsrsSendData Data)
        {
            var bytes = new List<byte>();

            bytes.AddRange(Data.BsStatMsgAddr.ToBytes());
            bytes.AddRange(Data.BlkSectNum.ToBytes());
            bytes.AddRange(Data.BaliseDirMsgAddr.ToBytes());
            bytes.AddRange(Data.LiaisonBaliseNum.ToBytes());
            bytes.AddRange(Data.PsdStatMsgAddr.ToBytes());
            bytes.AddRange(Data.PsdNum.ToBytes());

            return bytes;
        }

        #endregion
    }
}
