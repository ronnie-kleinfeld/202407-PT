namespace Comtec.BE.IO.File {
    public class HashFile {
        // methods
        private static System.IO.FileStream GetFileStream(string pathName) {
            return (new System.IO.FileStream(pathName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite));
        }
        public static string GetSha1Hash(string pathName) {
            string result = "";
            string hashData = "";

            byte[] hashValue;
            System.IO.FileStream fileStream = null;

            System.Security.Cryptography.SHA1CryptoServiceProvider sha1Hasher = new System.Security.Cryptography.SHA1CryptoServiceProvider();

            fileStream = GetFileStream(pathName);
            hashValue = sha1Hasher.ComputeHash(fileStream);
            fileStream.Close();

            hashData = System.BitConverter.ToString(hashValue);
            hashData = hashData.Replace("-", "");
            result = hashData;

            return (result);
        }
        public static string GetMd5Hash(string pathName) {
            string result = "";
            string hashData = "";

            byte[] hashValue;
            System.IO.FileStream fileStream = null;

            System.Security.Cryptography.MD5CryptoServiceProvider oMd5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();

            fileStream = GetFileStream(pathName);
            hashValue = oMd5Hasher.ComputeHash(fileStream);
            fileStream.Close();

            hashData = System.BitConverter.ToString(hashValue);
            hashData = hashData.Replace("-", "");
            result = hashData;

            return (result);
        }
    }
}