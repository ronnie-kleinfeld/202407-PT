using Comtec.AS400XML.Output.Model.Base;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Model.Screen {
    public class XZonesXmlElement : BaseAllowedValuesValidationModel {
        [XmlElement("x")]
        public List<XXmlElement>? XXmlElementList {
            get; set;
        }

        public new bool IsValidXmlElement() {
            return IsValidXmlElement(XXmlElementList);
        }
    }
}