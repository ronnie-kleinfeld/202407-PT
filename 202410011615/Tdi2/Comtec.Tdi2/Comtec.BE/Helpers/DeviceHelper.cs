namespace Comtec.BE.Helpers {
    public class DeviceHelper {
        public static string UserName => $"{Environment.UserDomainName}_{Environment.UserName}";
        public static string MachineName => Environment.MachineName;
        public static string OsVersion => Environment.OSVersion.ToString();
        public static string NewLine => Environment.NewLine;

        // methods
        public static void Exit(int exitCode) {
            Environment.Exit(exitCode);
        }
    }
}