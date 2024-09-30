using Comtec.AS400XML.Attributes;
using Comtec.AS400XML.Output.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Model.Screen {
    public class XXmlElement : BaseAllowedValuesValidationModel {
        [XmlAttribute("xs"), MaxLength(1000), AS400XmlAllowedValues]
        public string? XsXmlAttribute {
            get; set;
        }

        [XmlAttribute("xn"), MaxLength(1000), AS400XmlAllowedValues]
        public string? XnXmlAttribute {
            get; set;
        }

        [XmlAttribute("bkg"), MaxLength(1000), AS400XmlAllowedValues]
        public string? BkgXmlAttribute {
            get; set;
        }
    }
}