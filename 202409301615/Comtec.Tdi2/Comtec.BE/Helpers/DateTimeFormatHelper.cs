using System.Globalization;

namespace Comtec.BE.Helpers {
    public static class DateTimeFormatHelper {
        // methods
        public static DateTime? ToDateTimeFormat(string str, string format) {
            if (str == null) {
                return null;
            } else {
                return DateTime.ParseExact(s: str, format: format, provider: CultureInfo.GetCultureInfo("en-US"));
            }
        }

        public static string ToDateTimeFormatString(DateTime? dt, string format) {
            if (dt == null) {
                return string.Empty;
            } else {
                return ((DateTime)dt).ToString(format);
            }
        }

        /// <summary>
        /// 14:09:55
        /// </summary>
        public static string ToHhmmss(DateTime? dt) {
            return dt?.ToLongTimeString();
        }

        /// <summary>
        // 19/02/2018 14:09:55
        /// </summary>
        public static string ToDDMMYYYY_HHMMSS(DateTime? dt) {
            return dt?.ToString();
        }

        /// <summary>
        /// יום שני 19 פברואר 2018
        /// </summary>
        public static string ToLongDate(DateTime? dt) {
            return dt?.ToLongDateString();
        }

        /// <summary>
        /// 19/02/2018
        /// </summary>
        public static string ToDDmmyyyy(DateTime? dt) {
            return dt?.ToShortDateString();
        }

        /// <summary>
        /// 14:09
        /// </summary>
        public static string ToHhmm(DateTime? dt) {
            return dt?.ToShortTimeString();
        }

        /// <summary>
        /// 19/02/2018 12:09:55
        /// </summary>
        public static string ToUniversalTime(DateTime? dt) {
            return dt?.ToUniversalTime().ToString();
        }

        public static string ToYYYYMMDD_HHMMSS(DateTime dt) {
            return ((DateTime)dt).ToString(Consts.YyyymmddHhmmssFormat);
        }
        public static DateTime FromYYYYMMDD_HHMMSS(string str) {
            return DateTime.ParseExact(str, Consts.YyyymmddHhmmssFormat, CultureInfo.GetCultureInfo("en-US"));
        }
    }
}
