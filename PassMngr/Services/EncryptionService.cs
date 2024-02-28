using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PassMngr.Services
{
    public class EncryptionService
    {
        private static string EncryptKey;

        static EncryptionService()
        {
            // Read the encryption key from the text file
            EncryptKey = File.ReadAllText("../secretKey.txt");
        }

        private const int KeySize = 256;
        private const int BlockSize = 128;

        public string EncryptString(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                var myVal = Encoding.UTF8.GetBytes(EncryptKey);
                aesAlg.Key = myVal;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public string DecryptString(string cipherText)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(EncryptKey);
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                int ivLength = BitConverter.ToInt32(cipherBytes, 0);
                aesAlg.IV = cipherBytes.Skip(sizeof(int)).Take(ivLength).ToArray();

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes, sizeof(int) + ivLength, cipherBytes.Length - sizeof(int) - ivLength))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }
    }

}
