using DotNest.Models;
using DotNest.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace DotNest.Controllers
{
    [Authorize]
    public class RentalController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;

        private readonly IRentalService _rentalService;

        private static readonly Regex CITY_REGEX = new Regex("((\\p{L})+[- ]?)+");

        public RentalController(IHttpContextAccessor contextAccessor, IRentalService userService)
        {
            _contextAccessor = contextAccessor;
            _rentalService = userService;
        }



        // list of your rentals
        public IActionResult Index()
        {
            string? username = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Name)?.Value;
            if (username is null)
                RedirectToAction("User", "Login");

            List<RentalItemListModel> rentals = _rentalService.GetAllRentalItemsOf(username!);

            return View(rentals);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(RentalModel rental)
        {
            if (!ModelState.IsValid)
                return View();

            if (!CITY_REGEX.IsMatch(rental.City))
            {
                ModelState.AddModelError("City", "The city can only contain letters, - and spaces"); // show an error message under the username field
                return View(rental);
            }

            if (rental.Picture is null)
            {
                ModelState.AddModelError("Picture", "A picture must be added");
                return View(rental);
            }

            try
            {
                string? username = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Name)?.Value;

                _rentalService.CreateRental(username, rental);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Name", ex.Message); // show an error message under the username field
                return View(rental); // show the view with the submitted info
            }
        }

        public IActionResult Update(int id)
        {
            RentalModel? model = _rentalService.Get(id);

            if (model is null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        public IActionResult Update(int id, RentalModel rental)
        {
            if (!ModelState.IsValid)
                return View(rental);

            if (!CITY_REGEX.IsMatch(rental.City))
            {
                ModelState.AddModelError("City", "The city can only contain letters, - and spaces"); // show an error message under the username field
                return View(rental);
            }

            try
            {
                string? username = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Name)?.Value;
                rental.Id = id;
                _rentalService.UpdateRental(rental);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Name", ex.Message); // show an error message under the username field
                return View(rental); // show the view with the submitted info
            }
        }
    }
}
