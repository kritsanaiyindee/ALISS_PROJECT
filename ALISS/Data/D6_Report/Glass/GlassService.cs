using ALISS.GLASS.DTO;
using ALISS.Data.Client;
using ALISS.DropDownList.DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Log4NetLibrary;

namespace ALISS.Data.D6_Report.Glass
{
    public class GlassService
    {
        private IConfiguration Configuration { get; }
        private ApiHelper _apiHelper;
        private string _reportPath;
        private static readonly ILogService log = new LogService(typeof(GlassService));

        public GlassService(IConfiguration configuration)
        {           
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
            //_reportPath = configuration["ReportPath"];
        }

        public async Task<List<GlassFileListDTO>> GetGlassPublicFileListAsync()
        {
            List<GlassFileListDTO> objList = new List<GlassFileListDTO>();

            objList = await _apiHelper.GetDataListAsync<GlassFileListDTO>("glassreport_api/GetGlassPublicFileList");

            return objList;
        }

        public async Task<List<GlassFileListDTO>> GetGlassPublicFileListModelAsync(GlassFileListNationSearchDTO searchData)
        {
            List<GlassFileListDTO> objList = new List<GlassFileListDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<GlassFileListDTO, GlassFileListNationSearchDTO>("glassreport_api/GetGlassPublicFileListModel", searchData);

            return objList;
        }

        public async Task<List<GlassFileListDTO>> GetGlassPublicFileListRegHealthModelAsync(GlassFileListNationSearchDTO searchData)
        {
            List<GlassFileListDTO> objList = new List<GlassFileListDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<GlassFileListDTO, GlassFileListNationSearchDTO>("glassreport_api/GetGlassPublicRegHealthFileListModel", searchData);

            return objList;
        }

        public async Task<string> RequestAnalyseFileAsync(GlassFileListDTO selectedobj)
        {
            // -- Note --
            // สร้าง Report ใหม่ทุกครั้งที่กดปุ่ม Download
            var statuscode = "";
            try
            {
                log.MethodStart();
                var filename = selectedobj.analyze_file_path.Remove(0, 1) + "\\" + selectedobj.analyze_file_name;
                var strFullPath = "";
                // Get Report Path
                List<ParameterDTO> objParamList = new List<ParameterDTO>();
                var searchParam = new ParameterDTO() { prm_code_major = "RPT_PROCESS_PATH" };
                objParamList = await _apiHelper.GetDataListByModelAsync<ParameterDTO, ParameterDTO>("dropdownlist_api/GetParameterList", searchParam);

                if (objParamList.FirstOrDefault(x => x.prm_code_minor == "PATH") != null)
                {
                    _reportPath = objParamList.FirstOrDefault(x => x.prm_code_minor == "PATH").prm_value;
                    strFullPath = Path.Combine(_reportPath, filename);
                    FileInfo OriginalfileExcel = new FileInfo(strFullPath);
                    statuscode = await _apiHelper.DownloadDataAsync<GlassFileListDTO>("glassreport_api/GenerateAnalyzeFile", OriginalfileExcel, selectedobj);              
                }
                else
                {
                    statuscode = "ERR_PATH";
                }
                log.MethodFinish();  
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return statuscode;                           
        }

        public async Task<string> DownloadPDFFileAsync(GlassFileListDTO selectedobj)
        {
            var statuscode = "";
            try
            {
                log.MethodStart();
                var filename = selectedobj.analyze_file_path.Remove(0, 1) + "/" + selectedobj.analyze_file_name;          
                var fileextension = Path.GetExtension(selectedobj.analyze_file_name).Replace(".",""); //.xlsx   
                var extension = Path.GetExtension(selectedobj.analyze_file_name);
                var PdfFileName = filename.Replace(extension, ".pdf") ; // GLASS/20200730_01_GLASS/2019_13_glass.pdf
                
                var lstfullname = filename.Split("/");
                //var strFullPath = "";

                List<ParameterDTO> objParamList = new List<ParameterDTO>();
                var searchParam = new ParameterDTO() { prm_code_major = "RPT_PROCESS_PATH" };
                objParamList = await _apiHelper.GetDataListByModelAsync<ParameterDTO, ParameterDTO>("dropdownlist_api/GetParameterList", searchParam);

                if (objParamList.FirstOrDefault(x => x.prm_code_minor == "PATH") != null)
                {
                    _reportPath = objParamList.FirstOrDefault(x => x.prm_code_minor == "PATH").prm_value;
                    //FileInfo outputfileInfo = new FileInfo(Path.Combine(_reportPath, PdfFileName));
                    //strFullPath = Path.Combine(_reportPath, PdfFileName);
                    //FileInfo outputfileInfo = new FileInfo(strFullPath);
                    statuscode = await _apiHelper.ExportToPdfDataAsync("exportpdf_api/PdfGenerator",  lstfullname);
                }
                else
                {
                    statuscode = "ERR_PATH";
                }   

                log.MethodFinish();             
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return statuscode;            
        }    
    }
}
