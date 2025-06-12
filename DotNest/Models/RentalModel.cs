using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotNest.Models
{
    public class RentalModel
    {
        public int? Id { get; set; }

        public int UserId { get; set; }


        [Length(10, 100), DisplayName("Nom")]
        public string Name { get; set; }

        [Length(20, 2000)]
        public string Description { get; set; }

        /*[RegularExpression("((\\p{L})+[- ]?)*")] but it does not work with ModelState.IsValid because it becomes a javascript regex
         and javascript cannot handle \p{L}*/
        [Length(2, 100), DisplayName("Ville")] 
        public string City { get; set; }


        public int? PictureId { get; set; }
        public string? PictureContent { get; set; }

        public IFormFile? Picture { get; set; }
    }
}
