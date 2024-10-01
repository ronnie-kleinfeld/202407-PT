using Comtec.AS400XML.Output.Model.Base;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Model.Screen {
    public class RctXmlElement : BaseAllowedValuesValidationModel {
        [XmlElement("ar")]
        public List<ArXmlElement>? ArXmlElementList {
            get; set;
        }
        [XmlElement("r")]
        public List<RXmlElement>? RXmlElementList {
            get; set;
        }

        public new bool IsValidXmlElement() {
            return IsValidXmlElement(RXmlElementList) && IsValidXmlElement(ArXmlElementList);
        }
    }
}