using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ALISS.HISUpload.DTO;
using ALISS.HISUpload.Library.Models;

namespace ALISS.HISUpload.Library.AutoMapperConfiguration
{
    public class MapperProfile : Profile    
    {
        public MapperProfile()
        {
            CreateMap<TRHISFileUpload, HISUploadDataDTO>()
                .ReverseMap();
            CreateMap<TRHISFileUploadSummary, HISFileUploadSummaryDTO>()
                .ReverseMap();
        }
    }
}
