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
            User? user = _userRepository.GetByUsername(model.Username);

            if (user is null)
                return null;

            var salt = Convert.FromBase64String(user.PasswordSalt);
            var b64Hash = GenerateHash(model.Password, salt);

            if (user.HashedPassword != b64Hash)
                return null;

            return user.Username;
        }

        public void RegisterUser(RegisterModel model)
        {
            // check if there is a user with the same username or email => throw invalidOpExp
            if (_userRepository.GetByUsername(model.Username) is not null)
            {
                throw new InvalidOperationException("The username already exists");
            }
            if (_userRepository.GetByEmail(model.Email) is not null)
            {
                throw new InvalidOperationException("The email already exists");
            }

            // Password: Salt and hash password
            (byte[] salt, string b64Salt) = GenerateSalt();

            string b64Hash = GenerateHash(model.Password, salt);


            var newUser = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordSalt = b64Salt,
                HashedPassword = b64Hash
            };
            _userRepository.Create(newUser);

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
