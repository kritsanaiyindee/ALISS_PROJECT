using ALISS.MasterManagement.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.MasterManagement.Library
{
    public interface IMasterTemplateService
    {
        List<MasterTemplateDTO> GetList();
        List<MasterTemplateDTO> GetListWithParam(string param);
        List<MasterTemplateDTO> GetListWithModel(MasterTemplateSearchDTO searchModel);
        MasterTemplateDTO GetList_Active_WithModel(MasterTemplateSearchDTO searchModel);
        MasterTemplateDTO GetData(string hos_code);
        MasterTemplateDTO SaveData(MasterTemplateDTO model);
        MasterTemplateDTO SaveCopyData(MasterTemplateDTO model);
        string CheckDuplicate(MasterTemplateDTO model);
    }
}
