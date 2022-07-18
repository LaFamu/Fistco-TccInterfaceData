using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using TCC_TCC.A.ETBModel;
using TCC_TCC.A.ETBUtil;
using TCC_TCC.A.ETBModel.BinaryModel.BinaryStruct;

namespace TCC_TCC.A.ETBProcess
{
    class ConvertToBinary
    {
        /// <summary>
        /// 返回二进制总结构体DATStruct
        /// </summary>
        public static DATStruct ReturnDATStruct(DataModel dataModel, ref StringBuilder conflictSB)
        {
            MsgHelper.Instance(1, string.Format("==============================正在生成二进制============================"));
            DATStruct datStruct = new DATStruct();
            try
            {
                datStruct.Head_Tail_Struct = new Head_Tail_Struct(dataModel);
                datStruct.CommTccStruct = new CommTccStruct(dataModel);
                datStruct.CommTcc2CbiStruct = new CommCbiTccStruct(dataModel);
                datStruct.CommTcc2ZpwStruct = new CommTcc2ZpwStruct(dataModel);
                datStruct.CommTcc2CtcStruct = new CommTcc2CtcStruct(dataModel);
                datStruct.CommTcc2DmStruct = new CommTcc2DmStruct(dataModel);
                datStruct.CommTcc2TsrsStruct = new CommTcc2TsrsStruct(dataModel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return datStruct;
        }
    }
}
