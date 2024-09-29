using Comtec.DL.Repository.Base;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class RctXmlElement : BaseEntityIdModel {
        [XmlElement("r")]
        public List<RXmlElement> RXmlElementList {
            get; set;
        }

        [XmlElement("ar")]
        public List<ArXmlElement> ArXmlElementList {
            get; set;
        }
    }
}