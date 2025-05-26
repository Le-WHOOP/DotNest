using DotNest.Models;
using DotNest.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DotNest.Controllers
{
    public class RentalController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public RentalController(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
