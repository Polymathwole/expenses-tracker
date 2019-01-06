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
using System.Net.Mail;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExpensesTracker.Controllers.APIControllers
{
    [Route("api/[controller]/[action]")]
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
        [HttpGet("{username}")]
        public async Task<IActionResult> Get(string username)
        {
            AppUser user = await userStore.FindUser(username);

            if (user==null)
            {
                return NotFound(new { error = $"'{username}' not found" });
            }
            
            return Ok(user);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ApiCreateUser newUser)
        {
            int d, y, m;
            string path = configuration.GetSection("LogPath").Value;
            Logger log = new Logger(path);

            bool dd = int.TryParse(newUser.Day.Trim(), out d);
            bool mm = int.TryParse(newUser.Month.Trim(), out m);
            bool yy = int.TryParse(newUser.Year.Trim(), out y);

            if (!dd || !mm || !yy)
            {
                return BadRequest(new { error = "Invalid day/month/year" });
            }

            DateTime dob;

            try
            {
                dob = new DateTime(y, m, d);
            }
            catch
            {
                return BadRequest(new { error = "Invalid date" });
            }

            try
            {
                MailAddress email = new MailAddress(newUser.Email.Trim());
            }
            catch (FormatException fme)
            {
                return BadRequest(new { error = "Invalid e-mail" });
            }
            catch
            {
                return BadRequest(new { error = "E-mail cannot be null." });
            }

            if (!newUser.PhoneNumber.Trim().StartsWith("0"))
            {
                return BadRequest(new { error = "Phone number must start with 0." });
            }
            else if (newUser.PhoneNumber.Trim().Length != 11)
            {
                return BadRequest(new { error = "Phone number must be 11 digits." });
            }

            try
            {
                if (Convert.ToChar(newUser.Sex.Trim().ToUpper()) != 'M' && Convert.ToChar(newUser.Sex.ToUpper().Trim()) != 'F')
                {
                    return BadRequest(new { error = "Sex must be M or F." });
                }
            }
            catch
            {
                return BadRequest(new { error = "Invalid. Sex must be M or F." });
            }

            if (newUser.UserName.Trim().Length < 5 || newUser.UserName.Trim().Length > 10)
            {
                return BadRequest(new { error = "Username must have between 5 and 10 characters." });
            }

            if (newUser.FirstName.Trim().Length < 2)
            {
                return BadRequest(new { error = "First name must have at least 2 characters." });
            }

            if (newUser.LastName.Trim().Length < 2)
            {
                return BadRequest(new { error = "Last name must have at least 2 characters." });
            }

            AppUser user = new AppUser
            {
                UserName = newUser.UserName.Trim(),
                Email = newUser.Email.Trim(),
                PhoneNumber = newUser.PhoneNumber.Trim(),
                FirstName = newUser.FirstName.Trim(),
                LastName = newUser.LastName.Trim(),
                DoB = dob,
                Sex = newUser.Sex.ToUpper().Trim()
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
        /* [HttpPut("{username}")]
        public async Task<IActionResult> Put(string username, [FromBody]ApiCreateUser existingUser)
        {
            string path = configuration.GetSection("LogPath").Value;
            Logger log = new Logger(path);
            int d, y, m;

            if (username.Trim() == "")
            {
                return BadRequest(new { error = "User to delete not specified." });
            }

            bool dd = int.TryParse(existingUser.Day.Trim(), out d);
            bool mm = int.TryParse(existingUser.Month.Trim(), out m);
            bool yy = int.TryParse(existingUser.Year.Trim(), out y);

            if (!dd || !mm || !yy)
            {
                return BadRequest(new { error = "Invalid day/month/year" });
            }

            DateTime dob;

            try
            {
                dob = new DateTime(y, m, d);
            }
            catch
            {
                return BadRequest(new { error = "Invalid date" });
            }

            try
            {
                MailAddress email = new MailAddress(existingUser.Email.Trim());
            }
            catch (FormatException fme)
            {
                return BadRequest(new { error = "Invalid e-mail" });
            }
            catch
            {
                return BadRequest(new { error = "E-mail cannot be null." });
            }

            if (!newUser.PhoneNumber.Trim().StartsWith("0"))
            {
                return BadRequest(new { error = "Phone number must start with 0." });
            }
            else if (newUser.PhoneNumber.Trim().Length != 11)
            {
                return BadRequest(new { error = "Phone number must be 11 digits." });
            }

            try
            {
                if (Convert.ToChar(newUser.Sex.Trim().ToUpper()) != 'M' && Convert.ToChar(newUser.Sex.ToUpper().Trim()) != 'F')
                {
                    return BadRequest(new { error = "Sex must be M or F." });
                }
            }
            catch
            {
                return BadRequest(new { error = "Invalid. Sex must be M or F." });
            }

            if (newUser.UserName.Trim().Length < 5 || newUser.UserName.Trim().Length > 10)
            {
                return BadRequest(new { error = "Username must have between 5 and 10 characters." });
            }

            if (newUser.FirstName.Trim().Length < 2)
            {
                return BadRequest(new { error = "First name must have at least 2 characters." });
            }

            if (newUser.LastName.Trim().Length < 2)
            {
                return BadRequest(new { error = "Last name must have at least 2 characters." });
            }

            AppUser user = new AppUser
            {
                UserName = newUser.UserName.Trim(),
                Email = newUser.Email.Trim(),
                PhoneNumber = newUser.PhoneNumber.Trim(),
                FirstName = newUser.FirstName.Trim(),
                LastName = newUser.LastName.Trim(),
                DoB = dob,
                Sex = Convert.ToChar(newUser.Sex.ToUpper().Trim())
            };
        }

        // DELETE api/<controller>/5
        [HttpDelete("{username}")]
        public async Task<IActionResult> Delete(string username)
        {
            string path = configuration.GetSection("LogPath").Value;
            Logger log = new Logger(path);

            if (username.Trim() == "")
            {
                return BadRequest(new { error = "User to delete not specified." });
            }
            IdentityResult result = await userStore.DeleteUser(username);
            if (result==null)
            {
                return Ok(new { error = "User not found." });
            }
            else
            {
                if (result.Succeeded)
                {
                    return Ok(new { message = "User deleted successfully." });
                }
                else
                {
                    IEnumerable<IdentityError> errs = result.Errors;
                    foreach (IdentityError err in errs)
                    {
                        log.WriteError(new string[] { $"Error code: {err.Code}", $"Error description: {err.Description}" });
                    }

                    return BadRequest(errs);
                }
            }
        } */
    }
}
