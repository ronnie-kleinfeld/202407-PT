using Comtec.BE.Exceptions;
using Comtec.BE.Helpers;
using System.Reflection;

namespace Comtec.NUnitTest.BE.Exceptions {
    public class SwitchOutOfRangeExceptionUT {
        [Test]
        public void Init() {
            ExceptionEx ex = new SwitchOutOfRangeException(MethodBase.GetCurrentMethod(), -1);
            Assert.IsNotNull(ex);
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.OsVersion, Is.EqualTo(DeviceHelper.OsVersion));
            Assert.That(ex.UserName, Is.EqualTo(DeviceHelper.UserName));
            Assert.That(ex.MachineName, Is.EqualTo(DeviceHelper.MachineName));
            Assert.That(ex.Method, Is.Not.Null);
            Assert.That(ex.Namespace, Is.EqualTo("Comtec.NUnitTest.BE.Exceptions"));
            Assert.That(ex.ClassName, Is.EqualTo("SwitchOutOfRangeExceptionUT"));
            Assert.That(ex.MethodName, Is.EqualTo("Init"));
            Assert.That(ex.Data, Is.EqualTo(-1));
            Assert.That(ex.Text, Is.EqualTo("Switch value -1 is out of range"));
        }
    }
}