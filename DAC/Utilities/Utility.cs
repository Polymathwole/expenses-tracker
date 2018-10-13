using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace DAC.Utilities
{
    public class Utility
    {
        public static string HashPass(string pwd)
        {
            byte[] bite = Encoding.UTF8.GetBytes(pwd);
            byte[] hashbite = SHA512.Create().ComputeHash(bite);
            return Convert.ToBase64String(hashbite);
        }

        public static Guid GetGuid()
        {
            return Guid.NewGuid();
        }
    }
}
