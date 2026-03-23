using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Grimmory.Settings
{
    internal static class DeviceCrypto
    {
        private const string AppSalt = "Grimmory.CE.LocalCredential.v1";

        public static string EncryptToBase64(string plainText, string username)
        {
            if (plainText == null)
                plainText = string.Empty;

            byte[] key;
            byte[] iv;
            DeriveKeyMaterial(username, out key, out iv);

            byte[] input = Encoding.UTF8.GetBytes(plainText);

            using (TripleDESCryptoServiceProvider crypto = new TripleDESCryptoServiceProvider())
            {
                crypto.Mode = CipherMode.CBC;
                crypto.Padding = PaddingMode.PKCS7;
                crypto.Key = key;
                crypto.IV = iv;

                using (MemoryStream output = new MemoryStream())
                {
                    using (ICryptoTransform transform = crypto.CreateEncryptor())
                    using (CryptoStream stream = new CryptoStream(output, transform, CryptoStreamMode.Write))
                    {
                        stream.Write(input, 0, input.Length);
                        stream.FlushFinalBlock();
                        return Convert.ToBase64String(output.ToArray());
                    }
                }
            }
        }

        public static string DecryptFromBase64(string encryptedBase64, string username)
        {
            byte[] key;
            byte[] iv;
            DeriveKeyMaterial(username, out key, out iv);

            byte[] input = Convert.FromBase64String(encryptedBase64);

            using (TripleDESCryptoServiceProvider crypto = new TripleDESCryptoServiceProvider())
            {
                crypto.Mode = CipherMode.CBC;
                crypto.Padding = PaddingMode.PKCS7;
                crypto.Key = key;
                crypto.IV = iv;

                using (MemoryStream output = new MemoryStream())
                {
                    using (ICryptoTransform transform = crypto.CreateDecryptor())
                    using (MemoryStream inputStream = new MemoryStream(input))
                    using (CryptoStream stream = new CryptoStream(inputStream, transform, CryptoStreamMode.Read))
                    {
                        byte[] buffer = new byte[256];
                        int read;
                        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            output.Write(buffer, 0, read);
                        }
                    }

                    return Encoding.UTF8.GetString(output.ToArray(), 0, (int)output.Length);
                }
            }
        }

        private static void DeriveKeyMaterial(string username, out byte[] key, out byte[] iv)
        {
            string identity = BuildIdentity(username);
            byte[] keySource = BuildDerivedBytes(identity + "|KEY", 24);
            byte[] ivSource = BuildDerivedBytes(identity + "|IV", 8);

            key = new byte[24];
            iv = new byte[8];

            Buffer.BlockCopy(keySource, 0, key, 0, key.Length);
            Buffer.BlockCopy(ivSource, 0, iv, 0, iv.Length);
        }

        private static string BuildIdentity(string username)
        {
            string userPart = username ?? string.Empty;
            string machinePart = GetDeviceIdentity();
            return AppSalt + "|" + machinePart + "|" + userPart;
        }

        private static string GetDeviceIdentity()
        {
            try
            {
                string hostName = Dns.GetHostName();
                if (!string.IsNullOrEmpty(hostName))
                    return hostName;
            }
            catch
            {
            }

            try
            {
                string codeBase = Assembly.GetExecutingAssembly().GetName().CodeBase;
                if (!string.IsNullOrEmpty(codeBase))
                    return codeBase;
            }
            catch
            {
            }

            return "grimmory-device";
        }

        private static byte[] BuildDerivedBytes(string seed, int length)
        {
            byte[] output = new byte[length];
            int copied = 0;
            int counter = 0;

            while (copied < length)
            {
                byte[] hashed = HashString(seed + "|" + counter.ToString());
                int toCopy = Math.Min(hashed.Length, length - copied);
                Buffer.BlockCopy(hashed, 0, output, copied, toCopy);
                copied += toCopy;
                counter++;
            }

            return output;
        }

        private static byte[] HashString(string value)
        {
            byte[] input = Encoding.UTF8.GetBytes(value);
            using (SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider())
            {
                return sha.ComputeHash(input);
            }
        }
    }
}
