using Comtec.DL.Repository.Base;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class FolderDetailsXmlElement : BaseEntityIdModel {
        [XmlElement("d")]
        public List<DXmlElement> DXmlElementList {
            get; set;
        }
    }
}