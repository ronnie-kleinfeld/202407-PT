namespace Comtec.AS400XML.Output.Data {
    public class XmlFileModel(string xmlFileDirectory, string xmlFile, string sourcePath) : AS400XmlOutputModel {
        public string XmlFileDirectory {
            get;
        } = xmlFileDirectory;

        public string XmlFileName {
            get;
        } = xmlFile;

        public string XmlFilePath => Path.Combine(XmlFileDirectory, XmlFileName);

        public string SourcePath {
            get;
        } = sourcePath;

        public string? Hash {
            get; set;
        }
    }
}