using BL.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Common.Models.Exceptions;

namespace BL.Models.Services
{
    //https://github.com/zsuzitor/pass_code/blob/4c963113ee90642ca609d1881c00717b9ca3879e/PassCode/Models/BL/Interfaces/ICoder.cs#L5
    public class AesCoder1 : ICoder
    {
        public string DecryptFromBytes(byte[] input, string key)
        {
            var keyInBytes = KeyToBytes(key);

            try
            {
                return DecryptStringFromBytesAes(input, keyInBytes);
            }
            catch
            {
                throw new SomeCustomException("не удалось расшифровать, возможно неверный пароль");
            }
        }

        public byte[] EncryptWithByte(string input, string key)
        {
            var keyInBytes = KeyToBytes(key);
            var encr = EncryptStringToBytesAes(input, keyInBytes);
            return encr;
        }



        

        public string EncryptWithString(string input, string key)
        {
            return BytesToCustomString(EncryptWithByte(input, key));
        }

        public string DecryptFromString(string input, string key)
        {
            return DecryptFromBytes(CustomStringToBytes(input), key);
        }


        private byte[] KeyToBytes(string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytesGood = new byte[32];
            var j = 0;
            for (var i = 0; i < keyBytesGood.Length; ++i, ++j)
            {
                if (j >= keyBytes.Length)
                {
                    j = 0;
                }

                keyBytesGood[i] = keyBytes[j];
            }

            return keyBytesGood;
        }

        private byte[] EncryptStringToBytesAes(string plainText, byte[] key)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException(nameof(plainText));
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));

            byte[] encrypted;

            var input = Encoding.UTF8.GetBytes(plainText);

            using Aes aesAlg = Aes.Create();

            aesAlg.Key = key;
            aesAlg.GenerateIV();

            // Create an encryptor to perform the stream transform.
            using (ICryptoTransform encryptor = aesAlg.CreateEncryptor())
            {
                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var tBinaryWriter = new BinaryWriter(csEncrypt))
                        {
                            msEncrypt.Write(aesAlg.IV);
                            tBinaryWriter.Write(input);
                            csEncrypt.FlushFinalBlock();
                        }

                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }


        private string DecryptStringFromBytesAes(byte[] cipherText, byte[] key)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(nameof(cipherText));
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));

            string plaintext = null;

            var iv = new byte[16];
            var txt_ = new byte[cipherText.Length - 16];
            Array.Copy(cipherText, 0, iv, 0, iv.Length);
            Array.Copy(cipherText, 16, txt_, 0, cipherText.Length - 16);

            using (Aes aesAlg = Aes.Create())
            {

                aesAlg.Key = key;
                aesAlg.IV = iv;

                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor())
                {
                    using (MemoryStream msDecrypt = new MemoryStream(txt_))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }

            return plaintext;
        }


        private string BytesToCustomString(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        private byte[] CustomStringToBytes(string str)
        {
            return Convert.FromBase64String(str);
        }
    }
}
