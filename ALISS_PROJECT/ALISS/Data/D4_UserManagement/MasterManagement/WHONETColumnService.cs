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
    public class WHONETColumnService
    {
        private IConfiguration Configuration { get; }

        private ApiHelper _apiHelper;

        public WHONETColumnService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }

        public async Task<List<WHONETColumnDTO>> GetListAsync()
        {
            List<WHONETColumnDTO> objList = new List<WHONETColumnDTO>();

            objList = await _apiHelper.GetDataListAsync<WHONETColumnDTO>("whonetcolumn_api/Get_List");

            return objList;
        }

        public async Task<List<WHONETColumnDTO>> GetListByParamAsync(WHONETColumnDTO searchData)
        {
            List<WHONETColumnDTO> objList = new List<WHONETColumnDTO>();

            var searchJson = JsonSerializer.Serialize(searchData);

            objList = await _apiHelper.GetDataListByParamsAsync<WHONETColumnDTO>("whonetcolumn_api/Get_List", searchJson);

            return objList;
        }

        public async Task<List<WHONETColumnDTO>> GetListByModelAsync(WHONETColumnDTO searchData)
        {
            List<WHONETColumnDTO> objList = new List<WHONETColumnDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<WHONETColumnDTO, WHONETColumnDTO>("whonetcolumn_api/Get_ListByModel", searchData);

            return objList;
        }

        public async Task<List<WHONETColumnDTO>> GetListByModelActiveAsync(WHONETColumnDTO searchData)
        {
            List<WHONETColumnDTO> objList = new List<WHONETColumnDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<WHONETColumnDTO, WHONETColumnDTO>("whonetcolumn_api/Get_List_Active_ByModel", searchData);

            return objList;
        }

        public async Task<WHONETColumnDTO> GetDataAsync(string mst_code)
        {
            WHONETColumnDTO menu = new WHONETColumnDTO();

            menu = await _apiHelper.GetDataByIdAsync<WHONETColumnDTO>("whonetcolumn_api/Get_Data", mst_code);

            return menu;
        }

        public async Task<WHONETColumnDTO> SaveDataAsync(WHONETColumnDTO model)
        {
            //MenuDTO menu = new MenuDTO()
            //{
            //    mnu_id = "2",
            //    mnu_name = "TEST_name",
            //    mnu_area = "TEST_area",
            //    mnu_controller = "TEST_controller"
            //};

            var menua = await _apiHelper.PostDataAsync<WHONETColumnDTO>("whonetcolumn_api/Post_SaveData", model);

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
