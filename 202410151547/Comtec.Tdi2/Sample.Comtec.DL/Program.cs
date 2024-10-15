using Comtec.BE.Config;
using Comtec.BE.LogEx;

namespace Sample.Comtec.DL {
    internal class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello, World!");

            AppConfigHandler.Init();
            LogHelper.Init(AppConfigHandler.Data.Log);
        }
    }
}