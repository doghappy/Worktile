using System;
using System.Security.Cryptography;
using System.Text;

namespace Worktile.Common
{
    public static class HashEncryptor
    {
        public static string ComputeMd5(string text)
        {
            using (var md5 = MD5.Create())
            {
                var buffer = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
                return BitConverter.ToString(buffer).Replace("-","").ToLower();
            }
        }
    }
}
