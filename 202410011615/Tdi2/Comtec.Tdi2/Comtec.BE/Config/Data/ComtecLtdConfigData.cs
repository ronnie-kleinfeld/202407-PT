namespace Comtec.BE.Config.Data {
    public class ComtecLtdConfigData {
        public string Name {
            get; set;
        }
        public string WebAddress {
            get; set;
        }

        public ComtecLtdConfigData() {
            Name = "Comtec Global";
            WebAddress = "http://www.comtecglobal.com";
        }
    }
}