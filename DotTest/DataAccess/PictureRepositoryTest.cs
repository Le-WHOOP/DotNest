using DotNest.DataAccess.Entities;
using DotNest.DataAccess.Repositories;

namespace DotTest;

public class PictureRepositoryTest
{
    private readonly PictureRepository _pictureRepository;
    private readonly List<Picture> _pictureData;

    public PictureRepositoryTest()
    {
        var mockData = new MockData();
        _pictureRepository = mockData.PictureRepository;
        _pictureData = mockData.PictureData;
    }

    [Fact]
    public void Get_ValidId()
    {
        int id = 1;
        Picture expectedPicture = _pictureData[0];
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
        int id = -1;
        Picture? actualPicture = _pictureRepository.Get(id);

        Assert.Null(actualPicture);
    }
}
