using DotNest.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using static System.Net.Mime.MediaTypeNames;
using DotNest.Services.Interfaces;

namespace DotNest.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;

        private readonly IUserService _userService;

        public UserController(IHttpContextAccessor contextAccessor, IUserService userService)
        {
            _contextAccessor = contextAccessor;
            _userService = userService;
        }

        [Authorize]
        public ActionResult Index()
        { 
            return View();
        }



        public ActionResult Register()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel register)
        {
            if (!ModelState.IsValid)
                return View(register);

            try
            {
                //var userRequest = VMRegisterMapper.MapToBL(register);
                //_userService.Register(userRequest);


                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Username", ex.Message);
                return View(register);
            }

        }

        //GET
        public ActionResult Login()
        {
            string? username = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            if (username is not null)
                return RedirectToAction("Index");

            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel login)
        {
            string? username = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            if (username is not null)
                return RedirectToAction("Index");

            if (!ModelState.IsValid)
                return View(login);

            //var user = _userRepository.GetConfirmedUser(
            //    login.Username,
            //    login.Password);

            //if (user == null)
            //{
            //    ModelState.AddModelError("Username", "Invalid username or password");
            //    return View(login);
            //}

            //var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username) };
            //var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //HttpContext.SignInAsync(
            //    CookieAuthenticationDefaults.AuthenticationScheme,
            //    new ClaimsPrincipal(claimsIdentity),
            //    new AuthenticationProperties()).Wait();

            return RedirectToAction(nameof(Index));
        }

        //POST
        [HttpPost]
        [Authorize]
        public ActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();

            return RedirectToAction(nameof(Login));
        }


    }
}
