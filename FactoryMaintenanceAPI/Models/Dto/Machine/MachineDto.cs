using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FactoryMaintenanceAPI.Models.Dto.Machine
{
    public class MachineDto
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Machine name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Machine description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Factory id is required.")]
        public int FactoryId { get; set; }


    }
}
