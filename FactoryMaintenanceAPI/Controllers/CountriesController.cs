using AutoMapper;
using FactoryMaintenanceAPI.Models.Dto;
using FactoryMaintenanceAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Xml.Serialization;

namespace FactoryMaintenanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        private static bool isMethodGetAndSaveCountriesExecuted = false;

        public CountriesController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }


        [HttpGet(Name = "Index")]
        [ResponseCache(CacheProfileName = "DefaultCacheProfile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public  IActionResult Index([FromQuery] bool xml)
        {
            try {
                if (!isMethodGetAndSaveCountriesExecuted)
                {
                    if (_countryRepository.GetAndSaveCountries())
                    {
                        isMethodGetAndSaveCountriesExecuted = true;
                    }

                }

                var countries = _countryRepository.Index();
                var countriesDto = new List<CountryDto>();

                foreach (var country in countries)
                {
                    countriesDto.Add(_mapper.Map<CountryDto>(country));
                }

                if (xml)
                {
                    return parseToXml(countriesDto);
                }

                return Ok(countriesDto);
            } catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong while showing the countries {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);

            }

        }


        [HttpGet("{id:int}", Name = "Show")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Show(int id, [FromQuery] bool xml )
        {
            try {
                var country = _countryRepository.Show(id);

                if (country == null)
                {
                    return NotFound();
                }

                var countryDto = _mapper.Map<CountryDto>(country);
                
                if (xml) {
                    return parseToXml(countryDto);
                }

                return Ok(countryDto);

            } catch(Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong while showing the country {ex.Message}");
               return StatusCode(StatusCodes.Status500InternalServerError, ModelState);

            }

        }

        private OkObjectResult parseToXml(CountryDto countryDto) {
                var serializer = new XmlSerializer(countryDto.GetType());
                var stringWriter = new StringWriter();
                serializer.Serialize(stringWriter, countryDto);
                return Ok(stringWriter.ToString());
        }

        private OkObjectResult parseToXml(List<CountryDto> countriesDto)
        {
            var serializer = new XmlSerializer(countriesDto.GetType());
            var stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, countriesDto);
            return Ok(stringWriter.ToString());
        }

    }
}
