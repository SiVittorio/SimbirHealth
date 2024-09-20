using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimbirHealth.Common.Services
{
    /// <summary>
    /// Сервис для получения хэша и проверки строки и её хэш-интерпретации
    /// </summary>
    public static class Hasher
    {
        /// <summary>
        /// Получить хэш MD5
        /// </summary>
        public static string Hash(string s)
        {
            MD5 md5 = MD5.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes(s);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new();

            for (int i = 0; i < hashBytes.Length; i++)
                sb.Append(hashBytes[i].ToString("X2"));

            return sb.ToString();
        }

        /// <summary>
        /// Соответствует ли строка хэшу MD5
        /// </summary>
        public static bool Verify(string s, string hash)
        {
            return string.Equals(Hash(s), hash);
        }
    }
}
