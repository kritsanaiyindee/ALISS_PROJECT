using ALISS.ANTIBIOTREND.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.ANTIBIOTREND.Library
{
    public interface IAntibiotrendService
    {
        List<SP_AntimicrobialResistanceDTO> GetAMRWithModel(SP_AntimicrobialResistanceSearchDTO searchModel);
        List<NationHealthStrategyDTO> GetAMRNationStrategyWithModel(AMRStrategySearchDTO searchModel);
        List<AntibiotrendAMRStrategyDTO> GetAntibiotrendAMRStrategyWithModel(AMRStrategySearchDTO searchModel);
        List<SP_AntimicrobialResistanceDTO> GetAMRByOverallWithModel(SP_AntimicrobialResistanceSearchDTO searchModel);
        List<SP_AntimicrobialResistanceDTO> GetAMRBySpecimenWithModel(SP_AntimicrobialResistanceSearchDTO searchModel);
        List<SP_AntimicrobialResistanceDTO> GetAMRByWardWithModel(SP_AntimicrobialResistanceSearchDTO searchModel);
    }
}
