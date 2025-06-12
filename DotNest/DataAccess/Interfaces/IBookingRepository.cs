using DotNest.DataAccess.Entities;

namespace DotNest.DataAccess.Interfaces
{
    public interface IBookingRepository
    {
        public List<Booking> GetByUser(int userId);
        public Booking? Get(int id);
        public void Create(Booking booking);
        public void Delete(Booking booking);
        public List<Booking> GetWithOverlappingDates(DateOnly from, DateOnly to);
    }
}
