using Comtec.DL.Repository.Base;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class YZonesBkgXmlElement : BaseEntityIdModel {
        [XmlElement("y")]
        public List<YXmlElement> YXmlElementList {
            get; set;
        }
    }
}