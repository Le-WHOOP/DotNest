using DotNest.Models;

namespace DotNest.Services.Interfaces
{
    public interface ILocationService
    {
        public int GetIdFromUsername(string? username);
        public List<RentalModel> GetAvailableRentals();
        public List<RentalModel> GetAllAvailableRentalsAndUserBooking(string username);
    }
}
