using Comtec.AS400XML.Attributes;
using Comtec.AS400XML.Output.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Model.Screen {
    public class DXmlElement : BaseAllowedValuesValidationModel {
        [XmlAttribute("qtyp"), MaxLength(1000), AS400XmlAllowedValues]
        public string? QtypXmlAttribute {
            get; set;
        }
    }
}