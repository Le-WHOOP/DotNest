﻿using DotNest.DataAccess.Entities;
using DotNest.DataAccess.Repositories;

namespace DotTest;

public class UserRepositoryTest
{
    private readonly UserRepository _userRepository;
    private readonly List<User> _userData;

    public UserRepositoryTest()
    {
        MockData mockData = new();
        _userRepository = mockData.UserRepository;
        _userData = mockData.UserData;
    }

    [Fact]
    public void GetByEmail_ValidEmail()
    {
        string email = "user1@gmail.com";
        User expectedUser = _userData[0];
        User? actualUser = _userRepository.GetByEmail(email);

        Assert.Multiple(() =>
        {
            Assert.NotNull(actualUser);
            Assert.Equal(email, actualUser.Email);

            // Basics
            Assert.Equal(expectedUser.Id, actualUser.Id);
            Assert.Equal(expectedUser.Username, actualUser.Username);
            Assert.Equal(expectedUser.Email, actualUser.Email);
            Assert.Equal(expectedUser.HashedPassword, actualUser.HashedPassword);
            Assert.Equal(expectedUser.PasswordSalt, actualUser.PasswordSalt);

            // Bookings
            Assert.Equal(expectedUser.Bookings.Count, actualUser.Bookings.Count);

            List<Booking> orderedBookings = actualUser.Bookings.OrderBy(b => b.Id).ToList();
            for (int index = 0; index < orderedBookings.Count; index++)
            {
                Assert.Equal(expectedUser.Bookings.ElementAt(index).Id, orderedBookings[index].Id);
            }

            // Rentals
            Assert.Equal(expectedUser.Rentals.Count, actualUser.Rentals.Count);

            List<Rental> orderedRentals = actualUser.Rentals.OrderBy(r => r.Id).ToList();
            for (int index = 0; index < orderedRentals.Count; index++)
            {
                Assert.Equal(expectedUser.Rentals.ElementAt(index).Id, orderedRentals[index].Id);
            }
        });
    }

    [Fact]
    public void GetByEmail_InvalidEmail()
    {
        string email = "invalid@gmail.com";
        User? actualUser = _userRepository.GetByEmail(email);

        Assert.Null(actualUser);
    }

    [Fact]
    public void GetByUsername_ValidUsername()
    {
        string username = "user1";
        User expectedUser = _userData[0];
        User? actualUser = _userRepository.GetByUsername(username);

        Assert.Multiple(() =>
        {
            Assert.NotNull(actualUser);
            Assert.Equal(username, actualUser.Username);

            // Basics
            Assert.Equal(expectedUser.Id, actualUser.Id);
            Assert.Equal(expectedUser.Username, actualUser.Username);
            Assert.Equal(expectedUser.Email, actualUser.Email);
            Assert.Equal(expectedUser.HashedPassword, actualUser.HashedPassword);
            Assert.Equal(expectedUser.PasswordSalt, actualUser.PasswordSalt);

            // Bookings
            Assert.Equal(expectedUser.Bookings.Count, actualUser.Bookings.Count);

            List<Booking> orderedBookings = actualUser.Bookings.OrderBy(b => b.Id).ToList();
            for (int index = 0; index < orderedBookings.Count; index++)
            {
                Assert.Equal(expectedUser.Bookings.ElementAt(index).Id, orderedBookings[index].Id);
            }

            // Rentals
            Assert.Equal(expectedUser.Rentals.Count, actualUser.Rentals.Count);

            List<Rental> orderedRentals = actualUser.Rentals.OrderBy(r => r.Id).ToList();
            for (int index = 0; index < orderedRentals.Count; index++)
            {
                Assert.Equal(expectedUser.Rentals.ElementAt(index).Id, orderedRentals[index].Id);
            }
        });
    }

    [Fact]
    public void GetByUsername_InvalidUsername()
    {
        string username = "invalid";
        User? actualUser = _userRepository.GetByUsername(username);

        Assert.Null(actualUser);
    }
}
