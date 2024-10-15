using System.Text.RegularExpressions;

namespace Comtec.BE.Helpers {
    public class StringHelper {
        public static string ToProperCase(string str) {
            if (string.IsNullOrEmpty(str)) {
                return str;
            } else if (str.Length == 1) {
                return str.ToUpper();
            } else {
                return str[0].ToString().ToUpper() + str.Substring(1);
            }
        }
        public static string SpaceBeforeCamelCase(string str) {
            string result = string.Concat(str.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
            result = result.Replace("  ", " ");
            result = result.Replace("I D", "ID");
            result = result.Replace("P C", "PC");
            result = result.Replace("O S", "OS");
            result = result.Replace("G U ID", "GUID");
            result = result.Replace("U U ID", "UUID");
            result = result.Replace("U I", "UI");
            result = result.Replace("E Mail", "EMail");
            result = result.Replace("Un Locked", "UnLocked");
            result = result.Replace("User Name", "UserName");
            result = result.Replace("E I N", "EIN");
            result = result.Replace("D L", "DL");
            result = result.Replace("I D P", "IDP");
            return result;
        }
        public static string SafeString(string str) {
            if (str == null) {
                return string.Empty;
            } else {
                return str;
            }
        }
        public static string TrimToLength(string str, int length) {
            string result;
            if (str == null) {
                result = null;
            } else {
                result = str.Trim();
                if (result.Length > length) {
                    result = result.Substring(0, length);
                }
            }
            return result;
        }
        public static string Space(int count) {
            string result = string.Empty;
            for (int i = 0; i < count; i++) {
                result += " ";
            }
            return result;
        }
        public static string Concat(string str1, string sep, string str2) {
            if (str1 == string.Empty && str2 == string.Empty) {
                return string.Empty;
            } else if (str1 != string.Empty && str2 == string.Empty) {
                return str1;
            } else if (str1 == string.Empty && str2 != string.Empty) {
                return str2;
            } else {
                return str1 + sep + str2;
            }
        }
        public static string Combine(char seperator, params object[] args) {
            string result = string.Empty;
            for (int i = 0; i < args.Length; i++) {
                if (args[i].ToString().Length > 0) {
                    if (result.Length > 0) {
                        result += seperator.ToString();
                    }
                    result += args[i].ToString();
                }
            }
            return result;
        }
        public static string Left(string str, int length) {
            if (string.IsNullOrEmpty(str)) {
                return str;
            } else if (str.Length > length) {
                return str.Substring(0, length);
            } else {
                return str;
            }
        }
        public static string? ReverseIfHebrew(string? str) {
            if (str is null) {
                return null;
            }

            string result = str;

            if (Regex.IsMatch(str, @"\p{IsHebrew}")) {
                result = new string(str.Reverse().ToArray());
            }

            return result;
        }
    }
}