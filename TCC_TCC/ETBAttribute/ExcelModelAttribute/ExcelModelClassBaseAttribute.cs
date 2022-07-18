using System;
using System.Collections.Generic;
using TCC_TCC.A.ETBConfig;

namespace TCC_TCC.A.ETBAttribute
{
    public class ExcelModelClassBaseAttribute : Attribute
    {

        /// <summary>
        /// 与实体类对应的Excel表格中的Sheet的名称
        /// </summary>
        public String SheetName { get; private set; }

        /// <summary>
        /// 对应的Data中数据中的设备的类型
        /// </summary>
        public SHEET_TYPE CbiDataType { get; private set; }
        /// <summary>
        /// 表头行数
        /// </summary>
        public Int32 HeaderIndex { get; private set; }

        /// <summary>
        /// 单控区设备数量最大值字典
        /// </summary>
        public Dictionary<string, int> MaxNumDic { get; set; }

        /// <summary>
        /// 单控区设备数量最小值字典
        /// </summary>
        public Dictionary<string, int> MinNumDic { get; set; }

        /// <summary>
        /// Excel实体与Excel对应关系描述特性
        /// </summary>
        /// <param name="cbidataType"></param>
        public ExcelModelClassBaseAttribute(SHEET_TYPE cbidataType)
        {
            this.CbiDataType = cbidataType;
            this.GetSheetName();
        }
        protected void GetSheetName()
        {
            try
            {
                this.SheetName = ETB_APPConfig.AppConfig.AppSettings.Settings[this.CbiDataType.ToString()].Value;
                HeaderIndex = 0;

                //获取头行
                var configSheetHeader = ETB_APPConfig.AppConfig.GetSection(string.Format("{0}/{1}", "columnsetting", "SheetHeader")) as ConfigSection;
                if (configSheetHeader != null && configSheetHeader.KeyValues[this.CbiDataType.ToString()] != null)
                {
                    HeaderIndex = Int32.Parse(configSheetHeader.KeyValues[this.CbiDataType.ToString()].Value);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("TCC_TCC.A.ETBAttribute.ExcelModelClassBaseAttribute.GetSheetName", ex);
            }
        }

        public ExcelModelClassBaseAttribute()
        {
            this.GetSheetContentNum();
        }

        protected void GetSheetContentNum()
        {
            try
            {
                this.MaxNumDic = new Dictionary<string, int>();
                this.MinNumDic = new Dictionary<string, int>();
                int maxNum = 0;
                int minNum = 1;
                var configMinNum = ETB_APPConfig.AppConfig.GetSection(string.Format("{0}/{1}", "columnsetting", "MinNum")) as ConfigSection;
                var configMaxNum = ETB_APPConfig.AppConfig.GetSection(string.Format("{0}/{1}", "columnsetting", "MaxNum")) as ConfigSection;

                //获取单控区设备数量限值
                foreach (var cbiDataType in Enum.GetNames(typeof(SHEET_TYPE)))
                {
                    maxNum = 0;
                    if (configMaxNum != null && configMaxNum.KeyValues[cbiDataType] != null)
                    {
                        maxNum = int.Parse(configMaxNum.KeyValues[cbiDataType].Value);
                    }
                    this.MaxNumDic.Add(cbiDataType, maxNum);

                    minNum = 1;
                    if (configMinNum != null && configMinNum.KeyValues[cbiDataType] != null)
                    {
                        minNum = int.Parse(configMinNum.KeyValues[cbiDataType].Value);
                    }
                    this.MinNumDic.Add(cbiDataType, minNum);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("TCC_TCC.A.ETBAttribute.ExcelModelClassBaseAttribute.GetSheetName", ex);
            }
        }
    }
}
