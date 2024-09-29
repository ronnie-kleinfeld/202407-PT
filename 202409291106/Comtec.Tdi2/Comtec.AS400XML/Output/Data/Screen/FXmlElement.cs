using Comtec.DL.Repository.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class FXmlElement : BaseEntityIdModel {
        [XmlAttribute("bkg")]
        [MaxLength(1000)]
        public string? BkgXmlAttribute {
            get; set;
        }
        [XmlAttribute("num")]
        [MaxLength(1000)]
        public string? NumXmlAttribute {
            get; set;
        }

        [XmlAttribute("lin")]
        [MaxLength(1000)]
        public string? LinXmlAttribute {
            get; set;
        }

        [XmlAttribute("col")]
        [MaxLength(1000)]
        public string? ColXmlAttribute {
            get; set;
        }

        [XmlAttribute("typ")]
        [MaxLength(1000)]
        public string? TypXmlAttribute {
            get; set;
        }

        [XmlAttribute("len")]
        [MaxLength(1000)]
        public string? LenXmlAttribute {
            get; set;
        }

        [XmlAttribute("phi")]
        [MaxLength(1000)]
        public string? PhiXmlAttribute {
            get; set;
        }

        [XmlAttribute("val")]
        [MaxLength(1000)]
        public string? ValXmlAttribute {
            get; set;
        }

        [XmlAttribute("inp")]
        [MaxLength(1000)]
        public string? InpXmlAttribute {
            get; set;
        }

        [XmlAttribute("pcl")]
        [MaxLength(1000)]
        public string? PclXmlAttribute {
            get; set;
        }

        [XmlAttribute("rtp")]
        [MaxLength(1000)]
        public string? RtpXmlAttribute {
            get; set;
        }

        [XmlAttribute("alias")]
        [MaxLength(1000)]
        public string? AliasXmlAttribute {
            get; set;
        }

        [XmlAttribute("wd")]
        [MaxLength(1000)]
        public string? WdXmlAttribute {
            get; set;
        }

        [XmlAttribute("sgn")]
        [MaxLength(1000)]
        public string? SgnXmlAttribute {
            get; set;
        }

        [XmlAttribute("edt")]
        [MaxLength(1000)]
        public string? EdtXmlAttribute {
            get; set;
        }

        [XmlAttribute("ind")]
        [MaxLength(1000)]
        public string? IndXmlAttribute {
            get; set;
        }

        [XmlAttribute("pf4")]
        [MaxLength(1000)]
        public string? Pf4XmlAttribute {
            get; set;
        }

        [XmlAttribute("tab")]
        [MaxLength(1000)]
        public string? TabXmlAttribute {
            get; set;
        }

        [XmlElement("b")]
        public List<BXmlElement> BXmlElementList {
            get; set;
        }

        [XmlAttribute("psz")]
        [MaxLength(1000)]
        public string? PszXmlAttribute {
            get; set;
        }

        [XmlAttribute("per")]
        [MaxLength(1000)]
        public string? PerXmlAttribute {
            get; set;
        }

        [XmlAttribute("pht")]
        [MaxLength(1000)]
        public string? PhtXmlAttribute {
            get; set;
        }

        [XmlAttribute("prl")]
        [MaxLength(1000)]
        public string? PrlXmlAttribute {
            get; set;
        }

        [XmlAttribute("apr")]
        [MaxLength(1000)]
        public string? AprXmlAttribute {
            get; set;
        }

        [XmlAttribute("pxk")]
        [MaxLength(1000)]
        public string? PxkXmlAttribute {
            get; set;
        }

        [XmlAttribute("pxn")]
        [MaxLength(1000)]
        public string? PxnXmlAttribute {
            get; set;
        }

        [XmlAttribute("pxm")]
        [MaxLength(1000)]
        public string? PxmXmlAttribute {
            get; set;
        }

        [XmlAttribute("psl")]
        [MaxLength(1000)]
        public string? PslXmlAttribute {
            get; set;
        }

        [XmlAttribute("pch")]
        [MaxLength(1000)]
        public string? PchXmlAttribute {
            get; set;
        }

        [XmlAttribute("ptp")]
        [MaxLength(1000)]
        public string? PtpXmlAttribute {
            get; set;
        }

        [XmlAttribute("dfs")]
        [MaxLength(1000)]
        public string? DfsXmlAttribute {
            get; set;
        }

        [XmlAttribute("pxr")]
        [MaxLength(1000)]
        public string? PxrXmlAttribute {
            get; set;
        }

        [XmlAttribute("pfn")]
        [MaxLength(1000)]
        public string? PfnXmlAttribute {
            get; set;
        }

        [XmlAttribute("pbg")]
        [MaxLength(1000)]
        public string? PbgXmlAttribute {
            get; set;
        }

        [XmlAttribute("pul")]
        [MaxLength(1000)]
        public string? PulXmlAttribute {
            get; set;
        }

        [XmlAttribute("ewr")]
        [MaxLength(1000)]
        public string? EwrXmlAttribute {
            get; set;
        }

        [XmlAttribute("pwd")]
        [MaxLength(1000)]
        public string? PwdXmlAttribute {
            get; set;
        }

        [XmlAttribute("lvl")]
        [MaxLength(1000)]
        public string? LvlXmlAttribute {
            get; set;
        }

        [XmlAttribute("cry")]
        [MaxLength(1000)]
        public string? CryXmlAttribute {
            get; set;
        }

        [XmlAttribute("qfk")]
        [MaxLength(1000)]
        public string? QfkXmlAttribute {
            get; set;
        }

        [XmlAttribute("plc")]
        [MaxLength(1000)]
        public string? PlcXmlAttribute {
            get; set;
        }

        [XmlAttribute("heb")]
        [MaxLength(1000)]
        public string? HebXmlAttribute {
            get; set;
        }

        [XmlAttribute("pic")]
        [MaxLength(1000)]
        public string? PicXmlAttribute {
            get; set;
        }

        [XmlAttribute("pnt")]
        [MaxLength(1000)]
        public string? PntXmlAttribute {
            get; set;
        }

        [XmlAttribute("chb")]
        [MaxLength(1000)]
        public string? ChbXmlAttribute {
            get; set;
        }

        [XmlAttribute("dec")]
        [MaxLength(1000)]
        public string? DecXmlAttribute {
            get; set;
        }

        [XmlAttribute("hky")]
        [MaxLength(1000)]
        public string? HkyXmlAttribute {
            get; set;
        }

        [XmlAttribute("tip")]
        [MaxLength(1000)]
        public string? TipXmlAttribute {
            get; set;
        }

        [XmlAttribute("man")]
        [MaxLength(1000)]
        public string? ManXmlAttribute {
            get; set;
        }

        [XmlAttribute("min")]
        [MaxLength(1000)]
        public string? MinXmlAttribute {
            get; set;
        }

        [XmlAttribute("max")]
        [MaxLength(1000)]
        public string? MaxXmlAttribute {
            get; set;
        }

        [XmlAttribute("numv")]
        [MaxLength(1000)]
        public string? NumvXmlAttribute {
            get; set;
        }

        [XmlAttribute("pchl")]
        [MaxLength(1000)]
        public string? PchlXmlAttribute {
            get; set;
        }

        [XmlAttribute("only")]
        [MaxLength(1000)]
        public string? OnlyXmlAttribute {
            get; set;
        }

        [XmlAttribute("eml")]
        [MaxLength(1000)]
        public string? EmlXmlAttribute {
            get; set;
        }

        [XmlAttribute("lnk")]
        [MaxLength(1000)]
        public string? LnkXmlAttribute {
            get; set;
        }

        [XmlAttribute("bua")]
        [MaxLength(1000)]
        public string? BuaXmlAttribute {
            get; set;
        }

        [XmlAttribute("f4p")]
        [MaxLength(1000)]
        public string? F4PXmlAttribute {
            get; set;
        }

        [XmlAttribute("tch")]
        [MaxLength(1000)]
        public string? TchXmlAttribute {
            get; set;
        }

        [XmlAttribute("pkv")]
        [MaxLength(1000)]
        public string? PkvXmlAttribute {
            get; set;
        }
    }
}