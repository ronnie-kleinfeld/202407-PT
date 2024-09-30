using Comtec.AS400XML.Output.Model.Base;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Model.Screen {
    public class FieldsXmlElement : BaseAllowedValuesValidationModel {
        [XmlElement("f")]
        public List<FXmlElement>? FXmlElementList {
            get; set;
        }

        public new bool IsValidXmlElement() {
            return IsValidXmlElement(FXmlElementList);
        }
    }
}