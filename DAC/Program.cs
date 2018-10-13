using System;
using Microsoft.EntityFrameworkCore;
using DAC.Model;

namespace DAC
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDbContext dbContext = new AppDbContext();
            try
            {
                dbContext.Add(new User { FirstName = "Oluwaremi", LastName = "Oluwole", PhoneNo = "08063242564", Email = "oluwaremi.oluwole@gmail.com", Username = "wolexy007", DoB = new DateTime(1989, 04, 19), Password = "Adetoro", ConfirmPassword = "Adetoro", UserId = 100 });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception message {ex.Message}\n Stack trace {ex.StackTrace}");
            }        
        }
    }
}
