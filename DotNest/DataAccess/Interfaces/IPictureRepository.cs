using DotNest.DataAccess.Entities;

namespace DotNest.DataAccess.Interfaces
{
    public interface IPictureRepository
    {
        public Picture? Get(int id);
        public Picture Create(Picture picture);
        public void Update(Picture picture);
    }
}
