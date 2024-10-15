using Comtec.AS400XML.Output.Model.Screen;
using Comtec.DL.Model;
using Newtonsoft.Json;

namespace Comtec.AS400XML.Output.Model {
    public class AS400XmlOutputModel : BaseEntityIdModel {
        public string EnvironmentMachineName {
            get;
            set;
        }

        public string EnvironmentUserName {
            get;
            set;
        }

        public string EnviromentUserDomainName {
            get;
            set;
        }

        public string? GetFullPath {
            get;
            set;
        }

        public string? XmlFilePath {
            get;
            set;
        }

        public string RawXml {
            get;
            set;
        }

        public string Json {
            get;
            set;
        }

        [JsonIgnore]
        public string JsonZip {
            get {
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings {
                    Formatting = Formatting.None,
                    NullValueHandling = NullValueHandling.Ignore
                };
                return JsonConvert.SerializeObject(ScreenXmlRoot, jsonSerializerSettings);
            }
        }

        [JsonIgnore]
        public string JsonIndented {
            get {
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                };
                return JsonConvert.SerializeObject(ScreenXmlRoot, jsonSerializerSettings);
            }
        }

        public string? Hash {
            get;
            set;
        }

        public ScreenOutputXmlRoot? ScreenXmlRoot {
            get; set;
        }

        public AS400XmlOutputModel() {
            this.EnvironmentMachineName = Environment.MachineName;
            this.EnvironmentUserName = Environment.UserName;
            this.EnviromentUserDomainName = Environment.UserDomainName;
        }
        public AS400XmlOutputModel(string xmlFilePath) {
            this.EnvironmentMachineName = Environment.MachineName;
            this.EnvironmentUserName = Environment.UserName;
            this.EnviromentUserDomainName = Environment.UserDomainName;
            this.GetFullPath = Path.GetFullPath(xmlFilePath);
            this.XmlFilePath = xmlFilePath;
        }
    }
}