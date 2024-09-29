using Comtec.DL.Repository.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class YXmlElement : BaseEntityIdModel {
        [XmlAttribute("yt")]
        [MaxLength(1000)]
        public string? YtXmlAttribute {
            get; set;
        }

        [XmlAttribute("ys")]
        [MaxLength(1000)]
        public string? YsXmlAttribute {
            get; set;
        }

        [XmlAttribute("yn")]
        [MaxLength(1000)]
        public string? YnXmlAttribute {
            get; set;
        }

        [XmlAttribute("bkg")]
        [MaxLength(1000)]
        public string? BkgXmlAttribute {
            get; set;
        }
    }
}