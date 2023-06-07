using FactoryMaintenanceAPI.Models;

namespace FactoryMaintenanceAPI.Repository.IRepository
{
    public interface IMachineRepository
    {

        ICollection<Machine> Index();

        Machine Show(int id);

        bool exists(string name);

        bool exists(Machine machine);

        bool Store(Machine machine);

        bool Update(Machine machine);

        bool Delete(Machine machine);

        ICollection<Machine> ShowMachinesByIds(string Ids);

        ICollection<Machine> ShowMachinesByFactoryId(int factoryId);

        bool Save();

    }
}
