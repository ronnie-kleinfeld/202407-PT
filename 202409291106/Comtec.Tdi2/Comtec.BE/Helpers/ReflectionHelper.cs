using System.Reflection;
using System.Text;

namespace Comtec.BE.Helpers {
    public class ReflectionHelper {
        public static string Namespace(MethodBase methodBase) {
            string str = QualifiedMethodName(methodBase);
            try {
                string[] strs = str.Split('.');
                strs[strs.Length - 1] = null;
                strs[strs.Length - 2] = null;
                str = String.Join(".", strs);
                str = str.Substring(0, str.Length - 2);
                return str;
            } catch {
                return str;
            }
        }
        public static string ClassName(MethodBase methodBase) {
            string str = QualifiedMethodName(methodBase);
            try {
                string[] strs = str.Split('.');
                return strs[strs.Length - 2];
            } catch {
                return str;
            }
        }
        public static string MethodName(MethodBase methodBase) {
            string str = QualifiedMethodName(methodBase);
            try {
                string[] strs = str.Split('.');
                return strs[strs.Length - 1];
            } catch {
                return str;
            }
        }
        public static string FullName(MethodBase methodBase) {
            return $"{Namespace(methodBase)}.{ClassName(methodBase)}:{MethodName(methodBase)}";
        }

        private static string QualifiedMethodName(MethodBase methodBase) {
            if (methodBase != null && methodBase.ReflectedType != null && methodBase.ReflectedType.FullName != null) {
                StringBuilder sb = new StringBuilder();
                sb.Append(methodBase.ReflectedType.FullName.Substring(methodBase.ReflectedType.FullName.IndexOf("+") + 1));
                sb.Replace("+", ".");
                sb.Append(".");
                sb.Append(methodBase.Name);
                return sb.ToString();
            } else {
                return string.Empty;
            }
        }
        private static string QualifiedPropertyName(MethodBase methodBase) {
            if (methodBase != null && methodBase.ReflectedType != null && methodBase.ReflectedType.FullName != null) {
                StringBuilder sb = new StringBuilder();
                sb.Append(methodBase.ReflectedType.FullName.Substring(methodBase.ReflectedType.FullName.IndexOf("+") + 1));
                sb.Replace("+", ".");
                sb.Append(".");
                sb.Append(methodBase.Name.Substring(methodBase.Name.IndexOf("_") + 1));
                return sb.ToString();
            } else {
                return string.Empty;
            }
        }

        public static void InvokeMethodsInClass(Type type) {
            ConstructorInfo constructorInfo = type.GetConstructor(Type.EmptyTypes);
            object o = constructorInfo.Invoke(new object[] { });

            foreach (MethodInfo methodInfo in type.GetMethods()) {
                if (methodInfo.Name != "Init") {
                    ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                    object[] parameters = new object[parameterInfos.Length];
                    int index = 0;
                    foreach (ParameterInfo parameterInfo in parameterInfos) {
                        parameters.SetValue(null, index);
                    }

                    _ = methodInfo.Invoke(o, parameters);
                }
            }
        }

        public static bool ContainsMethod(Type type, string methodName) {
            ConstructorInfo constructorInfo = type.GetConstructor(Type.EmptyTypes);
            object o = constructorInfo.Invoke(new object[] { });

            foreach (MethodInfo methodInfo in type.GetMethods()) {
                if (methodInfo.Name == methodName) {
                    return true;
                }
            }

            return false;
        }
    }
}