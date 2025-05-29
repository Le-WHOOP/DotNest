using DotNest.DataAccess.Entities;
using DotNest.Models;

namespace DotNest.Services.Mapper
{
    public static class RentalMapper
    {
        public static List<RentalModel> MapToModel(List<Rental> entities)
        {
            return entities.Select(MapToModel).ToList();
        }

        public static RentalModel MapToModel(Rental entity)
        {
            return new RentalModel()
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Name = entity.Name,
                Description = entity.Description,
                City = entity.City,
                PictureId = entity.PictureId,
                PictureContent = entity.Picture!.Base64
            };
        }

        public static Rental MapToEntity(RentalModel model, int pictureId)
        {
            return new Rental()
            {
                UserId = model.UserId,
                Name = model.Name,
                Description = model.Description,
                City = model.City,
                PictureId = pictureId
            };
        }
    }
}
