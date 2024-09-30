using Comtec.AS400XML.Attributes;
using Comtec.AS400XML.Output.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Model.Screen {
    public class SXmlElement : BaseAllowedValuesValidationModel {
        [XmlAttribute("fil"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FilXmlAttribute {
            get; set;
        }

        [XmlAttribute("rec"), MaxLength(1000), AS400XmlAllowedValues]
        public string? RecXmlAttribute {
            get; set;
        }

        [XmlAttribute("win"), MaxLength(1000), AS400XmlAllowedValues]
        public string? WinXmlAttribute {
            get; set;
        }

        [XmlAttribute("fpc"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FpcXmlAttribute {
            get; set;
        }

        [XmlAttribute("flib"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FlibXmlAttribute {
            get; set;
        }

        [XmlAttribute("cli"), MaxLength(1000), AS400XmlAllowedValues]
        public string? CliXmlAttribute {
            get; set;
        }

        [XmlAttribute("fsid"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FsidXmlAttribute {
            get; set;
        }

        [XmlAttribute("ver"), MaxLength(1000), AS400XmlAllowedValues]
        public string? VerXmlAttribute {
            get; set;
        }

        //                           ,             1             ,            1              ,            11   1    1    ,      11    1            11,     1111   11    11 11 1  ,    1                      ,    1       1              ,    1       1    1 1  1    ,    1       11     1111 1  ,    11      1         1    ,    111     11     1111 1  ,    111     11   1    1    ,    111     11   1 1111 1  ,   1                       ,   1        1              ,   1   1    1              ,   1   1    1      11    11,   1   1   11      11    11,   1   1 1111      11    11,   1   11               111,   1   11 1 1      11    1 ,   1   11 111      11    1 ,   1   111               11,   1  1     1            11,   1  1 1                11,   1  11    1    1 11    11,   1  111 1 1      11      ,   1  111 1 1      11    1 ,   1 1    1 1      11  1 11,   1 11     1 1         1  ,   1 11   1 1      11  1 11,   1 11  1  1 1    1     11,   11       1              ,   11       1            11,   11    1     11        11,   11    1  1              ,   11  11   11   11     1  ,   11  1111 1      11      ,   11  1111 1      11    1 ,   11  111111      11    1 ,   11 1  1  1            11,   11 11    1     1      11,   11 11   11      11    11,   11 11 1  1            11,   1111     11     1 11 1  ,   111111   1     1     1  ,  1                        ,  1         1              ,  1      11   11   11      ,  1      11   11   11    11,  1      11  111    1   111,  1      11 1 1    11      ,  1   1     1            11,  1   1  11   11   11      ,  1   1  11   11   11    11,  1   1  11  111    1   111,  1   1  11 111     1   111,  1   1  11 111    11    11,  1 1 ,  1 1       1              ,  1 1       1            11,  1 1       1          1   ,  1 1       1        1     ,  1 11      1              ,  1 111     11             ,  1 111     11     1 11 1  ,  1 111     111    1 11 1  ,  1 11111   1     11 11 1  ,  1 11111   11    1     1  ,  1 111111111 11  111 111  ,  1 111111111 1111111 111  ,  11                       ,  11                     11,  11        1              ,  11     1  1 1  1         ,  11     11  111     1  111,  11     11  111     11 111,  11     11 111      11 111,  11   1111 1 1   1     111,  11  1     11      1    11,  11  1  1  111         111,  11  1  11  111     11 111,  11  111 1 1              ,  11  11111   11  1     111,  11  11111  111  1     111,  11 1    1 1            11,  11 1    111            11,  11 1 11 111            11,  11 11     1      11    11,  11 11     11     1  1 1  ,  111                      ,  111       1              ,  111       1 11  1        ,  111    1  1 11  1        ,  111   11 11  1      111  ,  111   11 111 1      111  ,  111  11         1        ,  111  11   1     1        ,  111 1     11     11    11,  1111      1           1  ,  1111   1 11111           ,  1111 1   11 1  1111    11,  1111 1  111 1  1111    11,  11111  1  1            11,  1111111         1        ,  1111111   1     1        ,  11111111  1   1  11 11 11,  11111111  11     11 11 11,  11111111 111    111  1 11,  11111111 111 1  111 11 11,  111111111111 1  111 11 11, 1          1              , 1          11     1  1    , 1          11     1  1 1  , 1          11     1 11 1  , 1     11   1    11        , 1   11     1    1         , 1   11     1    1 1  1    , 1   1111   1    11        , 1   1111   1    11     1  , 1   1111   11    11  1 1  , 1  1                   1  , 1  1       1      1111 1  , 1  1       11     1  1    , 1  1       11     1  1 1  , 1  1       11     1 11 1  , 1  1  11   1    11      1 , 1  1  11   11    11  1 1  , 1  1  11   11    11 11 1  , 1  1  11   11   11     1  , 1  1  11   11   111  1    , 1  111     1    1 1       , 1  111     1    1 1111 1  , 1  111     11     1  1 1  , 1  111     11   1         , 1  111     11   1 1       , 1  111     11   1 1  1 1  , 1  11111   1    11      1 , 1  11111   11    11  1 1  , 1  11111   11   11      1 , 1  11111   11   11     1  , 1  11111   11   111 11    , 1 1        11   1 1111 1  , 1 1   1  1 1      11    11, 1 1  11  1 1      11    11, 1 1 1 , 1 1 11     1    1 1  1    , 1 1 11     11   1 1111 1  , 1 1 1111   11    11 11 1  , 1 11  11   11   11     1  , 1 11  11   111   11111 1  , 1 11  111  1  1111 1  1 11, 1 111    111 1111 1    111, 1 111    111 111111    111, 1 1111     1    1    1    , 1 1111     1    1 1  1    , 1 111111   1     11  1 1  , 1 111111   11    11 11 1  , 1 111111   11   11     1  , 11                        , 11  11     11     11 1 1  , 11 1            1      1  , 11 1 11         1      1  , 11 11111   1 1   1     1  , 111  11    11   1 11    11, 111 11 11  11     11  1 11, 111 1111   11    11 11 1  , 1111  1    1    1         , 1111 11111 111        1 11, 11111      1              , 11111      1           1  , 11111    1 1              , 11111    1 1111111        , 11111    1111111111       , 11111 1  111 1   111    11, 11111111   1      1111 111, 11111111   11    11111 1  , 111111111111 1111111 111  , 11111111111111111111111  
        [XmlAttribute("fnn"), MaxLength(1000)] // , AS400XmlAllowedValues]
        public string? FnnXmlAttribute {
            get; set;
        }

        [XmlAttribute("adr"), MaxLength(1000), AS400XmlAllowedValues]
        public string? AdrXmlAttribute {
            get; set;
        }

        [XmlAttribute("wsml"), MaxLength(1000), AS400XmlAllowedValues]
        public string? WsmlXmlAttribute {
            get; set;
        }

        [XmlAttribute("wlst"), MaxLength(1000), AS400XmlAllowedValues]
        public string? WlstXmlAttribute {
            get; set;
        }

        [XmlAttribute("wlin"), MaxLength(1000), AS400XmlAllowedValues]
        public string? WlinXmlAttribute {
            get; set;
        }

        [XmlAttribute("wcst"), MaxLength(1000), AS400XmlAllowedValues]
        public string? WcstXmlAttribute {
            get; set;
        }

        [XmlAttribute("wcol"), MaxLength(1000), AS400XmlAllowedValues]
        public string? WcolXmlAttribute {
            get; set;
        }

        [XmlAttribute("strc"), MaxLength(1000), AS400XmlAllowedValues]
        public string? StrcXmlAttribute {
            get; set;
        }

        [XmlAttribute("fgr"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FgrXmlAttribute {
            get; set;
        }

        [XmlAttribute("fch"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FchXmlAttribute {
            get; set;
        }

        [XmlAttribute("pxr"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PxrXmlAttribute {
            get; set;
        }

        [XmlAttribute("srg"), MaxLength(1000), AS400XmlAllowedValues]
        public string? SrgXmlAttribute {
            get; set;
        }

        [XmlAttribute("flang"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FlangXmlAttribute {
            get; set;
        }

        [XmlAttribute("flr"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FlrXmlAttribute {
            get; set;
        }

        [XmlAttribute("qpxl"), MaxLength(1000), AS400XmlAllowedValues]
        public string? QpxlXmlAttribute {
            get; set;
        }

        [XmlAttribute("qflx"), MaxLength(1000), AS400XmlAllowedValues]
        public string? QflxXmlAttribute {
            get; set;
        }

        [XmlAttribute("qstr"), MaxLength(1000), AS400XmlAllowedValues]
        public string? QstrXmlAttribute {
            get; set;
        }

        [XmlAttribute("qend"), MaxLength(1000), AS400XmlAllowedValues]
        public string? QendXmlAttribute {
            get; set;
        }

        // 00000000, 20140501, 20160101, 20170601, 20180719, 20180801, 20190101, 20200101, 20210101, 20210602, 20210701, 20210803, 20210901, 20210902, 20211201, 20211216, 20220101, 20220106, 20220123, 20220131, 20220207, 20220228, 20220301, 20220309, 20220314, 20220406, 20220426, 20220427, 20220614, 20220912, 20221102, 20230101, 20230218, 20230226, 20230329, 20230419, 20230420, 20230423, 20230424, 20230427, 20230501, 20230514, 20230515, 20230516, 20230517, 20230518, 20230521, 20230601, 20230611, 20230612, 20230620, 20230626, 20230627, 20230701, 20230706, 20230709, 20230801, 20230913, 20231001, 20231026, 20231211, 20231220, 20240101, 20240104, 20240116, 20240128, 20240201, 20240207, 20240214, 20240225, 20240228, 20240229, 20240301, 20240310, 20240313, 20240319, 20240325, 20240326, 20240328, 20240401, 20240402, 20240407, 20240411, 20240501, 20240508, 20240519, 20240521, 20240526, 20240527, 20240528, 20240529, 20240617, 20240630, 20240701, 20240708, 20240711, 20240716, 20240718, 20240731, 20240807, 20240808, 20240812, 20240903, 20240904, 20240905, 20240915, 20240923
        [XmlAttribute("dtk"), MaxLength(1000)] // , AS400XmlAllowedValues]
        public string? DtkXmlAttribute {
            get; set;
        }

        // 00000000, 20140501, 20160101, 20170601, 20180719, 20180801, 20190101, 20200101, 20210101, 20210602, 20210701, 20210803, 20210901, 20210902, 20211201, 20211216, 20220101, 20220106, 20220123, 20220131, 20220207, 20220228, 20220301, 20220309, 20220314, 20220406, 20220426, 20220427, 20220614, 20220912, 20221102, 20230101, 20230218, 20230226, 20230329, 20230419, 20230420, 20230423, 20230424, 20230427, 20230501, 20230514, 20230515, 20230516, 20230517, 20230518, 20230521, 20230601, 20230611, 20230612, 20230620, 20230626, 20230627, 20230701, 20230706, 20230709, 20230801, 20230913, 20231001, 20231026, 20231211, 20231220, 20240101, 20240104, 20240116, 20240128, 20240201, 20240207, 20240214, 20240225, 20240228, 20240229, 20240301, 20240310, 20240313, 20240319, 20240325, 20240326, 20240328, 20240401, 20240402, 20240407, 20240411, 20240501, 20240508, 20240519, 20240521, 20240526, 20240527, 20240528, 20240529, 20240617, 20240630, 20240701, 20240708, 20240711, 20240716, 20240718, 20240731, 20240807, 20240808, 20240812, 20240903, 20240904, 20240905, 20240915, 20240923
        [XmlAttribute("dtr"), MaxLength(1000)] // , AS400XmlAllowedValues]
        public string? DtrXmlAttribute {
            get; set;
        }

        [XmlAttribute("pic"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PicXmlAttribute {
            get; set;
        }

        [XmlAttribute("pnt"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PntXmlAttribute {
            get; set;
        }

        [XmlAttribute("fdsp"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FdspXmlAttribute {
            get; set;
        }

        [XmlAttribute("dspa"), MaxLength(1000), AS400XmlAllowedValues]
        public string? DspaXmlAttribute {
            get; set;
        }

        [XmlAttribute("fort"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FortXmlAttribute {
            get; set;
        }

        [XmlAttribute("arw"), MaxLength(1000), AS400XmlAllowedValues]
        public string? ArwXmlAttribute {
            get; set;
        }

        [XmlAttribute("flp"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FlpXmlAttribute {
            get; set;
        }

        [XmlAttribute("msg"), MaxLength(1000), AS400XmlAllowedValues]
        public string? MsgXmlAttribute {
            get; set;
        }

        [XmlAttribute("fps"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FpsXmlAttribute {
            get; set;
        }

        [XmlAttribute("spo"), MaxLength(1000), AS400XmlAllowedValues]
        public string? SpoXmlAttribute {
            get; set;
        }

        [XmlAttribute("jacket"), MaxLength(1000), AS400XmlAllowedValues]
        public string? JacketXmlAttribute {
            get; set;
        }

        [XmlAttribute("wait"), MaxLength(1000), AS400XmlAllowedValues]
        public string? WaitXmlAttribute {
            get; set;
        }

        [XmlAttribute("pset"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PsetXmlAttribute {
            get; set;
        }

        [XmlAttribute("plgc"), MaxLength(1000), AS400XmlAllowedValues]
        public string? PlgcXmlAttribute {
            get; set;
        }

        [XmlAttribute("grk"), MaxLength(1000), AS400XmlAllowedValues]
        public string? GrkXmlAttribute {
            get; set;
        }

        [XmlAttribute("fld"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FldXmlAttribute {
            get; set;
        }

        [XmlAttribute("find"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FindXmlAttribute {
            get; set;
        }

        [XmlAttribute("fcmd"), MaxLength(1000), AS400XmlAllowedValues]
        public string? FcmdXmlAttribute {
            get; set;
        }
    }
}