using ALISS.ANTIBIOGRAM.DTO;
using ALISS.Master.DTO;
using ALISS.Data.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.IO;
//using ALISS.PDFReport.Library.DTO;
using ALISS.MasterManagement.DTO;
using ALISS.DropDownList.DTO;

namespace ALISS.Data.D6_Report.Antibiogram
{
    public class ReportService
    {
        private IConfiguration Configuration { get; }

        private ApiHelper _apiHelper;

        public ReportService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }
      
        public async Task<List<AntibiogramDataDTO>> GetListAsync()
        {
            List<AntibiogramDataDTO> objList = new List<AntibiogramDataDTO>();

            objList = await _apiHelper.GetDataListAsync<AntibiogramDataDTO>("report_api/GetAntiHosp");

            return objList;
        }

        public async Task<List<AntibiogramDataDTO>> GetListByParamAsync(AntibiogramDataDTO searchData)
        {
            List<AntibiogramDataDTO> objList = new List<AntibiogramDataDTO>();

            var searchJson = JsonSerializer.Serialize(searchData);

            objList = await _apiHelper.GetDataListByParamsAsync<AntibiogramDataDTO>("report_api/GetAntiHosp", searchJson);

            return objList;
        }

        public async Task<List<AntibiogramDataDTO>> GetListByModelAsync(AntiHospitalSearchDTO searchData)
        {
            List<AntibiogramDataDTO> objList = new List<AntibiogramDataDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<AntibiogramDataDTO, AntiHospitalSearchDTO>("report_api/GetAntiHospModel", searchData);

            return objList;
        }

        public async Task<List<AntibiogramDataDTO>> GetAntibiogramAreaHealthListAsync()
        {
            List<AntibiogramDataDTO> objList = new List<AntibiogramDataDTO>();

            objList = await _apiHelper.GetDataListAsync<AntibiogramDataDTO>("report_api/GetAntiAreaHealth");

            return objList;
        }
      
        public async Task<List<AntibiogramDataDTO>> GetAntibiogramAreaHealthListModelAsync(AntiAreaHealthSearchDTO searchData)
        {
            List<AntibiogramDataDTO> objList = new List<AntibiogramDataDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<AntibiogramDataDTO, AntiAreaHealthSearchDTO>("report_api/GetAntiAreaHealthModel", searchData);

            return objList;
        }

        public async Task<List<AntibiogramDataDTO>> GetAntibiogramNationListAsync()
        {
            List<AntibiogramDataDTO> objList = new List<AntibiogramDataDTO>();

            objList = await _apiHelper.GetDataListAsync<AntibiogramDataDTO>("report_api/GetAntiNation");

            return objList;
        }

        public async Task<List<AntibiogramDataDTO>> GetAntibiogramNationListModelAsync(AntiNationSearchDTO searchData)
        {
            List<AntibiogramDataDTO> objList = new List<AntibiogramDataDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<AntibiogramDataDTO, AntiNationSearchDTO>("report_api/GetAntiNationModel", searchData);

            return objList;
        }
 
    }
}
