using FactoryMaintenanceAPI.Data;
using FactoryMaintenanceAPI.Models;
using FactoryMaintenanceAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;


namespace FactoryMaintenanceAPI.Repository
{
    public class FactoryRepository : IFactoryRepository
    {
        private readonly ApplicationDbContext _db;

        public FactoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }


        public bool Delete(Factory factory)
        {
            _db.Factories.Remove(factory);
            return Save();
        }

        public bool exists(string name)
        {
            return _db.Factories.Any(factory => factory.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool exists(Factory factory)
        {
            return _db.Factories.Any(f => f.Id == factory.Id);
        }

        public ICollection<Factory> Index()
        {
            return _db.Factories.OrderBy(f => f.Type).ToList();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public Factory Show(int id)
        {
            return _db.Factories.First(f => f.Id == id);
        }

        public ICollection<Factory> ShowFactoriesByCountry(int countryId)
        {
            return _db.Factories.Include(c => c.Country).Where(f => f.CountryId == countryId).ToList();
        }

        public ICollection<Factory> ShowFactoriesByIds(string Ids) {

            var listOfIds = new List<int>();

            foreach (string id in Ids.Split(',')) { 
                listOfIds.Add(int.Parse(id));
            }

            return _db.Factories.Where(f => listOfIds.Contains(f.Id)).ToList();
        }

        public ICollection<Factory> ShowFactoriesByType(string type)
        {
            return _db.Factories.Where(f => f.Type.ToLower().Trim() == type.ToLower().Trim()).ToList();
        }

        public bool Store(Factory factory)
        {
            factory.CreationDtm = DateTime.Now;
            _db.Factories.Add(factory);
            return Save();
        }

        public bool Update(Factory factory)
        {
            factory.UpdatedDtm = DateTime.Now;
            _db.Factories.Update(factory);
            return Save();
        }
    }
}
