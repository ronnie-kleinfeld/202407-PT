using Comtec.DL.Repository.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class XXmlElement : BaseEntityIdModel {
        [XmlAttribute("xs")]
        [MaxLength(1000)]
        public string? XsXmlAttribute {
            get; set;
        }

        [XmlAttribute("xn")]
        [MaxLength(1000)]
        public string? XnXmlAttribute {
            get; set;
        }

        [XmlAttribute("bkg")]
        [MaxLength(1000)]
        public string? BkgXmlAttribute {
            get; set;
        }
    }
}