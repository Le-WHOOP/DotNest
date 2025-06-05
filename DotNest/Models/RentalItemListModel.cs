using System.ComponentModel.DataAnnotations;

namespace DotNest.Models
{
    public class RentalItemListModel
    {
        public int? Id { get; set; }

        public int UserId { get; set; }


        [Length(10, 100)]
        public string Name { get; set; }

        [Length(20, 2000)]
        public string Description { get; set; }

        /*[RegularExpression("((\\p{L})+[- ]?)*")] but it does not work with ModelState.IsValid because it becomes a javascript regex
         and javascript cannot handle \p{L}*/
        [Length(2, 100)]
        public string City { get; set; }

        public string? PictureContent { get; set; }

        public List<BookingItemListModel> FutureBookings;
    }
}
