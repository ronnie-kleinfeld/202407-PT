using Comtec.AS400XML.Output.Model.Base;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Model.Screen {
    public class BgrXmlElement : BaseAllowedValuesValidationModel {
        [XmlElement("z")]
        public List<ZXmlElement>? ZXmlElementList {
            get; set;
        }

        public new bool IsValidXmlElement() {
            return IsValidXmlElement(ZXmlElementList);
        }
    }
}