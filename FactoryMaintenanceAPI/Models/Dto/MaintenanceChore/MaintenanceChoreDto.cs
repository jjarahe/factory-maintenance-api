using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FactoryMaintenanceAPI.Models;

namespace FactoryMaintenanceAPI.Models.Dto.MaintenanceChore
{
    public class MaintenanceChoreDto
    {
        
        public int Id { get; set; }

       
        public string? Description { get; set; }

        
        public string? Status { get; set; }

        
        public DateTime CreationDate { get; set; }

        
        public DateTime ScheduleDate { get; set; }


        public string? Result { get; set; }

        
        public int MachineId { get; set; }


    }
}
