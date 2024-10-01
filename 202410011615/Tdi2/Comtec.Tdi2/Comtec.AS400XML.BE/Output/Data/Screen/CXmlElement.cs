using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class CXmlElement {
        [XmlAttribute("fk")]
        public string? FkXmlAttribute {
            get; set;
        }

        [XmlAttribute("ft")]
        public string? FtXmlAttribute {
            get; set;
        }

        [XmlAttribute("len")]
        public string? LenXmlAttribute {
            get; set;
        }

        [XmlAttribute("cgr")]
        public string? CgrXmlAttribute {
            get; set;
        }

        [XmlAttribute("fbua")]
        public string? FbuaXmlAttribute {
            get; set;
        }

        [XmlAttribute("pic")]
        public string? PicXmlAttribute {
            get; set;
        }

        [XmlAttribute("psz")]
        public string? PszXmlAttribute {
            get; set;
        }

        [XmlAttribute("pnt")]
        public string? PntXmlAttribute {
            get; set;
        }

        [XmlAttribute("pbg")]
        public string? PbgXmlAttribute {
            get; set;
        }
    }
}