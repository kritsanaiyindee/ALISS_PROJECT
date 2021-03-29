using ALISS.Data.Client;
using ALISS.Master.DTO;
using ALISS.MasterManagement.DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ALISS.Data.D4_UserManagement.MasterManagement
{
    public class MasterHospitalService
    {
        private IConfiguration Configuration { get; }

        private ApiHelper _apiHelper;

        public MasterHospitalService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }

        public async Task<List<MasterHospitalDTO>> GetListAsync()
        {
            List<MasterHospitalDTO> objList = new List<MasterHospitalDTO>();

            objList = await _apiHelper.GetDataListAsync<MasterHospitalDTO>("masterhospital_api/Get_List");

            return objList;
        }

        public async Task<List<MasterHospitalDTO>> GetListByParamAsync(MasterHospitalSearchDTO searchData)
        {
            List<MasterHospitalDTO> objList = new List<MasterHospitalDTO>();

            var searchJson = JsonSerializer.Serialize(searchData);

            objList = await _apiHelper.GetDataListByParamsAsync<MasterHospitalDTO>("masterhospital_api/Get_List", searchJson);

            return objList;
        }

        public async Task<List<MasterHospitalDTO>> GetListByModelAsync(MasterHospitalSearchDTO searchData)
        {
            List<MasterHospitalDTO> objList = new List<MasterHospitalDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<MasterHospitalDTO, MasterHospitalSearchDTO>("masterhospital_api/Get_ListByModel", searchData);

            return objList;
        }

        public async Task<MasterHospitalDTO> GetDataAsync(string hos_code)
        {
            MasterHospitalDTO menu = new MasterHospitalDTO();

            menu = await _apiHelper.GetDataByIdAsync<MasterHospitalDTO>("masterhospital_api/Get_Data", hos_code);

            return menu;
        }

        public async Task<MasterHospitalDTO> SaveDataAsync(MasterHospitalDTO model)
        {
            //MenuDTO menu = new MenuDTO()
            //{
            //    mnu_id = "2",
            //    mnu_name = "TEST_name",
            //    mnu_area = "TEST_area",
            //    mnu_controller = "TEST_controller"
            //};

            var menua = await _apiHelper.PostDataAsync<MasterHospitalDTO>("masterhospital_api/Post_SaveData", model);

            return menua;
        }

        public async Task<List<LogProcessDTO>> GetHistoryAsync()
        {
            List<LogProcessDTO> objList = new List<LogProcessDTO>();

            //LogProcessSearchDTO searchData = new LogProcessSearchDTO() { log_mnu_name = "MasterHospital", log_tran_id = tran_id };

            //objList = await _apiHelper.GetDataListByModelAsync<LogProcessDTO, LogProcessSearchDTO>("logprocess_api/Get_List", searchData);

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
