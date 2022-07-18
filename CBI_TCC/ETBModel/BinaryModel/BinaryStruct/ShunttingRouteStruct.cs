using System;
using System.Collections.Generic;
using System.Linq;
using JSDG.CBI.A.ETBProcess;
using JSDG.CBI.A.ETBUtil;

namespace JSDG.CBI.A.ETBModel
{
    public class ShunttingRouteStruct : RouteBaseStruct
    {
        private List<ShuttingRouteModel> sourceList;
        private List<SwitchModel> switchList;
        public List<ShunttingRouteStructData> Data;

        public ShunttingRouteStruct(DataModel dataModel, string currentZone): base(dataModel, currentZone)
        {
            sourceList = dataModel.InterLockData.ShuttingRouteList.Where(x => x.ControlledZone.SplitToList().Contains(currentZone)
                || x.AdjacentZone.SplitToList().Contains(currentZone)).ToList();
            switchList = AllDataModel.InterLockData.SwitchList.Where(x => x.ControlledZone.SplitToList().Contains(currentZone)
                || x.AdjacentZone.SplitToList().Contains(currentZone)).ToList();

            this.WaysideInfo.SegmentName = "ShunttingRoute";
            CheckMaxNum(sourceList);

            Transform();
        }

        public override void Transform()
        {
            var gateDoorList = AllDataModel.InterLockData.GateDoorList.Where(x => (x.ControlledZone.SplitToList().Contains(CurrentZone)
                || x.AdjacentZone.SplitToList().Contains(CurrentZone)) && x.Type == "列检库门").ToList();
            var signalList = AllDataModel.InterLockData.SignalList.Where(x => x.ControlledZone.SplitToList().Contains(CurrentZone)
                || x.AdjacentZone.SplitToList().Contains(CurrentZone)).ToList();

            Data = new List<ShunttingRouteStructData>();

            foreach (var item in sourceList)
            {
                var data = new ShunttingRouteStructData();

                data.DeviceID = item.Id;
                data.Route_Type = CONST_ShuttingRouteType;
                data.Direction = GetDir(item.SignalName);
                data.StartSignal = GetDeviceIndex(item.SignalName, DeviceIndexDic);
                data.SignalAspect = CONST_SIGS_WHITE;
                data.Block_protected = item.AimAreaProtect.StartsWith("Y");

                data.Route_Switches = GetRouteSwitchArray(20, item.EntryRouteSwitch, item.RailZone);
                data.Total_Switches = (UInt16)data.Route_Switches.Count(x => x.SwitchIndex.ID_index != CONST_DEFAULT);

                data.Flank_Switches = GetRouteSwitchArray(4, GetSwitchAndBlock(item.ProtectOrControlEntryRoute).ElementAt(0));
                data.FlankBlocks = GetDeviceIndexArray(GetSwitchAndBlock(item.ProtectOrControlEntryRoute).ElementAt(1), 8, DeviceIndexDic);
                data.Driving_Switches = GetRouteSwitchArray(2, GetSwitchAndBlock(item.ProtectOrControlEntryRoute).ElementAt(0));

                data.Route_Blocks = GetDeviceIndexArray(item.RailZone, 16, DeviceIndexDic);
                data.Total_Blocks =(UInt16) item.RailZone.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray().Length;

                data.FoulingBlocks = GetFoulingBlock(item.OverLineRailZone, 4);

                data.GateDoors = GetDeviceIndexArray(item.GateDoor, 3, DeviceIndexDic);

                data.AlarmBlocks = SetAlarmBlocks(signalList, gateDoorList, item, 3, out data.bEntryGateDoor);

                data.ConflictSignal = GetShuntingRouteConflictSignal(item, 32);

                data.Face_Train_route = GetDeviceIndex(item.FacingEntryRouteControlTrain, DeviceIndexDic);
                data.Face_Shuntting_route = GetDeviceIndex(item.FacingEntryRouteControlTrain, DeviceIndexDic);

                data.Approach_release_time = CONST_Approach_release_time;
               
                Data.Add(data);
            }
        }

        private DeviceIndex[] SetAlarmBlocks(List<SignalModel> signalList, List<GateDoorModel> gateDoorList,
            ShuttingRouteModel item, int length, out bool bEntryGateDoor)
        {
            DeviceIndex[] resultDevIndex = new DeviceIndex[length];
            var gateDoor = gateDoorList.FirstOrDefault(x => item.GateDoor.Contains(x.Name));

            if (gateDoor != null)
            {
                var entryAlarmBlocks = gateDoor.EntryAlarmBlocks.SplitToList();
                if (entryAlarmBlocks.All(x => item.RailZone.SplitToList().Contains(x)))//进路内区段包含库门报警区段
                {
                    List<string> blocks = signalList.FirstOrDefault(x => x.Name == item.SignalName).ApproachBlock.Split(
                        new char[] { '<', '>', ';', '+' }).Where(y => !string.IsNullOrWhiteSpace(y) && y.Contains("G")).ToList();
                    if (blocks.Contains(gateDoor.AxleBlocks))//进路内接近区段包含库门区段
                    {
                        bEntryGateDoor = false;
                        resultDevIndex = GetDeviceIndexArray("", length, DeviceIndexDic);
                    }
                    else
                    {
                        bEntryGateDoor = true;
                        resultDevIndex = GetDeviceIndexArray(gateDoor.EntryAlarmBlocks, length, DeviceIndexDic);
                    }
                }
                else
                {
                    bEntryGateDoor = false;
                    resultDevIndex = GetDeviceIndexArray("", length, DeviceIndexDic);
                }

            }
            else
            {
                bEntryGateDoor = true;
                resultDevIndex = GetDeviceIndexArray("", length, DeviceIndexDic);
            }

            return resultDevIndex;
        }

        private UInt16 GetDir(string signalName)
        {
            return "GD0" == AllDataModel.InterLockData.SignalList.Find(x => x.Name.Equals(signalName)).Direction
                ? CONST_DIR_UP : CONST_DIR_DOWN;
        }
    }

    public class ShunttingRouteStructData
    {
        public UInt16 DeviceID;
        public UInt16 Route_Type;
        public UInt16 Direction;
        public DeviceIndex StartSignal;
        public UInt16 SignalAspect;
        public bool Block_protected;
        public RouteSwitch[] Route_Switches;
        public UInt16 Total_Switches;
        public RouteSwitch[] Flank_Switches;
        public DeviceIndex[] FlankBlocks; //4
        public RouteSwitch[] Driving_Switches;
        public DeviceIndex[] Route_Blocks;
        public UInt16 Total_Blocks;
        public FoulingBlock[] FoulingBlocks;
        public DeviceIndex[] GateDoors;
        public bool bEntryGateDoor;
        public DeviceIndex[] AlarmBlocks;
        public ConflictSignal[] ConflictSignal;
        public DeviceIndex Face_Train_route;
        public DeviceIndex Face_Shuntting_route;
      //  public DeviceIndex ZC;
        public UInt16 Approach_release_time; //30
       // public Other_logic[] Other_logic;  
    }
}
