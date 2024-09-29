using Comtec.DL.Repository.Base;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class CmdXmlElement : BaseEntityIdModel {
        [XmlElement("c")]
        public List<CXmlElement> CXmlElementList {
            get; set;
        }
    }
}