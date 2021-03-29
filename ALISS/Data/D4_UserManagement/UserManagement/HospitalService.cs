using ALISS.Data.Client;
using ALISS.Master.DTO;
using ALISS.UserManagement.DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ALISS.Data.D4_UserManagement.UserManagement
{
    public class HospitalService
    {
        private IConfiguration Configuration { get; }

        private ApiHelper _apiHelper;

        public HospitalService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }

        public async Task<List<HospitalDTO>> GetListAsync()
        {
            List<HospitalDTO> objList = new List<HospitalDTO>();

            objList = await _apiHelper.GetDataListAsync<HospitalDTO>("hospital_api/Get_List");

            return objList;
        }

        public async Task<List<HospitalDTO>> GetListByParamAsync(HospitalSearchDTO searchData)
        {
            List<HospitalDTO> objList = new List<HospitalDTO>();

            var searchJson = JsonSerializer.Serialize(searchData);

            objList = await _apiHelper.GetDataListByParamsAsync<HospitalDTO>("hospital_api/Get_List", searchJson);

            return objList;
        }

        public async Task<List<HospitalDTO>> GetListByModelAsync(HospitalSearchDTO searchData)
        {
            List<HospitalDTO> objList = new List<HospitalDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<HospitalDTO, HospitalSearchDTO>("hospital_api/Get_ListByModel", searchData);

            return objList;
        }

        public async Task<HospitalDTO> GetDataAsync(string hos_code)
        {
            HospitalDTO menu = new HospitalDTO();

            menu = await _apiHelper.GetDataByIdAsync<HospitalDTO>("hospital_api/Get_Data", hos_code);

            return menu;
        }

        public async Task<List<HospitalLabDTO>> GetLabListByModelAsync(HospitalSearchDTO searchData)
        {
            List<HospitalLabDTO> objList = new List<HospitalLabDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<HospitalLabDTO, HospitalSearchDTO>("hospital_api/Get_LabListByModel", searchData);

            return objList;
        }

        public async Task<HospitalLabDTO> GetLabDataAsync(string hol_code)
        {
            HospitalLabDTO menu = new HospitalLabDTO();

            menu = await _apiHelper.GetDataByIdAsync<HospitalLabDTO>("hospital_api/Get_LabData", hol_code);

            return menu;
        }

        public async Task<HospitalDTO> SaveDataAsync(HospitalDTO model)
        {
            //MenuDTO menu = new MenuDTO()
            //{
            //    mnu_id = "2",
            //    mnu_name = "TEST_name",
            //    mnu_area = "TEST_area",
            //    mnu_controller = "TEST_controller"
            //};

            var menua = await _apiHelper.PostDataAsync<HospitalDTO>("hospital_api/Post_SaveData", model);

            return menua;
        }

        public async Task<List<HospitalLabDTO>> SaveLabDataAsync(List<HospitalLabDTO> modelList)
        {
            //MenuDTO menu = new MenuDTO()
            //{
            //    mnu_id = "2",
            //    mnu_name = "TEST_name",
            //    mnu_area = "TEST_area",
            //    mnu_controller = "TEST_controller"
            //};

            var returnModel = await _apiHelper.PostDataAsync<List<HospitalLabDTO>>("hospital_api/Post_SaveLabData", modelList);

            return returnModel;
        }

        public async Task<List<LogProcessDTO>> GetHistoryAsync(string tran_id)
        {
            List<LogProcessDTO> objList = new List<LogProcessDTO>();

            LogProcessSearchDTO searchData = new LogProcessSearchDTO() { log_mnu_name = "Hospital", log_tran_id = tran_id };

            objList = await _apiHelper.GetDataListByModelAsync<LogProcessDTO, LogProcessSearchDTO>("logprocess_api/Get_List", searchData);

            return objList;
        }

        public Task<GridData[]> GetHistoryAsync_Tmp()
        {
            var rng = new Random();
            var objReturn = new List<GridData>(){
                new GridData { Column_01 = "(A0000)Super admin", Column_02 = "Active role", Column_03 = "Lorem", Column_04 = "00/00/00 00:00", Column_05 = "" },
                new GridData { Column_01 = "(A0000)Super admin", Column_02 = "Inactive role", Column_03 = "Lorem", Column_04 = "00/00/00 00:00", Column_05 = "" },
            };
            return Task.FromResult(objReturn.ToArray());
        }
    }
}
