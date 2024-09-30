using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Sample.AS400XMLValidation.Model {
    public class FXmlElement : BaseAllowedValuesValidatorAttributeModel {
        [XmlAttribute("tab")]
        [MaxLength(1000)]
        public string? TabXmlAttribute {
            get; set;
        }

        [XmlElement("b")]
        public List<BXmlElement> BXmlElementList {
            get; set;
        }
    }
}