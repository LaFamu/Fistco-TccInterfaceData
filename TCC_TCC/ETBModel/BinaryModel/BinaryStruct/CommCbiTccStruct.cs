using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCC_TCC.A.ETBProcess;
using TCC_TCC.A.ETBUtil;
using NPOI.OpenXmlFormats;
using TCC_TCC.A.ETBModel;

namespace TCC_TCC.A.ETBModel
{
    public class CommCbiTccStruct
    {
        #region 私有变量
        private List<CommModel> sourceList;
        private UInt32 DirectionFlag = 0;
        private UInt32 Cnt_R = 0;
        #endregion

        public CommCbiTccStructData CbiTccData = new CommCbiTccStructData();
        public CommTccCbiStructData TccCbiData = new CommTccCbiStructData();
        public CommCbiTccParameter Parameter = new CommCbiTccParameter();

        public CommCbiTccStruct(DataModel dataModel)
        {
            MsgHelper.Instance(1, string.Format("正在生成结构体《Tcc2Cbi》"));
            sourceList = dataModel.BitData.TCC2CBIList;
            Parameter.TotalStruct = 1;
            Parameter.StructWidth = 0x7CCCU;
            Parameter.SegmentName = "Tcc2Cbi";

            for (int i = 0; i < sourceList.Count(); i++)
            {
                sourceList[i].Direction = sourceList[i].Direction.ToUpper();
                //if (dataModel.ProjectName.Contains("CBI"))
                //{
                //    if (sourceList[i].Direction.Contains("SEND"))
                //    {
                //        sourceList[i].Direction = "CBI-TCC";
                //    }
                //    if (sourceList[i].Direction.Contains("RECEIVE"))
                //    {
                //        sourceList[i].Direction = "TCC-CBI";
                //    }
                //}
                //if (dataModel.ProjectName.Contains("TCC"))
                //{
                //    if (sourceList[i].Direction.Contains("RECEIVE"))
                //    {
                //        sourceList[i].Direction = "CBI-TCC";
                //    }
                //    if (sourceList[i].Direction.Contains("SEND"))
                //    {
                //        sourceList[i].Direction = "TCC-CBI";
                //    }
                //}
            }

            Transform();
        }

        #region Transform
        private void Transform()
        {
            Parameter.ullID = 0xFFFFFFFFFFFFFFFF;
            Parameter.usMaxSilentCyclesTcc = (UInt32)250U;
            Parameter.usMaxTsnSkip = (UInt32)10U;
            Parameter.usMaxRsnSkip = (UInt32)10U;
            Parameter.usMaxSyncInteval = (UInt32)250U;
            Parameter.usMinSyncInteval = (UInt32)250U;
            Parameter.usTccCycleTime = (UInt32)250U;
            Parameter.usInitializedTime = (UInt32)250U;
            try
            {
                if ((!sourceList[0].Name.IsEmpty()) && (Parameter.ullID == 0xFFFFFFFFFFFFFFFF))
                {
                    UInt64 id = sourceList[0].Id << 16 ^ sourceList[0].Id;
                    Parameter.ullID = id << 32 ^ id;
                }
                else
                {
                    throw new Exception("通讯接口ID填写错误！");
                }
                for (int i = 0; i < sourceList.Count(); )
                {
                    if (sourceList[i].Direction.Contains("RECEIVE"))
                    {
                        DirectionFlag = 0x55;
                    }
                    if (sourceList[i].Direction.Contains("SEND"))
                    {
                        DirectionFlag = 0xAA;
                    }
                    //MsgHelper.Instance(1, string.Format("{0}\r\n",i));
                    if (0x55 == DirectionFlag)
                    {
                        CbiTccData.QJFXCmdMsgDB.ByteOffset = sourceList[i].ByteOffSet;
                        for (int j = 0; j < 16; j++)
                        {
                            if ((sourceList[i].MsgBlockType.Contains("区间方向控制命令")) || (sourceList[i].MsgBlockType.IsEmpty()))
                            {
                                for (int k = 0; k < 4; k++)
                                {
                                    var data = sourceList[i + k];
                                    CbiTccData.QJFXCmdMsg[j].MsgAdderss[k] = GetCommTccBaseStruct(data);
                                    if (!data.DeviceName.IsEmpty())
                                    {
                                        UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                        CbiTccData.QJFXCmdMsg[j].ullId = id << 32 ^ id;
                                        Parameter.ulCbiToTccQJGFNum++;
                                    }
                                }
                                i += 4;
                                Cnt_R++;
                            }
                        }
                        CbiTccData.QJFXCmdMsgDB.DevNum = Cnt_R;
                        CbiTccData.QJFXCmdMsgDB.ByteLen = (UInt32)(sourceList[i].ByteOffSet - CbiTccData.QJFXCmdMsgDB.ByteOffset);

                        Cnt_R = 0;
                        CbiTccData.RouteMsgDB.ByteOffset = sourceList[i].ByteOffSet;
                        for (int j = 0; j < 40; j++)
                        {
                            if ((sourceList[i].MsgBlockType.Contains("进路信息")) || (sourceList[i].MsgBlockType.IsEmpty()))
                            {
                                for (int k = 0; k < 24; k++)
                                {
                                    var data = sourceList[i + k];
                                    CbiTccData.RouteMsg[j].MsgAdderss[k] = GetCommTccBaseStruct(data);
                                    if (!data.DeviceName.IsEmpty())
                                    {
                                        UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                        CbiTccData.RouteMsg[j].ullId = id << 32 ^ id;
                                        Parameter.ulCbiToTccJLuNum++;
                                    }
                                }
                                i += 24;
                                Cnt_R++;
                            }
                        }
                        CbiTccData.RouteMsgDB.DevNum = Cnt_R;
                        CbiTccData.RouteMsgDB.ByteLen = (UInt32)(sourceList[i].ByteOffSet - CbiTccData.RouteMsgDB.ByteOffset);

                        Cnt_R = 0;
                        CbiTccData.XHHDDSMsgDB.ByteOffset = sourceList[i].ByteOffSet;
                        for (int j = 0; j < 32; j++)
                        {
                            if ((sourceList[i].MsgBlockType.Contains("进站信号机红灯断丝显示")) || (sourceList[i].MsgBlockType.IsEmpty()))
                            {
                                for (int k = 0; k < 1; k++)
                                {
                                    var data = sourceList[i + k];
                                    CbiTccData.XHHDDSMsg[j].MsgAdderss[k] = GetCommTccBaseStruct(data);
                                    if (!data.DeviceName.IsEmpty())
                                    {
                                        UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                        CbiTccData.XHHDDSMsg[j].ullId = id << 32 ^ id;
                                        Parameter.ulCbiToTccXHHDDSNum++;
                                    }
                                }
                                i += 1;
                                Cnt_R++;
                            }
                        }
                        CbiTccData.XHHDDSMsgDB.DevNum = Cnt_R;
                        CbiTccData.XHHDDSMsgDB.ByteLen = (UInt32)(sourceList[i].ByteOffSet - CbiTccData.XHHDDSMsgDB.ByteOffset);

                        Cnt_R = 0;
                        CbiTccData.XHDCMsgDB.ByteOffset = sourceList[i].ByteOffSet;
                        for (int j = 0; j < 64; j++)
                        {
                            if ((sourceList[i].MsgBlockType.Contains("信号机调车状态")) || (sourceList[i].MsgBlockType.IsEmpty()))
                            {
                                for (int k = 0; k < 1; k++)
                                {
                                    var data = sourceList[i + k];
                                    CbiTccData.XHDCMsg[j].MsgAdderss[k] = GetCommTccBaseStruct(data);
                                    if (!data.DeviceName.IsEmpty())
                                    {
                                        UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                        CbiTccData.XHDCMsg[j].ullId = id << 32 ^ id;
                                        Parameter.ulCbiToTccDiaoCXHNum++;
                                    }
                                }
                                i += 1;
                                Cnt_R++;
                            }
                        }
                        CbiTccData.XHDCMsgDB.DevNum = Cnt_R;
                        CbiTccData.XHDCMsgDB.ByteLen = (UInt32)(sourceList[i].ByteOffSet - CbiTccData.XHDCMsgDB.ByteOffset);

                        Cnt_R = 0;
                        CbiTccData.XHKZMsgDB.ByteOffset = sourceList[i].ByteOffSet;
                        for (int j = 0; j < 20; j++)
                        {
                            if ((sourceList[i].MsgBlockType.Contains("信号机控制")) || (sourceList[i].MsgBlockType.IsEmpty()))
                            {
                                for (int k = 0; k < 4; k++)
                                {
                                    var data = sourceList[i + k];
                                    CbiTccData.XHKZMsg[j].MsgAdderss[k] = GetCommTccBaseStruct(data);
                                    if (!data.DeviceName.IsEmpty())
                                    {
                                        UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                        CbiTccData.XHKZMsg[j].ullId = id << 32 ^ id;
                                        Parameter.ulCbiToTccXHKZNum++;
                                    }
                                }
                                i += 4;
                                Cnt_R++;
                            }
                        }
                        CbiTccData.XHKZMsgDB.DevNum = Cnt_R;
                        CbiTccData.XHKZMsgDB.ByteLen = (UInt32)(sourceList[i].ByteOffSet - CbiTccData.XHKZMsgDB.ByteOffset);

                        Cnt_R = 0;
                        CbiTccData.CTReseivedMsgDB.ByteOffset = sourceList[i].ByteOffSet;
                        for (int j = 0; j < 1; j++)
                        {
                            if ((sourceList[i].MsgBlockType.Contains("预留")) || (sourceList[i].MsgBlockType.IsEmpty()))
                            {
                                for (int k = 0; k < 1; k++)
                                {
                                    var data = sourceList[i + k];
                                    CbiTccData.CTReseivedMsg[j].MsgAdderss[k] = GetCommTccBaseStruct(data);
                                    if (!data.DeviceName.IsEmpty())
                                    {
                                        UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                        CbiTccData.CTReseivedMsg[j].ullId = id << 32 ^ id;
                                        Parameter.ulCbiToTccResevedNum++;
                                    }
                                }
                                i += 1;
                                Cnt_R++;
                            }
                        }
                        CbiTccData.CTReseivedMsgDB.DevNum = Cnt_R;
                        CbiTccData.CTReseivedMsgDB.ByteLen = (UInt32)(sourceList[i].ByteOffSet - CbiTccData.CTReseivedMsgDB.ByteOffset);

                        Cnt_R = 0;
                        CbiTccData.InterfaceVerMsgDB.ByteOffset = sourceList[i].ByteOffSet;
                        for (int j = 0; j < 1; j++)
                        {
                            if ((sourceList[i].MsgBlockType.Contains("版本校验信息")) || (sourceList[i].MsgBlockType.IsEmpty()))
                            {
                                for (int k = 0; k < 2; k++)
                                {
                                    var data = sourceList[i + k];
                                    CbiTccData.InterfaceVerMsg[j].MsgAdderss[k] = GetCommTccBaseStruct(data);
                                    if (!data.DeviceName.IsEmpty())
                                    {
                                        UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                        CbiTccData.InterfaceVerMsg[j].ullId = id << 32 ^ id;
                                        Parameter.ulCbiToTccInterfaceNum++;
                                    }
                                }
                                i += 2;
                            }
                            Cnt_R++;
                        }
                        CbiTccData.InterfaceVerMsgDB.DevNum = Cnt_R;
                        CbiTccData.InterfaceVerMsgDB.ByteLen = (UInt32)(450 - CbiTccData.InterfaceVerMsgDB.ByteOffset);
                    }
                    else if (0xAA == DirectionFlag)
                    {
                        Cnt_R = 0;
                        TccCbiData.QJFXMsgDB.ByteOffset = sourceList[i].ByteOffSet;
                        for (int j = 0; j < 16; j++)
                        {
                            if ((sourceList[i].MsgBlockType.Contains("区间方向信息")) || (sourceList[i].MsgBlockType.IsEmpty()))
                            {
                                for (int k = 0; k < 5; k++)
                                {
                                    var data = sourceList[i + k];
                                    TccCbiData.QJFXMsg[j].MsgAdderss[k] = GetCommTccBaseStruct(data);
                                    if (!data.DeviceName.IsEmpty())
                                    {
                                        UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                        TccCbiData.QJFXMsg[j].ullId = id << 32 ^ id;
                                        Parameter.ulTccToCbiQJFXNum++;
                                    }
                                }
                                i += 5;
                                Cnt_R++;
                            }
                        }
                        TccCbiData.QJFXMsgDB.DevNum = Cnt_R;
                        TccCbiData.QJFXMsgDB.ByteLen = (UInt32)(sourceList[i].ByteOffSet - TccCbiData.QJFXMsgDB.ByteOffset);

                        Cnt_R = 0;
                        TccCbiData.QJBSFQMsgDB.ByteOffset = sourceList[i].ByteOffSet;
                        for (int j = 0; j < 240; j++)
                        {
                            if ((sourceList[i].MsgBlockType.Contains("区间闭塞分区状态")) || (sourceList[i].MsgBlockType.IsEmpty()))
                            {
                                for (int k = 0; k < 1; k++)
                                {
                                    var data = sourceList[i + k];
                                    TccCbiData.QJBSFQMsg[j].MsgAdderss[k] = GetCommTccBaseStruct(data);
                                    if (!data.DeviceName.IsEmpty())
                                    {
                                        UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                        TccCbiData.QJBSFQMsg[j].ullId = id << 32 ^ id;
                                        Parameter.ulTccToCbiBSFQNum++;
                                    }
                                }
                                i += 1;
                                Cnt_R++;
                            }
                        }
                        TccCbiData.QJBSFQMsgDB.DevNum = Cnt_R;
                        TccCbiData.QJBSFQMsgDB.ByteLen = (UInt32)(sourceList[i].ByteOffSet - TccCbiData.QJBSFQMsgDB.ByteOffset);

                        Cnt_R = 0;
                        TccCbiData.XHXSMsgDB.ByteOffset = sourceList[i].ByteOffSet;
                        for (int j = 0; j < 40; j++)
                        {
                            if ((sourceList[i].MsgBlockType.Contains("信号限速信息")) || (sourceList[i].MsgBlockType.IsEmpty()))
                            {
                                for (int k = 0; k < 1; k++)
                                {
                                    var data = sourceList[i + k];
                                    TccCbiData.XHXSMsg[j].MsgAdderss[k] = GetCommTccBaseStruct(data);
                                    if (!data.DeviceName.IsEmpty())
                                    {
                                        UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                        TccCbiData.XHXSMsg[j].ullId = id << 32 ^ id;
                                        Parameter.ulTccToCbiBSFQNum++;
                                    }
                                }
                                i += 1;
                                Cnt_R++;
                            }
                        }
                        TccCbiData.XHXSMsgDB.DevNum = Cnt_R;
                        TccCbiData.XHXSMsgDB.ByteLen = (UInt32)(sourceList[i].ByteOffSet - TccCbiData.XHXSMsgDB.ByteOffset);

                        Cnt_R = 0;
                        TccCbiData.QJXHHDDSMsgDB.ByteOffset = sourceList[i].ByteOffSet;
                        for (int j = 0; j < 16; j++)
                        {
                            if ((sourceList[i].MsgBlockType.Contains("区间信号机红灯断丝信息")) || (sourceList[i].MsgBlockType.IsEmpty()))
                            {
                                for (int k = 0; k < 4; k++)
                                {
                                    var data = sourceList[i + k];
                                    TccCbiData.QJXHHDDS[j].MsgAdderss[k] = GetCommTccBaseStruct(data);
                                    if (!data.DeviceName.IsEmpty())
                                    {
                                        UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                        TccCbiData.QJXHHDDS[j].ullId = id << 32 ^ id;
                                        Parameter.ulTccToCbiQJXHHDNum++;
                                    }
                                }
                                i += 4;
                                Cnt_R++;
                            }
                        }
                        TccCbiData.QJXHHDDSMsgDB.DevNum = Cnt_R;
                        TccCbiData.QJXHHDDSMsgDB.ByteLen = (UInt32)(sourceList[i].ByteOffSet - TccCbiData.QJXHHDDSMsgDB.ByteOffset);

                        Cnt_R = 0;
                        TccCbiData.YWQXMsgDB.ByteOffset = sourceList[i].ByteOffSet;
                        for (int j = 0; j < 120; j++)
                        {
                            if ((sourceList[i].MsgBlockType.Contains("异物侵限信息")) || (sourceList[i].MsgBlockType.IsEmpty()))
                            {
                                for (int k = 0; k < 1; k++)
                                {
                                    var data = sourceList[i + k];
                                    TccCbiData.YWQXMsg[j].MsgAdderss[k] = GetCommTccBaseStruct(data);
                                    if (!data.DeviceName.IsEmpty())
                                    {
                                        UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                        TccCbiData.YWQXMsg[j].ullId = id << 32 ^ id;
                                        Parameter.ulTccToCbiYWQXNum++;
                                    }
                                }
                                i += 1;
                                Cnt_R++;
                            }
                        }
                        TccCbiData.YWQXMsgDB.DevNum = Cnt_R;
                        TccCbiData.YWQXMsgDB.ByteLen = (UInt32)(sourceList[i].ByteOffSet - TccCbiData.YWQXMsgDB.ByteOffset);

                        Cnt_R = 0;
                        TccCbiData.WPXQJFXMsgDB.ByteOffset = sourceList[i].ByteOffSet;
                        for (int j = 0; j < 16; j++)
                        {
                            if ((sourceList[i].MsgBlockType.Contains("无配线站区间方向信息")) || (sourceList[i].MsgBlockType.IsEmpty()))
                            {
                                for (int k = 0; k < 3; k++)
                                {
                                    var data = sourceList[i + k];
                                    TccCbiData.WPXQJFXMsg[j].MsgAdderss[k] = GetCommTccBaseStruct(data);
                                    if (!data.DeviceName.IsEmpty())
                                    {
                                        UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                        TccCbiData.WPXQJFXMsg[j].ullId = id << 32 ^ id;
                                        Parameter.ulTccToCbiWPXQJFXNum++;
                                    }
                                }
                                i += 3;
                                Cnt_R++;
                            }
                        }
                        TccCbiData.WPXQJFXMsgDB.DevNum = Cnt_R;
                        TccCbiData.WPXQJFXMsgDB.ByteLen = (UInt32)(sourceList[i].ByteOffSet - TccCbiData.WPXQJFXMsgDB.ByteOffset);

                        Cnt_R = 0;
                        TccCbiData.WPXGDZTMsgDB.ByteOffset = sourceList[i].ByteOffSet;
                        for (int j = 0; j < 128; j++)
                        {
                            if ((sourceList[i].MsgBlockType.Contains("无配线站及站内轨道状态信息")) || (sourceList[i].MsgBlockType.IsEmpty()))
                            {
                                for (int k = 0; k < 1; k++)
                                {
                                    var data = sourceList[i + k];
                                    TccCbiData.WPXGDZTMsg[j].MsgAdderss[k] = GetCommTccBaseStruct(data);
                                    if (!data.DeviceName.IsEmpty())
                                    {
                                        UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                        TccCbiData.WPXGDZTMsg[j].ullId = id << 32 ^ id;
                                        Parameter.ulTccToCbiWPXGDNum++;
                                    }
                                }
                                i += 1;
                                Cnt_R++;
                            }
                        }
                        TccCbiData.WPXGDZTMsgDB.DevNum = Cnt_R;
                        TccCbiData.WPXGDZTMsgDB.ByteLen = (UInt32)(sourceList[i].ByteOffSet - TccCbiData.WPXGDZTMsgDB.ByteOffset);

                        Cnt_R = 0;
                        TccCbiData.WPXXHHDDSMsgDB.ByteOffset = sourceList[i].ByteOffSet;
                        for (int j = 0; j < 16; j++)
                        {
                            if ((sourceList[i].MsgBlockType.Contains("无配线站信号机红灯断丝信息")) || (sourceList[i].MsgBlockType.IsEmpty()))
                            {
                                for (int k = 0; k < 4; k++)
                                {
                                    var data = sourceList[i + k];
                                    TccCbiData.WPXXHHDDSMsg[j].MsgAdderss[k] = GetCommTccBaseStruct(data);
                                    if (!data.DeviceName.IsEmpty())
                                    {
                                        UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                        TccCbiData.WPXXHHDDSMsg[j].ullId = id << 32 ^ id;
                                        Parameter.ulTccToCbiWPXXHDSNum++;
                                    }
                                }
                                i += 4;
                                Cnt_R++;
                            }
                        }
                        TccCbiData.WPXXHHDDSMsgDB.DevNum = Cnt_R;
                        TccCbiData.WPXXHHDDSMsgDB.ByteLen = (UInt32)(sourceList[i].ByteOffSet - TccCbiData.WPXXHHDDSMsgDB.ByteOffset);

                        Cnt_R = 0;
                        TccCbiData.XHCJMsgDB.ByteOffset = sourceList[i].ByteOffSet;
                        for (int j = 0; j < 20; j++)
                        {
                            if ((sourceList[i].MsgBlockType.Contains("信号机采集")) || (sourceList[i].MsgBlockType.IsEmpty()))
                            {
                                for (int k = 0; k < 6; k++)
                                {
                                    var data = sourceList[i + k];
                                    TccCbiData.XHCJMsg[j].MsgAdderss[k] = GetCommTccBaseStruct(data);
                                    if (!data.DeviceName.IsEmpty())
                                    {
                                        UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                        TccCbiData.XHCJMsg[j].ullId = id << 32 ^ id;
                                        Parameter.ulTccToCbiXHCJNum++;
                                    }
                                }
                                i += 6;
                                Cnt_R++;
                            }
                        }
                        TccCbiData.XHCJMsgDB.DevNum = Cnt_R;
                        TccCbiData.XHCJMsgDB.ByteLen = (UInt32)(sourceList[i].ByteOffSet - TccCbiData.XHCJMsgDB.ByteOffset);

                        Cnt_R = 0;
                        TccCbiData.TCReseivedMsgDB.ByteOffset = sourceList[i].ByteOffSet;
                        for (int j = 0; j < 1; j++)
                        {
                            if ((sourceList[i].MsgBlockType.Contains("预留")) || (sourceList[i].MsgBlockType.IsEmpty()))
                            {
                                for (int k = 0; k < 1; k++)
                                {
                                    var data = sourceList[i + k];
                                    TccCbiData.TCReseivedMsg[j].MsgAdderss[k] = GetCommTccBaseStruct(data);
                                    if (!data.DeviceName.IsEmpty())
                                    {
                                        UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                        TccCbiData.TCReseivedMsg[j].ullId = id << 32 ^ id;
                                        Parameter.ulTccToCbiResevedNum++;
                                    }
                                }
                                i += 1;
                                Cnt_R++;
                            }
                        }
                        TccCbiData.TCReseivedMsgDB.DevNum = Cnt_R;
                        TccCbiData.TCReseivedMsgDB.ByteLen = (UInt32)(sourceList[i].ByteOffSet - TccCbiData.TCReseivedMsgDB.ByteOffset);

                        Cnt_R = 0;
                        TccCbiData.InterfaceVersionDB.ByteOffset = sourceList[i].ByteOffSet;
                        for (int j = 0; j < 1; j++)
                        {
                            if ((sourceList[i].MsgBlockType.Contains("版本校验信息")) || (sourceList[i].MsgBlockType.IsEmpty()))
                            {
                                for (int k = 0; k < 2; k++)
                                {
                                    var data = sourceList[i + k];
                                    TccCbiData.InterfaceVersion[j].MsgAdderss[k] = GetCommTccBaseStruct(data);
                                    if (!data.DeviceName.IsEmpty())
                                    {
                                        UInt64 id = data.DeviceID << 16 ^ data.DeviceID;
                                        TccCbiData.InterfaceVersion[j].ullId = id << 32 ^ id;
                                        Parameter.ulTccToCbiInterfaceNum++;
                                    }
                                }
                                i += 2;
                                Cnt_R++;
                            }
                        }
                        TccCbiData.InterfaceVersionDB.DevNum = Cnt_R;
                        TccCbiData.InterfaceVersionDB.ByteLen = (UInt32)(300 - TccCbiData.InterfaceVersionDB.ByteOffset);
                    }
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
            data.BitOffset = (item.BitOffSet + item.Width - 1) % 8;
            data.Width = item.Width;

            return data;
        }
        #endregion

    }
    #region CommCbiTccClass
    public class CommCbiTccParameter
    {
        public string SegmentName { get; set; }
        public UInt32 StructWidth { get; set; }
        public UInt32 TotalStruct { get; set; }

        public UInt64 ullID { get; set; }
        public UInt32 usMaxSilentCyclesTcc { get; set; }
        public UInt32 usMaxTsnSkip { get; set; }
        public UInt32 usMaxRsnSkip { get; set; }
        public UInt32 usMaxSyncInteval { get; set; }
        public UInt32 usMinSyncInteval { get; set; }
        public UInt32 usTccCycleTime { get; set; }
        public UInt32 usInitializedTime { get; set; }
        public UInt32 ulCbiToTccQJGFNum { get; set; }
        public UInt32 ulCbiToTccJLuNum { get; set; }
        public UInt32 ulCbiToTccXHHDDSNum { get; set; }
        public UInt32 ulCbiToTccDiaoCXHNum { get; set; }
        public UInt32 ulCbiToTccXHKZNum { get; set; }
        public UInt32 ulCbiToTccResevedNum { get; set; }
        public UInt32 ulCbiToTccInterfaceNum { get; set; }
        public UInt32 ulTccToCbiQJFXNum { get; set; }
        public UInt32 ulTccToCbiBSFQNum { get; set; }
        public UInt32 ulTccToCbiXHXSNum { get; set; }
        public UInt32 ulTccToCbiQJXHHDNum { get; set; }
        public UInt32 ulTccToCbiYWQXNum { get; set; }
        public UInt32 ulTccToCbiWPXQJFXNum { get; set; }
        public UInt32 ulTccToCbiWPXGDNum { get; set; }
        public UInt32 ulTccToCbiWPXXHDSNum { get; set; }
        public UInt32 ulTccToCbiXHCJNum { get; set; }
        public UInt32 ulTccToCbiResevedNum { get; set; }
        public UInt32 ulTccToCbiInterfaceNum { get; set; }

    }

    public class CommCbiTccStructData
    {
        public DataBlockAddress QJFXCmdMsgDB { get; set; }
        public List<CbiTcc_A4> QJFXCmdMsg { get; set; }
        public DataBlockAddress RouteMsgDB { get; set; }
        public List<CbiTcc_A24> RouteMsg { get; set; }
        public DataBlockAddress XHHDDSMsgDB { get; set; }
        public List<CbiTcc_A1> XHHDDSMsg { get; set; }
        public DataBlockAddress XHDCMsgDB { get; set; }
        public List<CbiTcc_A1> XHDCMsg { get; set; }
        public DataBlockAddress XHKZMsgDB { get; set; }
        public List<CbiTcc_A4> XHKZMsg { get; set; }
        public DataBlockAddress CTReseivedMsgDB { get; set; }
        public List<CbiTcc_A1> CTReseivedMsg { get; set; }
        public DataBlockAddress InterfaceVerMsgDB { get; set; }
        public List<CbiTcc_A2> InterfaceVerMsg { get; set; }

        public CommCbiTccStructData()
        {
            this.QJFXCmdMsgDB = new DataBlockAddress();
            this.RouteMsgDB = new DataBlockAddress();
            this.XHHDDSMsgDB = new DataBlockAddress();
            this.XHDCMsgDB = new DataBlockAddress();
            this.XHKZMsgDB = new DataBlockAddress();
            this.CTReseivedMsgDB = new DataBlockAddress();
            this.InterfaceVerMsgDB = new DataBlockAddress();

            this.QJFXCmdMsg = new List<CbiTcc_A4>();
            this.RouteMsg = new List<CbiTcc_A24>();
            this.XHHDDSMsg = new List<CbiTcc_A1>();
            this.XHDCMsg = new List<CbiTcc_A1>();
            this.XHKZMsg = new List<CbiTcc_A4>();
            this.CTReseivedMsg = new List<CbiTcc_A1>();
            this.InterfaceVerMsg = new List<CbiTcc_A2>();

            CbiTcc_A1 Reseived = new CbiTcc_A1();
            this.CTReseivedMsg.Add(Reseived);
            CbiTcc_A2 A2 = new CbiTcc_A2();
            this.InterfaceVerMsg.Add(A2);

            for (int i = 0; i < 16; i++)
            {
                CbiTcc_A4 A4 = new CbiTcc_A4();
                this.QJFXCmdMsg.Add(A4);
            }

            for (int i = 0; i < 40; i++)
            {
                CbiTcc_A24 A24 = new CbiTcc_A24();
                this.RouteMsg.Add(A24);
            }

            for (int i = 0; i < 32; i++)
            {
                CbiTcc_A1 A1 = new CbiTcc_A1();
                this.XHHDDSMsg.Add(A1);
            }

            for (int i = 0; i < 64; i++)
            {
                CbiTcc_A1 A1 = new CbiTcc_A1();
                this.XHDCMsg.Add(A1);
            }

            for (int i = 0; i < 20; i++)
            {
                CbiTcc_A4 A4 = new CbiTcc_A4();
                this.XHKZMsg.Add(A4);
            }
        }
    }
    public class CommTccCbiStructData
    {
        public DataBlockAddress QJFXMsgDB { get; set; }
        public List<CbiTcc_A5> QJFXMsg { get; set; }
        public DataBlockAddress QJBSFQMsgDB { get; set; }
        public List<CbiTcc_A1> QJBSFQMsg { get; set; }
        public DataBlockAddress XHXSMsgDB { get; set; }
        public List<CbiTcc_A1> XHXSMsg { get; set; }
        public DataBlockAddress QJXHHDDSMsgDB { get; set; }
        public List<CbiTcc_A4> QJXHHDDS { get; set; }
        public DataBlockAddress YWQXMsgDB { get; set; }
        public List<CbiTcc_A1> YWQXMsg { get; set; }
        public DataBlockAddress WPXQJFXMsgDB { get; set; }
        public List<CbiTcc_A3> WPXQJFXMsg { get; set; }
        public DataBlockAddress WPXGDZTMsgDB { get; set; }
        public List<CbiTcc_A1> WPXGDZTMsg { get; set; }
        public DataBlockAddress WPXXHHDDSMsgDB { get; set; }
        public List<CbiTcc_A4> WPXXHHDDSMsg { get; set; }
        public DataBlockAddress XHCJMsgDB { get; set; }
        public List<CbiTcc_A6> XHCJMsg { get; set; }
        public DataBlockAddress TCReseivedMsgDB { get; set; }
        public List<CbiTcc_A1> TCReseivedMsg { get; set; }
        public DataBlockAddress InterfaceVersionDB { get; set; }
        public List<CbiTcc_A2> InterfaceVersion { get; set; }

        public CommTccCbiStructData()
        {
            this.QJFXMsgDB = new DataBlockAddress();
            this.QJBSFQMsgDB = new DataBlockAddress();
            this.XHXSMsgDB = new DataBlockAddress();
            this.QJXHHDDSMsgDB = new DataBlockAddress();
            this.YWQXMsgDB = new DataBlockAddress();
            this.WPXQJFXMsgDB = new DataBlockAddress();
            this.WPXGDZTMsgDB = new DataBlockAddress();
            this.WPXXHHDDSMsgDB = new DataBlockAddress();
            this.XHCJMsgDB = new DataBlockAddress();
            this.TCReseivedMsgDB = new DataBlockAddress();
            this.InterfaceVersionDB = new DataBlockAddress();

            this.QJFXMsg = new List<CbiTcc_A5>();
            this.QJBSFQMsg = new List<CbiTcc_A1>();
            this.XHXSMsg = new List<CbiTcc_A1>();
            this.QJXHHDDS = new List<CbiTcc_A4>();
            this.YWQXMsg = new List<CbiTcc_A1>();
            this.WPXQJFXMsg = new List<CbiTcc_A3>();
            this.WPXGDZTMsg = new List<CbiTcc_A1>();
            this.WPXXHHDDSMsg = new List<CbiTcc_A4>();
            this.XHCJMsg = new List<CbiTcc_A6>();
            this.TCReseivedMsg = new List<CbiTcc_A1>();
            this.InterfaceVersion = new List<CbiTcc_A2>();

            CbiTcc_A1 Reseived = new CbiTcc_A1();
            this.TCReseivedMsg.Add(Reseived);
            CbiTcc_A2 A2 = new CbiTcc_A2();
            this.InterfaceVersion.Add(A2);

            for (int i = 0; i < 16; i++)
            {
                CbiTcc_A4 ds = new CbiTcc_A4();
                this.QJXHHDDS.Add(ds);
                CbiTcc_A4 A4 = new CbiTcc_A4();
                this.WPXXHHDDSMsg.Add(A4);
                CbiTcc_A5 A5 = new CbiTcc_A5();
                this.QJFXMsg.Add(A5);
                CbiTcc_A3 A3 = new CbiTcc_A3();
                this.WPXQJFXMsg.Add(A3);
            }

            for (int i = 0; i < 40; i++)
            {
                CbiTcc_A1 A1 = new CbiTcc_A1();
                this.XHXSMsg.Add(A1);
            }

            for (int i = 0; i < 20; i++)
            {
                CbiTcc_A6 A6 = new CbiTcc_A6();
                this.XHCJMsg.Add(A6);
            }

            for (int i = 0; i < 240; i++)
            {
                CbiTcc_A1 A1 = new CbiTcc_A1();
                this.QJBSFQMsg.Add(A1);
            }
            for (int i = 0; i < 120; i++)
            {
                CbiTcc_A1 A1 = new CbiTcc_A1();
                this.YWQXMsg.Add(A1);
            }
            for (int i = 0; i < 128; i++)
            {
                CbiTcc_A1 A1 = new CbiTcc_A1();
                this.WPXGDZTMsg.Add(A1);
            }
        }
    }
    #endregion
}
