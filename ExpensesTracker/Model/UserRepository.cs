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

        public async Task<AppUser> DeleteUser(string username)
        {
            AppUser user = await userManager.FindByNameAsync(username);

            if (user!=null)
            {
                await userManager.DeleteAsync(user);
            }
            
            return user;
        }

        public List<AppUser> GetAllUsers() => userManager.Users.ToList();
    }
}
