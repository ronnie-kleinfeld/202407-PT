namespace Comtec.NUnitTest {
    [TestFixture]
    public class UnitTestLifeCycle : IDisposable {
        // 1. Called once for the class before methods
        [OneTimeSetUp]
        public static void ClassInitialize() {
            Console.WriteLine("ClassInitialize");
        }

        // 2. Called once before each test
        public UnitTestLifeCycle() {
            Console.WriteLine("ctor");
        }

        // 3. Called once before each test after the constructor
        [SetUp]
        public void TestInitialize() {
            Console.WriteLine("TestInitialize");
        }

        // 4. test
        [Test]
        public void Test1() {
            // prepare


            // test


            // clean


        }

        // 5. Called once after each test before the Dispose method
        [TearDown]
        public void TestCleanup() {
            Console.WriteLine("TestCleanup");
        }

        // 6. Called once after each test
        public void Dispose() {
            Console.WriteLine("Dispose");
        }

        // 7. Called once for the class after methods
        [OneTimeTearDown]
        public static void ClassCleanup() {
            Console.WriteLine("ClassCleanup");
        }
    }
}