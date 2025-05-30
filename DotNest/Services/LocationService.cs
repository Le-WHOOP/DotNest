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

        public LocationService(IRentalRepository rentalRepository, IUserRepository userRepository)
        {
            _rentalRepository = rentalRepository;
            _userRepository = userRepository;
        }

        public List<RentalModel> GetAvailableRentals()
        {
            // TODO: add the booking logic when it is finished
            List<Rental> rentals = _rentalRepository.Get();

            return RentalMapper.MapToModel(rentals);
        }

        public List<RentalModel> GetAllAvailableRentalsAndUserBooking(string username)
        {
            User user = _userRepository.GetByUsername(username)!;

            // TODO: add the booking logic when it is finished
            List<Rental> rentals = _rentalRepository.Get();

            return RentalMapper.MapToModel(rentals);
        }
    }
}
