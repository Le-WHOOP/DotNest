using AutoMapper;

using DotNest.DataAccess.Entities;
using DotNest.Models;
using DotNest.Services;
using DotNest.Services.Interfaces;
using DotNest.Services.Mapper;

using Xunit.Sdk;

namespace DotTest;

public class BookingServiceTest
{
    private readonly BookingService _bookingService;
    private readonly List<Booking> _bookingData;
    private readonly List<BookingModel> _bookingModelData;

    public BookingServiceTest()
    {
        MockData mockData = new ();
        Mapper mapper = new (new MapperConfiguration(config => config.AddProfile<AutomapperProfiles>()));
        _bookingService = new BookingService(mockData.BookingRepository, mockData.UserRepository, mapper);
        _bookingData = mockData.BookingData;
        _bookingModelData = mockData.BookingModelData;
    }

    [Fact]
    public void BookRental_OneBookingBeginningBeforeAndFinishingDuring()
    {
        string username = "user3";
        Booking booking = _bookingData[0];
        BookingModel bookingModel = new()
        {
            RentalName = booking.Rental.Name,
            RentalId = booking.RentalId,
            FromDate = new DateOnly(2026, 6, 3),
            ToDate = new DateOnly(2026, 7, 4),
        };

        Exception thrownException = Assert.Throws<Exception>(() => _bookingService.BookRental(username, bookingModel));
        Assert.Equal("Les périodes 01/07/2026 - 09/07/2026 sont réservées et sont donc en conflit avec votre sélection", thrownException.Message);
    }

    [Fact]
    public void BookRental_OneBookingBeginningDuringAndFinishingAfter()
    {
        string username = "user2";
        Booking booking = _bookingData[0];
        BookingModel bookingModel = new()
        {
            RentalName = booking.Rental.Name,
            RentalId = booking.RentalId,
            FromDate = new DateOnly(2026, 7, 27),
            ToDate = new DateOnly(2026, 8, 4),
        };

        Exception thrownException = Assert.Throws<Exception>(() => _bookingService.BookRental(username, bookingModel));
        Assert.Equal("Les périodes 25/07/2026 - 30/07/2026 sont réservées et sont donc en conflit avec votre sélection", thrownException.Message);
    }

    [Fact]
    public void BookRental_OneBookingInTheMiddle()
    {
        string username = "user3";
        Booking booking = _bookingData[0];
        BookingModel bookingModel = new()
        {
            RentalName = booking.Rental.Name,
            RentalId = booking.RentalId,
            FromDate = new DateOnly(2026, 7, 3),
            ToDate = new DateOnly(2026, 7, 4),
        };

        Exception thrownException = Assert.Throws<Exception>(() => _bookingService.BookRental(username, bookingModel));
        Assert.Equal("Les périodes 01/07/2026 - 09/07/2026 sont réservées et sont donc en conflit avec votre sélection", thrownException.Message);
    }


    [Fact]
    public void BookRental_SeveralBookingsNotOverlapping()
    {
        string username = "user3";
        Booking booking = _bookingData[0];
        BookingModel bookingModel = new()
        {
            RentalName = booking.Rental.Name,
            RentalId = booking.RentalId,
            FromDate = new DateOnly(2026, 7, 3),
            ToDate = new DateOnly(2026, 7, 15),
        };

        Exception thrownException = Assert.Throws<Exception>(() => _bookingService.BookRental(username, bookingModel));
        Assert.Equal("Les périodes 01/07/2026 - 09/07/2026, 11/07/2026 - 20/07/2026 sont réservées et sont donc en conflit avec votre sélection", thrownException.Message);
    }

    [Fact]
    public void BookRental_SeveralBookingsOverlapping()
    {
        string username = "user4";
        Booking booking = _bookingData[0];
        BookingModel bookingModel = new()
        {
            RentalName = booking.Rental.Name,
            RentalId = booking.RentalId,
            FromDate = new DateOnly(2023, 7, 2),
            ToDate = new DateOnly(2023, 7, 9),
        };

        Exception thrownException = Assert.Throws<Exception>(() => _bookingService.BookRental(username, bookingModel));
        Assert.Equal("Les périodes 01/07/2023 - 12/07/2023 sont réservées et sont donc en conflit avec votre sélection", thrownException.Message);
    }

    [Fact]
    public void BookRental_Available()
    {
        string username = "user4";
        Booking booking = _bookingData[0];
        BookingModel bookingModel = new()
        {
            RentalName = booking.Rental.Name,
            RentalId = booking.RentalId,
            FromDate = new DateOnly(2026, 8, 2),
            ToDate = new DateOnly(2026, 8, 9),
        };

        try
        {
            _bookingService.BookRental(username, bookingModel);
        }
        catch (Exception ex)
        {

            Assert.Fail("Expected no exception, but got: " + ex.Message);
        }
    }

    [Fact]
    public void GetBookingsByRentalId_ValidId()
    {
        int rentalId = 1;

        List<BookingModel> expectedBookingModels = [_bookingModelData[0], _bookingModelData[1], _bookingModelData[2], _bookingModelData[3], _bookingModelData[4]];
        List<BookingModel> actualBookingModels = _bookingService.GetBookingsByRentalId(rentalId);

        Assert.Multiple(() =>
        {
            Assert.Equal(expectedBookingModels.Count, actualBookingModels.Count);

            List<BookingModel> orderedBookings = actualBookingModels.OrderBy(r => r.Id).ToList();
            for (int index = 0; index < orderedBookings.Count; index++)
            {
                Assert.Equal(expectedBookingModels[index].Id, orderedBookings[index].Id);
            }
        });
    }


    [Fact]
    public void GetBookingsByRentalId_InvalidId()
    {
        int rentalId = -1;

        List<BookingModel> actualBookingModels = _bookingService.GetBookingsByRentalId(rentalId);

        Assert.Empty(actualBookingModels);
    }

    [Fact]
    public void GetAllBookingsFromUser_WithBookings()
    {
        string username = "user2";

        List<BookingModel> expectedBookingModels = [_bookingModelData[0], _bookingModelData[1], _bookingModelData[2]];

        List<BookingModel> actualBookingModels = _bookingService.GetAllBookingsFromUser(username);

        Assert.Multiple(() =>
        {
            Assert.Equal(expectedBookingModels.Count, actualBookingModels.Count);

            List<BookingModel> orderedBookings = actualBookingModels.OrderBy(r => r.Id).ToList();
            for (int index = 0; index < orderedBookings.Count; index++)
            {
                Assert.Equal(expectedBookingModels[index].Id, orderedBookings[index].Id);
            }
        });
    }

    [Fact]
    public void GetAllBookingsFromUser_NoBooking()
    {
        string username = "user4";

        List<BookingModel> actualBookingModels = _bookingService.GetAllBookingsFromUser(username);

        Assert.Empty(actualBookingModels);
    }
}
