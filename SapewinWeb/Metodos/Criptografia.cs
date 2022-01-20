using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace SapewinWeb.Metodos
{
    public static class Criptografia
    {
        public static string HashValue(string value)
        {
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] hashBytes;
            using (HashAlgorithm hash = SHA512.Create())
                hashBytes = hash.ComputeHash(encoding.GetBytes(value));

            StringBuilder hashValue = new StringBuilder(hashBytes.Length * 2);

            for (int i = 0; i < hashBytes.LongLength; i++)
            {
                hashValue.AppendFormat(CultureInfo.InvariantCulture, "{0:X2}", hashBytes[i]);
            }

            return hashValue.ToString();
        }

        public static string TiraAcentos(string value)
        {
            Regex r = new Regex(@"[áàãâä]");
            value = r.Replace(value, "a");

            r = new Regex(@"[ÁÀÃÂÄ]");
            value = r.Replace(value, "A");

            r = new Regex(@"[éèêë]");
            value = r.Replace(value, "e");

            r = new Regex(@"[ÉÈÊË]");
            value = r.Replace(value, "E");            

            r = new Regex(@"[íìîï]");
            value = r.Replace(value, "i");

            r = new Regex(@"[ÍÌÎÏ]");
            value = r.Replace(value, "I");

            r = new Regex(@"[óòõôö]");
            value = r.Replace(value, "o");

            r = new Regex(@"[ÓÒÕÔÖ]");
            value = r.Replace(value, "O");

            r = new Regex(@"[úùûü]");
            value = r.Replace(value, "u");

            r = new Regex(@"[ÚÙÛÜ]");
            value = r.Replace(value, "U");

            r = new Regex(@"\*");
            value = r.Replace(value, " ");

            return value;
        }
    }
}