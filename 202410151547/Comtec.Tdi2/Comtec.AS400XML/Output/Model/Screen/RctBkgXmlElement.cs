using Comtec.AS400XML.Output.Model.Base;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Model.Screen {
    public class RctBkgXmlElement : BaseAllowedValuesValidationModel {
        [XmlElement("r")]
        public List<RXmlElement>? RXmlElementList {
            get; set;
        }

        public new bool IsValidXmlElement() {
            return IsValidXmlElement(RXmlElementList);
        }
    }
}