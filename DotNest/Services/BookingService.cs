using System.Collections.Generic;

using AutoMapper;
using DotNest.DataAccess.Entities;
using DotNest.DataAccess.Interfaces;
using DotNest.Models;
using DotNest.Services.Interfaces;

namespace DotNest.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public BookingService(IBookingRepository bookingRepository, IUserRepository userRepository,
            IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public void BookRental(string username, BookingModel bookingModel)
        {
            User user = _userRepository.GetByUsername(username)!;

            List<Booking> conflictingBookings = _bookingRepository.GetWithOverlappingDates(bookingModel.RentalId, bookingModel.FromDate, bookingModel.ToDate);
            if (conflictingBookings.Count != 0)
            {
                List<DateOnly[]> periods = MergeOverlappingPeriods(conflictingBookings.Select(booking => new Tuple<DateOnly, DateOnly>(booking.FromDate, booking.ToDate)).ToList());
                string message = "Les périodes ";

                int index = 0;
                foreach (DateOnly[] period in periods)
                {
                    message += $"{period[0].ToShortDateString()} - {period[1].ToShortDateString()}";
                    if (index < periods.Count - 1)
                        message += ", ";
                    else
                    {
                        message += " ";
                    }
                    index++;
                }
                message += "sont réservées et sont donc en conflit avec votre sélection";
                throw new Exception(message);
            }

            Booking booking = _mapper.Map<Booking>(bookingModel);
            booking.UserId = user.Id;

            _bookingRepository.Create(booking);
        }

        private List<DateOnly[]> MergeOverlappingPeriods(List<Tuple<DateOnly, DateOnly>> periods)
        {
            periods.Sort((period1, period2) => period1.Item1.CompareTo(period2.Item1));

            List<DateOnly[]> overlappingPeriods = new List<DateOnly[]>();
            overlappingPeriods.Add([periods[0].Item1, periods[0].Item2]);

            for (int i = 1; i < periods.Count; i++)
            {
                DateOnly[] lastMergedInterval = overlappingPeriods[overlappingPeriods.Count - 1];
                DateOnly[] currentPeriodInterval = [periods[i].Item1, periods[i].Item2];

                // If current interval overlaps with the last merged interval, merge them 
                if (currentPeriodInterval[0] <= lastMergedInterval[1])
                    lastMergedInterval[1] = lastMergedInterval[1] > currentPeriodInterval[1] ? lastMergedInterval[1] : currentPeriodInterval[1]; // get max end date
                else
                    overlappingPeriods.Add([currentPeriodInterval[0], currentPeriodInterval[1]]);
            }

            return overlappingPeriods;
        }

        public void DeleteBooking(int id)
        {
            Booking booking = _bookingRepository.Get(id);
            _bookingRepository.Delete(booking);
        }

        public List<BookingModel> GetBookingsByRentalId(int rentalId)
        {
            return _mapper.Map <List<BookingModel>> (_bookingRepository.GetAll()
                .Where(b => b.RentalId == rentalId)
                .ToList());
        }


        public List<BookingModel> GetAllBookingsFromUser(string username)
        {
            User user = _userRepository.GetByUsername(username)!;

            return _mapper.Map<List<BookingModel>>(_bookingRepository.GetByUser(user.Id));
        }
    }
}
