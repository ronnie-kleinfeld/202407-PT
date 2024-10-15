using Comtec.BE.Helpers;

namespace Comtec.NUnitTest.BE.Helpers {
    public class HashHelperUT {
        [Test]
        public void Test10000Same() {
            string str = "InputIntoHash";
            string hash = HashHelper.OneWayHash(str);

            for (int i = 0; i < 10000; i++) {
                Assert.AreEqual(HashHelper.OneWayHash(str), hash);
            }
        }

        [Test]
        public void Test10000Random1s() {
            Random random = new Random(DateTime.Now.Millisecond);

            Dictionary<string, string> dic = new Dictionary<string, string>();

            for (long i = 1000000; i < 1010000; i++) {
                string str = i.ToString();
                string hash = HashHelper.OneWayHash(str);

                if (dic.ContainsKey(hash)) {
                    Assert.Fail("Hash not Unique");
                } else {
                    dic[hash] = str;
                }
            }

            Assert.AreEqual(dic.Count, 10000);
        }

        [Test]
        public void Test10000Random2() {
            Random random = new Random(DateTime.Now.Millisecond);

            Dictionary<string, string> dic = new Dictionary<string, string>();

            for (long i = 1000000; i < 1010000; i++) {
                string str = GuidHelper.NewGuid;
                string hash = HashHelper.OneWayHash(str);

                if (dic.ContainsKey(hash)) {
                    Assert.Fail("Hash not Unique");
                } else {
                    dic[hash] = str;
                }
            }

            Assert.AreEqual(dic.Count, 10000);
        }
    }
}