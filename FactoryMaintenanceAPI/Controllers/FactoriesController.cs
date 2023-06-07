using AutoMapper;
using FactoryMaintenanceAPI.Models;
using FactoryMaintenanceAPI.Models.Dto;
using FactoryMaintenanceAPI.Models.Dto.Factory;
using FactoryMaintenanceAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;


namespace FactoryMaintenanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FactoriesController : ControllerBase
    {
        private readonly IFactoryRepository _factoryRepository;
        private readonly IMapper _mapper;

        public FactoriesController(IFactoryRepository factoryRepository, IMapper mapper)
        {
            _factoryRepository = factoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Index([FromQuery] String? factoryIds, [FromQuery] String? countryId, [FromQuery] String? factoryType)
        {
            try {

                if (factoryIds != null) { 
                    return ShowFactoriesByIds(factoryIds);
                }

                if (countryId != null)
                {
                    return ShowFactoriesByCountry(int.Parse(countryId));
                }

                if (factoryType != null)
                {
                    return ShowFactoriesByType(factoryType);
                }

                var factories = _factoryRepository.Index();
                var factoriesDto = new List<FactoryDto>();
                foreach (var factory in factories)
                {
                    factoriesDto.Add(_mapper.Map<FactoryDto>(factory));
                }
                return Ok(factoriesDto);

            } catch(Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong while showing the factories {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

        }

        [HttpGet("{id:int}", Name = "ShowFactory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Show(int id)
        {
            try {
                var factory = _factoryRepository.Show(id);

                if (factory == null)
                {
                    return NotFound();
                }

                var factoryDto = _mapper.Map<FactoryDto>(factory);

                return Ok(factoryDto);
            } catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong while showing the factory {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

        }

        
        private IActionResult ShowFactoriesByIds(string ids)
        {
            var factories = _factoryRepository.ShowFactoriesByIds(ids);

            if (factories == null)
            {
                return NotFound();
            }

            var factoriesDto = new List<FactoryDto>();

                foreach (var factory in factories)
                {
                    factoriesDto.Add(_mapper.Map<FactoryDto>(factory));
                }

                return Ok(factoriesDto);
        }
            
        private IActionResult ShowFactoriesByCountry(int countryId) 
        {
            try {
                var factories = _factoryRepository.ShowFactoriesByCountry(countryId);

                if (factories == null)
                {
                    return NotFound();
                }

                var factoriesDto = new List<FactoryDto>();

                foreach (var factory in factories)
                {
                    factoriesDto.Add(_mapper.Map<FactoryDto>(factory));
                }

                return Ok(factoriesDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong while showing the factories {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }
        }

        private IActionResult ShowFactoriesByType(string type)
        {
            try {
                var factories = _factoryRepository.ShowFactoriesByType(type);

                if (factories == null)
                {
                    return NotFound();
                }

                var factoriesDto = new List<FactoryDto>();

                foreach (var factory in factories)
                {
                    factoriesDto.Add(_mapper.Map<FactoryDto>(factory));
                }

                return Ok(factoriesDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong while showing the factories {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }
        }


        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CreateFactoryDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Store([FromBody] CreateFactoryDto factoryDto)
        {
            try {
                if (!ModelState.IsValid || factoryDto == null)
                {
                    return BadRequest(ModelState);
                }

                if (_factoryRepository.exists(factoryDto.Name))
                {
                    ModelState.AddModelError("", "Factory already exists");
                    return StatusCode(404, ModelState);
                }

                var factory = _mapper.Map<Factory>(factoryDto);

                if (!_factoryRepository.Store(factory))
                {
                    ModelState.AddModelError("", $"Something went wrong while saving {factory.Name}");
                    return StatusCode(500, ModelState);
                }

                return CreatedAtRoute("ShowFactory", new { id = factory.Id }, factoryDto);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong while saving the factory {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }


        }

        [HttpPatch("{id:int}", Name = "UpdateFactory")]
        [ProducesResponseType(201, Type = typeof(FactoryDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Update(int id, [FromBody] FactoryDto factoryDto)
        {
            try {
                if (isBadRequest(factoryDto, id))
                {
                    return BadRequest();
                }

                var factory = _mapper.Map<Factory>(factoryDto);

                if (!_factoryRepository.Update(factory))
                {
                    ModelState.AddModelError("", $"Something went wrong while updating {factory.Name}");
                    return StatusCode(500, ModelState);
                }

                return CreatedAtRoute("ShowFactory", new { id = factory.Id }, factoryDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong while updating the factory {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }


        }

        [HttpDelete("{id:int}", Name = "DeleteFactory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            try {
                var factory = _factoryRepository.Show(id);

                if (factory == null)
                {
                    return NotFound();
                }

                if (!_factoryRepository.Delete(factory))
                {
                    ModelState.AddModelError("", $"Something went wrong while deleting {factory.Name}");
                    return StatusCode(500, ModelState);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong while deleting the factory {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

        }

        private bool isBadRequest(FactoryDto factoryDto, int id)
        {
            return !ModelState.IsValid || factoryDto == null || factoryDto.Id != id;
        }

    }
}
