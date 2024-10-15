using Comtec.AS400XML.Output.Model.Base;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Model.Screen {
    [XmlRoot("screen")]
    public class ScreenOutputXmlRoot : BaseAllowedValuesValidationModel {
        [XmlElement("bgr")]
        public BgrXmlElement? BgrXmlElement {
            get; set;
        }
        [XmlElement("cmd")]
        public CmdXmlElement? CmdXmlElement {
            get; set;
        }
        [XmlElement("fields")]
        public FieldsXmlElement? FieldsXmlElement {
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
        [XmlElement("rct")]
        public RctXmlElement? RctXmlElement {
            get; set;
        }
        [XmlElement("rct_bkg")]
        public RctBkgXmlElement? RctBkgXmlElement {
            get; set;
        }
        [XmlElement("s")]
        public SXmlElement? SXmlElement {
            get; set;
        }
        [XmlElement("xzones")]
        public XZonesXmlElement? XZonesXmlElement {
            get; set;
        }
        [XmlElement("xzones_bkg")]
        public XZonesBkgXmlElement? XZonesBkgXmlElement {
            get; set;
        }
        [XmlElement("yzones")]
        public YZonesXmlElement? YZonesXmlElement {
            get; set;
        }
        [XmlElement("yzones_bkg")]
        public YZonesBkgXmlElement? YZonesBkgXmlElement {
            get; set;
        }

        public new bool IsValidXmlElement() {
            return (SXmlElement?.IsValidXmlElement() ?? true) &&
                   (CmdXmlElement?.IsValidXmlElement() ?? true) &&
                   (FolderXmlElement?.IsValidXmlElement() ?? true) &&
                   (FolderDetailsXmlElement?.IsValidXmlElement() ?? true) &&
                   (BgrXmlElement?.IsValidXmlElement() ?? true) &&
                   (RctXmlElement?.IsValidXmlElement() ?? true) &&
                   (RctBkgXmlElement?.IsValidXmlElement() ?? true) &&
                   (FieldsXmlElement?.IsValidXmlElement() ?? true) &&
                   (YZonesXmlElement?.IsValidXmlElement() ?? true) &&
                   (XZonesXmlElement?.IsValidXmlElement() ?? true) &&
                   (YZonesBkgXmlElement?.IsValidXmlElement() ?? true) &&
                   (XZonesBkgXmlElement?.IsValidXmlElement() ?? true);
        }
    }
}