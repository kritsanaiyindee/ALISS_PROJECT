using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ALISS.LoginManagement.DTO;
using ALISS.LoginManagement.Library.Models;

namespace ALISS.LoginManagement.Library.AutoMapperConfiguration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<MenuData, MenuDataDTO>()
                .ReverseMap();
        }
    }
}
