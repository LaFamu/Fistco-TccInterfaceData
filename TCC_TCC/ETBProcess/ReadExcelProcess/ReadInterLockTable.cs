using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using NPOI.SS.UserModel;

using MSDG.CBI.ETB.MainTrack.B.ETBModel.ExcelModel;
using MSDG.CBI.ETB.MainTrack.B.ETBAttribute.ExcelModelAttribute;
using MSDG.CBI.ETB.MainTrack.B.ETBConfig.DeviceAttributeEnum;
using MSDG.CBI.ETB.MainTrack.B.ETBUtil;


namespace MSDG.CBI.ETB.MainTrack.B.ETBProcess.ReadExcelProcess
{
    /// <summary>
    /// 读取联锁表数据
    /// </summary>
    public class ReadInterLockTable
    {
        private String _deviceName;
        private Int32 _deviceCount;
        private Regex _nameRegex;
        private Regex _sioNameRegex;
        private Regex _flankSwitchRegex;
        private Regex _driveSwitchRegex;



        /// <summary>
        /// 通过特性从Excel的sheet中读取数据到设备Model
        /// </summary>
        /// <typeparam name="TExcelModel">Excel实体类型</typeparam>
        /// <param name="wb">Excel标签页</param>
        /// <param name="excelModelBaseList">Excel实体数据集合</param>
        /// <param name="isRailWay">是否大铁</param>
        /// <param name="zoneName">区域名称</param>
        /// <param name="act">打印函数（委托）</param>
        /// <param name="nameToDeviceDic">名称设备映射字典</param>
        /// <param name="signalDisplayTypeDic">信号机显示类型映射字典</param>
        public void ReadExcel<TExcelModel>(
            IWorkbook wb,
            List<TExcelModel> excelModelBaseList,
            Dictionary<String, DeviceModelBase> nameToDeviceDic,
            Dictionary<String, UInt32> signalDisplayTypeDic, Dictionary<String, UInt32> switchDisplayTypeDic, Action<Int32, String> act)
            where TExcelModel : DeviceModelBase, new()
        {
            this._nameRegex = this._nameRegex ?? new Regex(ExcelToDatProcess.ConfigData.NameRegexStr);
            this._sioNameRegex = this._sioNameRegex ?? new Regex(ExcelToDatProcess.ConfigData.SpecialIONameRegexStr);
            this._deviceCount = 0;
            this._deviceName = string.Empty;
            this._flankSwitchRegex = new Regex(@"(?<=\[).*[^\]]");
            this._driveSwitchRegex = new Regex(@"(?<={).*[^}]");
            var repeatedId = string.Empty;
            TExcelModel tExcelModel;
            var modelType = typeof(TExcelModel);
            ExcelModelClassBaseAttribute memcAtribt = null;
            ExcelModelPropertyBaseAttribute remcAtribt;
            String typeName;
            ISheet sheet;
            var classattributeType = typeof(MetroExcelModelClassAttribute);
            var propertyattributeType = typeof(MetroExcelModelPropertyAttribute);
            var deviceSheetKeyColumnIndex = ExcelToDatProcess.SheetKeyColumn.SheetKeyColumnDic[modelType.Name];
            foreach (var att in modelType.GetCustomAttributes(classattributeType, false))
            {
                if (att is ExcelModelClassBaseAttribute)
                {
                    memcAtribt = (ExcelModelClassBaseAttribute)att;
                }
            }
            typeName = memcAtribt.SheetName;
            sheet = wb.GetSheet(typeName);
            var railwayEixt = false;
            if (sheet == null)
            {
                act(-1, String.Format("Excel标签页名称不正确,没有找到名为{0}的标签页。", typeName));
            }
            for (var i = sheet.FirstRowNum + 1; i < sheet.LastRowNum + 1; i++)
            {
                Object tempObject;
                IRow row;
                row = sheet.GetRow(i);
                if (row == null)
                {
                    break;
                }
                if ((row.GetCell(deviceSheetKeyColumnIndex) == null) || (row.GetCell(deviceSheetKeyColumnIndex).ToString().PreprocessString() == ""))
                {
                    if ((row.GetCell(deviceSheetKeyColumnIndex + 1) != null) && (row.GetCell(deviceSheetKeyColumnIndex + 1).ToString().PreprocessString() != ""))
                    {
                        act(-1, String.Format("联锁表中的'{0}'页第{1}行有误检查，可能是由于未填写设备名称", typeName, i.ToString()));

                    }
                    break;
                }
                tExcelModel = new TExcelModel();
                foreach (var property in modelType.GetProperties())
                {
                    tempObject = null;
                    foreach (var attribute in property.GetCustomAttributes(propertyattributeType, true))
                    {
                        if (attribute is ExcelModelPropertyBaseAttribute)
                        {
                            remcAtribt = attribute as ExcelModelPropertyBaseAttribute;
                            switch (remcAtribt.PropertyType)
                            {
                                case ConstantInternal.ExcelModelPropertyType.StringType:
                                    tempObject = row.GetCell(row.FirstCellNum + remcAtribt.Index) == null ?
                                        string.Empty : (row.GetCell(row.FirstCellNum + remcAtribt.Index)).ToString().PreprocessString();

                                    if (property.Name == "Name" && this._nameRegex.IsMatch((String)tempObject) && !modelType.Name.Equals("SpecialConditionModel"))
                                    {
                                        act(-1, String.Format("设备'{1}'的名称格式不正确：'{0}'", tempObject, tExcelModel.Type));
                                        break;
                                    }
                                    else if (modelType.Name == "SpecialIOModel" && property.Name == "Name" && this._sioNameRegex.IsMatch((String)tempObject))
                                    {
                                        act(-1, String.Format("特殊设备名称格式不正确：'{0}'", tempObject));
                                    }
                                    break;

                                case ConstantInternal.ExcelModelPropertyType.SignalDisplayString:
                                    tempObject = row.GetCell(row.FirstCellNum + remcAtribt.Index) == null ? null : (row.GetCell(row.FirstCellNum + remcAtribt.Index)).ToString().PreprocessString();
                                    if (!tempObject.Equals("B"))
                                    {
                                        act(-1, "调车进路表中信号机显示内容不正确！");
                                    }
                                    break;

                                case ConstantInternal.ExcelModelPropertyType.Value16Type:
                                    try
                                    {
                                        tempObject = Convert.ToUInt16((row.GetCell(row.FirstCellNum + remcAtribt.Index)).ToString().PreprocessString());
                                    }
                                    catch (Exception)
                                    {
                                        act(-1, String.Format("设备'{0}'的第{1}行的{2}格式不正确", tExcelModel.Type, i.ToString(), property.Name));
                                    }
                                    break;

                                case ConstantInternal.ExcelModelPropertyType.Value32Type:
                                    try
                                    {
                                        if ((((row.GetCell(row.FirstCellNum + remcAtribt.Index)) == null ||
                                       (row.GetCell(row.FirstCellNum + remcAtribt.Index)).ToString().PreprocessString() == "")))
                                        {
                                            tempObject = UInt32.MinValue;
                                        }
                                        else
                                        {
                                            tempObject = Convert.ToUInt32((row.GetCell(row.FirstCellNum + remcAtribt.Index)).ToString().PreprocessString());
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        act(-1, String.Format("设备'{0}'的第{1}行的{2}格式不正确", tExcelModel.Type, i.ToString(), property.Name));
                                    }

                                    break;

                                case ConstantInternal.ExcelModelPropertyType.IndexType:
                                    tempObject = (UInt32)(i - 1);
                                    break;

                                case ConstantInternal.ExcelModelPropertyType.SignalType:
                                    try
                                    {
                                        tempObject = signalDisplayTypeDic[(row.GetCell(row.FirstCellNum + remcAtribt.Index)).ToString().PreprocessString()];
                                    }
                                    catch (Exception)
                                    {
                                        act(-1, String.Format("设备'{0}'的第{1}行的{2}格式不正确", tExcelModel.Type, i.ToString(), property.Name));
                                    }
                                    break;

                                case ConstantInternal.ExcelModelPropertyType.SwitchType:
                                    try
                                    {
                                        tempObject = switchDisplayTypeDic[(row.GetCell(row.FirstCellNum + remcAtribt.Index)).ToString().PreprocessString()];
                                    }
                                    catch (Exception)
                                    {
                                        act(-1, String.Format("设备'{0}'的第{1}行的{2}格式不正确", tExcelModel.Type, i.ToString(), property.Name));
                                    }
                                    break;


                                case ConstantInternal.ExcelModelPropertyType.InOrOutType:
                                    if (row.GetCell(row.FirstCellNum + remcAtribt.Index).ToString().PreprocessString().Equals("Y") || row.GetCell(row.FirstCellNum + remcAtribt.Index).ToString().PreprocessString() == "N")
                                    {
                                        tempObject = (row.GetCell(row.FirstCellNum + remcAtribt.Index)).ToString().PreprocessString() == "Y" ? ExcelToDatProcess.SpecialConditionDataType.DT_INPUT_POINT : ExcelToDatProcess.SpecialConditionDataType.DT_OUTPUT_POINT;
                                    }
                                    else
                                    {
                                        act(-1, String.Format("设备'{0}'的第{1}行的{2}格式不正确", tExcelModel.Type, i.ToString(), property.Name));
                                    }
                                    break;

                                case ConstantInternal.ExcelModelPropertyType.InternalDeviceType:
                                case ConstantInternal.ExcelModelPropertyType.IsRailBlockType:
                                    if (row.GetCell(row.FirstCellNum + remcAtribt.Index).ToString().PreprocessString().Equals("Y") || row.GetCell(row.FirstCellNum + remcAtribt.Index).ToString().PreprocessString() == "N")
                                    {
                                        tempObject = (row.GetCell(row.FirstCellNum + remcAtribt.Index)).ToString().PreprocessString() == "Y" ? UInt32.MaxValue : UInt32.MinValue;
                                    }
                                    else
                                    {
                                        act(-1, String.Format("设备'{0}'的第{1}行的{2}格式不正确", tExcelModel.Type, i.ToString(), property.Name));
                                    }
                                    break;

                                case ConstantInternal.ExcelModelPropertyType.RouteType:
                                    tempObject = !railwayEixt ? "ENTRY" : "EXIT";
                                    break;

                                case ConstantInternal.ExcelModelPropertyType.TimeType:
                                    try
                                    {
                                        if ((((row.GetCell(row.FirstCellNum + remcAtribt.Index)) == null ||
                                        (row.GetCell(row.FirstCellNum + remcAtribt.Index)).ToString().PreprocessString() == "")))
                                        {
                                            throw new Exception();
                                        }
                                        else
                                        {
                                            var time = row.GetCell(row.FirstCellNum + remcAtribt.Index).ToString().PreprocessString().ToLower();
                                            if (time.EndsWith("min"))
                                            {
                                                tempObject = Convert.ToUInt32(time.Replace("min", "")) * 600u;
                                            }
                                            else if (time.EndsWith("s"))
                                            {
                                                tempObject = Convert.ToUInt32(time.Replace("s", "")) * 10u;
                                            }
                                            else
                                            {
                                                tempObject = Convert.ToUInt32(time) * 10u;
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        act(-1, String.Format("设备'{0}'的第{1}行的{2}格式不正确", tExcelModel.Type, i.ToString(), property.Name));
                                    }
                                    break;
                            }
                            if (remcAtribt.IsCheckRepeat)
                            {
                                CheckRepeatContent(tempObject.ToString(), typeName, property.ToString(), i.ToString(), act);
                            }
                            property.SetValue(tExcelModel, tempObject);
                        }
                    }
                }
                if (tExcelModel.Name != String.Empty)
                {
                    this._deviceCount++;
                    var ids = excelModelBaseList.FindAll(d => d.Id == tExcelModel.Id);
                    if (ids.Count > 0)
                    {
                        ids.ForEach(d =>
                        {
                            if (d.Type.Contains("Route"))
                            {
                                repeatedId += string.Format("id为{0}.\r\n", d.Id);
                            }
                            else
                            {
                                repeatedId += string.Format("名称为{0}和{1}的id都为{2}.\r\n", d.Name, tExcelModel.Name, d.Id);
                            }
                        });
                    }

                    excelModelBaseList.Add(tExcelModel);
                    if (nameToDeviceDic.ContainsKey(tExcelModel.Name))
                    {
                        act(-1, string.Format("设备'{1}'名称为'{0}'发现重复", tExcelModel.Name, tExcelModel.Type));
                        continue;
                    }
                    nameToDeviceDic.Add(tExcelModel.Name, tExcelModel);
                }
            }
            if (repeatedId.Length > 0)
            {
                act(-1, string.Format("'{1}'设备id发现重复：{0}", repeatedId, typeName));
            }
            act(0, String.Format("{0}读取完毕", typeName.Replace("Model", "")));
        }


        /// <summary>
        /// 检查重复数据
        /// </summary>
        /// <param name="Content">要检查的数据</param>
        /// <param name="act">打印函数（委托）</param>
        private void CheckRepeatContent(String content, String typeName, String propertyName, String row, Action<Int32, String> act)
        {
            if (content.Contains(","))
            {
                var stringName = content.Split(new char[] { ',' });
                foreach (var s in stringName)
                {
                    try
                    {
                        string temp = stringName.SingleOrDefault(a => a == s);
                    }
                    catch (InvalidOperationException e)
                    {
                        act(-1, string.Format("{0}表中{1}列第{2}行内容{3}填写重复!", typeName, propertyName, row, s));
                    }
                }
            }
        }

        /// <summary>
        /// 检查设备名称是否正确
        /// </summary>
        /// <param name="content"></param>
        /// <param name="typeName"></param>
        /// <param name="propertyName"></param>
        /// <param name="row"></param>
        //private void CheckDeviceName(String content, String typeName, String propertyName, String row,Dictionary<String, DeviceModelBase> nameToDeviceDic)
        //{
        //    if (content.Contains(","))
        //    {
        //        var deviceName = content.Split(new char[] { ',' });
        //        foreach (var name in deviceName)
        //        {
        //            if (name.Contains(">"))
        //            {
        //                var stringName = name.Split(new char[] { '>' });
        //                if (!nameToDeviceDic.ContainsKey(stringName[1]))
        //                {
        //                    act(-1,string.Format("{0}表中{1}列第{2}行内容{3}不存在!\r\n", typeName, propertyName, row, stringName[1]));
        //                    LogHelper.Inst.Log(string.Format("{0}表中{1}列第{2}行内容{3}不存在!\r\n", typeName, propertyName, row, stringName[1]));
        //                }
        //            }
        //            if (!nameToDeviceDic.ContainsKey(name))
        //            {
        //                act(-1,string.Format("{0}表中{1}列第{2}行内容{3}不存在!\r\n", typeName, propertyName, row, name));
        //                LogHelper.Inst.Log(string.Format("{0}表中{1}列第{2}行内容{3}不存在!\r\n", typeName, propertyName, row, name));
        //            }
        //        }

        //    }

        //}

        /// <summary>
        /// 读取Excel文档的目录
        /// </summary>
        /// <param name="wb">目录标签页</param>
        /// <param name="directoryModel">目录实体</param>
        /// <param name="act">打印函数（委托）</param>
        public void ReadExcel(IWorkbook wb, DirectoryModel directoryModel, Action<Int32, String> act)
        {
            var modelType = typeof(DirectoryModel);
            MetroExcelModelClassAttribute memcAtribt = null;
            foreach (var att in modelType.GetCustomAttributes(false).OfType<MetroExcelModelClassAttribute>())
            {
                memcAtribt = att;
            }
            if (memcAtribt != null)
            {
                var typeName = memcAtribt.SheetName;
                var sheet = wb.GetSheet(typeName);
                directoryModel.ProjectName = sheet.GetRow(2).GetCell(3).ToString().PreprocessString();
                directoryModel.ProjectType = sheet.GetRow(3).GetCell(3).ToString().PreprocessString();
                directoryModel.ZoneName = sheet.GetRow(5).GetCell(3).ToString().PreprocessString();
                directoryModel.Version = sheet.GetRow(7).GetCell(3).ToString().PreprocessString();
                act(0, String.Format("{0}读取完毕", typeName.Replace("Model", "")));
            }
        }
    }
}