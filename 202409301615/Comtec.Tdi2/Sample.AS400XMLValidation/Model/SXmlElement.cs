using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Sample.AS400XMLValidation.Model {
    public class SXmlElement : BaseAllowedValuesValidatorAttributeModel {
        [XmlAttribute("fil")]
        [MaxLength(1000)]
        public string? FilXmlAttribute {
            get; set;
        }
    }
}