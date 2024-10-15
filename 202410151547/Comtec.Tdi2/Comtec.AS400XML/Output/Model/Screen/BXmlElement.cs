using Comtec.AS400XML.Attributes;
using Comtec.AS400XML.Output.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Model.Screen {
    public class BXmlElement : BaseAllowedValuesValidationModel {
        [XmlAttribute("nm"), MaxLength(1000), AS400XmlAllowedValues]
        public string? NmXmlAttribute {
            get; set;
        }
        [XmlAttribute("op"), MaxLength(1000), AS400XmlAllowedValues]
        public string? OpXmlAttribute {
            get; set;
        }
        [XmlAttribute("ro"), MaxLength(1000), AS400XmlAllowedValues]
        public string? RoXmlAttribute {
            get; set;
        }
    }
}