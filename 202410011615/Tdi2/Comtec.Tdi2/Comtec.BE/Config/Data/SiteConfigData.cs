namespace Comtec.BE.Config.Data {
    public class SiteConfigData {
        public string ClientName {
            get; set;
        }
        public string SiteId {
            get; set;
        }
        public string AssetsDirectory {
            get; set;
        }

        public SiteConfigData() {
            ClientName = "Vanilla";
            SiteId = "Vanilla";
            AssetsDirectory = "Vanilla";
        }
    }
}