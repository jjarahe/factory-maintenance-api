using FactoryMaintenanceAPI.Data;
using FactoryMaintenanceAPI.Models;
using FactoryMaintenanceAPI.Repository.IRepository;
using RESTCountries.NET.Services;

namespace FactoryMaintenanceAPI.Repository
{
    public class CountryRepository : ICountryRepository
    {

        private readonly ApplicationDbContext _db;
        private readonly HttpClient _httpClient;
     

        public CountryRepository(ApplicationDbContext db, HttpClient httpClient) 
        { 
            _db = db;
            _httpClient = httpClient;
        }


        public bool GetAndSaveCountries()
        {
            //DeleteAll();

            var countries = RestCountriesService.GetAllCountries().ToList();

            foreach (var country in countries) {
                var countryTemp = new Country();
                countryTemp.Name = country.Name.Common;
                countryTemp.CountryCode = country.Cioc;
                Store(countryTemp);
            }

            return Save();

        }


        public bool Delete(Country country)
        {
            _db.Countries.Remove(country);
            return Save();
        }

        public bool DeleteAll()
        {
            var countries = _db.Countries.OrderBy(c => c.Name).ToList();
            _db.Countries.RemoveRange(countries);
            return Save();
        }

        //public bool exists(string countryCode)
        //{
        //    return _db.Countries.Any(c => c.CountryCode.ToLower().Trim() == countryCode.ToLower().Trim());
        //}

        //public bool exists(Country country)
        //{
        //    bool result = _db.Countries.Any(c => c.CountryCode == country.CountryCode);
        //    return result;
        //}

        public ICollection<Country> Index()
        {
           return _db.Countries.OrderBy(c => c.Name).ToList();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public Country Show(int id)
        {
            return _db.Countries.FirstOrDefault(c => c.Id == id);
        }

        public bool Store(Country country)
        {
            country.CreationDtm = DateTime.Now;
            _db.Countries.AddAsync(country);
            return Save();
        }

        //public bool Update(Country country)
        //{
        //    country.UpdateDtm = DateTime.Now;
        //    _db.Countries.Update(country);
        //    return Save();
        //}
    }
}
