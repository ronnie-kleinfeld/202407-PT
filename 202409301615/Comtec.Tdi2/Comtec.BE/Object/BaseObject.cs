using Comtec.BE.Exceptions;
using Comtec.BE.Helpers;
using Newtonsoft.Json;
using System.Reflection;

namespace Comtec.BE.Object {
    public abstract class BaseObject {
        // validators
        protected virtual string IsNotNull(MethodBase method, string value) {
            if (value == null) {
                throw new ExceptionEx(null, MethodBase.GetCurrentMethod(), null, "{0} is required", method.Name);
            } else {
                return value;
            }
        }
        protected virtual string IsNotEmpty(MethodBase method, string value) {
            if (value == string.Empty) {
                throw new ExceptionEx(null, MethodBase.GetCurrentMethod(), null, "{0} is required", method.Name);
            } else {
                return value;
            }
        }
        protected virtual string IsNotNullNotEmpty(MethodBase method, string value) {
            if (string.IsNullOrEmpty(value)) {
                throw new ExceptionEx(null, MethodBase.GetCurrentMethod(), value, "{0} is required", method.Name);
            } else {
                return value;
            }
        }
        protected virtual string MaxLength(MethodBase method, string value, int length) {
            if (value == null || value.Length <= length) {
                return value;
            } else {
                throw new ExceptionEx(null, MethodBase.GetCurrentMethod(), value, "{0} length is out of range {1}", method.Name, length);
            }
        }
        protected virtual string IsNotTheSame(MethodBase method, string value, string oldValue) {
            if (value == oldValue) {
                throw new ExceptionEx(null, MethodBase.GetCurrentMethod(), value, "A new value is required for {0}", method.Name);
            } else {
                return value;
            }
        }
        protected virtual int IsPositiveNumber(MethodBase method, int value) {
            if (value < 0) {
                throw new ExceptionEx(null, MethodBase.GetCurrentMethod(), value, "{0} must be equal to 0 or higher", method.Name);
            } else {
                return value;
            }
        }
        protected virtual int IsInRange(MethodBase method, int value, int minValue, int maxValue) {
            if (value < minValue || value > maxValue) {
                throw new ExceptionEx(null, MethodBase.GetCurrentMethod(), value, "{0}={1} must be between {2} and {3}", method.Name, value, minValue, maxValue);
            } else {
                return value;
            }
        }
        protected virtual DateTime? IsInDateTimeRange(MethodBase method, DateTime? value) {
            if (value == null) {
                return null;
            } else if (value < new DateTime(1753, 1, 1, 0, 0, 0) || value > new DateTime(9999, 12, 31, 23, 59, 59)) {
                throw new ExceptionEx(null, MethodBase.GetCurrentMethod(), value, "{0} must be between Jan 1, 1753 and Dec 31, 9999", method.Name);
            } else {
                return value;
            }
        }

        // to...
        public virtual string ToJson(Formatting formatting = Formatting.None) {
            return ToVisibleHelper.ToJson(this, formatting);
        }
        public virtual string ToCsv() {
            return ToVisibleHelper.ToCsv(this);
        }
    }
}