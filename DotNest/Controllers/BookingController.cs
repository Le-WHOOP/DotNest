using DotNest.DataAccess.Entities;
using DotNest.Models;
using DotNest.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DotNest.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IRentalService _rentalService;
        private readonly IHttpContextAccessor _contextAccessor;


        public BookingController(IBookingService bookingService, IRentalService rentalService, IHttpContextAccessor contextAccessor)
        {
            _bookingService = bookingService;
            _rentalService = rentalService;
            _contextAccessor = contextAccessor;
        }

        public IActionResult Index()
        {
            string? username = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Name)?.Value;
            if (username is null)
                return RedirectToAction("Login", "User");

            List<BookingModel> bookings = _bookingService.GetAllBookingsFromUser(username!);
            return View(bookings);
        }

        public IActionResult Delete(int id)
        {
            string? username = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Name)?.Value;
            if (username is null)
                return RedirectToAction("Login", "User");

            _bookingService.DeleteBooking(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Book(int id)
        {
            string? username = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Name)?.Value;
            if (username is null)
                return RedirectToAction("Login", "User");

            ViewData["rentalId"] = id;
            RentalModel? rental = _rentalService.GetRental(id);

            if (rental is null)
            {
                return RedirectToAction("StatusCode", "Index", StatusCodes.Status404NotFound);
            }

            ViewData["rentalName"] = rental.Name;
            return View();
        }

        [HttpPost]
        public IActionResult Book(BookingModel booking)
        {
            string? username = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Name)?.Value;
            if (username is null)
                return RedirectToAction("Login", "User");

            booking.Id = null; // The path param id corresponding to the rental id is wrongly assigned as the booking id, so we reset booking.Id
            if (!ModelState.IsValid)
                return View();

            ViewData["rentalId"] = booking.RentalId;
            ViewData["rentalName"] = booking.RentalName;


            try
            {
                _bookingService.BookRental(username, booking);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("FromDate", ex.Message); // show an error message under the "from" field
                return View(booking); // show the view with the submitted info
            }
        }
    }
}
