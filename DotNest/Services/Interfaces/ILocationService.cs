using DotNest.DataAccess.Entities;
using DotNest.Models;

namespace DotNest.Services.Interfaces
{
    public interface ILocationService
    {
        public List<RentalModel> GetAllAvailableRentals(DateTime? fromDate, DateTime? toDate, string? city);
        public List<RentalModel> GetAllAvailableRentalsAndUserBooking(string username, DateTime? fromDate, DateTime? toDate, string? city);
        public List<string> GetUnavailableDates(List<BookingModel> bookings);
    }
}
