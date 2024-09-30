using Comtec.AS400XML.Attributes;
using Comtec.AS400XML.Output.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Model.Screen {
    public class CXmlElement : BaseAllowedValuesValidationModel {
        [XmlAttribute("fk"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FkXmlAttribute {
            get; set;
        }

        // �����         ,                  ללמ,                 םויס,                בושיח,            הטמ ףודפד,            ךשמה     ,           הלעמ ףודפד,          הארוה תחיתפ,          הטמ ףדפד,          הלעמ ףדפד,          םיפיעס תלבט,         הרימש אלל אצ,        אבה ךסמל ךשמה,        יאלקחב ךישמהל,       הטמ ףדפד,      םדוק חטובמ     ,     אבה ףד        ,     ךשמה      ,    Enter   ,    הטמ תוסילופ ,    הלעמ ףדפד  ,    הריחב   ,    םדוק ףד        ,   תואלבטב הדשה תרדגה, <-         , <--   , <---    , <-------    , ->      , ------->    , Back                , Breaks       , Down   , ENTER, Enter - ךשמה   , Enter-Search, ENTER-ךשמה, Exit          , Exit w/o Save       , F1  =אבה ךסמ             , F1  =אבה רדלופ           , F1  =היבג יונש           , F1  =טוריפ/זוכיר         , F1  =  , האיצי         , הארוה תחיתפ         , הדובע םויס    , הטילק          , הטמ ףדפ, הטמ ףדפד, הטמ ףודפד , הטמ ףודפד     |, הטמל          , הכישמ      , הלעמ ףדפד   , הלעמ ףודפד , הלעמ ףודפד    |, הלעמ ףןדפד          , הלעמ תוסילופ , הלעמל         , הסילופה םויס   , העצהל אלמ ךמסמ יוניש    , הקידב          , הקידב -ENTER   , הקתעה       , הרזח           , הרימש אלל האיצי    , ךשמה        , ךשמה-ENTER    , םדוק רדלופ          , םויס      , םינש חווט      , םיפיעס תלבט         , ןוכדע אלל אצ    , ןוניס           , ןונער      , ןכוס יוניש    , עוצב          , עוריא ךותיח   , עוריא תחיתפ   , רוזח           , רותיא         , רחב           , שדח         , שדח רותיא         , שופיח         , שופיח-Enter   , תויצפוא    , תועיבת.ש       , תמדוק הסילופ   
        [XmlAttribute("ft"), MaxLength(1000)] // , AS400XmlAllowedValues]
        public string? FtXmlAttribute {
            get; set;
        }

        // 00005, 00006, 00007, 00008, 00009, 00010, 00011, 00012, 00013, 00014, 00015, 00016, 00017, 00018, 00019, 00020, 00024, 00025
        [XmlAttribute("len"), MaxLength(1000)] // , AS400XmlAllowedValues]
        public string? LenXmlAttribute {
            get; set;
        }

        [XmlAttribute("cgr"), MaxLength(1000), AS400XmlAllowedValues]
        public string? CgrXmlAttribute {
            get; set;
        }

        [XmlAttribute("fbua"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FbuaXmlAttribute {
            get; set;
        }

        [XmlAttribute("pic"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PicXmlAttribute {
            get; set;
        }

        [XmlAttribute("psz"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PszXmlAttribute {
            get; set;
        }

        [XmlAttribute("pnt"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PntXmlAttribute {
            get; set;
        }

        [XmlAttribute("pbg"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PbgXmlAttribute {
            get; set;
        }
    }
}