using ALISS.MasterManagement.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.MasterManagement.Library
{
    public interface IAntibioticService
    {
        List<AntibioticDTO> GetList();
        List<AntibioticDTO> GetListWithParam(string param);
        List<AntibioticDTO> GetListWithModel(AntibioticDTO searchModel);
        List<AntibioticDTO> GetList_Active_WithModel(AntibioticDTO searchModel);
        AntibioticDTO GetData(string hos_code);
        AntibioticDTO GetDataById(int ant_code);
        AntibioticDTO SaveData(AntibioticDTO model);
        string CheckDuplicate(AntibioticDTO model);
    }
}
