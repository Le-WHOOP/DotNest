using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DotNest.Models
{
    public class RegisterModel
    {
        [Required, StringLength(50, MinimumLength = 6), DisplayName("Nom d'utilisateur")]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }


        [Required, StringLength(50, MinimumLength = 8), DisplayName("Mot de passe")]
        public string Password { get; set; }

        [DisplayName("Répéter le mot de passe")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
