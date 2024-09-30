using Comtec.AS400XML.Attributes;
using Comtec.AS400XML.Output.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Model.Screen {
    public class ZXmlElement : BaseAllowedValuesValidationModel {
        [XmlAttribute("zln"), MaxLength(1000), AS400XmlAllowedValues]
        public string? ZlnXmlAttribute {
            get; set;
        }

        [XmlAttribute("zcl"), MaxLength(1000), AS400XmlAllowedValues]
        public string? ZclXmlAttribute {
            get; set;
        }
    }
}