using ALISS.Data.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ALISS.LabFileUpload.DTO;
using Microsoft.JSInterop;
using OfficeOpenXml;
using System.IO;

namespace ALISS.Data.D1_Upload
{
    public class LabFileUploadService
    {
        private IConfiguration Configuration { get; }

        private ApiHelper _apiHelper;

        public LabFileUploadService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }

        public async Task<List<LabFileUploadDataDTO>> GetLabFileUploadListByModelAsync(LabFileUploadSearchDTO model)
        {
            List<LabFileUploadDataDTO> objList = new List<LabFileUploadDataDTO>();
            objList = await _apiHelper.GetDataListByModelAsync<LabFileUploadDataDTO, LabFileUploadSearchDTO>("labfileupload_api/Get_LabFileUploadListByModel", model);
            return objList;
        }


        public async Task<LabFileUploadDataDTO> GetLabFileUploadDataAsync(string lfu_Id)
        {
            LabFileUploadDataDTO LabFileUpload = new LabFileUploadDataDTO();

            LabFileUpload = await _apiHelper.GetDataByIdAsync<LabFileUploadDataDTO>("labfileupload_api/GetLabFileUploadDataById", lfu_Id);

            return LabFileUpload;
        }

        public async Task<LabFileUploadDataDTO> SaveLabFileUploadDataAsync(LabFileUploadDataDTO model)
        {
            if (model.lfu_id.Equals(Guid.Empty))
            {
                model.lfu_id = Guid.NewGuid();
                model.lfu_status = 'N';
                model.lfu_flagdelete = false;
            }
            

           
            var LabFile = await _apiHelper.PostDataAsync<LabFileUploadDataDTO>("labfileupload_api/Post_SaveLabFileUploadData", model);
           
            return LabFile;
        }

        public async Task<List<LabFileSummaryHeaderListDTO>> GetLabFileSummaryHeaderListAsync(string lfu_Id)
        {
            List<LabFileSummaryHeaderListDTO> objList = new List<LabFileSummaryHeaderListDTO>();

            objList = await _apiHelper.GetDataListByParamsAsync<LabFileSummaryHeaderListDTO>("labfileupload_api/GetLabFileSummaryHeaderBylfuId", lfu_Id);

            return objList;
        }

        public async Task<List<LabFileSummaryDetailListDTO>> GetLabFileSummaryDetailListAsync(string fsh_id)
        {
            List<LabFileSummaryDetailListDTO> objList = new List<LabFileSummaryDetailListDTO>();

            objList = await _apiHelper.GetDataListByParamsAsync<LabFileSummaryDetailListDTO>("labfileupload_api/GetLabFileSummaryDetailBylfuId", fsh_id);

            return objList;
        }

        public async Task<List<LabFileErrorHeaderListDTO>> GetLabFileErrorHeaderListAsync(string lfu_Id)
        {
            List<LabFileErrorHeaderListDTO> objList = new List<LabFileErrorHeaderListDTO>();

            objList = await _apiHelper.GetDataListByParamsAsync<LabFileErrorHeaderListDTO>("labfileupload_api/GetLabFileErrorHeaderBylfuId", lfu_Id);

            return objList;
        }

        public async Task<List<LabFileErrorDetailListDTO>> GetLabFileErrorDetailListAsync(string lfu_Id)
        {
            List<LabFileErrorDetailListDTO> objList = new List<LabFileErrorDetailListDTO>();

            objList = await _apiHelper.GetDataListByParamsAsync<LabFileErrorDetailListDTO>("labfileupload_api/GetLabFileErrorDetailBylfuId", lfu_Id);

            return objList;
        }


        public void GenerateExportSummary(IJSRuntime iJSRuntime, List<LabFileSummaryHeaderListDTO> LabFileSummaryHeaderList,
            List<LabFileErrorDetailListDTO> LabFileErrorDetailList)
        {
            byte[] fileContents;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using(var package = new ExcelPackage())
            {
                var workSheet = package.Workbook.Worksheets.Add("Summary");
                var workSheet2 = package.Workbook.Worksheets.Add("Error");
                #region Hearder Row
                workSheet.Cells[1, 1].Value = "Specimen";
                workSheet.Cells[1, 2].Value = "Organism";
                workSheet.Cells[1, 3].Value = "Total";
                #endregion

                int row = 2;
                foreach (LabFileSummaryHeaderListDTO item in LabFileSummaryHeaderList)
                {
                    workSheet.Cells[row, 1].Value = item.fsh_code + " - " + item.fsh_desc;
                    workSheet.Cells[row, 3].Value = item.fsh_total; ;
                    row++;
                    var detail = item.LabFileSummaryDetailLists;

                    if (detail != null)
                    {
                        foreach (LabFileSummaryDetailListDTO d in detail)
                        {
                            workSheet.Cells[row, 2].Value = d.fsd_organismcode + " - " + d.fsd_organismdesc;
                            workSheet.Cells[row, 3].Value = d.fsd_total;

                            row++;
                        }
                    }

                }


                workSheet2.Cells[1, 1].Value = "Message";
                workSheet2.Cells[1, 2].Value = "Local Value";
              

                row = 2;

                foreach (LabFileErrorDetailListDTO err in LabFileErrorDetailList)
                {
                    workSheet2.Cells[row, 1].Value = err.feh_message;
                    workSheet2.Cells[row, 2].Value = err.fed_localvalue;
                    row++;
                }

               fileContents = package.GetAsByteArray();
            }

            try
            {
                iJSRuntime.InvokeAsync<LabFileUploadService>(
                    "saveAsFile",
                    "Summary.xlsx",
                    Convert.ToBase64String(fileContents)
                    );
         
            }
            catch (Exception ex)
            {

            }
        }
    }
}

