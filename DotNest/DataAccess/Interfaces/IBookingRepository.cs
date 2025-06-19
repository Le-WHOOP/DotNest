using DotNest.DataAccess.Entities;

namespace DotNest.DataAccess.Interfaces
{
    public interface IBookingRepository
    {
        public List<Booking> GetByUser(int userId);
        public Booking? Get(int id);
        public List<Booking> GetAll();
        public void Create(Booking booking);
        public void Delete(Booking booking);
        public List<Booking> GetWithOverlappingDates(int rentalId, DateOnly from, DateOnly to);
    }
}
