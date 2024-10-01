using System.Security.Cryptography;
using System.Text;

namespace Comtec.BE.Helpers {
    public class HashHelper {
        private const string Salt = "yjgkyjgjyghtdgrsfdzxfbcgvjkkhbmvbncbccht";

        public static string OneWayHash(string str) {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(str + Salt);
            byte[] hash = sha256.ComputeHash(bytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++) {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}