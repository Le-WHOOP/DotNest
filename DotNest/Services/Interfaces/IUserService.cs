using DotNest.DataAccess.Entities;
using DotNest.Models;

namespace DotNest.Services.Interfaces
{
    public interface IUserService
    {
        public void RegisterUser(RegisterModel model);

        public string? GetUserFromLogin(LoginModel model);
        User? Test(string v);
    }
}
