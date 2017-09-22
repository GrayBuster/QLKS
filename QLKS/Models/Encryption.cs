using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace QLKS.Models
{
    public class Encryption
    {
        public static string encrypt(string ToEncrypt)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(ToEncrypt));
        }
        public static string decrypt(string cypherString)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(cypherString));
        }

    }
}