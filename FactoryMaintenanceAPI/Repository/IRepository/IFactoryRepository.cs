using FactoryMaintenanceAPI.Models;
using static FactoryMaintenanceAPI.Models.Factory;

namespace FactoryMaintenanceAPI.Repository.IRepository
{
    public interface IFactoryRepository
    {

        ICollection<Factory> Index();

        Factory Show(int id);

        bool exists(string name);

        bool exists(Factory factory);

        bool Store(Factory factory);

        bool Update(Factory factory);

        bool Delete(Factory factory);

        ICollection<Factory> ShowFactoriesByIds(string Ids);

        ICollection<Factory> ShowFactoriesByType(string type);

        ICollection<Factory> ShowFactoriesByCountry(int countryId);

        bool Save();
    }
}
