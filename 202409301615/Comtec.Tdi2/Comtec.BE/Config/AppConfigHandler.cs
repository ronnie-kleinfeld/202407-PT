using Comtec.BE.Config.Data;
using Comtec.BE.LogEx;
using Newtonsoft.Json;
using System.Reflection;

namespace Comtec.BE.Config {
    public class AppConfigHandler {
        // consts
        private const string AppConfigFileName = "AppConfig.json";

        // members
        private AppConfigData _appConfigData;
        public AppConfigData AppConfigData {
            get {
                return _appConfigData;
            }
            set {
                _appConfigData = value;
            }
        }

        // ctor
        public AppConfigHandler() {
            _appConfigData = new AppConfigData();
        }

        // singleton
        private static AppConfigHandler _instance;
        private static AppConfigHandler Instance {
            get {
                if (_instance == null) {
                    _instance = new AppConfigHandler();
                    Init();
                }
                return _instance;
            }
        }
        public static AppConfigData Data {
            get {
                return AppConfigHandler.Instance.AppConfigData;
            }
        }

        // methods
        public static void Init() {
            Instance.Load();
            Instance.Save();
        }
        private void Save() {
            try {
#if DEBUG
                string json = JsonConvert.SerializeObject(_appConfigData, Formatting.Indented);
#else
                string json = JsonConvert.SerializeObject(_appConfigData, Formatting.None);
#endif

                File.WriteAllText(AppConfigFileName, json);
            } catch (Exception ex) {
                LogHelper.WriteException(MethodBase.GetCurrentMethod(), ex);
            }

        }
        private void Load() {
            try {
                if (File.Exists(AppConfigFileName)) {
                    LogHelper.WriteDebug(MethodBase.GetCurrentMethod(), $"App config file: {AppConfigFileName} found");
                    string json = File.ReadAllText(AppConfigFileName);
                    if (json.Length > 0) {
                        _appConfigData = JsonConvert.DeserializeObject<AppConfigData>(json);
                        if (_appConfigData == null) {
                            _appConfigData = new AppConfigData();
                            Save();
                        }
                    } else {
                        _appConfigData = new AppConfigData();
                        Save();
                    }
                } else {
                    _appConfigData = new AppConfigData();
                    Save();
                }
            } catch (Exception ex) {
                LogHelper.WriteFatal(MethodBase.GetCurrentMethod(), $"Failed to load config from file {AppConfigFileName}");
                LogHelper.WriteException(MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}