using System;
using System.Collections.Generic;
using System.Text;

namespace DAC.Model
{
    interface IUserRepository
    {
        IEnumerable<User> GetUsers();
        User GetUser();
        void AddUser(User user);
        void RemoveUser(User user);
        void UpdateUser(User user);
    }
}
