using DotNest.DataAccess.Entities;
using DotNest.DataAccess.Interfaces;

namespace DotNest.DataAccess.Repositories
{
    public class PictureRepository : IPictureRepository
    {
        private readonly DotNestContext _dbContext;

        public PictureRepository(DotNestContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Picture? Get(int id)
        {
            return _dbContext.Pictures.FirstOrDefault(picture => picture.Id == id);
        }

        public Picture Create(Picture picture)
        {
            _dbContext.Pictures.Add(picture);
            _dbContext.SaveChanges();

            return picture;
        }

        public void Update(Picture picture)
        {
            _dbContext.Pictures.Update(picture);
            _dbContext.SaveChanges();
        }
    }
}
