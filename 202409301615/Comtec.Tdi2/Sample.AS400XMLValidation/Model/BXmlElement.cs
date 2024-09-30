using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Sample.AS400XMLValidation.Model {
    public class BXmlElement : BaseAllowedValuesValidatorAttributeModel {
        [XmlAttribute("op")]
        [MaxLength(1000)]
        public string? OpXmlAttribute {
            get; set;
        }
    }
}