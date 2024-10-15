using Comtec.BE.Helpers;

namespace Comtec.NUnitTest.BE.Helpers {
    public class DateTimeFormatHelperUT {
        private readonly DateTime _dt1 = new DateTime(1970, 6, 15, 1, 2, 3);
        private readonly DateTime _dt2 = new DateTime(1970, 6, 15, 13, 2, 3);

        [Test]
        public void ToDateTimeFormat() {
            Assert.AreEqual(null, DateTimeFormatHelper.ToDateTimeFormat(null, "dd$MM$yyyy"));
            Assert.AreEqual(new DateTime(1970, 6, 15, 0, 0, 0), DateTimeFormatHelper.ToDateTimeFormat("15$06$1970", "dd$MM$yyyy"));
        }

        [Test]
        public void ToDateTimeFormatString() {
            Assert.AreEqual("", DateTimeFormatHelper.ToDateTimeFormatString(null, "dd$MM$yyyy"));
            Assert.AreEqual("01*02*03", DateTimeFormatHelper.ToDateTimeFormatString(_dt1, "HH*mm*ss"));
            Assert.AreEqual("15$06$1970", DateTimeFormatHelper.ToDateTimeFormatString(_dt1, "dd$MM$yyyy"));
        }

        [Test]
        public void ToHhmmss() {
            Assert.AreEqual("1:02:03", DateTimeFormatHelper.ToHhmmss(_dt1));
            Assert.AreEqual("13:02:03", DateTimeFormatHelper.ToHhmmss(_dt2));
        }

        [Test]
        public void ToDDMMYYYY_HHMMSS() {
            Assert.AreEqual("15/06/1970 1:02:03", DateTimeFormatHelper.ToDDMMYYYY_HHMMSS(_dt1));
            Assert.AreEqual("15/06/1970 13:02:03", DateTimeFormatHelper.ToDDMMYYYY_HHMMSS(_dt2));
        }

        [Test]
        public void ToLongDate() {
            Assert.AreEqual("Monday, 15 June 1970", DateTimeFormatHelper.ToLongDate(_dt1));
        }

        [Test]
        public void ToDDmmyyyy() {
            Assert.AreEqual("15/06/1970", DateTimeFormatHelper.ToDDmmyyyy(_dt1));
        }

        [Test]
        public void ToHhmm() {
            Assert.AreEqual("1:02", DateTimeFormatHelper.ToHhmm(_dt1));
            Assert.AreEqual("13:02", DateTimeFormatHelper.ToHhmm(_dt2));
        }

        [Test]
        public void ToUniversalTime() {
            Assert.AreEqual("14/06/1970 22:02:03", DateTimeFormatHelper.ToUniversalTime(_dt1));
            Assert.AreEqual("15/06/1970 10:02:03", DateTimeFormatHelper.ToUniversalTime(_dt2));
        }

        [Test]
        public void ToYYYYMMDD_HHMMSS() {
            Assert.AreEqual("1970-06-15 01:02:03", DateTimeFormatHelper.ToYYYYMMDD_HHMMSS(_dt1));
        }

        [Test]
        public void FromYYYYMMDD_HHMMSS() {
            Assert.AreEqual(_dt1, DateTimeFormatHelper.FromYYYYMMDD_HHMMSS("1970-06-15 01:02:03"));
        }
    }
}