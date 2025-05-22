using DotNest.Models;
using DotNest.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            // check whether the user is already logged in
            string? username = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Name)?.Value;

            if (username is not null)
                return RedirectToAction("Index");


            // the user is not logged in, they can see the log in form
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel register)
        {
            // check whether the user is already logged in 
            string? username = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Name)?.Value;

            if (username is not null)
                return RedirectToAction("Index");


            // the user is not logged in, they can submit the form
            if (!ModelState.IsValid)
                return View(register);

            try
            {
                _userService.RegisterUser(register);

                // the user has created their account, they can now log in
                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Username", ex.Message); // show an error message under the username field
                return View(register); // show the view with the submitted info
            }

        }

        //GET
        public ActionResult Login()
        {
            // check whether the user is already logged in 
            string? username = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Name)?.Value;

            if (username is not null)
                return RedirectToAction("Index");

            // the user is not logged in, they can see the form
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel login)
        {
            // check whether the user is already logged in 
            string? username = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Name)?.Value;

            if (username is not null)
                return RedirectToAction("Index");


            // the user is not logged in, they can submit the form
            if (!ModelState.IsValid)
                return View(login);

            if (!_userService.ConfirmLoginValues(login))
            {
                ModelState.AddModelError("Username", "Invalid username or password");
                return View(login);
            }

            // add the user to the context
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, login.Username) };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties()).Wait();

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
