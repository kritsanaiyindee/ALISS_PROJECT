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
    public class MasterTemplateService
    {
        private IConfiguration Configuration { get; }

        private ApiHelper _apiHelper;

        public MasterTemplateService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }

        public async Task<List<MasterTemplateDTO>> GetListAsync()
        {
            List<MasterTemplateDTO> objList = new List<MasterTemplateDTO>();

            objList = await _apiHelper.GetDataListAsync<MasterTemplateDTO>("mastertemplate_api/Get_List");

            return objList;
        }

        public async Task<List<MasterTemplateDTO>> GetListByParamAsync(MasterTemplateSearchDTO searchData)
        {
            List<MasterTemplateDTO> objList = new List<MasterTemplateDTO>();

            var searchJson = JsonSerializer.Serialize(searchData);

            objList = await _apiHelper.GetDataListByParamsAsync<MasterTemplateDTO>("mastertemplate_api/Get_List", searchJson);

            return objList;
        }

        public async Task<List<MasterTemplateDTO>> GetListByModelAsync(MasterTemplateSearchDTO searchData)
        {
            List<MasterTemplateDTO> objList = new List<MasterTemplateDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<MasterTemplateDTO, MasterTemplateSearchDTO>("mastertemplate_api/Get_ListByModel", searchData);

            return objList;
        }

        public async Task<MasterTemplateDTO> GetListByModelActiveAsync(MasterTemplateSearchDTO searchData)
        {
            MasterTemplateDTO objList = new MasterTemplateDTO();

            objList = await _apiHelper.GetDataByModelAsync<MasterTemplateDTO, MasterTemplateSearchDTO>("mastertemplate_api/Get_List_Active_ByModel", searchData);

            return objList;
        }

        public async Task<MasterTemplateDTO> GetDataAsync(string mst_code)
        {
            MasterTemplateDTO menu = new MasterTemplateDTO();

            menu = await _apiHelper.GetDataByIdAsync<MasterTemplateDTO>("mastertemplate_api/Get_Data", mst_code);

            return menu;
        }

        public async Task<MasterTemplateDTO> SaveDataAsync(MasterTemplateDTO model)
        {
            //MenuDTO menu = new MenuDTO()
            //{
            //    mnu_id = "2",
            //    mnu_name = "TEST_name",
            //    mnu_area = "TEST_area",
            //    mnu_controller = "TEST_controller"
            //};

            var menua = await _apiHelper.PostDataAsync<MasterTemplateDTO>("mastertemplate_api/Post_SaveData", model);

            return menua;
        }

        public async Task<MasterTemplateDTO> SaveCopyDataAsync(MasterTemplateDTO model)
        {
            //MenuDTO menu = new MenuDTO()
            //{
            //    mnu_id = "2",
            //    mnu_name = "TEST_name",
            //    mnu_area = "TEST_area",
            //    mnu_controller = "TEST_controller"
            //};

            var menua = await _apiHelper.PostDataAsync<MasterTemplateDTO>("mastertemplate_api/Post_SaveCopyData", model);

            return menua;
        }

        public async Task<List<LogProcessDTO>> GetHistoryAsync(string tran_id)
        {
            List<LogProcessDTO> objList = new List<LogProcessDTO>();

            LogProcessSearchDTO searchData = new LogProcessSearchDTO() { log_mnu_name = "MasterTemplate", log_tran_id = tran_id };

            objList = await _apiHelper.GetDataListByModelAsync<LogProcessDTO, LogProcessSearchDTO>("logprocess_api/Get_List", searchData);

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
