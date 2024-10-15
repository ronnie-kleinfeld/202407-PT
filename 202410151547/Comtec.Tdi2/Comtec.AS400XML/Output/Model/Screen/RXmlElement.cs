using Comtec.AS400XML.Attributes;
using Comtec.AS400XML.Output.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Model.Screen {
    public class RXmlElement : BaseAllowedValuesValidationModel {
        [XmlAttribute("bkg"), MaxLength(1000), AS400XmlAllowedValues]
        public string? BkgXmlAttribute {
            get; set;
        }
        [XmlAttribute("ficn"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FicnXmlAttribute {
            get; set;
        }
        [XmlAttribute("ftit"), MaxLength(1000)] // , AS400XmlAllowedValues]
        public string? FtitXmlAttribute {
            get; set;
        }
        [XmlAttribute("rcl"), MaxLength(1000), AS400XmlAllowedValues]
        public string? RclXmlAttribute {
            get; set;
        }
        [XmlAttribute("rx1"), MaxLength(1000), AS400XmlAllowedValues]
        public string? Rx1XmlAttribute {
            get; set;
        }
        [XmlAttribute("rx2"), MaxLength(1000), AS400XmlAllowedValues]
        public string? Rx2XmlAttribute {
            get; set;
        }
        [XmlAttribute("ry1"), MaxLength(1000), AS400XmlAllowedValues]
        public string? Ry1XmlAttribute {
            get; set;
        }
        [XmlAttribute("ry2"), MaxLength(1000), AS400XmlAllowedValues]
        public string? Ry2XmlAttribute {
            get; set;
        }
    }
}