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

        public void Create(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        public User? GetByEmail(string email)
        {
            return _dbContext.Users.FirstOrDefault(user => user.Email == email);
        }

        public User? GetByUsername(string username)
        {
            return _dbContext.Users.FirstOrDefault(user => user.Username == username);
        }
    }
}
