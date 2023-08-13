using System;
using System.Security.Cryptography;
using System.Text;

namespace Beelog.UtilityClasses
{

    public static class HashUtility
    {
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("x2")); // Convert to hexadecimal
                }
                return builder.ToString();
            }
        }
    }

}
