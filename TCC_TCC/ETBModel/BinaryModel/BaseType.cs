using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC_TCC.A.ETBModel
{
    public struct DEVICE_ID
    {
        public DEVICE_ID(UInt32 id, int index)
        {
            if (id != UInt32.MaxValue)
            {
                Id = id << 16 ^ id;
                Index = (UInt32)index;
            }
            else
            {
                Id = UInt32.MaxValue;
                Index = UInt32.MaxValue;
            }
            
            
        }

        /// <summary>
        /// ID
        /// </summary>
        public UInt32 Id;

        /// <summary>
        /// id顺序
        /// </summary>
        public UInt32 Index;
    }

    public struct TCC_INF_VER
    {
        public Byte[] SpecVer;
        public Byte[] DataVer;
    }

    public class LOGIC_FACTOR
    {
        public UInt32 bAvailable = 0;
        public UInt32 Type = 0xFFFFFFFF;
        public DEVICE_ID DeviceID = new DEVICE_ID(0xFFFFFFFF, int.MaxValue);
        public UInt32 Condition = 0xFFFFFFFF;
        public UInt32 LogicalOperationType = 0xFFFFFFFF;
        public UInt32 LeftLeaf = 0xFFFFFFFF;
        public UInt32 RightLeaf = 0xFFFFFFFF;
    }

    public class OTHER_LOGIC
    {
        public UInt32 Type = 0xFFFFFFFF;
        public DEVICE_ID Id = new DEVICE_ID(0xFFFFFFFF, int.MaxValue);
    }

    public class CIOAddress
    {
        public UInt32 ByteOffset = 0xFFFFFFFF;
        public UInt32 BitOffset = 0xFFFFFFFF;
    }

    public class CommAddress
    {
        public UInt32 ByteOffset = 0xFFFFFFFF;
        public UInt32 BitOffset = 0xFFFFFFFF;
        public UInt32 Width = 0xFFFFFFFF; 
    }

    public class DataBlockAddress
    {
        public UInt32 ByteOffset = 0xFFFFFFFF;
        public UInt32 ByteLen = 0xFFFFFFFF;
        public UInt32 DevNum = 0xFFFFFFFF;
    }

    public class XLBJ
    {
        public UInt64 ullId = 0xFFFFFFFFFFFFFFFF;
        public CommAddress BSFQ_YL0;
        public CommAddress BSFQ_stu;
        public CommAddress BSFQDP_flag;
        public CommAddress BSFQDP_code;
        public CommAddress BSFQDP_attribute;
        public CommAddress BSFQ_YL1;
        public CommAddress FHXH1;
        public CommAddress FHXH2;
        public CommAddress FHXH3;
        public CommAddress BSFQ_YL2;
        public CommAddress BSFQ_YL3;
    }

    public class XLGF
    {
        public UInt64 ullId = 0xFFFFFFFFFFFFFFFF;
        public CommAddress BZQJ_stu;
        public CommAddress XLJL_flag;
        public CommAddress XLJLSB;
        public CommAddress XLFX_msg;
        public CommAddress XLGF_cmd;
        public CommAddress XLGF_type;
    }

    public class ZJZXH
    {
        public UInt64 ullId = 0xFFFFFFFFFFFFFFFF;
        public CommAddress ZJXH;
    }

    public class ZJZBSFQ
    {
        public UInt64 ullId = 0xFFFFFFFFFFFFFFFF;
        public CommAddress BSFQ_DP;
        public CommAddress BSFQ_ZT;
    }

    public class ZJZYWQX
    {
        public UInt64 ullId = 0xFFFFFFFFFFFFFFFF;
        public CommAddress ZJZ_YWQX;
    }

    public class XTLEU
    {
        public UInt64 ullId = 0xFFFFFFFFFFFFFFFF;
        public CommAddress COMtoCBI;
        public CommAddress COMtoCTC;
        public CommAddress ZJwork_stu;
        public CommAddress DSJwork_stu;
        public CommAddress COMtoTCC;
        public CommAddress COMtoTSRS;
        public CommAddress COMtoMain;
        public CommAddress BSFQ_initial;
        public CommAddress LEU1_TP1;
        public CommAddress LEU1_TP2;
        public CommAddress LEU1_TP3;
        public CommAddress LEU1_TP4;
        public CommAddress LEU1_TD1;
        public CommAddress LEU1_TD2;
        public CommAddress LEU1CFG_avlble;
        public CommAddress LEU2_TP1;
        public CommAddress LEU2_TP2;
        public CommAddress LEU2_TP3;
        public CommAddress LEU2_TP4;
        public CommAddress LEU2_TD1;
        public CommAddress LEU2_TD2;
        public CommAddress LEU2CFG_avlble;
        public CommAddress LEU3_TP1;
        public CommAddress LEU3_TP2;
        public CommAddress LEU3_TP3;
        public CommAddress LEU3_TP4;
        public CommAddress LEU3_TD1;
        public CommAddress LEU3_TD2;
        public CommAddress LEU3CFG_avlble;
        public CommAddress LEU4_TP1;
        public CommAddress LEU4_TP2;
        public CommAddress LEU4_TP3;
        public CommAddress LEU4_TP4;
        public CommAddress LEU4_TD1;
        public CommAddress LEU4_TD2;
        public CommAddress LEU4CFG_avlble;
    }

    public class WPXXQJ
    {
        public UInt64 ullId = 0xFFFFFFFFFFFFFFFF;
        public CommAddress XQJ_stu;
    }

    public class XHXK
    {
        public UInt64 ullId = 0xFFFFFFFFFFFFFFFF;
        public CommAddress XHXK_ID;
        public CommAddress XHXK_ZT;
        public CommAddress XHXK_LX;
        public CommAddress XHXK_ZY;
        public CommAddress XHXK_YL;
    }

    public class QJBS
    {
        public UInt64 ullId = 0xFFFFFFFFFFFFFFFF;
        public CommAddress TCC_ID;
        public CommAddress QRZT;
        public CommAddress CMD_type;
        public CommAddress QJK_ID;
        public CommAddress CTC_ID;
        public CommAddress ZJZ_CMDJG;
        public CommAddress ZJZ_CMDYY;
    }

    public class Reserved
    {
        public UInt64 ullId = 0xFFFFFFFFFFFFFFFF;
        public CommAddress XH_KDJ;
        public CommAddress XH_LXJ;
        public CommAddress XH_YXJ;
        public CommAddress XH_TXJ;
    }

    public class RM
    {
        public UInt64 ullId = 0xFFFFFFFFFFFFFFFF;
        public CommAddress RM_ID;
        public CommAddress RM_SIGS;
        public CommAddress RM_SIGDS;
        public CommAddress RM_BLKS1;
        public CommAddress RM_BLKS2;
        public CommAddress RM_BLKS3;
        public CommAddress RM_BLKS4;
    }
    public class InterfaceVersions
    {
        public UInt64 ullId = 0xFFFFFFFFFFFFFFFF;
        public CommAddress ITFspecification_version;
        public CommAddress ITFDatas_version;
    }

    public class CbiTcc_A1
    {
        public UInt64 ullId = 0xFFFFFFFFFFFFFFFF;
        public CommAddress[] MsgAdderss = new CommAddress[1];
    }

    public class CbiTcc_A2
    {
        public UInt64 ullId = 0xFFFFFFFFFFFFFFFF;
        public CommAddress[] MsgAdderss = new CommAddress[2];
    }

    public class CbiTcc_A3
    {
        public UInt64 ullId = 0xFFFFFFFFFFFFFFFF;
        public CommAddress[] MsgAdderss = new CommAddress[3];
    }

    public class CbiTcc_A4
    {
        public UInt64 ullId = 0xFFFFFFFFFFFFFFFF;
        public CommAddress[] MsgAdderss = new CommAddress[4];
    }

    public class CbiTcc_A5
    {
        public UInt64 ullId = 0xFFFFFFFFFFFFFFFF;
        public CommAddress[] MsgAdderss = new CommAddress[5];
    }

    public class CbiTcc_A6
    {
        public UInt64 ullId = 0xFFFFFFFFFFFFFFFF;
        public CommAddress[] MsgAdderss = new CommAddress[6];
    }

    public class CbiTcc_A24
    {
        public UInt64 ullId = 0xFFFFFFFFFFFFFFFF;
        public CommAddress[] MsgAdderss = new CommAddress[24];
    }

    public struct RouteSwitch
    {
        public DEVICE_ID Switch_ID;//12
        public UInt32 Positon;
    }

    public struct FoulingBlock
    {
        public DEVICE_ID Block_ID;
        public RouteSwitch[] RouteSwitches;//16*3
    }

    public struct ConflictSignal
    {

        public DEVICE_ID Conflictsignal;//12
        public UInt32 ConflictType;
        public RouteSwitch[] ConditionSwitch;//16*2

    }

    public struct InterfaceInfo
    {
        public string InterfaceName;
        public UInt32 InterfaceByteNum;
        public UInt32 InterfaceNum;
    }

    public struct WaysideInfo
    {
        public string SegmentName;
        public UInt32 StructWidth;
        public UInt32 TotalStruct;
    }

    public struct Path
    {
        public DEVICE_ID Block_ID;//12
        public RouteSwitch[] CondSwitch;//16*4
    }
}
