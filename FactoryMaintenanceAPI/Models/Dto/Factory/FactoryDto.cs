using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FactoryMaintenanceAPI.Models.Dto.Factory
{
    public class FactoryDto
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Factory name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Factory description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Factory type is required.")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Country id is required.")]
        public int CountryId { get; set; }

    }
}
