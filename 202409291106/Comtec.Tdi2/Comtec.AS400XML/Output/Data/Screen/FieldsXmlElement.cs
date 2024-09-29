using Comtec.DL.Repository.Base;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class FieldsXmlElement : BaseEntityIdModel {
        [XmlElement("f")]
        public List<FXmlElement> FXmlElementList {
            get; set;
        }
    }
}