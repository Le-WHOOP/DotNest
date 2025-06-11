using DotNest.DataAccess.Entities;

namespace DotNest.DataAccess.Interfaces
{
    public interface IBookingRepository
    {
        public List<Booking> GetBookingsByUser(int userId);
        public Booking? Get(int id);
        public List<Booking> GetAll();
        public void Create(Booking booking);
        public void Delete(Booking booking);
        public List<Booking> GetBookingsIncluding(DateOnly from, DateOnly to);
    }
}
