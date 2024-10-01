using System.Xml.Serialization;

namespace Sample.AS400XMLValidation.Model {
    public class CmdXmlElement : BaseAllowedValuesValidatorAttributeModel {
        [XmlElement("c")]
        public List<CXmlElement>? CXmlElementList {
            get; set;
        }
    }
}