using System.ComponentModel;

namespace DotNest.Models
{
    public class LoginModel
    {
        [DisplayName("Nom d'utilisateur")]
        public string Username { get; set; }
        [DisplayName("Mot de passe")]
        public string Password { get; set; }
    }
}
