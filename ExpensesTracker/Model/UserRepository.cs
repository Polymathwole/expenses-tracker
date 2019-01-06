using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ExpensesTracker.Model
{
    public class UserRepository:IUser
    {
        private UserManager<AppUser> userManager;

        public UserRepository(UserManager<AppUser> um)
        {
            userManager = um;
        }

        public async Task<IdentityResult> CreateNewUser(AppUser user, string password)
        {
            //userManager.PasswordHasher = new PasswordHasher();
            return await userManager.CreateAsync(user, password);
        }

        public async Task<AppUser> FindUser(string username) => await userManager.FindByNameAsync(username);

        public async Task<IdentityResult> DeleteUser(string username)
        {
            IdentityResult result = null;
            AppUser user = await userManager.FindByNameAsync(username);

            if (user!=null)
            {
                result = await userManager.DeleteAsync(user);
            }
            
            return result;
        }

        public List<AppUser> GetAllUsers() => userManager.Users.ToList();

        public async Task<IdentityResult> UpdateUser(AppUser user) => await userManager.UpdateAsync(user);
    }
}
