using ExpensesTracker.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace ExpensesTracker.Utilities
{
    public class PasswordHasher:IPasswordHasher<AppUser>
    {
        public string HashPassword(AppUser user, string password)
        {
            byte[] bites = Encoding.UTF8.GetBytes(password);
            byte[] encrBit = SHA512.Create().ComputeHash(bites);
            return Convert.ToBase64String(encrBit);
        }

        public PasswordVerificationResult VerifyHashedPassword(AppUser user, string hashedPwd, string claimPwd)
        {
            byte[] bites = Encoding.UTF8.GetBytes(claimPwd);
            byte[] encrBit = SHA512.Create().ComputeHash(bites);

            if (Convert.ToBase64String(encrBit).Equals(hashedPwd))
            {
                return PasswordVerificationResult.Success;
            }
            else
            {
                return PasswordVerificationResult.Failed;
            }
        }
    }
}
