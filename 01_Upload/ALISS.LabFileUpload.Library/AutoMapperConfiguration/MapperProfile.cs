using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ALISS.LabFileUpload.DTO;
using ALISS.LabFileUpload.Library.Models;

namespace ALISS.LabFileUpload.Library.AutoMapperConfiguration
{
    public class MapperProfile : Profile    
    {
        public MapperProfile()
        {
            CreateMap<TRLabFileUpload, LabFileUploadDataDTO>();
            CreateMap<LabFileUploadDataDTO, TRLabFileUpload>();

        }
    }
}
