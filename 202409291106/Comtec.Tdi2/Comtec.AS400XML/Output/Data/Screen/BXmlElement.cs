using Comtec.DL.Repository.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class BXmlElement : BaseEntityIdModel {
        [XmlAttribute("op")]
        [MaxLength(1000)]
        public string? OpXmlAttribute {
            get; set;
        }

        [XmlAttribute("nm")]
        [MaxLength(1000)]
        public string? NmXmlAttribute {
            get; set;
        }

        [XmlAttribute("ro")]
        [MaxLength(1000)]
        public string? RoXmlAttribute {
            get; set;
        }
    }
}