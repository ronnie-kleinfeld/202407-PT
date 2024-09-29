using Comtec.BE.LogEx;
using Comtec.BE.Object;
using System.Reflection;

namespace Comtec.BE.Result {
    public class ResultData :BaseObject {
        // members
        public bool IsSuccess => ResultCodeType >= 0;
        public int ResultCodeType {
            get; protected set;
        }

        public string Namespace {
            get; protected set;
        }
        public string ClassName {
            get; protected set;
        }
        public string MethodName {
            get; protected set;
        }

        public string Title {
            get; protected set;
        }
        public string Text {
            get; protected set;
        }

        public object Data {
            get; protected set;
        }

        public Exception Ex {
            get; protected set;
        }

        // class
        public ResultData() :
            this(null, (int)ResultCodeTypeEnum.Success, null, null, string.Empty, string.Empty) {
        }
        public ResultData(int resultCodeType) :
            this(null, resultCodeType, null, null, string.Empty, string.Empty) {
        }
        public ResultData(int resultCodeType, object data) :
            this(null, resultCodeType, null, data, string.Empty, string.Empty) {
        }
        public ResultData(int resultCodeType, MethodBase? method, object data, string title, string text, params object[] args) :
            this(null, resultCodeType, method, data, title, text, args) {
        }

        public ResultData(Exception ex, int resultCodeType) :
            this(ex, resultCodeType, null, null, string.Empty, string.Empty) {
        }
        public ResultData(Exception? ex, int resultCodeType, MethodBase? method, object data, string title, string text, params object[] args) {
            ResultCodeType = (int)resultCodeType;

            if (method != null && method.ReflectedType != null) {
                Namespace = method?.ReflectedType?.Namespace ?? "";
                ClassName = method?.ReflectedType?.Name ?? "";
                MethodName = method?.Name ?? "";
            }

            Data = data;

            Title = title;
            if (args == null || args.Length == 0) {
                Text = text;
            } else {
                Text = string.Format(text, args);
            }

            Ex = ex;

            if (ResultCodeType < 0) {
                LogHelper.WriteError(MethodBase.GetCurrentMethod(), Text);
            } else {
                LogHelper.WriteDebug(MethodBase.GetCurrentMethod(), Text);
            }
        }
    }
}