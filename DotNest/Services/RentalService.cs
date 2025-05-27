using DotNest.DataAccess.Entities;
using DotNest.DataAccess.Interfaces;
using DotNest.Models;
using DotNest.Services.Interfaces;
using DotNest.Services.Mapper;

namespace DotNest.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPictureRepository _pictureRepository;

        private readonly ILogger _logger;

        public RentalService(IRentalRepository rentalRepository, IUserRepository userRepository, IPictureRepository pictureRepository,
            ILogger<RentalService> logger)
        {
            _rentalRepository = rentalRepository;
            _userRepository = userRepository;
            _pictureRepository = pictureRepository;
            _logger = logger;
        }


        public RentalModel? Get(int id)
        {
            Rental? rental = _rentalRepository.Get(id);

            if (rental is null)
                return null;

            return RentalMapper.MapToModel(rental!);
        }
        public List<RentalModel> GetAllRentalsOf(string username)
        {
            // get user
            User user = _userRepository.GetByUsername(username)!; // the user is connected, it must exist

            
            List<Rental> rentals = _rentalRepository.GetByUser(user.Id);

            return RentalMapper.MapToModel(rentals);
        }

        /// <summary>
        /// create the picture
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// the id of the picture entity
        /// </returns>
        private int HandlePicture(RentalModel model, bool isCreate)
        {
            //handle image
            if (model.Picture != null)
            {
                var imageArray = GetFileByteAray(model.Picture);
                if (imageArray != null)
                {
                    // To simplify things - we always add image
                    Picture picture = new Picture()
                    {
                        Base64 = Convert.ToBase64String(imageArray)
                    };

                    if (isCreate)
                    {
                        picture = _pictureRepository.Create(picture);
                        return picture.Id;
                    }
                    else
                    {
                        // If update, the picture was created
                        picture.Id = (int)model.PictureId!;
                        _pictureRepository.Update(picture);
                        return picture.Id;
                    }
                }
                else
                {
                    throw new Exception("Could not get byte arrays from picture");
                }
            }
            else // no picture was uploaded
            {
                return(int)model.PictureId!;
            }
        }

        private static byte[] GetFileByteAray(IFormFile formFile)
        {
            if (formFile != null)
            {
                if (formFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        formFile.CopyTo(memoryStream);

                        if (memoryStream.Length < 50 * 1024 * 1024)
                        {
                            return memoryStream.ToArray();
                        }
                    }

                }
            }

            return [];
        }


        public void CreateRental(string username, RentalModel model)
        {
            User user = _userRepository.GetByUsername(username)!; // the user is connected, it must exist
            model.UserId = user.Id;

            // add picture
            int pictureId = HandlePicture(model, true);

            // create rental
            Rental rental = RentalMapper.MapToEntity(model, pictureId);
            _rentalRepository.Create(rental);
        }

        public void UpdateRental(RentalModel model)
        {
            // update picture
            int pictureId = HandlePicture(model, false);

            // create rental
            Rental rental = RentalMapper.MapToEntity(model, pictureId);
            rental.Id = (int)model.Id!;
            _rentalRepository.Update(rental);
        }
    }
}
