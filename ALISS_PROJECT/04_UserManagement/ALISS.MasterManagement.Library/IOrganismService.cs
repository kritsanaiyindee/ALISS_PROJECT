using ALISS.MasterManagement.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.MasterManagement.Library
{
    public interface IOrganismService
    {
        List<OrganismDTO> GetList();
        List<OrganismDTO> GetListWithParam(string param);
        List<OrganismDTO> GetListWithModel(OrganismDTO searchModel);
        List<OrganismDTO> GetList_Active_WithModel(OrganismDTO searchModel);
        OrganismDTO GetData(string hos_code);
        OrganismDTO SaveData(OrganismDTO model);
        string CheckDuplicate(OrganismDTO model);
    }
}
