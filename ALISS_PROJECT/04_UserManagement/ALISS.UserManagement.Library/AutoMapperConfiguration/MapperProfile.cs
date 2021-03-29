using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ALISS.UserManagement.DTO;
using ALISS.UserManagement.Library.Models;

namespace ALISS.UserManagement.Library.AutoMapperConfiguration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<TRHospital, HospitalDTO>()
                .ReverseMap();
            CreateMap<TRHospitalLab, HospitalLabDTO>()
                .ReverseMap();
            CreateMap<TCUserLogin, UserLoginDTO>()
                .ReverseMap();
            CreateMap<TCUserLoginPermission, UserLoginPermissionDTO>()
                .ReverseMap();
        }
    }
}
