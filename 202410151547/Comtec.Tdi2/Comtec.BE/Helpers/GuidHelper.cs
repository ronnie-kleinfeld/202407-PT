namespace Comtec.BE.Helpers {
    public class GuidHelper {
        public static string NewGuid => Guid.NewGuid().ToString("N");
    }
}