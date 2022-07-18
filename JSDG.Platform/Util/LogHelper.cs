using System;
using System.Linq;
using log4net;
using log4net.Appender;

namespace JSDG.Platform.Util
{
    public class LogHelper
    {
        private LogHelper() { }

        private static readonly LogHelper _instance = new LogHelper();

        private ILog _loger = LogManager.GetLogger("loginfo");

        private ESubSystemType _subSystemType;

        public static LogHelper Instance
        {
            get { return _instance; }
        }

        public void Init(ESubSystemType subSystemType, String filePath)
        {
            this._subSystemType = subSystemType;
            var repository = LogManager.GetRepository();
            var appenders = repository.GetAppenders();
            var targetApder = appenders.First(p => p.Name == "InfoAppender") as RollingFileAppender;
            targetApder.File = String.Format("{0}\\{1}.log", filePath, _subSystemType.ToString());
            targetApder.ActivateOptions();
        }

        public void Log(String message)
        {
            this._loger.Info(message);
        }
    }
}
