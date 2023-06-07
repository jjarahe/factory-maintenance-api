using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.ConstrainedExecution;

namespace FactoryMaintenanceAPI.Models
{
    public class MaintenanceChore
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public DateTime ScheduleDate { get; set; }



        public DateTime ExecutionDate { get; set; }


        public string Result { get; set; }


        [ForeignKey("machineId")]
        public int MachineId { get; set; }

        public Machine Machine { get; set; }


    }
}
