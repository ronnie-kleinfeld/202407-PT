using Comtec.AS400XML.Attributes;
using Comtec.AS400XML.Output.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Model.Screen {
    public class SXmlElement : BaseAllowedValuesValidationModel {
        [XmlAttribute("adr"), MaxLength(1000), AS400XmlAllowedValues]
        public string? AdrXmlAttribute {
            get; set;
        }
        [XmlAttribute("arw"), MaxLength(1000), AS400XmlAllowedValues]
        public string? ArwXmlAttribute {
            get; set;
        }
        [XmlAttribute("cli"), MaxLength(1000), AS400XmlAllowedValues]
        public string? CliXmlAttribute {
            get; set;
        }
        [XmlAttribute("dtk"), MaxLength(1000), AS400XmlAllowedValues]
        public string? DtkXmlAttribute {
            get; set;
        }
        [XmlAttribute("dtr"), MaxLength(1000), AS400XmlAllowedValues]
        public string? DtrXmlAttribute {
            get; set;
        }
        [XmlAttribute("dspa"), MaxLength(1000), AS400XmlAllowedValues]
        public string? DspaXmlAttribute {
            get; set;
        }
        [XmlAttribute("fch"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FchXmlAttribute {
            get; set;
        }
        [XmlAttribute("fcmd"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FcmdXmlAttribute {
            get; set;
        }
        [XmlAttribute("fdsp"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FdspXmlAttribute {
            get; set;
        }
        [XmlAttribute("fgr"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FgrXmlAttribute {
            get; set;
        }
        [XmlAttribute("fil"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FilXmlAttribute {
            get; set;
        }
        [XmlAttribute("find"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FindXmlAttribute {
            get; set;
        }
        [XmlAttribute("flang"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FlangXmlAttribute {
            get; set;
        }
        [XmlAttribute("fld"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FldXmlAttribute {
            get; set;
        }
        [XmlAttribute("flib"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FlibXmlAttribute {
            get; set;
        }
        [XmlAttribute("flp"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FlpXmlAttribute {
            get; set;
        }
        [XmlAttribute("flr"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FlrXmlAttribute {
            get; set;
        }
        [XmlAttribute("fnn"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FnnXmlAttribute {
            get; set;
        }
        [XmlAttribute("fort"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FortXmlAttribute {
            get; set;
        }
        [XmlAttribute("fpc"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FpcXmlAttribute {
            get; set;
        }
        [XmlAttribute("fps"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FpsXmlAttribute {
            get; set;
        }
        [XmlAttribute("fsid"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FsidXmlAttribute {
            get; set;
        }
        [XmlAttribute("grk"), MaxLength(1000), AS400XmlAllowedValues]
        public string? GrkXmlAttribute {
            get; set;
        }
        [XmlAttribute("jacket"), MaxLength(1000), AS400XmlAllowedValues]
        public string? JacketXmlAttribute {
            get; set;
        }
        [XmlAttribute("msg"), MaxLength(1000), AS400XmlAllowedValues]
        public string? MsgXmlAttribute {
            get; set;
        }
        /// <summary>
        /// Not used
        /// </summary>
        [XmlAttribute("pic"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PicXmlAttribute {
            get; set;
        }
        [XmlAttribute("plgc"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PlgcXmlAttribute {
            get; set;
        }
        /// <summary>
        /// Not used
        /// </summary>
        [XmlAttribute("pnt"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PntXmlAttribute {
            get; set;
        }
        [XmlAttribute("pset"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PsetXmlAttribute {
            get; set;
        }
        /// <summary>
        /// Not used
        /// </summary>
        [XmlAttribute("pxr"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PxrXmlAttribute {
            get; set;
        }
        [XmlAttribute("qend"), MaxLength(1000), AS400XmlAllowedValues]
        public string? QendXmlAttribute {
            get; set;
        }
        [XmlAttribute("qflx"), MaxLength(1000), AS400XmlAllowedValues]
        public string? QflxXmlAttribute {
            get; set;
        }
        [XmlAttribute("qpxl"), MaxLength(1000), AS400XmlAllowedValues]
        public string? QpxlXmlAttribute {
            get; set;
        }
        [XmlAttribute("qstr"), MaxLength(1000), AS400XmlAllowedValues]
        public string? QstrXmlAttribute {
            get; set;
        }
        [XmlAttribute("rec"), MaxLength(1000), AS400XmlAllowedValues]
        public string? RecXmlAttribute {
            get; set;
        }
        [XmlAttribute("spo"), MaxLength(1000), AS400XmlAllowedValues]
        public string? SpoXmlAttribute {
            get; set;
        }
        [XmlAttribute("srg"), MaxLength(1000), AS400XmlAllowedValues]
        public string? SrgXmlAttribute {
            get; set;
        }
        [XmlAttribute("strc"), MaxLength(1000), AS400XmlAllowedValues]
        public string? StrcXmlAttribute {
            get; set;
        }
        [XmlAttribute("ver"), MaxLength(1000), AS400XmlAllowedValues]
        public string? VerXmlAttribute {
            get; set;
        }
        [XmlAttribute("wait"), MaxLength(1000), AS400XmlAllowedValues]
        public string? WaitXmlAttribute {
            get; set;
        }
        [XmlAttribute("wcol"), MaxLength(1000), AS400XmlAllowedValues]
        public string? WcolXmlAttribute {
            get; set;
        }
        [XmlAttribute("wcst"), MaxLength(1000), AS400XmlAllowedValues]
        public string? WcstXmlAttribute {
            get; set;
        }
        [XmlAttribute("win"), MaxLength(1000), AS400XmlAllowedValues]
        public string? WinXmlAttribute {
            get; set;
        }
        [XmlAttribute("wlin"), MaxLength(1000), AS400XmlAllowedValues]
        public string? WlinXmlAttribute {
            get; set;
        }
        [XmlAttribute("wlst"), MaxLength(1000), AS400XmlAllowedValues]
        public string? WlstXmlAttribute {
            get; set;
        }
        [XmlAttribute("wsml"), MaxLength(1000), AS400XmlAllowedValues]
        public string? WsmlXmlAttribute {
            get; set;
        }
    }
}