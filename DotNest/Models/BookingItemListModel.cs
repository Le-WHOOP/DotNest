namespace DotNest.Models
{
    public class BookingItemListModel
    {
        public string UserName { get; set; }

        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
    }
}
