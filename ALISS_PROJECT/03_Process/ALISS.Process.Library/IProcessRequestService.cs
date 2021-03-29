using ALISS.Process.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.Process.Library
{
    public interface IProcessRequestService
    {
        List<ProcessRequestDTO> GetList();
        List<ProcessRequestDTO> GetListWithParam(string param);
        List<ProcessRequestDTO> GetListWithModel(ProcessRequestDTO searchModel);
        List<ProcessRequestDetailDTO> GetDetailListWithModel(ProcessRequestDTO searchModel);
        ProcessRequestDTO GetData(string pcr_code);
        ProcessRequestDTO SaveData(ProcessRequestDTO model);
        List<ProcessRequestDetailDTO> SaveDetailData(List<ProcessRequestDetailDTO> modelList);
    }
}
