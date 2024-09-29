using Comtec.DL.Repository.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class DXmlElement : BaseEntityIdModel {
        [XmlAttribute("qtyp")]
        [MaxLength(1000)]
        public string? QtypXmlAttribute {
            get; set;
        }
    }
}