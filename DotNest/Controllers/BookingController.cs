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
                RedirectToAction("User", "Login");

            List<BookingModel> bookings = _bookingService.GetAllBookingsFromUser(username!);
            return View(bookings);
        }

        public IActionResult Delete(int id)
        {
            _bookingService.DeleteBooking(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Book(int id)
        {
            ViewData["rentalId"] = id;
            RentalModel? rental = _rentalService.Get(id);

            if (rental is null)
            {
                return RedirectToAction("StatusCode", "Index", StatusCodes.Status404NotFound);
            }

            ViewData["rentalName"] = rental.Name;
            return View();
        }

        [HttpPost]
        public IActionResult Book(BookingModel model)
        {
            model.Id = null; // since it has the rental id as path param and that it must be named id, it assigns it to this attributs, which has the same name
            if (!ModelState.IsValid)
                return View();

            ViewData["rentalId"] = model.RentalId;
            ViewData["rentalName"] = model.RentalName;


            string? username = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Name)?.Value;

            if (username == null)
            {
                return RedirectToAction("User", "Login");
            }

            try
            {
                _bookingService.BookRental(username, model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("FromDate", ex.Message); // show an error message under the "from" field
                return View(model); // show the view with the submitted info
            }
        }
    }
}
