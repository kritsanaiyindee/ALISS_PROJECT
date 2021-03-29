using ALISS.Data.Client;
using ALISS.Master.DTO;
using ALISS.Process.DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ALISS.Data.D3_Process
{
    public class ProcessRequestService
    {
        private IConfiguration Configuration { get; }

        private ApiHelper _apiHelper;

        public ProcessRequestService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }

        public async Task<List<ProcessRequestDTO>> GetListAsync()
        {
            List<ProcessRequestDTO> objList = new List<ProcessRequestDTO>();

            objList = await _apiHelper.GetDataListAsync<ProcessRequestDTO>("processrequest_api/Get_List");

            return objList;
        }

        public async Task<List<ProcessRequestDTO>> GetListByParamAsync(ProcessRequestDTO searchData)
        {
            List<ProcessRequestDTO> objList = new List<ProcessRequestDTO>();

            var searchJson = JsonSerializer.Serialize(searchData);

            objList = await _apiHelper.GetDataListByParamsAsync<ProcessRequestDTO>("processrequest_api/Get_List", searchJson);

            return objList;
        }

        public async Task<List<ProcessRequestDTO>> GetListByModelAsync(ProcessRequestDTO searchData)
        {
            List<ProcessRequestDTO> objList = new List<ProcessRequestDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<ProcessRequestDTO, ProcessRequestDTO>("processrequest_api/Get_ListByModel", searchData);

            return objList;
        }

        public async Task<List<ProcessRequestCheckDetailDTO>> GetDetailListByModelAsync(ProcessRequestDTO searchData)
        {
            List<ProcessRequestCheckDetailDTO> objList = new List<ProcessRequestCheckDetailDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<ProcessRequestCheckDetailDTO, ProcessRequestDTO>("processrequest_api/Get_DetailListByModel", searchData);

            return objList;
        }

        public async Task<List<ProcessRequestCheckDetailDTO>> GetCheckDetailListByModelAsync(ProcessRequestDTO searchData)
        {
            List<ProcessRequestCheckDetailDTO> objList = new List<ProcessRequestCheckDetailDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<ProcessRequestCheckDetailDTO, ProcessRequestDTO>("processrequest_api/Get_CheckDetailListByModel", searchData);

            return objList;
        }

        public async Task<ProcessRequestDTO> GetDataAsync(string pcr_code)
        {
            ProcessRequestDTO menu = new ProcessRequestDTO();

            menu = await _apiHelper.GetDataByIdAsync<ProcessRequestDTO>("processrequest_api/Get_Data", pcr_code);

            return menu;
        }

        public async Task<ProcessRequestDTO> GetDataWithModelAsync(ProcessRequestDTO searchData)
        {
            ProcessRequestDTO data = new ProcessRequestDTO();

            data = await _apiHelper.PostDataAsync<ProcessRequestDTO>("processrequest_api/Get_DataWithModel", searchData);

            return data;
        }

        public async Task<ProcessRequestDTO> SaveDataAsync(ProcessRequestDTO model)
        {
            //MenuDTO menu = new MenuDTO()
            //{
            //    mnu_id = "2",
            //    mnu_name = "TEST_name",
            //    mnu_area = "TEST_area",
            //    mnu_controller = "TEST_controller"
            //};

            var menua = await _apiHelper.PostDataAsync<ProcessRequestDTO>("processrequest_api/Post_SaveData", model);

            return menua;
        }

        public async Task<List<ProcessRequestDetailDTO>> SaveDetailDataAsync(List<ProcessRequestDetailDTO> model)
        {
            //MenuDTO menu = new MenuDTO()
            //{
            //    mnu_id = "2",
            //    mnu_name = "TEST_name",
            //    mnu_area = "TEST_area",
            //    mnu_controller = "TEST_controller"
            //};

            var menua = await _apiHelper.PostDataAsync<List<ProcessRequestDetailDTO>>("processrequest_api/Post_SaveDetailData", model);

            return menua;
        }

        public async Task<ProcessRequestDTO> SaveDataToPublicAsync(string pcr_code)
        {
            //MenuDTO menu = new MenuDTO()
            //{
            //    mnu_id = "2",
            //    mnu_name = "TEST_name",
            //    mnu_area = "TEST_area",
            //    mnu_controller = "TEST_controller"
            //};
            ProcessRequestDTO data = new ProcessRequestDTO();

            data = await _apiHelper.GetDataByIdAsync<ProcessRequestDTO>("processrequest_api/Save_DataToPublic", pcr_code);

            return data;
        }

        public async Task<List<LogProcessDTO>> GetHistoryAsync(string tran_id)
        {
            List<LogProcessDTO> objList = new List<LogProcessDTO>();

            LogProcessSearchDTO searchData = new LogProcessSearchDTO() { log_mnu_name = "ProcessRequest", log_tran_id = tran_id };

            objList = await _apiHelper.GetDataListByModelAsync<LogProcessDTO, LogProcessSearchDTO>("processrequest_api/Get_List", searchData);

            return objList;
        }
    }
}
