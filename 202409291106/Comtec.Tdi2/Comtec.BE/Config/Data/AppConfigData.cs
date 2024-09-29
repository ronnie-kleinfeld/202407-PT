namespace Comtec.BE.Config.Data {
    public class AppConfigData {
        /// <summary>
        /// Application Name
        /// </summary>
        public string ApplicationName {
            get; set;
        }

        /// <summary>
        /// Application Version
        /// </summary>
        public string Version {
            get; set;
        }

        public ClientConfigData ClientConfig {
            get; set;
        }

        /// <summary>
        /// Log Configuration
        /// </summary>
        public LogConfigData Log {
            get; set;
        }

        /// <summary>
        /// Data Layer Configuration
        /// </summary>
        public DLConfigData DL {
            get; set;
        }

        public AppConfigData() {
            ApplicationName = "Tdi2";
            Version = "11.0.0";
            ClientConfig = new ClientConfigData();
            Log = new LogConfigData();
            DL = new DLConfigData();
        }
    }
}