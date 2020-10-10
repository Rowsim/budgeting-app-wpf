using System;
using System.Security.Cryptography;
using System.Text;

namespace FYP.DataAccess
{
    public static class Crypt
    {
        private static readonly byte[] key = {67, 196, 178, 92, 143, 103, 126, 216};
        private static readonly byte[] iv = {39, 135, 141, 208, 31, 138, 243, 168};

        public static string Encrypt(this string text)
        {
            ICryptoTransform cryptoTransform = DES.Create().CreateEncryptor(key, iv);
            byte[] inputbuffer = Encoding.Unicode.GetBytes(text);
            byte[] outputBuffer = cryptoTransform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Convert.ToBase64String(outputBuffer);
        }

        public static string Decrypt(this string text)
        {
            ICryptoTransform cryptoTransform = DES.Create().CreateDecryptor(key, iv);
            byte[] inputbuffer = Convert.FromBase64String(text);
            byte[] outputBuffer = cryptoTransform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Encoding.Unicode.GetString(outputBuffer);
        }
    }
}