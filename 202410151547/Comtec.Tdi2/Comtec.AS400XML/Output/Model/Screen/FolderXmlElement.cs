using Comtec.AS400XML.Output.Model.Base;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Model.Screen {
    public class FolderXmlElement : BaseAllowedValuesValidationModel {
        [XmlElement("l")]
        public List<LXmlElement>? LXmlElementList {
            get; set;
        }

        public new bool IsValidXmlElement() {
            return IsValidXmlElement(LXmlElementList);
        }
    }
}