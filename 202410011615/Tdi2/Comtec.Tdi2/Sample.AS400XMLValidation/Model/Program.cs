using Newtonsoft.Json;

namespace Sample.AS400XMLValidation.Model {
    internal class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello, World!");

            ScreenXmlRoot screenXmlRoot = new ScreenXmlRoot();
            string json = JsonConvert.SerializeObject(screenXmlRoot, Formatting.Indented);
        }
    }
}