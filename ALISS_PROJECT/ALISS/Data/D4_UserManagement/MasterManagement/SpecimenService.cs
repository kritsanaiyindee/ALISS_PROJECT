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
    public class SpecimenService
    {
        private IConfiguration Configuration { get; }

        private ApiHelper _apiHelper;

        public SpecimenService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }

        public async Task<List<SpecimenDTO>> GetListAsync()
        {
            List<SpecimenDTO> objList = new List<SpecimenDTO>();

            objList = await _apiHelper.GetDataListAsync<SpecimenDTO>("specimen_api/Get_List");

            return objList;
        }

        public async Task<List<SpecimenDTO>> GetListByParamAsync(SpecimenDTO searchData)
        {
            List<SpecimenDTO> objList = new List<SpecimenDTO>();

            var searchJson = JsonSerializer.Serialize(searchData);

            objList = await _apiHelper.GetDataListByParamsAsync<SpecimenDTO>("specimen_api/Get_List", searchJson);

            return objList;
        }

        public async Task<List<SpecimenDTO>> GetListByModelAsync(SpecimenDTO searchData)
        {
            List<SpecimenDTO> objList = new List<SpecimenDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<SpecimenDTO, SpecimenDTO>("specimen_api/Get_ListByModel", searchData);

            return objList;
        }

        public async Task<List<SpecimenDTO>> GetListByModelActiveAsync(SpecimenDTO searchData)
        {
            List<SpecimenDTO> objList = new List<SpecimenDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<SpecimenDTO, SpecimenDTO>("specimen_api/Get_List_Active_ByModel", searchData);

            return objList;
        }

        public async Task<SpecimenDTO> GetDataAsync(string mst_code)
        {
            SpecimenDTO menu = new SpecimenDTO();

            menu = await _apiHelper.GetDataByIdAsync<SpecimenDTO>("specimen_api/Get_Data", mst_code);

            return menu;
        }

        public async Task<SpecimenDTO> SaveDataAsync(SpecimenDTO model)
        {
            //MenuDTO menu = new MenuDTO()
            //{
            //    mnu_id = "2",
            //    mnu_name = "TEST_name",
            //    mnu_area = "TEST_area",
            //    mnu_controller = "TEST_controller"
            //};

            var menua = await _apiHelper.PostDataAsync<SpecimenDTO>("specimen_api/Post_SaveData", model);

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
