using FactoryMaintenanceAPI.Data;
using FactoryMaintenanceAPI.Models;
using FactoryMaintenanceAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FactoryMaintenanceAPI.Repository
{
    public class MachineRepository : IMachineRepository
    {
        private readonly ApplicationDbContext _db;

        public MachineRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool Delete(Machine machine)
        {
            _db.Machines.Remove(machine);
            return Save();  
        }

        public bool exists(string name)
        {
            return _db.Machines.Any(m => m.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool exists(Machine machine)
        {
            return _db.Machines.Any(m => m.Id == machine.Id);
        }

        public ICollection<Machine> Index()
        {
            return _db.Machines.OrderBy(m => m.Id).ToList();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public Machine Show(int id)
        {
            return _db.Machines.Include(f => f.Factory).First(m => m.Id == id);
        }

        public ICollection<Machine> ShowMachinesByFactoryId(int factoryId)
        {
            return _db.Machines.Include(f => f.Factory).Where(m => m.FactoryId == factoryId).ToList();
        }

        public ICollection<Machine> ShowMachinesByIds(string Ids)
        {
            var listOfIds = new List<int>();

            foreach (string id in Ids.Split(','))
            {
                listOfIds.Add(int.Parse(id));
            }

            return _db.Machines.Where(m => listOfIds.Contains(m.Id)).ToList();
        }

        public bool Store(Machine machine)
        {
            machine.CreationDtm = DateTime.Now;
            _db.Machines.Add(machine);
            return Save();
        }

        public bool Update(Machine machine)
        {
           machine.UpdatedDtm = DateTime.Now;
            _db.Machines.Update(machine); 
            return Save();
        }
    }
}
