using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
//using Comtec.TIS;
//using SqlInterface;


namespace UploadToSql
{
    class UploadToSql
    {
        static string s0 = "Data Source=test-tis;Initial Catalog=EncryptedFiles;User ID=dsp_tables;Password=server;Pooling=true";
        static byte[] key = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF }; //aes.Key;
        static byte[] IV = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10 }; // aes.IV;
        static string inputFile = "C:/Users/noy.g/source/repos/FileEncryptDecrypt/FileEncryptDecrypt/a1.txt",
                       encryptedFilePath = "C:/Users/noy.g/source/repos/FileEncryptDecrypt/FileEncryptDecrypt/encrypted.txt",
                       decryptedFilePath = "C:/Users/noy.g/source/repos/FileEncryptDecrypt/FileEncryptDecrypt/decrypted.txt";
        static void Main()
        {
           /* try
            {
                // save the hash of the file before encryption
                string FileHashBeforeEnc = GetHashCode(inputFile, new MD5CryptoServiceProvider());
                Console.WriteLine("FileHashBeforeEnc: " + FileHashBeforeEnc);

                // Generate a key and IV
                AesManaged aes = new AesManaged();  //aes.GenerateKey(); //aes.GenerateIV();

                // Encrypt the file
                EncryptFile(inputFile, encryptedFilePath, key, IV);

                uploadFile(FileHashBeforeEnc, encryptedFilePath);

                downloadFile(1);

                // Decrypt the file
                DecryptFile(encryptedFilePath, decryptedFilePath, key, IV);

                string FileHashAfterDec = GetHashCode(decryptedFilePath, new MD5CryptoServiceProvider());

                if (FileHashAfterDec == FileHashBeforeEnc)
                {
                    Console.WriteLine("Hashes match!");
                }
                else
                {
                    Console.WriteLine("Hashes do not match");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught: " + ex);
            }*/
        }


        static void EncryptFile(string inputFile, string outputFile, byte[] key, byte[] iv)
        {
            using AesManaged aes = new AesManaged();
            //    aes.Key = key;
            //    aes.IV = iv;

            // Create a encryptor to perform the stream transform.
            ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);

            // Create the streams used for encryption.
            using FileStream input = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
            using FileStream output = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
            using CryptoStream cryptoStream = new CryptoStream(output, encryptor, CryptoStreamMode.Write);
            input.CopyTo(cryptoStream);
        }

        static void DecryptFile(string inputFile, string outputFile, byte[] key, byte[] iv)
        {
            using AesManaged aes = new AesManaged();
            //aes.Key = key;
            //aes.IV = iv;

            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = aes.CreateDecryptor(key, iv);

            // Create the streams used for decryption.
            using FileStream input = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
            using FileStream output = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
            using CryptoStream cryptoStream = new CryptoStream(input, decryptor, CryptoStreamMode.Read);
            cryptoStream.CopyTo(output);
        }

        static string GetHashCode(string filePath, HashAlgorithm cryptoService)
        {
            // create or use the instance of the crypto service provider
            // this can be either MD5, SHA1, SHA256, SHA384 or SHA512
            using (cryptoService)
            {
                using (var fileStream = new FileStream(filePath,
                                                       FileMode.Open,
                                                       FileAccess.Read,
                                                       FileShare.ReadWrite))
                {
                    var hash = cryptoService.ComputeHash(fileStream);
                    var hashString = Convert.ToBase64String(hash);
                    return hashString.TrimEnd('=');
                }
            }
        }


        static void uploadFile(string fileHash, string filePath)
        {
            using SqlConnection connection = new SqlConnection(s0);
            connection.Open();
            SqlCommand command = new SqlCommand("INSERT INTO Files (file_hash, enc_file_stream) VALUES (@file_hash, @enc_file_stream)", connection);
            command.Parameters.Add("@file_hash", SqlDbType.NChar).Value = fileHash;// "QRTSGwNKLtixBGJhcL1wVA";
            command.Parameters.Add("@enc_file_stream", SqlDbType.VarBinary).Value = File.ReadAllBytes(filePath); //"example.txt"
            command.ExecuteNonQuery();
        }

        static void downloadFile(int id)
        {
            using SqlConnection connection = new SqlConnection(s0);
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT file_hash, enc_file_stream FROM Files WHERE id = @id", connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;// 1;
            using SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                string encryptedFilePath = reader.GetString(0);
                byte[] enc_file_stream = (byte[])reader.GetValue(1);
                File.WriteAllBytes(encryptedFilePath, enc_file_stream);
            }

        }
    }
}
