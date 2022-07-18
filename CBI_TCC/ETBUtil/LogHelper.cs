using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;
using log4net.Appender;

namespace JSDG.CBI.A.ETBUtil
{
    public enum VA_LogType
    {
        Undefined,
        LOG_OBCU,
        LOG_ZC,
        LOG_TranspMsg,
        LOG_BinComp,
        LOG_ATSLine,
        LOG_TATS,
        LOG_OCG,
        LOG_ATS,
        LOG_ES,
        LOG_ETB,
        LOG_POCT
    }

    public class LogHelper
    {
        private ILog m_log = null;
        private string m_filePath = "";
        private static volatile LogHelper m_inst;
        private static object syncRoot = new object();

        private Dictionary<VA_LogType, string> m_logFileName = new Dictionary<VA_LogType, string>();
        VA_LogType subSysType = VA_LogType.Undefined;

        private LogHelper()
        {
            m_logFileName[VA_LogType.LOG_OBCU] = "OBCU_Conv_log_A.log";
            m_logFileName[VA_LogType.LOG_ZC] = "ZC_Conv_log_A.log";
            m_logFileName[VA_LogType.LOG_TranspMsg] = "TraspMsg_log_A.log";
            m_logFileName[VA_LogType.LOG_BinComp] = "BinComp_log_A.log";
            m_logFileName[VA_LogType.LOG_ATSLine] = "ATSLine_log_A.log";
            m_logFileName[VA_LogType.LOG_TATS] = "TATS_log_A.log";
            m_logFileName[VA_LogType.LOG_OCG] = "OCG_log_A.log";
            m_logFileName[VA_LogType.LOG_ATS] = "ATS_log_A.log";
            m_logFileName[VA_LogType.LOG_ES] = "ES_log_A.log";
            m_logFileName[VA_LogType.LOG_ETB] = "ETB_log_A.log";
            m_logFileName[VA_LogType.LOG_POCT] = "POCT_log_A.log";
            m_logFileName[VA_LogType.Undefined] = "UnknownError_log_A.log";
        }

        public static LogHelper Inst
        {
            get
            {
                if (m_inst == null)
                {
                    lock (syncRoot)
                    {
                        if (m_inst == null)
                        {
                            m_inst = new LogHelper();
                        }
                    }
                }

                return m_inst;
            }
        }

        public void Init(VA_LogType vt, string filePath)
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo("Util\\ConfigFile\\log4net.xml"));

            m_log = LogManager.GetLogger("loginfo");
            m_filePath = filePath;
            subSysType = vt;
        }

        //LogType在执行init方法时已经传值，调用者无需再传入VA_LogType枚举
        public void Log(string str)
        {
            lock (this)
            {
                var targetApder = LogManager.GetRepository().GetAppenders().First(p => p.Name == "InfoAppender") as RollingFileAppender;
                targetApder.File = m_filePath + m_logFileName[subSysType];
                targetApder.ActivateOptions();

                //写log

                if (m_log.IsInfoEnabled)
                {
                    m_log.Info(str);
                }
            }
        }

        public void LogException(string info,Exception e)
        {
            Log(info);
            MsgHelper.Instance(-1,info);
            throw new Exception(info,e);
        }
        public void LogException(string info)
        {
            Log(info);
            MsgHelper.Instance(-1, info);
            throw new Exception(info);
        }
    }

    public class MsgHelper
    {
        public static Action<int, string> Instance { get; private set; }

        public static void Init(Action<int, string> act)
        {
            if (Instance == null)
                Instance = act;
        }
    }
}
