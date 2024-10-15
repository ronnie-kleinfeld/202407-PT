using Comtec.BE.Helpers;

namespace Comtec.NUnitTest.BE.Helpers {
    public class DateTimeHelperUT {
        private readonly DateTime _dt1 = new DateTime(1970, 6, 15);
        private readonly DateTime _dt2 = new DateTime(2000, 2, 15);
        private readonly DateTime _dt3 = new DateTime(2001, 2, 15);
        private readonly DateTime _dt4 = new DateTime(2001, 8, 15);
        private readonly DateTime _dt5 = new DateTime(2001, 10, 15);

        [Test]
        public void FirstDayOfYear() {
            Assert.AreEqual(new DateTime(1970, 1, 1, 0, 0, 0), DateTimeHelper.FirstDayOfYear(_dt1));
            Assert.AreEqual(new DateTime(2000, 1, 1, 0, 0, 0), DateTimeHelper.FirstDayOfYear(_dt2));
            Assert.AreEqual(new DateTime(2001, 1, 1, 0, 0, 0), DateTimeHelper.FirstDayOfYear(_dt3));
        }

        [Test]
        public void LastDayOfYear() {
            Assert.AreEqual(new DateTime(1970, 12, 31, 0, 0, 0), DateTimeHelper.LastDayOfYear(_dt1));
            Assert.AreEqual(new DateTime(2000, 12, 31, 0, 0, 0), DateTimeHelper.LastDayOfYear(_dt2));
            Assert.AreEqual(new DateTime(2001, 12, 31, 0, 0, 0), DateTimeHelper.LastDayOfYear(_dt3));
        }

        [Test]
        public void FirstDayOfMonth() {
            Assert.AreEqual(new DateTime(1970, 6, 1, 0, 0, 0), DateTimeHelper.FirstDayOfMonth(_dt1));
            Assert.AreEqual(new DateTime(2000, 2, 1, 0, 0, 0), DateTimeHelper.FirstDayOfMonth(_dt2));
            Assert.AreEqual(new DateTime(2001, 2, 1, 0, 0, 0), DateTimeHelper.FirstDayOfMonth(_dt3));
        }

        [Test]
        public void LastDayOfMonth() {
            Assert.AreEqual(new DateTime(1970, 6, 30, 0, 0, 0), DateTimeHelper.LastDayOfMonth(_dt1));
            Assert.AreEqual(new DateTime(2000, 2, 29, 0, 0, 0), DateTimeHelper.LastDayOfMonth(_dt2));
            Assert.AreEqual(new DateTime(2001, 2, 28, 0, 0, 0), DateTimeHelper.LastDayOfMonth(_dt3));
        }

        [Test]
        public void BeginOfQuarter() {
            Assert.AreEqual(new DateTime(1970, 4, 1, 0, 0, 0), DateTimeHelper.BeginOfQuarter(_dt1));
            Assert.AreEqual(new DateTime(2000, 1, 1, 0, 0, 0), DateTimeHelper.BeginOfQuarter(_dt2));
            Assert.AreEqual(new DateTime(2001, 1, 1, 0, 0, 0), DateTimeHelper.BeginOfQuarter(_dt3));
            Assert.AreEqual(new DateTime(2001, 7, 1, 0, 0, 0), DateTimeHelper.BeginOfQuarter(_dt4));
            Assert.AreEqual(new DateTime(2001, 10, 1, 0, 0, 0), DateTimeHelper.BeginOfQuarter(_dt5));
        }

        [Test]
        public void EndOfQuarter() {
            Assert.AreEqual(new DateTime(1970, 6, 30, 0, 0, 0), DateTimeHelper.EndOfQuarter(_dt1));
            Assert.AreEqual(new DateTime(2000, 3, 31, 0, 0, 0), DateTimeHelper.EndOfQuarter(_dt2));
            Assert.AreEqual(new DateTime(2001, 3, 31, 0, 0, 0), DateTimeHelper.EndOfQuarter(_dt3));
            Assert.AreEqual(new DateTime(2001, 9, 30, 0, 0, 0), DateTimeHelper.EndOfQuarter(_dt4));
            Assert.AreEqual(new DateTime(2001, 12, 31, 0, 0, 0), DateTimeHelper.EndOfQuarter(_dt5));
        }
    }
}