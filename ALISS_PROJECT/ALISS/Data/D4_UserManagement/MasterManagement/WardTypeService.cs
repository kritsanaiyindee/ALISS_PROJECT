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
    public class WardTypeService
    {
        private IConfiguration Configuration { get; }

        private ApiHelper _apiHelper;

        public WardTypeService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }

        public async Task<List<WardTypeDTO>> GetListAsync()
        {
            List<WardTypeDTO> objList = new List<WardTypeDTO>();

            objList = await _apiHelper.GetDataListAsync<WardTypeDTO>("wardtype_api/Get_List");

            return objList;
        }

        public async Task<List<WardTypeDTO>> GetListByParamAsync(WardTypeDTO searchData)
        {
            List<WardTypeDTO> objList = new List<WardTypeDTO>();

            var searchJson = JsonSerializer.Serialize(searchData);

            objList = await _apiHelper.GetDataListByParamsAsync<WardTypeDTO>("wardtype_api/Get_List", searchJson);

            return objList;
        }

        public async Task<List<WardTypeDTO>> GetListByModelAsync(WardTypeDTO searchData)
        {
            List<WardTypeDTO> objList = new List<WardTypeDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<WardTypeDTO, WardTypeDTO>("wardtype_api/Get_ListByModel", searchData);

            return objList;
        }

        public async Task<List<WardTypeDTO>> GetListByModelActiveAsync(WardTypeDTO searchData)
        {
            List<WardTypeDTO> objList = new List<WardTypeDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<WardTypeDTO, WardTypeDTO>("wardtype_api/Get_List_Active_ByModel", searchData);

            return objList;
        }

        public async Task<WardTypeDTO> GetDataAsync(string mst_code)
        {
            WardTypeDTO menu = new WardTypeDTO();

            menu = await _apiHelper.GetDataByIdAsync<WardTypeDTO>("wardtype_api/Get_Data", mst_code);

            return menu;
        }

        public async Task<WardTypeDTO> SaveDataAsync(WardTypeDTO model)
        {
            //MenuDTO menu = new MenuDTO()
            //{
            //    mnu_id = "2",
            //    mnu_name = "TEST_name",
            //    mnu_area = "TEST_area",
            //    mnu_controller = "TEST_controller"
            //};

            var menua = await _apiHelper.PostDataAsync<WardTypeDTO>("wardtype_api/Post_SaveData", model);

            return menua;
        }

        public async Task<List<LogProcessDTO>> GetHistoryAsync(LogProcessSearchDTO searchData)
        {
            List<LogProcessDTO> objList = new List<LogProcessDTO>();

            var searchJson = JsonSerializer.Serialize(searchData);

            objList = await _apiHelper.GetDataListByParamsAsync<LogProcessDTO>("logprocess_api/Get_List", searchJson);

            return objList;
        }

        public Task<GridData[]> GetHistoryAsync()
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
