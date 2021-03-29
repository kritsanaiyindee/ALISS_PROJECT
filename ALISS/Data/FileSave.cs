using ALISS.Data.Client;
using ALISS.GLASS.DTO;
using ALISS.DropDownList.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Log4NetLibrary;

namespace ALISS.Data
{
    public class FileSave : IFileSave
    {
        private IJSRuntime JSRuntime { get; }
        private IConfiguration Configuration { get; }
        private ApiHelper _apiHelper;
        private string _reportPath;
        private static readonly ILogService log = new LogService(typeof(FileSave));

        public FileSave(IConfiguration configuration, IJSRuntime jSRuntime)
        {
            //_reportPath = configuration["ReportPath"];
            JSRuntime = jSRuntime;
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }

        public async Task DownloadFile(string ServerFileName, string OutputFileName, string contentType) 
        {          
            try
            {
                log.MethodStart();
                List<ParameterDTO> objParamList = new List<ParameterDTO>();
                var searchParam = new ParameterDTO() { prm_code_major = "RPT_PROCESS_PATH" };

                objParamList = await _apiHelper.GetDataListByModelAsync<ParameterDTO, ParameterDTO>("dropdownlist_api/GetParameterList", searchParam);

                if (objParamList.FirstOrDefault(x => x.prm_code_minor == "PATH") != null)
                {
                    _reportPath = objParamList.FirstOrDefault(x => x.prm_code_minor == "PATH").prm_value;
                    var FullPath = Path.Combine(_reportPath, ServerFileName);
                    if (File.Exists(FullPath))
                    {
                        byte[] bytes = File.ReadAllBytes(FullPath);
                        using (MemoryStream oMemoryStream = new MemoryStream())
                        {
                            using (var stream = new FileStream(FullPath, FileMode.Open))
                            {
                                await stream.CopyToAsync(oMemoryStream);
                            }
                            await JSRuntime.InvokeVoidAsync("BlazorFileSaver.saveAsBase64", OutputFileName, oMemoryStream.ToArray(), contentType);
                        }
                    }
                    else
                    {
                        //File Not Found
                        await JSRuntime.InvokeAsync<object>("ShowAlert", "File not Found");
                    }        
                }
                else
                {
                    await JSRuntime.InvokeAsync<object>("ShowAlert", "ไม่พบ Config PATH กรุณาติดต่อผู้ดูแลระบบ");
                }
                log.MethodFinish();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }                        
        }           
    }
}
