namespace Comtec.BE.Helpers {
    public class SecurityHelper {
        // members
        private static string _numeric = "0123456789";
        private static string _alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private static string _alphaNumeric = _alpha + _numeric;

        private static Random _random = new Random(DateTime.Now.Millisecond);

        // methods
        public static string GenerateNumericPassword(int length) {
            return Generate(_numeric, length);
        }
        public static int GenerateNumericPassword(int minValue, int maxValue) {
            Random random = new Random(DateTime.Now.Millisecond);
            return random.Next(minValue, maxValue);
        }
        public static string GenerateAlphaPassword(int length) {
            return Generate(_alpha, length);
        }
        public static string GenerateAlphaNumericPassword(int length) {
            return Generate(_alphaNumeric, length);
        }

        private static string Generate(string pool, int length) {
            string result = "";

            for (var i = 0; i < length; i++) {
                result += pool.Substring(_random.Next(0, pool.Length), 1);
            }

            return result;
        }
    }
}