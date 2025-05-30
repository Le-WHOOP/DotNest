using DotNest.Models;
using DotNest.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DotNest.Controllers
{
    public class LocationController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILocationService _locationService;
        private readonly IRentalService _rentalService;

        public LocationController(IHttpContextAccessor contextAccessor, ILocationService locationService, IRentalService rentalService)
        {
            _contextAccessor = contextAccessor;
            _locationService = locationService;
            _rentalService = rentalService;
        }

        // List of all the rentals available
        public IActionResult Index()
        {
            string? username = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Name)?.Value;
            List<RentalModel> rentals = [];

            if (username == null)
            {
                rentals = _locationService.GetAvailableRentals();
            } else
            {
                rentals = _locationService.GetAllAvailableRentalsAndUserBooking(username!);
            }

             return View(rentals);
        }

        public IActionResult Detail(int id)
        {
            RentalModel? model = _rentalService.Get(id);

            if (model is null)
            {
                return NotFound();
            }

            // Gives to the View two booleans to determine the value/redirection of the book button:
            // isLoggedIn: the user is logged in or not
            // isOwner: the logged in user is the owner of the rental
            string? username = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Name)?.Value;
            bool isLoggedIn = User.Identity != null && User.Identity.IsAuthenticated;

            int userId = _locationService.GetIdFromUsername(username);

            bool isOwner = userId != -1;

            ViewData["IsOwner"] = isOwner;
            ViewData["IsLoggedIn"] = isLoggedIn;

            return View(model);
        }
    }
}
