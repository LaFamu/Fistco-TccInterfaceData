using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCC_TCC.A.ETBProcess;
using TCC_TCC.A.ETBUtil;
using NPOI.OpenXmlFormats;

namespace TCC_TCC.A.ETBModel
{
    public class CommTccStruct:BaseStruct
    {
        #region 私有变量
        private Dictionary<string,List<CommModel>> sourceListDic;
        private UInt32 XLBJCnt_R = 0;
        private UInt32 XLGFCnt_R = 0;
        private UInt32 ZJZXHCnt_R = 0;
        private UInt32 ZJZBSFQCnt_R = 0;
        private UInt32 ZJZYWQXCnt_R = 0;
        private UInt32 XTLeuCnt_R = 0;
        private UInt32 WPXXQJCnt_R = 0;
        private UInt32 XHXKCnt_R = 0;
        private UInt32 QJBSCnt_R = 0;
        private UInt32 OtherCnt_R = 0;
        private UInt32 XhjdqCnt_R = 0;
        private UInt32 RMCnt_R = 0;
        private UInt32 InterfaceCnt_R = 0;
        private UInt32 XLBJCnt_S = 0;
        private UInt32 XLGFCnt_S = 0;
        private UInt32 ZJZXHCnt_S = 0;
        private UInt32 ZJZBSFQCnt_S = 0;
        private UInt32 ZJZYWQXCnt_S = 0;
        private UInt32 XTLeuCnt_S = 0;
        private UInt32 WPXXQJCnt_S = 0;
        private UInt32 XHXKCnt_S = 0;
        private UInt32 QJBSCnt_S = 0;
        private UInt32 OtherCnt_S = 0;
        private UInt32 XhjdqCnt_S = 0;
        private UInt32 RMCnt_S = 0;
        private UInt32 InterfaceCnt_S = 0;
        private UInt32 DirectionFlag = 0;

        private const UInt32 MAX_XLBJ_NUM = 16;
        private const UInt32 MAX_XLGF_NUM = 16;
        private const UInt32 MAX_ZJZXH_NUM = 60;
        private const UInt32 MAX_ZJZBSFQ_NUM = 60;
        private const UInt32 MAX_ZJZYWQX_NUM = 40;
        private const UInt32 MAX_XTLEU_NUM = 1;
        private const UInt32 MAX_WPXXQJ_NUM = 16;
        private const UInt32 MAX_XHXK_NUM = 16;
        private const UInt32 MAX_QJBS_NUM = 1;
        private const UInt32 MAX_XHKZ_NUM = 8;
        private const UInt32 MAX_RM_NUM = 4;
        private const UInt32 MAX_INF_NUM = 1;
        #endregion

        public InterfaceInfo InterfaceInfo;
        public List<TccStrcutData> TccStrcutDatas = new List<TccStrcutData>();
        //public CommTccStructData ReceiveData = new CommTccStructData();
        //public CommTccStructData SendData = new CommTccStructData();
        //public CommTccParameter Parameter = new CommTccParameter();

        public CommTccStruct(DataModel dataModel)
        {
            MsgHelper.Instance(1, string.Format("正在生成结构体《Tcc2Tcc》"));
            sourceListDic = dataModel.BitData.TCC2TCCDic;
            //Parameter.TotalStruct = 2;
            //Parameter.StructWidth = 0x518CU;
            //Parameter.SegmentName = "Tcc2Tcc";
            InterfaceInfo.InterfaceName = "Tcc2Tcc";
            InterfaceInfo.InterfaceByteNum = 0x518CU;
            InterfaceInfo.InterfaceNum = 2; 
            Transform();
        }

        #region Transform
        public override void Transform()
        {
            //Parameter.ullID = 0xFFFFFFFFFFFFFFFF;
            //Parameter.usMaxSilentCyclesTcc = (UInt32)250U;
            //Parameter.usMaxTsnSkip = (UInt32)10U;
            //Parameter.usMaxRsnSkip = (UInt32)10U;
            //Parameter.usMaxSyncInteval = (UInt32)250U;
            //Parameter.usMinSyncInteval = (UInt32)250U;
            //Parameter.usTccCycleTime = (UInt32)250U;
            //Parameter.usInitializedTime = (UInt32)250U;
            try
            {

                foreach (var sourceList in sourceListDic)
                {
                    var structdata = new TccStrcutData();

                    structdata.Parameter.ullID = 0xFFFFFFFFFFFFFFFF;
                    structdata.Parameter.usMaxSilentCyclesTcc = (UInt32)250U;
                    structdata.Parameter.usMaxTsnSkip = (UInt32)10U;
                    structdata.Parameter.usMaxRsnSkip = (UInt32)10U;
                    structdata.Parameter.usMaxSyncInteval = (UInt32)250U;
                    structdata.Parameter.usMinSyncInteval = (UInt32)250U;
                    structdata.Parameter.usTccCycleTime = (UInt32)250U;
                    structdata.Parameter.usInitializedTime = (UInt32)250U;
                    if ((!sourceList.Value[0].Name.IsEmpty()) && (structdata.Parameter.ullID == 0xFFFFFFFFFFFFFFFF))
                    {
                        UInt64 id = sourceList.Value[0].Id << 16 ^ sourceList.Value[0].Id;
                        structdata.Parameter.ullID = id << 32 ^ id;
                    }
                   
                    for (int i = 0; i < sourceList.Value.Count();)
                    {
                        if (sourceList.Value[i].Direction.Contains("Receive"))
                        {
                            DirectionFlag = 0x55;
                        }
                        if (sourceList.Value[i].Direction.Contains("Send"))
                        {
                            DirectionFlag = 0xAA;
                        }
                        if (0x55 == DirectionFlag)
                        {
                            structdata.ReceiveData.XLBJMsgDB.ByteOffset = sourceList.Value[i].ByteOffSet;
                            for (int j = 0; j < MAX_XLBJ_NUM; j++)
                            {
                                if ((sourceList.Value[i].MsgBlockType.Contains("线路边界信息")) || (sourceList.Value[i].MsgBlockType.IsEmpty()))
                                {
                                    for (int k = 0; k < 11; k++)
                                    {
                                        var data = sourceList.Value[i + k];
                                        GetXLBJMsg(data, structdata.ReceiveData.XLBJMsg, j);
                                        if (!data.DeviceName.IsEmpty())
                                        {
                                            UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                            structdata.ReceiveData.XLBJMsg[j].ullId = id << 32 ^ id;
                                            structdata.Parameter.usReceiveXLBJNum++;
                                        }
                                    }
                                    i += 11;
                                    XLBJCnt_R++;
                                }
                            }
                            structdata.ReceiveData.XLBJMsgDB.DevNum = XLBJCnt_R;
                            structdata.ReceiveData.XLBJMsgDB.ByteLen = sourceList.Value[i].ByteOffSet - structdata.ReceiveData.XLBJMsgDB.ByteOffset;

                            structdata.ReceiveData.XLGFMsgDB.ByteOffset = sourceList.Value[i].ByteOffSet;
                            for (int j = 0; j < MAX_XLGF_NUM; j++)
                            {
                                if ((sourceList.Value[i].MsgBlockType.Contains("线路改方")) || (sourceList.Value[i].MsgBlockType.IsEmpty()))
                                {
                                    for (int k = 0; k < 6; k++)
                                    {
                                        var data = sourceList.Value[i + k];
                                        GetXLGFMsg(data, structdata.ReceiveData.XLGFMsg, j);
                                        if (!data.DeviceName.IsEmpty())
                                        {
                                            UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                            structdata.ReceiveData.XLGFMsg[j].ullId = id << 32 ^ id;
                                            structdata.Parameter.usReceiveXLGFNum++;
                                        }
                                    }
                                    i += 6;
                                    XLGFCnt_R++;
                                }
                            }
                            structdata.ReceiveData.XLGFMsgDB.DevNum = XLGFCnt_R;
                            structdata.ReceiveData.XLGFMsgDB.ByteLen = sourceList.Value[i].ByteOffSet - structdata.ReceiveData.XLGFMsgDB.ByteOffset;

                            structdata.ReceiveData.ZJZ_XHDB.ByteOffset = sourceList.Value[i].ByteOffSet;
                            for (int j = 0; j < MAX_ZJZXH_NUM; j++)
                            {
                                if ((sourceList.Value[i].MsgBlockType.Contains("中继站信号显示")) || (sourceList.Value[i].MsgBlockType.IsEmpty()))
                                {
                                    for (int k = 0; k < 1; k++)
                                    {
                                        var data = sourceList.Value[i + k];
                                        GetZJZXHMsg(data, structdata.ReceiveData.ZJZ_XH, j);
                                        if (!data.DeviceName.IsEmpty())
                                        {
                                            UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                            structdata.ReceiveData.ZJZ_XH[j].ullId = id << 32 ^ id;
                                            structdata.Parameter.usReceiveZJZXHNum++;
                                        }
                                    }
                                    i += 1;
                                    ZJZXHCnt_R++;
                                }
                            }
                            structdata.ReceiveData.ZJZ_XHDB.DevNum = ZJZXHCnt_R;
                            structdata.ReceiveData.ZJZ_XHDB.ByteLen = sourceList.Value[i].ByteOffSet - structdata.ReceiveData.ZJZ_XHDB.ByteOffset;

                            structdata.ReceiveData.ZJZ_BSFQDB.ByteOffset = sourceList.Value[i].ByteOffSet;
                            for (int j = 0; j < MAX_ZJZBSFQ_NUM; j++)
                            {
                                if ((sourceList.Value[i].MsgBlockType.Contains("中继站闭塞分区信息")) || (sourceList.Value[i].MsgBlockType.IsEmpty()))
                                {
                                    for (int k = 0; k < 2; k++)
                                    {
                                        var data = sourceList.Value[i + k];
                                        GetZJZBSFQMsg(data, structdata.ReceiveData.ZJZ_BSFQ, j);
                                        if (!data.DeviceName.IsEmpty())
                                        {
                                            UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                            structdata.ReceiveData.ZJZ_BSFQ[j].ullId = id << 32 ^ id;
                                            structdata.Parameter.usReceiveZJZBSFQNum++;
                                        }
                                    }
                                    i += 2;
                                    ZJZBSFQCnt_R++;
                                }
                            }
                            structdata.ReceiveData.ZJZ_BSFQDB.DevNum = ZJZBSFQCnt_R;
                            structdata.ReceiveData.ZJZ_BSFQDB.ByteLen = sourceList.Value[i].ByteOffSet - structdata.ReceiveData.ZJZ_BSFQDB.ByteOffset;

                            structdata.ReceiveData.ZJZ_YWQXDB.ByteOffset = sourceList.Value[i].ByteOffSet;
                            for (int j = 0; j < MAX_ZJZYWQX_NUM; j++)
                            {
                                if ((sourceList.Value[i].MsgBlockType.Contains("中继站异物侵限")) || (sourceList.Value[i].MsgBlockType.IsEmpty()))
                                {
                                    for (int k = 0; k < 1; k++)
                                    {
                                        var data = sourceList.Value[i + k];
                                        GetZJZYWQXMsg(data, structdata.ReceiveData.ZJZ_YWQX, j);
                                        if (!data.DeviceName.IsEmpty())
                                        {
                                            UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                            structdata.ReceiveData.ZJZ_YWQX[j].ullId = id << 32 ^ id;
                                            structdata.Parameter.usReceiveZJZYWQXNum++;
                                        }
                                    }
                                    i += 1;
                                    ZJZYWQXCnt_R++;
                                }
                            }
                            structdata.ReceiveData.ZJZ_YWQXDB.DevNum = ZJZYWQXCnt_R;
                            structdata.ReceiveData.ZJZ_YWQXDB.ByteLen = sourceList.Value[i].ByteOffSet - structdata.ReceiveData.ZJZ_YWQXDB.ByteOffset;

                            structdata.ReceiveData.XTLeuMsgDB.ByteOffset = sourceList.Value[i].ByteOffSet;
                            for (int j = 0; j < MAX_XTLEU_NUM; j++)
                            {
                                if ((sourceList.Value[i].MsgBlockType.Contains("系统及外设状态信息")) || (sourceList.Value[i].MsgBlockType.IsEmpty()))
                                {
                                    for (int k = 0; k < 36; k++)
                                    {
                                        var data = sourceList.Value[i + k];
                                        GetXTLeuMsg(data, structdata.ReceiveData.XTLeuMsg, j);
                                        if (!data.DeviceName.IsEmpty())
                                        {
                                            UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                            structdata.ReceiveData.XTLeuMsg[j].ullId = id << 32 ^ id;
                                            structdata.Parameter.usReceiveXTLEUNum++;
                                        }
                                    }
                                    i += 36;
                                    XTLeuCnt_R++;
                                }
                            }
                            structdata.ReceiveData.XTLeuMsgDB.DevNum = XTLeuCnt_R;
                            structdata.ReceiveData.XTLeuMsgDB.ByteLen = sourceList.Value[i].ByteOffSet - structdata.ReceiveData.XTLeuMsgDB.ByteOffset;

                            structdata.ReceiveData.WPX_XQJDB.ByteOffset = sourceList.Value[i].ByteOffSet;
                            for (int j = 0; j < MAX_WPXXQJ_NUM; j++)
                            {
                                if ((sourceList.Value[i].MsgBlockType.Contains("无配线站小区间")) || (sourceList.Value[i].MsgBlockType.IsEmpty()))
                                {
                                    for (int k = 0; k < 1; k++)
                                    {
                                        var data = sourceList.Value[i + k];
                                        GetWPXXQJMsg(data, structdata.ReceiveData.WPX_XQJ, j);
                                        if (!data.DeviceName.IsEmpty())
                                        {
                                            UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                            structdata.ReceiveData.WPX_XQJ[j].ullId = id << 32 ^ id;
                                            structdata.Parameter.usReceiveWPXXQJNum++;
                                        }
                                    }
                                    i += 1;
                                    WPXXQJCnt_R++;
                                }
                            }
                            structdata.ReceiveData.WPX_XQJDB.DevNum = WPXXQJCnt_R;
                            structdata.ReceiveData.WPX_XQJDB.ByteLen = sourceList.Value[i].ByteOffSet - structdata.ReceiveData.WPX_XQJDB.ByteOffset;

                            structdata.ReceiveData.XHXKDB.ByteOffset = sourceList.Value[i].ByteOffSet;
                            for (int j = 0; j < MAX_XHXK_NUM; j++)
                            {
                                if ((sourceList.Value[i].MsgBlockType.Contains("信号许可交互状态")) || (sourceList.Value[i].MsgBlockType.IsEmpty()))
                                {
                                    for (int k = 0; k < 5; k++)
                                    {
                                        var data = sourceList.Value[i + k];
                                        GetXHXKMsg(data, structdata.ReceiveData.XHXK, j);
                                        if (!data.DeviceName.IsEmpty())
                                        {
                                            UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                            structdata.ReceiveData.XHXK[j].ullId = id << 32 ^ id;
                                            structdata.Parameter.usReceiveXHXKNum++;
                                        }
                                    }
                                    i += 5;
                                    XHXKCnt_R++;
                                }
                            }
                            structdata.ReceiveData.XHXKDB.DevNum = XHXKCnt_R;
                            structdata.ReceiveData.XHXKDB.ByteLen = sourceList.Value[i].ByteOffSet - structdata.ReceiveData.XHXKDB.ByteOffset;

                            structdata.ReceiveData.QJBSDB.ByteOffset = sourceList.Value[i].ByteOffSet;
                            for (int j = 0; j < MAX_QJBS_NUM; j++)
                            {
                                if ((sourceList.Value[i].MsgBlockType.Contains("区间闭塞分区状态确认")) || (sourceList.Value[i].MsgBlockType.IsEmpty()))
                                {
                                    for (int k = 0; k < 7; k++)
                                    {
                                        var data = sourceList.Value[i + k];
                                        GetQJBSMsg(data, structdata.ReceiveData.QJBS, j);
                                        if (!data.DeviceName.IsEmpty())
                                        {
                                            UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                            structdata.ReceiveData.QJBS[j].ullId = id << 32 ^ id;
                                            structdata.Parameter.usReceiveQJBSNum++;
                                        }
                                    }
                                    i += 7;
                                    QJBSCnt_R++;
                                }
                            }
                            structdata.ReceiveData.QJBSDB.DevNum = QJBSCnt_R;
                            structdata.ReceiveData.QJBSDB.ByteLen = sourceList.Value[i].ByteOffSet - structdata.ReceiveData.QJBSDB.ByteOffset;

                            structdata.ReceiveData.OtherMsgDB.ByteOffset = sourceList.Value[i].ByteOffSet;
                            for (int j = 0; j < MAX_XHKZ_NUM; j++)
                            {
                                if ((sourceList.Value[i].MsgBlockType.Contains("信号机控制")) || (sourceList.Value[i].MsgBlockType.IsEmpty()))
                                {
                                    for (int k = 0; k < 4; k++)
                                    {
                                        var data = sourceList.Value[i + k];
                                        GetOtherMsg(data, structdata.ReceiveData.OtherMsg, j);
                                        if (!data.DeviceName.IsEmpty())
                                        {
                                            UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                            structdata.ReceiveData.OtherMsg[j].ullId = id << 32 ^ id;
                                            structdata.Parameter.usReceiveOtherNum++;
                                        }
                                    }
                                    i += 4;
                                    OtherCnt_R++;
                                }
                            }
                            structdata.ReceiveData.OtherMsgDB.DevNum = OtherCnt_R;
                            structdata.ReceiveData.OtherMsgDB.ByteLen = sourceList.Value[i].ByteOffSet - structdata.ReceiveData.OtherMsgDB.ByteOffset;

                            structdata.ReceiveData.RMDB.ByteOffset = sourceList.Value[i].ByteOffSet;
                            for (int j = 0; j < MAX_RM_NUM; j++)
                            {
                                if ((sourceList.Value[i].MsgBlockType.Contains("进路信息")) || (sourceList.Value[i].MsgBlockType.IsEmpty()))
                                {
                                    for (int k = 0; k < 7; k++)
                                    {
                                        var data = sourceList.Value[i + k];
                                        GetRmMsg(data, structdata.ReceiveData.RM, j);
                                        if (!data.DeviceName.IsEmpty())
                                        {
                                            UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                            structdata.ReceiveData.RM[j].ullId = id << 32 ^ id;
                                            structdata.Parameter.usReceiveRMNum++;
                                        }
                                    }
                                    i += 7;
                                    RMCnt_R++;
                                }
                            }
                            structdata.ReceiveData.RMDB.DevNum = RMCnt_R;
                            structdata.ReceiveData.RMDB.ByteLen = sourceList.Value[i].ByteOffSet - structdata.ReceiveData.RMDB.ByteOffset;

                            structdata.ReceiveData.InterfaceVersionDB.ByteOffset = sourceList.Value[i].ByteOffSet;
                            for (int j = 0; j < MAX_INF_NUM; j++)
                            {
                                if ((sourceList.Value[i].MsgBlockType.Contains("版本校验信息")) || (sourceList.Value[i].MsgBlockType.IsEmpty()))
                                {
                                    for (int k = 0; k < 2; k++)
                                    {
                                        var data = sourceList.Value[i + k];
                                        GetInterfaceVersionStruct(data, structdata.ReceiveData.InterfaceVersion, j);
                                        if (!data.DeviceName.IsEmpty())
                                        {
                                            UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                            structdata.ReceiveData.InterfaceVersion[j].ullId = id << 32 ^ id;
                                            structdata.Parameter.usReceiveInterfaceNum++;
                                        }
                                    }
                                    i += 2;
                                }
                                InterfaceCnt_R++;
                            }
                            structdata.ReceiveData.InterfaceVersionDB.DevNum = InterfaceCnt_R;
                            structdata.ReceiveData.InterfaceVersionDB.ByteLen = 295 - structdata.ReceiveData.InterfaceVersionDB.ByteOffset;
                        }
                        else if (0xAA == DirectionFlag)
                        {
                            structdata.SendData.XLBJMsgDB.ByteOffset = sourceList.Value[i].ByteOffSet;
                            for (int j = 0; j < MAX_XLBJ_NUM; j++)
                            {
                                if ((sourceList.Value[i].MsgBlockType.Contains("线路边界信息")) || (sourceList.Value[i].MsgBlockType.IsEmpty()))
                                {
                                    for (int k = 0; k < 11; k++)
                                    {
                                        var data = sourceList.Value[i + k];
                                        GetXLBJMsg(data, structdata.SendData.XLBJMsg, j);
                                        if (!data.DeviceName.IsEmpty())
                                        {
                                            UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                            structdata.SendData.XLBJMsg[j].ullId = id << 32 ^ id;
                                            structdata.Parameter.usSendXLBJNum++;
                                        }
                                    }
                                    i += 11;
                                    XLBJCnt_S++;
                                }
                            }
                            structdata.SendData.XLBJMsgDB.DevNum = XLBJCnt_S;
                            structdata.SendData.XLBJMsgDB.ByteLen = sourceList.Value[i].ByteOffSet - structdata.SendData.XLBJMsgDB.ByteOffset;

                            structdata.SendData.XLGFMsgDB.ByteOffset = sourceList.Value[i].ByteOffSet;
                            for (int j = 0; j < MAX_XLGF_NUM; j++)
                            {
                                if ((sourceList.Value[i].MsgBlockType.Contains("线路改方")) || (sourceList.Value[i].MsgBlockType.IsEmpty()))
                                {
                                    for (int k = 0; k < 6; k++)
                                    {
                                        var data = sourceList.Value[i + k];
                                        GetXLGFMsg(data, structdata.SendData.XLGFMsg, j);
                                        if (!data.DeviceName.IsEmpty())
                                        {
                                            UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                            structdata.SendData.XLGFMsg[j].ullId = id << 32 ^ id;
                                            structdata.Parameter.usSendXLGFNum++;
                                        }
                                    }
                                    i += 6;
                                    XLGFCnt_S++;
                                }
                            }
                            structdata.SendData.XLGFMsgDB.DevNum = XLGFCnt_S;
                            structdata.SendData.XLGFMsgDB.ByteLen = sourceList.Value[i].ByteOffSet - structdata.SendData.XLGFMsgDB.ByteOffset;

                            structdata.SendData.ZJZ_XHDB.ByteOffset = sourceList.Value[i].ByteOffSet;
                            for (int j = 0; j < MAX_ZJZXH_NUM; j++)
                            {
                                if ((sourceList.Value[i].MsgBlockType.Contains("中继站信号显示")) || (sourceList.Value[i].MsgBlockType.IsEmpty()))
                                {
                                    for (int k = 0; k < 1; k++)
                                    {
                                        var data = sourceList.Value[i + k];
                                        GetZJZXHMsg(data, structdata.SendData.ZJZ_XH, j);
                                        if (!data.DeviceName.IsEmpty())
                                        {
                                            UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                            structdata.SendData.ZJZ_XH[j].ullId = id << 32 ^ id;
                                            structdata.Parameter.usSendZJZXHNum++;
                                        }
                                    }
                                    i += 1;
                                    ZJZXHCnt_S++;
                                }
                            }
                            structdata.SendData.ZJZ_XHDB.DevNum = ZJZXHCnt_S;
                            structdata.SendData.ZJZ_XHDB.ByteLen = sourceList.Value[i].ByteOffSet - structdata.SendData.ZJZ_XHDB.ByteOffset;

                            structdata.SendData.ZJZ_BSFQDB.ByteOffset = sourceList.Value[i].ByteOffSet;
                            for (int j = 0; j < MAX_ZJZBSFQ_NUM; j++)
                            {
                                if ((sourceList.Value[i].MsgBlockType.Contains("中继站闭塞分区信息")) || (sourceList.Value[i].MsgBlockType.IsEmpty()))
                                {
                                    for (int k = 0; k < 2; k++)
                                    {
                                        var data = sourceList.Value[i + k];
                                        GetZJZBSFQMsg(data, structdata.SendData.ZJZ_BSFQ, j);
                                        if (!data.DeviceName.IsEmpty())
                                        {
                                            UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                            structdata.SendData.ZJZ_BSFQ[j].ullId = id << 32 ^ id;
                                            structdata.Parameter.usSendZJZBSFQNum++;
                                        }
                                    }
                                    i += 2;
                                    ZJZBSFQCnt_S++;
                                }
                            }
                            structdata.SendData.ZJZ_BSFQDB.DevNum = ZJZBSFQCnt_S;
                            structdata.SendData.ZJZ_BSFQDB.ByteLen = sourceList.Value[i].ByteOffSet - structdata.SendData.ZJZ_BSFQDB.ByteOffset;

                            structdata.SendData.ZJZ_YWQXDB.ByteOffset = sourceList.Value[i].ByteOffSet;
                            for (int j = 0; j < MAX_ZJZYWQX_NUM; j++)
                            {
                                if ((sourceList.Value[i].MsgBlockType.Contains("中继站异物侵限")) || (sourceList.Value[i].MsgBlockType.IsEmpty()))
                                {
                                    for (int k = 0; k < 1; k++)
                                    {
                                        var data = sourceList.Value[i + k];
                                        GetZJZYWQXMsg(data, structdata.SendData.ZJZ_YWQX, j);
                                        if (!data.DeviceName.IsEmpty())
                                        {
                                            UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                            structdata.SendData.ZJZ_YWQX[j].ullId = id << 32 ^ id;
                                            structdata.Parameter.usSendZJZYWQXNum++;
                                        }
                                    }
                                    i += 1;
                                    ZJZYWQXCnt_S++;
                                }
                            }
                            structdata.SendData.ZJZ_YWQXDB.DevNum = ZJZYWQXCnt_S;
                            structdata.SendData.ZJZ_YWQXDB.ByteLen = sourceList.Value[i].ByteOffSet - structdata.SendData.ZJZ_YWQXDB.ByteOffset;

                            structdata.SendData.XTLeuMsgDB.ByteOffset = sourceList.Value[i].ByteOffSet;
                            for (int j = 0; j < MAX_XTLEU_NUM; j++)
                            {
                                if ((sourceList.Value[i].MsgBlockType.Contains("系统及外设状态信息")) || (sourceList.Value[i].MsgBlockType.IsEmpty()))
                                {
                                    for (int k = 0; k < 36; k++)
                                    {
                                        var data = sourceList.Value[i + k];
                                        GetXTLeuMsg(data, structdata.SendData.XTLeuMsg, j);
                                        if (!data.DeviceName.IsEmpty())
                                        {
                                            UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                            structdata.SendData.XTLeuMsg[j].ullId = id << 32 ^ id;
                                            structdata.Parameter.usSendXTLEUNum++;
                                        }
                                    }
                                    i += 36;
                                    XTLeuCnt_S++;
                                }
                            }
                            structdata.SendData.XTLeuMsgDB.DevNum = XTLeuCnt_S;
                            structdata.SendData.XTLeuMsgDB.ByteLen = sourceList.Value[i].ByteOffSet - structdata.SendData.XTLeuMsgDB.ByteOffset;

                            structdata.SendData.WPX_XQJDB.ByteOffset = sourceList.Value[i].ByteOffSet;
                            for (int j = 0; j < MAX_WPXXQJ_NUM; j++)
                            {
                                if ((sourceList.Value[i].MsgBlockType.Contains("无配线站小区间")) || (sourceList.Value[i].MsgBlockType.IsEmpty()))
                                {
                                    for (int k = 0; k < 1; k++)
                                    {
                                        var data = sourceList.Value[i + k];
                                        GetWPXXQJMsg(data, structdata.SendData.WPX_XQJ, j);
                                        if (!data.DeviceName.IsEmpty())
                                        {
                                            UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                            structdata.SendData.WPX_XQJ[j].ullId = id << 32 ^ id;
                                            structdata.Parameter.usSendWPXXQJNum++;
                                        }
                                    }
                                    i += 1;
                                    WPXXQJCnt_S++;
                                }
                            }
                            structdata.SendData.WPX_XQJDB.DevNum = WPXXQJCnt_S;
                            structdata.SendData.WPX_XQJDB.ByteLen = sourceList.Value[i].ByteOffSet - structdata.SendData.WPX_XQJDB.ByteOffset;

                            structdata.SendData.XHXKDB.ByteOffset = sourceList.Value[i].ByteOffSet;
                            for (int j = 0; j < MAX_XHXK_NUM; j++)
                            {
                                if ((sourceList.Value[i].MsgBlockType.Contains("信号许可交互状态")) || (sourceList.Value[i].MsgBlockType.IsEmpty()))
                                {
                                    for (int k = 0; k < 5; k++)
                                    {
                                        var data = sourceList.Value[i + k];
                                        GetXHXKMsg(data, structdata.SendData.XHXK, j);
                                        if (!data.DeviceName.IsEmpty())
                                        {
                                            UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                            structdata.SendData.XHXK[j].ullId = id << 32 ^ id;
                                            structdata.Parameter.usSendXHXKNum++;
                                        }
                                    }
                                    i += 5;
                                    XHXKCnt_S++;
                                }
                            }
                            structdata.SendData.XHXKDB.DevNum = XHXKCnt_S;
                            structdata.SendData.XHXKDB.ByteLen = sourceList.Value[i].ByteOffSet - structdata.SendData.XHXKDB.ByteOffset;

                            structdata.SendData.QJBSDB.ByteOffset = sourceList.Value[i].ByteOffSet;
                            for (int j = 0; j < MAX_QJBS_NUM; j++)
                            {
                                if ((sourceList.Value[i].MsgBlockType.Contains("区间闭塞分区状态确认")) || (sourceList.Value[i].MsgBlockType.IsEmpty()))
                                {
                                    for (int k = 0; k < 7; k++)
                                    {
                                        var data = sourceList.Value[i + k];
                                        GetQJBSMsg(data, structdata.SendData.QJBS, j);
                                        if (!data.DeviceName.IsEmpty())
                                        {
                                            UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                            structdata.SendData.QJBS[j].ullId = id << 32 ^ id;
                                            structdata.Parameter.usSendQJBSNum++;
                                        }
                                    }
                                    i += 7;
                                    QJBSCnt_S++;
                                }
                            }
                            structdata.SendData.QJBSDB.DevNum = QJBSCnt_S;
                            structdata.SendData.QJBSDB.ByteLen = sourceList.Value[i].ByteOffSet - structdata.SendData.QJBSDB.ByteOffset;

                            structdata.SendData.OtherMsgDB.ByteOffset = sourceList.Value[i].ByteOffSet;
                            for (int j = 0; j < MAX_XHKZ_NUM; j++)
                            {
                                if ((sourceList.Value[i].MsgBlockType.Contains("信号机控制")) || (sourceList.Value[i].MsgBlockType.IsEmpty()))
                                {
                                    for (int k = 0; k < 4; k++)
                                    {
                                        var data = sourceList.Value[i + k];
                                        GetOtherMsg(data, structdata.SendData.OtherMsg, j);
                                        if (!data.DeviceName.IsEmpty())
                                        {
                                            UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                            structdata.SendData.OtherMsg[j].ullId = id << 32 ^ id;
                                            structdata.Parameter.usSendOtherNum++;
                                        }
                                    }
                                    i += 4;
                                    OtherCnt_S++;
                                }
                            }
                            structdata.SendData.OtherMsgDB.DevNum = OtherCnt_S;
                            structdata.SendData.OtherMsgDB.ByteLen = sourceList.Value[i].ByteOffSet - structdata.SendData.OtherMsgDB.ByteOffset;

                            structdata.SendData.RMDB.ByteOffset = sourceList.Value[i].ByteOffSet;
                            for (int j = 0; j < MAX_RM_NUM; j++)
                            {
                                if ((sourceList.Value[i].MsgBlockType.Contains("进路信息")) || (sourceList.Value[i].MsgBlockType.IsEmpty()))
                                {
                                    for (int k = 0; k < 7; k++)
                                    {
                                        var data = sourceList.Value[i + k];
                                        GetRmMsg(data, structdata.SendData.RM, j);
                                        if (!data.DeviceName.IsEmpty())
                                        {
                                            UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                            structdata.SendData.RM[j].ullId = id << 32 ^ id;
                                            structdata.Parameter.usSendRMNum++;
                                        }
                                    }
                                    i += 7;
                                    RMCnt_S++;
                                }
                            }
                            structdata.SendData.RMDB.DevNum = RMCnt_S;
                            structdata.SendData.RMDB.ByteLen = sourceList.Value[i].ByteOffSet - structdata.SendData.RMDB.ByteOffset;

                            structdata.SendData.InterfaceVersionDB.ByteOffset = sourceList.Value[i].ByteOffSet;
                            for (int j = 0; j < MAX_INF_NUM; j++)
                            {
                                if ((sourceList.Value[i].MsgBlockType.Contains("版本校验信息")) || (sourceList.Value[i].MsgBlockType.IsEmpty()))
                                {
                                    for (int k = 0; k < 2; k++)
                                    {
                                        var data = sourceList.Value[i + k];
                                        GetInterfaceVersionStruct(data, structdata.SendData.InterfaceVersion, j);
                                        if (!data.DeviceName.IsEmpty())
                                        {
                                            UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                            structdata.SendData.InterfaceVersion[j].ullId = id << 32 ^ id;
                                            structdata.Parameter.usSendInterfaceNum++;
                                        }
                                    }
                                    i += 2;
                                    InterfaceCnt_S++;
                                }
                            }
                            structdata.SendData.InterfaceVersionDB.DevNum = InterfaceCnt_S;
                            structdata.SendData.InterfaceVersionDB.ByteLen = 295 - structdata.SendData.InterfaceVersionDB.ByteOffset;
                        }
                    }
                    TccStrcutDatas.Add(structdata);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private CommAddress GetCommTccBaseStruct(CommModel item)
        {
            var data = new CommAddress();

            data.ByteOffset = item.ByteOffSet;
            data.BitOffset = item.BitOffSet;
            data.Width = item.Width;
            data.BitOffset = (data.BitOffset + data.Width - 1) % 8;
            return data;
        }

        private void GetXLBJMsg(CommModel item, List<XLBJ> dataList, int msgIndex)
        {
            switch (item.InformationName)
            {
                case "预留0":
                    dataList[msgIndex].BSFQ_YL0 = GetCommTccBaseStruct(item);
                    break;
                case "闭塞分区状态":
                    dataList[msgIndex].BSFQ_stu = GetCommTccBaseStruct(item);
                    break;
                case "闭塞分区转频预告标志":
                    dataList[msgIndex].BSFQDP_flag = GetCommTccBaseStruct(item);
                    break;
                case "闭塞分区发送低频编码":
                    dataList[msgIndex].BSFQDP_code = GetCommTccBaseStruct(item);
                    break;
                case "低频属性":
                    dataList[msgIndex].BSFQDP_attribute = GetCommTccBaseStruct(item);
                    break;
                case "预留1":
                    dataList[msgIndex].BSFQ_YL1 = GetCommTccBaseStruct(item);
                    break;
                case "防护信号机1的红灯断丝信息":
                    dataList[msgIndex].FHXH1 = GetCommTccBaseStruct(item);
                    break;
                case "防护信号机2的红灯断丝信息":
                    dataList[msgIndex].FHXH2 = GetCommTccBaseStruct(item);
                    break;
                case "防护信号机3的红灯断丝信息":
                    dataList[msgIndex].FHXH3 = GetCommTccBaseStruct(item);
                    break;
                case "预留2":
                    dataList[msgIndex].BSFQ_YL2 = GetCommTccBaseStruct(item);
                    break;
                case "预留3":
                    dataList[msgIndex].BSFQ_YL3 = GetCommTccBaseStruct(item);
                    break;
                default:
                    throw new Exception("信息名称填写错误！");
            }
        }

        private void GetXLGFMsg(CommModel item, List<XLGF> dataList, int msgIndex)
        {
            switch (item.InformationName)
            {
                case "区间状态":
                    dataList[msgIndex].BZQJ_stu = GetCommTccBaseStruct(item);
                    break;
                case "进路标志":
                    dataList[msgIndex].XLJL_flag = GetCommTccBaseStruct(item);
                    break;
                case "发车进路锁闭状态":
                    dataList[msgIndex].XLJLSB = GetCommTccBaseStruct(item);
                    break;
                case "当前方向信息":
                    dataList[msgIndex].XLFX_msg = GetCommTccBaseStruct(item);
                    break;
                case "改方命令":
                    dataList[msgIndex].XLGF_cmd = GetCommTccBaseStruct(item);
                    break;
                case "改方类型":
                    dataList[msgIndex].XLGF_type = GetCommTccBaseStruct(item);
                    break;
                default:
                    throw new Exception("信息名称填写错误！");
            }
        }

        private void GetZJZXHMsg(CommModel item, List<ZJZXH> dataList, int msgIndex)
        {
            switch (item.InformationName)
            {
                case "显示状态":
                    dataList[msgIndex].ZJXH = GetCommTccBaseStruct(item);
                    break;
                default:
                    throw new Exception("信息名称填写错误！");
            }
        }

        private void GetZJZBSFQMsg(CommModel item, List<ZJZBSFQ> dataList, int msgIndex)
        {
            switch (item.InformationName)
            {
                case "低频信息":
                    dataList[msgIndex].BSFQ_DP = GetCommTccBaseStruct(item);
                    break;
                case "状态":
                    dataList[msgIndex].BSFQ_ZT = GetCommTccBaseStruct(item);
                    break;
                default:
                    throw new Exception("信息名称填写错误！");
            }
        }

        private void GetZJZYWQXMsg(CommModel item, List<ZJZYWQX> dataList, int msgIndex)
        {
            switch (item.InformationName)
            {
                case "侵限状态":
                    dataList[msgIndex].ZJZ_YWQX = GetCommTccBaseStruct(item);
                    break;
                default:
                    throw new Exception("信息名称填写错误！");
            }
        }

        private void GetXTLeuMsg(CommModel item, List<XTLEU> dataList, int msgIndex)
        {
            switch (item.InformationName)
            {
                case "CBI连接状态":
                    dataList[msgIndex].COMtoCBI = GetCommTccBaseStruct(item);
                    break;
                case "CTC连接状态":
                    dataList[msgIndex].COMtoCTC = GetCommTccBaseStruct(item);
                    break;
                case "A/B主控状态":
                    dataList[msgIndex].ZJwork_stu = GetCommTccBaseStruct(item);
                    break;
                case "单机/双机状态":
                    dataList[msgIndex].DSJwork_stu = GetCommTccBaseStruct(item);
                    break;
                case "列控站间连接状态":
                    dataList[msgIndex].COMtoTCC = GetCommTccBaseStruct(item);
                    break;
                case "与TSRS连接状态":
                    dataList[msgIndex].COMtoTSRS = GetCommTccBaseStruct(item);
                    break;
                case "与主站通信状态":
                    dataList[msgIndex].COMtoMain = GetCommTccBaseStruct(item);
                    break;
                case "闭塞分区上电初始化状态":
                    dataList[msgIndex].BSFQ_initial = GetCommTccBaseStruct(item);
                    break;
                case "LEU1应答器1通信状态":
                    dataList[msgIndex].LEU1_TP1 = GetCommTccBaseStruct(item);
                    break;
                case "LEU1应答器2通信状态":
                    dataList[msgIndex].LEU1_TP2 = GetCommTccBaseStruct(item);
                    break;
                case "LEU1应答器3通信状态":
                    dataList[msgIndex].LEU1_TP3 = GetCommTccBaseStruct(item);
                    break;
                case "LEU1应答器4通信状态":
                    dataList[msgIndex].LEU1_TP4 = GetCommTccBaseStruct(item);
                    break;
                case "LEU1通道1通信状态":
                    dataList[msgIndex].LEU1_TD1 = GetCommTccBaseStruct(item);
                    break;
                case "LEU1通道2通信状态":
                    dataList[msgIndex].LEU1_TD2 = GetCommTccBaseStruct(item);
                    break;
                case "LEU1是否有效配置":
                    dataList[msgIndex].LEU1CFG_avlble = GetCommTccBaseStruct(item);
                    break;
                case "LEU2应答器1通信状态":
                    dataList[msgIndex].LEU2_TP1 = GetCommTccBaseStruct(item);
                    break;
                case "LEU2应答器2通信状态":
                    dataList[msgIndex].LEU2_TP2 = GetCommTccBaseStruct(item);
                    break;
                case "LEU2应答器3通信状态":
                    dataList[msgIndex].LEU2_TP3 = GetCommTccBaseStruct(item);
                    break;
                case "LEU2应答器4通信状态":
                    dataList[msgIndex].LEU2_TP4 = GetCommTccBaseStruct(item);
                    break;
                case "LEU2通道1通信状态":
                    dataList[msgIndex].LEU2_TD1 = GetCommTccBaseStruct(item);
                    break;
                case "LEU2通道2通信状态":
                    dataList[msgIndex].LEU2_TD2 = GetCommTccBaseStruct(item);
                    break;
                case "LEU2是否有效配置":
                    dataList[msgIndex].LEU2CFG_avlble = GetCommTccBaseStruct(item);
                    break;
                case "LEU3应答器1通信状态":
                    dataList[msgIndex].LEU3_TP1 = GetCommTccBaseStruct(item);
                    break;
                case "LEU3应答器2通信状态":
                    dataList[msgIndex].LEU3_TP2 = GetCommTccBaseStruct(item);
                    break;
                case "LEU3应答器3通信状态":
                    dataList[msgIndex].LEU3_TP3 = GetCommTccBaseStruct(item);
                    break;
                case "LEU3应答器4通信状态":
                    dataList[msgIndex].LEU3_TP4 = GetCommTccBaseStruct(item);
                    break;
                case "LEU3通道1通信状态":
                    dataList[msgIndex].LEU3_TD1 = GetCommTccBaseStruct(item);
                    break;
                case "LEU3通道2通信状态":
                    dataList[msgIndex].LEU3_TD2 = GetCommTccBaseStruct(item);
                    break;
                case "LEU3是否有效配置":
                    dataList[msgIndex].LEU3CFG_avlble = GetCommTccBaseStruct(item);
                    break;
                case "LEU4应答器1通信状态":
                    dataList[msgIndex].LEU4_TP1 = GetCommTccBaseStruct(item);
                    break;
                case "LEU4应答器2通信状态":
                    dataList[msgIndex].LEU4_TP2 = GetCommTccBaseStruct(item);
                    break;
                case "LEU4应答器3通信状态":
                    dataList[msgIndex].LEU4_TP3 = GetCommTccBaseStruct(item);
                    break;
                case "LEU4应答器4通信状态":
                    dataList[msgIndex].LEU4_TP4 = GetCommTccBaseStruct(item);
                    break;
                case "LEU4通道1通信状态":
                    dataList[msgIndex].LEU4_TD1 = GetCommTccBaseStruct(item);
                    break;
                case "LEU4通道2通信状态":
                    dataList[msgIndex].LEU4_TD2 = GetCommTccBaseStruct(item);
                    break;
                case "LEU4是否有效配置":
                    dataList[msgIndex].LEU4CFG_avlble = GetCommTccBaseStruct(item);
                    break;
                default:
                    throw new Exception("信息名称填写错误！");
            }
        }

        private void GetWPXXQJMsg(CommModel item, List<WPXXQJ> dataList, int msgIndex)
        {
            switch (item.InformationName)
            {
                case "小区间状态":
                    dataList[msgIndex].XQJ_stu = GetCommTccBaseStruct(item);
                    break;
                default:
                    throw new Exception("信息名称填写错误！");
            }
        }

        private void GetXHXKMsg(CommModel item, List<XHXK> dataList, int msgIndex)
        {
            switch (item.InformationName)
            {
                case "信号许可ID号":
                    dataList[msgIndex].XHXK_ID = GetCommTccBaseStruct(item);
                    break;
                case "信号许可状态":
                    dataList[msgIndex].XHXK_ZT = GetCommTccBaseStruct(item);
                    break;
                case "信号许可流向":
                    dataList[msgIndex].XHXK_LX = GetCommTccBaseStruct(item);
                    break;
                case "中继站线路区间占用逻辑检查":
                    dataList[msgIndex].XHXK_ZY = GetCommTccBaseStruct(item);
                    break;
                case "预留":
                    dataList[msgIndex].XHXK_YL = GetCommTccBaseStruct(item);
                    break;
                default:
                    throw new Exception("信息名称填写错误！");
            }
        }

        private void GetQJBSMsg(CommModel item, List<QJBS> dataList, int msgIndex)
        {
            switch (item.InformationName)
            {
                case "TCC站编号":
                    dataList[msgIndex].TCC_ID = GetCommTccBaseStruct(item);
                    break;
                case "确认状态属性":
                    dataList[msgIndex].QRZT = GetCommTccBaseStruct(item);
                    break;
                case "状态确认命令类型":
                    dataList[msgIndex].CMD_type = GetCommTccBaseStruct(item);
                    break;
                case "确认闭塞分区编号/区间方向口编号":
                    dataList[msgIndex].QJK_ID = GetCommTccBaseStruct(item);
                    break;
                case "确认操作源CTC编号":
                    dataList[msgIndex].CTC_ID = GetCommTccBaseStruct(item);
                    break;
                case "命令执行结果":
                    dataList[msgIndex].ZJZ_CMDJG = GetCommTccBaseStruct(item);
                    break;
                case "命令执行失败原因":
                    dataList[msgIndex].ZJZ_CMDYY = GetCommTccBaseStruct(item);
                    break;
                default:
                    throw new Exception("信息名称填写错误！");
            }
        }

        private void GetOtherMsg(CommModel item, List<Reserved> dataList, int msgIndex)
        {
            switch (item.InformationName)
            {
                case "KDJ_信号机":
                    dataList[msgIndex].XH_KDJ = GetCommTccBaseStruct(item);
                    break;
                case "LXJ_信号机":
                    dataList[msgIndex].XH_LXJ = GetCommTccBaseStruct(item);
                    break;
                case "YXJ_信号机":
                    dataList[msgIndex].XH_YXJ = GetCommTccBaseStruct(item);
                    break;
                case "TXJ_信号机":
                    dataList[msgIndex].XH_TXJ = GetCommTccBaseStruct(item);
                    break;
                default:
                    throw new Exception("信息名称填写错误！");
            }
        }
        private void GetRmMsg(CommModel item, List<RM> dataList, int msgIndex)
        {
            switch (item.InformationName)
            {
                case "进路编号":
                    dataList[msgIndex].RM_ID = GetCommTccBaseStruct(item);
                    break;
                case "信号显示状态":
                    dataList[msgIndex].RM_SIGS = GetCommTccBaseStruct(item);
                    break;
                case "信号机点灭灯状态":
                    dataList[msgIndex].RM_SIGDS = GetCommTccBaseStruct(item);
                    break;
                case "区段锁闭状态1":
                    dataList[msgIndex].RM_BLKS1 = GetCommTccBaseStruct(item);
                    break;
                case "区段锁闭状态2":
                    dataList[msgIndex].RM_BLKS2 = GetCommTccBaseStruct(item);
                    break;
                case "区段锁闭状态3":
                    dataList[msgIndex].RM_BLKS3 = GetCommTccBaseStruct(item);
                    break;
                case "区段锁闭状态4":
                    dataList[msgIndex].RM_BLKS4 = GetCommTccBaseStruct(item);
                    break;
                default:
                    throw new Exception("信息名称填写错误！");
            }
        }
        private void GetInterfaceVersionStruct(CommModel item, List<InterfaceVersions> dataList, int msgIndex)
        {
            switch (item.InformationName)
            {
                case "协议版本校验信息":
                    dataList[msgIndex].ITFspecification_version = GetCommTccBaseStruct(item);
                    break;
                case "数据版本校验信息":
                    dataList[msgIndex].ITFDatas_version = GetCommTccBaseStruct(item);
                    break;
                default:
                    throw new Exception("信息名称填写错误！");
            }
        }

       

        #endregion

    }

    

    public class CommTccParameter
    {
        //public string SegmentName { get; set; }
        //public UInt32 StructWidth { get; set; }
        //public UInt32 TotalStruct { get; set; }
        public UInt64 ullID { get; set; }
        public UInt32 usMaxSilentCyclesTcc { get; set; }
        public UInt32 usMaxTsnSkip { get; set; }
        public UInt32 usMaxRsnSkip { get; set; }
        public UInt32 usMaxSyncInteval { get; set; }
        public UInt32 usMinSyncInteval { get; set; }
        public UInt32 usTccCycleTime { get; set; }
        public UInt32 usInitializedTime { get; set; }
        public UInt32 usReceiveXLBJNum { get; set; }
        public UInt32 usReceiveXLGFNum { get; set; }
        public UInt32 usReceiveZJZXHNum { get; set; }
        public UInt32 usReceiveZJZBSFQNum { get; set; }
        public UInt32 usReceiveZJZYWQXNum { get; set; }
        public UInt32 usReceiveXTLEUNum { get; set; }
        public UInt32 usReceiveWPXXQJNum { get; set; }
        public UInt32 usReceiveXHXKNum { get; set; }
        public UInt32 usReceiveQJBSNum { get; set; }
        public UInt32 usReceiveOtherNum { get; set; }
        public UInt32 usReceiveRMNum { get; set; }
        public UInt32 usReceiveInterfaceNum { get; set; }
        public UInt32 usSendXLBJNum { get; set; }
        public UInt32 usSendXLGFNum { get; set; }
        public UInt32 usSendZJZXHNum { get; set; }
        public UInt32 usSendZJZBSFQNum { get; set; }
        public UInt32 usSendZJZYWQXNum { get; set; }
        public UInt32 usSendXTLEUNum { get; set; }
        public UInt32 usSendWPXXQJNum { get; set; }
        public UInt32 usSendXHXKNum { get; set; }
        public UInt32 usSendQJBSNum { get; set; }
        public UInt32 usSendOtherNum { get; set; }
        public UInt32 usSendRMNum { get; set; }
        public UInt32 usSendInterfaceNum { get; set; }

        
    }

    public class TccStrcutData
    {
        public CommTccParameter Parameter;
        public CommTccStructData ReceiveData;
        public CommTccStructData SendData;

        public TccStrcutData()
        {
            Parameter = new CommTccParameter();
            ReceiveData = new CommTccStructData();
            SendData = new CommTccStructData();
        }
    }

    public class CommTccStructData
    {
        public DataBlockAddress XLBJMsgDB { get; set; }
        public List<XLBJ> XLBJMsg { get; set; }
        public DataBlockAddress XLGFMsgDB { get; set; }
        public List<XLGF> XLGFMsg { get; set; }
        public DataBlockAddress ZJZ_XHDB { get; set; }
        public List<ZJZXH> ZJZ_XH { get; set; }
        public DataBlockAddress ZJZ_BSFQDB { get; set; }
        public List<ZJZBSFQ> ZJZ_BSFQ { get; set; }
        public DataBlockAddress ZJZ_YWQXDB { get; set; }
        public List<ZJZYWQX> ZJZ_YWQX { get; set; }
        public DataBlockAddress XTLeuMsgDB { get; set; }
        public List<XTLEU> XTLeuMsg { get; set; }
        public DataBlockAddress WPX_XQJDB { get; set; }
        public List<WPXXQJ> WPX_XQJ { get; set; }
        public DataBlockAddress XHXKDB { get; set; }
        public List<XHXK> XHXK { get; set; }
        public DataBlockAddress QJBSDB { get; set; }
        public List<QJBS> QJBS { get; set; }
        public DataBlockAddress OtherMsgDB { get; set; }
        public List<Reserved> OtherMsg { get; set; }
        public DataBlockAddress RMDB { get; set; }
        public List<RM> RM { get; set; }
        public DataBlockAddress InterfaceVersionDB { get; set; }
        public List<InterfaceVersions> InterfaceVersion { get; set; }

        public CommTccStructData()
        {
            this.XLBJMsgDB = new DataBlockAddress();
            this.XLGFMsgDB = new DataBlockAddress();
            this.ZJZ_XHDB = new DataBlockAddress();
            this.ZJZ_BSFQDB = new DataBlockAddress();
            this.ZJZ_YWQXDB = new DataBlockAddress();
            this.XTLeuMsgDB = new DataBlockAddress();
            this.WPX_XQJDB = new DataBlockAddress();
            this.XHXKDB = new DataBlockAddress();
            this.QJBSDB = new DataBlockAddress();
            this.OtherMsgDB = new DataBlockAddress();
            this.RMDB = new DataBlockAddress();
            this.InterfaceVersionDB = new DataBlockAddress();

            this.XLBJMsg = new List<XLBJ>();
            this.XLGFMsg = new List<XLGF>();
            this.ZJZ_XH = new List<ZJZXH>();
            this.ZJZ_BSFQ = new List<ZJZBSFQ>();
            this.ZJZ_YWQX = new List<ZJZYWQX>();
            this.XTLeuMsg = new List<XTLEU>();
            this.WPX_XQJ = new List<WPXXQJ>();
            this.XHXK = new List<XHXK>();
            this.QJBS = new List<QJBS>();
            this.OtherMsg = new List<Reserved>();
            this.RM = new List<RM>();
            this.InterfaceVersion = new List<InterfaceVersions>();

            XTLEU XTLEUdata = new XTLEU();
            XTLeuMsg.Add(XTLEUdata);
            QJBS QJBSdata = new QJBS();
            QJBS.Add(QJBSdata);
            for (int i = 0; i < 8; i++)
            {
                Reserved Reserveddata = new Reserved();
                OtherMsg.Add(Reserveddata);
            }
            for (int i = 0; i < 4; i++)
            {
                RM rMdata = new RM();
                RM.Add(rMdata);
            }
            InterfaceVersions InterfaceVersionsdata = new InterfaceVersions();
            InterfaceVersion.Add(InterfaceVersionsdata);
            for (int i = 0; i < 16; i++)
            {
                XLBJ XLBJdata = new XLBJ();
                this.XLBJMsg.Add(XLBJdata);

                XLGF XLGFdata = new XLGF();
                this.XLGFMsg.Add(XLGFdata);

                WPXXQJ WPXXQJdata = new WPXXQJ();
                this.WPX_XQJ.Add(WPXXQJdata);

                XHXK XHXKdata = new XHXK();
                this.XHXK.Add(XHXKdata);
            }

            for (int i = 0; i < 60; i++)
            {
                ZJZXH ZJZXHdata = new ZJZXH();
                this.ZJZ_XH.Add(ZJZXHdata);

                ZJZBSFQ ZJZBSFQdata = new ZJZBSFQ();
                this.ZJZ_BSFQ.Add(ZJZBSFQdata);
            }

            for (int i = 0; i < 40; i++)
            {
                ZJZYWQX data = new ZJZYWQX();
                this.ZJZ_YWQX.Add(data);
            }
        }
    }
}
