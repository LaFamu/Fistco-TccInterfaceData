using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TCC_TCC.A.ETBUtil;
using TCC_TCC.A.ETBModel;

namespace TCC_TCC.A.ETBProcess
{
    class CheckAllDataProcess
    {
        public static void CheckAllData(DataModel dataModel)
        {
            //CheckInterLockDataProcess.CheckData(dataModel.InterLockData);
            //CheckProject(dataModel.DataVersion, dataModel.ProjectType, dataModel.ProjectName);
            //CheckIOBitmap(dataModel);
        }
        /// <summary>
        /// 检查项目名称
        /// </summary>
        private static void CheckProject(string DataVersion, string ProjectType, string ProjectName)
        {
            try
            {
                MsgHelper.Instance(1, string.Format("正在检查数据版本和数据类型"));
                if (ProjectName == "")
                {
                    throw new Exception(string.Format("项目名称不能为空"));
                }

                int version = int.Parse(DataVersion);
                if (version > 99 || version < 1)
                {
                    throw new Exception(string.Format("版本号[{0}]错误", DataVersion));
                }

                if (ProjectType != "HIGH-SPEED Railway")
                {
                    throw new Exception(string.Format("项目类型[{0}]错误，应为HIGH-SPEED Railway", ProjectType));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //#region IOBitmap检查

        ///// <summary>
        ///// 检查IOBitmap
        ///// </summary>
        //private static void CheckIOBitmap(DataModel dataModel)
        //{
        //    MsgHelper.Instance(1, string.Format("正在检查IOBitmap数据"));
        //    CheckIODevice(dataModel);
        //    CheckIORepeat(dataModel.IOMapList);
        //    CheckOutAndIn(dataModel.IOMapList);
        //}

        ///// <summary>
        ///// 检查继电器配置的设备
        ///// </summary>
        //private static void CheckIODevice(DataModel dataModel)
        //{
        //    List<string> ioDevice = new List<string>();
        //    List<string> deviceList = new List<string>();

        //    foreach (IOMapModel io in dataModel.IOMapList)
        //    {
        //        for (int i = 1; i <= 16; i++)
        //        {
        //            if (io.RelayDictionary[i] != null)
        //            {
        //                ioDevice.Add(io.RelayDictionary[i].Value.Key);
        //            }
        //        }
        //    }

        //    dataModel.InterLockData.SignalList.Where(x => x.InterfaceName == "RELAY").ToList().ForEach(y => deviceList.Add(y.Name));
        //    dataModel.InterLockData.SwitchList.Where(x => x.InterfaceName == "RELAY").ToList().ForEach(y => deviceList.Add(y.Name));

        //    dataModel.InterLockData.BlockList.Where(x => x.InterfaceName == "RELAY").ToList().ForEach(y => deviceList.Add(y.Name));
        //    //deviceList.AddRange(new List<string> { "X1LQG"});//区段部分特殊处理

        //    if (dataModel.InterLockData.RequestToAgreeList != null && dataModel.InterLockData.RequestToAgreeList.Count != 0)
        //    {
        //        dataModel.InterLockData.RequestToAgreeList.ForEach(x => deviceList.Add(x.Name));
        //    }
        //    //if (dataModel.InterLockData.SemiAutoCloseList != null && dataModel.InterLockData.SemiAutoCloseList.Count != 0)
        //    //{
        //    //    dataModel.InterLockData.SemiAutoCloseList.ForEach(x => deviceList.Add(x.Name));
        //    //}
        //    if (dataModel.InterLockData.SpecialIOList != null && dataModel.InterLockData.SpecialIOList.Count != 0)
        //    {
        //        dataModel.InterLockData.SpecialIOList.ForEach(x => deviceList.Add(x.Name));
        //    }

        //    //deviceList.AddRange(new List<string> { "GZIJ", "ZJIJ", "QHIJ", "SDIJ", "GZOJ", "TBOJ", "ZJOJ", "QHOJ" });

        //    foreach (string device in ioDevice)
        //    {
        //        if (!deviceList.Contains(device))
        //        {
        //            throw new Exception(string.Format("[{0}]非联锁配置设备", device));
        //        }
        //    }

        //    foreach (string device in deviceList)
        //    {
        //        if (!ioDevice.Contains(device)
        //            && !dataModel.InterLockData.SwitchList.Where(x => x.RelateSwitch != "").Select(x => x.RelateSwitch).ToList().Contains(device))
        //        {
        //            throw new Exception(string.Format("[{0}]未配置继电器", device));
        //        }
        //    }
        //}

        ///// <summary>
        ///// 继电器重复性检查
        ///// </summary>
        //private static void CheckIORepeat(List<IOMapModel> ioList)
        //{
        //    HashSet<string> ioHS = new HashSet<string>();
        //    foreach (IOMapModel io in ioList)
        //    {
        //        for (int i = 1; i <= 16; i++)
        //        {
        //            int count = ioHS.Count();

        //            if (io.RelayDictionary[i] != null)
        //            {
        //                var nullable = io.RelayDictionary[i];
        //                string ioString = nullable.Value.Key + "_" + nullable.Value.Value;
        //                ioHS.Add(ioString);
        //                if (ioHS.Count() != count + 1)
        //                {
        //                    throw new Exception(string.Format("[{0}]继电器重复配置", ioString));
        //                }
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// 检查输入输出关系
        ///// </summary>
        //private static void CheckOutAndIn(List<IOMapModel> ioList)
        //{
        //    foreach (IOMapModel io in ioList)
        //    {
        //        for (int i = 1; i <= 16; i++)
        //        {
        //            var nullable = io.RelayDictionary[i];

        //            if (nullable != null)
        //            {
        //                if ((new List<string> { "_Q", "_H", "CK" }).Any(x => nullable.Value.Value.Contains(x)) && io.IOType != IOType.输入)
        //                {
        //                    throw new Exception(string.Format("[{0}]应为输入继电器", nullable.Value.Key + "_" + nullable.Value.Value));
        //                }

        //                //if (nullable.Value.Value.EndsWith("Q"))
        //                //{
        //                //    if (!CheckIOExist(ioList, IOType.输入, nullable.Value.Key, nullable.Value.Value.Replace("_Q", "_H")))
        //                //    {
        //                //        throw new Exception(string.Format("[{0}]缺少对后节点应继电器", nullable.Value.Key + "_" + nullable.Value.Value));
        //                //    }
        //                //}
        //                //else if (nullable.Value.Value.EndsWith("H"))
        //                //{
        //                //    if (!CheckIOExist(ioList, IOType.输入, nullable.Value.Key, nullable.Value.Value.Replace("_Q", "_H")))
        //                //    {
        //                //        throw new Exception(string.Format("[{0}]缺少前节点继电器", nullable.Value.Key + "_" + nullable.Value.Value));
        //                //    }
        //                //}
        //                //else if (nullable.Value.Value.EndsWith("_CK"))
        //                //{
        //                //    if (!CheckIOExist(ioList, IOType.输入, nullable.Value.Key, nullable.Value.Value.Replace("_CK", string.Empty)))
        //                //    {
        //                //        throw new Exception(string.Format("[{0}]缺少对应输出继电器", nullable.Value.Key + "_" + nullable.Value.Value));
        //                //    }
        //                //}
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// 检查是否存在某个继电器
        ///// </summary>
        //private static bool CheckIOExist(List<IOMapModel> ioList, IOType ioType, string devName, string relay)
        //{
        //    foreach (IOMapModel io in ioList)
        //    {
        //        for (int i = 1; i <= 16; i++)
        //        {
        //            var nullable = io.RelayDictionary[i];

        //            if (nullable != null)
        //            {
        //                if (nullable.Value.Key == devName && nullable.Value.Value == relay)
        //                {
        //                    return true;
        //                }
        //            }
        //        }
        //    }

        //    return false;
        //}

        //#endregion
    }
}
