using ALISS.MasterManagement.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.MasterManagement.Library
{
    public interface ISpecimenService
    {
        List<SpecimenDTO> GetList();
        List<SpecimenDTO> GetListWithParam(string param);
        List<SpecimenDTO> GetListWithModel(SpecimenDTO searchModel);
        List<SpecimenDTO> GetList_Active_WithModel(SpecimenDTO searchModel);
        SpecimenDTO GetData(string hos_code);
        SpecimenDTO SaveData(SpecimenDTO model);
        string CheckDuplicate(SpecimenDTO model);
    }
}
