using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FactoryMaintenanceAPI.Models.Dto
{
    public class CreateFactoryDto
    {
       
        public int Id { get; set; }

        [Required(ErrorMessage = "Factory name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Factory Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Factory Type is required.")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Country Id is required.")]
        public int CountryId { get; set; }

    }
}
