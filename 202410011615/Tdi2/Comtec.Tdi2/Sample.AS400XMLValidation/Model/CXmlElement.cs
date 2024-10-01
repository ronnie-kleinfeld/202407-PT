using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Sample.AS400XMLValidation.Model {
    public class CXmlElement : BaseAllowedValuesValidatorAttributeModel {
        [XmlAttribute("fk")]
        [MaxLength(1000)]
        public string? FkXmlAttribute {
            get; set;
        }

        [XmlAttribute("ft")]
        [MaxLength(1000)]
        public string? FtXmlAttribute {
            get; set;
        }
    }
}