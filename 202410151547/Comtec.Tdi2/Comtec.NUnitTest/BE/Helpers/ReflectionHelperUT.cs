using Comtec.BE.Helpers;
using System.Reflection;

namespace Comtec.NUnitTest.BE.Helpers {
    public class ReflectionHelperUT {
        [Test]
        public void Namespace() {
            Assert.AreEqual("Comtec.NUnitTest.BE.Helpers", ReflectionHelper.Namespace(MethodBase.GetCurrentMethod()));
        }

        [Test]
        public void ClassName() {
            Assert.AreEqual("ReflectionHelperUT", ReflectionHelper.ClassName(MethodBase.GetCurrentMethod()));
        }

        [Test]
        public void MethodNameNull() {
            Assert.AreEqual("", ReflectionHelper.MethodName(null));
        }

        [Test]
        public void FullName() {
            Assert.AreEqual("Comtec.NUnitTest.BE.Helpers.ReflectionHelperUT:FullName", ReflectionHelper.FullName(MethodBase.GetCurrentMethod()));
        }

        [Test]
        public void MethodNameGetCurrentMethod() {
            Assert.AreEqual("MethodNameGetCurrentMethod", ReflectionHelper.MethodName(MethodBase.GetCurrentMethod()));
        }
    }
}