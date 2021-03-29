using ALISS.Mapping.DTO;
using ALISS.Data.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace ALISS.Data.D2_Mapping
{
    public class MappingService
    {
        private IConfiguration Configuration { get; }

        private ApiHelper _apiHelper;

        public MappingService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }

        #region Mapping

        public async Task<List<MappingListsDTO>> GetMappingListByParamAsync(MappingSearchDTO mappingSearch)
        {
            List<MappingListsDTO> objList = new List<MappingListsDTO>();

            var searchJson = JsonSerializer.Serialize(mappingSearch);

            objList = await _apiHelper.GetDataListByParamsAsync<MappingListsDTO>("mapping_api/GetMappingList", searchJson);

            return objList;
        }

        public async Task<MappingDataDTO> GetMappingDataAsync(string mp_Id)
        {
            MappingDataDTO mapping = new MappingDataDTO();

            mapping = await _apiHelper.GetDataByIdAsync<MappingDataDTO>("mapping_api/GetMappingDataById", mp_Id);

            return mapping;
        }

        public async Task<MappingDataDTO> SaveMappingDataAsync(MappingDataDTO model)
        {
            if (model.mp_id.Equals(Guid.Empty))
            {
                model.mp_id = Guid.NewGuid();
                model.mp_status = 'N';
                if(model.mp_version == 0)
                {
                    model.mp_version = 1;
                }                                
                model.mp_flagdelete = false;
            }

            if (model.mp_status.Equals('A'))
            {
                model.mp_status = 'A';
            }


            model.mp_updatedate = DateTime.Now;

            var mapping = await _apiHelper.PostDataAsync<MappingDataDTO>("mapping_api/Post_SaveMappingData", model);

            return mapping;
        }

        
        public async Task<MappingDataDTO> CopyMappingDataAsync(MappingDataDTO model)
        {

            var mapping = await _apiHelper.PostDataAsync<MappingDataDTO>("mapping_api/Post_CopyMappingData", model);

            return mapping;
        }

        public async Task<MappingDataDTO> GetMappingDataByModelAsync(MappingDataDTO model)
        {
            MappingDataDTO objList = new MappingDataDTO();
            objList = await _apiHelper.GetDataByModelAsync<MappingDataDTO, MappingDataDTO>("mapping_api/Get_MappingDataByModel", model);
            return objList;
        }

        public async Task<MappingDataDTO> GetchkDuplicateMappingApprovedAsync(MappingDataDTO model)
        {
            MappingDataDTO objList = new MappingDataDTO();
            objList = await _apiHelper.GetDataByModelAsync<MappingDataDTO, MappingDataDTO>("mapping_api/Get_chkDuplicateMappingApproved", model);
            return objList;
        }

        public async Task<MappingDataDTO> GetMappingDataActiveWithModelAsync(MappingDataDTO model)
        {
            MappingDataDTO objList = new MappingDataDTO();
            objList = await _apiHelper.GetDataByModelAsync<MappingDataDTO, MappingDataDTO>("mapping_api/Get_MappingDataActiveWithModel", model);
            return objList;
        }

        #endregion

        #region WHONetMapping
        public async Task<List<WHONetMappingListsDTO>> GetWHONetMappingListByModelAsync(WHONetMappingSearch searchData)
        {
            List<WHONetMappingListsDTO> objList = new List<WHONetMappingListsDTO>();
            objList = await _apiHelper.GetDataListByModelAsync<WHONetMappingListsDTO, WHONetMappingSearch>("mapping_api/Get_WHONetMappingListByModel", searchData);
            return objList;
        }

        public async Task<WHONetMappingDataDTO> GetWHONetMappingDataAsync(string wnm_Id)
        {
            WHONetMappingDataDTO menu = new WHONetMappingDataDTO();
            menu = await _apiHelper.GetDataByIdAsync<WHONetMappingDataDTO>("mapping_api/Get_WHONetMappingData", wnm_Id);
            return menu;
        }

        public async Task<WHONetMappingDataDTO> SaveWHONetMappingDataAsync(WHONetMappingDataDTO model)
        {
            if (model.wnm_id.Equals(Guid.Empty))
            {
                model.wnm_id = Guid.NewGuid();
                model.wnm_status = 'N';
                model.wnm_flagdelete = false;
            }
            else
            {
                model.wnm_status = 'E';
            }

            model.wnm_updatedate = DateTime.Now;
            var whonetmapping = await _apiHelper.PostDataAsync<WHONetMappingDataDTO>("mapping_api/Post_SaveWHONetMappingData", model);
                    

            return whonetmapping;
        }

        public async Task<WHONetMappingDataDTO> GetWHONetMappingDataByModelAsync(WHONetMappingDataDTO model)
        {
            WHONetMappingDataDTO objList = new WHONetMappingDataDTO();
            objList = await _apiHelper.GetDataByModelAsync<WHONetMappingDataDTO, WHONetMappingDataDTO>("mapping_api/Get_WHONetMappingDataByModel", model);
            return objList;
        }
        #endregion

        #region SpecimenMapping
        public async Task<List<SpecimenMappingListsDTO>> GetSpecimenMappingListByModelAsync(SpecimenMappingSearch searchData)
        {
            List<SpecimenMappingListsDTO> objList = new List<SpecimenMappingListsDTO>();
            objList = await _apiHelper.GetDataListByModelAsync<SpecimenMappingListsDTO, SpecimenMappingSearch>("mapping_api/Get_SpecimenMappingListByModel", searchData);
            return objList;
        }

        public async Task<SpecimenMappingDataDTO> GetSpecimenMappingDataByModelAsync(SpecimenMappingDataDTO model)
        {
            SpecimenMappingDataDTO objList = new SpecimenMappingDataDTO();
            objList = await _apiHelper.GetDataByModelAsync<SpecimenMappingDataDTO, SpecimenMappingDataDTO>("mapping_api/Get_SpecimenMappingDataByModel", model);
            return objList;
        }

        public async Task<SpecimenMappingDataDTO> GetSpecimenMappingDataAsync(string spm_Id)
        {
            SpecimenMappingDataDTO SpecimenMapping = new SpecimenMappingDataDTO();
            SpecimenMapping = await _apiHelper.GetDataByIdAsync<SpecimenMappingDataDTO>("mapping_api/Get_SpecimenMappingData", spm_Id);
            return SpecimenMapping;
        }

        public async Task<SpecimenMappingDataDTO> SaveSpecimenMappingDataAsync(SpecimenMappingDataDTO model)
        {
            if (model.spm_id.Equals(Guid.Empty))
            {
                model.spm_id = Guid.NewGuid();
                model.spm_status = 'N';
                model.spm_flagdelete = false;
            }
            else
            {
                model.spm_status = 'E';
            }

            model.spm_updatedate = DateTime.Now;
            var specimenmapping = await _apiHelper.PostDataAsync<SpecimenMappingDataDTO>("mapping_api/Post_SaveSpecimenMappingData", model);
            return specimenmapping;
        }
        #endregion

        #region OrganismMapping
        public async Task<List<OrganismMappingListsDTO>> GetOrganismMappingListByModelAsync(OrganismMappingSearch searchData)
        {
            List<OrganismMappingListsDTO> objList = new List<OrganismMappingListsDTO>();
            objList = await _apiHelper.GetDataListByModelAsync<OrganismMappingListsDTO, OrganismMappingSearch>("mapping_api/Get_OrganismMappingListByModel", searchData);
            return objList;
        }

        public async Task<OrganismMappingDataDTO> GetOrganismMappingDataByModelAsync(OrganismMappingDataDTO model)
        {
            OrganismMappingDataDTO objList = new OrganismMappingDataDTO();
            objList = await _apiHelper.GetDataByModelAsync<OrganismMappingDataDTO, OrganismMappingDataDTO>("mapping_api/Get_OrganismMappingDataByModel", model);
            return objList;
        }

        public async Task<OrganismMappingDataDTO> GetOrganismMappingDataAsync(string ogm_Id)
        {
            OrganismMappingDataDTO OrganismMapping = new OrganismMappingDataDTO();
            OrganismMapping = await _apiHelper.GetDataByIdAsync<OrganismMappingDataDTO>("mapping_api/Get_OrganismMappingData", ogm_Id);
            return OrganismMapping;
        }

        public async Task<OrganismMappingDataDTO> SaveOrganismMappingDataAsync(OrganismMappingDataDTO model)
        {
            if (model.ogm_id.Equals(Guid.Empty))
            {
                model.ogm_id = Guid.NewGuid();
                model.ogm_status = 'N';
                model.ogm_flagdelete = false;
            }
            else
            {
                model.ogm_status = 'E';
            }



            model.ogm_updatedate = DateTime.Now;
            var organismmapping = await _apiHelper.PostDataAsync<OrganismMappingDataDTO>("mapping_api/Post_SaveOrganismMappingData", model);
            return organismmapping;
        }
        #endregion

        #region WardTypeMapping
        public async Task<List<WardTypeMappingListsDTO>> GetWardTypeMappingListByModelAsync(WardTypeMappingSearch searchData)
        {
            List<WardTypeMappingListsDTO> objList = new List<WardTypeMappingListsDTO>();
            objList = await _apiHelper.GetDataListByModelAsync<WardTypeMappingListsDTO, WardTypeMappingSearch>("mapping_api/Get_WardTypeMappingListByModel", searchData);
            return objList;
        }

        public async Task<WardTypeMappingDataDTO> GetWardTypeMappingDataByModelAsync(WardTypeMappingDataDTO model)
        {
            WardTypeMappingDataDTO objList = new WardTypeMappingDataDTO();
            objList = await _apiHelper.GetDataByModelAsync<WardTypeMappingDataDTO, WardTypeMappingDataDTO>("mapping_api/Get_WardTypeMappingDataByModel", model);
            return objList;
        }

        public async Task<WardTypeMappingDataDTO> GetWardTypeMappingDataAsync(string wdm_Id)
        {
            WardTypeMappingDataDTO WardTypeMapping = new WardTypeMappingDataDTO();
            WardTypeMapping = await _apiHelper.GetDataByIdAsync<WardTypeMappingDataDTO>("mapping_api/Get_WardTypeMappingData", wdm_Id);
            return WardTypeMapping;
        }

        public async Task<WardTypeMappingDataDTO> SaveWardTypeMappingDataAsync(WardTypeMappingDataDTO model)
        {
            if (model.wdm_id.Equals(Guid.Empty))
            {
                model.wdm_id = Guid.NewGuid();
                model.wdm_status = 'N';
                model.wdm_flagdelete = false;
            }
            else
            {
                model.wdm_status = 'E';
            }

            model.wdm_updatedate = DateTime.Now;
            var wardtypemapping = await _apiHelper.PostDataAsync<WardTypeMappingDataDTO>("mapping_api/Post_SaveWardTypeMappingData", model);
            return wardtypemapping;
        }
        #endregion
    }
}
