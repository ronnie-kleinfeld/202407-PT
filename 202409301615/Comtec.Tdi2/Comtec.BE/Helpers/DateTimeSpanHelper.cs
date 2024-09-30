using Comtec.BE.Enum;

namespace Comtec.BE.Helpers {
    public static class DateTimeSpanHelper {
        public static string Span(DateTime dt, DateTimeSpanFormatTypeEnum dateTimeSpanFormatType) {
            return Calculate(DateTime.Now, dt, dateTimeSpanFormatType).Trim();
        }

        private static string Calculate(DateTime dt1, DateTime dt2, DateTimeSpanFormatTypeEnum dateTimeSpanFormatType) {
            TimeSpan interval = dt1 - dt2;

            double years = interval.Days / 365.25;
            double months = interval.Days / (365.25 / 12);
            double days = interval.TotalDays;
            double hours = interval.TotalHours;
            double minutes = interval.TotalMinutes;
            double seconds = interval.TotalSeconds;
            double milliseconds = interval.TotalMilliseconds;

            string str;
            if (dateTimeSpanFormatType >= DateTimeSpanFormatTypeEnum.YYYY && years >= 1) {
                str = Part((int)years, "year");

                return str + " " + Calculate(dt1, dt2.AddYears((int)years), dateTimeSpanFormatType);
            }

            if (dateTimeSpanFormatType >= DateTimeSpanFormatTypeEnum.YYYYMM && months >= 1) {
                str = Part((int)months, "month");

                return str + " " + Calculate(dt1, dt2.AddMonths((int)months), dateTimeSpanFormatType);
            }

            if (dateTimeSpanFormatType >= DateTimeSpanFormatTypeEnum.YYYYMMDD && days >= 1) {
                str = Part((int)days, "day");

                return str + " " + Calculate(dt1, dt2.AddDays((int)days), dateTimeSpanFormatType);
            }

            if (dateTimeSpanFormatType >= DateTimeSpanFormatTypeEnum.YYYYMMDDHH && hours >= 1) {
                str = Part((int)hours, "hour");

                return str + " " + Calculate(dt1, dt2.AddHours((int)hours), dateTimeSpanFormatType);
            }

            if (dateTimeSpanFormatType >= DateTimeSpanFormatTypeEnum.YYYYMMDDHHMM && minutes >= 1) {
                str = Part((int)minutes, "minute");

                return str + " " + Calculate(dt1, dt2.AddMinutes((int)minutes), dateTimeSpanFormatType);
            }

            if (dateTimeSpanFormatType >= DateTimeSpanFormatTypeEnum.YYYYMMDDHHMMSS && seconds >= 1) {
                str = Part((int)seconds, "second");

                return str + " " + Calculate(dt1, dt2.AddSeconds((int)seconds), dateTimeSpanFormatType);
            }

            if (dateTimeSpanFormatType >= DateTimeSpanFormatTypeEnum.YYYYMMDDHHMMSSMMMM && milliseconds >= 1) {
                str = Part((int)milliseconds, "millisecond");

                return str + " " + Calculate(dt1, dt2.AddMilliseconds((int)milliseconds), dateTimeSpanFormatType);
            }

            return "";
        }

        private static string Part(int count, string vowel) {
            string result = $"{count} {vowel}";

            if (count > 1) {
                result += "s";
            }

            return result;
        }
    }
}