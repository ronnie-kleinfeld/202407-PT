using Comtec.DL.Repository.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class ZXmlElement : BaseEntityIdModel {
        [XmlAttribute("zln")]
        [MaxLength(1000)]
        public string? ZlnXmlAttribute {
            get; set;
        }

        [XmlAttribute("zcl")]
        [MaxLength(1000)]
        public string? ZclXmlAttribute {
            get; set;
        }
    }
}