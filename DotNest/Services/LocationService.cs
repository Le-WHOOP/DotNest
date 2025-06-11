using DotNest.DataAccess.Interfaces;
using DotNest.DataAccess.Entities;
using DotNest.Services.Mapper;
using DotNest.Models;
using DotNest.Services.Interfaces;

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

        public List<RentalModel> GetAvailableRentals(DateTime? fromDate, DateTime? toDate, string? city)
        {
            List<Rental> rentals = _rentalRepository.Get();

            DateOnly from = fromDate.HasValue ? DateOnly.FromDateTime(fromDate.Value) : DateOnly.MinValue;
            DateOnly to = toDate.HasValue ? DateOnly.FromDateTime(toDate.Value) : from;

            if (!string.IsNullOrEmpty(city))
            {
                rentals = rentals.Where(r => r.City.ToLower().Equals(city.ToLower())).ToList();
            }

            if (from == DateOnly.MinValue)
            {
                return RentalMapper.MapToModel(rentals);
            }

            List<Rental> rentalsFiltered = rentals
                .Where(r =>
                    !r.Bookings.Any(b =>
                        (b.FromDate.CompareTo(from) >= 0 && b.ToDate.CompareTo(to) <= 0) ||
                        (b.ToDate.CompareTo(from) >= 0 && b.ToDate.CompareTo(to) <= 0) ||
                        (b.FromDate.CompareTo(from) <= 0 && b.ToDate.CompareTo(to) >= 0)
                    )
                )
                .ToList();

            return RentalMapper.MapToModel(rentalsFiltered);
        }



        public List<RentalModel> GetAllAvailableRentalsAndUserBooking(
            string username,
            DateTime? fromDate,
            DateTime? toDate,
            string? city
            )
        {
            User user = _userRepository.GetByUsername(username)!;

            // Get the From date of the filter. If there is nothing, it is the date of today.
            DateOnly from = fromDate.HasValue ? DateOnly.FromDateTime(fromDate.Value) : DateOnly.MinValue;
            // Get the To date of the filter. If there is nothing, then it is the same as the start.
            DateOnly to = toDate.HasValue ? DateOnly.FromDateTime(toDate.Value) : from;

            // Get all the rentals that are not the users
            List<Rental> allRentals = _rentalRepository.Get().Where(r => r.UserId != user.Id).ToList();

            // Filters with the given city
            if (!string.IsNullOrEmpty(city))
            {
                allRentals = allRentals.Where(r => r.City.ToLower().Equals(city.ToLower())).ToList();
            }

            // No starting date = we return all the rentals
            if (from == DateOnly.MinValue)
            {
                return RentalMapper.MapToModel(allRentals);
            }

            // Here is the logic. We want the all the rentals except:
            // - The ones that are already booked on the date when the user checks the page
            List<Rental> rentals = allRentals
                .Where(r =>
                    !r.Bookings.Any(b =>
                        (b.FromDate.CompareTo(from) >= 0 && b.ToDate.CompareTo(to) <= 0) ||
                        (b.ToDate.CompareTo(from) >= 0 && b.ToDate.CompareTo(to) <= 0) ||
                        (b.FromDate.CompareTo(from) <= 0 && b.ToDate.CompareTo(to) >= 0)
                    )
                )   
                .ToList();

            return RentalMapper.MapToModel(rentals);
        }
    }
}
