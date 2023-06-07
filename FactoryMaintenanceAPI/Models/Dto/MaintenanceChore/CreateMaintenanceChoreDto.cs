using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FactoryMaintenanceAPI.Models.Dto.MaintenanceChore
{
    public class CreateMaintenanceChoreDto
    {
        [Required(ErrorMessage = "Maintenance Chore description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Schedule date is required")]
        public DateTime ScheduleDate { get; set; }

        [Required(ErrorMessage = "Machine id is required")]
        public int MachineId { get; set; }

    }
}
