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
            // ServiceType mappings
            CreateMap<ServiceType, ServiceTypeDto>().ReverseMap();
        }
    }
}
