namespace Comtec.AS400XML.Output.Data {
    public class XmlFileModel : AS400XmlOutputModel {
        public string XmlFileDirectory {
            get;
            set;
        }

        public string XmlFileName {
            get;
            set;
        }

        public string XmlFilePath => Path.Combine(XmlFileDirectory, XmlFileName);

        public string SourcePath {
            get;
            set;
        }

        public string? Hash {
            get;
            set;
        }

        public XmlFileModel() {
        }
        public XmlFileModel(string xmlFileDirectory, string xmlFileName, string sourcePath) {
            this.XmlFileDirectory = xmlFileDirectory;
            this.XmlFileName = xmlFileName;
            this.SourcePath = sourcePath;
        }
    }
}