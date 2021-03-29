using ALISS.Data.Client;
using ALISS.DropDownList.DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALISS.Data.D0_Master
{
    public class DropDownListDataService
    {
        private IConfiguration Configuration { get; }

        private ApiHelper _apiHelper;

        public DropDownListDataService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }

        public async Task<List<HospitalDataDTO>> GetAreaHealthListByModelAsync(HospitalDataDTO searchModel)
        {
            List<HospitalDataDTO> objList = new List<HospitalDataDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<HospitalDataDTO, HospitalDataDTO>("dropdownlist_api/GetAreaHealthList", searchModel);

            return objList;
        }

        public async Task<List<HospitalDataDTO>> GetProvinceListByModelAsync(HospitalDataDTO searchModel)
        {
            List<HospitalDataDTO> objList = new List<HospitalDataDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<HospitalDataDTO, HospitalDataDTO>("dropdownlist_api/GetProvinceList", searchModel);

            return objList;
        }

        public async Task<List<HospitalDataDTO>> GetHospitalListByModelAsync(HospitalDataDTO searchModel)
        {
            List<HospitalDataDTO> objList = new List<HospitalDataDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<HospitalDataDTO, HospitalDataDTO>("dropdownlist_api/GetHospitalList", searchModel);

            return objList;
        }

        public async Task<List<HospitalDataDTO>> GetTCHospitalListByModelAsync(HospitalDataDTO searchModel)
        {
            List<HospitalDataDTO> objList = new List<HospitalDataDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<HospitalDataDTO, HospitalDataDTO>("dropdownlist_api/GetTCHospitalList", searchModel);

            return objList;
        }

        public async Task<List<DropDownListDTO>> GetRoleListByModelAsync()
        {
            List<DropDownListDTO> objList = new List<DropDownListDTO>();
            var searchModel = new DropDownListDTO();

            objList = await _apiHelper.GetDataListByModelAsync<DropDownListDTO, DropDownListDTO>("dropdownlist_api/GetRoleList", searchModel);

            return objList;
        }

        public async Task<List<ParameterDTO>> GetParameterListByModelAsync(string prm_code_major)
        {
            List<ParameterDTO> objList = new List<ParameterDTO>();
            var searchModel = new ParameterDTO() { prm_code_major = prm_code_major };

            objList = await _apiHelper.GetDataListByModelAsync<ParameterDTO, ParameterDTO>("dropdownlist_api/GetParameterList", searchModel);

            return objList;
        }

        public async Task<List<DropDownListDTO>> GetLabListByModelAsync(DropDownListDTO searchModel)
        {
            List<DropDownListDTO> objList = new List<DropDownListDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<DropDownListDTO, DropDownListDTO>("dropdownlist_api/GetLabList", searchModel);

            return objList;
        }

        public async Task<List<HospitalLabDataDTO>> GetAllLabListByModelAsync(HospitalLabDataDTO searchModel)
        {
            List<HospitalLabDataDTO> objList = new List<HospitalLabDataDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<HospitalLabDataDTO, HospitalLabDataDTO>("dropdownlist_api/GetAllLabList", searchModel);

            return objList;
        }
    }
}
