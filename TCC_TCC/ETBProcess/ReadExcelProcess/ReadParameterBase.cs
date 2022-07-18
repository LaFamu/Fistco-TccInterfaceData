using System;
using System.Collections.Generic;
using System.Linq;

using NPOI.SS.UserModel;

using MSDG.CBI.ETB.MainTrack.B.ETBModel.ExcelModel;
using MSDG.CBI.ETB.MainTrack.B.ETBAttribute.ExcelModelAttribute;


using MSDG.CBI.ETB.MainTrack.B.ETBUtil;


namespace MSDG.CBI.ETB.MainTrack.B.ETBProcess.ReadExcelProcess
{
    /// <summary>
    /// 读取参数数据基类
    /// </summary>
    public abstract class ReadParameterBase
    {
        /// <summary>
        /// 读取Excel数据
        /// </summary>
        /// <typeparam name="TExcelModel">参数类型</typeparam>
        /// <param name="wb">Excel标签页</param>
        /// <param name="excelModelBaseList">Excel实体数据集合</param>
        /// <param name="isRailWay">是否大铁</param>
        /// <param name="act">打印函数（委托）</param>
        public void ReadExcel<TExcelModel>(
            IWorkbook wb,
            List<TExcelModel> excelModelBaseList,
            bool isRailWay, Action<Int32, String> act)
           where TExcelModel : ParameterBase, new()
        {
            var modelType = typeof(TExcelModel);
            ExcelModelClassBaseAttribute memcAtribt = null;
            var classattributeType = typeof(MetroExcelModelClassAttribute);
            var systemParaBase = new TExcelModel();
            modelType.GetCustomAttributes(classattributeType, false)
                .OfType<ExcelModelClassBaseAttribute>()
                .Foreach(att => memcAtribt = att);
            var typeName = memcAtribt.SheetName;
            var sheet = wb.GetSheet(typeName);
            var message = String.Empty;
            if (sheet == null)
            {
                switch (typeName)
                {
                    case "SubSystemID":
                        message = String.Format("子系统ID表SheetName不对或者子系统ID表不存在。\r\n");
                        break;

                    case "TimePara":
                        message += String.Format("时间参数表SheetName不对或者时间参数表不存在\r\n");
                        break;

                    case "CommPara":
                        message += String.Format("通讯参数表SheetName不对或者通讯参数表不存在\r\n");
                        break;
                }

                throw new Exception(message);
            }
            var firstRow = sheet.GetRow(sheet.FirstRowNum);
            switch (typeName)
            {
                case "SubSystemID":
                    if (!firstRow.GetCell(firstRow.FirstCellNum).ToString().Trim().Equals("ID")
                      || !firstRow.GetCell(firstRow.FirstCellNum + 1).ToString().Trim().Equals("Name"))
                    {
                        act(-1, "子系统IDSheet列名不正确。");
                    }
                    break;

                case "TimePara":
                case "CommPara":
                    if (!firstRow.GetCell(firstRow.FirstCellNum).ToString().Equals("Name")
                      || !firstRow.GetCell(firstRow.FirstCellNum + 1).ToString().Equals("Value"))
                    {
                        act(-1, String.Format("参数表Sheet列名不正确。"));
                    }
                    break;
            }

            for (var i = sheet.FirstRowNum + 1; i < sheet.LastRowNum + 1; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null || row.GetCell(0) == null || row.GetCell(0).ToString().PreprocessString() == String.Empty)
                {
                    act(-1, string.Format("{0}表中内容填写缺失，请核查{0}表，并修改！", typeName, i, typeName));
                }
                try
                {
                    SetParameter(systemParaBase, row);
                }
                catch (FormatException fx)
                {
                    act(-1, "解析参数时发生错误，可能是参数值填写错误。");
                }
            }
            excelModelBaseList.Add(systemParaBase);
            act(0, String.Format("{0}读取完毕", typeName.Replace("Model", "")));
        }

        /// <summary>
        /// 填充读取的Excel中的数据
        /// </summary>
        /// <typeparam name="TExcelModel">参数类型</typeparam>
        /// <param name="systemParaBase">参数实体</param>
        /// <param name="row">Excel数据行</param>
        protected abstract void SetParameter<TExcelModel>(TExcelModel systemParaBase, IRow row)
            where TExcelModel : ParameterBase, new();
    }
}