using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExpensesTracker.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ExpensesTracker.Utilities;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExpensesTracker.Controllers.APIControllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private IUser userStore;
        private IConfiguration configuration;
        public UserController(IUser users,IConfiguration conf)
        {
            userStore = users;
            configuration = conf;
        }
        // GET: api/<controller>
        [HttpGet]
        public List<AppUser> AllUsers() => userStore.GetAllUsers();

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ApiCreateUser newUser)
        {
            int d, y, m;
            string path = configuration.GetSection("LogPath").Value;
            Logger log = new Logger(path);

            bool dd = int.TryParse(newUser.Day, out d);
            bool mm = int.TryParse(newUser.Month, out m);
            bool yy = int.TryParse(newUser.Year, out y);

            if (!dd || !mm || !yy)
            {
                return BadRequest(new { error = "Invalid day/month/year" });
            }

            DateTime dob = new DateTime(y, m, d);

            AppUser user = new AppUser
            {
                UserName = newUser.UserName,
                Email = newUser.Email,
                PhoneNumber = newUser.PhoneNumber,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                DoB = dob,
                Sex = newUser.Sex
            };

            try
            {
                IdentityResult identityResult = await userStore.CreateNewUser(user, newUser.Password);

                if (identityResult.Succeeded)
                {
                    AppUser u = await userStore.FindUser(user.UserName);
                    user = new AppUser
                    {
                        Id = u.Id,
                        UserName = u.UserName,
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        DoB = u.DoB,
                        Sex = u.Sex
                    };
                    return Ok(user);
                }
                else
                {
                    IEnumerable<IdentityError> errs = identityResult.Errors;
                    foreach (IdentityError err in errs)
                    {
                        log.WriteError(new string[] { $"Error code: {err.Code}", $"Error description: {err.Description}" });
                    }

                    return BadRequest(errs);
                }
            }
            catch (Exception ex)
            {
                log.WriteError(new string[] { $"Exception msg: {ex.Message}", $"Exception stack trace: {ex.StackTrace}" });
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
