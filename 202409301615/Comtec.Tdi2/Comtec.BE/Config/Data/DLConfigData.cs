namespace Comtec.BE.Config.Data {
    public class DLConfigData {
        public string ConnectionString {
            get; set;
        }

        public DLConfigData() {
#if DEBUG
            ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AS400XMLDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
#else
            ConnectionString = "";
#endif
        }
    }
}