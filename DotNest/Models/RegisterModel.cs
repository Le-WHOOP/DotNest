using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DotNest.Models
{
    public class RegisterModel
    {
        [Required, StringLength(50, MinimumLength = 6)]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }


        [Required, StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }

        [DisplayName("Repeat password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
