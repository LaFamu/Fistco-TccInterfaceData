using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JSDG.CBI.A.ETBModel;
using JSDG.CBI.A.ETBUtil;

namespace JSDG.CBI.A.ETBProcess
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
            returnBytes.AddRange(ToBytes(datStruct.CommCbiTccStruct.Parameter));
            returnBytes.AddRange(ToBytes(datStruct.CommCbiTccStruct.CbiTccData));
            returnBytes.AddRange(ToBytes(datStruct.CommCbiTccStruct.TccCbiData));
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

        #region CommCbiTccStruct
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
    }
}
