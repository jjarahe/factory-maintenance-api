using AutoMapper;
using FactoryMaintenanceAPI.Models;
using FactoryMaintenanceAPI.Models.Dto;
using FactoryMaintenanceAPI.Models.Dto.MaintenanceChore;
using FactoryMaintenanceAPI.Models.Dto.Factory;
using FactoryMaintenanceAPI.Models.Dto.Machine;

namespace FactoryMaintenanceAPI.ModelsDtoMapper
{
    public class ModelsDtoMapper : Profile
    {
        public ModelsDtoMapper() {
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Factory, FactoryDto>().ReverseMap();
            CreateMap<Factory, CreateFactoryDto>().ReverseMap();
            CreateMap<Machine, MachineDto>().ReverseMap();
            CreateMap<Machine, CreateMachineDto>().ReverseMap();
            CreateMap<MaintenanceChore, CreateMaintenanceChoreDto>().ReverseMap();
            CreateMap<MaintenanceChore, MaintenanceChoreDto>().ReverseMap();
        }
        
    }
}
