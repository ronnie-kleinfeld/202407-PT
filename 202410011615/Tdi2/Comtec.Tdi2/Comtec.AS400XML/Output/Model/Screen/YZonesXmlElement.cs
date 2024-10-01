using Comtec.AS400XML.Output.Model.Base;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Model.Screen {
    public class YZonesXmlElement : BaseAllowedValuesValidationModel {
        [XmlElement("y")]
        public List<YXmlElement>? YXmlElementList {
            get; set;
        }

        public new bool IsValidXmlElement() {
            return IsValidXmlElement(YXmlElementList);
        }
    }
}