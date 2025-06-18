using DotNest.DataAccess.Entities;
using DotNest.DataAccess.Repositories;

namespace DotTest;

public class BookingRepositoryTest
{
    private readonly BookingRepository _bookingRepository;
    private readonly List<Booking> _bookingData;

    public BookingRepositoryTest()
    {
        MockData mockData = new();
        _bookingRepository = mockData.BookingRepository;
        _bookingData = mockData.BookingData;
    }

    [Fact]
    public void Get_ValidId()
    {
        int id = 1;
        Booking expectedBooking = _bookingData[0];
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
        int id = -1;
        Booking? actualBooking = _bookingRepository.Get(id);

        Assert.Null(actualBooking);
    }

    [Fact]
    public void GetAll_Success()
    {
        List<Booking> actualBookings = _bookingRepository.GetAll();

        Assert.Multiple(() =>
        {
            Assert.Equal(_bookingData.Count, actualBookings.Count);

            List<Booking> orderedBookings = actualBookings.OrderBy(r => r.Id).ToList();
            for (int index = 0; index < orderedBookings.Count; index++)
            {
                Assert.Equal(_bookingData[index].Id, orderedBookings[index].Id);
            }
        });
    }

    [Fact]
    public void GetByUser_ValidUserId()
    {
        int userId = 1;

        List<Booking> expectedBookings = _bookingData.Where(r => r.UserId == userId).OrderBy(r => r.Id).ToList();
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
        int userId = -1;

        List<Booking> actualBookings = _bookingRepository.GetByUser(userId);

        Assert.Empty(actualBookings);
    }

    [Fact]
    public void GetWithOverlappingDates_EmptyWithDifferentFromAndTo()
    {
        int rentalId = 1;

        DateOnly from = new DateOnly(2026, 8, 8);
        DateOnly to = new DateOnly(2026, 8, 12);

        List<Booking> actualBookings = _bookingRepository.GetWithOverlappingDates(rentalId, from, to);

        Assert.Empty(actualBookings);
    }

    [Fact]
    public void GetWithOverlappingDates_EmptyWithSameFromAndTo()
    {
        int rentalId = 1;

        DateOnly from = new DateOnly(2026, 7, 10);
        DateOnly to = from;

        List<Booking> actualBookings = _bookingRepository.GetWithOverlappingDates(rentalId, from, to);

        Assert.Empty(actualBookings);
    }

    [Fact]
    public void GetWithOverlappingDates_BookingSurroundingOverlap()
    {
        int rentalId = 1;

        DateOnly from = new DateOnly(2026, 7, 3);
        DateOnly to = new DateOnly(2026, 7, 5);

        Booking expectedBooking = _bookingData[1];
        List<Booking> actualBookings = _bookingRepository.GetWithOverlappingDates(rentalId, from, to);

        Assert.Multiple(() =>
        {
            Assert.Single(actualBookings);

            Assert.Equal(expectedBooking.Id, actualBookings[0].Id);
        });
    }

    [Fact]
    public void GetWithOverlappingDates_BookingIncludedInOverlap()
    {
        int rentalId = 1;

        DateOnly from = new DateOnly(2026, 6, 3);
        DateOnly to = new DateOnly(2026, 7, 10);

        Booking expectedBooking = _bookingData[1];
        List<Booking> actualBookings = _bookingRepository.GetWithOverlappingDates(rentalId, from, to);

        Assert.Multiple(() =>
        {
            Assert.Single(actualBookings);

            Assert.Equal(expectedBooking.Id, actualBookings[0].Id);
        });
    }

    [Fact]
    public void GetWithOverlappingDates_LastDayMatchesFrom()
    {
        int rentalId = 1;

        DateOnly from = new DateOnly(2026, 7, 30);
        DateOnly to = new DateOnly(2026, 8, 10);

        Booking expectedBooking = _bookingData[4];
        List<Booking> actualBookings = _bookingRepository.GetWithOverlappingDates(rentalId, from, to);

        Assert.Multiple(() =>
        {
            Assert.Single(actualBookings);

            Assert.Equal(expectedBooking.Id, actualBookings[0].Id);
        });
    }

    [Fact]
    public void GetWithOverlappingDates_FirstDayMatchesTo()
    {
        int rentalId = 1;

        DateOnly from = new DateOnly(2026, 6, 19);
        DateOnly to = new DateOnly(2026, 7, 1);

        Booking expectedBooking = _bookingData[1];
        List<Booking> actualBookings = _bookingRepository.GetWithOverlappingDates(rentalId, from, to);

        Assert.Multiple(() =>
        {
            Assert.Single(actualBookings);

            Assert.Equal(expectedBooking.Id, actualBookings[0].Id);
        });
    }

    [Fact]
    public void GetWithOverlappingDates_SeveralBookingsOverlap()
    {
        int rentalId = 1;

        DateOnly from = new DateOnly(2026, 7, 8);
        DateOnly to = new DateOnly(2026, 7, 14);

        List<Booking> expectedBookings = [_bookingData[1], _bookingData[2]];
        List<Booking> actualBookings = _bookingRepository.GetWithOverlappingDates(rentalId, from, to);

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
