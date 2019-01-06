using System;
using System.ComponentModel.DataAnnotations;

namespace ExpensesTracker.Model
{
    public class SignupViewModel
    {
        [Required(ErrorMessage ="Specify first name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Specify last name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Specify user name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Specify password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords mismatch")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Specify e-mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Specify phone number")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Specify sex")]
        public string Sex { get; set; }
        [Required(ErrorMessage = "Specify DoB")]
        public DateTime DoB { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [UIHint("password")]
        public string Password { get; set; }
    }
}
