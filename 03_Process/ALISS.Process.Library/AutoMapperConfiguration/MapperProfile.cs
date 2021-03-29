using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ALISS.Process.DTO;
using ALISS.Process.Library.Models;

namespace ALISS.Process.Library.AutoMapperConfiguration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<TRProcessRequest, ProcessRequestDTO>()
                .ReverseMap();
            CreateMap<TRProcessRequestDetail, ProcessRequestDetailDTO>()
                .ReverseMap();
            CreateMap<ProcessRequestDetailDTO, ProcessRequestCheckDetailDTO>()
                .ReverseMap();
        }
    }
}
