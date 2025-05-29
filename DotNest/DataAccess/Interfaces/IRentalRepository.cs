using DotNest.DataAccess.Entities;

namespace DotNest.DataAccess.Interfaces
{
    public interface IRentalRepository
    {
        public List<Rental> Get();
        public Rental? Get(int id);

        public List<Rental> GetByUser(int userId);
        public void Create(Rental rental);
        public void Update(Rental rental);
    }
}
