using System;
using System.Collections.Generic;
using System.Linq;
using TCC_TCC.A.ETBProcess;
using TCC_TCC.A.ETBUtil;

namespace TCC_TCC.A.ETBModel
{
    public class Head_Tail_Struct
    {
        public Head_Tail_Struct(DataModel dataModel)
        {
            MsgHelper.Instance(1, string.Format("正在生成结构体《Head_Tail_Struct》"));

            this.GenerateDate = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"), 16);
            this.MajorVersion = 2;
            this.MinorVersion = 1;
            this.DataVersion = 3002;
            this.StationName = "同济站";
        }

        /// <summary>
        ///  数据生成日期，按照十六进制显示。（YYYYMMDD）
        /// </summary>
        public UInt32 GenerateDate;

        /// <summary>
        ///  主版本号。2
        /// </summary>
        public UInt32 MajorVersion;

        /// <summary>
        ///   次版本号。1
        /// </summary>
        public UInt32 MinorVersion;

        /// <summary>
        ///    数据版本号，由联锁表数据版本号确定
        /// </summary>
        public UInt32 DataVersion;

        /// <summary>
        ///   站场名称，实为联锁表目录项目名称
        /// </summary>
        public string StationName;
    }
}
