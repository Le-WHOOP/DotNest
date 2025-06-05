using DotNest.Models;

namespace DotNest.Services.Interfaces
{
    public interface IBookingService
    {
        public List<BookingModel> GetAllBookingsFromUser(string username);
        public void BookRental(string username, BookingModel booking);
        public void DeleteBooking(int id);
    }
}
