using Comtec.BE.Enum;
using Comtec.BE.Helpers;

namespace Comtec.NUnitTest.BE.Helpers {
    public class DateTimeSpanHelperUT {
        [Test]
        public void Span() {
            Helper(DateTime.Now.AddMinutes(-1), DateTimeSpanFormatTypeEnum.YYYYMMDD, "");
            Helper(DateTime.Now.AddMinutes(-1), DateTimeSpanFormatTypeEnum.YYYYMMDDHH, "");
            Helper(DateTime.Now.AddMinutes(-1), DateTimeSpanFormatTypeEnum.YYYYMMDDHHMM, "1 minute");
            Helper(DateTime.Now.AddMilliseconds(-1), DateTimeSpanFormatTypeEnum.YYYYMMDDHHMMSSMMMM, "1 millisecond");
            Helper(DateTime.Now.AddMilliseconds(-456), DateTimeSpanFormatTypeEnum.YYYYMMDDHHMMSSMMMM, "456 milliseconds");
            Helper(DateTime.Now.AddMinutes(-1), DateTimeSpanFormatTypeEnum.YYYYMMDDHHMMSS, "1 minute");
            Helper(DateTime.Now.AddMinutes(-1), DateTimeSpanFormatTypeEnum.YYYYMMDDHHMMSS, "1 minute");
            Helper(DateTime.Now, DateTimeSpanFormatTypeEnum.YYYYMMDDHHMMSS, "");
            Helper(DateTime.Now.AddSeconds(-5), DateTimeSpanFormatTypeEnum.YYYYMMDDHHMMSS, "5 seconds");
            Helper(DateTime.Now.AddMinutes(-5), DateTimeSpanFormatTypeEnum.YYYYMMDDHHMMSS, "5 minutes");
            Helper(DateTime.Now.AddHours(-5), DateTimeSpanFormatTypeEnum.YYYYMMDDHHMMSS, "5 hours");
            Helper(DateTime.Now.AddDays(-5), DateTimeSpanFormatTypeEnum.YYYYMMDDHHMMSS, "5 days");
            Helper(DateTime.Now.AddDays(-1), DateTimeSpanFormatTypeEnum.YYYYMMDDHHMMSS, "1 day");
            Helper(DateTime.Now.AddYears(-1).AddMonths(-2).AddDays(-1), DateTimeSpanFormatTypeEnum.YYYYMMDDHHMMSS, "1 year 2 months 1 day");
            //Helper(DateTime.Now.AddYears(-1), DateTimeSpanFormatTypeEnum.YYYY, "1 year");
            Helper(DateTime.Now.AddYears(-21).AddDays(-1), DateTimeSpanFormatTypeEnum.YYYY, "21 years");
        }
        private string Helper(DateTime dt, DateTimeSpanFormatTypeEnum dateTimeSpanFormatType, string expected) {
            string str = DateTimeSpanHelper.Span(dt, dateTimeSpanFormatType);
            Assert.AreEqual(expected, DateTimeSpanHelper.Span(dt, dateTimeSpanFormatType));
            return str;
        }
    }
}