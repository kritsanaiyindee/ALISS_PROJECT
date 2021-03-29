using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using log4net;
using log4net.Repository;

namespace Log4NetLibrary
{
    public class LogService : ILogService
    {
        private readonly ILog _logger;

        static LogService()
        {
            var log4NetConfigDirectory = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;

            var log4NetConfigFilePath = Path.Combine(log4NetConfigDirectory, "log4net.config");
            //log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(log4NetConfigFilePath));

            //

            XmlDocument log4netConfig = new XmlDocument();
            //log4netConfig.Load(File.OpenRead("log4net.config"));
            log4netConfig.Load(log4NetConfigFilePath);

            var repo = log4net.LogManager.CreateRepository(
                Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
        }

        public LogService(Type logClass)
        {
            _logger = LogManager.GetLogger(logClass);
        }

        public void Fatal(string message)
        {
            if (_logger.IsFatalEnabled)
                _logger.Fatal(message);
        }

        public void Error(string message)
        {
            if (_logger.IsErrorEnabled)
                _logger.Error(message);
        }

        public void Error(Exception message)
        {
            if (_logger.IsErrorEnabled)
                _logger.Error(message);
        }

        public void Warn(string message)
        {
            if (_logger.IsWarnEnabled)
                _logger.Warn(message);
        }

        public void Info(string message)
        {
            if (_logger.IsInfoEnabled)
                _logger.Info(message);
        }

        public void Debug(string message)
        {
            if (_logger.IsDebugEnabled)
                _logger.Debug(message);
        }

        public void MethodStart([CallerMemberName] string callerName = "")
        {
            if (_logger.IsInfoEnabled)
                _logger.Info(callerName + " : MethodStart" );
        }

        public void MethodFinish([CallerMemberName] string callerName = "")
        {
            if (_logger.IsInfoEnabled)
                _logger.Info(callerName + " : MethodFinish");
        }
    }
}
