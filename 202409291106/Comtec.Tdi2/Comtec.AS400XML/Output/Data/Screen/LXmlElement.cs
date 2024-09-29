using Comtec.DL.Repository.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class LXmlElement : BaseEntityIdModel {
        [XmlAttribute("lk")]
        [MaxLength(1000)]
        public string? LkXmlAttribute {
            get; set;
        }

        [XmlAttribute("ltr")]
        [MaxLength(1000)]
        public string? LtrXmlAttribute {
            get; set;
        }

        [XmlAttribute("lon")]
        [MaxLength(1000)]
        public string? LonXmlAttribute {
            get; set;
        }

        [XmlAttribute("ln")]
        [MaxLength(1000)]
        public string? LnXmlAttribute {
            get; set;
        }

        [XmlAttribute("icn")]
        [MaxLength(1000)]
        public string? IcnXmlAttribute {
            get; set;
        }

        [XmlAttribute("fcl")]
        [MaxLength(1000)]
        public string? FclXmlAttribute {
            get; set;
        }
    }
}