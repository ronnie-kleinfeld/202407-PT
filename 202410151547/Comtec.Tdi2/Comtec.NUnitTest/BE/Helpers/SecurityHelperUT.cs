using Comtec.BE.Helpers;

namespace Comtec.NUnitTest.BE.Helpers {
    public class SecurityHelperUT {
        [Test]
        public void GenerateStringNumericPassword() {
            Random random = new Random(DateTime.Now.Millisecond);
            string numeric = "0123456789";

            for (long i = 10000000; i < 10100000; i++) {
                int length = random.Next(5, 15);
                string str = SecurityHelper.GenerateNumericPassword(length);

                Assert.IsNotNull(str);
                Assert.AreEqual(length, str.Length);

                foreach (char chr in str) {
                    Assert.AreEqual(false, !numeric.Contains(chr));
                }
            }
        }

        [Test]
        public void GenerateIntNumericPassword() {
            Random random = new Random(DateTime.Now.Millisecond);

            for (long i = 10000000; i < 10100000; i++) {
                int minValue = random.Next(5, 15);
                int maxValue = minValue + random.Next(5, 15);
                int number = SecurityHelper.GenerateNumericPassword(minValue, maxValue);

                Assert.IsNotNull(number);
                Assert.AreEqual(true, number >= minValue);
                Assert.AreEqual(true, number <= maxValue);
            }
        }

        [Test]
        public void GenerateAlphaPassword() {
            Random random = new Random(DateTime.Now.Millisecond);
            string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            for (long i = 10000000; i < 10100000; i++) {
                int length = random.Next(5, 15);
                string str = SecurityHelper.GenerateAlphaPassword(length);

                Assert.IsNotNull(str);
                Assert.AreEqual(length, str.Length);

                foreach (char chr in str) {
                    Assert.AreEqual(false, !alpha.Contains(chr));
                }
            }
        }

        [Test]
        public void GenerateAlphaNumericPassword() {
            Random random = new Random(DateTime.Now.Millisecond);
            string alphaNumeric = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            for (long i = 10000000; i < 10100000; i++) {
                int length = random.Next(5, 15);
                string str = SecurityHelper.GenerateAlphaNumericPassword(length);

                Assert.IsNotNull(str);
                Assert.AreEqual(length, str.Length);

                foreach (char chr in str) {
                    Assert.AreEqual(false, !alphaNumeric.Contains(chr));
                }
            }
        }
    }
}