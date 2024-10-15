using System.Xml.Serialization;

namespace Comtec.AS400XML.Input.Model.Screen {
    [XmlRoot("screen")]
    public class ScreenInputXmlRoot {
        [XmlElement("s")]
        public SInputXmlElement SInputXmlElement {
            get; set;
        }

        [XmlElement("cmd")]
        public CmdInputXmlElement? CmdInputXmlElement {
            get; set;
        }

        [XmlElement("fields")]
        public FieldsInputXmlElement? FieldsInputXmlElement {
            get; set;
        }

        public ScreenInputXmlRoot() {
        }
        public ScreenInputXmlRoot(SInputXmlElement sInputXmlElement, List<CInputXmlElement>? cInputXmlElementList, List<FInputXmlElement>? fInputXmlElementList) {
            SInputXmlElement = sInputXmlElement;

            CmdInputXmlElement = new CmdInputXmlElement();
            CmdInputXmlElement.CInputXmlElementList = cInputXmlElementList;

            FieldsInputXmlElement = new FieldsInputXmlElement();
            FieldsInputXmlElement.FInputXmlElementList = fInputXmlElementList;
        }
    }
}