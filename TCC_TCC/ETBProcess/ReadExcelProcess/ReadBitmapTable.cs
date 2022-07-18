using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using NPOI.SS.UserModel;

using MSDG.CBI.ETB.MainTrack.B.ETBModel.ExcelModel;
using MSDG.CBI.ETB.MainTrack.B.ETBAttribute.ExcelModelAttribute;
using MSDG.CBI.ETB.MainTrack.B.ETBUtil;




namespace MSDG.CBI.ETB.MainTrack.B.ETBProcess.ReadExcelProcess
{
    /// <summary>
    /// 读取IOBitmap表
    /// </summary>
    public class ReadBitmapTable
    {
        private Action<Int32, String> _act;

        /// <summary>
        ///  读取IOBitmap图
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="ioMapFormatModellist"></param>
        /// <param name="nameToDeviceDic"></param>
        /// <param name="signalModelList"></param>
        /// <param name="blockModelList"></param>
        /// <param name="switchModelList"></param>
        /// <param name="cpzModelList"></param>
        /// <param name="pesbModelList"></param>
        /// <param name="psdModelList"></param>
        /// <param name="entryRouteModelList"></param>
        /// <param name="exitRouteModelList"></param>
        /// <param name="specialIOModelList"></param>
        /// <param name="specialConditionModelList"></param>
        /// <param name="spksModelList"></param>
        /// <param name="cycleRouteButtonModelList"></param>
        /// <param name="platformBuckleModelList"></param>
        /// <param name="msIOModel"></param>
        /// <param name="inputOutputNum"></param>
        /// <param name="isRailWay"></param>
        /// <param name="act"></param>
        public void ReadExcel(
            IWorkbook wb,
            List<IOMapFormatModel> ioMapFormatModellist,
            Dictionary<String, DeviceModelBase> nameToDeviceDic,
            List<SignalModel> signalModelList,
            List<BlockModel> blockModelList,
            List<SwitchModel> switchModelList,
            List<CPZModel> cpzModelList,
            List<PESBModel> pesbModelList,
            List<PSDModel> psdModelList,
            List<EntryRouteModel> entryRouteModelList,
            List<ExitRouteModel> exitRouteModelList,
            List<SpecialIOModel> specialIOModelList,
            List<SPKSModel> spksModelList,
            List<ATBModel> cycleRouteButtonModelList,
            List<PCCBModel> pccbModelList,
            List<PCDBModel> pcdbModelList,
            List<PHSModel> platformBuckleModelList,
            MSIOModel msIOModel,
            Dictionary<String, UInt32> inputOutputNum, Action<Int32, String> act)
        {
            this._act = act;
            var modelType = typeof(IOMapFormatModel);
            ExcelModelClassBaseAttribute memcAtribt = null;
            var classattributeType = typeof(MetroExcelModelClassAttribute);
            foreach (var att in modelType.GetCustomAttributes(classattributeType, false))
            {
                if (att is ExcelModelClassBaseAttribute)
                {
                    memcAtribt = (ExcelModelClassBaseAttribute)att;
                }
            }
            var typeName = memcAtribt.SheetName;
            var sheet = wb.GetSheet(typeName);
            if (sheet == null)
            {
                act(-1, String.Format("Excel标签页名称不正确,BitMap中没有找到名为{0}的标签页。", typeName));
            }
            var reg = new Regex(@"\d+");
            MatchCollection m = null;
            string input = string.Empty, output = string.Empty;
            var headerRow = sheet.GetRow(3);
            ICell iopointCell = null;
            Int32 cellCount = headerRow.LastCellNum;
            var rackNum = 0;
            var deviceName = String.Empty;
            string[] DeviceAndRelay;
            for (var k = 0; k < cellCount; )
            {
                var ioMapFormat = new IOMapFormatModel();
                for (var j = sheet.FirstRowNum + 21 * rackNum; j < 21 + 21 * rackNum; j++)
                {
                    var cellValue = sheet.GetRow(j).GetCell(k).ToString().PreprocessString();
                    if (cellValue.Equals(""))
                    {
                        continue;
                    }
                    var rowLayerIndex = (UInt32)(j % 21);
                    if (cellValue.Equals("输入") || cellValue.Equals("输出"))
                    {
                        ioMapFormat.InputOutpuType = cellValue;
                    }
                    else if (cellValue.EndsWith("槽道"))
                    {
                        var slotAndLayer = cellValue;
                        m = reg.Matches(slotAndLayer);
                        ioMapFormat.SlotNo = Convert.ToUInt32(m[1].Value);
                        ioMapFormat.LayerNo = Convert.ToUInt32(m[0].Value);
                        if (ioMapFormat.InputOutpuType != null)
                        {
                            if (ioMapFormat.InputOutpuType.Equals("输入"))
                            {
                                input += (((ioMapFormat.LayerNo - 1) * 15) + ioMapFormat.SlotNo - 1).ToString() + ",";
                            }
                            else
                            {
                                output += (((ioMapFormat.LayerNo - 1) * 15) + ioMapFormat.SlotNo - 1).ToString() + ",";
                            }
                        }
                    }
                    else if (cellValue.Equals("序号"))
                    {
                        continue;
                    }
                    else if (rowLayerIndex > 4)
                    {
                        iopointCell = sheet.GetRow(j).GetCell(k + 1);
                        if (iopointCell != null)
                        {
                            deviceName = iopointCell.ToString().PreprocessString();
                            if (!deviceName.Equals(""))
                            {
                                var rowIndex = (UInt32)(rowLayerIndex - 5 + 1);
                                DeviceAndRelay = StringOperation.ObtainDeviceNameAndRelay(deviceName);
                                if (ioMapFormat.LayerNo == 1 && ioMapFormat.SlotNo < 3)
                                {
                                    if (DeviceAndRelay[0] != msIOModel.MSList[(Int32)(rowIndex - 1 + ((ioMapFormat.SlotNo - 1) * 4))].Name)
                                    {
                                        act(-1, string.Format("第一层第{0}槽道点位为{1}名称为‘{2}’的主备信息不正确。", ioMapFormat.SlotNo.ToString(), rowIndex.ToString(), deviceName));
                                    }
                                }
                                else
                                {
                                    ioMapFormat.Dic[rowIndex] = new KeyValuePair<string, string>(DeviceAndRelay[0], DeviceAndRelay[1]);
                                }
                            }
                        }
                    }
                }
                if (ioMapFormat.LayerNo == 1 && ioMapFormat.SlotNo == 1 && ioMapFormat.InputOutpuType != "输入")
                {
                    act(-1, "第一层第一槽道应配置为输入");
                }
                if (ioMapFormat.LayerNo == 1 && ioMapFormat.SlotNo == 2 && ioMapFormat.InputOutpuType != "输出")
                {
                    act(-1, "第一层第二槽道应配置为输出");
                }
                if (ioMapFormat.InputOutpuType != null)
                {
                    ioMapFormatModellist.Add(ioMapFormat);
                }
                k += 2;
                if (k >= cellCount)
                {
                    k = 0;
                    rackNum++;
                    if (rackNum.Equals(8))
                    {
                        break;
                    }
                }
            }
            IOMapFormatModelToDeviceModel(ioMapFormatModellist, nameToDeviceDic, signalModelList, blockModelList, switchModelList, cpzModelList, pesbModelList, psdModelList, entryRouteModelList, exitRouteModelList, specialIOModelList, spksModelList, cycleRouteButtonModelList, pccbModelList, pcdbModelList, platformBuckleModelList, msIOModel, inputOutputNum);
            //判断道岔输出接点是否在同一子架上
            CheckSwitchOutRelay(ioMapFormatModellist, nameToDeviceDic);
            //找出联锁表中定义的了但没在bitmap上配置信息的设备

            var noBitMapDevice = new StringBuilder(64);
            (from keyvalue in nameToDeviceDic
             where !keyvalue.Value.IOPointDictionary.IsValuesChanged
             select keyvalue.Value).ToList().ForEach(device => noBitMapDevice.Append(String.Format("'{0}',", device.Name)));
            if (noBitMapDevice.Length > 0)
            {
                act(-1, String.Format("以下设备未配置BitMap信息：{0}", noBitMapDevice.ToString()));
            }
            act(0, "IOBitMap读取完毕");
        }


        /// <summary>
        /// 读取地铁数据
        /// </summary>
        /// <param name="ioMapFormatModellist">IOMap结构实体数据</param>
        /// <param name="nameToDeviceDic">名称设备映射字典</param>
        /// <param name="signalModelList">信号机设备数据集合</param>
        /// <param name="blockModelList">区段设备数据集合</param>
        /// <param name="switchModelList">道岔设备数据集合</param>
        /// <param name="cpzModelList">cpz设备数据集合</param>
        /// <param name="pesbModelList">pesb设备数据集合</param>
        /// <param name="psdModelList">psd设备数据集合</param>
        /// <param name="entryRouteModelList">进入进路设备数据集合</param>
        /// <param name="exitRouteModelList">退出进路设备数据集合</param>
        /// <param name="specialIOModelList">特殊IO设备数据集合</param>
        /// <param name="specialConditionModelList">特殊条件设备数据集合</param>
        /// <param name="msIOModel">msio数据集合</param>
        /// <param name="inputOutputNum">输入输出数映射</param>
        /// <returns>是否成功</returns>
        private Boolean IOMapFormatModelToDeviceModel(
            List<IOMapFormatModel> ioMapFormatModellist,
            Dictionary<String, DeviceModelBase> nameToDeviceDic,
            List<SignalModel> signalModelList,
            List<BlockModel> blockModelList,
            List<SwitchModel> switchModelList,
            List<CPZModel> cpzModelList,
            List<PESBModel> pesbModelList,
            List<PSDModel> psdModelList,
            List<EntryRouteModel> entryRouteModelList,
            List<ExitRouteModel> exitRouteModelList,
            List<SpecialIOModel> specialIOModelList,
            List<SPKSModel> spksModelList,
            List<ATBModel> cycleRouteButtonModelList,
            List<PCCBModel> pccbModelList,
            List<PCDBModel> pcdbModelList,
            List<PHSModel> platformBuckleModelList,
            MSIOModel msIOModel,
            Dictionary<String, UInt32> inputOutputNum
            )
        {
            var result = false;
            var PointNum = 16;
            UInt32 inputPointCounter = 0;
            UInt32 outputPointCounter = 0;
            UInt32 inputCounter = 0;
            UInt32 outputCounter = 0;
            var nullable = new KeyValuePair<string, string>?();
            if (ioMapFormatModellist != null)
            {
                var relayname = String.Empty;
                var missDevices = new List<string>(16);
                var missDevice = new StringBuilder(32);
                var unknownRelay = new StringBuilder(32);
                var repeatedIOMap = new StringBuilder(32);
                var faultRelay = new StringBuilder(32);
                foreach (var ioMapFormatModel in ioMapFormatModellist)
                {
                    List<DeviceModelBase> modelBaseList = null;
                    if (ioMapFormatModel.InputOutpuType == "输入")
                    {
                        for (UInt32 i = 1; i <= PointNum; i++)
                        {
                            nullable = ioMapFormatModel.Dic[i];
                            if (nullable == null)
                            {
                                continue;
                            }
                            relayname = nullable.Value.Value;
                            if (!nameToDeviceDic.ContainsKey(nullable.Value.Key))
                            {
                                if (!missDevices.Contains(nullable.Value.Key))
                                {
                                    missDevices.Add(nullable.Value.Key);
                                }
                                continue;
                            }

                            switch (nameToDeviceDic[nullable.Value.Key].Type)
                            {
                                case "Signal":
                                    modelBaseList = (from m in signalModelList
                                                     let devicebase = m as DeviceModelBase
                                                     where (devicebase.Name == nullable.Value.Key)
                                                     select devicebase).ToList();
                                    foreach (var signalModel in signalModelList)
                                    {
                                        if (signalModel.Name == nullable.Value.Key && signalModel.InternalDevice == UInt32.MaxValue)
                                        {
                                            //TODO：后期添加ZCJ_CK
                                            if (nullable.Value.Value == "ZXJ_CK" || nullable.Value.Value == "CXJ_CK" || nullable.Value.Value == "YXJ_CK")
                                            {
                                                relayname = nullable.Value.Value + "_R";
                                            }
                                            else if (nullable.Value.Value == "DJ" || nullable.Value.Value == "2DJ")
                                            {
                                                relayname = nullable.Value.Value + "_F";
                                            }
                                            else if (nullable.Value.Value == "ZXJ_CK_F" ||
                                                nullable.Value.Value == "ZXJ_CK_R" ||
                                                nullable.Value.Value == "CXJ_CK_F" ||
                                                nullable.Value.Value == "CXJ_CK_R" ||
                                                nullable.Value.Value == "YXJ_CK_F" ||
                                                nullable.Value.Value == "YXJ_CK_R" ||
                                                nullable.Value.Value == "DJ_F" ||
                                                nullable.Value.Value == "DJ_R" ||
                                                nullable.Value.Value == "2DJ_F" ||
                                                nullable.Value.Value == "2DJ_R")
                                            {
                                                faultRelay.AppendFormat("设备名称为“{0}”的“{1}”输入接点配置错误,该设备采用全采模式并非单采模式\r\n", nullable.Value.Key, nullable.Value.Value);
                                            }
                                            else
                                            {
                                                faultRelay.AppendFormat("设备名称为“{0}”的“{1}”输入接点配置错误或与同类型设备配置不一致\r\n", nullable.Value.Key, nullable.Value.Value);
                                                continue;
                                            }
                                            ++inputPointCounter;
                                        }
                                        else if (signalModel.Name == nullable.Value.Key && signalModel.InternalDevice == UInt32.MinValue)
                                        {
                                            if (nullable.Value.Value == "CXJ_F" ||
                                                nullable.Value.Value == "CXJ_R")
                                            {
                                                relayname = nullable.Value.Value.Replace("CXJ", "CXJ_CK");
                                            }
                                            if (nullable.Value.Value == "YXJ_F" ||
                                                nullable.Value.Value == "YXJ_R")
                                            {
                                                relayname = nullable.Value.Value.Replace("YXJ", "YXJ_CK");
                                            }
                                            ++inputPointCounter;
                                        }
                                    }
                                    //++inputPointCounter;
                                    break;

                                case "Switch":
                                    modelBaseList = (from m in switchModelList
                                                     let devicebase = m as DeviceModelBase
                                                     where (devicebase.Name == nullable.Value.Key)
                                                     select devicebase).ToList();
                                    if (nullable.Value.Value == "DBJ" || nullable.Value.Value == "FBJ" || nullable.Value.Value == "YCJ" || nullable.Value.Value == "BDJ" || nullable.Value.Value == "K7ENJ" || nullable.Value.Value == "LBJ" || nullable.Value.Value == "RBJ" || nullable.Value.Value == "NBJ" || nullable.Value.Value == "L1BJ" || nullable.Value.Value == "R1BJ" || nullable.Value.Value == "L2BJ" || nullable.Value.Value == "R2BJ")
                                    {
                                        //因为道岔继电器采用单采 所以不允许同时出现Sw1501_YCJ、Sw1501_YCJ_R,所以暂时不给Sw1501_YCJ_E赋值
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”输入接点配置错误,该设备采用单采模式并非全采模式\r\n", nullable.Value.Key, nullable.Value.Value);
                                        relayname = nullable.Value.Value + "_F";
                                    }
                                    else if (nullable.Value.Value == "ENJ_CK" || nullable.Value.Value == "SQJ_CK")
                                    {
                                        relayname = nullable.Value.Value + "_R";
                                    }
                                    else if (nullable.Value.Value == "DBJ_F" ||
                                        nullable.Value.Value == "DBJ_R" ||
                                        nullable.Value.Value == "FBJ_F" ||
                                        nullable.Value.Value == "FBJ_R" ||
                                        nullable.Value.Value == "ENJ_CK_F" ||
                                        nullable.Value.Value == "ENJ_CK_R" ||
                                        nullable.Value.Value == "YCJ_F" ||
                                        nullable.Value.Value == "YCJ_R" ||
                                        nullable.Value.Value == "BDJ_F" ||
                                        nullable.Value.Value == "BDJ_R" ||
                                        nullable.Value.Value == "SQJ_CK_F" ||
                                        nullable.Value.Value == "SQJ_CK_R" ||
                                        nullable.Value.Value == "K7ENJ_F" ||
                                        nullable.Value.Value == "K7ENJ_R" ||
                                        nullable.Value.Value == "LBJ_F" ||
                                        nullable.Value.Value == "LBJ_R" ||
                                        nullable.Value.Value == "RBJ_F" ||
                                        nullable.Value.Value == "RBJ_R" ||
                                        nullable.Value.Value == "NBJ_F" ||
                                        nullable.Value.Value == "NBJ_R" ||
                                        nullable.Value.Value == "L1BJ_F" ||
                                        nullable.Value.Value == "L1BJ_R" ||
                                        nullable.Value.Value == "R1BJ_F" ||
                                        nullable.Value.Value == "R1BJ_R" ||
                                        nullable.Value.Value == "L2BJ_F" ||
                                        nullable.Value.Value == "L2BJ_R" ||
                                        nullable.Value.Value == "R2BJ_F" ||
                                        nullable.Value.Value == "R2BJ_R")
                                    {
                                    }
                                    else
                                    {
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”输入接点配置错误或与同类型设备配置不一致\r\n", nullable.Value.Key, nullable.Value.Value);
                                        continue;
                                    }
                                    ++inputPointCounter;
                                    break;

                                case "Block":
                                    modelBaseList = (from m in blockModelList
                                                     let devicebase = m as DeviceModelBase
                                                     where (devicebase.Name == nullable.Value.Key)
                                                     select devicebase).ToList();
                                    if (nullable.Value.Value == "GJ")
                                    {
                                        //同道岔
                                        //因为道岔继电器采用单采 所以不允许同时出现Sw1501_YCJ、Sw1501_YCJ_R,所以暂时不给Sw1501_YCJ_E赋值
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”输入接点配置错误,该设备采用单采模式并非全采模式\r\n", nullable.Value.Key, nullable.Value.Value);
                                        relayname = nullable.Value.Value + "_F";
                                    }
                                    else if (nullable.Value.Value == "FWJ_CK")
                                    {
                                        relayname = nullable.Value.Value + "_R";
                                    }
                                    else if (nullable.Value.Value == "FWJ_CK_F" ||
                                        nullable.Value.Value == "FWJ_CK_R" ||
                                        nullable.Value.Value == "GJ_F" ||
                                        nullable.Value.Value == "GJ_R")
                                    {
                                    }
                                    else
                                    {
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”输入接点配置错误或与同类型设备配置不一致\r\n", nullable.Value.Key, nullable.Value.Value);
                                        continue;
                                    }
                                    ++inputPointCounter;
                                    break;

                                case "PSD":
                                    modelBaseList = (from m in psdModelList
                                                     let devicebase = m as DeviceModelBase
                                                     where (devicebase.Name == nullable.Value.Key)
                                                     select devicebase).ToList();
                                    if (nullable.Value.Value == "DSJ" || nullable.Value.Value == "IOJ")
                                    {
                                        //同道岔
                                        //因为道岔继电器采用单采 所以不允许同时出现Sw1501_YCJ、Sw1501_YCJ_R,所以暂时不给Sw1501_YCJ_E赋值
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”输入接点配置错误,该设备采用单采模式并非全采模式\r\n", nullable.Value.Key, nullable.Value.Value);
                                        relayname = nullable.Value.Value + "_F";
                                    }
                                    else if (nullable.Value.Value == "ECJ_CK")
                                    {
                                        relayname = nullable.Value.Value + "_R";
                                    }
                                    else if (nullable.Value.Value == "DSJ_F" ||
                                        nullable.Value.Value == "DSJ_R" ||
                                        nullable.Value.Value == "IOJ_F" ||
                                        nullable.Value.Value == "IOJ_R")
                                    {
                                    }
                                    else
                                    {
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”输入接点配置错误或与同类型设备配置不一致\r\n", nullable.Value.Key, nullable.Value.Value);
                                        continue;
                                    }
                                    ++inputPointCounter;
                                    break;

                                case "PESB":
                                    modelBaseList = (from m in pesbModelList
                                                     let devicebase = m as DeviceModelBase
                                                     where (devicebase.Name == nullable.Value.Key)
                                                     select devicebase).ToList();
                                    if (nullable.Value.Value == "JTJ")
                                    {
                                        //同道岔
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”输入接点配置错误,该设备采用单采模式并非全采模式\r\n", nullable.Value.Key, nullable.Value.Value);
                                        relayname = nullable.Value.Value + "_F";
                                    }
                                    else if (nullable.Value.Value == "JTJ_F" ||
                                        nullable.Value.Value == "JTJ_R")
                                    {
                                    }
                                    else
                                    {
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”输入接点配置错误或与同类型设备配置不一致\r\n", nullable.Value.Key, nullable.Value.Value);
                                        continue;
                                    }
                                    ++inputPointCounter;
                                    break;

                                case "CPZ":
                                    modelBaseList = (from m in cpzModelList
                                                     let devicebase = m as DeviceModelBase
                                                     where (devicebase.Name == nullable.Value.Key)
                                                     select devicebase).ToList();
                                    if (nullable.Value.Value == "CPAJ_CK")
                                    {
                                        relayname = nullable.Value.Value + "_R";
                                    }
                                    else if (nullable.Value.Value == "CP1J" || nullable.Value.Value == "CP2J")
                                    {
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”输入接点配置错误,该设备采用单采模式并非全采模式\r\n", nullable.Value.Key, nullable.Value.Value);
                                        relayname = nullable.Value.Value + "_F";
                                    }
                                    else if (nullable.Value.Value == "CP1J_F" ||
                                        nullable.Value.Value == "CP1J_R" ||
                                        nullable.Value.Value == "CP2J_F" ||
                                        nullable.Value.Value == "CP2J_R" ||
                                        nullable.Value.Value == "CPAJ_CK_F" ||
                                        nullable.Value.Value == "CPAJ_CK_R")
                                    {
                                    }
                                    else
                                    {
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”输入接点配置错误或与同类型设备配置不一致\r\n", nullable.Value.Key, nullable.Value.Value);
                                        continue;
                                    }
                                    ++inputPointCounter;
                                    break;

                                case "EntryRoute":
                                    modelBaseList = (from m in entryRouteModelList
                                                     let devicebase = m as DeviceModelBase
                                                     where (devicebase.Name == nullable.Value.Key)
                                                     select devicebase).ToList();

                                    if (nullable.Value.Value == "ZCJ_F" ||
                                        nullable.Value.Value == "ZCJ_R")
                                    {
                                    }
                                    else
                                    {
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”输入接点配置错误,该设备采用单采模式并非全采模式\r\n", nullable.Value.Key, nullable.Value.Value);
                                        continue;
                                    }
                                    ++inputPointCounter;
                                    break;

                                case "ExitRoute":
                                    modelBaseList = (from m in exitRouteModelList
                                                     let devicebase = m as DeviceModelBase
                                                     where (devicebase.Name == nullable.Value.Key)
                                                     select devicebase).ToList();
                                    if (nullable.Value.Value == "ZCJ_CK")
                                    {
                                        relayname = nullable.Value.Value + "_R";
                                    }
                                    else if (nullable.Value.Value == "ZCJ_CK_F" ||
                                            nullable.Value.Value == "ZCJ_CK_R")
                                    {
                                    }
                                    else
                                    {
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”输入接点配置错误或与同类型设备配置不一致\r\n", nullable.Value.Key, nullable.Value.Value);
                                        continue;
                                    }
                                    ++inputPointCounter;
                                    break;

                                case "ATB":
                                    modelBaseList = (from m in cycleRouteButtonModelList
                                                     let devicebase = m as DeviceModelBase
                                                     where (devicebase.Name == nullable.Value.Key)
                                                     select devicebase).ToList();
                                    if (nullable.Value.Value == "ATBJ")
                                    {
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”输入接点配置错误,该设备采用单采模式并非全采模式\r\n", nullable.Value.Key, nullable.Value.Value);
                                        relayname = nullable.Value.Value + "_F";
                                    }

                                    ++inputPointCounter;
                                    break;

                                case "PCCB":
                                    modelBaseList = (from m in pccbModelList
                                                     let devicebase = m as DeviceModelBase
                                                     where (devicebase.Name == nullable.Value.Key)
                                                     select devicebase).ToList();
                                    if (nullable.Value.Value == "PCCBJ")
                                    {
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”输入接点配置错误,该设备采用单采模式并非全采模式\r\n", nullable.Value.Key, nullable.Value.Value);
                                        relayname = nullable.Value.Value + "_F";
                                    }

                                    ++inputPointCounter;
                                    break;

                                case "PCDB":
                                    modelBaseList = (from m in pcdbModelList
                                                     let devicebase = m as DeviceModelBase
                                                     where (devicebase.Name == nullable.Value.Key)
                                                     select devicebase).ToList();
                                    if (nullable.Value.Value == "PCDBJ")
                                    {
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”输入接点配置错误,该设备采用单采模式并非全采模式\r\n", nullable.Value.Key, nullable.Value.Value);
                                        relayname = nullable.Value.Value + "_F";
                                    }

                                    ++inputPointCounter;
                                    break;

                                case "PHS":
                                    modelBaseList = (from m in platformBuckleModelList
                                                     let devicebase = m as DeviceModelBase
                                                     where (devicebase.Name == nullable.Value.Key)
                                                     select devicebase).ToList();
                                    if (nullable.Value.Value == "KCJ")
                                    {
                                        relayname = nullable.Value.Value + "_F";
                                    }
                                    ++inputPointCounter;
                                    break;

                                case "SPKS":
                                    modelBaseList = (from m in spksModelList
                                                     let devicebase = m as DeviceModelBase
                                                     where (devicebase.Name == nullable.Value.Key)
                                                     select devicebase).ToList();
                                    if (nullable.Value.Value == "SPJ")
                                    {
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”输入接点配置错误,该设备采用单采模式并非全采模式\r\n", nullable.Value.Key, nullable.Value.Value);
                                        relayname = nullable.Value.Value + "_F";
                                    }
                                    ++inputPointCounter;
                                    break;

                                case "SpecialIO":
                                    modelBaseList = (from m in specialIOModelList
                                                     let devicebase = m as DeviceModelBase
                                                     where (devicebase.Name == nullable.Value.Key)
                                                     select devicebase).ToList();
                                    if (nullable.Value.Value == "J")
                                    {
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”输入接点配置错误,该设备采用单采模式并非全采模式\r\n", nullable.Value.Key, nullable.Value.Value);
                                        relayname = nullable.Value.Value + "_F";
                                    }
                                    ++inputPointCounter;
                                    break;

                                case "MasterSlaveIO":
                                    modelBaseList = (from m in msIOModel.MSList
                                                     let devicebase = m as DeviceModelBase
                                                     where (devicebase.Name == nullable.Value.Key)
                                                     select devicebase).ToList();
                                    if (nullable.Value.Value == "J")
                                    {
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”输入接点配置错误,该设备采用单采模式并非全采模式\r\n", nullable.Value.Key, nullable.Value.Value);
                                        relayname = nullable.Value.Value + "_F";
                                    }
                                    ++inputPointCounter;
                                    break;
                            }
                            if (relayname.EndsWith("_F") || relayname.EndsWith("_R"))
                            {
                                foreach (var s in modelBaseList)
                                {
                                    try
                                    {
                                        var iobyte = relayname + "_Byte";
                                        var iobit = relayname + "_Bit";
                                        if (s.IOPointDictionary[iobyte] != UInt32.MaxValue || s.IOPointDictionary[iobit] != UInt32.MaxValue)
                                        {
                                            repeatedIOMap.AppendFormat("设备名称为“{0}”的“{1}”设备的继电器{2}多次设置点位信息\r\n", nullable.Value.Key, nameToDeviceDic[nullable.Value.Key].Type, relayname);
                                            continue;
                                        }
                                        s.IOPointDictionary[iobyte] = 2 * inputCounter + (i - 1) / 8;
                                        s.IOPointDictionary[iobit] = (i - 1) - ((i - 1) / 8) * 8;
                                    }
                                    catch (KeyNotFoundException)
                                    {
                                        unknownRelay.AppendFormat("设备类型为{0}的继电器名为{1}未定义,设备名称为：{2}\r\n", nameToDeviceDic[nullable.Value.Key].Type, relayname, nullable.Value.Key);
                                    }
                                }
                            }
                            else
                            {
                                unknownRelay.AppendFormat("设备类型为{0}的继电器名为{1}未定义,设备名称为：{2}\r\n", nameToDeviceDic[nullable.Value.Key].Type, nullable.Value.Value, nullable.Value.Key);
                            }
                        }
                        inputCounter++;
                    }
                    else if (ioMapFormatModel.InputOutpuType == "输出")
                    {
                        for (UInt32 i = 1; i <= PointNum; i++)
                        {
                            nullable = ioMapFormatModel.Dic[i];
                            if (nullable == null)
                            {
                                continue;
                            }
                            var relayAvailable = false;
                            if (!nameToDeviceDic.ContainsKey(nullable.Value.Key))
                            {
                                if (!missDevices.Contains(nullable.Value.Key))
                                {
                                    missDevices.Add(nullable.Value.Key);
                                }
                                continue;
                            }
                            switch (nameToDeviceDic[nullable.Value.Key].Type)
                            {
                                case "Signal":
                                    modelBaseList = (from m in signalModelList
                                                     let devicebase = m as DeviceModelBase
                                                     where (devicebase.Name == nullable.Value.Key)
                                                     select devicebase).ToList();
                                    //TODO：后期添加ZC
                                    if (nullable.Value.Value == "ZXJ" || nullable.Value.Value == "CXJ" || nullable.Value.Value == "YXJ" || nullable.Value.Value == "MDJ")
                                    {
                                        relayAvailable = true;
                                    }
                                    else
                                    {
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”继电器不是输出继电器\r\n", nullable.Value.Key, nullable.Value.Value);
                                        continue;
                                    }
                                    ++outputPointCounter;
                                    break;

                                case "Switch":
                                    modelBaseList = (from m in switchModelList
                                                     let devicebase = m as DeviceModelBase
                                                     where (devicebase.Name == nullable.Value.Key)
                                                     select devicebase).ToList();
                                    if (nullable.Value.Value == "DCJ" || nullable.Value.Value == "FCJ" || nullable.Value.Value == "ENJ" || nullable.Value.Value == "SQJ" || nullable.Value.Value == "LCJ" || nullable.Value.Value == "RCJ" || nullable.Value.Value == "NCJ" || nullable.Value.Value == "L1CJ" || nullable.Value.Value == "R1CJ" || nullable.Value.Value == "L2CJ" || nullable.Value.Value == "R2CJ")
                                    {
                                        relayAvailable = true;
                                    }
                                    else
                                    {
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”继电器不是输出继电器\r\n", nullable.Value.Key, nullable.Value.Value);
                                        continue;
                                    }

                                    ++outputPointCounter;
                                    break;

                                case "Block":
                                    modelBaseList = (from m in blockModelList
                                                     let devicebase = m as DeviceModelBase
                                                     where (devicebase.Name == nullable.Value.Key)
                                                     select devicebase).ToList();
                                    if (nullable.Value.Value == "FWJ")
                                    {
                                        relayAvailable = true;
                                    }
                                    else
                                    {
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”继电器不是输出继电器\r\n", nullable.Value.Key, nullable.Value.Value);
                                        continue;
                                    }
                                    ++outputPointCounter;
                                    break;

                                case "PSD":
                                    modelBaseList = (from m in psdModelList
                                                     let devicebase = m as DeviceModelBase
                                                     where (devicebase.Name == nullable.Value.Key)
                                                     select devicebase).ToList();
                                    if (nullable.Value.Value == "OCJ" || nullable.Value.Value == "ECJ" || nullable.Value.Value == "CCJ")
                                    {
                                        relayAvailable = true;
                                    }
                                    else
                                    {
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”继电器不是输出继电器\r\n", nullable.Value.Key, nullable.Value.Value);
                                        continue;
                                    }
                                    ++outputPointCounter;
                                    break;

                                case "CPZ":
                                    modelBaseList = (from m in cpzModelList
                                                     let devicebase = m as DeviceModelBase
                                                     where (devicebase.Name == nullable.Value.Key)
                                                     select devicebase).ToList();
                                    if (nullable.Value.Value == "CPAJ")
                                    {
                                        relayAvailable = true;
                                    }
                                    else
                                    {
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”继电器不是输出继电器\r\n", nullable.Value.Key, nullable.Value.Value);
                                        continue;
                                    }
                                    ++outputPointCounter;
                                    break;

                                case "ExitRoute":
                                    modelBaseList = (from m in exitRouteModelList
                                                     let devicebase = m as DeviceModelBase
                                                     where (devicebase.Name == nullable.Value.Key)
                                                     select devicebase).ToList();
                                    if (nullable.Value.Value == "ZCJ")
                                    {
                                        relayAvailable = true;
                                    }
                                    else
                                    {
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”继电器不是输出继电器\r\n", nullable.Value.Key, nullable.Value.Value);
                                        continue;
                                    }
                                    ++outputPointCounter;
                                    break;

                                case "SPKS":
                                    modelBaseList = (from m in spksModelList
                                                     let devicebase = m as DeviceModelBase
                                                     where (devicebase.Name == nullable.Value.Key)
                                                     select devicebase).ToList();
                                    if (nullable.Value.Value == "SPIJ")
                                    {
                                        relayAvailable = true;
                                    }
                                    else
                                    {
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”继电器不是输出继电器\r\n", nullable.Value.Key, nullable.Value.Value);
                                        continue;
                                    }
                                    ++outputPointCounter;
                                    break;

                                case "SpecialIO":
                                    modelBaseList = (from m in specialIOModelList
                                                     let devicebase = m as DeviceModelBase
                                                     where (devicebase.Name == nullable.Value.Key)
                                                     select devicebase).ToList();
                                    if (nullable.Value.Value == "J_F")
                                    {
                                        relayAvailable = true;
                                    }
                                    else
                                    {
                                        faultRelay.AppendFormat("设备名称为“{0}”的“{1}”继电器不是输出继电器\r\n", nullable.Value.Key, nullable.Value.Value);
                                        continue;
                                    }
                                    ++outputPointCounter;
                                    break;

                                case "MasterSlaveIO":
                                    modelBaseList = (from m in msIOModel.MSList
                                                     let devicebase = m as DeviceModelBase
                                                     where (devicebase.Name == nullable.Value.Key)
                                                     select devicebase).ToList();
                                    if (nullable.Value.Value == "J")
                                    {
                                        relayname = nullable.Value.Value + "_F";
                                    }
                                    if (relayname.EndsWith("_F"))
                                    {
                                        relayAvailable = true;
                                    }
                                    ++outputPointCounter;
                                    break;
                            }
                            if (relayAvailable)
                            {
                                foreach (var s in modelBaseList)
                                {
                                    var iobyte = nullable.Value.Value + "_Byte";
                                    var iobit = nullable.Value.Value + "_Bit";
                                    if (s.IOPointDictionary[iobyte] != UInt32.MaxValue || s.IOPointDictionary[iobit] != UInt32.MaxValue)
                                    {
                                        repeatedIOMap.AppendFormat("设备名称为“{0}”的“{1}”设备的继电器{2}多次设置点位信息\r\n", nullable.Value.Key, nameToDeviceDic[nullable.Value.Key].Type, nullable.Value.Value);
                                        continue;
                                    }
                                    s.IOPointDictionary[iobyte] = 2 * outputCounter + (i - 1) / 8;
                                    s.IOPointDictionary[iobit] = (i - 1) - ((i - 1) / 8) * 8;
                                }
                            }
                            else
                            {
                                unknownRelay.AppendFormat("设备类型为{0}的继电器名为{1}未定义,设备名称为：'{2}'\r\n", nameToDeviceDic[nullable.Value.Key].Type, nullable.Value.Value, nullable.Value.Key);
                            }
                        }
                        outputCounter++;
                    }
                }
                missDevices.ForEach(s => missDevice.AppendFormat("'{0}',", s));
                result = missDevice.Length <= 0;
                this.PrintErrorMessage(missDevice, unknownRelay, repeatedIOMap, faultRelay);
            }
            inputOutputNum.Add("InputDataLength", inputCounter * 2);
            inputOutputNum.Add("OutputDataLength", outputCounter * 2);
            return result;
        }

        /// <summary>
        /// 打印信息
        /// </summary>
        /// <param name="messages">信息数组，注意顺序</param>
        private void PrintErrorMessage(params StringBuilder[] messages)
        {
            if (messages[0].Length > 0)
            {
                _act(-1, String.Format("IOBitMap中填写了联锁表中未定义的设备：{0}", messages[0].ToString()));
            }
            if (messages[1].Length > 0)
            {
                _act(-1, String.Format("未知的继电器：{0}", messages[1].ToString()));
            }
            if (messages[2].Length > 0)
            {
                _act(-1, String.Format("重复的IO点位：{0}", messages[2].ToString()));
            }
            if (messages[3].Length > 0)
            {
                _act(-1, String.Format("输入输出继电器填写错误：{0}", messages[3].ToString()));
            }
        }

        /// <summary>
        /// 检查道岔和屏蔽门输出接点是否配置在同一子架
        /// </summary>
        /// <param name="ioBitmapModelList"></param>
        /// <param name="nameToDeviceDic"></param>
        private void CheckSwitchOutRelay(List<IOMapFormatModel> ioBitmapModelList, Dictionary<String, DeviceModelBase> nameToDeviceDic)
        {
            var relayname = String.Empty;
            var deviceName = String.Empty;
            var nullable = new KeyValuePair<string, string>?();
            uint _LayerNo = 0;
            var switchDCJ = new Dictionary<string, uint>();
            var switchFCJ = new Dictionary<string, uint>();
            var switchENJ = new Dictionary<string, uint>();

            var switchSQJ = new Dictionary<string, uint>();
            var switchLCJ = new Dictionary<string, uint>();
            var switchRCJ = new Dictionary<string, uint>();
            var switchNCJ = new Dictionary<string, uint>();
            var switchL1CJ = new Dictionary<string, uint>();
            var switchR1CJ = new Dictionary<string, uint>();
            var switchL2CJ = new Dictionary<string, uint>();
            var switchR2CJ = new Dictionary<string, uint>();

            var psdECJ = new Dictionary<string, uint>();
            var psdOCJ = new Dictionary<string, uint>();
            var psdCCJ = new Dictionary<string, uint>();

            foreach (var ioMapFormatModel in ioBitmapModelList)
            {
                if (ioMapFormatModel.InputOutpuType == "输出")
                {
                    try
                    {
                        for (UInt32 i = 1; i <= 16; i++)
                        {
                            nullable = ioMapFormatModel.Dic[i];
                            if (nullable == null)
                            {
                                continue;
                            }
                            relayname = nullable.Value.Value;
                            deviceName = nullable.Value.Key;
                            if (nameToDeviceDic[nullable.Value.Key].Type == "Switch")
                            {
                                _LayerNo = ioMapFormatModel.LayerNo;
                                switch (relayname)
                                {
                                    case "DCJ":
                                        if (!switchDCJ.ContainsKey(deviceName))
                                        {
                                            switchDCJ.Add(deviceName, _LayerNo);
                                        }
                                        break;

                                    case "FCJ":
                                        if (!switchFCJ.ContainsKey(deviceName))
                                        {
                                            switchFCJ.Add(deviceName, _LayerNo);
                                        }
                                        break;

                                    case "ENJ":
                                        if (!switchENJ.ContainsKey(deviceName))
                                        {
                                            switchENJ.Add(deviceName, _LayerNo);
                                        }
                                        break;

                                    case "SQJ":
                                        if (!switchSQJ.ContainsKey(deviceName))
                                        {
                                            switchSQJ.Add(deviceName, _LayerNo);
                                        }
                                        break;

                                    case "LCJ":
                                        if (!switchLCJ.ContainsKey(deviceName))
                                        {
                                            switchLCJ.Add(deviceName, _LayerNo);
                                        }
                                        break;

                                    case "RCJ":
                                        if (!switchRCJ.ContainsKey(deviceName))
                                        {
                                            switchRCJ.Add(deviceName, _LayerNo);
                                        }
                                        break;

                                    case "NCJ":
                                        if (!switchNCJ.ContainsKey(deviceName))
                                        {
                                            switchNCJ.Add(deviceName, _LayerNo);
                                        }
                                        break;

                                    case "L1CJ":
                                        if (!switchL1CJ.ContainsKey(deviceName))
                                        {
                                            switchL1CJ.Add(deviceName, _LayerNo);
                                        }
                                        break;

                                    case "R1CJ":
                                        if (!switchR1CJ.ContainsKey(deviceName))
                                        {
                                            switchR1CJ.Add(deviceName, _LayerNo);
                                        }
                                        break;

                                    case "L2CJ":
                                        if (!switchL2CJ.ContainsKey(deviceName))
                                        {
                                            switchL2CJ.Add(deviceName, _LayerNo);
                                        }
                                        break;

                                    case "R2CJ":
                                        if (!switchR2CJ.ContainsKey(deviceName))
                                        {
                                            switchR2CJ.Add(deviceName, _LayerNo);
                                        }
                                        break;
                                }
                            }
                            else if (nameToDeviceDic[nullable.Value.Key].Type == "PSD")
                            {
                                _LayerNo = ioMapFormatModel.LayerNo;
                                switch (relayname)
                                {
                                    case "ECJ":
                                        if (!psdECJ.ContainsKey(deviceName))
                                        {
                                            psdECJ.Add(deviceName, _LayerNo);
                                        }
                                        break;

                                    case "OCJ":
                                        if (!psdOCJ.ContainsKey(deviceName))
                                        {
                                            psdOCJ.Add(deviceName, _LayerNo);
                                        }
                                        break;

                                    case "CCJ":
                                        if (!psdCCJ.ContainsKey(deviceName))
                                        {
                                            psdCCJ.Add(deviceName, _LayerNo);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        //do nothing
                    }
                }
            }
            switchDCJ.Where(temp => switchFCJ.ContainsKey(temp.Key) && switchENJ.ContainsKey(temp.Key)
                && switchSQJ.ContainsKey(temp.Key) && switchLCJ.ContainsKey(temp.Key)
                && switchRCJ.ContainsKey(temp.Key) && switchNCJ.ContainsKey(temp.Key)
                && switchL1CJ.ContainsKey(temp.Key) && switchR1CJ.ContainsKey(temp.Key)
                && switchL2CJ.ContainsKey(temp.Key) && switchR2CJ.ContainsKey(temp.Key))
                .Where(temp => switchFCJ[temp.Key] != temp.Value || switchENJ[temp.Key] != temp.Value
                    || switchSQJ[temp.Key] != temp.Value || switchLCJ[temp.Key] != temp.Value
                    || switchRCJ[temp.Key] != temp.Value || switchNCJ[temp.Key] != temp.Value
                    || switchL1CJ[temp.Key] != temp.Value || switchR1CJ[temp.Key] != temp.Value
                    || switchL2CJ[temp.Key] != temp.Value || switchR2CJ[temp.Key] != temp.Value).Foreach(temp =>
                    {
                        _act(-1, String.Format("IOBitmap中，道岔{0}输出接点未配置在同一子架上", temp.Key));
                    });

            psdECJ.Where(temp => psdCCJ.ContainsKey(temp.Key) && psdOCJ.ContainsKey(temp.Key))
                .Where(temp => psdCCJ[temp.Key] != temp.Value || psdOCJ[temp.Key] != temp.Value).Foreach(temp =>
                {
                    _act(-1, String.Format("IOBitmap中，屏蔽门{0}输出接点未配置在同一子架上", temp.Key));
                });
        }
    }
}