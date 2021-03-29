using ALISS.AUTH.DTO;
using ALISS.Data.Client;
using ALISS.Master.DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ALISS.Data.D4_UserManagement.AUTH
{
    public class MenuService
    {
        private IConfiguration Configuration { get; }

        private ApiHelper _apiHelper;

        public MenuService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }

        public async Task<List<MenuDTO>> GetListAsync()
        {
            List<MenuDTO> objList = new List<MenuDTO>();

            objList = await _apiHelper.GetDataListAsync<MenuDTO>("menu_api/Get_List");

            return objList;
        }

        public async Task<List<MenuDTO>> GetListByParamAsync(MenuSearchDTO searchData)
        {
            List<MenuDTO> objList = new List<MenuDTO>();

            var searchJson = JsonSerializer.Serialize(searchData);

            objList = await _apiHelper.GetDataListByParamsAsync<MenuDTO>("menu_api/Get_List", searchJson);

            return objList;
        }

        public async Task<List<MenuDTO>> GetListByModelAsync(MenuSearchDTO searchData)
        {
            List<MenuDTO> objList = new List<MenuDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<MenuDTO, MenuSearchDTO>("menu_api/Get_ListByModel", searchData);

            return objList;
        }

        public async Task<MenuDTO> GetDataAsync(string mnu_Id)
        {
            MenuDTO menu = new MenuDTO();

            menu = await _apiHelper.GetDataByIdAsync<MenuDTO>("menu_api/Get_Data", mnu_Id);

            return menu;
        }

        public async Task<MenuDTO> SaveDataAsync(MenuDTO model)
        {
            //MenuDTO menu = new MenuDTO()
            //{
            //    mnu_id = "2",
            //    mnu_name = "TEST_name",
            //    mnu_area = "TEST_area",
            //    mnu_controller = "TEST_controller"
            //};

            var menua = await _apiHelper.PostDataAsync<MenuDTO>("menu_api/Post_SaveData", model);

            return menua;
        }

        public async Task<List<LogProcessDTO>> GetHistoryAsync()
        {
            List<LogProcessDTO> objList = new List<LogProcessDTO>();

            LogProcessSearchDTO searchData = new LogProcessSearchDTO() { log_mnu_name = "Menu" };

            objList = await _apiHelper.GetDataListByModelAsync<LogProcessDTO, LogProcessSearchDTO>("logprocess_api/GetAuth_List", searchData);

            return objList;
        }

        public Task<GridData[]> GetHistory1Async()
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
