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

        public SiteConfigData SiteConfig {
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

        /// <summary>
        /// AS400XML Configuration
        /// </summary>
        public AS400XmlConfigData AS400Xml {
            get; set;
        }

        public AppConfigData() {
            ApplicationName = "Tdi2";
            Version = "11.0.0";
            SiteConfig = new SiteConfigData();
            Log = new LogConfigData();
            DL = new DLConfigData();
            AS400Xml = new AS400XmlConfigData();
        }
    }
}