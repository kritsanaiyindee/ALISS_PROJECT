using ALISS.HISUpload.Batch.Models;
using ALISS.HISUpload.DTO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.HISUpload.Batch.AutoMapperConfiguration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<TRSTGHISFileUploadHeader, STGHISFileUploadHeaderDTO>();
            CreateMap<STGHISFileUploadHeaderDTO, TRSTGHISFileUploadHeader>();

            CreateMap<TRSTGHISFileUploadDetail, STGHISFileUploadDetailDTO>();
            CreateMap<STGHISFileUploadDetailDTO, TRSTGHISFileUploadDetail>();

            CreateMap<TRHISFileUploadSummary, HISFileUploadSummaryDTO>();
            CreateMap<HISFileUploadSummaryDTO, TRHISFileUploadSummary>();
        }
    }
}
