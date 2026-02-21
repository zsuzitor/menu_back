using BL.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BL.Models.Services
{
    public class AesCoder2 : ICoder
    {

        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            if (bytesToBeEncrypted == null || bytesToBeEncrypted.Length == 0)
                throw new ArgumentException("Data to encrypt cannot be empty", nameof(bytesToBeEncrypted));

            if (passwordBytes == null || passwordBytes.Length == 0)
                throw new ArgumentException("Password cannot be empty", nameof(passwordBytes));

            // Генерируем случайную соль для каждого шифрования (лучшая практика)
            byte[] saltBytes = new byte[16]; // 16 байт соли
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            using (var ms = new MemoryStream())
            {
                // Сначала записываем соль, чтобы потом можно было расшифровать
                ms.Write(saltBytes, 0, saltBytes.Length);

                using (var aes = Aes.Create())
                {
                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    // Используем статический метод PBKDF2 для генерации ключа и IV
                    int keySizeInBytes = aes.KeySize / 8; // 32 байта
                    int ivSizeInBytes = aes.BlockSize / 8; // 16 байт

                    // Генерируем ключ и IV за один проход
                    byte[] keyAndIv = new byte[keySizeInBytes + ivSizeInBytes];
                    keyAndIv = Rfc2898DeriveBytes.Pbkdf2(
                        passwordBytes,
                        saltBytes,
                        100000,        // Итерации
                        HashAlgorithmName.SHA256,
                        keySizeInBytes + ivSizeInBytes
                    );

                    // Разделяем на ключ и IV
                    aes.Key = keyAndIv.Take(keySizeInBytes).ToArray();
                    aes.IV = keyAndIv.Skip(keySizeInBytes).Take(ivSizeInBytes).ToArray();

                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                    }

                    return ms.ToArray();
                }
            }
        }

        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            if (bytesToBeDecrypted == null || bytesToBeDecrypted.Length < 16)
                throw new ArgumentException("Invalid encrypted data", nameof(bytesToBeDecrypted));

            if (passwordBytes == null || passwordBytes.Length == 0)
                throw new ArgumentException("Password cannot be empty", nameof(passwordBytes));

            using (var ms = new MemoryStream(bytesToBeDecrypted))
            {
                // Читаем соль из начала зашифрованных данных
                byte[] saltBytes = new byte[16];
                ms.Read(saltBytes, 0, 16);

                using (var aes = Aes.Create())
                {
                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    // Генерируем ключ и IV из пароля и соли
                    byte[] keyAndIv = Rfc2898DeriveBytes.Pbkdf2(
                        password: passwordBytes,
                        salt: saltBytes,
                        iterations: 100000,
                        hashAlgorithm: HashAlgorithmName.SHA256,
                        outputLength: 48 // 32 (key) + 16 (IV)
                    );

                    // Используние диапазонов C# 8.0+ для более чистого кода
                    aes.Key = keyAndIv[0..32];
                    aes.IV = keyAndIv[32..48];

                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    using (var resultMs = new MemoryStream())
                    {
                        cs.CopyTo(resultMs);
                        return resultMs.ToArray();
                    }
                }
            }
        }

        public static string EncryptString(string text, string password)
        {
            byte[] baPwd = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            byte[] baPwdHash = SHA256.HashData(baPwd);

            byte[] baText = Encoding.UTF8.GetBytes(text);

            byte[] baSalt = GetRandomBytes();
            byte[] baEncrypted = new byte[baSalt.Length + baText.Length];

            // Combine Salt + Text
            for (int i = 0; i < baSalt.Length; i++)
                baEncrypted[i] = baSalt[i];
            for (int i = 0; i < baText.Length; i++)
                baEncrypted[i + baSalt.Length] = baText[i];

            baEncrypted = AES_Encrypt(baEncrypted, baPwdHash);

            string result = Convert.ToBase64String(baEncrypted);
            return result;
        }

        public static string DecryptString(string text, string password)
        {
            byte[] baPwd = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            byte[] baPwdHash = SHA256.HashData(baPwd);

            byte[] baText = Convert.FromBase64String(text);

            byte[] baDecrypted = AES_Decrypt(baText, baPwdHash);

            // Remove salt
            int saltLength = GetSaltLength();
            byte[] baResult = new byte[baDecrypted.Length - saltLength];
            for (int i = 0; i < baResult.Length; i++)
                baResult[i] = baDecrypted[i + saltLength];

            string result = Encoding.UTF8.GetString(baResult);
            return result;
        }

        public static byte[] GetRandomBytes()
        {
            int saltLength = GetSaltLength();
            byte[] ba = new byte[saltLength];
            RandomNumberGenerator.Create().GetBytes(ba);
            return ba;
        }

        public static int GetSaltLength()
        {
            return 8;
        }

        public byte[] EncryptWithByte(string input, string key)
        {
            throw new NotImplementedException();
        }

        public string EncryptWithString(string input, string key)
        {
            // Get the bytes of the string
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(key);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            string result = Convert.ToBase64String(bytesEncrypted);

            return result;
        }

        public string DecryptFromString(string input, string key)
        {
            // Get the bytes of the string
            byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(key);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            string result = Encoding.UTF8.GetString(bytesDecrypted);

            return result;
        }

        public string DecryptFromBytes(byte[] input, string key)
        {
            throw new NotImplementedException();
        }
    }
}
