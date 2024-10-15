namespace Comtec.BE.Helpers {
    public static class NumberFormatHelper {
        // int
        /// <summary>
        /// 12,345
        /// </summary>
        public static string ToString(int i) {
            return i.ToString("N0");
        }
        /// <summary>
        /// $ 12,345
        /// </summary>
        public static string ToCurrency(int i) {
            return i.ToString("C0");
        }
        /// <summary>
        /// 12,345%
        /// </summary>
        public static string ToPercent(int i) {
            return i.ToString("P0");
        }
        /// <summary>
        /// FF3F
        /// </summary>
        public static string ToHex(int i) {
            return i.ToString("X0");
        }

        // long
        /// <summary>
        /// 12,345,678,901,234,567
        /// </summary>
        public static string ToString(long l) {
            return l.ToString("N0");
        }
        /// <summary>
        /// $ 12,345,678,901,234,567
        /// </summary>
        public static string ToCurrency(long l) {
            return l.ToString("C0");
        }
        /// <summary>
        /// 12,345,678,901,234,567 %
        /// </summary>
        public static string ToPercent(long l) {
            return l.ToString("P0");
        }
        /// <summary>
        /// FF3FFF3FFF3F
        /// </summary>
        public static string ToHex(long l) {
            return l.ToString("X0");
        }

        // double
        /// <summary>
        /// 12,345.67
        /// </summary>
        public static string ToString(double d, int decimalDigits = 2) {
            return d.ToString($"N{decimalDigits}");
        }
        /// <summary>
        /// $ 12,345.67
        /// </summary>
        public static string ToCurrency(double d, int decimalDigits = 2) {
            return d.ToString($"C{decimalDigits}");
        }
        /// <summary>
        /// 12,345.67 %
        /// </summary>
        public static string ToPercent(double d, int decimalDigits = 2) {
            return d.ToString($"P{decimalDigits}");
        }

        // float
        /// <summary>
        /// 12,345.67
        /// </summary>
        public static string ToString(float f, int decimalDigits = 2) {
            return f.ToString($"N{decimalDigits}");
        }
        /// <summary>
        /// $ 12,345.67
        /// </summary>
        public static string ToCurrency(float f, int decimalDigits = 2) {
            return f.ToString($"C{decimalDigits}");
        }
        /// <summary>
        /// 12,345.67 %
        /// </summary>
        public static string ToPercent(float f, int decimalDigits = 2) {
            return f.ToString($"P{decimalDigits}");
        }

        // decimal
        /// <summary>
        /// 12,345.67
        /// </summary>
        public static string ToString(decimal d, int decimalDigits = 2) {
            return d.ToString($"N{decimalDigits}");
        }
        /// <summary>
        /// $ 12,345.67
        /// </summary>
        public static string ToCurrency(decimal d, int decimalDigits = 2) {
            return d.ToString($"C{decimalDigits}");
        }
        /// <summary>
        /// 12,345.67 %
        /// </summary>
        public static string ToPercent(decimal d, int decimalDigits = 2) {
            return d.ToString($"P{decimalDigits}");
        }
    }
}