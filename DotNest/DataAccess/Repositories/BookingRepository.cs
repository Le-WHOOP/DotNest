using DotNest.DataAccess.Entities;
using DotNest.DataAccess.Interfaces;

namespace DotNest.DataAccess.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly DotNestContext _dbContext;

        public BookingRepository(DotNestContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Booking? Get(int id)
        {
            return _dbContext.Bookings.FirstOrDefault(booking => booking.Id == id);
        }

        public List<Booking> GetAll()
        {
            return _dbContext.Bookings.ToList();
        }

        public List<Booking> GetBookingsByUser(int userId)
        {
            return _dbContext.Bookings.Where(booking => booking.UserId == userId)/*.OrderBy(booking => booking.FromDate)*/.ToList();
        }

        public void Create(Booking booking)
        {
            _dbContext.Bookings.Add(booking);
            _dbContext.SaveChanges();
        }

        public void Delete(Booking booking)
        {
            _dbContext.Bookings.Remove(booking);
            _dbContext.SaveChanges();
        }

        public List<Booking> GetBookingsIncluding(DateOnly from, DateOnly to)
        {
            return _dbContext.Bookings.Where(booking => 
                (booking.ToDate > from && booking.ToDate < to) // the booking ends between "from" and "to"
                || (booking.FromDate > from && booking.FromDate < to) // the booking starts between "from" and "to"
            ).ToList();
        }
    }
}
