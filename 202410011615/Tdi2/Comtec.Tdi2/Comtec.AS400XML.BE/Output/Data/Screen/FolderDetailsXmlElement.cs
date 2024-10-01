using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class FolderDetailsXmlElement {
        [XmlElement("d")]
        public List<DXmlElement> DXmlElementList {
            get; set;
        }
    }
}