using Comtec.AS400XML.Output.Model.Screen;
using Comtec.DL.Model;
using Newtonsoft.Json;

namespace Comtec.AS400XML.Output.Model {
    public class AS400XmlOutputModel : BaseEntityIdModel {
        public ScreenOutputXmlRoot? ScreenXmlRoot {
            get; set;
        }

        public string JsonZip => JsonConvert.SerializeObject(ScreenXmlRoot, Formatting.None);
        public string JsonIndented => JsonConvert.SerializeObject(ScreenXmlRoot, Formatting.Indented);
    }
}