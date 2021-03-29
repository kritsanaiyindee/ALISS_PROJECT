using ALISS.LabFileUpload.Batch.Models;
using ALISS.LabFileUpload.DTO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.LabFileUpload.Batch.AutoMapperConfiguration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<TRSTGLabFileDataHeader, STGLabFileDataHeaderDTO>();
            CreateMap<STGLabFileDataHeaderDTO, TRSTGLabFileDataHeader>();

            CreateMap<TRSTGLabFileDataDetail, STGLabFileDataDetailDTO>();
            CreateMap<STGLabFileDataDetailDTO, TRSTGLabFileDataDetail>();

        }
    }
}
