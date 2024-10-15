using System.Xml.Serialization;

namespace Comtec.AS400XML.Input.Model.Screen {
    public class FieldsInputXmlElement {
        [XmlElement("f")]
        public List<FInputXmlElement>? FInputXmlElementList {
            get; set;
        }

        public FieldsInputXmlElement() {
            FInputXmlElementList = new List<FInputXmlElement>();
        }
    }
}