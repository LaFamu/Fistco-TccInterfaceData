using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using JSDG.CBI.A.ETBModel;
using JSDG.CBI.A.ETBUtil;

namespace JSDG.CBI.A.ETBProcess
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
                datStruct.CommCbiTccStruct = new CommCbiTccStruct(dataModel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return datStruct;
        }
    }
}
