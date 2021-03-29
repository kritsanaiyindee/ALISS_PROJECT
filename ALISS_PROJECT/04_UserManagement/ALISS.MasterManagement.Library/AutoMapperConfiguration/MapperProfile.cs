using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ALISS.MasterManagement.DTO;
using ALISS.MasterManagement.Library.Models;

namespace ALISS.MasterManagement.Library.AutoMapperConfiguration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<TCHospital, MasterHospitalDTO>()
                .ReverseMap();
            CreateMap<TCMasterTemplate, MasterTemplateDTO>()
                .ReverseMap();
            CreateMap<TCAntibiotic, AntibioticDTO>()
                .ReverseMap();
            CreateMap<TCExpertRule, ExpertRuleDTO>()
                .ReverseMap();
            CreateMap<TCOrganism, OrganismDTO>()
                .ReverseMap();
            CreateMap<TCQCRange, QCRangeDTO>()
                .ReverseMap();
            CreateMap<TCSpecimen, SpecimenDTO>()
                .ReverseMap();
            CreateMap<TCWardType, WardTypeDTO>()
                .ReverseMap();
            CreateMap<TCWHONETColumn, WHONETColumnDTO>()
                .ReverseMap();
        }
    }
}
