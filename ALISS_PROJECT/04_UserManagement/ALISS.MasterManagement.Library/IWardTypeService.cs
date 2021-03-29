using ALISS.MasterManagement.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.MasterManagement.Library
{
    public interface IWardTypeService
    {
        List<WardTypeDTO> GetList();
        List<WardTypeDTO> GetListWithParam(string param);
        List<WardTypeDTO> GetListWithModel(WardTypeDTO searchModel);
        List<WardTypeDTO> GetList_Active_WithModel(WardTypeDTO searchModel);
        WardTypeDTO GetData(string hos_code);
        WardTypeDTO SaveData(WardTypeDTO model);
        string CheckDuplicate(WardTypeDTO model);
    }
}
