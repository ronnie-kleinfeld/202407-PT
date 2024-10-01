using Comtec.AS400XML.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Input.Model.Screen {
    public class FInputXmlElement {
        [XmlAttribute("num"), MaxLength(1000), AS400XmlAllowedValues]
        public string? NumXmlAttribute {
            get; set;
        }
        [XmlAttribute("ind"), MaxLength(1000), AS400XmlAllowedValues]
        public string? IndXmlAttribute {
            get; set;
        }
        [XmlAttribute("sgn"), MaxLength(1000), AS400XmlAllowedValues]
        public string? SgnXmlAttribute {
            get; set;
        }
        [XmlAttribute("val")] // , AS400XmlAllowedValues]
        public string? ValXmlAttribute {
            get; set;
        }

        [XmlAttribute("alias"), MaxLength(1000)] // , AS400XmlAllowedValues]
        public string? AliasXmlAttribute {
            get; set;
        }
        [XmlAttribute("apr"), MaxLength(1000), AS400XmlAllowedValues]
        public string? AprXmlAttribute {
            get; set;
        }
        [XmlAttribute("bkg"), MaxLength(1000), AS400XmlAllowedValues]
        public string? BkgXmlAttribute {
            get; set;
        }
        [XmlAttribute("bua"), MaxLength(1000), AS400XmlAllowedValues]
        public string? BuaXmlAttribute {
            get; set;
        }
        [XmlAttribute("chb"), MaxLength(1000), AS400XmlAllowedValues]
        public string? ChbXmlAttribute {
            get; set;
        }
        [XmlAttribute("col"), MaxLength(1000)] // , AS400XmlAllowedValues]
        public string? ColXmlAttribute {
            get; set;
        }
        [XmlAttribute("cry"), MaxLength(1000), AS400XmlAllowedValues]
        public string? CryXmlAttribute {
            get; set;
        }
        [XmlAttribute("dec"), MaxLength(1000), AS400XmlAllowedValues]
        public string? DecXmlAttribute {
            get; set;
        }
        [XmlAttribute("dfs"), MaxLength(1000), AS400XmlAllowedValues]
        public string? DfsXmlAttribute {
            get; set;
        }
        [XmlAttribute("edt"), MaxLength(1000), AS400XmlAllowedValues]
        public string? EdtXmlAttribute {
            get; set;
        }
        [XmlAttribute("eml"), MaxLength(1000), AS400XmlAllowedValues]
        public string? EmlXmlAttribute {
            get; set;
        }
        [XmlAttribute("ewr"), MaxLength(1000), AS400XmlAllowedValues]
        public string? EwrXmlAttribute {
            get; set;
        }
        [XmlAttribute("f4p"), MaxLength(1000), AS400XmlAllowedValues]
        public string? F4PXmlAttribute {
            get; set;
        }
        [XmlAttribute("heb"), MaxLength(1000), AS400XmlAllowedValues]
        public string? HebXmlAttribute {
            get; set;
        }
        [XmlAttribute("hky"), MaxLength(1000)] // , AS400XmlAllowedValues]
        public string? HkyXmlAttribute {
            get; set;
        }
        [XmlAttribute("inp"), MaxLength(1000), AS400XmlAllowedValues]
        public string? InpXmlAttribute {
            get; set;
        }
        [XmlAttribute("len"), MaxLength(1000), AS400XmlAllowedValues]
        public string? LenXmlAttribute {
            get; set;
        }
        [XmlAttribute("lin"), MaxLength(1000)] // , AS400XmlAllowedValues]
        public string? LinXmlAttribute {
            get; set;
        }
        [XmlAttribute("lnk"), MaxLength(1000), AS400XmlAllowedValues]
        public string? LnkXmlAttribute {
            get; set;
        }
        [XmlAttribute("lvl"), MaxLength(1000), AS400XmlAllowedValues]
        public string? LvlXmlAttribute {
            get; set;
        }
        [XmlAttribute("man"), MaxLength(1000), AS400XmlAllowedValues]
        public string? ManXmlAttribute {
            get; set;
        }
        [XmlAttribute("max"), MaxLength(1000), AS400XmlAllowedValues]
        public string? MaxXmlAttribute {
            get; set;
        }
        [XmlAttribute("min"), MaxLength(1000), AS400XmlAllowedValues]
        public string? MinXmlAttribute {
            get; set;
        }
        [XmlAttribute("numv"), MaxLength(1000), AS400XmlAllowedValues]
        public string? NumvXmlAttribute {
            get; set;
        }
        [XmlAttribute("only"), MaxLength(1000), AS400XmlAllowedValues]
        public string? OnlyXmlAttribute {
            get; set;
        }
        [XmlAttribute("pbg"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PbgXmlAttribute {
            get; set;
        }
        [XmlAttribute("pch"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PchXmlAttribute {
            get; set;
        }
        [XmlAttribute("pchl"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PchlXmlAttribute {
            get; set;
        }
        [XmlAttribute("pcl"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PclXmlAttribute {
            get; set;
        }
        [XmlAttribute("per"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PerXmlAttribute {
            get; set;
        }
        [XmlAttribute("pf4"), MaxLength(1000), AS400XmlAllowedValues]
        public string? Pf4XmlAttribute {
            get; set;
        }
        [XmlAttribute("pfn"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PfnXmlAttribute {
            get; set;
        }
        [XmlAttribute("phi"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PhiXmlAttribute {
            get; set;
        }
        [XmlAttribute("pht"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PhtXmlAttribute {
            get; set;
        }
        [XmlAttribute("pic"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PicXmlAttribute {
            get; set;
        }
        [XmlAttribute("pkv"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PkvXmlAttribute {
            get; set;
        }
        [XmlAttribute("plc"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PlcXmlAttribute {
            get; set;
        }
        [XmlAttribute("pnt"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PntXmlAttribute {
            get; set;
        }
        [XmlAttribute("prl"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PrlXmlAttribute {
            get; set;
        }
        [XmlAttribute("psl"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PslXmlAttribute {
            get; set;
        }
        [XmlAttribute("psz"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PszXmlAttribute {
            get; set;
        }
        [XmlAttribute("ptp"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PtpXmlAttribute {
            get; set;
        }
        [XmlAttribute("pul"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PulXmlAttribute {
            get; set;
        }
        [XmlAttribute("pwd"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PwdXmlAttribute {
            get; set;
        }
        [XmlAttribute("pxk"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PxkXmlAttribute {
            get; set;
        }
        [XmlAttribute("pxm"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PxmXmlAttribute {
            get; set;
        }
        [XmlAttribute("pxn"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PxnXmlAttribute {
            get; set;
        }
        [XmlAttribute("pxr"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PxrXmlAttribute {
            get; set;
        }
        [XmlAttribute("qfk"), MaxLength(1000), AS400XmlAllowedValues]
        public string? QfkXmlAttribute {
            get; set;
        }
        [XmlAttribute("rtp"), MaxLength(1000), AS400XmlAllowedValues]
        public string? RtpXmlAttribute {
            get; set;
        }
        [XmlAttribute("tab"), MaxLength(1000)] // , AS400XmlAllowedValues]
        public string? TabXmlAttribute {
            get; set;
        }
        [XmlAttribute("tch"), MaxLength(1000), AS400XmlAllowedValues]
        public string? TchXmlAttribute {
            get; set;
        }
        [XmlAttribute("tip"), MaxLength(1000), AS400XmlAllowedValues]
        public string? TipXmlAttribute {
            get; set;
        }
        [XmlAttribute("typ"), MaxLength(1000), AS400XmlAllowedValues]
        public string? TypXmlAttribute {
            get; set;
        }
        [XmlAttribute("wd"), MaxLength(1000), AS400XmlAllowedValues]
        public string? WdXmlAttribute {
            get; set;
        }

        public FInputXmlElement() {
        }
        public FInputXmlElement(string numXmlAttribute, string indXmlAttribute, string sgnXmlAttribute, string valXmlAttribute) {
            NumXmlAttribute = numXmlAttribute;
            IndXmlAttribute = indXmlAttribute;
            SgnXmlAttribute = sgnXmlAttribute;
            ValXmlAttribute = valXmlAttribute;
        }
    }
}