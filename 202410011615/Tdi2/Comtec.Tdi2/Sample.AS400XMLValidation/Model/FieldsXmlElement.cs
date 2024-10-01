using System.Xml.Serialization;

namespace Sample.AS400XMLValidation.Model {
    public class FieldsXmlElement : BaseAllowedValuesValidatorAttributeModel {
        [XmlElement("f")]
        public List<FXmlElement>? FXmlElementList {
            get; set;
        }
    }
}