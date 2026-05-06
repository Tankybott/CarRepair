using AutoMapper;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Mapping
{
    public class GlobalMappingProfile : Profile
    {
        public GlobalMappingProfile()
        {
            CreateMap<ServiceType, ServiceTypeDto>().ReverseMap();

            CreateMap<Model.DomainModel.Service, IntranetServiceReadDto>()
                .ForMember(d => d.ServiceName, opt => opt.MapFrom(s => s.ShortDescription))
                .ForMember(d => d.ServiceType, opt => opt.MapFrom(s => s.ServiceType.Name));

            CreateMap<IntranetServiceUpsertDto, Model.DomainModel.Service>();

            CreateMap<Part, IntranetPartReadDto>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.StatusValue, opt => opt.MapFrom(s => (int)s.Status));

            CreateMap<IntranetPartUpsertDto, Part>();

            CreateMap<ClientProfile, IntranetClientReadDto>()
                .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.Name + " " + s.Surname))
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.ApplicationUser != null ? s.ApplicationUser.Email : string.Empty));

            CreateMap<Car, IntranetCarReadDto>()
                .ForMember(d => d.OwnerFullName, opt => opt.MapFrom(s => s.Client != null ? s.Client.Name + " " + s.Client.Surname : string.Empty))
                .ForMember(d => d.FuelType, opt => opt.MapFrom(s => s.FuelType.ToString()))
                .ForMember(d => d.FuelTypeValue, opt => opt.MapFrom(s => (int)s.FuelType))
                .ForMember(d => d.Transmission, opt => opt.MapFrom(s => s.Transmission.ToString()))
                .ForMember(d => d.TransmissionValue, opt => opt.MapFrom(s => (int)s.Transmission))
                .ForMember(d => d.BodyType, opt => opt.MapFrom(s => s.BodyType.ToString()))
                .ForMember(d => d.BodyTypeValue, opt => opt.MapFrom(s => (int)s.BodyType));

            CreateMap<IntranetCarUpsertDto, Car>();

            CreateMap<Repair, IntranetRepairReadDto>()
                .ForMember(d => d.CarLabel, opt => opt.MapFrom(s => s.Car != null ? $"{s.Car.Brand} {s.Car.Model} ({s.Car.Year})" : string.Empty))
                .ForMember(d => d.ClientFullName, opt => opt.MapFrom(s => s.Car != null && s.Car.Client != null ? s.Car.Client.Name + " " + s.Car.Client.Surname : string.Empty))
                .ForMember(d => d.ClientName, opt => opt.MapFrom(s => s.Car != null && s.Car.Client != null ? s.Car.Client.Name : string.Empty))
                .ForMember(d => d.ClientSurname, opt => opt.MapFrom(s => s.Car != null && s.Car.Client != null ? s.Car.Client.Surname : string.Empty))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.ServiceNames, opt => opt.MapFrom(s => s.ServicesSelected.Select(svc => svc.ShortDescription).ToList()));

            CreateMap<EmployeeProfile, IntranetEmployeeReadDto>()
                .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.Name + " " + s.Surname))
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.ApplicationUser != null ? s.ApplicationUser.Email : string.Empty))
                .ForMember(d => d.SpecializationIds, opt => opt.MapFrom(s => s.Specializations.Select(st => st.Id).ToList()))
                .ForMember(d => d.SpecializationNames, opt => opt.MapFrom(s => s.Specializations.Select(st => st.Name).ToList()));

            CreateMap<WorkTask, IntranetWorkTaskReadDto>()
                .ForMember(d => d.PredictedEnd, opt => opt.MapFrom(s => s.PredicetedEnd))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.StatusValue, opt => opt.MapFrom(s => (int)s.Status))
                .ForMember(d => d.ServiceId, opt => opt.MapFrom(s => s.ServiceId))
                .ForMember(d => d.AssignedEmployeeNames, opt => opt.MapFrom(s =>
                    s.EmployeesAssigned.Select(e => ((e.Name ?? string.Empty) + " " + (e.Surname ?? string.Empty)).Trim()).ToList()))
                .ForMember(d => d.AssignedEmployeeIds, opt => opt.MapFrom(s =>
                    s.EmployeesAssigned.Select(e => e.Id).ToList()));

            CreateMap<CostEstimationItem, IntranetCostEstimationItemReadDto>()
                .ForMember(d => d.Type, opt => opt.MapFrom(s => s.Type.ToString()))
                .ForMember(d => d.TypeValue, opt => opt.MapFrom(s => (int)s.Type));

            CreateMap<Repair, IntranetRepairManageDto>()
                .ForMember(d => d.RepairId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.StatusValue, opt => opt.MapFrom(s => (int)s.Status))
                .ForMember(d => d.CarBrand, opt => opt.MapFrom(s => s.Car.Brand))
                .ForMember(d => d.CarModel, opt => opt.MapFrom(s => s.Car.Model))
                .ForMember(d => d.CarYear, opt => opt.MapFrom(s => s.Car.Year))
                .ForMember(d => d.CarVIN, opt => opt.MapFrom(s => s.Car.VIN))
                .ForMember(d => d.CarEngineCode, opt => opt.MapFrom(s => s.Car.EngineCode))
                .ForMember(d => d.CarFuelType, opt => opt.MapFrom(s => s.Car.FuelType.ToString()))
                .ForMember(d => d.CarTransmission, opt => opt.MapFrom(s => s.Car.Transmission.ToString()))
                .ForMember(d => d.CarBodyType, opt => opt.MapFrom(s => s.Car.BodyType.ToString()))
                .ForMember(d => d.ClientName, opt => opt.MapFrom(s => s.Car.Client.Name))
                .ForMember(d => d.ClientSurname, opt => opt.MapFrom(s => s.Car.Client.Surname))
                .ForMember(d => d.ClientFullName, opt => opt.MapFrom(s => s.Car.Client.Name + " " + s.Car.Client.Surname))
                .ForMember(d => d.ClientEmail, opt => opt.MapFrom(s => s.Car.Client.ApplicationUser != null ? s.Car.Client.ApplicationUser.Email : string.Empty))
                .ForMember(d => d.Services, opt => opt.MapFrom(s => s.ServicesSelected))
                .ForMember(d => d.Parts, opt => opt.MapFrom(s => s.PartsUsed.Where(p => p.DeletedAt == null).ToList()))
                .ForMember(d => d.CostEstimations, opt => opt.MapFrom(s => s.CostEstimationCollection.Where(c => c.DeletedAt == null).ToList()));
        }
    }
}
