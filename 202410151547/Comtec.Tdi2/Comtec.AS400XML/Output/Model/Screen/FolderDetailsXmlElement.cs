using Comtec.AS400XML.Output.Model.Base;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Model.Screen {
    public class FolderDetailsXmlElement : BaseAllowedValuesValidationModel {
        [XmlElement("d")]
        public List<DXmlElement>? DXmlElementList {
            get; set;
        }

        public new bool IsValidXmlElement() {
            return IsValidXmlElement(DXmlElementList);
        }
    }
}