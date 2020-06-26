using System;
using System.IO;
using System.Threading.Tasks;
using MetroLog;

namespace EG.Sherpany.Boardroom.Tests.Fakes
{
    class FakeLogManager : ILogManager
    {
        public ILogger GetLogger<T>(LoggingConfiguration config = null) => new FakeLogger();

        public ILogger GetLogger(Type type, LoggingConfiguration config = null) => new FakeLogger();

        public ILogger GetLogger(string name, LoggingConfiguration config = null) => new FakeLogger();

        public Task<Stream> GetCompressedLogs() => null;

        public LoggingConfiguration DefaultConfiguration => null;
        public event EventHandler<LoggerEventArgs> LoggerCreated;
    }

    class FakeLogger : ILogger
    {
        public void Trace(string message, Exception ex = null)
        {
        }

        public void Trace(string message, params object[] ps)
        {
        }

        public void Debug(string message, Exception ex = null)
        {
        }

        public void Debug(string message, params object[] ps)
        {
        }

        public void Info(string message, Exception ex = null)
        {
        }

        public void Info(string message, params object[] ps)
        {
        }

        public void Warn(string message, Exception ex = null)
        {
        }

        public void Warn(string message, params object[] ps)
        {
        }

        public void Error(string message, Exception ex = null)
        {
        }

        public void Error(string message, params object[] ps)
        {
        }

        public void Fatal(string message, Exception ex = null)
        {
        }

        public void Fatal(string message, params object[] ps)
        {
        }

        public void Log(LogLevel logLevel, string message, Exception ex)
        {
        }

        public void Log(LogLevel logLevel, string message, params object[] ps)
        {
        }

        public bool IsEnabled(LogLevel level)
        {
            return true;
        }

        public string Name => "FakeLogger";
        public bool IsTraceEnabled => true;
        public bool IsDebugEnabled => true;
        public bool IsInfoEnabled => true;
        public bool IsWarnEnabled => true;
        public bool IsErrorEnabled => true;
        public bool IsFatalEnabled => true;
    }
}
