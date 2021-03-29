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
    public class ExpertRuleService
    {
        private IConfiguration Configuration { get; }

        private ApiHelper _apiHelper;

        public ExpertRuleService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }

        public async Task<List<ExpertRuleDTO>> GetListAsync()
        {
            List<ExpertRuleDTO> objList = new List<ExpertRuleDTO>();

            objList = await _apiHelper.GetDataListAsync<ExpertRuleDTO>("expertrule_api/Get_List");

            return objList;
        }

        public async Task<List<ExpertRuleDTO>> GetListByParamAsync(ExpertRuleDTO searchData)
        {
            List<ExpertRuleDTO> objList = new List<ExpertRuleDTO>();

            var searchJson = JsonSerializer.Serialize(searchData);

            objList = await _apiHelper.GetDataListByParamsAsync<ExpertRuleDTO>("expertrule_api/Get_List", searchJson);

            return objList;
        }

        public async Task<List<ExpertRuleDTO>> GetListByModelAsync(ExpertRuleDTO searchData)
        {
            List<ExpertRuleDTO> objList = new List<ExpertRuleDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<ExpertRuleDTO, ExpertRuleDTO>("expertrule_api/Get_ListByModel", searchData);

            return objList;
        }

        public async Task<List<ExpertRuleDTO>> GetListByModelActiveAsync(ExpertRuleDTO searchData)
        {
            List<ExpertRuleDTO> objList = new List<ExpertRuleDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<ExpertRuleDTO, ExpertRuleDTO>("expertrule_api/Get_List_Active_ByModel", searchData);

            return objList;
        }

        public async Task<ExpertRuleDTO> GetDataAsync(string mst_code)
        {
            ExpertRuleDTO menu = new ExpertRuleDTO();

            menu = await _apiHelper.GetDataByIdAsync<ExpertRuleDTO>("expertrule_api/Get_Data", mst_code);

            return menu;
        }

        public async Task<ExpertRuleDTO> SaveDataAsync(ExpertRuleDTO model)
        {
            //MenuDTO menu = new MenuDTO()
            //{
            //    mnu_id = "2",
            //    mnu_name = "TEST_name",
            //    mnu_area = "TEST_area",
            //    mnu_controller = "TEST_controller"
            //};

            var menua = await _apiHelper.PostDataAsync<ExpertRuleDTO>("expertrule_api/Post_SaveData", model);

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
