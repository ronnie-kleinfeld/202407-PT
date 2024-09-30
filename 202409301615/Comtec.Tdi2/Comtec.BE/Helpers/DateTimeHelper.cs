using Comtec.BE.Exceptions;
using System.Reflection;

namespace Comtec.BE.Helpers {
    public class DateTimeHelper {
        public static DateTime FirstDayOfYear(DateTime dt) {
            return new DateTime(dt.Year, 1, 1, 0, 0, 0);
        }
        public static DateTime LastDayOfYear(DateTime dt) {
            return new DateTime(dt.Year, 12, 31, 0, 0, 0);
        }
        public static DateTime FirstDayOfMonth(DateTime dt) {
            return new DateTime(dt.Year, dt.Month, 1, 0, 0, 0);
        }
        public static DateTime LastDayOfMonth(DateTime dt) {
            return FirstDayOfMonth(dt).AddMonths(1).AddDays(-1);
        }
        public static DateTime BeginOfQuarter(DateTime dt) {
            switch (dt.Month) {
                case 1:
                case 2:
                case 3:
                    return new DateTime(dt.Year, 1, 1, 0, 0, 0);
                case 4:
                case 5:
                case 6:
                    return new DateTime(dt.Year, 4, 1, 0, 0, 0);
                case 7:
                case 8:
                case 9:
                    return new DateTime(dt.Year, 7, 1, 0, 0, 0);
                case 10:
                case 11:
                case 12:
                    return new DateTime(dt.Year, 10, 1, 0, 0, 0);
                default:
                    throw new SwitchOutOfRangeException(MethodBase.GetCurrentMethod(), dt.Month);
            }
        }
        public static DateTime EndOfQuarter(DateTime dt) {
            switch (dt.Month) {
                case 1:
                case 2:
                case 3:
                    return new DateTime(dt.Year, 3, 31, 0, 0, 0);
                case 4:
                case 5:
                case 6:
                    return new DateTime(dt.Year, 6, 30, 0, 0, 0);
                case 7:
                case 8:
                case 9:
                    return new DateTime(dt.Year, 9, 30, 0, 0, 0);
                case 10:
                case 11:
                case 12:
                    return new DateTime(dt.Year, 12, 31, 0, 0, 0);
                default:
                    throw new SwitchOutOfRangeException(MethodBase.GetCurrentMethod(), dt.Month);
            }
        }
    }
}