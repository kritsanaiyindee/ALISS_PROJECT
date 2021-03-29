using ALISS.MasterManagement.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.MasterManagement.Library
{
    public interface IExpertRuleService
    {
        List<ExpertRuleDTO> GetList();
        List<ExpertRuleDTO> GetListWithParam(string param);
        List<ExpertRuleDTO> GetListWithModel(ExpertRuleDTO searchModel);
        List<ExpertRuleDTO> GetList_Active_WithModel(ExpertRuleDTO searchModel);
        ExpertRuleDTO GetData(string hos_code);
        ExpertRuleDTO SaveData(ExpertRuleDTO model);
        string CheckDuplicate(ExpertRuleDTO model);
    }
}
