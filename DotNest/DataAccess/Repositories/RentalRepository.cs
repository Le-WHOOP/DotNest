using DotNest.DataAccess.Entities;
using DotNest.DataAccess.Interfaces;

namespace DotNest.DataAccess.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly DotNestContext _dbContext;

        public RentalRepository(DotNestContext dbContext)
        {
            _dbContext = dbContext;
        }


        public List<Rental> Get()
        {
            return _dbContext.Rentals.ToList();
        }

        public Rental? Get(int id)
        {
            return _dbContext.Rentals.FirstOrDefault(rental => rental.Id == id);
        }


        public List<Rental> GetByUser(int userId)
        {
            return _dbContext.Rentals.Where(rental => rental.UserId == userId).ToList();
        }


        public void Create(Rental rental)
        {
            _dbContext.Rentals.Add(rental);
            _dbContext.SaveChanges();
        }

        public void Update(Rental rental)
        {
            _dbContext.Rentals.Update(rental);
            _dbContext.SaveChanges();
        }
    }
}
