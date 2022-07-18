using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using JSDG.CBI.A.ETBUtil;
using JSDG.CBI.A.ETBModel;
using NPOI.SS.UserModel;

namespace JSDG.CBI.A.ETBProcess
{
    public class DataModel
    {
        public InterLockData InterLockData;
        public string DataVersion { get; set; }
        public string ProjectName { get; set; }
        public string ProjectType { get; set; }
        public List<string> SheetName { get; set; }

        public DataModel(String fileName)
        {
            this.SheetName = new List<string>();
            try
            {
                using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    //var startOffset = fileName.LastIndexOf("\\") + 1;
                    //var endOffset = fileName.LastIndexOf(".");
                    //this.ProjectName = fileName.Substring(startOffset, endOffset - startOffset);
                    this.ProjectName = System.IO.Path.GetFileNameWithoutExtension(fileName);
                    MsgHelper.Instance(1, "正在读取: " + fileName);
                    var wb = WorkbookFactory.Create(fileStream);

                    this.InterLockData = new InterLockData(wb);
                    //ReadProject(wb);
                    for (int i = 0; i < wb.NumberOfSheets; i++)
                    {
                        ISheet sheet = wb.GetSheetAt(i);
                        SheetName.Add(sheet.SheetName);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Inst.LogException("Method:ExcelModel   " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 读取项目名和版本号
        /// </summary>
        /// <param name="workBook">联锁表</param>
        private void ReadProject(IWorkbook workBook)
        {
            string sheetName = "目录";
            var sheet = workBook.GetSheet(sheetName);
            MsgHelper.Instance(1, string.Format("正在读取{0}数据", sheetName));
            if (sheet == null)
            {
                throw new Exception(string.Format("sheet:[{0}]未找到", sheetName));
            }
            try
            {
                this.DataVersion = sheet.GetRow(7).GetCell(2).ToStr().PreprocessString();
                this.ProjectName = sheet.GetRow(1).GetCell(2).ToStr().PreprocessString();
                this.ProjectType = sheet.GetRow(2).GetCell(2).ToStr().PreprocessString();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}


