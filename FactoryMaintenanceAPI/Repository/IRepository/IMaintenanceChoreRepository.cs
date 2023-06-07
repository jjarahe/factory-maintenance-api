using FactoryMaintenanceAPI.Models;

namespace FactoryMaintenanceAPI.Repository.IRepository
{
    public interface IMaintenanceChoreRepository
    {
        ICollection<MaintenanceChore> Index();

        MaintenanceChore Show(int id);

        bool exists(int id);

        bool exists(MaintenanceChore maintenanceChore);

        bool Store(MaintenanceChore maintenanceChore);

        bool Update(MaintenanceChore maintenanceChore);

        ICollection<MaintenanceChore> ShowMaintenanceChoresByIds(string Ids);

        ICollection<MaintenanceChore> ShowMaintenanceChoresByMachineId(int machineId);

        bool Save();

    }
}
