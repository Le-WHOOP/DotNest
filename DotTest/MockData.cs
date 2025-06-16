
using AutoMapper;

using DotNest.DataAccess.Entities;
using DotNest.DataAccess.Interfaces;
using DotNest.DataAccess.Repositories;
using DotNest.Services.Mapper;

using Microsoft.EntityFrameworkCore;

using Moq;
using Moq.EntityFrameworkCore;

namespace DotTest
{
    internal class MockData
    {
        // Data
        public Booking[] BookingsData { get; private set; }
        public Picture[] PicturesData { get; private set; }
        public Rental[] RentalsData { get; private set; }
        public User[] UsersData { get; private set; }

        // Repositories
        public BookingRepository BookingRepository { get; private set; }
        public PictureRepository PictureRepository { get; private set; }
        public RentalRepository RentalRepository { get; private set; }
        public UserRepository UserRepository { get; private set; }

        public MockData()
        {
            Mapper mapper = new(new MapperConfiguration(config => config.AddProfile<AutomapperProfiles>()));

            InitData();

            Mock<DotNestContext> mockContext = InitMockContext(mapper);

            InitRepositories(mockContext);
        }

        private void InitData()
        {
            // Users, without Bookings nor Rentals
            User user1 = new ()
            {
                Id = 1,
                Username = "user1",
                Email = "user1@gmail.com",
                // user1-password
                HashedPassword = "xWq7vWEA7lQMX5K9RZh4Vj6kd0ZRiW+ivE2uPEug0jg=",
                PasswordSalt = "//Wnob5zMWEzC9dS4Y2eLw==",
            };
            User user2 = new ()
            {
                Id = 2,
                Username = "user2",
                Email = "user2@gmail.com",
                // user2-password
                HashedPassword = "FTO9il61cy62M0wJWW0fH13nOxI7KKFx2vd2HZzlcyk=",
                PasswordSalt = "KSW/Egf5b14sjOlPkZQLsA==",
            };
            User user3 = new()
            {
                Id = 3,
                Username = "user3",
                Email = "user3@gmail.com",
                // user3-password
                HashedPassword = "PCFh3clie0FlvSExlMFp+mnF1t9hfEXFekRdhcmsh50=",
                PasswordSalt = "o5exm461ZOiNaM6P0kV3eQ==",
            };

            // Pictures, without Rentals
            Picture picture1A = new()
            {
                Id = 1,
                Base64 = ""
            };
            Picture picture1B = new()
            {
                Id = 2,
                Base64 = ""
            };
            Picture picture2A = new()
            {
                Id = 3,
                Base64 = ""
            };
            // Register the base64
            try
            {
                using StreamReader picture1ABase64 = new("Resources/Picture1A_Base64.txt");
                picture1A.Base64 = picture1ABase64.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            try
            {
                using StreamReader picture1BBase64 = new("Resources/Picture1B_Base64.txt");
                picture1B.Base64 = picture1BBase64.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            try
            {
                using StreamReader picture2ABase64 = new("Resources/Picture2A_Base64.txt");
                picture2A.Base64 = picture2ABase64.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            // Rentals, without Bookings
            Rental rental1A = new()
            {
                Id = 1,
                PictureId = 1,
                UserId = 1,
                Name = "Location 1A",
                Description = "Endroit calme et reposant",
                City = "Paris",
                Picture = picture1A,
                User = user1,
            };
            Rental rental1B = new()
            {
                Id = 2,
                PictureId = 2,
                UserId = 1,
                Name = "Location 1B",
                Description = "Immeuble rénové récemment",
                City = "Paris",
                Picture = picture1B,
                User = user1,
            };
            Rental rental2A = new()
            {
                Id = 3,
                PictureId = 3,
                UserId = 2,
                Name = "Location 2A",
                Description = "Parfait pour travailler",
                City = "Le Kremlin-Bicetre",
                Picture = picture2A,
                User = user2,
            };

            // Bookings
            Booking booking1OfRental1AByUser2 = new()
            {
                Id = 1,
                UserId = 2,
                RentalId = 1,
                FromDate = new DateOnly(2025, 7, 1),
                ToDate = new DateOnly(2025, 7, 9),
                Rental = rental1A,
                User = user2,
            };
            Booking booking2OfRental1AByUser2 = new()
            {
                Id = 2,
                UserId = 2,
                RentalId = 1,
                FromDate = new DateOnly(2025, 7, 11),
                ToDate = new DateOnly(2025, 7, 20),
                Rental = rental1A,
                User = user2,
            };
            Booking bookingOfRental1AByUser3 = new()
            {
                Id = 3,
                UserId = 3,
                RentalId = 1,
                FromDate = new DateOnly(2025, 7, 25),
                ToDate = new DateOnly(2025, 7, 30),
                Rental = rental1A,
                User = user3,
            };
            Booking bookingOfRental2AByUser3 = new()
            {
                Id = 4,
                UserId = 3,
                RentalId = 3,
                FromDate = new DateOnly(2025, 7, 13),
                ToDate = new DateOnly(2025, 7, 24),
                Rental = rental2A,
                User = user3,
            };

            // Set Bookings and Rentals for each User
            user1.Rentals = [rental1A, rental1B];
            user2.Bookings = [booking1OfRental1AByUser2, booking2OfRental1AByUser2];
            user2.Rentals = [rental2A];
            user3.Bookings = [bookingOfRental1AByUser3, bookingOfRental2AByUser3];

            // Set Rentals for each Picture
            picture1A.Rentals = [rental1A];
            picture1B.Rentals = [rental1B];
            picture2A.Rentals =[rental2A];

            // Set Bookings for each Rental
            rental1A.Bookings = [booking1OfRental1AByUser2, booking2OfRental1AByUser2, bookingOfRental1AByUser3];
            rental1B.Bookings = [bookingOfRental2AByUser3];

            // Initialise properties
            BookingsData = [booking1OfRental1AByUser2, booking2OfRental1AByUser2, bookingOfRental1AByUser3, bookingOfRental2AByUser3];
            PicturesData = [picture1A, picture1B, picture2A];
            RentalsData = [rental1A, rental1B, rental2A];
            UsersData = [user1, user2, user3];
        }

        private Mock<DbSet<Entity>> InitMockSet<Entity>(Entity[] data)
    where Entity : class, new()
        {
            IQueryable<Entity> queryableData = data.AsQueryable();
            Mock<DbSet<Entity>> mockSet = new Mock<DbSet<Entity>>();

            mockSet.As<IQueryable<Entity>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockSet.As<IQueryable<Entity>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<Entity>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<Entity>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());

            return mockSet;
        }

        private Mock<DotNestContext> InitMockContext(Mapper mapper)
        {
            // Init mock sets
            Mock<DbSet<Booking>> mockBookingsSet = InitMockSet(BookingsData);
            Mock<DbSet<Picture>> mockPicturesSet = InitMockSet(PicturesData);
            Mock<DbSet<Rental>> mockRentalsSet = InitMockSet(RentalsData);
            Mock<DbSet<User>> mockUsersSet = InitMockSet(UsersData);

            Mock<DotNestContext> mockContext = new();

            // Link mock sets to mock context
            // Bookings
            mockContext.Setup(c => c.Bookings).ReturnsDbSet(mockBookingsSet.Object);
            mockContext.Setup(c => c.Set<Booking>()).ReturnsDbSet(mockBookingsSet.Object);
            // Pictures
            mockContext.Setup(c => c.Pictures).ReturnsDbSet(mockPicturesSet.Object);
            mockContext.Setup(c => c.Set<Picture>()).ReturnsDbSet(mockPicturesSet.Object);
            // Rentals
            mockContext.Setup(c => c.Rentals).ReturnsDbSet(mockRentalsSet.Object);
            mockContext.Setup(c => c.Set<Rental>()).ReturnsDbSet(mockRentalsSet.Object);
            // Users
            mockContext.Setup(c => c.Users).ReturnsDbSet(mockUsersSet.Object);
            mockContext.Setup(c => c.Set<User>()).ReturnsDbSet(mockUsersSet.Object);

            return mockContext;
        }

        private void InitRepositories(Mock<DotNestContext> mockContext)
        {
            BookingRepository = new BookingRepository(mockContext.Object);
            PictureRepository = new PictureRepository(mockContext.Object);
            RentalRepository = new RentalRepository(mockContext.Object);
            UserRepository = new UserRepository(mockContext.Object);
        }
    }
}
