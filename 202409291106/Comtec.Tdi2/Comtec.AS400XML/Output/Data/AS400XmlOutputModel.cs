using Comtec.AS400XML.Output.Data.Screen;
using Comtec.DL.Repository.Base;
using Newtonsoft.Json;

namespace Comtec.AS400XML.Output.Data {
    public class AS400XmlOutputModel : BaseEntityIdModel {
        public ScreenXmlRoot? ScreenXmlRoot {
            get; set;
        }

        public string JsonZip => JsonConvert.SerializeObject(ScreenXmlRoot, Formatting.None);
        public string JsonIndented => JsonConvert.SerializeObject(ScreenXmlRoot, Formatting.Indented);
    }
}