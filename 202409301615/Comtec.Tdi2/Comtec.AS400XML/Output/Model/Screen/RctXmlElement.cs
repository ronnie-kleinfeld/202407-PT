using System.Xml.Serialization;
using Comtec.AS400XML.Output.Model.Base;

namespace Comtec.AS400XML.Output.Model.Screen {
    public class RctXmlElement : BaseAllowedValuesValidationModel {
        [XmlElement("r")]
        public List<RXmlElement>? RXmlElementList {
            get; set;
        }

        [XmlElement("ar")]
        public List<ArXmlElement>? ArXmlElementList {
            get; set;
        }

        public new bool IsValidXmlElement() {
            return IsValidXmlElement(RXmlElementList) &&
                   IsValidXmlElement(ArXmlElementList);
        }
    }
}