using DotNest.DataAccess.Entities;
using DotNest.DataAccess.Interfaces;
using DotNest.Models;
using DotNest.Services.Interfaces;
using DotNest.Services.Mapper;
using Humanizer;

namespace DotNest.Services
{
    public class LocationService : ILocationService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBookingRepository _bookingRepository;

        public LocationService(
            IRentalRepository rentalRepository,
            IUserRepository userRepository,
            IBookingRepository bookingRepository
            )
        {
            _rentalRepository = rentalRepository;
            _userRepository = userRepository;
            _bookingRepository = bookingRepository;
        }

        public List<RentalModel> GetAllAvailableRentals(DateTime? fromDate, DateTime? toDate, string? city)
        {
            // Get all the rentals (the user is not logged in so we include every rental
            List<Rental> allRentals = _rentalRepository.Get();

            return GetFilteredAndMappedRentals(allRentals, fromDate, toDate, city);
        }



        public List<RentalModel> GetAllAvailableRentalsAndUserBooking(
            string username,
            DateTime? fromDate,
            DateTime? toDate,
            string? city
            )
        {
            User user = _userRepository.GetByUsername(username)!;

            // Get all the rentals that are not the users
            List<Rental> allRentals = _rentalRepository.Get().Where(r => r.UserId != user.Id).ToList();

            return GetFilteredAndMappedRentals(allRentals, fromDate, toDate, city);
        }

        private List<RentalModel> GetFilteredAndMappedRentals(
            List<Rental> rentals,
            DateTime? fromDate,
            DateTime? toDate,
            string? city
            )
        {
            // Get the From date of the filter. If there is nothing, it is the date of today.
            DateOnly from = fromDate.HasValue ? DateOnly.FromDateTime(fromDate.Value) : DateOnly.MinValue;
            // Get the To date of the filter. If there is nothing, then it is the same as the start.
            DateOnly to = toDate.HasValue ? DateOnly.FromDateTime(toDate.Value) : from;

            // Filters with the given city
            if (!string.IsNullOrEmpty(city))
            {
                rentals = rentals.Where(r =>
                    string.Equals(r.City, city, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            // No starting date = we return all the rentals
            if (from == DateOnly.MinValue)
            {
                return RentalMapper.MapToModel(rentals);
            }

            var filteredRentals = GetFilteredRentals(rentals, from, to);

            return RentalMapper.MapToModel(filteredRentals);
        }

        private List<Rental> GetFilteredRentals(List<Rental> allRentals, DateOnly from, DateOnly to)
        {
            // Here is the logic. We want the all the rentals except
            // the ones that are already booked on the date when the user checks the page
            return allRentals
                .Where(r =>
                    !r.Bookings.Any(b =>
                        (b.FromDate.CompareTo(from) >= 0 && b.ToDate.CompareTo(to) <= 0) ||
                        (b.ToDate.CompareTo(from) >= 0 && b.ToDate.CompareTo(to) <= 0) ||
                        (b.FromDate.CompareTo(from) <= 0 && b.ToDate.CompareTo(to) >= 0)
                    )
                )
                .ToList();
        }

        public List<string> GetUnavailableDates(List<BookingModel> bookings)
        {
            // For each booking, generate a list of all dates (inclusive) between FromDate and ToDate.
            // Then flatten the result into a single list of strings formatted as "yyyy-MM-dd".
            // This is used for marking unavailable dates on the calendar (disabling already booked days).
            return bookings
                .SelectMany(b =>
                    Enumerable.Range(0, (b.ToDate.DayNumber - b.FromDate.DayNumber + 1)) // number of days in booking
                              .Select(offset => b.FromDate.AddDays(offset)               // shift by offset days
                                                      .ToDateTime(TimeOnly.MinValue)    // convert to DateTime at 00:00
                                                      .ToString("yyyy-MM-dd"))          // format as string
                ).ToList();
        }
    }
}
