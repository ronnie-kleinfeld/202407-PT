using Comtec.DL.Repository.Base;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class RctBkgXmlElement : BaseEntityIdModel {
        [XmlElement("r")]
        public List<RXmlElement> RXmlElementList {
            get; set;
        }
    }
}