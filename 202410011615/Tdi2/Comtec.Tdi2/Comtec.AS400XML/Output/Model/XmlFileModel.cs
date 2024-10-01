namespace Comtec.AS400XML.Output.Model {
    public class XmlFileModel : AS400XmlOutputModel {
        public string XmlFilePath {
            get;
            set;
        }

        public string RawXML {
            get;
            set;
        }

        public string? Hash {
            get;
            set;
        }

        public XmlFileModel() {
        }
        public XmlFileModel(string xmlFilePath) {
            this.XmlFilePath = xmlFilePath;
        }
    }
}