using FactoryMaintenanceAPI.Models;

namespace FactoryMaintenanceAPI.Repository.IRepository
{
    public interface ICountryRepository
    {
        //Task<List<Country>> GetAndSaveCountriesAsync();

        bool GetAndSaveCountries();

        ICollection<Country> Index();
        
        Country Show(int id);

        //bool exists(string countryCode);

        //bool exists(Country country);

        bool Store(Country country);

       // bool Update(Country country);

       // bool Delete(Country country);

        bool Save();


    }
}
