using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using TCC_TCC.A.ETBUtil;
using TCC_TCC.A.ETBModel;
using NPOI.SS.UserModel;
using TCC_TCC.A.ETBAttribute;
using System.Reflection;
using TCC_TCC.A.ETBModel.ExcelModel.InterLock;
using TCC_TCC.A.ETBConfig;
using NPOI.SS.Util;

namespace TCC_TCC.A.ETBProcess
{
    public class DataModel
    {
        //数据表类
        public BitData BitData;

        //数据版本
        public string DataVersion { get; set; }

        //码位表名称
        public string BitSheetName { get; set; }

        //项目类型
        public string ProjectType { get; set; }

        //表名
        //public List<string> SheetName { get; set; }

        //构造函数
        public DataModel(List<String> fileNames)
        {
            try
            {
                this.BitData = new BitData();

                foreach (var fileName in fileNames)
                {
                    using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    {
                        this.BitSheetName = System.IO.Path.GetFileNameWithoutExtension(fileName);
                        MsgHelper.Instance(1, "正在读取: " + fileName);
                        var wb = WorkbookFactory.Create(fileStream);
                        if (BitSheetName.Contains("Tcc2Tcc"))
                        {       
                            this.BitData.TCC2TCCDic.Add(BitSheetName, ReadInterLockSheet<CommModel>(wb));
                        }
                        else if (BitSheetName.Contains("Tcc2Cbi"))
                        {
                            this.BitData.TCC2CBIList = ReadInterLockSheet<CommModel>(wb);
                        }
                        else if (BitSheetName.Contains("Tcc2Zpw"))
                        {
                            this.BitData.Tcc2ZpwInterface.interfaceConfigList = ReadInterLockSheet<InterfaceConfigModel>(wb);
                            this.BitData.Tcc2ZpwInterface.interfaceBitList = ReadInterLockSheet<InterfaceBitModel>(wb);
                            this.BitData.Tcc2ZpwInterface.dataPacketList = ReadInterLockSheet<DataPacketModel>(wb);
                            this.BitData.Tcc2ZpwInterface.dataBlockList = ReadMergeBitSheet<TccZpwDataBlockModel>(wb);
                            this.BitData.Tcc2ZpwInterface.cabinetList = ReadMergeBitSheet<CabinetModel>(wb);
                        }
                        else if (BitSheetName.Contains("Tcc2Ctc"))
                        {
                            this.BitData.Tcc2CtcInterface.interfaceConfigList = ReadInterLockSheet<InterfaceConfigModel>(wb);
                            this.BitData.Tcc2CtcInterface.interfaceBitList = ReadMergeBitSheet<InterfaceBitModel>(wb);
                            this.BitData.Tcc2CtcInterface.dataPacketList = ReadInterLockSheet<DataPacketModel>(wb);
                            this.BitData.Tcc2CtcInterface.dataBlockList = ReadMergeBitSheet<DataBlockModel>(wb);
                        }
                        else if (BitSheetName.Contains("Tcc2Dm"))
                        {
                            this.BitData.Tcc2DmInterface.interfaceConfigList = ReadInterLockSheet<InterfaceConfigModel>(wb);
                            this.BitData.Tcc2DmInterface.interfaceBitList = ReadInterLockSheet<InterfaceBitModel>(wb);
                            this.BitData.Tcc2DmInterface.dataPacketList = ReadInterLockSheet<DataPacketModel>(wb);
                            this.BitData.Tcc2DmInterface.dataBlockList = ReadMergeBitSheet<DataBlockModel>(wb);
                        }
                        else if (BitSheetName.Contains("Tcc2Tsrs"))
                        {
                            this.BitData.Tcc2TsrsInterface.interfaceConfigList = ReadInterLockSheet<InterfaceConfigModel>(wb);
                            this.BitData.Tcc2TsrsInterface.interfaceBitList = ReadMergeBitSheet<InterfaceBitModel>(wb);
                            this.BitData.Tcc2TsrsInterface.dataPacketList = ReadInterLockSheet<DataPacketModel>(wb);
                            this.BitData.Tcc2TsrsInterface.dataBlockList = ReadMergeBitSheet<DataBlockModel>(wb);
                        }
                       
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Inst.LogException("Method:ExcelModel   " + ex.Message);
                throw;
            }
        }

        private List<T> ReadInterLockSheet<T>(IWorkbook workBook) where T : new()
        {
            try
            {
                ExcelModelClassBaseAttribute att = GetExcelModelClassBaseAttribute<T>();
                ISheet sheet = workBook.GetSheet(att.SheetName);
                MsgHelper.Instance(1, string.Format("正在读取{0}数据", att.SheetName));
                if (sheet == null)
                {
                    throw new Exception(string.Format("sheet:[{0}]未找到", att.SheetName));
                }

                Dictionary<PropertyInfo, MetroExcelModelPropertyAttribute> propertydic = GetModelPropertyAttributes<T>();
                var listT = new List<T>();


                for (var i = att.HeaderIndex; i < sheet.LastRowNum + 1; i++)
                {
                    IRow row = (IRow)sheet.GetRow(i);

                    if (CheckRowIsNull(row, propertydic.Count()))
                    {
                        break;
                    }

                    var t = new T();
                    foreach (var property in propertydic.Keys)
                    {
                        var attribute = propertydic[property];
                        try
                        {
                            if (attribute != null)
                            {
                                var value = GetPropertyValue(property.PropertyType.Name, row.GetCell(attribute.CellNum));
                                property.SetValue(t, value);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format("列[{0}]读取数据错误:{1}", attribute.ColName, ex.Message));
                        }
                    }

                    listT.Add(t);
                }

                //if (this.MinNumDic.ContainsKey(att.CbiDataType.ToString()))//验证数据最小值
                //{
                //    if (listT.Count() < this.MinNumDic[att.CbiDataType.ToString()])
                //    {
                //        throw new Exception(string.Format("[{0}]缺少数据", sheet.SheetName));
                //    }
                //}

                return listT;
            }
            catch (Exception ex)
            {
                LogHelper.Inst.LogException("Method:ReadInterLockSheet   " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="workBook"></param>
        /// <returns></returns>
        private List<T> ReadMergeBitSheet<T>(IWorkbook workBook) where T : new()
        {
            try
            {
                var listT = new List<T>();
                ExcelModelClassBaseAttribute att = GetExcelModelClassBaseAttribute<T>();
                
                ISheet sheet = workBook.GetSheet(att.SheetName);
                MsgHelper.Instance(1,string.Format("正在读取{0}数据",att.SheetName));
                if (sheet == null)
                {
                    throw new Exception(string.Format("sheet:[{0}]未找到",att.SheetName));
                }
                Dictionary<PropertyInfo, MetroExcelModelPropertyAttribute> propertydic = GetModelPropertyAttributes<T>();

                for (var i = att.HeaderIndex; i < sheet.LastRowNum + 1;i++)
                {
                    IRow row = (IRow)sheet.GetRow(i);
                    if (CheckRowIsNull(row, propertydic.Count()))
                    {
                        break;
                    }

                    var t = new T();
                    foreach (var property in propertydic.Keys)
                    {
                        var attribute = propertydic[property];
                        try
                        {
                            ICell mergecell = null;
                            if (attribute != null)
                            {
                                //var value;
                                //if (row.GetCell(attribute.CellNum).IsMergedCell)
                                if(GetMergeCellValue(sheet,row.GetCell(attribute.CellNum),ref mergecell))
                                {
                                   var value = GetPropertyValue(property.PropertyType.Name,mergecell);
                                    property.SetValue(t, value);
                                }
                                else
                                {
                                   var value = GetPropertyValue(property.PropertyType.Name, row.GetCell(attribute.CellNum));
                                   property.SetValue(t, value);
                                }
                                
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format("列[{0}]读取数据错误:{1}", attribute.ColName, ex.Message));
                        }
                    }
                    listT.Add(t);
                }

                return listT;
            }
            catch (Exception ex)
            {
                LogHelper.Inst.LogException("Method:ReadBitSheet   " + ex.Message);
                throw;
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //private String GetMergeCellValue(ICell cell,ISheet sheet)
        //{
        //    var result = string.Empty;

        //    if (string.IsNullOrEmpty(cell.ToString()))
        //    {
        //        int firstRegionRow = 0;
        //        if (IsMergedRegionCell(cell.ColumnIndex, cell.RowIndex, sheet, ref firstRegionRow))
        //        {
        //            if (cell.RowIndex != firstRegionRow)
        //            { 
                        
        //            }
        //        }
        //    }

        //    return result;
        //}



        /// <summary>
        /// 根据属性描述  更改从cell中获取的值
        /// </summary>
        /// <returns></returns>
        protected virtual object GetPropertyValue(string type, ICell cellValue)
        {
            object objectValue = null;
            if ("String" == type)
            {
                objectValue = cellValue != null ? cellValue.ToString().PreprocessString() : "";
            }
            else
            {
                try
                {
                    switch (type)
                    {
                        case "UInt16":
                            objectValue = UInt16.Parse(cellValue.ToString().PreprocessString());
                            break;
                        case "Int16":
                            objectValue = Int16.Parse(cellValue.ToString().PreprocessString());
                            break;
                        case "UInt32":
                            objectValue = UInt32.Parse(cellValue.ToString().PreprocessString());
                            break;
                        case "Int32":
                            objectValue = Int32.Parse(cellValue.ToString().PreprocessString());
                            break;
                    }
                }
                catch (Exception)
                {
                    return (UInt16)0;
                }
            }
            return objectValue;
        }


        public  bool GetMergeCellValue(ISheet sheet,ICell cell,ref ICell resultcell)
        {
            

            var regionLists = GetMergedCellRegion(sheet);

            foreach (var cellRangeAddress in regionLists)
            {
                for (int i = cellRangeAddress.FirstRow; i <= cellRangeAddress.LastRow; i++)
                {
                    if (cell.RowIndex == i)
                    {
                        for (int j = cellRangeAddress.FirstColumn; j <= cellRangeAddress.LastColumn; j++)
                        {
                            if (cell.ColumnIndex == j)
                            {
                                var row = sheet.GetRow(cellRangeAddress.FirstRow);
                                resultcell = row.GetCell(cellRangeAddress.FirstColumn);
                                return true;
                            }
                        }
                    }
                }
            }

            resultcell = null;
            return false;
        }

        public bool IsMergedRegionCell(int cellIndex, int rowIndex, ISheet sheet, ref int firstRegionRow)
          {
              bool isMerged = false;
              var regionLists = GetMergedCellRegion(sheet);
  
              foreach (var cellRangeAddress in regionLists)
              {
                  for (int i = cellRangeAddress.FirstRow; i <= cellRangeAddress.LastRow; i++)
                 {
                     if (rowIndex == i)
                     {
                         for (int j = cellRangeAddress.FirstColumn; j <= cellRangeAddress.LastColumn; j++)
                         {
                             if (cellIndex == j)
                             {
                                 isMerged = true;
                                 firstRegionRow = cellRangeAddress.FirstRow;
                                 break;
                             }
                             else
                             {
                                 continue;
                             }
                         }
                     }
                     else
                    {
                        continue;
                     }
                 }
             }

             return isMerged;
         }

        private List<CellRangeAddress> GetMergedCellRegion(ISheet sheet)
         {
             int mergedRegionCellCount = sheet.NumMergedRegions;
             var returnList = new List<CellRangeAddress>();
 
             for (int i = 0; i<mergedRegionCellCount; i++)
             {
                 returnList.Add(sheet.GetMergedRegion(i));
             }
 
             return returnList;
         }

        private Dictionary<PropertyInfo, MetroExcelModelPropertyAttribute> GetModelPropertyAttributes<T>() where T : new()
        {
            var config = ETB_APPConfig.AppConfig.GetSection(String.Format("{0}/{1}", "columnsetting", typeof(T).Name)) as ConfigSection;
            var dic = new Dictionary<PropertyInfo, MetroExcelModelPropertyAttribute>();
            if (config != null)
            {
                try
                {
                    foreach (var property in typeof(T).GetProperties())
                    {
                        var attribute = property.GetCustomAttribute<MetroExcelModelPropertyAttribute>(true);

                        if (config.KeyValues[property.Name] == null)
                        {
                            continue;
                        }
                        attribute.CellNum = Int32.Parse(config.KeyValues[property.Name].Value);//列号的配置在sheet名下,要求属性名和配置同名(否则会报错)
                        dic.Add(property, attribute);
                    }
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.ToString());
                }
            }
            else
            {
                throw new Exception("未读取到联锁表数据的自定义特性");
            }

            return dic;
        }

        private ExcelModelClassBaseAttribute GetExcelModelClassBaseAttribute<T>()
        {
            return typeof(T).GetCustomAttribute<ExcelModelClassBaseAttribute>(false);
        }

        /// <summary>
        /// 检查该行是否位空
        /// </summary>
        /// <param name="row"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        protected bool CheckRowIsNull(IRow row, int length)
        {
            if (row == null)
            {
                return true;
            }

            for (int j = 0; j < length; j++)
            {
                if (null != row.GetCell(j))
                {
                    int aaa = row.GetCell(j).ColumnIndex;
                }

                if (null != row.GetCell(j) && row.GetCell(j).CellType != CellType.Blank && row.GetCell(j).ToStr() != "N/A")
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 读取项目名和版本号
        /// </summary>
        /// <param name="workBook">联锁表</param>
        //private void ReadProject(IWorkbook workBook)
        //{
        //    string sheetName = "目录";
        //    var sheet = workBook.GetSheet(sheetName);
        //    MsgHelper.Instance(1, string.Format("正在读取{0}数据", sheetName));
        //    if (sheet == null)
        //    {
        //        throw new Exception(string.Format("sheet:[{0}]未找到", sheetName));
        //    }
        //    try
        //    {
        //        this.DataVersion = sheet.GetRow(7).GetCell(2).ToStr().PreprocessString();
        //        this.ProjectName = sheet.GetRow(1).GetCell(2).ToStr().PreprocessString();
        //        this.ProjectType = sheet.GetRow(2).GetCell(2).ToStr().PreprocessString();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
    }
}


