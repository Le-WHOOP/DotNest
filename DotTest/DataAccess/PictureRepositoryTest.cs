using DotNest.DataAccess.Entities;
using DotNest.DataAccess.Repositories;

namespace DotTest;

public class PictureRepositoryTest
{
    private PictureRepository _pictureRepository;
    private Picture[] _picturesData;

    // Used to reset the data at each test
    private void InitTest()
    {
        var mockData = new MockData();
        _pictureRepository = mockData.PictureRepository;
        _picturesData = mockData.PicturesData;
    }

    [Fact]
    public void Get_ValidId()
    {
        InitTest();

        int id = 1;
        Picture expectedPicture = _picturesData[0];
        Picture? actualPicture = _pictureRepository.Get(id);

        Assert.Multiple(() =>
        {
            Assert.NotNull(actualPicture);
            Assert.Equal(id, actualPicture.Id);

            // Basics
            Assert.Equal(expectedPicture.Id, actualPicture.Id);
            Assert.Equal(expectedPicture.Base64, actualPicture.Base64);

            // Rentals
            Assert.Equal(expectedPicture.Rentals.Count, actualPicture.Rentals.Count);

            List<Rental> orderedRentals = actualPicture.Rentals.OrderBy(r => r.Id).ToList();
            for (int index = 0; index < orderedRentals.Count; index++)
            {
                Assert.Equal(expectedPicture.Rentals.ElementAt(index).Id, orderedRentals[index].Id);
            }
        });
    }

    [Fact]
    public void Get_InvalidId()
    {
        InitTest();

        int id = -1;
        Picture? actualPicture = _pictureRepository.Get(id);

        Assert.Null(actualPicture);
    }
}
