using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class SXmlElement {
        /// <summary>
        /// שם קובץ לתצוגה, הוחזר מרשת ללא שינוי
        /// </summary>
        [XmlAttribute("fil")]
        public string? FilXmlAttribute {
            get; set;
        }

        /// <summary>
        /// שם רשומה לתצוגה, הוחזה מרשת ללא שינוי
        /// </summary>
        [XmlAttribute("rec")]
        public string? RecXmlAttribute {
            get; set;
        }

        /// <summary>
        /// סוג מסך
        /// Default: N
        /// Y   מסך בצורת חלון : יש להציג מסך כמו חלון מודלי (חלון מהיר, עובד ללא רענון שדות    INPUT ) -  לא הועבר ערך הזה מתוכנית UNSNDWEB 
        /// S   מסך בצורת חלון : יש להציג מסך כמו חלון מודלי (חלון ישן , איטי)
        /// R   מסך בצורת חלון  :יש להציג מסך כמו חלון רגיל , לצייר מסגרת בקואורדינטות של פרמטרים WLST,WLIN,WCST,WCOL
        /// N   תצוגת מסך רגילה
        /// </summary>
        [XmlAttribute("win")]
        public string? WinXmlAttribute {
            get; set;
        }

        [XmlAttribute("fpc")]
        public string? FpcXmlAttribute {
            get; set;
        }

        [XmlAttribute("flib")]
        public string? FlibXmlAttribute {
            get; set;
        }

        [XmlAttribute("cli")]
        public string? CliXmlAttribute {
            get; set;
        }

        [XmlAttribute("fsid")]
        public string? FsidXmlAttribute {
            get; set;
        }

        [XmlAttribute("ver")]
        public string? VerXmlAttribute {
            get; set;
        }

        [XmlAttribute("fnn")]
        public string? FnnXmlAttribute {
            get; set;
        }

        [XmlAttribute("adr")]
        public string? AdrXmlAttribute {
            get; set;
        }

        [XmlAttribute("wsml")]
        public string? WsmlXmlAttribute {
            get; set;
        }

        [XmlAttribute("wlst")]
        public string? WlstXmlAttribute {
            get; set;
        }

        [XmlAttribute("wlin")]
        public string? WlinXmlAttribute {
            get; set;
        }

        [XmlAttribute("wcst")]
        public string? WcstXmlAttribute {
            get; set;
        }

        [XmlAttribute("wcol")]
        public string? WcolXmlAttribute {
            get; set;
        }

        [XmlAttribute("strc")]
        public string? StrcXmlAttribute {
            get; set;
        }

        [XmlAttribute("fgr")]
        public string? FgrXmlAttribute {
            get; set;
        }

        [XmlAttribute("fch")]
        public string? FchXmlAttribute {
            get; set;
        }

        [XmlAttribute("pxr")]
        public string? PxrXmlAttribute {
            get; set;
        }

        [XmlAttribute("srg")]
        public string? SrgXmlAttribute {
            get; set;
        }

        [XmlAttribute("flang")]
        public string? FlangXmlAttribute {
            get; set;
        }

        [XmlAttribute("flr")]
        public string? FlrXmlAttribute {
            get; set;
        }

        [XmlAttribute("qpxl")]
        public string? QpxlXmlAttribute {
            get; set;
        }

        [XmlAttribute("qflx")]
        public string? QflxXmlAttribute {
            get; set;
        }

        [XmlAttribute("qstr")]
        public string? QstrXmlAttribute {
            get; set;
        }

        [XmlAttribute("qend")]
        public string? QendXmlAttribute {
            get; set;
        }

        [XmlAttribute("dtk")]
        public string? DtkXmlAttribute {
            get; set;
        }

        [XmlAttribute("dtr")]
        public string? DtrXmlAttribute {
            get; set;
        }

        [XmlAttribute("pic")]
        public string? PicXmlAttribute {
            get; set;
        }

        [XmlAttribute("pnt")]
        public string? PntXmlAttribute {
            get; set;
        }

        [XmlAttribute("fdsp")]
        public string? FdspXmlAttribute {
            get; set;
        }

        [XmlAttribute("dspa")]
        public string? DspaXmlAttribute {
            get; set;
        }

        [XmlAttribute("fort")]
        public string? FortXmlAttribute {
            get; set;
        }

        [XmlAttribute("arw")]
        public string? ArwXmlAttribute {
            get; set;
        }

        [XmlAttribute("flp")]
        public string? FlpXmlAttribute {
            get; set;
        }

        [XmlAttribute("msg")]
        public string? MsgXmlAttribute {
            get; set;
        }

        [XmlAttribute("fps")]
        public string? FpsXmlAttribute {
            get; set;
        }

        [XmlAttribute("spo")]
        public string? SpoXmlAttribute {
            get; set;
        }

        [XmlAttribute("jacket")]
        public string? JacketXmlAttribute {
            get; set;
        }

        [XmlAttribute("wait")]
        public string? WaitXmlAttribute {
            get; set;
        }

        [XmlAttribute("pset")]
        public string? PsetXmlAttribute {
            get; set;
        }

        [XmlAttribute("plgc")]
        public string? PlgcXmlAttribute {
            get; set;
        }

        [XmlAttribute("grk")]
        public string? GrkXmlAttribute {
            get; set;
        }
    }
}