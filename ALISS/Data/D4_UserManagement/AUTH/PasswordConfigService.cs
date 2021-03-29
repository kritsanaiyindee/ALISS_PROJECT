using ALISS.AUTH.DTO;
using ALISS.Data.Client;
using ALISS.Master.DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALISS.Data.D4_UserManagement.AUTH
{
    public class PasswordConfigService
    {
        private IConfiguration Configuration { get; }

        private ApiHelper _apiHelper;

        public PasswordConfigService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }

        public async Task<PasswordConfigDTO> GetDataAsync()
        {
            PasswordConfigDTO menu = new PasswordConfigDTO();

            menu = await _apiHelper.GetDataByIdAsync<PasswordConfigDTO>("passwordconfig_api/Get_Data", "1");

            return menu;
        }

        public async Task<PasswordConfigDTO> SaveDataAsync(PasswordConfigDTO model)
        {
            //MenuDTO menu = new MenuDTO()
            //{
            //    mnu_id = "2",
            //    mnu_name = "TEST_name",
            //    mnu_area = "TEST_area",
            //    mnu_controller = "TEST_controller"
            //};

            var menua = await _apiHelper.PostDataAsync<PasswordConfigDTO>("passwordconfig_api/Post_SaveData", model);

            return menua;
        }

        public async Task<List<LogProcessDTO>> GetHistoryAsync()
        {
            List<LogProcessDTO> objList = new List<LogProcessDTO>();

            LogProcessSearchDTO searchData = new LogProcessSearchDTO() { log_mnu_name = "PasswordConfig" };

            objList = await _apiHelper.GetDataListByModelAsync<LogProcessDTO, LogProcessSearchDTO>("logprocess_api/GetAuth_List", searchData);

            return objList;
        }

        public Task<List<GridData>> GetHistoryAsync_tmp()
        {
            var rng = new Random();
            var objReturn = new List<GridData>(){
                new GridData { Column_01 = "(A0000)Super admin", Column_02 = "Active role", Column_03 = "Lorem", Column_04 = "00/00/00 00:00", Column_05 = "" },
                new GridData { Column_01 = "(A0000)Super admin", Column_02 = "Inactive role", Column_03 = "Lorem", Column_04 = "00/00/00 00:00", Column_05 = "" },
            };
            return Task.FromResult(objReturn);
        }
    }
}
