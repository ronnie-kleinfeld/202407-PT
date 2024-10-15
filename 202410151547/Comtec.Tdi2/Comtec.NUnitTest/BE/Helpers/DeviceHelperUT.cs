using Comtec.BE.Helpers;

namespace Comtec.NUnitTest.BE.Helpers {
    public class DeviceHelperUT {
        [Test]
        public void UserName() {
            Assert.AreEqual("COMTEC-LTD_ronnie.k", DeviceHelper.UserName);
        }

        [Test]
        public void MachineName() {
            Assert.AreEqual("COM-WS005", DeviceHelper.MachineName);
        }

        [Test]
        public void OsVersion() {
            Assert.AreEqual("Microsoft Windows NT 10.0.26100.0", DeviceHelper.OsVersion);
        }

        [Test]
        public void NewLine() {
            Assert.AreEqual("\r\n", DeviceHelper.NewLine);
        }
    }
}