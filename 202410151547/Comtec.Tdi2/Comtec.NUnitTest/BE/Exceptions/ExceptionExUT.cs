using Comtec.BE.Exceptions;
using Comtec.BE.Helpers;
using System.Reflection;

namespace Comtec.NUnitTest.BE.Exceptions {
    public class ExceptionExUT {
        [Test]
        public void Init() {
            ExceptionEx ex = new ExceptionEx(null, MethodBase.GetCurrentMethod(), -1, "text {0} {1}", 1, "a");
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.OsVersion, Is.EqualTo(DeviceHelper.OsVersion));
            Assert.That(ex.UserName, Is.EqualTo(DeviceHelper.UserName));
            Assert.That(ex.MachineName, Is.EqualTo(DeviceHelper.MachineName));
            Assert.That(ex.Method, Is.Not.Null);
            Assert.That(ex.Namespace, Is.EqualTo("Comtec.NUnitTest.BE.Exceptions"));
            Assert.That(ex.ClassName, Is.EqualTo("ExceptionExUT"));
            Assert.That(ex.MethodName, Is.EqualTo("Init"));
            Assert.That(ex.Data, Is.EqualTo(-1));
            Assert.That(ex.Text, Is.EqualTo("text 1 a"));
        }
    }
}