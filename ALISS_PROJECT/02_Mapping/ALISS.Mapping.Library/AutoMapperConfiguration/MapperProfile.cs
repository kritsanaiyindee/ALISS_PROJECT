using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ALISS.Mapping.DTO;
using ALISS.Mapping.Library.Models;

namespace ALISS.Mapping.Library.AutoMapperConfiguration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<TRMapping, MappingDataDTO>();
            CreateMap<MappingDataDTO, TRMapping>();

            //CreateMap<TRMapping, MappingDataDTO>();
            //CreateMap<MappingDataDTO, TRMapping>();
            
            CreateMap<TRWHONetMapping, WHONetMappingDataDTO>();
            CreateMap<WHONetMappingDataDTO, TRWHONetMapping>();

            CreateMap<TRSpecimenMapping, SpecimenMappingDataDTO>();
            CreateMap<SpecimenMappingDataDTO, TRSpecimenMapping>();

            CreateMap<TROrganismMapping, OrganismMappingDataDTO>();
            CreateMap<OrganismMappingDataDTO, TROrganismMapping>();

            CreateMap<TRWardTypeMapping, WardTypeMappingDataDTO>();
            CreateMap<WardTypeMappingDataDTO, TRWardTypeMapping>();


        }
    }
}
