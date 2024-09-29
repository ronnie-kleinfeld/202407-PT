using Comtec.DL.Repository.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    public class SXmlElement : BaseEntityIdModel {
        /// <summary>
        /// שם קובץ לתצוגה, הוחזר מרשת ללא שינוי
        ///	SXmlElement	.	FilXmlAttribute	 Use DCBLDF    
        ///	SXmlElement	.	FilXmlAttribute	 Use DCBTS1F   
        ///	SXmlElement	.	FilXmlAttribute	 Use DCCHGF    
        ///	SXmlElement	.	FilXmlAttribute	 Use DCDSP00F  
        ///	SXmlElement	.	FilXmlAttribute	 Use DCDSP01F  
        ///	SXmlElement	.	FilXmlAttribute	 Use DCDSP05F  
        ///	SXmlElement	.	FilXmlAttribute	 Use DCDSP21F  
        ///	SXmlElement	.	FilXmlAttribute	 Use DCDSP61F  
        ///	SXmlElement	.	FilXmlAttribute	 Use DCDSP70F  
        ///	SXmlElement	.	FilXmlAttribute	 Use DCDSPKPDF 
        ///	SXmlElement	.	FilXmlAttribute	 Use DCDSPKPUF 
        ///	SXmlElement	.	FilXmlAttribute	 Use DCHFKNF   
        ///	SXmlElement	.	FilXmlAttribute	 Use DCHZRF    
        ///	SXmlElement	.	FilXmlAttribute	 Use DCISURSCF 
        ///	SXmlElement	.	FilXmlAttribute	 Use DCLKANF   
        ///	SXmlElement	.	FilXmlAttribute	 Use DCLKCOMF  
        ///	SXmlElement	.	FilXmlAttribute	 Use DCLKHF    
        ///	SXmlElement	.	FilXmlAttribute	 Use DCORAAF   
        ///	SXmlElement	.	FilXmlAttribute	 Use DCORAF    
        ///	SXmlElement	.	FilXmlAttribute	 Use DCSOCBTDF 
        ///	SXmlElement	.	FilXmlAttribute	 Use DCSPRSF   
        ///	SXmlElement	.	FilXmlAttribute	 Use DCSYTRF   
        ///	SXmlElement	.	FilXmlAttribute	 Use DCTOTKPF  
        ///	SXmlElement	.	FilXmlAttribute	 Use FNHSKLKFM 
        ///	SXmlElement	.	FilXmlAttribute	 Use FNISRMVFM 
        ///	SXmlElement	.	FilXmlAttribute	 Use HFANAF6FM 
        ///	SXmlElement	.	FilXmlAttribute	 Use HFANFFM   
        ///	SXmlElement	.	FilXmlAttribute	 Use HFAS03FFM 
        ///	SXmlElement	.	FilXmlAttribute	 Use HFAS03FM  
        ///	SXmlElement	.	FilXmlAttribute	 Use HFAS04AF  
        ///	SXmlElement	.	FilXmlAttribute	 Use HFAS04FM  
        ///	SXmlElement	.	FilXmlAttribute	 Use HFAS05FM  
        ///	SXmlElement	.	FilXmlAttribute	 Use HFAS16FM  
        ///	SXmlElement	.	FilXmlAttribute	 Use HFAS19FM  
        ///	SXmlElement	.	FilXmlAttribute	 Use HFAS47FM  
        ///	SXmlElement	.	FilXmlAttribute	 Use HFASYRFM  
        ///	SXmlElement	.	FilXmlAttribute	 Use HFFLNAV2FM
        ///	SXmlElement	.	FilXmlAttribute	 Use HFFLNAVFM 
        ///	SXmlElement	.	FilXmlAttribute	 Use HFKRNSCR  
        ///	SXmlElement	.	FilXmlAttribute	 Use HFLKLSFFM 
        ///	SXmlElement	.	FilXmlAttribute	 Use HFMELELFM 
        ///	SXmlElement	.	FilXmlAttribute	 Use HFMHLFM   
        ///	SXmlElement	.	FilXmlAttribute	 Use HFPEILUTFM
        ///	SXmlElement	.	FilXmlAttribute	 Use HFPRHHITFM
        ///	SXmlElement	.	FilXmlAttribute	 Use HFPSENDSFM
        ///	SXmlElement	.	FilXmlAttribute	 Use HFRKZMFM  
        ///	SXmlElement	.	FilXmlAttribute	 Use HFRKZPFFM 
        ///	SXmlElement	.	FilXmlAttribute	 Use HFRKZPFM  
        ///	SXmlElement	.	FilXmlAttribute	 Use HFSCREEN  
        ///	SXmlElement	.	FilXmlAttribute	 Use HFSHWPSKFM
        ///	SXmlElement	.	FilXmlAttribute	 Use HFTIPHIDFM
        ///	SXmlElement	.	FilXmlAttribute	 Use MENUCLSFM 
        ///	SXmlElement	.	FilXmlAttribute	 Use MENUDDSFM 
        ///	SXmlElement	.	FilXmlAttribute	 Use NPMNGNFM  
        ///	SXmlElement	.	FilXmlAttribute	 Use PRLAK13FM 
        ///	SXmlElement	.	FilXmlAttribute	 Use RDMAVFM   
        ///	SXmlElement	.	FilXmlAttribute	 Use STRIMAGEFM
        ///	SXmlElement	.	FilXmlAttribute	 Use TBL006FM  
        ///	SXmlElement	.	FilXmlAttribute	 Use TBL0107FM 
        ///	SXmlElement	.	FilXmlAttribute	 Use TBL010FM  
        ///	SXmlElement	.	FilXmlAttribute	 Use TBL034FM  
        ///	SXmlElement	.	FilXmlAttribute	 Use TBL050FM  
        ///	SXmlElement	.	FilXmlAttribute	 Use TBL146FM  
        ///	SXmlElement	.	FilXmlAttribute	 Use TBL298FM  
        ///	SXmlElement	.	FilXmlAttribute	 Use TBLMAVFM  
        ///	SXmlElement	.	FilXmlAttribute	 Use TBLSOCFM  
        ///	SXmlElement	.	FilXmlAttribute	 Use TBSHFM    
        ///	SXmlElement	.	FilXmlAttribute	 Use TVC200FM  
        ///	SXmlElement	.	FilXmlAttribute	 Use TVH201FM  
        ///	SXmlElement	.	FilXmlAttribute	 Use TVH209FM  
        ///	SXmlElement	.	FilXmlAttribute	 Use TVH218FM  
        ///	SXmlElement	.	FilXmlAttribute	 Use TVH231FM  
        ///	SXmlElement	.	FilXmlAttribute	 Use TVKLTRHMFM
        ///	SXmlElement	.	FilXmlAttribute	 Use TVMOKTFM  
        ///	SXmlElement	.	FilXmlAttribute	 Use TVSCREEN  
        ///	SXmlElement	.	FilXmlAttribute	 Use UNOVRFM   
        ///	SXmlElement	.	FilXmlAttribute	 Use UNREMMFM  
        ///	SXmlElement	.	FilXmlAttribute	 Use UNSCRERRFM
        ///	SXmlElement	.	FilXmlAttribute	 Use UNSNDERR  
        ///	SXmlElement	.	FilXmlAttribute	 Use UNSNDSPL
        /// </summary>
        [XmlAttribute("fil")]
        [MaxLength(1000)]
        public string? FilXmlAttribute {
            get; set;
        }

        /// <summary>
        /// שם רשומה לתצוגה, הוחזה מרשת ללא שינוי
        ///	SXmlElement	.	RecXmlAttribute	 Use DETAIL    
        ///	SXmlElement	.	RecXmlAttribute	 Use DETAIL1   
        ///	SXmlElement	.	RecXmlAttribute	 Use DETAL2    
        ///	SXmlElement	.	RecXmlAttribute	 Use DTAIL     
        ///	SXmlElement	.	RecXmlAttribute	 Use DTAIL1    
        ///	SXmlElement	.	RecXmlAttribute	 Use DTAIL2    
        ///	SXmlElement	.	RecXmlAttribute	 Use DTAIL2S   
        ///	SXmlElement	.	RecXmlAttribute	 Use DTAIL3S   
        ///	SXmlElement	.	RecXmlAttribute	 Use DTAILP    
        ///	SXmlElement	.	RecXmlAttribute	 Use DTL7      
        ///	SXmlElement	.	RecXmlAttribute	 Use DTLSHRT05 
        ///	SXmlElement	.	RecXmlAttribute	 Use DTLTOT    
        ///	SXmlElement	.	RecXmlAttribute	 Use ELPRTFIL  
        ///	SXmlElement	.	RecXmlAttribute	 Use ERRSPL    
        ///	SXmlElement	.	RecXmlAttribute	 Use ESLPRTFIL 
        ///	SXmlElement	.	RecXmlAttribute	 Use GENERAL_DF
        ///	SXmlElement	.	RecXmlAttribute	 Use GENERAL_PF
        ///	SXmlElement	.	RecXmlAttribute	 Use KLTASH    
        ///	SXmlElement	.	RecXmlAttribute	 Use KLTASHX   
        ///	SXmlElement	.	RecXmlAttribute	 Use KLTKSF    
        ///	SXmlElement	.	RecXmlAttribute	 Use KLTPOLH   
        ///	SXmlElement	.	RecXmlAttribute	 Use KLTRHMM1  
        ///	SXmlElement	.	RecXmlAttribute	 Use KLTRHMM3  
        ///	SXmlElement	.	RecXmlAttribute	 Use KLTTASOP  
        ///	SXmlElement	.	RecXmlAttribute	 Use KLTTV1P   
        ///	SXmlElement	.	RecXmlAttribute	 Use KLTTVI    
        ///	SXmlElement	.	RecXmlAttribute	 Use MKZRAMIT  
        ///	SXmlElement	.	RecXmlAttribute	 Use MKZRSRBT  
        ///	SXmlElement	.	RecXmlAttribute	 Use MOUT      
        ///	SXmlElement	.	RecXmlAttribute	 Use MSCHITBID 
        ///	SXmlElement	.	RecXmlAttribute	 Use PEANFK6   
        ///	SXmlElement	.	RecXmlAttribute	 Use PEANFT6   
        ///	SXmlElement	.	RecXmlAttribute	 Use PEANFT6F  
        ///	SXmlElement	.	RecXmlAttribute	 Use PEEXITDC  
        ///	SXmlElement	.	RecXmlAttribute	 Use PEEXITKR  
        ///	SXmlElement	.	RecXmlAttribute	 Use PEHAZ01   
        ///	SXmlElement	.	RecXmlAttribute	 Use PEHAZ01TDI
        ///	SXmlElement	.	RecXmlAttribute	 Use PEHAZ03   
        ///	SXmlElement	.	RecXmlAttribute	 Use PEHRIG1   
        ///	SXmlElement	.	RecXmlAttribute	 Use PEKOT0    
        ///	SXmlElement	.	RecXmlAttribute	 Use PEKOT0FN  
        ///	SXmlElement	.	RecXmlAttribute	 Use PEKOT1    
        ///	SXmlElement	.	RecXmlAttribute	 Use PEMAINFN  
        ///	SXmlElement	.	RecXmlAttribute	 Use PEMAINK   
        ///	SXmlElement	.	RecXmlAttribute	 Use PEMAINKRN 
        ///	SXmlElement	.	RecXmlAttribute	 Use PEMAINSFN 
        ///	SXmlElement	.	RecXmlAttribute	 Use PEMENU    
        ///	SXmlElement	.	RecXmlAttribute	 Use PEMISHFS  
        ///	SXmlElement	.	RecXmlAttribute	 Use PEOPT     
        ///	SXmlElement	.	RecXmlAttribute	 Use PEPRNTHL  
        ///	SXmlElement	.	RecXmlAttribute	 Use PEQRYAW   
        ///	SXmlElement	.	RecXmlAttribute	 Use PETOKFHZ  
        ///	SXmlElement	.	RecXmlAttribute	 Use PRTNHG    
        ///	SXmlElement	.	RecXmlAttribute	 Use RCD5      
        ///	SXmlElement	.	RecXmlAttribute	 Use REC01     
        ///	SXmlElement	.	RecXmlAttribute	 Use REC1      
        ///	SXmlElement	.	RecXmlAttribute	 Use RECCUT    
        ///	SXmlElement	.	RecXmlAttribute	 Use RECKEY    
        ///	SXmlElement	.	RecXmlAttribute	 Use REXIT     
        ///	SXmlElement	.	RecXmlAttribute	 Use RISURMVTH 
        ///	SXmlElement	.	RecXmlAttribute	 Use RLIST     
        ///	SXmlElement	.	RecXmlAttribute	 Use RLIST1    
        ///	SXmlElement	.	RecXmlAttribute	 Use RLIST2    
        ///	SXmlElement	.	RecXmlAttribute	 Use RLIST5    
        ///	SXmlElement	.	RecXmlAttribute	 Use RLIST6    
        ///	SXmlElement	.	RecXmlAttribute	 Use RLIST7    
        ///	SXmlElement	.	RecXmlAttribute	 Use RLISTDDS  
        ///	SXmlElement	.	RecXmlAttribute	 Use RLISTMG   
        ///	SXmlElement	.	RecXmlAttribute	 Use ROPTION   
        ///	SXmlElement	.	RecXmlAttribute	 Use RPAGE     
        ///	SXmlElement	.	RecXmlAttribute	 Use RSTVIOT2  
        ///	SXmlElement	.	RecXmlAttribute	 Use SCR       
        ///	SXmlElement	.	RecXmlAttribute	 Use SHLLMUNM  
        ///	SXmlElement	.	RecXmlAttribute	 Use SHLLST    
        ///	SXmlElement	.	RecXmlAttribute	 Use SHLLSTL   
        ///	SXmlElement	.	RecXmlAttribute	 Use SHLLSTR   
        ///	SXmlElement	.	RecXmlAttribute	 Use SLAVE     
        ///	SXmlElement	.	RecXmlAttribute	 Use TAFRIT    
        ///	SXmlElement	.	RecXmlAttribute	 Use TITLE0ITUR
        ///	SXmlElement	.	RecXmlAttribute	 Use TKWIN01   
        ///	SXmlElement	.	RecXmlAttribute	 Use TKWINGM   
        ///	SXmlElement	.	RecXmlAttribute	 Use TVC200    
        ///	SXmlElement	.	RecXmlAttribute	 Use TVIA1FN   
        ///	SXmlElement	.	RecXmlAttribute	 Use TVPERUT   
        ///	SXmlElement	.	RecXmlAttribute	 Use WB005     
        ///	SXmlElement	.	RecXmlAttribute	 Use WB018     
        ///	SXmlElement	.	RecXmlAttribute	 Use WB801     
        ///	SXmlElement	.	RecXmlAttribute	 Use WB900     
        ///	SXmlElement	.	RecXmlAttribute	 Use WIND      
        ///	SXmlElement	.	RecXmlAttribute	 Use WIND4     
        ///	SXmlElement	.	RecXmlAttribute	 Use WINDATE   
        ///	SXmlElement	.	RecXmlAttribute	 Use WINDTAIL  
        ///	SXmlElement	.	RecXmlAttribute	 Use WINHSKLK  
        ///	SXmlElement	.	RecXmlAttribute	 Use WINLST2   
        ///	SXmlElement	.	RecXmlAttribute	 Use WINLST6K  
        ///	SXmlElement	.	RecXmlAttribute	 Use WINLSTGM  
        ///	SXmlElement	.	RecXmlAttribute	 Use WINOPTRC  
        ///	SXmlElement	.	RecXmlAttribute	 Use WINREMWEB 
        ///	SXmlElement	.	RecXmlAttribute	 Use WNDRECHOIC
        /// </summary>
        [XmlAttribute("rec")]
        [MaxLength(1000)]
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
        ///	SXmlElement	.	WinXmlAttribute	 Use R
        ///	SXmlElement	.	WinXmlAttribute	 Use S
        /// </summary>
        [XmlAttribute("win")]
        [MaxLength(1000)]
        public string? WinXmlAttribute {
            get; set;
        }

        /// <summary>
        ///	SXmlElement	.	FpcXmlAttribute	 Use 0000
        ///	SXmlElement	.	FpcXmlAttribute	 Use 0001
        ///	SXmlElement	.	FpcXmlAttribute	 Use 0002
        ///	SXmlElement	.	FpcXmlAttribute	 Use 0003
        ///	SXmlElement	.	FpcXmlAttribute	 Use 0004
        ///	SXmlElement	.	FpcXmlAttribute	 Use 0005
        ///	SXmlElement	.	FpcXmlAttribute	 Use 0006
        ///	SXmlElement	.	FpcXmlAttribute	 Use 0007
        ///	SXmlElement	.	FpcXmlAttribute	 Use 0008
        ///	SXmlElement	.	FpcXmlAttribute	 Use 0009
        ///	SXmlElement	.	FpcXmlAttribute	 Use 0010
        ///	SXmlElement	.	FpcXmlAttribute	 Use 0011
        ///	SXmlElement	.	FpcXmlAttribute	 Use 0013
        ///	SXmlElement	.	FpcXmlAttribute	 Use 0014
        ///	SXmlElement	.	FpcXmlAttribute	 Use 0015
        ///	SXmlElement	.	FpcXmlAttribute	 Use 0016
        ///	SXmlElement	.	FpcXmlAttribute	 Use 0023
        ///	SXmlElement	.	FpcXmlAttribute	 Use 0029
        ///	SXmlElement	.	FpcXmlAttribute	 Use 0039
        ///	SXmlElement	.	FpcXmlAttribute	 Use 0048
        ///	SXmlElement	.	FpcXmlAttribute	 Use 0050
        ///	SXmlElement	.	FpcXmlAttribute	 Use 0051
        /// </summary>
        [XmlAttribute("fpc")]
        [MaxLength(1000)]
        public string? FpcXmlAttribute {
            get; set;
        }

        /// <summary>
        ///	SXmlElement	.	FlibXmlAttribute	 Use           
        ///	SXmlElement	.	FlibXmlAttribute	 Use CRM449    
        ///	SXmlElement	.	FlibXmlAttribute	 Use CRM600    
        ///	SXmlElement	.	FlibXmlAttribute	 Use CRM713    
        ///	SXmlElement	.	FlibXmlAttribute	 Use CRMAYALON 
        ///	SXmlElement	.	FlibXmlAttribute	 Use CRMENGTIS 
        ///	SXmlElement	.	FlibXmlAttribute	 Use CRMKPL    
        ///	SXmlElement	.	FlibXmlAttribute	 Use CRMKRNTST 
        ///	SXmlElement	.	FlibXmlAttribute	 Use CRMMENORA 
        ///	SXmlElement	.	FlibXmlAttribute	 Use CRMRMA    
        ///	SXmlElement	.	FlibXmlAttribute	 Use CRMSHO    
        ///	SXmlElement	.	FlibXmlAttribute	 Use CRMSRB    
        ///	SXmlElement	.	FlibXmlAttribute	 Use CRMTMP    
        ///	SXmlElement	.	FlibXmlAttribute	 Use CRMTST1   
        ///	SXmlElement	.	FlibXmlAttribute	 Use CRMTST4   
        ///	SXmlElement	.	FlibXmlAttribute	 Use CRMUNIX   
        ///	SXmlElement	.	FlibXmlAttribute	 Use CRMVNL    
        ///	SXmlElement	.	FlibXmlAttribute	 Use CRMYESHUV 
        /// </summary>
        [XmlAttribute("flib")]
        [MaxLength(1000)]
        public string? FlibXmlAttribute {
            get; set;
        }

        [XmlAttribute("cli")]
        [MaxLength(1000)]
        public string? CliXmlAttribute {
            get; set;
        }

        [XmlAttribute("fsid")]
        [MaxLength(1000)]
        public string? FsidXmlAttribute {
            get; set;
        }

        [XmlAttribute("ver")]
        [MaxLength(1000)]
        public string? VerXmlAttribute {
            get; set;
        }

        [XmlAttribute("fnn")]
        [MaxLength(1000)]
        public string? FnnXmlAttribute {
            get; set;
        }

        [XmlAttribute("adr")]
        [MaxLength(1000)]
        public string? AdrXmlAttribute {
            get; set;
        }

        [XmlAttribute("wsml")]
        [MaxLength(1000)]
        public string? WsmlXmlAttribute {
            get; set;
        }

        [XmlAttribute("wlst")]
        [MaxLength(1000)]
        public string? WlstXmlAttribute {
            get; set;
        }

        [XmlAttribute("wlin")]
        [MaxLength(1000)]
        public string? WlinXmlAttribute {
            get; set;
        }

        [XmlAttribute("wcst")]
        [MaxLength(1000)]
        public string? WcstXmlAttribute {
            get; set;
        }

        [XmlAttribute("wcol")]
        [MaxLength(1000)]
        public string? WcolXmlAttribute {
            get; set;
        }

        [XmlAttribute("strc")]
        [MaxLength(1000)]
        public string? StrcXmlAttribute {
            get; set;
        }

        [XmlAttribute("fgr")]
        [MaxLength(1000)]
        public string? FgrXmlAttribute {
            get; set;
        }

        [XmlAttribute("fch")]
        [MaxLength(1000)]
        public string? FchXmlAttribute {
            get; set;
        }

        [XmlAttribute("pxr")]
        [MaxLength(1000)]
        public string? PxrXmlAttribute {
            get; set;
        }

        [XmlAttribute("srg")]
        [MaxLength(1000)]
        public string? SrgXmlAttribute {
            get; set;
        }

        [XmlAttribute("flang")]
        [MaxLength(1000)]
        public string? FlangXmlAttribute {
            get; set;
        }

        [XmlAttribute("flr")]
        [MaxLength(1000)]
        public string? FlrXmlAttribute {
            get; set;
        }

        [XmlAttribute("qpxl")]
        [MaxLength(1000)]
        public string? QpxlXmlAttribute {
            get; set;
        }

        [XmlAttribute("qflx")]
        [MaxLength(1000)]
        public string? QflxXmlAttribute {
            get; set;
        }

        [XmlAttribute("qstr")]
        [MaxLength(1000)]
        public string? QstrXmlAttribute {
            get; set;
        }

        [XmlAttribute("qend")]
        [MaxLength(1000)]
        public string? QendXmlAttribute {
            get; set;
        }

        [XmlAttribute("dtk")]
        [MaxLength(1000)]
        public string? DtkXmlAttribute {
            get; set;
        }

        [XmlAttribute("dtr")]
        [MaxLength(1000)]
        public string? DtrXmlAttribute {
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

        [XmlAttribute("fdsp")]
        [MaxLength(1000)]
        public string? FdspXmlAttribute {
            get; set;
        }

        [XmlAttribute("dspa")]
        [MaxLength(1000)]
        public string? DspaXmlAttribute {
            get; set;
        }

        [XmlAttribute("fort")]
        [MaxLength(1000)]
        public string? FortXmlAttribute {
            get; set;
        }

        [XmlAttribute("arw")]
        [MaxLength(1000)]
        public string? ArwXmlAttribute {
            get; set;
        }

        [XmlAttribute("flp")]
        [MaxLength(1000)]
        public string? FlpXmlAttribute {
            get; set;
        }

        [XmlAttribute("msg")]
        [MaxLength(1000)]
        public string? MsgXmlAttribute {
            get; set;
        }

        [XmlAttribute("fps")]
        [MaxLength(1000)]
        public string? FpsXmlAttribute {
            get; set;
        }

        [XmlAttribute("spo")]
        [MaxLength(1000)]
        public string? SpoXmlAttribute {
            get; set;
        }

        [XmlAttribute("jacket")]
        [MaxLength(1000)]
        public string? JacketXmlAttribute {
            get; set;
        }

        [XmlAttribute("wait")]
        [MaxLength(1000)]
        public string? WaitXmlAttribute {
            get; set;
        }

        [XmlAttribute("pset")]
        [MaxLength(1000)]
        public string? PsetXmlAttribute {
            get; set;
        }

        [XmlAttribute("plgc")]
        [MaxLength(1000)]
        public string? PlgcXmlAttribute {
            get; set;
        }

        [XmlAttribute("grk")]
        [MaxLength(1000)]
        public string? GrkXmlAttribute {
            get; set;
        }

        [XmlAttribute("fld")]
        [MaxLength(1000)]
        public string? FldXmlAttribute {
            get; set;
        }

        [XmlAttribute("find")]
        [MaxLength(1000)]
        public string? FindXmlAttribute {
            get; set;
        }

        [XmlAttribute("fcmd")]
        [MaxLength(1000)]
        public string? FcmdXmlAttribute {
            get; set;
        }
    }
}