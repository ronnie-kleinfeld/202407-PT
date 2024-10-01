using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class YZonesBkgXmlElement {
        [XmlElement("y")]
        public List<YXmlElement> YXmlElementList {
            get; set;
        }
    }
}