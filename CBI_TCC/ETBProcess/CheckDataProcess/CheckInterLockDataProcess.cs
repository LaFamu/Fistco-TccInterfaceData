using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JSDG.CBI.A.ETBAttribute;
using JSDG.CBI.A.ETBConfig;
using JSDG.CBI.A.ETBModel;
using JSDG.CBI.A.ETBUtil;

namespace JSDG.CBI.A.ETBProcess
{
    public class CheckInterLockDataProcess
    {
        public static InterLockData Data;

        public static void CheckData(InterLockData interLockData)
        {
            MsgHelper.Instance(1, string.Format("==============================正在检查联锁数据============================"));

            Data = interLockData;
            var nameHashSet = new HashSet<string>();//用于存储设备名称

            //CheckRoute(Data.RouteList, nameHashSet);
            ////CheckContinueRoute(Data.ContinueRouteList, nameHashSet);
            //CheckShuttingRoute(Data.ShuttingRouteList, nameHashSet);
            //CheckNotShuttingRoute(Data.NotShuttingRouteList, nameHashSet);
            //CheckSignal(Data.SignalList, nameHashSet);
            //CheckSwitch(Data.SwitchList, nameHashSet);
            //CheckBlock(Data.BlockList, nameHashSet);
            //CheckPlatform(Data.PlatformList, nameHashSet);
            //CheckSpecialIO(Data.SpecialIOList, nameHashSet);
            //CheckContactInterface(Data.ContactInterfaceList, nameHashSet);
            //CheckAutoClose(Data.AutoCloseList, nameHashSet);
            //CheckSubSystemID(Data.SubSystemIDList, nameHashSet);
            //CheckSemiAutoClose(Data.SemiAutoCloseList, nameHashSet);
            ////CheckRequestToAgree(Data.RequestToAgreeList, nameHashSet);
        }

        //#region 各数据检查方法

        ///// <summary>
        ///// 检查列车进路
        ///// </summary>
        //private static void CheckRoute(List<RouteModel> list, HashSet<string> hashSet)
        //{
        //    string sheetName = typeof(RouteModel).GetCustomAttribute<ExcelModelClassBaseAttribute>(false).SheetName;
        //    CheckBasicDataByProperty(list, sheetName);

        //    foreach (var perList in list)
        //    {
        //        CheckNameRepeat(null, hashSet, sheetName, list.IndexOf(perList) + 2, perList.Id);
        //        //CheckZCList(perList.ControlledZone, perList.AdjacentZone, sheetName, list.IndexOf(perList) + 2);

        //        if (perList.RouteType != 1 && perList.RouteType != 2)
        //        {
        //            throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行数据[{3}]为非法数据", sheetName,
        //                GetAttribute<RouteModel>("RouteType").ColName, (list.IndexOf(perList) + 2).ToString(), perList.RouteType));
        //        }

        //        //排列进路按下按钮
        //        if ((perList.RouteType == 1 && perList.RouteButton.SplitToList().Count() != 2)
        //            || (perList.RouteType != 1 && perList.RouteButton.SplitToList().Count() != 3))
        //        {
        //            throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行数据[{3}]与其进路方式不对应", sheetName,
        //                GetAttribute<RouteModel>("RouteButton").ColName, (list.IndexOf(perList) + 2).ToString(), perList.RouteButton));
        //        }//检查按钮个数

        //        CheckDevice<RouteModel>(perList.SignalName, "SignalName", sheetName, list.IndexOf(perList) + 2);//检查信号机名称

        //        //检查信号机显示
        //        List<string> showList = new List<string> { "L", "LU", "U", "UU", "LL", "USU" };
        //        if (perList.SignalCloseShow.SplitToList().Any(x => !showList.Contains(x))
        //            || perList.SignalOpenShow.SplitToList().Any(x => !showList.Contains(x)))
        //        {
        //            throw new Exception(string.Format("[{0}]表信号机显示第[{1}]行存在问题", sheetName, (list.IndexOf(perList) + 2).ToString()));
        //        }

        //        //检查信号机显示
        //        if (!perList.JJSBJCYS.EndsWith("s") && !perList.JJSBJCYS.EndsWith("min"))
        //        {
        //            throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行数据[{3}]格式存在错误", sheetName,
        //                GetAttribute<RouteModel>("JJSBJCYS").ColName, (list.IndexOf(perList) + 2).ToString(), perList.JJSBJCYS));
        //        }

        //        CheckDevice<RouteModel>(perList.RouteSwitch, "RouteSwitch", sheetName, list.IndexOf(perList) + 2);//检查进路道岔

        //        CheckDevice<RouteModel>(perList.ProtectSwitch, "ProtectSwitch", sheetName, list.IndexOf(perList) + 2);//检查防护道岔（只有道岔）

        //        CheckDevice<RouteModel>(perList.HostileSignal, "HostileSignal", sheetName, list.IndexOf(perList) + 2);//检查敌对信号

        //        CheckDevice<RouteModel>(perList.RailBlock, "RailBlock", sheetName, list.IndexOf(perList) + 2);//检查进路中轨道区段

        //        CheckDevice<RouteModel>(perList.OverControl, "OverControl", sheetName, list.IndexOf(perList) + 2);//检查超限区段

        //        CheckDevice<RouteModel>(perList.ApproachBlock, "ApproachBlock", sheetName, list.IndexOf(perList) + 2);//检查接近锁闭区段

        //        CheckDevice<RouteModel>(perList.FacingRouteBlock, "FacingRouteBlock", sheetName, list.IndexOf(perList) + 2);//检查迎面进路列车区段

        //        CheckDevice<RouteModel>(perList.FacingShuttingBlock, "FacingShuttingBlock", sheetName, list.IndexOf(perList) + 2);//检查迎面进路调车区段

        //        //var conditionList = Data.SpecialConditionList.Select(x => x.Name).ToList().Union(
        //        //    Data.SemiAutoCloseList.Select(x => x.Name).ToList()).ToList();
        //        //if(perList.OpenCondition != "" && perList.OpenCondition.SplitToList().Any(x => !conditionList.Contains(x)))//检查开灯
        //        //{
        //        //    throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行数据[{3}]未被定义", sheetName,
        //        //        GetAttribute<RouteModel>("OpenCondition").ColName, (list.IndexOf(perList) + 2).ToString(), perList.OpenCondition));
        //        //}
        //        //if(perList.CloseCondition != "" && perList.CloseCondition.SplitToList().Any(x => !conditionList.Contains(x)))//检查灭灯
        //        //{
        //        //    throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行数据[{3}]未被定义", sheetName,
        //        //        GetAttribute<RouteModel>("CloseCondition").ColName, (list.IndexOf(perList) + 2).ToString(), perList.CloseCondition));
        //        //}

        //        CheckDevice<RouteModel>(perList.Platform, "Platform", sheetName, list.IndexOf(perList) + 2);//检查站台

        //        CheckDevice<RouteModel>(perList.FloodGate, "FloodGate", sheetName, list.IndexOf(perList) + 2);//检查防淹门
        //    }
        //}

        ///// <summary>
        ///// 检查延续进路
        ///// </summary>
        //private static void CheckContinueRoute(List<ContinueRouteModel> list, HashSet<string> hashSet)
        //{
        //    string sheetName = typeof(ContinueRouteModel).GetCustomAttribute<ExcelModelClassBaseAttribute>(false).SheetName;
        //    CheckBasicDataByProperty(list, sheetName);

        //    foreach (var perList in list)
        //    {
        //        CheckNameRepeat(null, hashSet, sheetName, list.IndexOf(perList) + 2, perList.Id);
        //        //CheckZCList(perList.ControlledZone, perList.AdjacentZone, sheetName, list.IndexOf(perList) + 2);

        //        //CheckDevice<ContinueRouteModel>(perList.StartBlock, "StartBlock", sheetName, list.IndexOf(perList) + 2);//检查延续进路起始股道

        //        if (perList.RouteType != 1 && perList.RouteType != 2)
        //        {
        //            throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行数据[{3}]为非法数据", sheetName,
        //                GetAttribute<ContinueRouteModel>("RouteType").ColName, (list.IndexOf(perList) + 2).ToString(), perList.RouteType));
        //        }

        //        //延续进路终端
        //        if ((perList.RouteType == 1 && perList.RouteFinal.SplitToList().Count() != 2)
        //            || (perList.RouteType != 1 && perList.RouteFinal.SplitToList().Count() != 3))
        //        {
        //            throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行数据[{3}]与其进路方式不对应", sheetName,
        //                GetAttribute<ContinueRouteModel>("RouteFinal").ColName, (list.IndexOf(perList) + 2).ToString(), perList.RouteFinal));
        //        }//检查按钮个数

        //        //延续调车进路编号
        //        List<UInt32> shuttingId = Data.ShuttingRouteList.Select(x => x.Id).ToList();
        //        if (perList.ContinueShuttingId != "" && perList.ContinueShuttingId.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Any(x =>
        //            shuttingId.All(y => y != UInt32.Parse(x))))
        //        {
        //            throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行数据[{3}]为非法数据", sheetName,
        //                GetAttribute<ContinueRouteModel>("ContinueShuttingId").ColName, (list.IndexOf(perList) + 2).ToString(), perList.ContinueShuttingId));
        //        }
        //    }
        //}

        ///// <summary>
        ///// 检查调车进路
        ///// </summary>
        //private static void CheckShuttingRoute(List<ShuttingRouteModel> list, HashSet<string> hashSet)
        //{
        //    string sheetName = typeof(ShuttingRouteModel).GetCustomAttribute<ExcelModelClassBaseAttribute>(false).SheetName;
        //    CheckBasicDataByProperty(list, sheetName);

        //    foreach (var perList in list)
        //    {
        //        CheckNameRepeat(null, hashSet, sheetName, list.IndexOf(perList) + 2, perList.Id);

        //        //检查进路方式
        //        if (perList.RouteType > 10 || perList.RouteType < 1)
        //        {
        //            throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行数据须为1~10之间的整数",
        //                    sheetName, GetAttribute<ShuttingRouteModel>("SignalShow").ColName,
        //                    (list.IndexOf(perList) + 3).ToString()));
        //        }

        //        //排列进路按下按钮
        //        if ((perList.RouteType == 1 && perList.RouteButton.SplitToList().Count() != 2)
        //            || (perList.RouteType != 1 && perList.RouteButton.SplitToList().Count() != 3))
        //        {
        //            throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行数据[{3}]与其进路方式不对应", sheetName,
        //                GetAttribute<ShuttingRouteModel>("RouteButton").ColName, (list.IndexOf(perList) + 2).ToString(), perList.RouteButton));
        //        }//检查按钮个数

        //        CheckDevice<ShuttingRouteModel>(perList.SignalName, "SignalName", sheetName, list.IndexOf(perList) + 3);//检查信号机(名称)

        //        //检查信号机（显示）
        //        if (perList.SignalShow != "B")
        //        {
        //            throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行数据不合法", sheetName,
        //                GetAttribute<ShuttingRouteModel>("SignalShow").ColName, (list.IndexOf(perList) + 3).ToString()));
        //        }

        //        CheckDevice<ShuttingRouteModel>(perList.RouteSwitch, "RouteSwitch", sheetName, list.IndexOf(perList) + 3);//检查进路内道岔

        //        CheckDevice<ShuttingRouteModel>(perList.ProtectSwitch, "ProtectSwitch", sheetName, list.IndexOf(perList) + 3);//检查防护(带动)道岔

        //        CheckDevice<ShuttingRouteModel>(perList.HostileSignal, "HostileSignal", sheetName, list.IndexOf(perList) + 3);//检查敌对信号

        //        CheckDevice<ShuttingRouteModel>(perList.RailBlock, "RailBlock", sheetName, list.IndexOf(perList) + 3);//检查轨道区段

        //        CheckDevice<ShuttingRouteModel>(perList.OverControl, "OverControl", sheetName, list.IndexOf(perList) + 2);//检查超限区段

        //        CheckDevice<ShuttingRouteModel>(perList.ApproachBlock, "ApproachBlock", sheetName, list.IndexOf(perList) + 2);//检查接近锁闭区段

        //        CheckDevice<ShuttingRouteModel>(perList.FacingRouteBlock, "FacingRouteBlock", sheetName, list.IndexOf(perList) + 3);//检查迎面进路列车

        //        CheckDevice<ShuttingRouteModel>(perList.FacingShuttingBlock, "FacingShuttingBlock", sheetName,
        //            list.IndexOf(perList) + 3);//检查迎面进路调车
        //    }
        //}

        ///// <summary>
        ///// 检查非进路调车
        ///// </summary>
        //private static void CheckNotShuttingRoute(List<NotShuttingRouteModel> list, HashSet<string> hashSet)
        //{
        //    string sheetName = typeof(NotShuttingRouteModel).GetCustomAttribute<ExcelModelClassBaseAttribute>(false).SheetName;
        //    CheckBasicDataByProperty(list, sheetName);

        //    foreach (var perList in list)
        //    {
        //        if (perList.Id != (list.IndexOf(perList) + 1))
        //        {
        //            throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行数据不合法:{1}是由1递增的自然数", sheetName,
        //                GetAttribute<NotShuttingRouteModel>("Id").ColName, (list.IndexOf(perList) + 2).ToString()));
        //        }

        //        CheckNameRepeat(perList.Name, hashSet, sheetName, list.IndexOf(perList) + 2);

        //        CheckDevice<NotShuttingRouteModel>(perList.Switch, "Switch", sheetName, list.IndexOf(perList) + 2);//检查道岔

        //        CheckDevice<NotShuttingRouteModel>(perList.Block, "Block", sheetName, list.IndexOf(perList) + 2);//检查区段

        //        CheckDevice<NotShuttingRouteModel>(perList.OpenedSignal, "OpenedSignal", sheetName, list.IndexOf(perList) + 2);//检查开放信号

        //        CheckDevice<NotShuttingRouteModel>(perList.HandleNotRouteCheckBlock, "HandleNotRouteCheckBlock", sheetName,
        //            list.IndexOf(perList) + 2);//检查办理非进路时检查区段

        //        CheckDevice<NotShuttingRouteModel>(perList.IntrudeLimitBlock, "IntrudeLimitBlock", sheetName,
        //            list.IndexOf(perList) + 2);//检查侵入限界区段

        //        CheckDevice<NotShuttingRouteModel>(perList.CancelNotRouteCheckBlock, "CancelNotRouteCheckBlock", sheetName,
        //            list.IndexOf(perList) + 2);//检查取消非进路时检查区段

        //        if (perList.StartTrainRun != "" && !Data.SpecialIOList.Select(x => x.Name).ToList().Contains(perList.StartTrainRun))
        //        {
        //            throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行数据未在特殊设备中查找到", sheetName,
        //                GetAttribute<NotShuttingRouteModel>("StartTrainRun").ColName, (list.IndexOf(perList) + 2).ToString()));
        //        }//检查试车开始

        //        //CheckDevice(perList.OpenSpecialCondition, ExcelModelPropertyType.SpecialCondition, sheetName,
        //        //    GetAttribute<NotShuttingRouteModel>("OpenSpecialCondition").ColName, list.IndexOf(perList) + 2);//检查取消非进路时检查区段
        //    }
        //}

        ///// <summary>
        ///// 检查信号机
        ///// </summary>
        //private static void CheckSignal(List<SignalModel> list, HashSet<string> hashSet)
        //{
        //    string sheetName = typeof(SignalModel).GetCustomAttribute<ExcelModelClassBaseAttribute>(false).SheetName;
        //    CheckBasicDataByProperty(list, sheetName);

        //    foreach (var perList in list)
        //    {
        //        CheckNameRepeat(perList.Name, hashSet, sheetName, list.IndexOf(perList) + 2);
        //        //CheckZCList(perList.ControlledZone, perList.AdjacentZone, sheetName, list.IndexOf(perList) + 2);

        //        ////检查信号机显示
        //        //var signalShow = new List<string>() { "L", "LL", "LU", "U", "UU", "USU", "B", "Y", "A", "H", "LS", "US", "BS", "HS" };
        //        //if (perList.SignalShow.SplitToList().Any(x => !signalShow.Contains(x)))
        //        //{
        //        //    throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行数据[{3}]非法", sheetName,
        //        //        GetAttribute<SignalModel>("SignalShow").ColName, (list.IndexOf(perList) + 2).ToString(), perList.SignalShow));
        //        //}

        //        //检查信号机方向
        //        if (perList.Direction != "S" && perList.Direction != "X")
        //        {
        //            throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行数据[{3}]不合法", sheetName,
        //                GetAttribute<SignalModel>("Direction").ColName, (list.IndexOf(perList) + 2).ToString(), perList.Direction));
        //        }

        //        CheckDevice<SignalModel>(perList.RelateSignal, "RelateSignal", sheetName, list.IndexOf(perList) + 2);//检查关联信号机

        //        CheckDevice<SignalModel>(perList.SaveWhiteSignal, "SaveWhiteSignal", sheetName, list.IndexOf(perList) + 2);//检查白灯保留

        //        //if (perList.SaveWhiteSignal.StartsWith("Y") && !perList.SignalShow.Contains("B"))
        //        //{
        //        //    throw new Exception(string.Format("[{0}]表第[{1}]行白灯保持为YES时,信号显示需要有B", sheetName,
        //        //        (list.IndexOf(perList) + 2).ToString()));
        //        //}

        //        //检查常态
        //        var commonState = new List<string>() { "持续点灯", "常态点灯", "常态灭灯", "不适用" };
        //        if (commonState.All(x => !perList.OrdinaryState.Equals(x)))
        //        {
        //            throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行数据[{3}]不合法", sheetName,
        //                GetAttribute<SignalModel>("OrdinaryState").ColName, (list.IndexOf(perList) + 2).ToString(), perList.OrdinaryState));
        //        }

        //        CheckDevice<SignalModel>(perList.InternalDevice, "InternalDevice", sheetName, list.IndexOf(perList) + 2);//检查内部设备

        //        CheckDevice<SignalModel>(perList.Virtual, "Virtual", sheetName, list.IndexOf(perList) + 2);//检查虚拟信号机

        //        CheckDevice<SignalModel>(perList.DoubleControl, "DoubleControl", sheetName, list.IndexOf(perList) + 2);//检查白灯保留

        //        CheckDevice<SignalModel>(perList.InterfaceName, "InterfaceName", sheetName, list.IndexOf(perList) + 2);//检查接口名称

        //        //检查常态
        //        var signalType = new List<string>() { "进站/进路", "出站", "出站带双绿", "出站带反向表示器",
        //            "驼峰辅助", "股道间隔", "调车", "终端" };
        //        if (signalType.All(x => !perList.Type.Equals(x)))
        //        {
        //            throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行数据[{3}]不合法", sheetName,
        //                GetAttribute<SignalModel>("Type").ColName, (list.IndexOf(perList) + 2).ToString(), perList.Type));
        //        }
        //    }
        //}

        ///// <summary>
        ///// 检查道岔
        ///// </summary>
        //private static void CheckSwitch(List<SwitchModel> list, HashSet<string> hashSet)
        //{
        //    string sheetName = typeof(SwitchModel).GetCustomAttribute<ExcelModelClassBaseAttribute>(false).SheetName;
        //    CheckBasicDataByProperty(list, sheetName);

        //    foreach (var perList in list)
        //    {
        //        CheckNameRepeat(null, hashSet, sheetName, list.IndexOf(perList) + 2);
        //        //CheckZCList(perList.ControlledZone, perList.AdjacentZone, sheetName, list.IndexOf(perList) + 2);

        //        CheckDevice<SwitchModel>(perList.SwitchBlock, "SwitchBlock", sheetName, list.IndexOf(perList) + 2);//检查道岔区段

        //        if (perList.SwitchType != "单动道岔" && perList.SwitchType != "双动道岔")
        //        {
        //            throw new Exception(string.Format("[{0}]表第[{1}]行的道岔类型[{2}]不正确", sheetName,
        //                (list.IndexOf(perList) + 2).ToString(), perList.SwitchType));
        //        }

        //        //检查道岔类型和关联道岔
        //        if (perList.Name == perList.RelateSwitch)
        //        {
        //            throw new Exception(string.Format("[{0}]表第[{1}]行[{2}]的双动道岔不能为本道岔", sheetName,
        //                (list.IndexOf(perList) + 2).ToString(), perList.Name));
        //        }
        //        if (perList.SwitchType == "双动道岔" && perList.RelateSwitch == "")
        //        {
        //            throw new Exception(string.Format("[{0}]表第[{1}]行道岔类型错误或者关联道岔缺失", sheetName,
        //                (list.IndexOf(perList) + 2).ToString()));
        //        }
        //        if (perList.SwitchType == "双动道岔" && list.Any(x => (x.RelateSwitch == perList.Name && (x.Name != perList.RelateSwitch || x.SwitchType != "双动道岔"))))
        //        {
        //            throw new Exception(string.Format("[{0}]表第[{1}]行[{2}]的双动道岔不存在", sheetName,
        //                (list.IndexOf(perList) + 2).ToString(), perList.Name));
        //        }

        //        //检查道岔制式
        //        if ((new List<string>() { "单机", "多机", "单机带下拉", "多机带下拉" }).All(x => !perList.OperateType.Equals(x)))
        //        {
        //            throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行数据[{3}]非法", sheetName,
        //                GetAttribute<SwitchModel>("OperateType").ColName, (list.IndexOf(perList) + 2).ToString(), perList.OperateType));
        //        }

        //        CheckDevice<SwitchModel>(perList.IsSpeedSwitch, "IsSpeedSwitch", sheetName, list.IndexOf(perList) + 2);//检查提速道岔

        //        CheckDevice<SwitchModel>(perList.IsSafeSwitch, "IsSafeSwitch", sheetName, list.IndexOf(perList) + 2);//检查安全线道岔

        //        CheckDevice<SwitchModel>(perList.InternalDevice, "InternalDevice", sheetName, list.IndexOf(perList) + 2);//检查内部设备

        //        CheckDevice<SwitchModel>(perList.InterfaceName, "InterfaceName", sheetName, list.IndexOf(perList) + 2);//检查接口名称
        //    }
        //}

        ///// <summary>
        ///// 检查区段
        ///// </summary>
        //private static void CheckBlock(List<BlockModel> list, HashSet<string> hashSet)
        //{
        //    string sheetName = typeof(BlockModel).GetCustomAttribute<ExcelModelClassBaseAttribute>(false).SheetName;
        //    CheckBasicDataByProperty(list, sheetName);

        //    foreach (var perList in list)
        //    {
        //        CheckNameRepeat(perList.Name, hashSet, sheetName, list.IndexOf(perList) + 2);
        //        //CheckZCList(perList.ControlledZone, perList.AdjacentZone, sheetName, list.IndexOf(perList) + 2);

        //        //检查道岔类型
        //        if (!(new List<string> {"上行股道", "下行股道", "双向股道", "道岔区段", "无岔区段", "尽头线", "中岔道岔区段", 
        //            "中岔无岔区段", "灯泡线", "超长股道腰岔区段", "区间轨道" }).Contains(perList.BlockType))
        //        {
        //            throw new Exception(string.Format("[{0}]表第[{1}]行道岔类型[{2}]不合法",
        //                    sheetName, (list.IndexOf(perList) + 2).ToString(), perList.BlockType));
        //        }

        //        CheckDevice<BlockModel>(perList.IsC3Block, "IsC3Block", sheetName, list.IndexOf(perList) + 2);//检查计轴可复位

        //        CheckDevice<BlockModel>(perList.IsAxleReset, "IsAxleReset", sheetName, list.IndexOf(perList) + 2);//检查计轴可复位

        //        CheckDevice<BlockModel>(perList.InterfaceName, "InterfaceName", sheetName, list.IndexOf(perList) + 2);//检查接口名称

        //        CheckDevice<BlockModel>(perList.InternalDevice, "InternalDevice", sheetName, list.IndexOf(perList) + 2);//检查内部设备
        //    }
        //}

        ///// <summary>
        ///// 检查站台
        ///// </summary>
        //private static void CheckPlatform(List<PlatformModel> list, HashSet<string> hashSet)
        //{
        //    string sheetName = typeof(PlatformModel).GetCustomAttribute<ExcelModelClassBaseAttribute>(false).SheetName;
        //    CheckBasicDataByProperty(list, sheetName);

        //    foreach (var perList in list)
        //    {
        //        CheckNameRepeat(perList.Name, hashSet, sheetName, list.IndexOf(perList) + 2);
        //        //CheckZCList(perList.ControlledZone, perList.AdjacentZone, sheetName, list.IndexOf(perList) + 2);

        //        CheckDevice<PlatformModel>(perList.InterfaceName, "InterfaceName", sheetName, list.IndexOf(perList) + 2);//检查接口名称
        //    }
        //}

        ///// <summary>
        ///// 检查防淹门
        ///// </summary>
        ////private static void CheckFloodGate(List<FloodGateModel> list, HashSet<string> hashSet)
        ////{
        ////    string sheetName = typeof(FloodGateModel).GetCustomAttribute<ExcelModelClassBaseAttribute>(false).SheetName;
        ////    CheckBasicDataByProperty(list, sheetName);

        ////    foreach (var perList in list)
        ////    {
        ////        CheckNameRepeat(perList.Name, hashSet, sheetName, list.IndexOf(perList) + 2);
        ////        CheckZCList(perList.ControlledZone, perList.AdjacentZone, sheetName, list.IndexOf(perList) + 2);

        ////        CheckDevice<FloodGateModel>(perList.InterfaceName, "InterfaceName", sheetName, list.IndexOf(perList) + 2);//检查接口名称
        ////    }
        ////}

        ///// <summary>
        ///// 检查特殊设备
        ///// </summary>
        //private static void CheckSpecialIO(List<SpecialIOModel> list, HashSet<string> hashSet)
        //{
        //    string sheetName = typeof(SpecialIOModel).GetCustomAttribute<ExcelModelClassBaseAttribute>(false).SheetName;
        //    CheckBasicDataByProperty(list, sheetName);

        //    list.ForEach(x =>
        //    {
        //        CheckNameRepeat(x.Name, hashSet, sheetName, list.IndexOf(x) + 2);
        //        //CheckZCList(x.ControlledZone, x.AdjacentZone, sheetName, list.IndexOf(x) + 2);

        //        //检查类型
        //        var typeList = new List<string>() { "照查", "计轴电源故障", "指示灯", "故障告警灯", "轨道停电", "故障报警" };
        //        if (typeList.All(y => y != x.SpecialIOType))
        //        {
        //            throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行数据[{3}]不存在", sheetName,
        //                GetAttribute<SpecialIOModel>("SignalShow").ColName, (list.IndexOf(x) + 2).ToString(), x.SpecialIOType));
        //        }

        //        CheckDevice<SpecialIOModel>(x.IsInputIO, "IsInputIO", sheetName, list.IndexOf(x) + 2);//检查输入类型
        //    });
        //}

        ///// <summary>
        ///// 检查联系口定义表
        ///// </summary>
        //private static void CheckContactInterface(List<ContactInterfaceModel> list, HashSet<string> hashSet)
        //{
        //    string sheetName = typeof(ContactInterfaceModel).GetCustomAttribute<ExcelModelClassBaseAttribute>(false).SheetName;
        //    CheckBasicDataByProperty(list, sheetName);

        //    foreach (var perList in list)
        //    {
        //        CheckNameRepeat(perList.Name, hashSet, sheetName, list.IndexOf(perList) + 2);
        //        //CheckZCList(perList.ControlledZone, perList.AdjacentZone, sheetName, list.IndexOf(perList) + 2);

        //        CheckDevice<ContactInterfaceModel>(perList.GuardSignal, "GuardSignal", sheetName, list.IndexOf(perList) + 2);//检查把门信号机

        //        //检查联系口类型
        //        var type = new List<string>() { "BS", "BZD", "ZJB", "CL", "ZL", "QT", "JXTF", "JYTF", "FJL" };
        //        if (!type.Contains(perList.CIType))
        //        {
        //            throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行数据[{3}]非法", sheetName,
        //                GetAttribute<ContactInterfaceModel>("CIType").ColName, (list.IndexOf(perList) + 2).ToString(), perList.CIType));
        //        }
        //    }
        //}

        ///// <summary>
        ///// 检查子系统ID
        ///// </summary>
        //private static void CheckSubSystemID(List<SubSystemIDModel> list, HashSet<string> hashSet)
        //{
        //    string sheetName = typeof(SubSystemIDModel).GetCustomAttribute<ExcelModelClassBaseAttribute>(false).SheetName;
        //    CheckBasicDataByProperty(list, sheetName);

        //    foreach (var perList in list)
        //    {
        //        //CheckZCList(perList.ControlledZone, null, sheetName, list.IndexOf(perList) + 2);
        //    }
        //}

        ///// <summary>
        ///// 检查半自动闭塞
        ///// </summary>
        //private static void CheckSemiAutoClose(List<SemiAutoCloseModel> list, HashSet<string> hashSet)
        //{
        //    if (list.Select(x => x.ContactInterface).ToList().Any(y => String.IsNullOrWhiteSpace(y)))
        //    {
        //        return;
        //    }
        //    string sheetName = typeof(SemiAutoCloseModel).GetCustomAttribute<ExcelModelClassBaseAttribute>(false).SheetName;
        //    CheckBasicDataByProperty(list, sheetName);

        //    foreach (var perList in list)
        //    {
        //        CheckNameRepeat(perList.Name, hashSet, sheetName, list.IndexOf(perList) + 2);

        //        CheckDevice < SemiAutoCloseModel>(perList.RelateSignal, "RelateSignal", sheetName, list.IndexOf(perList) + 3);//检查关联信号机
        //    }
        //}

        ///// <summary>
        ///// 检查自动闭塞
        ///// </summary>
        //private static void CheckAutoClose(List<AutoCloseModel> list, HashSet<string> hashSet)
        //{
        //    if (list.Select(x => x.ContactInterface).ToList().Any(y => String.IsNullOrWhiteSpace(y)))
        //    {
        //        return;
        //    }
        //    string sheetName = typeof(AutoCloseModel).GetCustomAttribute<ExcelModelClassBaseAttribute>(false).SheetName;
        //    CheckBasicDataByProperty(list, sheetName);

        //    foreach (var perList in list)
        //    {
        //        CheckNameRepeat(perList.Name, hashSet, sheetName, list.IndexOf(perList) + 2);

        //        CheckDevice<AutoCloseModel>(perList.ContactInterface, "ContactInterface", sheetName, list.IndexOf(perList) + 3);//检查联系口名称

        //        CheckDevice<AutoCloseModel>(perList.RelateSignal, "RelateSignal", sheetName, list.IndexOf(perList) + 3);//检查关联信号机

        //        CheckDevice<AutoCloseModel>(perList.FirstLeaveBlock, "FirstLeaveBlock", sheetName, list.IndexOf(perList) + 3);//检查1离去区段

        //        CheckDevice<AutoCloseModel>(perList.SecondLeaveBlock, "SecondLeaveBlock", sheetName, list.IndexOf(perList) + 3);//检查2离去区段

        //        CheckDevice<AutoCloseModel>(perList.ThirdLeaveBlock, "ThirdLeaveBlock", sheetName, list.IndexOf(perList) + 3);//检查3离去区段

        //        CheckDevice<AutoCloseModel>(perList.SetSignal, "SetSignal", sheetName, list.IndexOf(perList) + 2);//检查区间设置通过信号机

        //        //检查显示类型
        //        if (perList.ShowType != "三显示" && perList.ShowType != "四显示")
        //        {
        //            throw new Exception(string.Format("[{0}]表第[{1}]行的显示类型[{2}]不正确", sheetName,
        //                (list.IndexOf(perList) + 2).ToString(), perList.ShowType));
        //        }

        //        CheckDevice<AutoCloseModel>(perList.InterfaceName, "InterfaceName", sheetName, list.IndexOf(perList) + 2);//检查接口名称
        //    }
        //}

        ///// <summary>
        ///// 检查请求同意
        ///// </summary>
        ////private static void CheckRequestToAgree(List<RequestToAgreeModel> list, HashSet<string> hashSet)
        ////{
        ////    string sheetName = typeof(RequestToAgreeModel).GetCustomAttribute<ExcelModelClassBaseAttribute>(false).SheetName;
        ////    CheckBasicDataByProperty(list, sheetName);

        ////    foreach (var perList in list)
        ////    {
        ////        CheckNameRepeat(perList.Name, hashSet, sheetName, list.IndexOf(perList) + 2);
        ////    }
        ////}

        //#endregion

        /// <summary>
        /// 校验非空数据是否为空/不能重复数据是否重复/id列是否合法/枚举列是否在枚举值中
        /// </summary>
        /// <param name="list">Sheet数据</param>
        /// <param name="sheetName">表格名称</param>
        private static void CheckBasicDataByProperty<T>(List<T> sourceList, string sheetName)
        {
            MsgHelper.Instance(1, string.Format("正在检查联锁表【{0}】数据", sheetName));
            var list = PropertyValueItemHelper<T>.GetPropertyValueItems(sourceList).ToList();
            try
            {
                foreach (var t in list)
                {
                    if (t.Attribute.IsCheckNull && t.Value.Any(x => x.IsEmpty()))
                    {
                        throw new Exception(string.Format("sheet[{1}][{0}]列存在空数据", t.Attribute.ColName, sheetName));
                    }

                    if (t.Attribute.IsCheckRepeat && t.Value.Count() > t.Value.Select(ObjectHelper.ToStr).Distinct().Count())
                    {
                        throw new Exception(string.Format("sheet[{1}][{0}]列存在重复数据", t.Attribute.ColName, sheetName));
                    }

                    if (t.Attribute.PropertyType == ExcelModelPropertyType.IndexType && t.Value.Any(x => "0" == x.ToString()))
                    {
                        throw new Exception(string.Format("sheet[{1}][{0}]列存在非法数据", t.Attribute.ColName, sheetName));
                    }

                    if (t.Attribute.PropertyType == ExcelModelPropertyType.IndexType)
                    {
                        if (t.Value.Any(x => !ObjectHelper.IsPositiveInteger(x)))
                        {
                            throw new Exception(string.Format("sheet[{1}][{0}]列存在非法数据", t.Attribute.ColName, sheetName));
                        }
                    }
                    else if (t.Attribute.PropertyType.ToString().EndsWith("EnumType"))
                    {
                        if (t.Attribute.IsCheckNull)
                        {
                            if (t.Value.Any(x => !ETBConfig.ETB_APPConfig.GetEnums(t.Attribute.PropertyType.ToString()).Contains(x.ToStr())))
                            {
                                var temp = t.Value.Where(x => !ETBConfig.ETB_APPConfig.GetEnums(t.Attribute.PropertyType.ToString()).Contains(x.ToStr()));
                                throw new Exception(string.Format("sheet[{1}][{0}]列存在非法数据", t.Attribute.ColName, sheetName));
                            }
                        }
                        else
                        {
                            foreach (var it in t.Value)
                            {
                                if (!it.IsEmpty() && !ETBConfig.ETB_APPConfig.GetEnums(t.Attribute.PropertyType.ToString()).Contains(it.ToString()))
                                {
                                    throw new Exception(string.Format("sheet[{1}][{0}]列存在非法数据", t.Attribute.ColName, sheetName));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// 名称以及部分ID重复检查
        /// </summary>
        /// <param name="name">设备名称</param>
        /// <param name="hashSet">存储设备名称总哈希表</param>
        private static void CheckNameRepeat(string name, HashSet<string> hashSet, string sheetName, int rowIndex, UInt32 id = UInt32.MaxValue)
        {
            int beginindex = hashSet.Count();
            if (name != null)
            {
                hashSet.Add(name);
                if (name != null && hashSet.Count() != (beginindex + 1))
                {
                    throw new Exception(string.Format("[{0}]表中[{1}]行的名称[{2}]为重复名称", sheetName, rowIndex.ToString(), name));
                }
            }

            if (id != UInt32.MaxValue)
            {
                hashSet.Add("CHECKID" + id.ToString());
                if ((name != null && hashSet.Count() != (beginindex + 2)) || (name == null && hashSet.Count() != (beginindex + 1)))
                {
                    throw new Exception(string.Format("[{0}]表中[{1}]行的ID[{2}]重复", sheetName, rowIndex.ToString(), id.ToString()));
                }
            }
        }

        /// <summary>
        /// 校验一格中所列设备数据是否在设备列表中存在(比如 信号机/道岔/区段--
        /// </summary>
        /// <param name="sourceDic">由待检验设备与所属控区组成的字典</param>
        /// <param name="destinationDic">匹配目标设备与所属控区所组成的字典</param>
        /// <param name="sheetName">表名</param>
        /// <param name="colName">列名</param>
        /// <param name="rowIndex">行号</param>
        //private static void CheckDevice<T>(string sourceString, string colProperty, string sheetName, int rowIndex)
        //{
        //    ExcelModelPropertyType propertyType = GetAttribute<T>(colProperty).PropertyType;
        //    string colName = GetAttribute<T>(colProperty).ColName;

        //    var sourceList = new List<string>();
        //    var sourceDic = new Dictionary<string, string>();

        //    if (string.IsNullOrWhiteSpace(sourceString))
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        if (sourceString.ToArray().ToList().Count(x => x == '<') != sourceString.ToArray().ToList().Count(x => x == '>')
        //        || sourceString.ToArray().ToList().Count(x => x == '(') != sourceString.ToArray().ToList().Count(x => x == ')')
        //        || sourceString.ToArray().ToList().Count(x => x == '[') != sourceString.ToArray().ToList().Count(x => x == ']')
        //        || sourceString.ToArray().ToList().Count(x => x == '{') != sourceString.ToArray().ToList().Count(x => x == '}'))
        //        {
        //            throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行存在格式错误", sheetName, colName, rowIndex.ToString()));
        //        }
        //    }

        //    if (propertyType == ExcelModelPropertyType.ProtectSwitchType && sourceString.Contains("{"))
        //    {
        //        propertyType = ExcelModelPropertyType.SwitchType;//处理带动道岔
        //    }

        //    if (propertyType == ExcelModelPropertyType.YNEnumType)
        //    {
        //        if (sourceString != "Y" && sourceString != "N")
        //        {
        //            throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行[{3}]应填Y或N",
        //            sheetName, colName, rowIndex.ToString(), sourceString));
        //        }
        //    }
        //    else if (propertyType == ExcelModelPropertyType.SwitchType)
        //    {
        //        List<string> tempSwitchList = sourceString.SplitToList();
        //        foreach (string item in tempSwitchList)
        //        {
        //            string tempSwitch = item;
        //            if (tempSwitch.Contains("(") && tempSwitch.Contains(")"))
        //            {
        //                tempSwitch = item.Replace("(", string.Empty).Replace(")", string.Empty);
        //            }
        //            if (tempSwitch.Contains("{") && tempSwitch.Contains("}"))
        //            {
        //                tempSwitch = item.Replace("{", string.Empty).Replace("}", string.Empty);
        //            }
        //            if (!tempSwitch.Contains("/"))
        //            {
        //                sourceList.Add(tempSwitch);
        //            }
        //            else
        //            {
        //                var middleSwitch = tempSwitch.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        //                if (Data.SwitchList.FirstOrDefault(x => x.Name == middleSwitch.First() && x.RelateSwitch == middleSwitch.Last()) == null)
        //                {
        //                    throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行数据[{3}]有误",
        //                        sheetName, colName, rowIndex.ToString(), sourceString));
        //                }
        //                else
        //                {
        //                    sourceList.Add(middleSwitch.First());
        //                    sourceList.Add(middleSwitch.Last());
        //                }
        //            }
        //        }
        //    }
        //    else if (propertyType != ExcelModelPropertyType.HostileSignal && propertyType != ExcelModelPropertyType.Insulation
        //        && propertyType != ExcelModelPropertyType.ProtectSwitchType)
        //    {
        //        switch (propertyType)
        //        {
        //            case ExcelModelPropertyType.BlockType:
        //            case ExcelModelPropertyType.SignalType:
        //            case ExcelModelPropertyType.RouteType:
        //            case ExcelModelPropertyType.SpecialCondition:
        //                sourceList = sourceString.SplitToList();
        //                break;
        //        }
        //    }

        //    switch (propertyType)
        //    {
        //        case ExcelModelPropertyType.BlockType:
        //            CheckDeviceBase(sourceList, Data.BlockList.Select(x => x.Name).ToList(), sheetName, colName, rowIndex);
        //            break;
        //        case ExcelModelPropertyType.SignalType:
        //            CheckDeviceBase(sourceList, Data.SignalList.Select(x => x.Name).ToList(), sheetName, colName, rowIndex);
        //            break;
        //        case ExcelModelPropertyType.SwitchType:
        //            CheckDeviceBase(sourceList, Data.SwitchList.Select(x => x.Name).ToList(), sheetName, colName, rowIndex);
        //            break;
        //        case ExcelModelPropertyType.RouteType:
        //            CheckDeviceBase(sourceList, Data.RouteList.Select(x => x.Name).ToList(), sheetName, colName, rowIndex);
        //            break;
        //        case ExcelModelPropertyType.InterfaceType:
        //            CheckDeviceBase(sourceList, Data.InterfaceDefineList.Select(x => x.Name).ToList(), sheetName, colName, rowIndex);
        //            break;
        //        case ExcelModelPropertyType.ContactInterface:
        //            CheckDeviceBase(sourceList, Data.ContactInterfaceList.Select(x => x.Name).ToList(), sheetName, colName, rowIndex);
        //            break;
        //        case ExcelModelPropertyType.HostileSignal:
        //            CheckOtherDevice(StringSplit(sourceString, propertyType), sheetName, colName, rowIndex);
        //            break;
        //        case ExcelModelPropertyType.Insulation:
        //            CheckOtherDevice(StringSplit(sourceString, propertyType), sheetName, colName, rowIndex);
        //            break;
        //        case ExcelModelPropertyType.ProtectSwitchType:
        //            CheckOtherDevice(StringSplit(sourceString, propertyType), sheetName, colName, rowIndex);
        //            break;
        //    }
        //}

        /// <summary>
        /// 校验敌对信号等当中的设备
        /// </summary>
        /// <param name="sourceDic">分解敌对信号字符获得的字典</param>
        /// <param name="destinationDic">所属控区</param>
        /// <param name="sheetName">表名</param>
        /// <param name="colName">列名</param>
        /// <param name="rowIndex">行号</param>
        //private static void CheckOtherDevice(Dictionary<string, string> sourceDic, string sheetName, string colName, int rowIndex)
        //{
        //    var switchList = new List<string>();
        //    var blockList = new List<string>();
        //    var signalList = new List<string>();

        //    foreach (var data in sourceDic)
        //    {
        //        switch (data.Value)
        //        {
        //            case "SwitchType":
        //                switchList.Add(data.Key);
        //                break;
        //            case "BlockType":
        //                blockList.Add(data.Key);
        //                break;
        //            case "SignalType":
        //                signalList.Add(data.Key);
        //                break;
        //            case "COUNT":
        //                throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行存在[{3}]",
        //            sheetName, colName, rowIndex.ToString(), data.Key));
        //        }
        //    }

        //    CheckDeviceBase(switchList, Data.SwitchList.Select(x => x.Name).ToList(), sheetName, colName, rowIndex);
        //    CheckDeviceBase(blockList, Data.BlockList.Select(x => x.Name).ToList(), sheetName, colName, rowIndex);
        //    CheckDeviceBase(signalList, Data.SignalList.Select(x => x.Name).ToList(), sheetName, colName, rowIndex);
        //}

        /// <summary>
        /// 分解敌对信号、防侵限、防护道岔的字符串获得其中各设备
        /// </summary>
        /// <param name="sourceString">模型中相应类型的字符串</param>
        /// <param name="property">列类型</param>
        //private static Dictionary<string, string> StringSplit(string sourceString, ExcelModelPropertyType property)
        //{
        //    //用于存储返回值，前者为设备名称，后者表征设备类型（SwitchType表征道岔，BlockType表征区段，SignalType表征信号机）
        //    var returnDic = new Dictionary<string, string>();

        //    var switchString = "";//存储截取的道岔字符
        //    var switchHS = new HashSet<string>();//存储道岔，防重复处理，字典直接添加重复键值会报错
        //    var blockHS = new HashSet<string>();//存储区段
        //    var signalHS = new HashSet<string>();//存储信号机
        //    var errorHS = new HashSet<string>();//存储错误信息


        //    if (property == ExcelModelPropertyType.HostileSignal)//处理敌对信号
        //    {
        //        if (sourceString.ToArray().ToList().Count(x => x == '<') != 0)
        //        {
        //            int count = sourceString.ToArray().ToList().Count(x => x == '<');
        //            for (int index = 0; index < count; index++)
        //            {
        //                switchString = sourceString.Substring(sourceString.IndexOf('<', 0),
        //                    sourceString.IndexOf('>', 0) - sourceString.IndexOf('<', 0) + 1);
        //                sourceString.Replace(switchString, string.Empty);
        //                switchString = switchString.Replace("<", string.Empty).Replace(">", "");
        //                if (switchString == "")//道岔少于0个
        //                {
        //                    returnDic.Add("道岔个数错误", "COUNT");
        //                }
        //                switchHS.AddRangeList(switchString.SplitToList());
        //            }
        //            signalHS.AddRangeList(sourceString.SplitToList());
        //        }
        //        else
        //        {
        //            signalHS.AddRangeList(sourceString.SplitToList());
        //        }
        //    }
        //    else if (property == ExcelModelPropertyType.Insulation)//处理防侵限
        //    {
        //        if (sourceString.ToArray().ToList().Count(x => x == '<') != 0)
        //        {
        //            int count = sourceString.ToArray().ToList().Count(x => x == '<');
        //            for (int index = 0; index < count; index++)
        //            {
        //                switchString = sourceString.Substring(sourceString.IndexOf('<', 0),
        //                    sourceString.IndexOf('>', 0) - sourceString.IndexOf('<', 0) + 1);
        //                sourceString.Replace(switchString, string.Empty);
        //                switchString = switchString.Replace("<", string.Empty).Replace(">", "");
        //                if (switchString == "" || switchString.SplitToList().Count() > 3)//道岔少于0个或者大于3个
        //                {
        //                    returnDic.Add("道岔个数错误", "COUNT");
        //                }
        //                switchHS.AddRangeList(switchString.SplitToList());
        //            }
        //            signalHS.AddRangeList(sourceString.SplitToList());
        //        }
        //        else
        //        {
        //            blockHS.AddRangeList(sourceString.SplitToList());
        //        }
        //    }
        //    else//处理防护道岔
        //    {
        //        sourceString = sourceString.Replace("],", "$");
        //        sourceString = sourceString.Replace("]", "").Replace("[", "");
        //        List<string> stringList = sourceString.Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        //        foreach (var data in stringList)
        //        {
        //            if (!data.StartsWith(","))
        //            {
        //                data.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(x => switchHS.Add(x));
        //            }
        //            else
        //            {
        //                blockHS.Add(data.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().Last());

        //                data.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().First().Split(
        //                    new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(x => switchHS.Add(x));
        //            }
        //        }
        //    }

        //    switchHS.Foreach(x => returnDic.Add(x, "SwitchType"));
        //    blockHS.Foreach(x => returnDic.Add(x, "BlockType"));
        //    signalHS.Foreach(x => returnDic.Add(x, "SignalType"));

        //    return returnDic;
        //}

        /// <summary>
        /// 校验一格中所列设备数据是否在设备列表中存在(比如 信号机/道岔/区段--
        /// </summary>
        /// <param name="sourceDic">由待检验设备与所属控区组成的字典</param>
        /// <param name="destinationDic">匹配目标设备与所属控区所组成的字典</param>
        private static void CheckDeviceBase(List<string> sourceList, List<string> destinationList,
            string sheetName, string colName, int rowIndex)
        {
            foreach (var data in sourceList)
            {
                if (!destinationList.Contains(data))
                {
                    throw new Exception(string.Format("[{0}]表[{1}]第[{2}]行[{3}]不存在",
                    sheetName, colName, rowIndex.ToString(), data));
                }
            }
        }

        ///// <summary>
        ///// 校验控区是否存在
        ///// </summary>
        ///// <param name="sourceCZC">所属控区</param>
        ///// <param name="sourceAZC">相邻控区</param>
        ///// <param name="sheetName">表名</param>
        ///// <param name="colName">列名</param>
        ///// <param name="rowIndex">行号</param>
        //private static void CheckZCList(string sourceCZC, string sourceAZC, string sheetName, int rowIndex)
        //{
        //    if (!string.IsNullOrWhiteSpace(sourceCZC) && ZoneName.All(x => !sourceCZC.SplitToList().Contains(x)))
        //    {
        //        throw new Exception(string.Format("[{0}]表第[{1}]行所属控区[{2}]不存在",
        //            sheetName, rowIndex.ToString(), sourceCZC));
        //    }

        //    if (!string.IsNullOrWhiteSpace(sourceAZC) && ZoneName.All(x => !sourceAZC.SplitToList().Contains(x)))
        //    {
        //        throw new Exception(string.Format("[{0}]表第[{1}]行相邻控区[{2}]不存在",
        //            sheetName, rowIndex.ToString(), sourceAZC));
        //    }
        //}

        /// <summary>
        /// 获取T的成员属性名是propertyName的属性描述
        /// </summary>
        private static MetroExcelModelPropertyAttribute GetAttribute<T>(string propertyName)
        {
            return typeof(T).GetProperties().First(x => x.Name == propertyName).GetCustomAttribute<MetroExcelModelPropertyAttribute>(false);
        }
    }
}
