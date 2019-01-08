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
using ExpensesTracker.Utilities;

namespace ExpensesTracker.Controllers
{
    public class UserController : Controller
    {
        private IUser userStore;
        private IConfiguration configuration;
        private SignInManager<AppUser> signInManager;
        private IAuditRepo auditRepo;

        public UserController(IServiceProvider serviceProvider)
        {
            userStore = serviceProvider.GetService<IUser>();
            configuration = serviceProvider.GetService<IConfiguration>();
            signInManager = serviceProvider.GetService<SignInManager<AppUser>>();
            auditRepo = serviceProvider.GetService<IAuditRepo>();
        }

        public IActionResult Login()
        {
            return View();
        }

        private Logger Logger()
        {
            string path = configuration.GetSection("LogPath").Value;
            Logger log = new Logger(path);
            return log;
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

                        try
                        {
                            string remoteIpAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                            {
                                remoteIpAddress = Request.Headers["X-Forwarded-For"];
                            }
                                
                            auditRepo.AddAuditRecord(new Audit
                            {
                                UserName = lvm.UserName,
                                Event = Model.Action.Login.ToString(),
                                UserIP = remoteIpAddress,
                                TimeStamp = DateTime.Now
                            });
                        }
                        catch (Exception ex)
                        {
                            Logger().WriteError(new string[] { $"Exception msg: {ex.Message}", $"Exception stack trace: {ex.StackTrace}" });
                        }
                        
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
                    PhoneNumber = svm.PhoneNumber,
                    DateCreated = DateTime.Now
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