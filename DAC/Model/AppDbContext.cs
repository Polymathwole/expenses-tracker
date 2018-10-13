using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using DAC.Utilities;

namespace DAC.Model
{
    public class AppDbContext : DbContext
    {
       public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = @"Data Source=OLUWAREMI\SQLEXPRESS16;Initial Catalog=Expenses_Tracker;Integrated Security=True";
            optionsBuilder.UseSqlServer(connectionString);
            //optionsBuilder.UseSqlServer($"{ConfigurationManager.ConnectionStrings["EFPlayDb"].ConnectionString}");
        }

       protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<User>().HasData(
                new User {FirstName="Oluwaremi",LastName="Oluwole",PhoneNo="08063242564",Email="oluwaremi.oluwole@gmail.com",Username="wolexy007",DoB=new DateTime(1989,04,19),hashedPassword= Utility.HashPass("Adetoro"),UserId=100},
                new User { FirstName = "Adetoro", LastName = "Thompson", PhoneNo = "08139509623", Email = "tashabiadetoro@gmail.com", Username = "tadetoro", DoB = new DateTime(1994, 03, 23), hashedPassword = Utility.HashPass("ThompsonAdetoro@007"), UserId = 01 },
                new User { FirstName = "Deborah", LastName = "Israel", PhoneNo = "08167870592", Email = "airattitilayo@yahoo.com", Username = "mom1234", DoB = new DateTime(1965, 09, 24), hashedPassword = Utility.HashPass("Oluwole@007"), UserId = 53},
                new User { FirstName = "Aderemi", LastName = "Oyebo", PhoneNo = "08131540283", Email = "oyeboremi@yahoo.com", Username = "herdeyremmy", DoB = new DateTime(1992, 05, 12), hashedPassword = Utility.HashPass("mariam"), UserId = 21},
                new User { FirstName = "Olaide", LastName = "Adesopo", PhoneNo = "08025170477", Email = "adesopoolaide@gmail.com", Username = "olanoni666", DoB = new DateTime(1992, 07, 25), hashedPassword = Utility.HashPass("Ola"), UserId = 666}
            );
        }
    }
}
