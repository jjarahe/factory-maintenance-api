using AutoMapper;
using FactoryMaintenanceAPI.Models;
using FactoryMaintenanceAPI.Models.Dto;
using FactoryMaintenanceAPI.Models.Dto.Machine;
using FactoryMaintenanceAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using RESTCountries.NET.Models;

namespace FactoryMaintenanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class MachinesController : ControllerBase
    {
        private readonly IMachineRepository _machineRepository;
        private readonly IMapper _mapper;

        public MachinesController(IMachineRepository machineRepository, IMapper mapper)
        {
            _machineRepository = machineRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Index([FromQuery] String? machineIds, [FromQuery] String? factoryId)
        {
            try {

                if (machineIds != null) {
                    return ShowMachinesByIds(machineIds);
                }

                if (factoryId != null)
                {
                    ShowMachinesByFactory(int.Parse(factoryId));
                }

                var machines = _machineRepository.Index();
                var machinesDto = new List<MachineDto>();

                foreach (var machine in machines)
                {
                    machinesDto.Add(_mapper.Map<MachineDto>(machine));
                }

                return Ok(machinesDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong while showing the machines {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

        }


        [HttpGet("{id:int}", Name = "ShowMachine")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Show(int id)
        {
            try {
                var machine = _machineRepository.Show(id);

                if (machine == null)
                {
                    return NotFound();
                }

                var machineDto = _mapper.Map<MachineDto>(machine);

                return Ok(machineDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong while showing the machine {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }
        }

        //[HttpGet("MachinesByIds", Name = "ShowMachinesByIds")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        private IActionResult ShowMachinesByIds(string ids)
        {
            try {
                var machines = _machineRepository.ShowMachinesByIds(ids);

                if (machines == null)
                {
                    return NotFound();
                }

                var machinesDto = new List<MachineDto>();

                foreach (var machine in machines)
                {
                    machinesDto.Add(_mapper.Map<MachineDto>(machine));
                }

                return Ok(machinesDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong while showing the machines {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }


        }

        //[HttpGet("{factoryId:int}", Name = "ShowMachinesByFactory")]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        private IActionResult ShowMachinesByFactory(int factoryId)
        {
            try {
                var machines = _machineRepository.ShowMachinesByFactoryId(factoryId);

                if (machines == null)
                {
                    return NotFound();
                }

                var machinesDto = new List<MachineDto>();

                foreach (var machine in machines)
                {
                    machinesDto.Add(_mapper.Map<MachineDto>(machine));
                }

                return Ok(machinesDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong while showing the machines {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }


        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CreateMachineDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Store([FromBody] CreateMachineDto machineDto)
        {
            try
            {
                if (!ModelState.IsValid || machineDto == null)
                {
                    return BadRequest(ModelState);
                }

                if (_machineRepository.exists(machineDto.Name))
                {
                    ModelState.AddModelError("", "Machine already exists");
                    return StatusCode(StatusCodes.Status404NotFound, ModelState);
                }

                var machine = _mapper.Map<Machine>(machineDto);

                if (!_machineRepository.Store(machine))
                {
                    ModelState.AddModelError("", $"Something went wrong while saving {machine.Name}");
                    return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
                }

                return CreatedAtRoute("ShowMachine", new { id = machine.Id }, machineDto);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong while saving the machine {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }


        }

        [HttpPatch("{id:int}", Name = "UpdateMachine")]
        [ProducesResponseType(201, Type = typeof(MachineDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Update(int id, [FromBody] MachineDto machineDto) 
        {
            try {
                if (isBadRequest(machineDto, id))
                {
                    return BadRequest(ModelState);
                }

                var machine = _mapper.Map<Machine>(machineDto);

                if (!_machineRepository.Update(machine))
                {
                    ModelState.AddModelError("", $"Something went wrong while updating {machine.Name}");
                    return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong while updating the machine {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }


        }

        [HttpDelete("{id:int}", Name = "DeleteMachine")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id) 
        {
            try {
                var machine = _machineRepository.Show(id);

                if (machine == null)
                {
                    return NotFound();
                }

                if (!_machineRepository.Delete(machine))
                {
                    ModelState.AddModelError("", $"Something went wrong while deleting {machine.Name}");
                    return StatusCode(500, ModelState);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong while deleting the machine {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }


        }


        private bool isBadRequest(MachineDto machineDto, int id)
        {
            return !ModelState.IsValid || machineDto == null || machineDto.Id != id;
        }


    }
}
