using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JSDG.CBI.A.ETBProcess;
using JSDG.CBI.A.ETBUtil;

namespace JSDG.CBI.A.ETBModel
{
    public class DeviceNameStruct : BaseStruct
   {
        public List<Name> Names;//104

        public DeviceNameStruct(DataModel dataModel, string currentZone): base(dataModel, currentZone)
        {
            this.WaysideInfo.SegmentName = "DeviceName";

            Transform();
        }

       /* public static readonly IReadOnlyDictionary<EDeviceType, UInt16> DicNameCode = new Dictionary<EDeviceType, UInt16>
        {
            {EDeviceType.DT_ROUTE ,             0x8000},
            {EDeviceType.DT_BLOCK,              0x8100},
            {EDeviceType.DT_LOGICBLOCK,         0x8400},
            {EDeviceType.DT_SIGNAL,             0x8200},
            {EDeviceType.DT_SWITCH,             0x8300},
            {EDeviceType.DT_PESB,               0x8500},
            {EDeviceType.DT_PLATFORM,           0x8700},
            {EDeviceType.DT_PSD,                0x8600},
            {EDeviceType.DT_RETURNCYCLE,        0x8800},
            {EDeviceType.DT_SPECIALIO,          0x8a00},
            {EDeviceType.DT_ZCJ,                0x8900},
            {EDeviceType.DT_RTNCYBUTTON,        0x8b00},
            {EDeviceType.DT_SPECIALCONDITION,   0x8c00},
            {EDeviceType.DT_TRANSPONDER,        0x8d00},
            {EDeviceType.DT_SPKS,               0x8e00},
        };*/
        public override void Transform()
        {
            Names = new List<Name>();
            string stationname = AllDataModel.InterLockData.ProjectName + " " + CurrentZone;
            Names.Add(new Name(stationname));

            Names.AddRange(SetName(0x8000, AllDataModel.InterLockData.ATBList.Where(x => 
                x.ControlledZone.SplitToList().Contains(CurrentZone) 
                || x.AdjacentZone.SplitToList().Contains(CurrentZone)).ToDictionary(x => x.Id, y => y.Name)));

            Names.AddRange(SetName(0x8100, AllDataModel.InterLockData.AutoturnbackRouteList.Where(x => 
                x.ControlledZone.SplitToList().Contains(CurrentZone) 
                || x.AdjacentZone.SplitToList().Contains(CurrentZone)).ToDictionary(x => x.Id, y => y.Name)));

            Names.AddRange(SetName(0x8200, AllDataModel.InterLockData.BlockList.Where(x => 
                x.ControlledZone.SplitToList().Contains(CurrentZone) 
                || x.AdjacentZone.SplitToList().Contains(CurrentZone)).ToDictionary(x => x.Id, y => y.Name)));

            Names.AddRange(SetName(0x8300, AllDataModel.InterLockData.GateDoorList.Where(x => 
                x.ControlledZone.SplitToList().Contains(CurrentZone) 
                || x.AdjacentZone.SplitToList().Contains(CurrentZone)).ToDictionary(x => x.Id, y => y.Name)));

            Names.AddRange(SetName(0x8400, AllDataModel.InterLockData.NotShuttingRouteList.Where(x => CurrentZone.Equals("ZC10")
                || CurrentZone.Equals("ZC9")).ToDictionary(x => x.Id, y => y.Name)));

            Names.AddRange(SetName(0x8500, AllDataModel.InterLockData.PCCBList.Where(x => 
                x.ControlledZone.SplitToList().Contains(CurrentZone) 
                || x.AdjacentZone.SplitToList().Contains(CurrentZone)).ToDictionary(x => x.Id, y => y.Name)));

            Names.AddRange(SetName(0x8600, AllDataModel.InterLockData.PCDBList.Where(x => 
                x.ControlledZone.SplitToList().Contains(CurrentZone) 
                || x.AdjacentZone.SplitToList().Contains(CurrentZone)).ToDictionary(x => x.Id, y => y.Name)));

            Names.AddRange(SetName(0x8700, AllDataModel.InterLockData.PESBList.Where(x => 
                x.ControlledZone.SplitToList().Contains(CurrentZone) 
                || x.AdjacentZone.SplitToList().Contains(CurrentZone)).ToDictionary(x => x.Id, y => y.Name)));

            Names.AddRange(SetName(0x8800, AllDataModel.InterLockData.PlatformList.Where(x => 
                x.ControlledZone.SplitToList().Contains(CurrentZone) 
                || x.AdjacentZone.SplitToList().Contains(CurrentZone)).ToDictionary(x => x.Id, y => y.Name)));

            Names.AddRange(SetName(0x8900, AllDataModel.InterLockData.StationList.Where(x => 
                x.ControlledZone.SplitToList().Contains(CurrentZone) 
                || x.AdjacentZone.SplitToList().Contains(CurrentZone)).ToDictionary(x => x.Id, y => y.Name)));

            Names.AddRange(SetName(0x8a00, AllDataModel.InterLockData.POBList.Where(x => 
                x.ControlledZone.SplitToList().Contains(CurrentZone) 
                || x.AdjacentZone.SplitToList().Contains(CurrentZone)).ToDictionary(x => x.Id, y => y.Name)));

            Names.AddRange(SetName(0x8b00, AllDataModel.InterLockData.PSDList.Where(x => 
                x.ControlledZone.SplitToList().Contains(CurrentZone) 
                || x.AdjacentZone.SplitToList().Contains(CurrentZone)).ToDictionary(x => x.Id, y => y.Name)));

            Names.AddRange(SetName(0x8c00, AllDataModel.InterLockData.ShuttingRouteList.Where(x => 
                x.ControlledZone.SplitToList().Contains(CurrentZone) 
                || x.AdjacentZone.SplitToList().Contains(CurrentZone)).ToDictionary(x => x.Id, y => y.Name)));

            Names.AddRange(SetName(0x8d00, AllDataModel.InterLockData.SignalList.Where(x => 
                x.ControlledZone.SplitToList().Contains(CurrentZone) 
                || x.AdjacentZone.SplitToList().Contains(CurrentZone)).ToDictionary(x => x.Id, y => y.Name)));

            Names.AddRange(SetName(0x8e00, AllDataModel.InterLockData.SpecialIOList.Where(x => 
                x.ControlledZone.SplitToList().Contains(CurrentZone) 
                || x.AdjacentZone.SplitToList().Contains(CurrentZone)).ToDictionary(x => x.Id, y => y.Name)));

            Names.AddRange(SetName(0x8f00, AllDataModel.InterLockData.SPKSList.Where(x => 
                x.ControlledZone.SplitToList().Contains(CurrentZone) 
                || x.AdjacentZone.SplitToList().Contains(CurrentZone)).ToDictionary(x => x.Id, y => y.Name)));

            Names.AddRange(SetName(0x9000, AllDataModel.InterLockData.SwitchList.Where(x => 
                x.ControlledZone.SplitToList().Contains(CurrentZone) 
                || x.AdjacentZone.SplitToList().Contains(CurrentZone)).ToDictionary(x => x.Id, y => y.Name)));

            Names.AddRange(SetName(0x9100, AllDataModel.InterLockData.RouteList.Where(x => 
                x.ControlledZone.SplitToList().Contains(CurrentZone) 
                || x.AdjacentZone.SplitToList().Contains(CurrentZone)).ToDictionary(x => x.Id, y => y.Name)));

            Names.AddRange(SetName(0x9300, AllDataModel.InterLockData.ZCJList.Where(x => 
                x.ControlledZone.SplitToList().Contains(CurrentZone) 
                || x.AdjacentZone.SplitToList().Contains(CurrentZone)).ToDictionary(x => x.Id, y => y.Name)));

            Names.AddRange(SetName(0x9400, AllDataModel.InterLockData.WashMachineList.Where(x => 
                x.ControlledZone.SplitToList().Contains(CurrentZone) 
                || x.AdjacentZone.SplitToList().Contains(CurrentZone)).ToDictionary(x => x.Id, y => y.Name)));

            Names.Sort((x, y) => (int)(x.CodedDeviceID - y.CodedDeviceID));
        }
        private List<Name> SetName(UInt16 code,Dictionary<UInt16,string> idNameDic)
        {
            List<Name> names = new List<Name>();
            foreach (var item in idNameDic)
            {
                names.Add(new Name(code, item.Key, item.Value));
            }
            return names;
        }
   }

    
    public class Name
    {
        public static readonly UInt16 DEVICENAME = 36;

        public UInt32 CodedDeviceID;
        public byte[] Names=Enumerable.Repeat((byte)0, (int) DEVICENAME).ToArray();  //36??

        public Name(string stationname)
        {
            CodedDeviceID = 0x71000000;

            var charBytes = Encoding.GetEncoding("GB2312").GetBytes(stationname.Trim());

            for (int i = 0; i < DEVICENAME && i < charBytes.Length; i++)
            {
                Names[i] = charBytes[i];
            }
        }
        public Name(UInt16 coded, UInt16 devid, string devname)
        {
            CodedDeviceID = (UInt32)((coded << 16) + devid);

            if (!string.IsNullOrEmpty(devname))
            {
                for (int i = 0; i < devname.Length; i++)
                {
                    Names[i] = (byte)devname[i];
                }
            }
        }
    }
}
