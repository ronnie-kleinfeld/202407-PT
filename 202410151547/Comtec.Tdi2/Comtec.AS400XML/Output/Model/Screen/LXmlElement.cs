using Comtec.AS400XML.Attributes;
using Comtec.AS400XML.Output.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Model.Screen {
    public class LXmlElement : BaseAllowedValuesValidationModel {
        [XmlAttribute("fcl"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FclXmlAttribute {
            get; set;
        }
        [XmlAttribute("icn"), MaxLength(1000), AS400XmlAllowedValues]
        public string? IcnXmlAttribute {
            get; set;
        }
        [XmlAttribute("lk"), MaxLength(1000), AS400XmlAllowedValues]
        public string? LkXmlAttribute {
            get; set;
        }
        [XmlAttribute("ln"), MaxLength(1000)] // , AS400XmlAllowedValues]
        public string? LnXmlAttribute {
            get; set;
        }
        [XmlAttribute("lon"), MaxLength(1000), AS400XmlAllowedValues]
        public string? LonXmlAttribute {
            get; set;
        }
        [XmlAttribute("ltr"), MaxLength(1000), AS400XmlAllowedValues]
        public string? LtrXmlAttribute {
            get; set;
        }
    }
}