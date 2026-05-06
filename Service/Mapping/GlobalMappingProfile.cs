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
        }
    }
}
