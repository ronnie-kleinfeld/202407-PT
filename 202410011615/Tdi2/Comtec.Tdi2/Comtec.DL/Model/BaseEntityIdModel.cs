using System.Xml.Serialization;

namespace Comtec.DL.Model {
    public abstract class BaseEntityIdModel : BaseEntityModel {
        [XmlIgnore]
        public int Id {
            get; set;
        }
    }
}