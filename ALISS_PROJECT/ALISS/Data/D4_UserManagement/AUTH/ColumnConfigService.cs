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
    public class ColumnConfigService
    {
        private IConfiguration Configuration { get; }

        private ApiHelper _apiHelper;

        public ColumnConfigService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }

        public async Task<List<ColumnConfigDTO>> GetListAsync()
        {
            List<ColumnConfigDTO> objList = new List<ColumnConfigDTO>();

            objList = await _apiHelper.GetDataListAsync<ColumnConfigDTO>("columnconfig_api/Get_List");

            return objList;
        }

        public async Task<List<ColumnConfigDTO>> GetListByParamAsync(ColumnConfigSearchDTO searchData)
        {
            List<ColumnConfigDTO> objList = new List<ColumnConfigDTO>();

            var searchJson = JsonSerializer.Serialize(searchData);

            objList = await _apiHelper.GetDataListByParamsAsync<ColumnConfigDTO>("columnconfig_api/Get_List", searchJson);

            return objList;
        }

        public async Task<List<ColumnConfigDTO>> GetListByModelAsync(ColumnConfigSearchDTO searchData)
        {
            List<ColumnConfigDTO> objList = new List<ColumnConfigDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<ColumnConfigDTO, ColumnConfigSearchDTO>("columnconfig_api/Get_ListByModel", searchData);

            return objList;
        }

        public async Task<ColumnConfigDTO> GetDataAsync(string mnu_Id)
        {
            ColumnConfigDTO objModel = new ColumnConfigDTO();

            objModel = await _apiHelper.GetDataByIdAsync<ColumnConfigDTO>("columnconfig_api/Get_Data", mnu_Id);

            return objModel;
        }

        public async Task<ColumnConfigDTO> SaveDataAsync(ColumnConfigDTO model)
        {
            ColumnConfigDTO objModel = new ColumnConfigDTO();

            objModel = await _apiHelper.PostDataAsync<ColumnConfigDTO>("columnconfig_api/Post_SaveData", model);

            return objModel;
        }

        public async Task<List<LogProcessDTO>> GetHistoryAsync()
        {
            List<LogProcessDTO> objList = new List<LogProcessDTO>();

            LogProcessSearchDTO searchData = new LogProcessSearchDTO() { log_mnu_name = "ColumnConfig" };

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
