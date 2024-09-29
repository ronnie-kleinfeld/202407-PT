using Comtec.DL.Repository.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class CXmlElement : BaseEntityIdModel {
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

        [XmlAttribute("len")]
        [MaxLength(1000)]
        public string? LenXmlAttribute {
            get; set;
        }

        [XmlAttribute("cgr")]
        [MaxLength(1000)]
        public string? CgrXmlAttribute {
            get; set;
        }

        [XmlAttribute("fbua")]
        [MaxLength(1000)]
        public string? FbuaXmlAttribute {
            get; set;
        }

        [XmlAttribute("pic")]
        [MaxLength(1000)]
        public string? PicXmlAttribute {
            get; set;
        }

        [XmlAttribute("psz")]
        [MaxLength(1000)]
        public string? PszXmlAttribute {
            get; set;
        }

        [XmlAttribute("pnt")]
        [MaxLength(1000)]
        public string? PntXmlAttribute {
            get; set;
        }

        [XmlAttribute("pbg")]
        [MaxLength(1000)]
        public string? PbgXmlAttribute {
            get; set;
        }
    }
}