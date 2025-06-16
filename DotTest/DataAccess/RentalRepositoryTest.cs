using DotNest.DataAccess.Entities;
using DotNest.DataAccess.Repositories;

namespace DotTest;

public class RentalRepositoryTest
{
    private RentalRepository _rentalRepository;
    private Rental[] _rentalsData;

    // Used to reset the data at each test
    private void InitTest()
    {
        var mockData = new MockData();
        _rentalRepository = mockData.RentalRepository;
        _rentalsData = mockData.RentalsData;
    }

    [Fact]
    public void Get_NoId()
    {
        InitTest();

        List<Rental> actualRentals = _rentalRepository.Get();

        Assert.Multiple(() =>
        {
            Assert.Equal(_rentalsData.Length, actualRentals.Count);

            List<Rental> orderedRentals = actualRentals.OrderBy(r => r.Id).ToList();
            for (int index = 0; index < orderedRentals.Count; index++)
            {
                Assert.Equal(_rentalsData[index].Id, orderedRentals[index].Id);
            }
        });
    }

    [Fact]
    public void Get_ValidId()
    {
        InitTest();

        int id = 1;
        Rental expectedRental = _rentalsData[0];
        Rental? actualRental = _rentalRepository.Get(id);

        Assert.Multiple(() =>
        {
            Assert.NotNull(actualRental);
            Assert.Equal(id, actualRental.Id);

            // Basics
            Assert.Equal(expectedRental.Id, actualRental.Id);
            Assert.Equal(expectedRental.PictureId, actualRental.PictureId);
            Assert.Equal(expectedRental.UserId, actualRental.UserId);
            Assert.Equal(expectedRental.Name, actualRental.Name);
            Assert.Equal(expectedRental.Description, actualRental.Description);
            Assert.Equal(expectedRental.City, actualRental.City);

            // Bookings
            Assert.Equal(expectedRental.Bookings.Count, actualRental.Bookings.Count);

            List<Booking> orderedBookings = actualRental.Bookings.OrderBy(b => b.Id).ToList();
            for (int index = 0; index < orderedBookings.Count; index++)
            {
                Assert.Equal(expectedRental.Bookings.ElementAt(index).Id, orderedBookings[index].Id);
            }

            // Other virtual properties
            Assert.NotNull(actualRental.Picture);
            Assert.Equal(expectedRental.Picture!.Id, actualRental.Picture.Id);

            Assert.Equal(expectedRental.User.Id, actualRental.User.Id);
        });
    }

    [Fact]
    public void Get_InvalidId()
    {
        InitTest();

        int id = -1;
        Rental? actualRental = _rentalRepository.Get(id);

        Assert.Null(actualRental);
    }

    [Fact]
    public void GetByUser_ValidUserId()
    {
        InitTest();

        int userId = 1;

        List<Rental> expectedRentals = _rentalsData.Where(r => r.UserId == userId).OrderBy(r => r.Id).ToList();
        List<Rental> actualRentals = _rentalRepository.GetByUser(userId);

        Assert.Multiple(() =>
        {
            Assert.Equal(expectedRentals.Count, actualRentals.Count);

            List<Rental> orderedRentals = actualRentals.OrderBy(r => r.Id).ToList();
            for (int index = 0; index < orderedRentals.Count; index++)
            {
                Assert.Equal(expectedRentals[index].Id, orderedRentals[index].Id);
            }
        });
    }

    [Fact]
    public void GetByUser_InvalidUserId()
    {
        InitTest();

        int userId = -1;

        List<Rental> actualRentals = _rentalRepository.GetByUser(userId);

        Assert.Empty(actualRentals);
    }
}
