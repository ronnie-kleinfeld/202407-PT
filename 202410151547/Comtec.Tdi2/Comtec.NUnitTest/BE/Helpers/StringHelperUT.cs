using Comtec.BE.Helpers;

namespace Comtec.NUnitTest.BE.Helpers {
    public class StringHelperUT {
        public void SpaceBeforeCamelCase() {
            Assert.AreEqual("Space Before Camel Case?", StringHelper.SpaceBeforeCamelCase("SpaceBeforeCamelCase?"));
        }

        [TestCase("", "")]
        [TestCase("sample", "sample")]
        public void SafeString(string expected, string str) {
            Assert.AreEqual(expected, StringHelper.SafeString(str));
        }

        [TestCase("", "", 0)]
        [TestCase("", "", 1)]
        [TestCase("", " ", 0)]
        [TestCase("", " ", 1)]
        [TestCase("", "  ", 0)]
        [TestCase("", "  ", 1)]
        [TestCase("", "a", 0)]
        [TestCase("a", "a", 1)]
        [TestCase("", "a ", 0)]
        [TestCase("a", "a ", 1)]
        [TestCase("", "a  ", 0)]
        [TestCase("a", "a  ", 1)]
        [TestCase("", " a ", 0)]
        [TestCase("a", " a ", 1)]
        public void TrimToLength(string expected, string str, int length) {
            Assert.AreEqual(expected, StringHelper.TrimToLength(str, length));
        }

        [TestCase("", 0)]
        [TestCase(" ", 1)]
        [TestCase("  ", 2)]
        public void Space(string expected, int count) {
            Assert.AreEqual(expected, StringHelper.Space(count));
        }

        [TestCase("", "", "", "")]
        [TestCase("a", "a", "", "")]
        [TestCase("b", "", "", "b")]
        [TestCase("ab", "a", "", "b")]
        [TestCase("", "", "/", "")]
        [TestCase("a", "a", "/", "")]
        [TestCase("b", "", "/", "b")]
        [TestCase("a/b", "a", "/", "b")]
        public void Concat(string expected, string str1, string sep, string str2) {
            Assert.AreEqual(expected, StringHelper.Concat(str1, sep, str2));
        }

        [TestCase("", ' ')]
        [TestCase("", ',')]
        [TestCase("1", ',', "1")]
        [TestCase("1,2", ',', "1", "2")]
        [TestCase("1,2,3", ',', "1", "2", "3")]
        public void Combine(string expected, char seperator, params object[] args) {
            Assert.AreEqual(expected, StringHelper.Combine(seperator, args));
        }

        [TestCase("", "", 0)]
        [TestCase("", "", 1)]
        [TestCase("", "1", 0)]
        [TestCase("1", "1", 1)]
        public void Left(string expected, string str, int length) {
            Assert.AreEqual(expected, StringHelper.Left(str, length));
        }
    }
}