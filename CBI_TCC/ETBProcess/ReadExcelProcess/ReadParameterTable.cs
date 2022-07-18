using System;

using NPOI.SS.UserModel;

using MSDG.CBI.ETB.MainTrack.B.ETBUtil;



namespace MSDG.CBI.ETB.MainTrack.B.ETBProcess.ReadExcelProcess
{
    /// <summary>
    /// 读取参数表数据
    /// </summary>
    public class ReadParameterTable : ReadParameterBase
    {
        /// <summary>
        /// 填充读取的Excel中的数据
        /// </summary>
        /// <typeparam name="TExcelModel">参数类型</typeparam>
        /// <param name="systemParaBase">参数实体</param>
        /// <param name="row">Excel数据行</param>
        protected override void SetParameter<TExcelModel>(TExcelModel systemParaBase, IRow row)
        {
            var valueCell = row.GetCell(row.FirstCellNum + 1);
            var nameCell = row.GetCell(row.FirstCellNum);
            if (nameCell != null && valueCell != null)
            {
                var key = nameCell.ToString().PreprocessString();
                try
                {
                    var subSystemID = Convert.ToInt32(valueCell.ToString().PreprocessString());
                    var value = subSystemID == -1 ? UInt32.MaxValue : (UInt32)(subSystemID);
                    systemParaBase.ParameterDiactionary.Add(key, value);
                }
                catch (ArgumentException)
                {
                    throw new Exception(string.Format("系统参数{0}值重复,请检查参数表！", key));
                }
                catch (Exception)
                {
                    throw new Exception(String.Format("参数表中'{0}'的值填写不正确！\r\n", key));
                }
            }
        }
    }
}