using System;
using System.Collections.Generic;
using System.Linq;

using NPOI.SS.UserModel;

using MSDG.ATS.ATSAttribute.ExcelModelAttribute;
using MSDG.ATS.ATSConfig.DeviceAttributeEnum;
using MSDG.ATS.ATSModel.ExcelModel;
using MSDG.ATS.ATSConfig.DataTypeStruct;
using MSDG.ATS.ATSConfig.DataTypeEnum;
using MSDG.ATS.ATSUtil.TopoGraphicHelper.DesignData.DesignDataEnum;
using MSDG.ATS.ATSUtil;

namespace MSDG.ATS.ATSProcess.ReadExcelProcess
{
    /// <summary>
    /// 读取ATSData数据
    /// </summary>
    public class ReadATSDataTable
    {
        private String _deviceName;
        private Int32 _deviceCount;

        /// <summary>
        /// 通过特性从Excel的sheet中读取数据到设备Model
        /// </summary>
        /// <typeparam name="TExcelModel">Excel实体类型</typeparam>
        /// <param name="wb">Excel标签页</param>
        /// <param name="excelModelBaseList">Excel实体数据集合</param>
        /// <param name="act">打印函数（委托）</param>
        public void ReadAtsExcel<TExcelModel>(
            IWorkbook wb,
            List<TExcelModel> excelModelBaseList,
            Dictionary<String, ExcelSheetModelBase> nameToDeviceDic,
            Action<Int32, String> act)
            where TExcelModel : ExcelSheetModelBase, new()
        {
            nameToDeviceDic.Clear();
            this._deviceCount = 0;
            this._deviceName = string.Empty;
            var repeatedId = string.Empty;
            TExcelModel tExcelModel;
            var modelType = typeof(TExcelModel);
            ExcelModelClassBaseAttribute memcAtribt = null;
            ExcelModelPropertyBaseAttribute remcAtribt;
            String typeName;
            ISheet sheet;
            Int32 deviceSheetKeyColumnIndex;

            var classattributeType = typeof(MetroExcelModelClassAttribute);
            var propertyattributeType = typeof(MetroExcelModelPropertyAttribute);
            foreach (var att in modelType.GetCustomAttributes(classattributeType, false))
            {
                if (att is ExcelModelClassBaseAttribute)
                {
                    memcAtribt = (ExcelModelClassBaseAttribute)att;
                }
            }
            typeName = memcAtribt.SheetName;
            deviceSheetKeyColumnIndex = memcAtribt.SheetKeyColumn;

            sheet = wb.GetSheet(typeName);
            if (sheet == null)
            {
                throw new Exception(String.Format("Excel标签页名称不正确,没有找到名为{0}的标签页。", typeName));
            }
            Int32 rowIndex = memcAtribt.HeaderIndex;

            for (var i = rowIndex; i < sheet.LastRowNum + 1; i++)
            {
                Object tempObject;
                IRow row;
                row = sheet.GetRow(i);
                if (row == null)
                {
                    break;
                }

                if (((row.GetCell(deviceSheetKeyColumnIndex) == null) || (row.GetCell(deviceSheetKeyColumnIndex).ToString().PreprocessString() == "")))
                {
                    if (modelType.Name == "PlatformModel" || modelType.Name == "ZoneModel" ||
                        modelType.Name == "RouteModel" || modelType.Name == "BlockModel" ||
                        modelType.Name == "ZCPathModel" || modelType.Name == "TrackZoneModel" ||
                        modelType.Name == "SwitchModel" || modelType.Name == "StationModel" ||
                        modelType.Name == "TravelTimeBtwPlatformModel" || modelType.Name == "ISPModel" ||
                        modelType.Name == "CBIRouteModel" || modelType.Name == "RegionModel" || modelType.Name == "EntryPortalModel"
                        || modelType.Name == "WashMachineModel" || modelType.Name == "WetAreaModel" || modelType.Name == "FASModel" ||
                        modelType.Name == "PowerSectionModel")
                    {
                        continue;
                    }
                    else
                    {
                        if ((row.GetCell(deviceSheetKeyColumnIndex + 1) != null) && (row.GetCell(deviceSheetKeyColumnIndex + 1).ToString().PreprocessString() != ""))
                        {
                            act(-1, String.Format("表中的'{0}'页第{1}行有误检查，可能是由于未填写设备名称", typeName, i + 1));

                        }
                        break;
                    }

                }

                tExcelModel = new TExcelModel();
                foreach (var property in modelType.GetProperties())
                {
                    var tempRouteCfg = new RouteConfig();
                    var tempSwAlig = new SwitchAlignment();
                    TravelTime tempTravelTime = new TravelTime();
                    WetPlatform tempWetPlatform = new WetPlatform();
                    UInt16 tempFASPlatform = 0;
                    var tempRouteCfgList = new List<RouteConfig>();
                    var tempCoordinateList = new List<Coordinate>();
                    var tempSwAligList = new List<SwitchAlignment>();
                    var tempTravelTimeList = new List<TravelTime>();
                    var tempWetPlatformList = new List<WetPlatform>();
                    var tempFASPlatformList = new List<UInt16>();
                    var tempStrList = new List<string>();

                    tempObject = null;
                    foreach (var attribute in property.GetCustomAttributes(propertyattributeType, true))
                    {
                        if (attribute is ExcelModelPropertyBaseAttribute)
                        {
                            remcAtribt = attribute as ExcelModelPropertyBaseAttribute;
                            String cellValue = String.Empty;
                            if (remcAtribt.PropertyType != ConstantInternal.ExcelModelPropertyType.IndexType)
                            {
                                ICell cell = row.GetCell(row.FirstCellNum + remcAtribt.Index);
                                cell.SetCellType(CellType.String);
                                cellValue = cell.StringCellValue.PreprocessString();
                            }

                            switch (remcAtribt.PropertyType)
                            {
                                case ConstantInternal.ExcelModelPropertyType.StringType:
                                    tempObject = cellValue;
                                    break;

                                case ConstantInternal.ExcelModelPropertyType.Value16Type:
                                    try
                                    {
                                        if (cellValue == "-" || cellValue == "")
                                        {
                                            tempObject = UInt16.MinValue;
                                        }
                                        else
                                        {
                                            tempObject = Convert.ToUInt16(cellValue);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        act(-1, String.Format("设备'{0}'的第{1}行的{2}格式不正确", tExcelModel.Type, i + 1, property.Name));
                                    }
                                    break;

                                case ConstantInternal.ExcelModelPropertyType.Value32Type:
                                    try
                                    {
                                        if (cellValue == "-" || cellValue == "")
                                        {
                                            tempObject = UInt32.MinValue;
                                        }
                                        else
                                        {
                                            tempObject = Convert.ToUInt32(cellValue);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        act(-1, String.Format("设备'{0}'的第{1}行的{2}格式不正确", tExcelModel.Type, i + 1, property.Name));
                                    }
                                    break;

                                case ConstantInternal.ExcelModelPropertyType.IndexType:
                                    tempObject = (UInt32)(i - 1);
                                    break;

                                case ConstantInternal.ExcelModelPropertyType.DecimalType:
                                    tempObject = Convert.ToDecimal(cellValue);
                                    break;

                                case ConstantInternal.ExcelModelPropertyType.BlockType:
                                    EBlockType blockType = EBlockType.UNKNOWN;
                                    if (Enum.TryParse<EBlockType>(cellValue, out blockType))
                                    {
                                        tempObject = blockType;
                                    }
                                    else
                                    {
                                        act(-1, String.Format("设备'{0}'的第{1}行的{2}格式不正确", tExcelModel.Type, i + 1, property.Name));
                                    }

                                    break;

                                case ConstantInternal.ExcelModelPropertyType.WetAreaType:
                                    EWetAreaType wetAreaType = EWetAreaType.UNKNOWN;
                                    if (Enum.TryParse<EWetAreaType>(cellValue, out wetAreaType))
                                    {
                                        tempObject = wetAreaType;
                                    }
                                    else
                                    {
                                        act(-1, String.Format("设备'{0}'的第{1}行的{2}格式不正确", tExcelModel.Type, i + 1, property.Name));
                                    }

                                    break;

                                case ConstantInternal.ExcelModelPropertyType.BoolValueType:
                                    if (cellValue == "Y" || cellValue == "Virtual" || cellValue == "YES" || cellValue == "1")
                                    {
                                        tempObject = true;
                                    }
                                    else if (cellValue == "NotVirtual" || cellValue == "N" || cellValue == "NO" || cellValue == "0" || cellValue == "")
                                    {
                                        tempObject = false;
                                    }
                                    else
                                    {
                                        act(-1, String.Format("设备'{0}'的第{1}行的{2}格式不正确", tExcelModel.Type, i + 1, property.Name));
                                    }
                                    break;

                                case ConstantInternal.ExcelModelPropertyType.ListArray:
                                    if ((modelType.Name == "ZoneModel"
                                        || modelType.Name == "BlockModel"
                                        || modelType.Name == "ZCPathModel"
                                        || modelType.Name == "ISPModel"
                                        || modelType.Name == "RegionModel"
                                        || modelType.Name == "CBIRouteModel"
                                        || modelType.Name == "EntryPortalModel"
                                        || modelType.Name == "PlatformModel"
                                        || modelType.Name == "TrackZoneModel"
                                        || modelType.Name == "WashMachineModel"
                                        || modelType.Name == "PowerSectionModel") &&
                                        !(string.IsNullOrEmpty(row.GetCell(deviceSheetKeyColumnIndex).ToString())))
                                    {
                                        this.ProcessCoordinateList(tempCoordinateList, sheet, i, remcAtribt.Index, deviceSheetKeyColumnIndex);
                                    }
                                    else if ((modelType.Name == "RouteModel") && !(string.IsNullOrEmpty(row.GetCell(deviceSheetKeyColumnIndex).ToString())))
                                    {
                                        for (var k = i; k < sheet.LastRowNum + 1; k++)
                                        {
                                            IRow tempRow = sheet.GetRow(k);
                                            if (tempRow == null)
                                            {
                                                break;
                                            }
                                            if (string.IsNullOrEmpty(tempRow.GetCell(tempRow.FirstCellNum).ToString()) || (k == i))
                                            {
                                                for (var j = remcAtribt.Index; j < remcAtribt.Index + 3; j++)
                                                {
                                                    tempObject = (tempRow.GetCell(j)).ToString();
                                                    if (tempObject == "" || tempObject == null)
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        if (j == remcAtribt.Index)
                                                        {
                                                            tempRouteCfg.NodeId = Convert.ToUInt16(tempObject);
                                                        }
                                                        else if (j == remcAtribt.Index + 1)
                                                        {
                                                            //tempRouteCfg.GuidewayDirection = Convert.ToUInt16(tempObject);
                                                            tempRouteCfg.GuidewayDirection = (EDirection)Enum.Parse(
                                                                    typeof(EDirection),
                                                                    tempObject.ToString());
                                                        }
                                                        else if (j == remcAtribt.Index + 2)
                                                        {
                                                            tempRouteCfg.SwPos = tempObject.ToString();
                                                        }
                                                    }
                                                }
                                                if (tempRouteCfg.NodeId != 0)
                                                {
                                                    tempRouteCfgList.Add(tempRouteCfg);
                                                }
                                            }
                                            else
                                            {
                                                if (k != i)
                                                    break;
                                            }
                                        }

                                    }
                                    else if ((modelType.Name == "SwitchModel") && !(string.IsNullOrEmpty(row.GetCell(deviceSheetKeyColumnIndex).ToString())))
                                    {
                                        for (var k = i; k < sheet.LastRowNum + 1; k++)
                                        {
                                            IRow tempRow = sheet.GetRow(k);
                                            if (tempRow == null)
                                            {
                                                break;
                                            }
                                            if (string.IsNullOrEmpty(tempRow.GetCell(tempRow.FirstCellNum).ToString()) || (k == i))
                                            {
                                                for (var j = remcAtribt.Index; j < remcAtribt.Index + 3; j++)
                                                {
                                                    tempObject = (tempRow.GetCell(j)).ToString();
                                                    if (tempObject == "")
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        if (j == remcAtribt.Index)
                                                        {
                                                            tempSwAlig.SwitchPos = tempObject.ToString();
                                                        }
                                                        else if (j == remcAtribt.Index + 1)
                                                        {
                                                            tempSwAlig.Edge1 = Convert.ToUInt16(tempObject);
                                                        }
                                                        else if (j == remcAtribt.Index + 2)
                                                        {
                                                            tempSwAlig.Edge2 = Convert.ToUInt16(tempObject);
                                                        }
                                                    }
                                                }
                                                tempSwAligList.Add(tempSwAlig);
                                            }
                                            else
                                            {
                                                if (k != i)
                                                    break;
                                            }
                                        }
                                    }
                                    else if ((modelType.Name == "TravelTimeBtwPlatformModel") && !(string.IsNullOrEmpty(row.GetCell(deviceSheetKeyColumnIndex).ToString())))
                                    {
                                        for (var k = i; k < sheet.LastRowNum + 1; k++)
                                        {
                                            IRow tempRow = sheet.GetRow(k);
                                            if (tempRow == null)
                                            {
                                                break;
                                            }
                                            tempObject = (tempRow.GetCell(remcAtribt.Index));
                                            if (tempObject == null)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                if (string.IsNullOrEmpty(tempRow.GetCell(tempRow.FirstCellNum).ToString()) || (k == i))
                                                {
                                                    for (var j = remcAtribt.Index; j < remcAtribt.Index + 2; j++)
                                                    {
                                                        if (tempRow.GetCell(j).CellType == CellType.Formula)
                                                        {
                                                            tempRow.GetCell(j).SetCellType(CellType.String);
                                                        }

                                                        tempObject = (tempRow.GetCell(j)).ToString();
                                                        if (tempObject == "")
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            if (j == remcAtribt.Index)
                                                            {
                                                                tempTravelTime.Level = Convert.ToUInt16(tempObject);
                                                            }
                                                            else if (j == remcAtribt.Index + 1)
                                                            {
                                                                tempTravelTime.Time = Convert.ToDecimal(tempObject);
                                                            }
                                                        }
                                                    }
                                                    tempTravelTimeList.Add(tempTravelTime);
                                                }
                                                else
                                                {
                                                    if (k != i)
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                    else if ((modelType.Name == "StationModel") && !(string.IsNullOrEmpty(row.GetCell(deviceSheetKeyColumnIndex).ToString())))
                                    {
                                        for (var k = i; k < sheet.LastRowNum + 1; k++)
                                        {
                                            IRow tempRow = sheet.GetRow(k);
                                            if (tempRow == null)
                                            {
                                                break;
                                            }
                                            tempObject = (tempRow.GetCell(remcAtribt.Index));
                                            if (tempObject == null)
                                            {
                                                break;
                                            }
                                            {
                                                if (string.IsNullOrEmpty(tempRow.GetCell(tempRow.FirstCellNum).ToString()) || (k == i))
                                                {
                                                    tempStrList.Add(tempObject.ToString());
                                                }
                                                else
                                                {
                                                    if (k != i)
                                                        break;
                                                }
                                            }
                                        }
                                    }

                                    else if ((modelType.Name == "WetAreaModel") && !(string.IsNullOrEmpty(row.GetCell(deviceSheetKeyColumnIndex).ToString())))
                                    {
                                        for (var k = i; k < sheet.LastRowNum + 1; k++)
                                        {
                                            IRow tempRow = sheet.GetRow(k);
                                            if (tempRow == null)
                                            {
                                                break;
                                            }
                                            tempObject = (tempRow.GetCell(remcAtribt.Index));
                                            if (tempObject == null)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                if (string.IsNullOrEmpty(tempRow.GetCell(tempRow.FirstCellNum).ToString()) || (k == i))
                                                {
                                                    for (var j = remcAtribt.Index; j < remcAtribt.Index + 3; j++)
                                                    {
                                                        tempObject = (tempRow.GetCell(j)).ToString();
                                                        if (tempObject == "")
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            if (j == remcAtribt.Index)
                                                            {
                                                                tempWetPlatform.PlatformDirId = Convert.ToUInt16(tempObject);
                                                            }
                                                            else if (j == remcAtribt.Index + 1)
                                                            {
                                                                tempWetPlatform.AssPlatformId = Convert.ToUInt16(tempObject);
                                                            }
                                                            else if (j == remcAtribt.Index + 2)
                                                            {
                                                                tempWetPlatform.Direction = (EDirection)Enum.Parse(
                                                                    typeof(EDirection),
                                                                    tempObject.ToString());
                                                            }
                                                        }
                                                    }
                                                    tempWetPlatformList.Add(tempWetPlatform);
                                                }
                                                else
                                                {
                                                    if (k != i)
                                                        break;
                                                }
                                            }
                                        }
                                    }

                                    else if ((modelType.Name == "FASModel") && !(string.IsNullOrEmpty(row.GetCell(deviceSheetKeyColumnIndex).ToString())))
                                    {
                                        for (var k = i; k < sheet.LastRowNum + 1; k++)
                                        {
                                            IRow tempRow = sheet.GetRow(k);
                                            if (tempRow == null)
                                            {
                                                break;
                                            }
                                            tempObject = (tempRow.GetCell(remcAtribt.Index));
                                            if (tempObject == null)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                if (string.IsNullOrEmpty(tempRow.GetCell(tempRow.FirstCellNum).ToString()) || (k == i))
                                                {

                                                    tempObject = (tempRow.GetCell(remcAtribt.Index)).ToString();
                                                    if (tempObject == "")
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        tempFASPlatform = Convert.ToUInt16(tempObject);
                                                    }
                                                    tempFASPlatformList.Add(tempFASPlatform);
                                                }
                                                else
                                                {
                                                    if (k != i)
                                                        break;
                                                }
                                            }
                                        }
                                    }

                                    break;
                            }

                            switch (property.Name)
                            {
                                case "LastBlockCoordinateList":
                                case "CoordinateList":
                                case "ZcCommArea":
                                case "Area":
                                case "DestBlock":
                                case "Region":
                                    property.SetValue(tExcelModel, tempCoordinateList);
                                    break;
                                case "AtsRouteCfg":
                                    property.SetValue(tExcelModel, tempRouteCfgList);
                                    break;
                                case "SwAlign":
                                    property.SetValue(tExcelModel, tempSwAligList);
                                    break;
                                case "PlatformName":
                                    property.SetValue(tExcelModel, tempStrList);
                                    break;
                                case "TravelTime":
                                    property.SetValue(tExcelModel, tempTravelTimeList);
                                    break;
                                case "WetPlatform":
                                    property.SetValue(tExcelModel, tempWetPlatformList);
                                    break;
                                case "FASPlatformList":
                                    property.SetValue(tExcelModel, tempFASPlatformList);
                                    break;
                                default:
                                    property.SetValue(tExcelModel, tempObject);
                                    break;
                            }
                        }
                    }
                }

                if (tExcelModel.Name != String.Empty)
                {
                    this._deviceCount++;

                    if (!tExcelModel.Type.Equals("Parameter"))
                    {
                        var ids = excelModelBaseList.FindAll(d => d.Id == tExcelModel.Id);
                        if (ids.Count > 0)
                        {
                            ids.ForEach(m =>
                            {
                                if (!tExcelModel.Type.Equals("TravelTime"))
                                {
                                    if (!tExcelModel.Type.Equals("Route")
                                        && !tExcelModel.Type.Equals("HeadCode")
                                        && !tExcelModel.Type.Equals("TrainConfig")
                                        )
                                    {
                                        act(-1, String.Format("设备'{3}'名称为 '{0}' 和 '{1}' 的Id都为 '{2}' ！", m.Name, tExcelModel.Name, m.Id, tExcelModel.Type));
                                    }
                                    else
                                    {
                                        act(-1, String.Format("设备'{0}'存在相同Id都为 '{1}' ！", tExcelModel.Type, m.Id));
                                    }
                                }
                            });
                        }
                    }

                    if (!tExcelModel.Type.Equals("Route") && !tExcelModel.Type.Equals("HeadCode")
                         && !tExcelModel.Type.Equals("TrainConfig")
                         && !tExcelModel.Type.Equals("TravelTime")
                         && nameToDeviceDic.ContainsKey(tExcelModel.Name))
                    {

                        act(-1, string.Format("设备'{1}'名称为'{0}'发现重复。\r\n", tExcelModel.Name, tExcelModel.Type));
                        continue;

                    }
                    if (tExcelModel.Type.Equals("Route") || tExcelModel.Type.Equals("HeadCode")
                        || tExcelModel.Type.Equals("TrainConfig")
                        || tExcelModel.Type.Equals("TravelTime")
                        || tExcelModel.Type.Equals("CBIRoute"))
                    {
                        nameToDeviceDic.Add(string.Format("ER:{1}-{0}", tExcelModel.Type, i), tExcelModel);
                    }
                    else
                    {
                        nameToDeviceDic.Add(tExcelModel.Name, tExcelModel);
                    }
                    this._deviceName += string.Format("<add key=\"{0}\"  value=\"{1}\" />\r\n", tExcelModel.Index.ToString(), tExcelModel.Name);
                    excelModelBaseList.Add(tExcelModel);
                }


                //if (tExcelModel.Name != String.Empty)
                //{
                //    this._deviceCount++;
                //    excelModelBaseList.Add(tExcelModel);
                //    this._deviceName += string.Format("<add key=\"{0}\"  value=\"{1}\" />\r\n", tExcelModel.Index.ToString(), tExcelModel.Name);
                //}
            }


            act(0, String.Format("{0}读取完毕", typeName.Replace("Model", "")));
        }

        /// <summary>
        /// 构建CoordianteList
        /// </summary>
        /// <param name="coordinateList"></param>
        /// <param name="sheet"></param>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <param name="keyColumnIndex"></param>
        private void ProcessCoordinateList(
            List<Coordinate> coordinateList,
            ISheet sheet,
            Int32 rowIndex,
            Int32 colIndex,
            Int32 keyColumnIndex)
        {
            IRow row;
            do
            {
                row = sheet.GetRow(rowIndex);
                if (row.GetCell(colIndex).ToString().PreprocessString() == "-" || row.GetCell(colIndex).ToString().PreprocessString() == "")
                {
                    break;
                }
                foreach (var cell in row.Cells)
                {
                    cell.SetCellType(CellType.String);
                }
                Coordinate coor = new Coordinate();
                coor.TrackId = Convert.ToInt32(row.GetCell(colIndex).ToString().PreprocessString());
                coor.Chainage = Convert.ToDecimal(row.GetCell(colIndex + 1).ToString().PreprocessString());
                coor.Orient = (EOrientationType)Enum.Parse(
                    typeof(EOrientationType),
                    row.GetCell(colIndex + 2).ToString().PreprocessString());
                coordinateList.Add(coor);
                rowIndex++;
                if (rowIndex > sheet.LastRowNum)
                    break;
                row = sheet.GetRow(rowIndex);
                if (row == null)
                    break;
            } while (row.Cells.Any(m => !String.IsNullOrEmpty(m.ToString()))
                && String.IsNullOrEmpty(row.GetCell(keyColumnIndex).ToString().PreprocessString()));
        }
    }
}
