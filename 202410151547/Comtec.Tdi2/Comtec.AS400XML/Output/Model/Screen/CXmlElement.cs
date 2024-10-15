using Comtec.AS400XML.Attributes;
using Comtec.AS400XML.Output.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Model.Screen {
    public class CXmlElement : BaseAllowedValuesValidationModel {
        [XmlAttribute("cgr"), MaxLength(1000), AS400XmlAllowedValues]
        public string? CgrXmlAttribute {
            get; set;
        }
        [XmlAttribute("fbua"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FbuaXmlAttribute {
            get; set;
        }
        [XmlAttribute("fk"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FkXmlAttribute {
            get; set;
        }
        [XmlAttribute("ft"), MaxLength(1000)] // , AS400XmlAllowedValues]
        public string? FtXmlAttribute {
            get; set;
        }
        [XmlAttribute("len"), MaxLength(1000)] // , AS400XmlAllowedValues]
        public string? LenXmlAttribute {
            get; set;
        }
        [XmlAttribute("pbg"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PbgXmlAttribute {
            get; set;
        }
        [XmlAttribute("pic"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PicXmlAttribute {
            get; set;
        }
        [XmlAttribute("pnt"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PntXmlAttribute {
            get; set;
        }
        [XmlAttribute("psz"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PszXmlAttribute {
            get; set;
        }
    }
}