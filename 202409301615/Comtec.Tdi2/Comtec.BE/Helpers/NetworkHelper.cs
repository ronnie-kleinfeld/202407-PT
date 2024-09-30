using System.Diagnostics;

namespace Comtec.BE.Helpers {
    public static class NetworkHelper {
        public static void OpenLink(string url) {
            Process.Start(url);
        }
    }
}