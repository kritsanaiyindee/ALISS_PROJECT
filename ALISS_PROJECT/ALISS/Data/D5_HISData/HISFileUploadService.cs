using ALISS.Data.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ALISS.HISUpload.DTO;
using Microsoft.JSInterop;
using OfficeOpenXml;
using ALISS.MasterManagement.DTO;
using ALISS.Data.D4_UserManagement.MasterManagement;
using System.IO;

namespace ALISS.Data.D5_HISData
{
    public class HISFileUploadService
    {
        private IConfiguration configuration { get; }
        private ApiHelper _apiHelper;
        private WHONETColumnService _WhonetColumnService;
        public HISFileUploadService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }

        public async Task<List<HISUploadDataDTO>> GetHISFileUploadListByModelAsync(HISUploadDataSearchDTO model)
        {
            List<HISUploadDataDTO> objList = new List<HISUploadDataDTO>();
            objList = await _apiHelper.GetDataListByModelAsync<HISUploadDataDTO, HISUploadDataSearchDTO>("his_api/GetHISFileUploadList", model);
            return objList;
        }
        public async Task<HISUploadDataDTO> GetHISFileUploadDataByIdAsync(int hfu_Id)
        {
            HISUploadDataDTO objList = new HISUploadDataDTO();

            objList = await _apiHelper.GetDataByIdAsync<HISUploadDataDTO>("his_api/GetHISFileUploadDataById", hfu_Id.ToString());

            return objList;
        }

        public async Task<List<HISFileTemplateDTO>> GetHISTemplate_Active_Async(HISFileTemplateDTO model)
        {
            List<HISFileTemplateDTO> objList = new List<HISFileTemplateDTO>();
            objList = await _apiHelper.GetDataListByModelAsync<HISFileTemplateDTO, HISFileTemplateDTO>("his_api/GetHISTemplateActive", model);
            return objList;
        }
        public async Task<List<HISFileUploadSummaryDTO>> GetHISFileUploadSummaryListByIdAsync(int HISuploadID)
        {
            List<HISFileUploadSummaryDTO> objList = new List<HISFileUploadSummaryDTO>();

            objList = await _apiHelper.GetDataListByParamsAsync<HISFileUploadSummaryDTO>("his_api/GetHISFileUploadSummaryById", HISuploadID.ToString());

            return objList;
        }
        public async Task<HISUploadDataDTO> SaveHISFileUploadDataAsync(HISUploadDataDTO model)
        {
            if (model.hfu_id == 0)
            {
                //model.lfu_id = Guid.NewGuid();
                model.hfu_status = 'N';
                model.hfu_delete_flag = false;
            }
            var LabFile = await _apiHelper.PostDataAsync<HISUploadDataDTO>("his_api/Post_SaveSPFileUploadData", model);
            return LabFile;
        }
        public async Task<List<LabDataWithHISDTO>> GetLabDataWithHISByModelAsync(LabDataWithHISSearchDTO model)
        {
            List<LabDataWithHISDTO> objList = new List<LabDataWithHISDTO>();
            objList = await _apiHelper.GetDataListByModelAsync<LabDataWithHISDTO, LabDataWithHISSearchDTO>("his_api/GetLabDataWithHIS", model);
            return objList;
        }
        public async Task<List<WHONETColumnDTO>> GetListByModelActiveAsync(WHONETColumnDTO searchData)
        {
            List<WHONETColumnDTO> objList = new List<WHONETColumnDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<WHONETColumnDTO, WHONETColumnDTO>("whonetcolumn_api/Get_List_Active_ByModel", searchData);

            return objList;
        }

        public async Task ExportLabDataWithHIS(IJSRuntime iJSRuntime, LabDataWithHISSearchDTO model, MasterTemplateDTO ActiveMasterTemplate)       
        {
            byte[] fileContents;
            const int idxColLabNo = 3;
            const int idxColSpcDate = 4;
            const int idxColHN = 2;
            const int idxColRef = 1;

            int idcColWhonetConfig = 5;
            var lstDynamicColumn = new List<int>();

            //const int idxColAdmisDate = 6;
            //const int idxColCINI = 7;
            int idxRowCurrent = 1;
           
            List<LabDataWithHISDTO> objLabHIS = new List<LabDataWithHISDTO>();
            List<WHONETColumnDTO> objWhonetActive = new List<WHONETColumnDTO>();
            WHONETColumnDTO objSearchWhonet = new WHONETColumnDTO();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
           
            using (var package = new ExcelPackage())
            {
                var workSheet = package.Workbook.Worksheets.Add("REF_HIS");

                // Header Column
                workSheet.Cells[idxRowCurrent, idxColRef].Value = "Ref";
                workSheet.Cells[idxRowCurrent, idxColHN].Value = "HN";
                workSheet.Cells[idxRowCurrent, idxColLabNo].Value = "Lab No.";
                workSheet.Cells[idxRowCurrent, idxColSpcDate].Value = "Date";
                // Dynamic Column From Whonet Setting

                objSearchWhonet.wnc_mst_code = ActiveMasterTemplate.mst_code;

                objWhonetActive = await _apiHelper.GetDataListByModelAsync<WHONETColumnDTO, WHONETColumnDTO>("whonetcolumn_api/Get_List_Active_ByModel", objSearchWhonet);
                if (objWhonetActive != null)
                {
                    // ToDo : change wnc_mendatory to wnc_his_export_flag
                    var objWhonetHIS = objWhonetActive.Where(w => w.wnc_mendatory == true).ToList();
                    foreach(var obj in objWhonetHIS)
                    {
                        workSheet.Cells[idxRowCurrent, idcColWhonetConfig].Value = obj.wnc_name;
                        lstDynamicColumn.Add(idcColWhonetConfig);
                        idcColWhonetConfig++;
                    }
                }

                //workSheet.Cells[idxRowCurrent, idxColAdmisDate].Value = "Admission Date";
                //workSheet.Cells[idxRowCurrent, idxColCINI].Value = "CI/NI";

                objLabHIS = await _apiHelper.GetDataListByModelAsync<LabDataWithHISDTO, LabDataWithHISSearchDTO>("his_api/GetLabDataWithHIS", model);

                foreach (LabDataWithHISDTO obj in objLabHIS)
                {
                    idxRowCurrent += 1;
                    workSheet.Cells[idxRowCurrent, idxColRef].Value = obj.ref_no;
                    workSheet.Cells[idxRowCurrent, idxColHN].Value = obj.hn_no;
                    workSheet.Cells[idxRowCurrent, idxColLabNo].Value = obj.lab_no;
                    workSheet.Cells[idxRowCurrent, idxColSpcDate].Value = obj.spec_date;

                    //if ((obj.admission_date.HasValue)) { workSheet.Cells[idxRowCurrent, idxColAdmisDate].Value = obj.admission_date; }
                    //if (!string.IsNullOrEmpty(obj.cini)) { workSheet.Cells[idxRowCurrent, idxColCINI].Value = obj.cini; }
                   
                }

                workSheet.Protection.IsProtected = true; // Protect whole sheet
                foreach(var colHIS in lstDynamicColumn)
                {                    
                    workSheet.Column(colHIS).Style.Locked = false;//unlock column
                }
               
                fileContents = package.GetAsByteArray();
            }

            try
            {
                await iJSRuntime.InvokeAsync<HISFileUploadService>(
                      "saveAsFile",
                      "HIS_WITH_SP.xlsx",
                      Convert.ToBase64String(fileContents)
                      );

            }
            catch (Exception ex)
            {

            }

        }

        public void GenerateExportSummary(IJSRuntime iJSRuntime, List<HISFileUploadSummaryDTO> HISFileSummary, HISUploadDataDTO HISFileUploadData)
        {
            byte[] fileContents;

            var idxRowCurrent = 1;

            var idxColErrorRowNo = 1;
            var idxColErrorColumn = 2;
            var idxColErrorDesc = 2;
            var strSheetName = string.Format("{0}_{1}"
                                        , Path.GetFileNameWithoutExtension(HISFileUploadData.hfu_file_name)
                                        , "Summary");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var workSheet = package.Workbook.Worksheets.Add(strSheetName);

                // Header Column
                workSheet.Cells[idxRowCurrent, idxColErrorRowNo].Value = "Row No.";
                workSheet.Cells[idxRowCurrent, idxColErrorColumn].Value = "Field";
                workSheet.Cells[idxRowCurrent, idxColErrorDesc].Value = "Description";
              
                foreach(var obj in HISFileSummary)
                {
                    idxRowCurrent += 1;
                    workSheet.Cells[idxRowCurrent, idxColErrorRowNo].Value = obj.hus_error_fieldrecord;
                    workSheet.Cells[idxRowCurrent, idxColErrorColumn].Value = obj.hus_error_fieldname;
                    workSheet.Cells[idxRowCurrent, idxColErrorDesc].Value = obj.hus_error_fielddescr;
                }

                fileContents = package.GetAsByteArray();
            }

            try
            {
                var strExportFileName = string.Format("{0}_{1}_{2}"
                                        , DateTime.Today.ToString("yyyyMMdd")
                                        , Path.GetFileNameWithoutExtension(HISFileUploadData.hfu_file_name)
                                        , "Summary");
                iJSRuntime.InvokeAsync<HISFileUploadService>(
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
