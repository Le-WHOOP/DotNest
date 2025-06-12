using DotNest.DataAccess.Entities;
using DotNest.Models;
using DotNest.Services;
using DotNest.Services.Interfaces;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DotNest.Controllers
{
    public class LocationController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILocationService _locationService;
        private readonly IRentalService _rentalService;
        private readonly IUserService _userService;
        private readonly IBookingService _bookingService;

        public LocationController(
            IHttpContextAccessor contextAccessor,
            ILocationService locationService,
            IRentalService rentalService,
            IUserService userService,
            IBookingService bookingService)
        {
            _contextAccessor = contextAccessor;
            _locationService = locationService;
            _rentalService = rentalService;
            _userService = userService;
            _bookingService = bookingService;
        }

        // List of all the rentals available
        public IActionResult Index(DateTime? fromDate, DateTime? toDate, string? city)
        {
            string? username = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Name)?.Value;
            List<RentalModel> rentals = [];

            if (username == null)
            {
                rentals = _locationService.GetAvailableRentals(fromDate, toDate, city);
            } else
            {
                rentals = _locationService.GetAllAvailableRentalsAndUserBooking(username!, fromDate, toDate, city);
            }

             return View(rentals);
        }

        public IActionResult Detail(int id)
        {
            RentalModel? rental = _rentalService.Get(id);

            if (rental is null)
            {
                return RedirectToAction("Index", "StatusCode", StatusCodes.Status404NotFound);
            }

            // Gives to the View two booleans to determine the value/redirection of the book button:
            // isLoggedIn: the user is logged in or not
            // isOwner: the logged in user is the owner of the rental
            string? username = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Name)?.Value;
            bool isLoggedIn = User.Identity != null && User.Identity.IsAuthenticated;

            int userId = _userService.GetIdFromUsername(username);

            bool isOwner = rental.UserId == userId;

            ViewData["IsOwner"] = isOwner;
            ViewData["IsLoggedIn"] = isLoggedIn;

            List<Booking> bookings = _bookingService.GetBookingsByRentalId(id);

            List<string> unavailableDates = _locationService.GetUnavailableDates(bookings);

            ViewData["UnavailableDates"] = unavailableDates;

            return View(rental);
        }
    }
}
