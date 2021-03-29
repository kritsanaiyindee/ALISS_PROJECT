using System;
using System.Collections.Generic;
using ALISS.Mapping.DTO;

namespace ALISS.Mapping.Library
{
    public interface IMappingDataService
    {

        #region Mapping
        List<MappingListsDTO> GetMappingList(string Param);
        MappingDataDTO GetMappingDataById(string mp_id);
        MappingDataDTO SaveMappingData(MappingDataDTO model);
        MappingDataDTO CopyMappingData(MappingDataDTO objParam);
        MappingDataDTO GetMappingDataWithModel(MappingDataDTO model);
        MappingDataDTO chkDuplicateMappingApproved(MappingDataDTO model);
        MappingDataDTO GetMappingDataActiveWithModel(MappingDataDTO model);
        #endregion

        #region WHONetMapping

        List<WHONetMappingListsDTO> GetWHONetMappingListWithModel(WHONetMappingSearch searchModel);
        WHONetMappingDataDTO GetWHONetMappingData(string wnm_id);
        WHONetMappingDataDTO SaveWHONetMappingData(WHONetMappingDataDTO model);
        WHONetMappingDataDTO GetWHONetMappingDataWithModel(WHONetMappingDataDTO model);
        #endregion


        #region SpecimenMapping

        List<SpecimenMappingListsDTO> GetSpecimenMappingListWithModel(SpecimenMappingSearch searchModel);
        SpecimenMappingDataDTO GetSpecimenMappingData(string spm_id);
        SpecimenMappingDataDTO GetSpecimenMappingDataWithModel(SpecimenMappingDataDTO model);
        SpecimenMappingDataDTO SaveSpecimenMappingData(SpecimenMappingDataDTO model);

        #endregion

        #region OrganismMapping
        List<OrganismMappingListsDTO> GetOrganismMappingListWithModel(OrganismMappingSearch searchModel);
        OrganismMappingDataDTO GetOrganismMappingData(string ogm_id);
        OrganismMappingDataDTO GetOrganismMappingDataWithModel(OrganismMappingDataDTO model);
        OrganismMappingDataDTO SaveOrganismMappingData(OrganismMappingDataDTO model);

        #endregion

        #region WardTypeMapping
        List<WardTypeMappingListsDTO> GetWardTypeMappingListWithModel(WardTypeMappingSearch searchModel);
        WardTypeMappingDataDTO GetWardTypeMappingData(string wdm_id);
        WardTypeMappingDataDTO GetWardTypeMappingDataWithModel(WardTypeMappingDataDTO model);
        WardTypeMappingDataDTO SaveWardTypeMappingData(WardTypeMappingDataDTO model);
        #endregion

    }
}
