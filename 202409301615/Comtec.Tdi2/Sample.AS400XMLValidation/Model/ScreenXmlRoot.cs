using System.Xml.Serialization;

namespace Sample.AS400XMLValidation.Model {
    [XmlRoot("screen")]
    public class ScreenXmlRoot : BaseAllowedValuesValidatorAttributeModel {
        [XmlElement("s")]
        public SXmlElement? SXmlElement {
            get; set;
        }

        [XmlElement("cmd")]
        public CmdXmlElement? CmdXmlElement {
            get; set;
        }

        [XmlElement("fields")]
        public FieldsXmlElement? FieldsXmlElement {
            get; set;
        }
    }
}