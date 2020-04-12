using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Gatekeeper.Utils {
    public static class CryptoUtils {
        public const int IV_LENGTH = 16;
        public const int SALT_LENGTH = 16;
        public const int KEY_LENGTH = 16;

        public static byte[] encrypt(byte[] text, byte[] key) {
            using (var aes = Aes.Create()) {
                using (var encryptor = aes.CreateEncryptor(key, aes.IV)) {
                    using (var mem = new MemoryStream()) {
                        using (var encryptStream = new CryptoStream(mem, encryptor, CryptoStreamMode.Write)) {
                            using var bw = new BinaryWriter(encryptStream);
                            bw.Write(text);
                        }

                        var iv = aes.IV;
                        var data = mem.ToArray();
                        var result = new byte[iv.Length + data.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(data, 0, result, iv.Length, data.Length);

                        return result;
                    }
                }
            }
        }

        public static byte[] decrypt(byte[] data, byte[] key) {
            var iv = new byte[IV_LENGTH];
            var cipher = new byte[data.Length - iv.Length];

            Buffer.BlockCopy(data, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(data, iv.Length, cipher, 0, cipher.Length);
            using (var aes = Aes.Create()) {
                using (var decryptor = aes.CreateDecryptor(key, iv)) {
                    using (var mem = new MemoryStream(cipher)) {
                        using (var decryptStream = new CryptoStream(mem, decryptor, CryptoStreamMode.Read)) {
                            using (var sr = new StreamReader(decryptStream)) {
                                var plaintext = sr.ReadToEnd();
                                return Encoding.UTF8.GetBytes(plaintext);
                            }
                        }
                    }
                }
            }
        }

        public static string encrypt(string text, string key) =>
            Convert.ToBase64String(encrypt(Encoding.UTF8.GetBytes(text), Convert.FromBase64String(key)));

        public static string decrypt(string data, string key) =>
            Encoding.UTF8.GetString(decrypt(Convert.FromBase64String(data), Convert.FromBase64String(key)));

        public static byte[] randomBytes(uint length) {
            using var rng = new RNGCryptoServiceProvider();
            var randomData = new byte[length];
            rng.GetBytes(randomData);
            return randomData;
        }
    }
}