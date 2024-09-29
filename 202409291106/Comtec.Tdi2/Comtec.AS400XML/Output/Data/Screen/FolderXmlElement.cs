using Comtec.DL.Repository.Base;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class FolderXmlElement : BaseEntityIdModel {
        [XmlElement("l")]
        public List<LXmlElement> LXmlElementList {
            get; set;
        }
    }
}