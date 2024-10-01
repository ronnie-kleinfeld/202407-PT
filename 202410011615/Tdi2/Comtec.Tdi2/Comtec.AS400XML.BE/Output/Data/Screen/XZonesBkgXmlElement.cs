using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class XZonesBkgXmlElement {
        [XmlElement("x")]
        public List<XXmlElement> XXmlElementList {
            get; set;
        }
    }
}