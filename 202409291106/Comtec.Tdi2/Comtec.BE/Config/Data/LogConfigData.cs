using Newtonsoft.Json;
using Serilog;
using Serilog.Events;

namespace Comtec.BE.Config.Data {
    public class LogConfigData {
        /// <summary>
        /// Write log messages to the console
        /// Default: true
        /// </summary>
        public bool LogToConsole {
            get; set;
        }
        /// <summary>
        /// Write log messages to the file
        /// Default: false
        /// </summary>
        public bool LogToFile {
            get; set;
        }
        /// <summary>
        /// Log file directory
        /// Relative to the executable directory
        /// </summary>
        public string LogFileDirectory {
            get; set;
        }
        /// <summary>
        /// Log file name
        /// Default: log.txt
        /// </summary>
        public string LogFileName {
            get; set;
        }
        /// <summary>
        /// Log file path
        /// </summary>
        [JsonIgnore] public string LogPath => Path.Combine(LogFileDirectory, LogFileName);
        /// <summary>
        /// Specifies the frequency at which the log file should roll.
        /// Default: RollingInterval.Day
        /// 0 - Infinite    The log file will never roll; no time period information will be appended to the log file name.
        /// 1 - Year        Roll every year. Filenames will have a four-digit year appended in the pattern <code>yyyy</code>.
        /// 2 - Month       Roll every calendar month. Filenames will have <code>yyyyMM</code> appended.
        /// 3 - Day         Roll every day. Filenames will have <code>yyyyMMdd</code> appended.
        /// 4 - Hour        Roll every hour. Filenames will have <code>yyyyMMddHH</code> appended.
        /// 5 - Minute      Roll every minute. Filenames will have <code>yyyyMMddHHmm</code> appended.
        /// </summary>
        public RollingInterval RollingInterval {
            get; set;
        }

        /// <summary>
        /// Log event level
        /// Default: Debug
        /// 
        /// 0 - Verbose     - Anything and everything you might want to know about a running block of code.
        /// 1 - Debug       - Internal system events that aren't necessarily observable from the outside.
        /// 2 - Information - The lifeblood of operational intelligence - things happen.
        /// 3 - Warning     - Service is degraded or endangered.
        /// 4 - Error       - Functionality is unavailable, invariants are broken or data is lost.
        /// 5 - Fatal       - If you have a pager, it goes off when one of these occurs.
        /// </summary>
        public LogEventLevel LogEventLevel {
            get; set;
        }

        public LogConfigData() {
#if DEBUG
            LogToConsole = true;
            LogToFile = false;
            LogFileDirectory = "log";
            LogFileName = "log.txt";
            RollingInterval = RollingInterval.Day;
            LogEventLevel = LogEventLevel.Debug;
#else
            LogToConsole = true;
            LogToFile = false;
            LogFileDirectory = "log";
            LogFileName = "log.txt";
            RollingInterval = RollingInterval.Day;
            LogEventLevel = LogEventLevel.Error;
#endif
        }
    }
}