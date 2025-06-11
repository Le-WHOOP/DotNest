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

        public LocationService(IRentalRepository rentalRepository, IUserRepository userRepository, IBookingRepository bookingRepository)
        {
            _rentalRepository = rentalRepository;
            _userRepository = userRepository;
            _bookingRepository = bookingRepository;
        }

        public List<RentalModel> GetAvailableRentals(DateTime? fromDate, DateTime? toDate)
        {
            List<Rental> rentals = _rentalRepository.Get();
            List<Booking> bookings = _bookingRepository.GetAll();

            DateOnly from = fromDate.HasValue ? DateOnly.FromDateTime(fromDate.Value) : DateOnly.FromDateTime(DateTime.Today);
            DateOnly to = toDate.HasValue ? DateOnly.FromDateTime(toDate.Value) : DateOnly.MaxValue;

            var bookedRentalIds = bookings
                .Where(b => b.FromDate < to && b.ToDate > from)
                .Select(b => b.RentalId)
                .ToHashSet();

            var availableRentals = rentals
                .Where(r => !bookedRentalIds.Contains(r.Id))
                .ToList();

            return RentalMapper.MapToModel(availableRentals);
        }



        public List<RentalModel> GetAllAvailableRentalsAndUserBooking(string username, DateTime? fromDate, DateTime? toDate)
        {
            User user = _userRepository.GetByUsername(username)!;

            // Get the From date of the filter. If there is nothing, it is the date of today.
            DateOnly from = fromDate.HasValue ? DateOnly.FromDateTime(fromDate.Value) : DateOnly.FromDateTime(DateTime.Today);
            // Get the To date of the filter. If there is nothing, then it is the max value a DateOnly can have.
            DateOnly to = toDate.HasValue ? DateOnly.FromDateTime(toDate.Value) : DateOnly.MaxValue;

            // Get all the rentals
            List<Rental> allRentals = _rentalRepository.Get().Where(r => r.UserId != user.Id).ToList();

            // Here is the logic. We want the all the rentals except:
            // - The ones that where made by the user (he's the owner of the rentals)
            // - The ones that are already booked on the date where the user checks the page
            List<Rental> rentals = allRentals
                .Where(r =>
                    r.Bookings.Any(b =>
                        b.UserId != user.Id &&
                        (
                            (b.FromDate.CompareTo(from) >= 0 && b.FromDate.CompareTo(to) <= 0) ||
                            (b.ToDate.CompareTo(from) >= 0 && b.ToDate.CompareTo(to) <= 0) ||
                            (b.FromDate.CompareTo(from) <= 0 && b.ToDate.CompareTo(to) >= 0)
                        )
                    )
                )   
                .ToList();

            return RentalMapper.MapToModel(rentals);
        }
    }
}
