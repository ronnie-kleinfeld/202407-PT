namespace Comtec.BE.Config.Data {
    public class AS400XmlConfigData {
        public bool ValidateAllowedData {
            get; set;
        }

        public bool WriteToDatabase {
            get; set;
        }

        public string ConnectionString {
            get; set;
        }

        public AS400XmlConfigData() {
            ValidateAllowedData = false;
            WriteToDatabase = true;
            ConnectionString = "Data Source=test-tis;Initial Catalog=AS400XmlFileDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        }
    }
}