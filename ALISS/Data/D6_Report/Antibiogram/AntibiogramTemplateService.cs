using ALISS.ANTIBIOGRAM.DTO;
using ALISS.Data.Client;
using ALISS.DropDownList.DTO;
using ALISS.Master.DTO;
using ALISS.MasterManagement.DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ALISS.Data.D6_Report.Antibiogram
{
    public class AntibiogramTemplateService
    {
        private IConfiguration Configuration { get; }
        private string _reportPath;
        private ApiHelper _apiHelper;
      
        public AntibiogramTemplateService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
            //_reportPath = configuration["ReportPath"];
        }
        public async Task<List<AntibiogramHospitalTemplateDTO>> GetAntibiogramHospitalTemplateListModelAsync(AntiHospitalSearchDTO searchData)
        {
            List<AntibiogramHospitalTemplateDTO> objList = new List<AntibiogramHospitalTemplateDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<AntibiogramHospitalTemplateDTO, AntiHospitalSearchDTO>("antitemplate_api/GetAntiHospitalTemplateModel", searchData);

            return objList;
        }

        public async Task<List<AntibiogramAreaHealthTemplateDTO>> GetAntibiogramAreaHealthTemplateListModelAsync(AntiHospitalSearchDTO searchData)
        {
            List<AntibiogramAreaHealthTemplateDTO> objList = new List<AntibiogramAreaHealthTemplateDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<AntibiogramAreaHealthTemplateDTO, AntiHospitalSearchDTO>("antitemplate_api/GetAntiAreaHealthTemplateModel", searchData);

            return objList;
        }

        public async Task<List<AntibiogramNationTemplateDTO>> GetAntibiogramNationTemplateListModelAsync(AntiHospitalSearchDTO searchData)
        {
            List<AntibiogramNationTemplateDTO> objList = new List<AntibiogramNationTemplateDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<AntibiogramNationTemplateDTO, AntiHospitalSearchDTO>("antitemplate_api/GetAntiNationTemplateModel", searchData);

            return objList;
        }

        public async Task<string> DownloadPDFFileAsync(AntibiogramNationTemplateDTO selectedobj)
        {
            var filename = selectedobj.file_path.Remove(0, 1) + "/" + selectedobj.file_name;
            var extension = Path.GetExtension(selectedobj.file_name);//.xlsx   
            var PdfFileName = filename.Replace(extension, ".pdf"); 
            var statuscode = "";
            var lstfullname = filename.Split("/");

            List<ParameterDTO> objParamList = new List<ParameterDTO>();
            var searchParam = new ParameterDTO() { prm_code_major = "RPT_PROCESS_PATH" };
            objParamList = await _apiHelper.GetDataListByModelAsync<ParameterDTO, ParameterDTO>("dropdownlist_api/GetParameterList", searchParam);

            if (objParamList.FirstOrDefault(x => x.prm_code_minor == "PATH") != null)
            {
                _reportPath = objParamList.FirstOrDefault(x => x.prm_code_minor == "PATH").prm_value;
                FileInfo outputfileInfo = new FileInfo(Path.Combine(_reportPath, filename));

                if (File.Exists(Path.Combine(_reportPath, filename)))
                {
                   statuscode = await _apiHelper.ExportToPdfDataAsync("exportpdf_api/PdfGenerator", lstfullname);
                }                                  
            }
            else
            {
                statuscode = "ERR_PATH";
            }
            return statuscode;
        }

        public async Task<string> DownloadPDFFileAsync(AntibiogramHospitalTemplateDTO selectedobj)
        {
            var filename = selectedobj.file_path.Remove(0, 1) + "/" + selectedobj.file_name;
            var extension = Path.GetExtension(selectedobj.file_name);//.xlsx   
            var PdfFileName = filename.Replace(extension, ".pdf");
            var statuscode = "";
            var lstfullname = filename.Split("/");

            List<ParameterDTO> objParamList = new List<ParameterDTO>();
            var searchParam = new ParameterDTO() { prm_code_major = "RPT_PROCESS_PATH" };
            objParamList = await _apiHelper.GetDataListByModelAsync<ParameterDTO, ParameterDTO>("dropdownlist_api/GetParameterList", searchParam);

            if (objParamList.FirstOrDefault(x => x.prm_code_minor == "PATH") != null)
            {
                _reportPath = objParamList.FirstOrDefault(x => x.prm_code_minor == "PATH").prm_value;
                FileInfo outputfileInfo = new FileInfo(Path.Combine(_reportPath, filename));

                if (File.Exists(Path.Combine(_reportPath, filename)))
                {
                    statuscode = await _apiHelper.ExportToPdfDataAsync("exportpdf_api/PdfGenerator", lstfullname);
                }
            }
            else
            {
                statuscode = "ERR_PATH";
            }
            return statuscode;
        }

        public async Task<string> DownloadPDFFileAsync(AntibiogramAreaHealthTemplateDTO selectedobj)
        {
            var filename = selectedobj.file_path.Remove(0, 1) + "/" + selectedobj.file_name;
            var extension = Path.GetExtension(selectedobj.file_name);//.xlsx   
            var PdfFileName = filename.Replace(extension, ".pdf");
            var statuscode = "";
            var lstfullname = filename.Split("/");

            List<ParameterDTO> objParamList = new List<ParameterDTO>();
            var searchParam = new ParameterDTO() { prm_code_major = "RPT_PROCESS_PATH" };
            objParamList = await _apiHelper.GetDataListByModelAsync<ParameterDTO, ParameterDTO>("dropdownlist_api/GetParameterList", searchParam);

            if (objParamList.FirstOrDefault(x => x.prm_code_minor == "PATH") != null)
            {
                _reportPath = objParamList.FirstOrDefault(x => x.prm_code_minor == "PATH").prm_value;
                FileInfo outputfileInfo = new FileInfo(Path.Combine(_reportPath, filename));

                if (File.Exists(Path.Combine(_reportPath, filename)))
                {
                    statuscode = await _apiHelper.ExportToPdfDataAsync("exportpdf_api/PdfGenerator", lstfullname);
                }
            }
            else
            {
                statuscode = "ERR_PATH";
            }
            return statuscode;
        }

    }
}
