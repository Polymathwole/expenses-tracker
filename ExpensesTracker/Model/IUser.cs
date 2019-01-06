using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Model
{
    public interface IUser
    {
        Task<IdentityResult> CreateNewUser(AppUser user, string password);
        Task<AppUser> FindUser(string username);
        List<AppUser> GetAllUsers();
        Task<IdentityResult> DeleteUser(string username);
        Task<IdentityResult> UpdateUser(AppUser user);
    }
}
