using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Data.Screen {
    [XmlRoot("screen")]
    public class ScreenXmlRoot {
        [XmlElement("s")]
        public SXmlElement? SXmlElement {
            get; set;
        }

        [XmlElement("cmd")]
        public CmdXmlElement? CmdXmlElement {
            get; set;
        }

        [XmlElement("folder")]
        public FolderXmlElement? FolderXmlElement {
            get; set;
        }

        [XmlElement("folderdetails")]
        public FolderDetailsXmlElement? FolderDetailsXmlElement {
            get; set;
        }

        [XmlElement("bgr")]
        public BgrXmlElement? BgrXmlElement {
            get; set;
        }

        [XmlElement("rct")]
        public RctXmlElement? RctXmlElement {
            get; set;
        }

        [XmlElement("rct_bkg")]
        public RctBkgXmlElement? RctBkgXmlElement {
            get; set;
        }

        [XmlElement("fields")]
        public FieldsXmlElement? FieldsXmlElement {
            get; set;
        }

        [XmlElement("yzones")]
        public YZonesXmlElement? YZonesXmlElement {
            get; set;
        }

        [XmlElement("xzones")]
        public XZonesXmlElement? XZonesXmlElement {
            get; set;
        }

        [XmlElement("yzones_bkg")]
        public YZonesBkgXmlElement? YZonesBkgXmlElement {
            get; set;
        }

        [XmlElement("xzones_bkg")]
        public XZonesBkgXmlElement? XZonesBkgXmlElement {
            get; set;
        }
    }
}