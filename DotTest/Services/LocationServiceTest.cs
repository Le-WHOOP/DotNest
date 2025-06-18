using DotNest.Models;
using DotNest.Services;

namespace DotTest;

public class LocationServiceTest
{
    private readonly LocationService _locationService;
    private readonly List<RentalModel> _rentalModelData;
    private readonly List<BookingModel> _bookingModelData;

    public LocationServiceTest()
    {
        MockData mockData = new ();
        _locationService = new LocationService(mockData.RentalRepository, mockData.UserRepository, mockData.BookingRepository);
        _rentalModelData = mockData.RentalModelData;
        _bookingModelData = mockData.BookingModelData;
    }

    [Fact]
    public void GetAllAvailableRentals_PeriodIncludingAllBookingsWithoutCity()
    {
        DateTime fromDate = new DateTime(2022, 7, 27);
        DateTime toDate = new DateTime(2027, 8, 4);
        string? city = null;

        List<RentalModel> expectedRentalModels = [_rentalModelData[1]];
        List<RentalModel> actualRentalModels = _locationService.GetAllAvailableRentals(fromDate, toDate, city);

        Assert.Multiple(() =>
        {
            Assert.Equal(expectedRentalModels.Count, actualRentalModels.Count);

            List<RentalModel> orderedRentals = actualRentalModels.OrderBy(r => r.Id).ToList();
            for (int index = 0; index < orderedRentals.Count; index++)
            {
                Assert.Equal(expectedRentalModels[index].Id, orderedRentals[index].Id);
            }
        });
    }

    [Fact]
    public void GetAllAvailableRentals_PeriodIncludingAllBookingsWithCity()
    {
        DateTime fromDate = new DateTime(2022, 7, 27);
        DateTime toDate = new DateTime(2027, 8, 4);
        string? city = "Paris";

        List<RentalModel> actualRentalModels = _locationService.GetAllAvailableRentals(fromDate, toDate, city);

        Assert.Empty(actualRentalModels);
    }

    [Fact]
    public void GetAllAvailableRentals_AvailableWithoutCity()
    {
        DateTime fromDate = new DateTime(2026, 7, 11);
        DateTime toDate = new DateTime(2026, 7, 12);
        string? city = null;

        List<RentalModel> expectedRentalModels = [_rentalModelData[1], _rentalModelData[2]];
        List<RentalModel> actualRentalModels = _locationService.GetAllAvailableRentals(fromDate, toDate, city);

        Assert.Multiple(() =>
        {
            Assert.Equal(expectedRentalModels.Count, actualRentalModels.Count);

            List<RentalModel> orderedRentals = actualRentalModels.OrderBy(r => r.Id).ToList();
            for (int index = 0; index < orderedRentals.Count; index++)
            {
                Assert.Equal(expectedRentalModels[index].Id, orderedRentals[index].Id);
            }
        });
    }

    [Fact]
    public void GetAllAvailableRentals_AvailableWithCity()
    {
        DateTime fromDate = new DateTime(2026, 7, 11);
        DateTime toDate = new DateTime(2026, 7, 12);
        string? city = "Paris";

        List<RentalModel> expectedRentalModels = [_rentalModelData[2]];
        List<RentalModel> actualRentalModels = _locationService.GetAllAvailableRentals(fromDate, toDate, city);

        Assert.Multiple(() =>
        {
            Assert.Equal(expectedRentalModels.Count, actualRentalModels.Count);

            List<RentalModel> orderedRentals = actualRentalModels.OrderBy(r => r.Id).ToList();
            for (int index = 0; index < orderedRentals.Count; index++)
            {
                Assert.Equal(expectedRentalModels[index].Id, orderedRentals[index].Id);
            }
        });
    }

    [Fact]
    public void GetAllAvailableRentalsAndUserBooking_PeriodIncludingBookingsWithoutCity()
    {
        string username = "user1";
        DateTime fromDate = new DateTime(2026, 7, 3);
        DateTime toDate = new DateTime(2026, 7, 12);
        string? city = null;

        List<RentalModel> expectedRentalModels = [_rentalModelData[2]];
        List<RentalModel> actualRentalModels = _locationService.GetAllAvailableRentalsAndUserBooking(username, fromDate, toDate, city);

        Assert.Multiple(() =>
        {
            Assert.Equal(expectedRentalModels.Count, actualRentalModels.Count);

            List<RentalModel> orderedRentals = actualRentalModels.OrderBy(r => r.Id).ToList();
            for (int index = 0; index < orderedRentals.Count; index++)
            {
                Assert.Equal(expectedRentalModels[index].Id, orderedRentals[index].Id);
            }
        });
    }

    [Fact]
    public void GetAllAvailableRentalsAndUserBooking_PeriodIncludingBookingsWithCity()
    {
        string username = "user1";
        DateTime fromDate = new DateTime(2026, 7, 3);
        DateTime toDate = new DateTime(2026, 7, 12);
        string? city = "Paris";

        List<RentalModel> expectedRentalModels = [_rentalModelData[2]];
        List<RentalModel> actualRentalModels = _locationService.GetAllAvailableRentalsAndUserBooking(username, fromDate, toDate, city);

        Assert.Multiple(() =>
        {
            Assert.Equal(expectedRentalModels.Count, actualRentalModels.Count);

            List<RentalModel> orderedRentals = actualRentalModels.OrderBy(r => r.Id).ToList();
            for (int index = 0; index < orderedRentals.Count; index++)
            {
                Assert.Equal(expectedRentalModels[index].Id, orderedRentals[index].Id);
            }
        });
    }
    [Fact]
    public void GetAllAvailableRentalsAndUserBooking_AvailableWithoutCity()
    {
        string username = "user3";
        DateTime fromDate = new DateTime(2026, 8, 10);
        DateTime toDate = new DateTime(2026, 8, 15);
        string? city = null;

        List<RentalModel> expectedRentalModels = [_rentalModelData[0], _rentalModelData[1],_rentalModelData[2]];
        List<RentalModel> actualRentalModels = _locationService.GetAllAvailableRentalsAndUserBooking(username, fromDate, toDate, city);

        Assert.Multiple(() =>
        {
            Assert.Equal(expectedRentalModels.Count, actualRentalModels.Count);

            List<RentalModel> orderedRentals = actualRentalModels.OrderBy(r => r.Id).ToList();
            for (int index = 0; index < orderedRentals.Count; index++)
            {
                Assert.Equal(expectedRentalModels[index].Id, orderedRentals[index].Id);
            }
        });
    }

    [Fact]
    public void GetAllAvailableRentalsAndUserBooking_AvailableWithCity()
    {
        string username = "user3";
        DateTime fromDate = new DateTime(2026, 8, 10);
        DateTime toDate = new DateTime(2026, 8, 15);
        string? city = "Paris";

        List<RentalModel> expectedRentalModels = [_rentalModelData[0], _rentalModelData[2]];
        List<RentalModel> actualRentalModels = _locationService.GetAllAvailableRentalsAndUserBooking(username, fromDate, toDate, city);

        Assert.Multiple(() =>
        {
            Assert.Equal(expectedRentalModels.Count, actualRentalModels.Count);

            List<RentalModel> orderedRentals = actualRentalModels.OrderBy(r => r.Id).ToList();
            for (int index = 0; index < orderedRentals.Count; index++)
            {
                Assert.Equal(expectedRentalModels[index].Id, orderedRentals[index].Id);
            }
        });
    }

    [Fact]
    public void GetUnavailableDates_OneBooking()
    {
        List<BookingModel> bookingModelsToConvert = [_bookingModelData[3]];

        List<string> expectedDates = ["2023-07-06", "2023-07-07", "2023-07-08", "2023-07-09", "2023-07-10", "2023-07-11", "2023-07-12"];
        List<string> actualDates = _locationService.GetUnavailableDates(bookingModelsToConvert);

        Assert.Multiple(() =>
        {
            Assert.Equal(expectedDates.Count, actualDates.Count);

            for (int index = 0; index < actualDates.Count; index++)
            {
                Assert.Equal(expectedDates[index], actualDates[index]);
            }
        });
    }

    [Fact]
    public void GetUnavailableDates_TwoBookings()
    {
        List<BookingModel> bookingModelsToConvert = [_bookingModelData[0], _bookingModelData[3]];

        List<string> expectedDates = ["2023-07-01", "2023-07-02", "2023-07-03", "2023-07-04", "2023-07-05", "2023-07-06", "2023-07-07", "2023-07-08", "2023-07-09", "2023-07-06", "2023-07-07", "2023-07-08", "2023-07-09", "2023-07-10", "2023-07-11", "2023-07-12"];
        List<string> actualDates = _locationService.GetUnavailableDates(bookingModelsToConvert);

        Assert.Multiple(() =>
        {
            Assert.Equal(expectedDates.Count, actualDates.Count);

            for (int index = 0; index < actualDates.Count; index++)
            {
                Assert.Equal(expectedDates[index], actualDates[index]);
            }
        });
    }
}
