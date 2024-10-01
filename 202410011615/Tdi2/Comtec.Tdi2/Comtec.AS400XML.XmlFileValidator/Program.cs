using Comtec.BE.Config;
using Comtec.BE.LogEx;

namespace Comtec.AS400XML.XmlFileValidator {
    internal static class Program {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            AppConfigHandler.Init();
            LogHelper.Init(AppConfigHandler.Data.Log);

            Application.Run(new MainForm());
        }
    }
}