using Comtec.DL.Repository.Base;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class BgrXmlElement : BaseEntityIdModel {
        [XmlElement("z")]
        public List<ZXmlElement> ZXmlElementList {
            get; set;
        }
    }
}