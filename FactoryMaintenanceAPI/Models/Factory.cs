using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryMaintenanceAPI.Models
{
    public class Factory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }


        [Required]
        public string Type { get; set; }

        [Required]
        public DateTime CreationDtm { get; set; }

        [Required]
        public DateTime UpdatedDtm { get; set; }

        [ForeignKey("Id")]
        public int CountryId { get; set; }
        public Country Country { get; set; }

    }
}
