using AutoMapper;
using FactoryMaintenanceAPI.Models;
using FactoryMaintenanceAPI.Models.Dto.MaintenanceChore;
using FactoryMaintenanceAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using RESTCountries.NET.Models;


namespace FactoryMaintenanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceChoresController : ControllerBase
    {
        private readonly IMaintenanceChoreRepository _maintenanceChoreRepository;
        private readonly IMapper _mapper;

        public MaintenanceChoresController(IMaintenanceChoreRepository maintenanceChoreRepository, IMapper mapper)
        {
            _maintenanceChoreRepository = maintenanceChoreRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Index([FromQuery] String? maintenanceChoreIds, [FromQuery] String? machineId) 
        {
            try {

                if (maintenanceChoreIds != null) {
                    return ShowMaintenanceChoresByIds(maintenanceChoreIds);
                }
                if (maintenanceChoreIds != null)
                {
                   return ShowMaintenaceChoresByMachine(int.Parse(machineId));
                }


                var maintenanceChores = _maintenanceChoreRepository.Index();
                var maintenanceChoresDto = new List<MaintenanceChoreDto>();

                foreach (var chore in maintenanceChores)
                {
                    maintenanceChoresDto.Add(_mapper.Map<MaintenanceChoreDto>(chore));
                }
                return Ok(maintenanceChoresDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong while showing the maintenance chores {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

        }

        [HttpGet("{id:int}", Name = "ShowMaintenanceChore")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Show(int id)
        {
            try {
                var maintenanceChore = _maintenanceChoreRepository.Show(id);

                if (maintenanceChore == null)
                {
                    return NotFound();
                }

                var maintenanceChoreDto = _mapper.Map<MaintenanceChoreDto>(maintenanceChore);

                return Ok(maintenanceChore);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong while showing the maintenance chore {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

        }

        //[HttpGet("{Ids}", Name = "ShowMaintenanceChoresByIds")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        private IActionResult ShowMaintenanceChoresByIds(string ids) 
        {
            try {
                var maintenanceChores = _maintenanceChoreRepository.ShowMaintenanceChoresByIds(ids);

                if (maintenanceChores == null)
                {
                    return NotFound();
                }

                var maintenanceChoresDto = new List<MaintenanceChoreDto>();

                foreach (var chore in maintenanceChores)
                {
                    maintenanceChoresDto.Add(_mapper.Map<MaintenanceChoreDto>(chore));
                }

                return Ok(maintenanceChoresDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong while showing the maintenance chores {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }


        }

        //[HttpGet("{machineId:int}", Name = "ShowMaintenaceChoresByMachine")]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        private IActionResult ShowMaintenaceChoresByMachine(int machineId) 
        {
            try {
                var maintenanceChores = _maintenanceChoreRepository.ShowMaintenanceChoresByMachineId(machineId);

                if (maintenanceChores == null)
                {
                    return NotFound();
                }

                var maintenanceChoresDto = new List<MaintenanceChoreDto>();

                foreach (var chore in maintenanceChores)
                {
                    maintenanceChoresDto.Add(_mapper.Map<MaintenanceChoreDto>(chore));
                }

                return Ok(maintenanceChoresDto);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong while showing the maintenance chores {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(MaintenanceChoreDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Store([FromBody] MaintenanceChoreDto maintenanceChoreDto)
        {
            try
            {
                if (!ModelState.IsValid || maintenanceChoreDto == null)
                {
                    return BadRequest(ModelState);
                }

                if (_maintenanceChoreRepository.exists(maintenanceChoreDto.Id))
                {
                    ModelState.AddModelError("", "MaintenanceChore already exists");
                    return StatusCode(StatusCodes.Status404NotFound, ModelState);
                }

                var maintenanceChore = _mapper.Map<MaintenanceChore>(maintenanceChoreDto);

                if (!_maintenanceChoreRepository.Store(maintenanceChore))
                {
                    ModelState.AddModelError("", $"Something went wrong while saving {maintenanceChore.Description}");
                    return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
                }

                return Ok(ModelState);

            } catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong while saving {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
                
            } 
        }

        [HttpPatch("{id:int}", Name = "UpdateMaintenanceChore")]
        [ProducesResponseType(201, Type = typeof(MaintenanceChoreDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Update(int id, [FromBody] MaintenanceChoreDto maintenanceChoreDto)
        {
            try {
                if (isBadRequest(maintenanceChoreDto, id))
                {
                    return BadRequest(ModelState);
                }

                if (isValidScheduleDate(maintenanceChoreDto))
                {
                    ModelState.AddModelError("", $"Schedule Date should not be a past date {maintenanceChoreDto.ScheduleDate} Creation Date: {maintenanceChoreDto.CreationDate}");
                    return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
                }

                if (canUpdate(maintenanceChoreDto))
                {
                    ModelState.AddModelError("", $"Update not allowed when chore is complete {maintenanceChoreDto.ScheduleDate} Creation Date: {maintenanceChoreDto.CreationDate}");
                    return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
                }

                var maintenanceChore = _mapper.Map<MaintenanceChore>(maintenanceChoreDto);

                if (!_maintenanceChoreRepository.Update(maintenanceChore))
                {
                    ModelState.AddModelError("", $"Something went wrong while updating {maintenanceChore.Description}");
                    return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong while updating the maintenance chores {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);

            }
        }


        private bool isBadRequest( MaintenanceChoreDto maintenanceChoreDto,int id) 
        {
            return !ModelState.IsValid || maintenanceChoreDto == null || maintenanceChoreDto.Id != id;
        }

        private bool isValidScheduleDate(MaintenanceChoreDto maintenanceChoreDto)
        {
            return maintenanceChoreDto.ScheduleDate.Date < maintenanceChoreDto.CreationDate.Date;
        }

        private bool canUpdate(MaintenanceChoreDto maintenanceChoreDto) {
            return _maintenanceChoreRepository.exists(maintenanceChoreDto.Id) && _maintenanceChoreRepository.Show(maintenanceChoreDto.Id).Status == "Complete";
        }

    }
}
