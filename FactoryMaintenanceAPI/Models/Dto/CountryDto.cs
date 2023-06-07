using System.ComponentModel.DataAnnotations;

namespace FactoryMaintenanceAPI.Models.Dto
{
    public class CountryDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Country's name is required")]
        [MaxLength(100, ErrorMessage = "Country's name must have less than 101 characters")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Country's code is required")]
        public string CountryCode { get; set; }

    }
}
