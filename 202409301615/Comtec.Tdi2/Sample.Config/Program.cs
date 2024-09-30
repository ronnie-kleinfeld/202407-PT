using Comtec.BE.Config;
using Comtec.BE.LogEx;
using System.Reflection;

namespace Sample.Config {
    internal class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello, World!");

            LogHelper.Init(AppConfigHandler.Data.Log);

            LogHelper.WriteInformation(MethodBase.GetCurrentMethod(), $"This is the {AppConfigHandler.Data.ApplicationName} application");
        }
    }
}