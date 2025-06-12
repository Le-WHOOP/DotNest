using DotNest.Models;

namespace DotNest.Services.Interfaces
{
    public interface IRentalService
    {
        RentalModel? GetRental(int id);
        public List<RentalModel> GetAllRentalsOf(string username);
        public void CreateRental(string username, RentalModel rental);
        public void UpdateRental(RentalModel rental);
        List<RentalItemListModel> GetAllRentalItemsOf(string username);
    }
}
