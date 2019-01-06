using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExpensesTracker.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace ExpensesTracker.Controllers
{
    public class UserController : Controller
    {
        private IUser userStore;
        private IConfiguration configuration;
        private SignInManager<AppUser> signInManager;

        public UserController(IServiceProvider serviceProvider)
        {
            userStore = serviceProvider.GetService<IUser>();
            configuration = serviceProvider.GetService<IConfiguration>();
            signInManager = serviceProvider.GetService<SignInManager<AppUser>>();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                AppUser claimedUser = await userStore.FindUser(lvm.UserName);

                if (claimedUser !=null)
                {
                    await signInManager.SignOutAsync();

                    Microsoft.AspNetCore.Identity.SignInResult signInResult = await signInManager.PasswordSignInAsync(claimedUser, lvm.Password, false, false);

                    if (signInResult.Succeeded)
                    {
                        HttpContext.Session.SetString("User", lvm.UserName);
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else if (signInResult.IsNotAllowed)
                    {
                        ModelState.AddModelError("", "Invalid login details");
                        return View();
                    }
                    else if (signInResult.IsLockedOut)
                    {
                        ModelState.AddModelError("", "User locked out");
                        return View();
                    }
                }
                ModelState.AddModelError("", "Invalid login details");
                return View();
            }
            return View();
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupViewModel svm)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    FirstName = svm.FirstName,
                    LastName = svm.LastName,
                    Sex = svm.Sex,
                    DoB = svm.DoB,
                    UserName = svm.UserName,
                    Email = svm.Email,
                    PhoneNumber = svm.PhoneNumber
                };

                IdentityResult result = await userStore.CreateNewUser(user, svm.Password);

                if (result.Succeeded)
                {
                    TempData["message"] = "User successfully created.";
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    foreach (IdentityError err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }
            return View();
        }
    }
}