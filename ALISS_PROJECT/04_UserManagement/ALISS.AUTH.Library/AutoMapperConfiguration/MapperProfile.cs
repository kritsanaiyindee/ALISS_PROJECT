using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ALISS.AUTH.DTO;
using ALISS.AUTH.Library.Models;

namespace ALISS.AUTH.Library.AutoMapperConfiguration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<TCMenu, MenuDTO>()
                .ReverseMap();
            CreateMap<TBConfig, ColumnConfigDTO>()
                .ReverseMap();
            CreateMap<TCRole, RoleDTO>()
                .ReverseMap();
            CreateMap<TCPasswordConfig, PasswordConfigDTO>()
                .ReverseMap();
        }
    }
}
