namespace Comtec.NUnitTest {
    [TestFixture]
    public class UnitTestMultipleParams {
        [TestCase(1, 1, 2)]
        [TestCase(12, 30, 42)]
        [TestCase(14, 1, 15)]
        public void Test_Add(int a, int b, int expected) {
            var actual = a + b;
            Assert.AreEqual(expected, actual);
        }
    }
}