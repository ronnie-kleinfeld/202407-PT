using Comtec.DL.Repository.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class ArXmlElement : BaseEntityIdModel {
        [XmlAttribute("rx1")]
        [MaxLength(1000)]
        public string? Rx1XmlAttribute {
            get; set;
        }

        [XmlAttribute("ry1")]
        [MaxLength(1000)]
        public string? Ry1XmlAttribute {
            get; set;
        }

        [XmlAttribute("rx2")]
        [MaxLength(1000)]
        public string? Rx2XmlAttribute {
            get; set;
        }

        [XmlAttribute("ry2")]
        [MaxLength(1000)]
        public string? Ry2XmlAttribute {
            get; set;
        }

        [XmlAttribute("rcl")]
        [MaxLength(1000)]
        public string? RclXmlAttribute {
            get; set;
        }

        [XmlAttribute("ficn")]
        [MaxLength(1000)]
        public string? FicnXmlAttribute {
            get; set;
        }

        [XmlAttribute("ftit")]
        [MaxLength(1000)]
        public string? FtitXmlAttribute {
            get; set;
        }
    }
}