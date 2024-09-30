using Comtec.AS400XML.Attributes;
using Comtec.AS400XML.Output.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Model.Screen {
    public class YXmlElement : BaseAllowedValuesValidationModel {
        [XmlAttribute("yt"), MaxLength(1000), AS400XmlAllowedValues]
        public string? YtXmlAttribute {
            get; set;
        }

        [XmlAttribute("ys"), MaxLength(1000), AS400XmlAllowedValues]
        public string? YsXmlAttribute {
            get; set;
        }

        [XmlAttribute("yn"), MaxLength(1000), AS400XmlAllowedValues]
        public string? YnXmlAttribute {
            get; set;
        }

        [XmlAttribute("bkg"), MaxLength(1000), AS400XmlAllowedValues]
        public string? BkgXmlAttribute {
            get; set;
        }
    }
}