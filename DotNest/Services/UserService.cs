using DotNest.DataAccess.Entities;
using DotNest.DataAccess.Interfaces;
using DotNest.Models;
using DotNest.Services.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace DotNest.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;


        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        // TODO inject repository
        public string? GetUserFromLogin(LoginModel model)
        {
            // get user with username
            // if null => return null

            //var salt = Convert.FromBase64String(dbUser.PwdSalt);
            //var b64Hash = GenerateHash(password, salt);

            //if (dbUser.PwdHash != b64Hash)
                return null;

            // return the username
        }

        public void RegisterUser(RegisterModel model)
        {
            // check if there is a user with the same username or email => throw invalidOpExp

            // like below but in a better way
            //if (dbUsers.Any(x => x.Username.Equals(normalizedUsername)))
            //    throw new InvalidOperationException("Username already exists");
            //if (dbUsers.Any(x => x.Email.Equals(normalizedEmail)))
            //    throw new InvalidOperationException("Email already exists");

            // Password: Salt and hash password
            (byte[] salt, string b64Salt) = GenerateSalt();

            string b64Hash = GenerateHash(model.Password, salt);

            /*
            var newUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                PwdSalt = b64Salt,
                PwdHash = b64Hash
            };
            */
        }


        public User? Test(string v)
        {
            return _userRepository.GetByUsername(v);
        }


        #region Security related functions

        /// <summary>
        /// generate a hash from the password and the salt
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private static string GenerateHash(string password, byte[] salt)
        {
            byte[] hash =
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8);
            string b64Hash = Convert.ToBase64String(hash);

            return b64Hash;
        }

        /// <summary>
        /// Generate a random salt
        /// </summary>
        /// <returns></returns>
        private static (byte[], string) GenerateSalt()
        {
            var salt = RandomNumberGenerator.GetBytes(128 / 8);
            var b64Salt = Convert.ToBase64String(salt);

            return (salt, b64Salt);
        }

        #endregion
    }
}
