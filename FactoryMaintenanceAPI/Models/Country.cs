using System.ComponentModel.DataAnnotations;

namespace FactoryMaintenanceAPI.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        
        public string? CountryCode { get; set; }

        [Required]
        public DateTime CreationDtm { get; set; }

        [Required]
        public DateTime UpdateDtm { get; set; }
    }
}
