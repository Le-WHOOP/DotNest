using DotNest.DataAccess.Entities;
using DotNest.DataAccess.Interfaces;

namespace DotNest.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DotNestContext _dbContext;

        public UserRepository(DotNestContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User? GetByUsername(string username)
        {
            return _dbContext.Users.FirstOrDefault(user => user.Username == username);
        }
    }
}
