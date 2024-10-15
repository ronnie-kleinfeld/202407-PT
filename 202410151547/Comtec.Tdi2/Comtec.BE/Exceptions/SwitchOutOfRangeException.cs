using System.Reflection;

namespace Comtec.BE.Exceptions {
    public class SwitchOutOfRangeException : ExceptionEx {
        // class
        public SwitchOutOfRangeException(MethodBase method, object data) : base(null, method, data, "Switch value {0} is out of range", data) {
        }
    }
}