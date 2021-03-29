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
        List<ProcessRequestCheckDetailDTO> GetDetailListWithModel(ProcessRequestDTO searchModel);
        List<ProcessRequestCheckDetailDTO> GetCheckDetailListWithModel(ProcessRequestDTO searchModel);
        ProcessRequestDTO GetData(string pcr_code);
        ProcessRequestDTO GetDataWithModel(ProcessRequestDTO searchModel);
        ProcessRequestDTO SaveData(ProcessRequestDTO model);
        ProcessRequestDTO SaveDataToPublic(string pcr_code);
        List<ProcessRequestDetailDTO> SaveDetailData(List<ProcessRequestDetailDTO> modelList);
    }
}
