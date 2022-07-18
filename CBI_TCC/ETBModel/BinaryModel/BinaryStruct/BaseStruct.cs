using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using JSDG.CBI.A.ETBProcess;
using JSDG.CBI.A.ETBUtil;
using JSDG.CBI.A.ETBAttribute;

namespace JSDG.CBI.A.ETBModel
{
    public abstract class BaseStruct
    {
        #region const
         /// <summary>
         /// true
         /// </summary>
        public const UInt32 CONST_TRUE = 0xFFFFFFFF;

        /// <summary>
        /// false
        /// </summary>
        public const UInt32 CONST_FALSE = 0x0;

        /// <summary>
        /// 无符号32位整型默认值
        /// </summary>
        public const UInt32 CONST_DEFAULT = 0xFFFFFFFF;

        /// <summary>
        /// DEVICE_ID默认值
        /// </summary>
        public DEVICE_ID CONST_DEVICE_ID = new DEVICE_ID { Id = CONST_DEFAULT, Index = CONST_DEFAULT };

        // 信号机类型
        public const UInt32 CONST_SIGNAL_RECEIVE_OR_ROUTE               = 0xD4D4D4D4U;
        public const UInt32 CONST_SIGNAL_EXIT                           = 0xD5D5D5D5U;        
        public const UInt32 CONST_SIGNAL_SHUNTING                       = 0xD1D1D1D1U;
        public const UInt32 CONST_SIGNAL_EXIT_DOUBLEGREEN               = 0xD6D6D6D6U;
        public const UInt32 CONST_SIGNAL_EXIT_REVERSE_INDICATOR         = 0xD7D7D7D7U;
        public const UInt32 CONST_SIGNAL_TRACK_SPACING                  = 0xD0D0D0D0U;
        public const UInt32 CONST_SIGNAL_HUMP_AUX                       = 0xEFEFEFEFU;
        public const UInt32 CONST_SIGNAL_TERMINAL                       = 0xD3D3D3D3U;

        //信号机方向(同进路方向)
        public const UInt32 CONST_DIR_UP                                = 0xBEBEBEBEU;//上行GD0
        public const UInt32 CONST_DIR_DOWN                              = 0xBDBDBDBDU;//下行GD1

        // 信号机Aspect
        public const UInt32 CONST_SIGS_RED                              = 0x30303030U;//H
        public const UInt32 CONST_SIGS_BURNOUT                          = 0x31313131U;//FB
        public const UInt32 CONST_SIGS_LIGHTOFF                         = 0x32323232U;
        public const UInt32 CONST_SIGS_WHITE                            = 0x33333333U;//B
        public const UInt32 CONST_SIGS_GREEN                            = 0x34343434U;//L
        public const UInt32 CONST_SIGS_CALLON                           = 0x35353535U;//Y
        public const UInt32 CONST_SIGS_YELLOW                           = 0x36363636U;//U
        public const UInt32 CONST_SIGS_DOUBLE_YELLOW                    = 0x37373737U;//UU
        public const UInt32 CONST_SIGS_BLUE                             = 0x38383838U;//A
        public const UInt32 CONST_SIGS_GREEN_YELLOW                     = 0x39393939U;//LU
        public const UInt32 CONST_SIGS_DOUBLE_GREEN                     = 0x3A3A3A3AU;//LL
        public const UInt32 CONST_SIGS_USU                              = 0x3F3F3F3FU;//LL
        public const UInt32 CONST_SIGS_SHUNTING_BS                      = 0x44444444U;//BS
        public const UInt32 CONST_SIGS_HUMP_HS                          = 0x46464646U;//HS
        public const UInt32 CONST_SIGS_HUMP_US                          = 0x48484848U;//US
        public const UInt32 CONST_SIGS_HUMP_LS                          = 0x4A4A4A4AU;//LS


        //信号机显示类型
        public const UInt32 CONST_DISPLAY_CAN_OFF_LIGHT                 = 0xCCCCCCCCU;
        public const UInt32 CONST_DISPLAY_CAN_ON_OFFLIGHT               = 0xCDCDCDCDU;
        public const UInt32 CONST_DISPLAY_LIGHT_ONLY                    = 0xCECECECEU;

        //区段类型
        public const UInt32 CONST_BLOCK_TRACK_UP                        = 0xD9D9D9D9U;//上行股道
        public const UInt32 CONST_BLOCK_TRACK_DOWN                      = 0xDADADADAU;//下行股道
        public const UInt32 CONST_BLOCK_TRACK                           = 0xDBDBDBDBU;//双方向股道
        public const UInt32 CONST_BLOCK_SWITCH                          = 0xDCDCDCDCU; //道岔区段
        public const UInt32 CONST_BLOCK_NO_SWITCH                       = 0xDDDDDDDDU;//无岔区段
        public const UInt32 CONST_BLOCK_TERMINAL                        = 0xDEDEDEDEU;//尽头线
        public const UInt32 CONST_BLOCK_APPROACH                        = 0xDFDFDFDFU;//接近区段
        public const UInt32 CONST_BLOCK_LEAVE                           = 0xE0E0E0E0U;//离去区段
        public const UInt32 CONST_BLOCK_LAMP                            = 0xE1E1E1E1U;//灯泡线
        public const UInt32 CONST_TRACK_MIDDLE_SWITCH                   = 0xE3E3E3E3U;//中岔道岔区段
        public const UInt32 CONST_TRACK_NO_SWITCH                       = 0xE4E4E4E4U;//中岔无岔区段
        public const UInt32 CONST_BLOCK_YARD                            = 0xE2E2E2E2U;//区间轨道

        //接口类型
        public const UInt32 CONST_INTERFACE_RELAY                       = 0x44004400U;
        public const UInt32 CONST_INTERFACE_CBI_TCC                     = 0x44114411U;
        public const UInt32 CONST_INTERFACE_TCC_TCC                     = 0x44224422U;
        public const UInt32 CONST_INTERFACE_CBI_CBI                     = 0x44334433U;

        //道岔类型
        public const UInt32 CONST_SWS_NORMAL                            = 0x5B5B5B5BU;
        public const UInt32 CONST_SWS_REVERSE                           = 0x5C5C5C5CU;

        //进路位置
        public const UInt32 CONST_ROUTETYPE_SHUNTING                    = 0xC2C2C2C2U;
        public const UInt32 CONST_ROUTETYPE_DEPARTURE                   = 0xC6C6C6C6U;
        public const UInt32 CONST_ROUTETYPE_RECEIVE                     = 0xC7C7C7C7U;

        /// <summary>
        /// 指示器类型
        /// </summary>
        public const UInt32 INDICATOR_FX_ON                             = 0x4D4D4D4DU;
        public const UInt32 INDICATOR_A_ON                              = 0x4E4E4E4EU;
        public const UInt32 INDICATOR_B_ON                              = 0x4F4F4F4FU;
        public const UInt32 INDICATOR_C_ON                              = 0x50505050U;
        public const UInt32 INDICATOR_D_ON                              = 0x51515151U;
        public const UInt32 INDICATOR_E_ON                              = 0x52525252U;
        public const UInt32 INDICATOR_F_ON                              = 0x53535353U;

        /// <summary>
        /// 其它联锁类型
        /// </summary>
        public const UInt32 OL_INVALID                                  = 0xF5F5F5F5U;
        public const UInt32 OL_SPEC_COND                                = 0xF6F6F6F6U;
        public const UInt32 OL_RA                                       = 0xF7F7F7F7U;
        public const UInt32 OL_SAB                                      = 0xF8F8F8F8U;
        public const UInt32 OL_SUCCESSIVE                               = 0xF9F9F9F9U;
        public const UInt32 OL_LIAISON                                  = 0xFAFAFAFAU;
        public const UInt32 OL_JK                                       = 0x55005500U;
        public const UInt32 OL_F                                        = 0x55115511U;
        public const UInt32 OL_AGREE                                    = 0x55225522U;

        /// <summary>
        /// 是否输入点  
         /// </summary>
         public const UInt32 CONST_SPEICALIO_INPUT                          = 0xE5E5E5E5U;
         public const UInt32 CONST_SPEICALIO_OUTPUT                         = 0xE6E6E6E6U;

         /// <summary>
         /// 联系口类型
         /// </summary>
         public const UInt32 LIAISON_DOUBLE_SIGNAL_DISPLAY                  = 0x11661166U;
         public const UInt32 LIAISON_TYPE_YARD                              = 0x11771177U;
         public const UInt32 LIAISON_TYPE_YARD_NO_SWITCH_AGREE              = 0x11881188U;
         public const UInt32 LIAISON_TYPE_YARD_WITH_SWITCH_AGREE            = 0x11991199U;
         public const UInt32 LIAISON_TYPE_AUTO_BLOCKING_TYPE1               = 0x11AA11AAU;
         public const UInt32 LIAISON_TYPE_AUTO_BLOCKING_TYPE2               = 0x11BB11BBU;
         public const UInt32 LIAISON_TYPE_AUTO_BLOCKING_TYPE3               = 0x11CC11CCU;
         public const UInt32 LIAISON_TYPE_AUTO_BLOCKING_TYPE4               = 0x11DD11DDU;
         public const UInt32 LIAISON_TYPE_AUTO_STATION_BLOCKING_TYPE1       = 0x11EE11EEU;
         public const UInt32 LIAISON_TYPE_AUTO_STATION_BLOCKING_TYPE2       = 0x11FF11FFU;
         public const UInt32 LIAISON_TYPE_SEMI_AUTO_BLOCKING                = 0x22002200U;
         public const UInt32 LIAISON_TYPE_SINGLE_STATION                    = 0x22112211U;
         public const UInt32 LIAISON_TYPE_DOUBLE_STATION                    = 0x22332233U;
         public const UInt32 LIAISON_TYPE_MECHNICAL_HUMP                    = 0x22442244U;

        /// <summary>
        /// speicalIo类型 计轴电源故障  
         /// </summary>
        public const UInt32 CONST_SPEICALIO_TYPE_ZCJ                        = 0x49494949U;
        public const UInt32 CONST_SPEICALIO_TYPE_ALEXSOURCE                 = 0x4A4A4A4AU;
        public const UInt32 CONST_SPEICALIO_TYPE_INDACTOR                   = 0x4B4B4B4BU;
        public const UInt32 CONST_SPEICALIO_TYPE_DEFAULT                    = 0x4C4C4C4CU;
        public const UInt32 CONST_SPEICALIO_TYPE_TD                         = 0x99229922U;
        public const UInt32 CONST_SPEICALIO_TYPE_FAULT                      = 0x99119911U;

        /// <summary>
        /// 区间信号显示类型
        /// </summary>
        public const UInt32 THREE_ASPECT = 0x00000000;//无效值
        public const UInt32 FOUR_ASPECT = 0x50505050;//特殊条件

        #endregion

        protected DataModel AllDataModel;
        public abstract void Transform();
        public WaysideInfo WaysideInfo;

        //protected BaseStruct(DataModel dataModel)
        //{
        //    UnionRoute(dataModel.InterLockData.RouteList, dataModel.InterLockData.ShuttingRouteList);
        //    AllDataModel = dataModel;
        //}

        /// <summary>
        /// 检查单控区设备数量是否超过限值
        /// </summary>
        protected virtual void CheckMaxNum<T>(List<T> list)
        {
            ExcelModelClassBaseAttribute att = typeof(T).GetCustomAttribute<ExcelModelClassBaseAttribute>(false);
            int max = 0;
            if(AllDataModel.InterLockData.MaxNumDic.ContainsKey(att.CbiDataType.ToString()))
            {
                max = AllDataModel.InterLockData.MaxNumDic[att.CbiDataType.ToString()];
                if(list.Count() > max)
                {
                    throw new Exception(string.Format("[{1}]的设备数量超过上限[{2}]", att.SheetName, max.ToString()));
                }
            }
        }

        #region 获取CommAddress类

        /// <summary>
        /// 返回二进制结构的asInput和asOutput
        /// </summary>
        protected virtual CommAddress[] LoadCommAddress(string devName, int length)
        {
            var putIO = new CommAddress[length];

            for (int i = 0; i < length; i++)
            {
                putIO[i] = new CommAddress();
            }

            return putIO;
        }

        #endregion

        #region 获取asInput和asOutput（CIOAddress[]类型）

        /// <summary>
        /// 返回二进制结构的asInput和asOutput
        /// </summary>
        /// <param name="devName">设备名</param>
        /// <param name="zoneName">当前控区</param>
        /// <param name="type">输入输出类型</param>
        /// <param name="ioList">继电器列表</param>
        //protected virtual CIOAddress[] LoadIOmapsBase(string devName, IOType type, List<string> ioList)
        //{
        //    var putIO = new CIOAddress[ioList.Count];

        //    for (int i = 0; i < putIO.Length; i++)
        //    {
        //        putIO[i] = new CIOAddress();
        //    }

        //    try
        //    {
        //        bool[] flags = new bool[ioList.Count()];
        //        var iOMaps = AllDataModel.IOMapList.Where(x => x.IOType == type).ToList();
        //        foreach (var io in iOMaps)
        //        {
        //            for (int i = 1; i <= 16; i++)
        //            {
        //                var nullable = io.RelayDictionary[i];
        //                if (null == nullable)
        //                {
        //                    continue;
        //                }
        //                if (!devName.Equals(nullable.Value.Key))
        //                {
        //                    continue;
        //                }

        //                if (ioList.Contains(nullable.Value.Value))
        //                {
        //                    int index = ioList.IndexOf(nullable.Value.Value);

        //                    putIO[index] = new CIOAddress();
        //                    putIO[index].ByteOffset = (byte)(2 * iOMaps.IndexOf(io) + (i - 1) / 8);
        //                    putIO[index].BitOffset = (byte)((i - 1) - ((i - 1) / 8) * 8);

        //                    flags[index] = true;
        //                }
        //                else
        //                {
        //                    throw new Exception(string.Format("设备[{0}]的继电器[{1}]不属于配置继电器", nullable.Value.Key, nullable.Value.Value));
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.ToString());
        //    }

        //    return putIO;
        //}

        #endregion

        #region 获取设备的DEVICE_ID

        protected DEVICE_ID[] GetDEVICE_IDArray<T>(string deviceName, List<T> list, int length) where T : InterLockBase
        {
            DEVICE_ID[] device_IDArray = new DEVICE_ID[length];
            var nameList = new List<string>();

            for (int i = 0; i < length; i++)
            {
                device_IDArray[i] = CONST_DEVICE_ID;
            }

            if (deviceName == "" || deviceName.SplitToList().Count() == 0)
            {
                return device_IDArray;
            }
            else
            {
                nameList = deviceName.SplitToList();
            }

            if (deviceName.SplitToList().Count() > length)
            {
                throw new Exception(string.Format("设备{0}的数量超过生成数据所需设备的上限{1}", deviceName, length.ToString()));
            }
            else
            {
                foreach (var name in nameList)
                {
                    device_IDArray[nameList.IndexOf(name)] = GetDEVICE_ID(name, list);
                }
            }

            return device_IDArray;
        }

        protected DEVICE_ID GetDEVICE_ID<T>(string deviceName, List<T> list, UInt32 id = UInt32.MaxValue) where T : InterLockBase
        {
            DEVICE_ID device_ID = new DEVICE_ID();
            if (id != UInt32.MaxValue)
            {
                device_ID.Id = list.FirstOrDefault(x => x.Id == id).Id << 16 ^ list.FirstOrDefault(x => x.Id == id).Id;
                device_ID.Index = (UInt32)list.Select(x => x.Id).ToList().IndexOf(id);
            }
            else if (string.IsNullOrWhiteSpace(deviceName))
            {
                return CONST_DEVICE_ID;
            }
            else
            {
                device_ID.Id = list.FirstOrDefault(x => x.Name == deviceName).Id << 16 ^ list.FirstOrDefault(x => x.Name == deviceName).Id;
                device_ID.Index = (UInt32)list.Select(x => x.Name).ToList().IndexOf(deviceName);
            }

            return device_ID;
        }

        #endregion

        #region 部分字段获取

        /// <summary>
        /// 获取灯色
        /// </summary>
        protected UInt32[] GetSignalLight(string colorString, int length)
        {
            var colorList = colorString.SplitToList();
            if (colorList.Count() > length)
            {
                throw new Exception(string.Format("{0}的灯色个数超过最大值", colorString));
            }

            UInt32[] result = new UInt32[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = CONST_DEFAULT;
            }

            if (string.IsNullOrEmpty(colorString))
            {
                return result;
            }

            foreach (var color in colorList)
            {
                switch (color)
                {
                    case "L":
                        result[colorList.IndexOf(color)] = CONST_SIGS_GREEN;
                        break;
                    case "Y":
                        result[colorList.IndexOf(color)] = CONST_SIGS_CALLON;
                        break;
                    case "U":
                        result[colorList.IndexOf(color)] = CONST_SIGS_YELLOW;
                        break;
                    case "A":
                        result[colorList.IndexOf(color)] = CONST_SIGS_BLUE;
                        break;
                    case "B":
                        result[colorList.IndexOf(color)] = CONST_SIGS_WHITE;
                        break;
                    case "H":
                        result[colorList.IndexOf(color)] = CONST_SIGS_RED;
                        break;
                    case "FB":
                        result[colorList.IndexOf(color)] = CONST_SIGS_BURNOUT;
                        break;
                    case "UU":
                        result[colorList.IndexOf(color)] = CONST_SIGS_DOUBLE_YELLOW;
                        break;
                    case "LU":
                        result[colorList.IndexOf(color)] = CONST_SIGS_GREEN_YELLOW;
                        break;
                    case "LL":
                        result[colorList.IndexOf(color)] = CONST_SIGS_DOUBLE_GREEN;
                        break;
                    case "USU":
                        result[colorList.IndexOf(color)] = CONST_SIGS_USU;
                        break;
                    case "BS":
                        result[colorList.IndexOf(color)] = CONST_SIGS_SHUNTING_BS;
                        break;
                    case "HS":
                        result[colorList.IndexOf(color)] = CONST_SIGS_HUMP_HS;
                        break;
                    case "LS":
                        result[colorList.IndexOf(color)] = CONST_SIGS_HUMP_LS;
                        break;
                    case "US":
                        result[colorList.IndexOf(color)] = CONST_SIGS_HUMP_US;
                        break;
                    //default:
                    //    throw new Exception(string.Format("转换信号机灯色异常:无法转换字符串[{0}]", colorString));
                }
            }

            return result;
        }

        /// <summary>
        /// 获取咽喉区
        /// </summary>
        protected UInt32 GetThroat(string throatString)
        {
            UInt32 result = CONST_DEFAULT;
            switch (throatString)
            {
                case "X":
                    result = 0X000FU;
                    break;
                case "S":
                    result = 0x00F0U;
                    break;
                case "Q":
                    result = 0x0F00U;
                    break;
                default:
                    throw new Exception(string.Format("咽喉区[{0}]为非法值", throatString));
            }

            return result;
        }

        /// <summary>
        /// 获取联系口类型
        /// </summary>
        /// <param name="condition">联锁条件，即联系口</param>
        //protected UInt32 GetLiaisonType(string condition)
        //{
        //    UInt32 result = CONST_DEFAULT;

        //    var CommCI = AllDataModel.InterLockData.CLCommonList.Select(x => x.ContactInterface).ToList();
        //    var AgreeCI = AllDataModel.InterLockData.CLAgreeSwitchList.Select(x => x.ContactInterface).ToList();
        //    var partCI = AllDataModel.InterLockData.CLSwitchPartList.Select(x => x.ContactInterface).ToList();

        //    //if ((AgreeCI.Contains(condition) && partCI != null) || (partCI.Contains(condition) && AgreeCI != null))
        //    //{
        //    //    throw new Exception(string.Format("联系口[{0}]存在着配置错误", condition));
        //    //}

        //    if (CommCI.Contains(condition))
        //    {
        //        if (AgreeCI == null && partCI == null)
        //        {
        //            return LIAISON_TYPE_YARD;
        //        }
        //        else if (AgreeCI.Contains(condition) && partCI == null)
        //        {
        //            return LIAISON_TYPE_YARD_WITH_SWITCH_AGREE;
        //        }
        //        else if (partCI.Contains(condition) && AgreeCI == null)
        //        {
        //            return LIAISON_TYPE_YARD_NO_SWITCH_AGREE;
        //        }
        //    }

        //    if(AllDataModel.InterLockData.AutoCloseList.Select(x => x.ContactInterface).ToList().Contains(condition))
        //    {
        //        AutoCloseModel autoClose = AllDataModel.InterLockData.AutoCloseList.First(x => x.ContactInterface == condition);
        //        if(autoClose.SetSignal.StartsWith("N") && autoClose.ShowType == "四显示")
        //        {
        //            return LIAISON_TYPE_AUTO_BLOCKING_TYPE1;
        //        }
        //        else if(autoClose.SetSignal.StartsWith("Y") && autoClose.ShowType == "三显示")
        //        {
        //            return LIAISON_TYPE_AUTO_BLOCKING_TYPE4;
        //        }
        //        else if(autoClose.SetSignal.StartsWith("Y") && autoClose.ShowType == "四显示")
        //        {
        //            if(AllDataModel.InterLockData.InterfaceDefineList.FirstOrDefault(x => x.Name == autoClose.InterfaceName).InterfaceType == "CBI-JSDG.CBI")
        //            {
        //                return LIAISON_TYPE_AUTO_BLOCKING_TYPE2;
        //            }
        //            else
        //            {
        //                return LIAISON_TYPE_AUTO_BLOCKING_TYPE3;
        //            }
        //        }
        //    }

        //    if(AllDataModel.InterLockData.MiddleCloseList.Select(x => x.ContactInterface).ToList().Contains(condition))
        //    {
        //        MiddleCloseModel middleClose = AllDataModel.InterLockData.MiddleCloseList.First(x => x.ContactInterface == condition);
        //        if(middleClose.IntervalDirecton != "")
        //        {
        //            return LIAISON_TYPE_AUTO_BLOCKING_TYPE1;
        //        }
        //        else
        //        {
        //            return LIAISON_TYPE_AUTO_BLOCKING_TYPE2;
        //        }
        //    }

        //    if(AllDataModel.InterLockData.MiddleContactList.Select(x => x.ContactInterface).ToList().Contains(condition))
        //    {
        //        MiddleContactModel middleContact = AllDataModel.InterLockData.MiddleContactList.First(x => x.ContactInterface == condition);
        //        if(middleContact.SingleOrDouble != "D")
        //        {
        //            return LIAISON_TYPE_SINGLE_STATION;
        //        }
        //        else
        //        {
        //            return LIAISON_TYPE_DOUBLE_STATION;
        //        }
        //    }

        //    if(AllDataModel.InterLockData.SemiAutoCloseList.Select(x => x.ContactInterface).ToList().Contains(condition))
        //    {
        //        return LIAISON_TYPE_SEMI_AUTO_BLOCKING;
        //    }

        //    if(AllDataModel.InterLockData.HumpContactList.Select(x => x.ContactInterface).ToList().Contains(condition))
        //    {
        //        return LIAISON_TYPE_MECHNICAL_HUMP;
        //    }

        //    return result;
        //}

        /// <summary>
        /// 获取接口类型
        /// </summary>
        protected UInt32 GetInterfaceType(string InterfaceName)
        {
            UInt32 result = CONST_DEFAULT;
            switch (InterfaceName)
            {
                case "RELAY":
                    result = CONST_INTERFACE_RELAY;
                    break;
                case "TCC-TJ_TCC":
                case "TCC-KZ_TCC":
                    result = CONST_INTERFACE_TCC_TCC;
                    break;
                case "CBI-TJ_TCC":
                    result = CONST_INTERFACE_CBI_TCC;
                    break;
                case "CBI_CBI":
                    result = CONST_INTERFACE_CBI_CBI;
                    break;
                default:
                    throw new Exception(string.Format("接口名称[{0}]为非法值", InterfaceName));
            }

            return result;
        }

        /// <summary>
        /// 获取结构体的path
        /// </summary>
        /// <param name="approachBlock"> 接近区段字符串</param>
        /// <param name="count">path个数，用;加以区分</param>
        //protected Path[] GetPaths(string approachBlock, int count)
        //{
        //    var paths = new Path[count];
        //    for (int i = 0; i < count; i++)
        //    {
        //        paths[i].Block_ID = CONST_DEVICE_ID;

        //        paths[i].CondSwitch = new RouteSwitch[4];
        //        paths[i].CondSwitch[0].Switch_ID = CONST_DEVICE_ID;
        //        paths[i].CondSwitch[0].Positon = CONST_DEFAULT;
        //        paths[i].CondSwitch[1].Switch_ID = CONST_DEVICE_ID;
        //        paths[i].CondSwitch[1].Positon = CONST_DEFAULT;
        //        paths[i].CondSwitch[2].Switch_ID = CONST_DEVICE_ID;
        //        paths[i].CondSwitch[2].Positon = CONST_DEFAULT;
        //        paths[i].CondSwitch[3].Switch_ID = CONST_DEVICE_ID;
        //        paths[i].CondSwitch[3].Positon = CONST_DEFAULT;

        //    }

        //    if (string.IsNullOrWhiteSpace(approachBlock))
        //    {
        //        return paths;
        //    }

        //    int pathIndex = 0;
        //    var switchBlockDic = new Dictionary<string, string>();
        //    AllDataModel.InterLockData.SwitchList.ForEach(x => switchBlockDic.Add(x.Name, x.SwitchBlock));

        //    foreach (var item in approachBlock.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList())
        //    {
        //        string tempBlock = "";//存储区段
        //        var tempSwitchs = new List<string>();//存储道岔（包含括号）
        //        if (item.Contains('<'))
        //        {
        //            string switchString = item.Substring(item.IndexOf('<') + 1, item.IndexOf('>') - item.IndexOf('<') - 1);
        //            tempSwitchs.AddRange(switchString.SplitToList());
        //            tempBlock = item.Substring(item.IndexOf('>') + 1);
        //        }
        //        else
        //        {
        //            tempBlock = item;
        //        }

        //        tempSwitchs = tempSwitchs.Distinct().ToList();
        //        int switchIndex = -1;

        //        paths[pathIndex].Block_ID = GetDEVICE_ID(tempBlock, AllDataModel.InterLockData.BlockList);

        //        foreach (var tempSwitch in tempSwitchs)
        //        {
        //            var tempSwitchList = tempSwitch.Split('/').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

        //            for (int i = 0; i < tempSwitchList.Count; i++)
        //            {
        //                if (tempSwitchList[i].Contains("(") || tempSwitchList[i].Contains(")"))
        //                {
        //                    switchIndex++;
        //                    string reverseSwitch = tempSwitchList[i].Replace("(", string.Empty).Replace(")", string.Empty);

        //                    if (tempBlock != switchBlockDic[reverseSwitch])
        //                    {
        //                        //throw new Exception(string.Format("道岔{0}与区段{1}不对应", reverseSwitch, tempBlock));
        //                    }

        //                    paths[pathIndex].CondSwitch[switchIndex].Switch_ID =
        //                        GetDEVICE_ID(reverseSwitch, AllDataModel.InterLockData.SwitchList);
        //                    paths[pathIndex].CondSwitch[switchIndex].Positon = CONST_SWS_REVERSE;
        //                }
        //                else
        //                {
        //                    if (tempBlock != switchBlockDic[tempSwitchList[i]])
        //                    {
        //                        //throw new Exception(string.Format("道岔{0}与区段{1}不对应", tempSwitchList[i], tempBlock));
        //                    }
        //                    switchIndex++;

        //                    paths[pathIndex].CondSwitch[switchIndex].Switch_ID =
        //                        GetDEVICE_ID(tempSwitchList[i], AllDataModel.InterLockData.SwitchList);
        //                    paths[pathIndex].CondSwitch[switchIndex].Positon = CONST_SWS_NORMAL;
        //                }
        //            }
        //        }

        //        pathIndex++;
        //    }
        //    paths.Reverse();
        //    return paths;
        //}

        #endregion

        /// <summary>
        /// 将调车进路合并到列车进路表中
        /// </summary>
        //private void UnionRoute(List<RouteModel> routeList, List<ShuttingRouteModel> shuttingRouteList)
        //{

        //    if (routeList.Select(x => x.Name).ToList().Contains(shuttingRouteList.First().Name))
        //    {
        //        return;
        //    }

        //    var tempRouteList = new List<RouteModel>();
        //    foreach (var route in routeList)
        //    {
        //        route.AimAreaProtect = "Y";
        //        tempRouteList.Add(route);
        //    }

        //    routeList.Clear();

        //    foreach (var shuttingRoute in shuttingRouteList)
        //    {
        //        var route = new RouteModel();

        //        route.Direction = shuttingRoute.Direction;
        //        route.Id = shuttingRoute.Id;
        //        route.Name = shuttingRoute.Name;
        //        route.RouteType = 0;//便于区别调车进路和列车进路
        //        route.RouteButton = shuttingRoute.RouteButton;
        //        route.SignalName = shuttingRoute.SignalName;
        //        route.SignalOpenShow = shuttingRoute.SignalShow;
        //        route.SignalCloseShow = shuttingRoute.SignalShow;
        //        route.SignalScreen = "";
        //        route.SignalYorN = "N";//???
        //        route.JJSBJCYS = "30s";//???
        //        route.RouteSwitch = shuttingRoute.RouteSwitch;
        //        route.ProtectSwitch = shuttingRoute.ProtectSwitch;
        //        route.HostileSignal = shuttingRoute.HostileSignal;
        //        route.RailBlock = shuttingRoute.RailBlock;
        //        route.OverControl = shuttingRoute.OverControl;
        //        route.ApproachBlock = shuttingRoute.ApproachBlock;
        //        route.FacingRouteBlock = shuttingRoute.FacingRouteBlock;
        //        route.FacingShuttingBlock = shuttingRoute.FacingShuttingBlock;
        //        route.OpenCondition = shuttingRoute.OpenCondition;
        //        route.CloseCondition = shuttingRoute.CloseCondition;
        //        route.AimAreaProtect = shuttingRoute.AimAreaProtect;
        //        route.Platform = "";
        //        route.FloodGate = "";

        //        routeList.Add(route);
        //    }

        //    tempRouteList.ForEach(x => routeList.Add(x));
        //}
    }
}
