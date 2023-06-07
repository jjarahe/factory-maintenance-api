using FactoryMaintenanceAPI.Data;
using FactoryMaintenanceAPI.Models;
using FactoryMaintenanceAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FactoryMaintenanceAPI.Repository
{
    public class MaintenanceChoreRepository : IMaintenanceChoreRepository
    {
        private readonly ApplicationDbContext _db;


        public MaintenanceChoreRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool exists(int id)
        {
            return _db.MaintenanceChores.Any(c => c.Id == id);
        }

        public bool exists(MaintenanceChore maintenanceChore)
        {
            return _db.MaintenanceChores.Any(m => m.Id == maintenanceChore.Id);
        }

        public ICollection<MaintenanceChore> Index()
        {
            return _db.MaintenanceChores.Include(machine => machine.Machine).OrderBy(m => m.MachineId).ToList();
        }

        public bool Save()
        {
            
            return _db.SaveChanges() >= 0 ? true : false;
      
        }

        public MaintenanceChore Show(int id)
        {
            return _db.MaintenanceChores.Include(machine => machine.Machine).First(m => m.Id == id);
        }

        public ICollection<MaintenanceChore> ShowMaintenanceChoresByIds(string Ids)
        {
            var listOfIds = new List<int>();

            foreach (string id in Ids.Split(','))
            {
                listOfIds.Add(int.Parse(id));
            }

            return _db.MaintenanceChores.Include(machine => machine.Machine).Where(m => listOfIds.Contains(m.Id)).ToList();
        }

        public ICollection<MaintenanceChore> ShowMaintenanceChoresByMachineId(int machineId)
        {
            return _db.MaintenanceChores.Include(machine => machine.Machine).Where(maintenance => maintenance.MachineId == machineId).ToList();
        }

        public bool Store(MaintenanceChore maintenanceChore)
        {
            maintenanceChore.CreationDate = DateTime.Now;

            if (maintenanceChore.CreationDate.Date == maintenanceChore.ScheduleDate.Date) {
                maintenanceChore.ExecutionDate = maintenanceChore.ScheduleDate;
                maintenanceChore.Status = "In Progress";
            }
            
            _db.MaintenanceChores.Add(maintenanceChore);
            return Save();

        }

        public bool Update(MaintenanceChore maintenanceChore)
        {
            _db.MaintenanceChores.Update(maintenanceChore);   
            
            return Save();
        }
    }
}
