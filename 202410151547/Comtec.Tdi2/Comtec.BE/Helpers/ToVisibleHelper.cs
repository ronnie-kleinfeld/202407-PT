using Comtec.BE.Object;
using Newtonsoft.Json;
using System.Collections;
using System.Reflection;

namespace Comtec.BE.Helpers {
    public static class ToVisibleHelper {
        // ToJson
        public static string ToJson(IList collection, Formatting formatting = Formatting.None) {
            return JsonConvert.SerializeObject(collection, formatting);
        }
        public static string ToJson(SortedList sortedList, Formatting formatting = Formatting.None) {
            return JsonConvert.SerializeObject(sortedList, formatting);
        }
        public static string ToJson(object o, Formatting formatting = Formatting.None) {
            return JsonConvert.SerializeObject(o, formatting);
        }

        // ToCSV
        public static string ToCsv(IList collection) {
            string result = "";

            foreach (object o in collection) {
                result += ToCsv(o) + DeviceHelper.NewLine;
            }

            return result;
        }

        public static string ToCsv(object o) {
            string result = string.Empty;

            try {
                if (o is IList) {
                    return ToCsv((IList)o);
                } else {
                    foreach (PropertyInfo prop in o.GetType().GetProperties()) {
                        try {
                            object[] attribute = prop.GetCustomAttributes(typeof(IgnoreToCsvAttribute), true);
                            if (attribute.Length == 0) {
                                if (result != string.Empty) {
                                    result += ", ";
                                }

                                result += $"{prop.Name}={prop.GetValue(o, null)}";
                            } else {
                                IgnoreToCsvAttribute ignoreToStringAttribute = (IgnoreToCsvAttribute)attribute[0];
                                if (ignoreToStringAttribute == null) {
                                    if (result != string.Empty) {
                                        result += ", ";
                                    }

                                    result += $"{prop.Name}={prop.GetValue(o, null)}";
                                }
                            }
                        } catch {
                        }
                    }
                }
            } catch {
                return o.ToString();
            }

            return result;
        }
    }
}