using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;

namespace JSDG.Platform.Util
{
    public enum OutputFormat
    {
        //一列一列输出
        Column = 0,
        //一行一行输出
        Row = 1
    }

    [System.AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class)]
    public class OutputFormatAttribute : Attribute
    {
        public OutputFormat format { get; private set; }
        public OutputFormatAttribute(OutputFormat _format)
        {
            this.format = _format;
        }
    }


    [System.AttributeUsage(AttributeTargets.Property)]
    public class InformationAttribute : Attribute
    {
        public Int16 Index { get; private set; }
        public String RowName { get; private set; }

        public InformationAttribute(Int16 _index, String _rowName)
        {
            this.Index = _index;
            this.RowName = _rowName;
        }
    }

    [OutputFormat(OutputFormat.Column)]
    public class SubSysGenInfo
    {
        [Information(0, "")]
        public String GenSoftware { get; set; }

        [Information(1, "数据表格文件名称")]
        public String DataFileName { get; set; }

        [Information(2, "数据表格文件版本信息")]
        public String DataFileVersion { get; set; }

        [Information(3, "数据生成软件名称")]
        public String GenSoftwareName { get; set; }

        [Information(4, "数据生成软件版本信息")]
        public String GenSoftwareVersion { get; set; }

        [Information(5, "数据生成后二进制文件名称")]
        public String OutputFiles { get; set; }

        [Information(6, "数据生成工作负责人")]
        public String GenPrincipal { get; set; }

        [Information(7, "")]
        public String CompSoftware { get; set; }

        [Information(8, "数据比较工具名称")]
        public String CompSoftwareName { get; set; }

        [Information(9, "数据比较工具版本信息")]
        public String CompSoftwareVersion { get; set; }

        [Information(10, "数据比较结果")]
        public String CompResult { get; set; }

        [Information(11, "数据比较工作负责人")]
        public String CompPrincipal { get; set; }

        [Information(12, "数据发布文件名称")]
        public String PublishFileName { get; set; }

        public SubSysGenInfo()
        {
            this.GenSoftware = "";
            this.DataFileName = "";
            this.DataFileVersion = "";
            this.GenSoftwareName = "";
            this.GenSoftwareVersion = "";
            this.OutputFiles = "";
            this.GenPrincipal = "";
            this.CompSoftware = "";
            this.CompSoftwareName = "";
            this.CompSoftwareVersion = "";
            this.CompResult = "";
            this.CompPrincipal = "";
            this.PublishFileName = "";
        }
    }

    public class GenerateCompareResult
    {
        private SubSysGenInfo GenerateSubSysInfo(
            ESubSystemType subSystemType,
            string[] fileNames,
            List<string> outputFileList,
            string outputFile,
            int compareResult
            )
        {
            SubSysGenInfo result = new SubSysGenInfo();
            for (int i = 0; i < fileNames.Length; i++)
            {
                result.DataFileName += fileNames[i].Split('\\').Last();
                if (i != fileNames.Length - 1)
                    result.DataFileName += Environment.NewLine;
            }
            switch (subSystemType)
            {
                case ESubSystemType.JSDG_ATS:
                    break;
                case ESubSystemType.JSDG_ES:
                    break;
                case ESubSystemType.JSDG_CBI_A:
                case ESubSystemType.JSDG_OBCU_A:
                case ESubSystemType.JSDG_POCT_A:
                    result.GenSoftware = "A链数据";
                    result.CompSoftware = "比较工具A";
                    result.OutputFiles = string.Join(Environment.NewLine, outputFileList);
                    result.PublishFileName = outputFile;
                    break;
                case ESubSystemType.JSDG_ZC_B:
                case ESubSystemType.JSDG_CBI_B:
                case ESubSystemType.JSDG_OBCU_B:
                case ESubSystemType.JSDG_POCT_B:
                    result.GenSoftware = "B链数据";
                    result.CompSoftware = "比较工具B";
                    result.OutputFiles = string.Join(Environment.NewLine, outputFileList);
                    break;
                case ESubSystemType.JSDG_Telegram:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("subSystemType");
            }
            if (compareResult == 1) result.CompResult = "Y";
            else if (compareResult == 0)  result.CompResult = "N";
            return result;
        }

        public void StartGenerateCompareResult(
            ESubSystemType a, 
            ESubSystemType b, 
            string[] fileNamesA,
            string[] fileNamesB,
            List<string> outputFileListA,
            List<string> outputFileListB,
            string outputDirectory,
            bool compareResult,
            string outputPath)
        {

            List<SubSysGenInfo> buffer = new List<SubSysGenInfo>(){
                        GenerateSubSysInfo(a,fileNamesA, outputFileListA, outputDirectory,compareResult?1:0),
                        GenerateSubSysInfo(b,fileNamesB, outputFileListB, outputDirectory,-1),
                        new SubSysGenInfo(){GenSoftware = "备注"}};

            string tableName = "";
            switch (a)
            {
                case ESubSystemType.JSDG_CBI_A:
                case ESubSystemType.JSDG_CBI_B:
                    tableName = "CBI子系统数据生成比较结果表格";
                    break;
                case ESubSystemType.JSDG_OBCU_A:
                case ESubSystemType.JSDG_OBCU_B:
                    tableName = "OBCU子系统数据生成比较结果表格";
                    break;
                case ESubSystemType.JSDG_POCT_A:
                case ESubSystemType.JSDG_POCT_B:
                    tableName = "POCT子系统数据生成比较结果表格";
                    break;
                default:
                    break;
            }
            NpoiExcelClass excelHelper = new NpoiExcelClass(string.Format("{0}\\{1}\\{2}.xlsx",outputPath, outputDirectory,tableName));
            excelHelper.CreateWorkBook();
            int cols = 0;
            excelHelper.CreateSheet(buffer, tableName, ref cols, tableName);
            excelHelper.Save();
        }
    }


    public class NpoiExcelClass
    {
        private IWorkbook _workbook;
        private string _filename;
        private FileStream fs;

        public ICellStyle cellStyle1
        {
            get { return CellStyle1(); }
        }

        public ICellStyle cellStyle2
        {
            get { return CellStyle2(); }
        }

        public NpoiExcelClass(string filename)
        {
            this._filename = filename;
        }

        public void CreateWorkBook()
        {
            if (_workbook == null)
                if (_filename.IndexOf(".xlsx") > 0)//2007
                    _workbook = new XSSFWorkbook();
                else if (_filename.IndexOf(".xls") > 0)//2003
                    _workbook = new HSSFWorkbook();
        }

        public void OpenWorkBook()
        {
            if (_workbook == null)
                using (var fs = new FileStream(_filename, FileMode.Open, FileAccess.Read))
                {
                    if (_filename.IndexOf(".xlsx") > 0)//2007
                        _workbook = new XSSFWorkbook(fs);
                    else if (_filename.IndexOf(".xls") > 0)//2003
                        _workbook = new HSSFWorkbook(fs);
                }
        }

        public void Save()
        {
            try
            {
                fs = new FileStream(_filename, FileMode.Create);
                this._workbook.Write(fs);
                fs.Close();
            }
            catch (Exception e)
            {
                throw new Exception("保存比较结果表格失败", e);
            }
        }

        private ICellStyle CellStyle1()
        {
            ICellStyle style = _workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.WrapText = true;
            return style;
        }

        private ICellStyle CellStyle2()
        {
            ICellStyle style = _workbook.CreateCellStyle();
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            return style;
        }

        public bool CreateSheet<T>(IEnumerable<T> data, string sheetName, ref int colIndex, string header = "")
        {
            ISheet sheet = null;
            try
            {
                if (_workbook != null)
                    sheet = _workbook.CreateSheet(sheetName);
                else
                    return false;

                int startIndex = 0;
                if (header != string.Empty)
                {
                    IRow row = sheet.CreateRow(startIndex);
                    ICell cell = row.CreateCell(0);
                    cell.CellStyle = cellStyle1;
                    cell.SetCellValue(header);
                    for (int col = 1; col <= 3; col++)
                    {
                        ICell cel = row.CreateCell(col);
                        cel.CellStyle = cellStyle1;
                    }

                }
                startIndex++;

                if (data.Count() > 0)
                {
                    OutputFormatAttribute attr = (OutputFormatAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(OutputFormatAttribute));
                    if (attr == null)
                    {
                        throw new Exception("数据结构缺少特性");
                    }
                    // alarm:need to add OutputFormatAttribute to class
                    else if (attr.format == OutputFormat.Row)
                    {
                        //need to implement
                    }
                    else if (attr.format == OutputFormat.Column)
                    {
                        colIndex = 0;
                        bool hasWrittenFiled = false;
                        foreach (var obj in data)
                        {
                            int rowIndex = startIndex;
                            IEnumerable<PropertyInfo> props = obj.GetType().GetProperties().OrderBy(m =>
                                ((InformationAttribute)m.GetCustomAttribute(typeof(InformationAttribute))).Index
                            );


                            //设置字段名称
                            int tmp = rowIndex;
                            if (!hasWrittenFiled)
                            {
                                //每列中最长的一行，用于设置列宽
                                string max = "";
                                foreach (var p in props)
                                {
                                    IRow row = sheet.CreateRow(tmp);
                                    ICell cell = row.CreateCell(colIndex);
                                    string str = ((InformationAttribute)p.GetCustomAttribute(typeof(InformationAttribute))).RowName;
                                    if (max.Length < str.Length)
                                        max = str;
                                    cell.CellStyle = cellStyle2;
                                    cell.SetCellValue(str);
                                    tmp++;
                                }
                                int nameColumnWidth = Encoding.UTF8.GetBytes(max).Length + 1;
                                sheet.SetColumnWidth(colIndex, nameColumnWidth * 180);
                                colIndex++;
                                hasWrittenFiled = true;
                            }

                            //设置字段值
                            //每列中最长的一行，用于设置列宽
                            string valueMax = "";
                            foreach (var p in props)
                            {
                                IRow row = sheet.GetRow(rowIndex);
                                // row.Height = 1000;
                                ICell cell = row.CreateCell(colIndex);
                                cell.CellStyle = cellStyle1;
                                object cellValue = p.GetValue(obj);
                                if (p.PropertyType.Equals(typeof(bool)))
                                {
                                    cell.SetCellValue((bool)cellValue == true ? "Y" : "N");
                                }
                                else
                                {
                                    string[] str = cellValue.ToString().Split('\n');

                                    int h = str.Length;
                                    foreach (var s in str)
                                    {
                                        if (valueMax.Length < s.Length)
                                            valueMax = s;
                                        if (s.Length > 40)
                                        {
                                            h += s.Length % 40 == 0 ? s.Length / 40 - 1 : s.Length / 40;
                                        }
                                    }

                                    //int h_length = Encoding.UTF8.GetBytes(str).Length;
                                    //row.HeightInPoints = 20 * (h_length / 60 + 1);
                                    if (h > 1)
                                    {
                                        row.Height = (short)(h * 300);
                                    }
                                    cell.SetCellValue(cellValue.ToString());
                                }

                                rowIndex++;
                            }

                            //int valueColumnWidth = Encoding.UTF8.GetBytes(valueMax).Length + 1;
                            //排除备注
                            if (valueMax.Length > 40)
                            {
                                sheet.SetColumnWidth(colIndex, 12000);
                            }
                            colIndex++;
                        }
                    }

                }
                MergeCellWithResultTable(sheetName);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("创建比较结果表格失败", e);
            }
        }

        private void MergeCellWithResultTable(string sheetName)
        {
            MergeCell(sheetName, 0, 0, 0, 3);
            MergeCell(sheetName, 2, 3, 1, 1);
            MergeCell(sheetName, 4, 5, 1, 1);
            MergeCell(sheetName, 9, 10, 1, 1);
            MergeCell(sheetName, 2, 3, 2, 2);
            MergeCell(sheetName, 4, 5, 2, 2);
            MergeCell(sheetName, 9, 10, 2, 2);
            MergeCell(sheetName, 13, 13, 1, 2);

        }
        public void MergeCell(string sheetName, int firstRow, int lastRow, int firstCol, int lastCol)
        {
            ISheet sheet = null;
            try
            {
                if (_workbook != null)
                    sheet = _workbook.GetSheet(sheetName);

                if (sheet != null)
                {
                    CellRangeAddress merge = new CellRangeAddress(firstRow, lastRow, firstCol, lastCol);
                    sheet.AddMergedRegion(merge);
                }
            }
            catch (Exception e)
            {
                throw new Exception("合并单元格失败", e);
            }
        }
    }
}
