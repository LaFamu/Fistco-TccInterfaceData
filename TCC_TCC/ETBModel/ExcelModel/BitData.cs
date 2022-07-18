using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TCC_TCC.A.ETBAttribute;
using TCC_TCC.A.ETBConfig;
using TCC_TCC.A.ETBModel;
using TCC_TCC.A.ETBUtil;
using NPOI.SS.UserModel;
using TCC_TCC.A.ETBModel.ExcelModel.InterLock;

namespace TCC_TCC.A.ETBModel
{
    public class BitData
    {
        public BitData()
        {
            this.MaxNumDic = new Dictionary<string, int>();
            this.MinNumDic = new Dictionary<string, int>();
            ExcelModelClassBaseAttribute att = new ExcelModelClassBaseAttribute();
            att.MaxNumDic.Foreach(x => this.MaxNumDic.Add(x.Key, x.Value));
            att.MinNumDic.Foreach(x => this.MinNumDic.Add(x.Key, x.Value));

            this.Tcc2ZpwInterface = new TccZpwModel();
            this.Tcc2DmInterface = new TccCommModel();
            this.Tcc2CtcInterface = new TccCommModel();
            this.Tcc2TsrsInterface = new TccCommModel();
            this.TCC2TCCDic = new Dictionary<string, List<CommModel>>();
        }

        #region read excel method

        /// <summary>
        /// 读取码位表Sheet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="workBook"></param>
        /// <returns></returns>
        private List<T> ReadBitSheet<T>(IWorkbook workBook) where T : new()
        {
            try
            {
                var listT = new List<T>();
                ExcelModelClassBaseAttribute att = GetExcelModelClassBaseAttribute<T>();
                ISheet sheet = workBook.GetSheet(att.SheetName);


                return listT;
            }
            catch (Exception ex)
            {
                LogHelper.Inst.LogException("Method:ReadBitSheet   " + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 读取联锁表Sheet
        /// </summary>
        /// <param name="workBook">联锁表</param>
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

        private ExcelModelClassBaseAttribute GetExcelModelClassBaseAttribute<T>()
        {
            return typeof(T).GetCustomAttribute<ExcelModelClassBaseAttribute>(false);
        }

        private Dictionary<PropertyInfo, MetroExcelModelPropertyAttribute> GetModelPropertyAttributes<T>()
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

        /// <summary>
        /// 检查该行是否为空
        /// </summary>
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


        #endregion

        #region property list

        /// <summary>
        /// TCC_TCC码位表
        /// </summary>
        public Dictionary<string, List<CommModel>> TCC2TCCDic { get; set; }

        /// <summary>
        /// TCC-TCC码位表
        /// </summary>
        public List<CommModel> TCC2TCCList { get; set; }

        /// <summary>
        /// TCC-CBI码位表
        /// </summary>
        public List<CommModel> TCC2CBIList { get; set; }

        /// <summary>
        /// TCC-Zpw码位表
        /// </summary>
        public TccZpwModel Tcc2ZpwInterface { get; set; } 

        /// <summary>
        /// Tcc2Ctc表
        /// </summary>
        public TccCommModel Tcc2CtcInterface { get; set; } 

        /// <summary>
        /// Tcc2Tsrs表
        /// </summary>
        public TccCommModel Tcc2TsrsInterface { get; set; }

        /// <summary>
        /// Tcc2Dm表
        /// </summary>
        public TccCommModel Tcc2DmInterface { get; set; }

        /// <summary>
        /// 码位表
        /// </summary>
        public List<CommModel> CommTccList { get; set; }

        /// <summary>
        ///   
        /// </summary>
        public List<CommModel> CommCbiTccList { get; set; }

        /// <summary>
        /// 单控区最多设备数量
        /// </summary>
        public Dictionary<string, int> MaxNumDic { get; set; }

        /// <summary>
        /// 单控区最少设备数量
        /// </summary>
        public Dictionary<string, int> MinNumDic { get; set; }
        #endregion
    }
}
