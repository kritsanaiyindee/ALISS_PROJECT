using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ALISS.Master.DTO;
using ALISS.TC.WHONET_Antibiotics.Models;

namespace ALISS.Master.WHONET.Library.AutoMapperConfiguration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<TCWHONET_AntibioticsDTO, TCWHONET_Antibiotics>()
                .ReverseMap();
        }
    }
}
