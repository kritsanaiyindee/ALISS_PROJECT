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
    public class UserLoginService
    {
        private IConfiguration Configuration { get; }

        private ApiHelper _apiHelper;

        public UserLoginService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }

        public async Task<List<UserLoginDTO>> GetListAsync()
        {
            List<UserLoginDTO> objList = new List<UserLoginDTO>();

            objList = await _apiHelper.GetDataListAsync<UserLoginDTO>("userlogin_api/Get_List");

            return objList;
        }

        public async Task<List<UserLoginDTO>> GetListByParamAsync(UserLoginSearchDTO searchData)
        {
            List<UserLoginDTO> objList = new List<UserLoginDTO>();

            var searchJson = JsonSerializer.Serialize(searchData);

            objList = await _apiHelper.GetDataListByParamsAsync<UserLoginDTO>("userlogin_api/Get_List", searchJson);

            return objList;
        }

        public async Task<List<UserLoginPermissionDTO>> GetListByModelAsync(UserLoginSearchDTO searchData)
        {
            List<UserLoginPermissionDTO> objList = new List<UserLoginPermissionDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<UserLoginPermissionDTO, UserLoginSearchDTO>("userlogin_api/Get_ListByModel", searchData);

            return objList;
        }

        public async Task<List<UserLoginPermissionDTO>> GetPermissionListByModelAsync(UserLoginPermissionDTO searchData)
        {
            List<UserLoginPermissionDTO> objList = new List<UserLoginPermissionDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<UserLoginPermissionDTO, UserLoginPermissionDTO>("userlogin_api/Get_PermissionListByModel", searchData);

            return objList;
        }

        public async Task<UserLoginDTO> GetDataAsync(string hos_code)
        {
            UserLoginDTO menu = new UserLoginDTO();

            menu = await _apiHelper.GetDataByIdAsync<UserLoginDTO>("userlogin_api/Get_Data", hos_code);

            return menu;
        }

        public async Task<UserLoginDTO> SaveDataAsync(UserLoginDTO model)
        {
            //MenuDTO menu = new MenuDTO()
            //{
            //    mnu_id = "2",
            //    mnu_name = "TEST_name",
            //    mnu_area = "TEST_area",
            //    mnu_controller = "TEST_controller"
            //};

            var menua = await _apiHelper.PostDataAsync<UserLoginDTO>("userlogin_api/Post_SaveData", model);

            return menua;
        }

        public async Task<List<UserLoginPermissionDTO>> SavePermissionDataAsync(List<UserLoginPermissionDTO> model)
        {
            //MenuDTO menu = new MenuDTO()
            //{
            //    mnu_id = "2",
            //    mnu_name = "TEST_name",
            //    mnu_area = "TEST_area",
            //    mnu_controller = "TEST_controller"
            //};

            var menua = await _apiHelper.PostDataAsync<List<UserLoginPermissionDTO>>("userlogin_api/Post_SavePermissionData", model);

            return menua;
        }

        public async Task<UserLoginDTO> SavePasswordDataAsync(UserLoginDTO model)
        {
            //MenuDTO menu = new MenuDTO()
            //{
            //    mnu_id = "2",
            //    mnu_name = "TEST_name",
            //    mnu_area = "TEST_area",
            //    mnu_controller = "TEST_controller"
            //};

            var menua = await _apiHelper.PostDataAsync<UserLoginDTO>("userlogin_api/Post_SavePasswordData", model);

            return menua;
        }

        public async Task<List<LogProcessDTO>> GetHistoryAsync(string tran_id)
        {
            List<LogProcessDTO> objList = new List<LogProcessDTO>();

            LogProcessSearchDTO searchData = new LogProcessSearchDTO() { log_mnu_name = "UserLogin", log_tran_id = tran_id };

            objList = await _apiHelper.GetDataListByModelAsync<LogProcessDTO, LogProcessSearchDTO>("logprocess_api/GetAuth_List", searchData);

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
