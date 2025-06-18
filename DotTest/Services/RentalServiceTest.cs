using DotNest.DataAccess.Entities;
using DotNest.Models;
using DotNest.Services;

namespace DotTest;

public class RentalServiceTest
{
    private readonly RentalService _rentalService;
    private readonly List<RentalModel> _rentalModelData;
    private readonly List<RentalItemListModel> _rentalItemData;

    public RentalServiceTest()
    {
        MockData mockData = new();
        _rentalService = new RentalService(mockData.RentalRepository, mockData.UserRepository, mockData.PictureRepository, mockData.BookingRepository);
        _rentalModelData = mockData.RentalModelData;
        _rentalItemData = mockData.RentalItemData;
    }

    [Fact]
    public void GetRental_ValidId()
    {
        int id = 1;

        RentalModel expectedRentalModel = _rentalModelData[0];
        RentalModel? actualRentalModel = _rentalService.GetRental(id);

        Assert.Multiple(() =>
        {
            Assert.NotNull(actualRentalModel);
            Assert.Equal(id, actualRentalModel.Id);

            Assert.Equal(expectedRentalModel.Id, actualRentalModel.Id);
            Assert.Equal(expectedRentalModel.UserId, actualRentalModel.UserId);
            Assert.Equal(expectedRentalModel.Name, actualRentalModel.Name);
            Assert.Equal(expectedRentalModel.Description, actualRentalModel.Description);
            Assert.Equal(expectedRentalModel.City, actualRentalModel.City);
            Assert.Equal(expectedRentalModel.PictureId, actualRentalModel.PictureId);
            Assert.Equal(expectedRentalModel.PictureContent, actualRentalModel.PictureContent);
        });
    }

    [Fact]
    public void GetRental_InvalidId()
    {
        int id = -1;

        RentalModel? actualRentalModel = _rentalService.GetRental(id);

        Assert.Null(actualRentalModel);
    }

    [Fact]
    public void GetAllRentalsOf_UserWithRentals()
    {
        string username = "user1";

        List<RentalModel> expectedRentalModels = [_rentalModelData[0], _rentalModelData[1]];
        List<RentalModel> actualRentalModels = _rentalService.GetAllRentalsOf(username);

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
    public void GetAllRentalsOf_UserWithoutRental()
    {
        string username = "user3";

        List<RentalModel> actualRentalModels = _rentalService.GetAllRentalsOf(username);

        Assert.Empty(actualRentalModels);
    }


    [Fact]
    public void GetAllRentalItemsOf_UserWithRentals()
    {
        string username = "user1";

        List<RentalItemListModel> expectedRentalItems = [_rentalItemData[0], _rentalItemData[1]];
        List<RentalItemListModel> actualRentalItems = _rentalService.GetAllRentalItemsOf(username);

        Assert.Multiple(() =>
        {
            Assert.Equal(expectedRentalItems.Count, actualRentalItems.Count);

            List<RentalItemListModel> orderedRentalItems = actualRentalItems.OrderBy(r => r.Id).ToList();
            for (int indexRentalItems = 0; indexRentalItems < orderedRentalItems.Count; indexRentalItems++)
            {
                Assert.Equal(expectedRentalItems[indexRentalItems].Id, orderedRentalItems[indexRentalItems].Id);

                Assert.Equal(expectedRentalItems[indexRentalItems].FutureBookings.Count, orderedRentalItems[indexRentalItems].FutureBookings.Count);

                for (int indexBookingItem = 0;
                indexBookingItem < orderedRentalItems[indexBookingItem].FutureBookings.Count;
                indexBookingItem++)
                {
                    Assert.Equal(expectedRentalItems[indexBookingItem].FutureBookings[indexBookingItem].UserName,
                        orderedRentalItems[indexBookingItem].FutureBookings[indexBookingItem].UserName);
                    Assert.Equal(expectedRentalItems[indexBookingItem].FutureBookings[indexBookingItem].FromDate,
                        orderedRentalItems[indexBookingItem].FutureBookings[indexBookingItem].FromDate);
                    Assert.Equal(expectedRentalItems[indexBookingItem].FutureBookings[indexBookingItem].ToDate,
                        orderedRentalItems[indexBookingItem].FutureBookings[indexBookingItem].ToDate);
                }
            }
        });
    }

    [Fact]
    public void GetAllRentalItemsOf_UserWithoutRental()
    {
        string username = "user3";

        List<RentalItemListModel> actualRentalItems = _rentalService.GetAllRentalItemsOf(username);

        Assert.Empty(actualRentalItems);
    }
}
