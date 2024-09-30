using Comtec.BE.Config.Data;
using Comtec.BE.Helpers;
using Serilog;
using Serilog.Events;
using System.Reflection;

namespace Comtec.BE.LogEx {
    public static class LogHelper {
        private static Serilog.Core.Logger _logger;
        private static LogEventLevel _logEventLevel;

        public static LogEventLevel LogLevel {
            get {
                return _logEventLevel;
            }
            set {
                _logEventLevel = value;
                _logger.IsEnabled(_logEventLevel);
                WriteInformation(MethodBase.GetCurrentMethod(), $"Log level set to {_logEventLevel}");
            }
        }

        static LogHelper() {
            LogToConsole();
            WriteInformation(MethodBase.GetCurrentMethod(), $"Log to Console initialized, Level: {LogLevel}");
        }

        public static void Init(LogConfigData logConfigData) {
            string logPath = logConfigData.LogPath == "" ? "log/log.txt" : logConfigData.LogPath;

            if (logConfigData.LogToConsole && logConfigData.LogToFile) {
                LogToConsoleAndFile(logConfigData.LogPath, logConfigData.RollingInterval);
            } else if (logConfigData.LogToConsole) {
                LogToConsole();
            } else if (logConfigData.LogToFile) {
                LogToFile(logConfigData.LogPath, logConfigData.RollingInterval);
            } else {
                LogToConsole();
                WriteFatal(MethodBase.GetCurrentMethod(), "log not registered");
            }

            LogLevel = logConfigData.LogEventLevel;
        }
        private static void LogToConsole() {
            _logger = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Debug()
                .CreateLogger();
            WriteInformation(MethodBase.GetCurrentMethod(), $"Log to Console initialized, Level: {LogLevel}");
        }
        private static void LogToFile(string logPath, RollingInterval rollingInterval) {
            _logger = new LoggerConfiguration()
                .WriteTo.File(logPath, rollingInterval: rollingInterval)
                .MinimumLevel.Debug()
                .CreateLogger();
            WriteInformation(MethodBase.GetCurrentMethod(), $"Log to File initialized, Path: {logPath}, Level: {LogLevel}");
        }
        private static void LogToConsoleAndFile(string logPath, RollingInterval rollingInterval) {
            _logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(logPath, rollingInterval: rollingInterval)
                .MinimumLevel.Debug()
                .CreateLogger();
            WriteInformation(MethodBase.GetCurrentMethod(), $"Log to Console and File initialized, Path: {logPath}, Level: {LogLevel}");
        }

        public static void WriteVerbose(MethodBase? methodBase, string message) {
            Write(methodBase, LogEventLevel.Verbose, message);
        }
        public static void WriteDebug(MethodBase? methodBase, string message) {
            Write(methodBase, LogEventLevel.Debug, message);
        }
        public static void WriteInformation(MethodBase? methodBase, string message) {
            Write(methodBase, LogEventLevel.Information, message);
        }
        public static void WriteWarning(MethodBase? methodBase, string message) {
            Write(methodBase, LogEventLevel.Warning, message);
        }
        public static void WriteError(MethodBase? methodBase, string message) {
            Write(methodBase, LogEventLevel.Error, message);
        }
        public static void WriteException(MethodBase? methodBase, Exception exception) {
            Write(methodBase, LogEventLevel.Error, exception.Message);
            if (exception.StackTrace != null) {
                Write(methodBase, LogEventLevel.Error, exception.StackTrace);
            }
        }
        public static void WriteFatal(MethodBase? methodBase, string message) {
            Write(methodBase, LogEventLevel.Fatal, message);
        }
        public static void Write(MethodBase? methodBase, LogEventLevel logEventLevel, string message) {
            string formatedMessage = FormatMessage(methodBase, message);

            switch (logEventLevel) {
                case LogEventLevel.Verbose:
                    _logger.Verbose(formatedMessage);
                    break;
                case LogEventLevel.Debug:
                    _logger.Debug(formatedMessage);
                    break;
                case LogEventLevel.Information:
                    _logger.Information(formatedMessage);
                    break;
                case LogEventLevel.Warning:
                    _logger.Warning(formatedMessage);
                    break;
                case LogEventLevel.Error:
                    _logger.Error(formatedMessage);
                    break;
                case LogEventLevel.Fatal:
                    _logger.Fatal(formatedMessage);
                    break;
                default:
                    Write(methodBase, LogEventLevel.Fatal, message);
                    break;
            }
        }

        private static string FormatMessage(MethodBase? method, string message) {
            if (method != null) {
                message = $"[{ReflectionHelper.ShortName(method)}] {message}";
            }

            return message;
        }
    }
}