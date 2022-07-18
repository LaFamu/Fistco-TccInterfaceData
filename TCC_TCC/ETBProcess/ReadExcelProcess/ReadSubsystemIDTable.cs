using System;

using NPOI.SS.UserModel;

using MSDG.CBI.ETB.MainTrack.B.ETBUtil;


namespace MSDG.CBI.ETB.MainTrack.B.ETBProcess.ReadExcelProcess
{
    /// <summary>
    /// 读取子系统ID表数据
    /// </summary>
    public class ReadSubsystemIDTable : ReadParameterBase
    {
        /// <summary>
        /// 填充读取的Excel中的数据
        /// </summary>
        /// <typeparam name="TExcelModel">参数类型</typeparam>
        /// <param name="systemParaBase">参数实体</param>
        /// <param name="row">Excel数据行</param>
        protected override void SetParameter<TExcelModel>(TExcelModel systemParaBase, IRow row)
        {
            var nameCell = row.GetCell(row.FirstCellNum + 1);
            var valueCell = row.GetCell(row.FirstCellNum);
            var subSystemName = nameCell.ToString().PreprocessString();
            try
            {
                var subSystemID = Convert.ToInt32(valueCell.ToString().PreprocessString());
                systemParaBase.ParameterDiactionary.Add(subSystemName, (UInt32)subSystemID);
            }
            catch (ArgumentException)
            {
                throw new Exception(string.Format("子系统参数{0}值重复,请检查子系统ID表！", nameCell));
            }
            catch (Exception)
            {
                throw new Exception(String.Format("子系统参数'{0}'的值填写不正确！\r\n", subSystemName));
            }
        }
    }
}