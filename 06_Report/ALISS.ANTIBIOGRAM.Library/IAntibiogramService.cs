using ALISS.ANTIBIOGRAM.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.ANTIBIOGRAM.Library
{
    public interface IAntibiogramService
    {
        List<AntibiogramHospitalTemplateDTO> GetAntibiogramHospitalTemplateWithModel(AntiHospitalSearchDTO searchModel);
        List<AntibiogramAreaHealthTemplateDTO> GetAntibiogramAreaHealthTemplateWithModel(AntiAreaHealthSearchDTO searchModel);
        List<AntibiogramNationTemplateDTO> GetAntibiogramNationTemplateWithModel(AntiNationSearchDTO searchModel);
        List<RPAntibiogramSurveilAntibioticDTO> GetAntibiogramSurveilAntibioticList_Active_WithModel(RPAntibiogramSurveilAntibioticDTO searchModel);
        List<RPAntibiogramSurveilOrganismDTO> GetAntibiogramSurveilOrganismList_Active_WithModel(RPAntibiogramSurveilOrganismDTO searchModel);
    }
}
