using DotNest.DataAccess.Entities;
using DotNest.DataAccess.Interfaces;
using DotNest.DataAccess.Repositories;

namespace DotTest;

public class BookingRepositoryTest
{
    private BookingRepository _bookingRepository;
    private Booking[] _bookingsData;

    // Used to reset the data at each test
    private void InitTest()
    {
        var mockData = new MockData();
        _bookingRepository = mockData.BookingRepository;
        _bookingsData = mockData.BookingsData;
    }

    [Fact]
    public void Get_ValidId()
    {
        InitTest();

        int id = 1;
        Booking expectedBooking = _bookingsData[0];
        Booking? actualBooking = _bookingRepository.Get(id);

        Assert.Multiple(() =>
        {
            Assert.NotNull(actualBooking);
            Assert.Equal(id, actualBooking.Id);

            // Basics
            Assert.Equal(expectedBooking.Id, actualBooking.Id);
            Assert.Equal(expectedBooking.UserId, actualBooking.UserId);
            Assert.Equal(expectedBooking.RentalId, actualBooking.RentalId);
            Assert.Equal(expectedBooking.FromDate, actualBooking.FromDate);
            Assert.Equal(expectedBooking.ToDate, actualBooking.ToDate);

            // Virtual properties
            Assert.Equal(expectedBooking.Rental.Id, actualBooking.Rental.Id);
            Assert.Equal(expectedBooking.User.Id, actualBooking.User.Id);
        });
    }

    [Fact]
    public void Get_InvalidId()
    {
        InitTest();

        int id = -1;
        Booking? actualBooking = _bookingRepository.Get(id);

        Assert.Null(actualBooking);
    }

    [Fact]
    public void GetAll_Success()
    {
        InitTest();

        List<Booking> actualBookings = _bookingRepository.GetAll();

        Assert.Multiple(() =>
        {
            Assert.Equal(_bookingsData.Length, actualBookings.Count);

            List<Booking> orderedBookings = actualBookings.OrderBy(r => r.Id).ToList();
            for (int index = 0; index < orderedBookings.Count; index++)
            {
                Assert.Equal(_bookingsData[index].Id, orderedBookings[index].Id);
            }
        });
    }

    [Fact]
    public void GetByUser_ValidUserId()
    {
        InitTest();

        int userId = 1;

        List<Booking> expectedBookings = _bookingsData.Where(r => r.UserId == userId).OrderBy(r => r.Id).ToList();
        List<Booking> actualBookings = _bookingRepository.GetByUser(userId);

        Assert.Multiple(() =>
        {
            Assert.Equal(expectedBookings.Count, actualBookings.Count);

            List<Booking> orderedBookings = actualBookings.OrderBy(r => r.Id).ToList();
            for (int index = 0; index < orderedBookings.Count; index++)
            {
                Assert.Equal(expectedBookings[index].Id, orderedBookings[index].Id);
            }
        });
    }

    [Fact]
    public void GetByUser_InvalidUserId()
    {
        InitTest();

        int userId = -1;

        List<Booking> actualBookings = _bookingRepository.GetByUser(userId);

        Assert.Empty(actualBookings);
    }

    [Fact]
    public void GetWithOverlappingDates_EmptyWithDifferentFromAndTo()
    {
        InitTest();

        DateOnly from = new DateOnly(2025, 8, 8);
        DateOnly to = new DateOnly(2025, 8, 12);

        List<Booking> actualBookings = _bookingRepository.GetWithOverlappingDates(from, to);

        Assert.Empty(actualBookings);
    }

    [Fact]
    public void GetWithOverlappingDates_EmptyWithSameFromAndTo()
    {
        InitTest();

        DateOnly from = new DateOnly(2025, 7, 10);
        DateOnly to = from;

        List<Booking> actualBookings = _bookingRepository.GetWithOverlappingDates(from, to);

        Assert.Empty(actualBookings);
    }

    [Fact]
    public void GetWithOverlappingDates_BookingSurroundingOverlap()
    {
        InitTest();

        DateOnly from = new DateOnly(2025, 7, 3);
        DateOnly to = new DateOnly(2025, 7, 5);

        Booking expectedBooking = _bookingsData[0];
        List<Booking> actualBookings = _bookingRepository.GetWithOverlappingDates(from, to);

        Assert.Multiple(() =>
        {
            Assert.Single(actualBookings);

            Assert.Equal(expectedBooking.Id, actualBookings[0].Id);
        });
    }

    [Fact]
    public void GetWithOverlappingDates_BookingIncludedInOverlap()
    {
        InitTest();

        DateOnly from = new DateOnly(2025, 6, 3);
        DateOnly to = new DateOnly(2025, 7, 10);

        Booking expectedBooking = _bookingsData[0];
        List<Booking> actualBookings = _bookingRepository.GetWithOverlappingDates(from, to);

        Assert.Multiple(() =>
        {
            Assert.Single(actualBookings);

            Assert.Equal(expectedBooking.Id, actualBookings[0].Id);
        });
    }

    [Fact]
    public void GetWithOverlappingDates_SeveralBookingsOverlap()
    {
        InitTest();

        DateOnly from = new DateOnly(2025, 7, 8);
        DateOnly to = new DateOnly(2025, 7, 14);

        List<Booking> expectedBookings = [_bookingsData[0], _bookingsData[1], _bookingsData[3]];
        List<Booking> actualBookings = _bookingRepository.GetWithOverlappingDates(from, to);

        Assert.Multiple(() =>
        {
            Assert.Equal(expectedBookings.Count, actualBookings.Count);

            List<Booking> orderedBookings = actualBookings.OrderBy(r => r.Id).ToList();
            for (int index = 0; index < orderedBookings.Count; index++)
            {
                Assert.Equal(expectedBookings[index].Id, orderedBookings[index].Id);
            }
        });
    }
}
