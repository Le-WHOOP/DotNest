using System.ComponentModel;

namespace DotNest.Models
{
    public class BookingModel
    {
        public int? Id { get; set; } = null;
        [DisplayName("Nom du bien")]
        public string RentalName { get; set; }
        public int RentalId { get; set; }

        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }

    }
}
