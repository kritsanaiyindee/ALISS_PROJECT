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
using OfficeOpenXml.Style;
using System.Drawing;
using ALISS.DropDownList.DTO;

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

        public async Task<HISFileTemplateDTO> GetHISTemplate_Active_Async(HISFileTemplateDTO model)
        {
            List<HISFileTemplateDTO> objList = new List<HISFileTemplateDTO>();
            
            objList = await _apiHelper.GetDataListByModelAsync<HISFileTemplateDTO, HISFileTemplateDTO>("his_api/GetHISTemplateActive", model);
            
            return objList.FirstOrDefault();
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
            var HISFile = await _apiHelper.PostDataAsync<HISUploadDataDTO>("his_api/Post_SaveSPFileUploadData", model);
            return HISFile;
        }
        public async Task<List<LabDataWithHISDTO>> GetLabDataWithHISByModelAsync(LabDataWithHISSearchDTO model)
        {
            List<LabDataWithHISDTO> objList = new List<LabDataWithHISDTO>();
            objList = await _apiHelper.GetDataListByModelAsync<LabDataWithHISDTO, LabDataWithHISSearchDTO>("his_api/GetLabDataWithHIS", model);
            return objList;
        }

        public async Task<List<STGHISFileUploadDetailDTO>> GetHISDeatilByModelAsync(HISUploadDataDTO model)
        {
            List<STGHISFileUploadDetailDTO> objList = new List<STGHISFileUploadDetailDTO>();
            objList = await _apiHelper.GetDataListByModelAsync<STGHISFileUploadDetailDTO, HISUploadDataDTO>("his_api/GetSTGHISUploadDetail", model);
            return objList;
        }
        public async Task<List<WHONETColumnDTO>> GetListByModelActiveAsync(WHONETColumnDTO searchData)
        {
            List<WHONETColumnDTO> objList = new List<WHONETColumnDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<WHONETColumnDTO, WHONETColumnDTO>("whonetcolumn_api/Get_List_Active_ByModel", searchData);

            return objList;
        }



        public async Task ExportLabDataWithHIS(IJSRuntime iJSRuntime, LabDataWithHISSearchDTO model, MasterTemplateDTO ActiveMasterTemplate, HISFileTemplateDTO HISTemplateActive)       
        {
            byte[] fileContents;
            const int idxColLabNo = 3;
            const int idxColSpcDate = 4;
            const int idxColHN = 2;
            const int idxColRef = 1;

            int idcColWhonetConfig = 5;
            var lstDynamicColumn = new List<int>();
            var dctWhonetColumn = new Dictionary<string, int>();

            //const int idxColAdmisDate = 6;
            //const int idxColCINI = 7;
            int idxRowCurrent = 1;

            string COL_REF_NO = HISTemplateActive.hft_field1; // "Ref No";
            string COL_HN_NO = HISTemplateActive.hft_field2; //"HN";
            string COL_LAB_NO = HISTemplateActive.hft_field3; // "Lab";
            string COL_DATE = HISTemplateActive.hft_field4; //"Date";

            List<LabDataWithHISDTO> objLabWithRef = new List<LabDataWithHISDTO>();
            List<HISDetailDTO> objHISDetail = new List<HISDetailDTO>();
            List<WHONETColumnDTO> objWhonetActive = new List<WHONETColumnDTO>();
            List<string> objWhonetHISColumn = new List<string>();
            WHONETColumnDTO objSearchWhonet = new WHONETColumnDTO();
            List<STGLabFileDataDetailDTO> objLabDetail = new List<STGLabFileDataDetailDTO>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //sp_GET_LabDataWithHIS
            objLabWithRef = await _apiHelper.GetDataListByModelAsync<LabDataWithHISDTO, LabDataWithHISSearchDTO>("his_api/GetLabDataWithHIS", model);
            objHISDetail = await _apiHelper.GetDataListByModelAsync<HISDetailDTO, LabDataWithHISSearchDTO>("his_api/GetHISDetail", model);
            objLabDetail = await _apiHelper.GetDataListByModelAsync<STGLabFileDataDetailDTO, LabDataWithHISSearchDTO>("his_api/GetLabDataWithHISDetail", model);
           
            if (objLabWithRef.Count == 0)
            {
                // Show message No data to export
            }
            using (var package = new ExcelPackage())
            {
                var workSheet = package.Workbook.Worksheets.Add("REF_HIS");

                // Header Column
                workSheet.Cells[idxRowCurrent, idxColRef].Value = COL_REF_NO;
                workSheet.Cells[idxRowCurrent, idxColHN].Value = COL_HN_NO;
                workSheet.Cells[idxRowCurrent, idxColLabNo].Value = COL_LAB_NO;
                workSheet.Cells[idxRowCurrent, idxColSpcDate].Value = COL_DATE;               

                objSearchWhonet.wnc_mst_code = ActiveMasterTemplate.mst_code;

                objWhonetActive = await _apiHelper.GetDataListByModelAsync<WHONETColumnDTO, WHONETColumnDTO>("whonetcolumn_api/Get_List_Active_ByModel", objSearchWhonet);
                if (objWhonetActive != null)
                {
                    // ToDo : change wnc_mendatory to wnc_his_export_flag
                    var objWhonetHIS = objWhonetActive.Where(w => w.wnc_mendatory == true).ToList();
                    objWhonetHISColumn = objWhonetHIS.Select(s => s.wnc_name).ToList();
                    //objWhonetHISColumn = new List<string> { "Last name", "First name", "Sex", "Date of birth", "Age", "Location" };
                    foreach (var obj in objWhonetHISColumn)
                    {
                        // Dynamic Column From Whonet Setting
                        workSheet.Cells[idxRowCurrent, idcColWhonetConfig].Value = obj;

                        if (!dctWhonetColumn.ContainsKey(obj))
                        {
                            dctWhonetColumn.Add(obj, idcColWhonetConfig);
                        }

                        //lstDynamicColumn.Add(idcColWhonetConfig);
                        idcColWhonetConfig++;
                    }
                }

                foreach (LabDataWithHISDTO objLab in objLabWithRef)
                {
                    //if (idxRowCurrent == 11) { break; } //for test
                    idxRowCurrent += 1;
                    workSheet.Cells[idxRowCurrent, idxColRef].Value = objLab.ref_no;
                    workSheet.Cells[idxRowCurrent, idxColHN].Value = objLab.hn_no;
                    workSheet.Cells[idxRowCurrent, idxColLabNo].Value = objLab.lab_no;
                  
                    workSheet.Cells[idxRowCurrent, idxColSpcDate].Value = objLab.spec_date;
                    workSheet.Column(idxColSpcDate).Style.Numberformat.Format = "dd/MM/yyyy";

                    // Note : obj.huh_id ที่เอามา where ต้องหาจาก query มาแล้วว่าเป็นตัวล่าสุด ที่จะ export ออกมา สถานะไม่ใช่ delete ...    
                    //(First) Find HIS Data from Upload HIS
                    var objHISDetailFromHIS = objHISDetail.Where(w => w.ref_no == objLab.ref_no
                                                            && w.hn_no == objLab.hn_no
                                                            && w.lab_no == objLab.lab_no
                                                            && w.spec_date == objLab.spec_date).ToList();
                    foreach (var objHIS in objHISDetailFromHIS)
                    {
                        if (objWhonetHISColumn.Contains(objHIS.hud_field_name))
                        {
                            if (!string.IsNullOrEmpty(objHIS.hud_field_value))
                            {
                                workSheet.Cells[idxRowCurrent, dctWhonetColumn[objHIS.hud_field_name]].Value = objHIS.hud_field_value;
                            }
                        }
                    }

                    //(Second) Find HIS Data from Lab 
                    Guid guidHeaderId = new Guid(objLab.lab_header_id);
                    var objHISDetailFromLab = objLabDetail.Where(w => w.ldd_ldh_id == guidHeaderId).ToList();
                    foreach (var obj in objHISDetailFromLab)
                    {
                        if (objWhonetHISColumn.Contains(obj.ldd_whonetfield))
                        {
                            if (!string.IsNullOrEmpty(obj.ldd_originalvalue))
                            {
                                //var DecodeValue = CryptoHelper.UnicodeDecoding(obj.ldd_originalvalue);
                                if (workSheet.Cells[idxRowCurrent, dctWhonetColumn[obj.ldd_whonetfield]].Value == null)
                                {
                                    workSheet.Cells[idxRowCurrent, dctWhonetColumn[obj.ldd_whonetfield]].Value = obj.ldd_originalvalue;
                                }
                                
                            }
                        }
                    }              

                } // End Loop Lab Data

                workSheet.Column(idxColRef).AutoFit();
                workSheet.Column(idxColHN).AutoFit();
                workSheet.Column(idxColLabNo).AutoFit();
                workSheet.Column(idxColSpcDate).AutoFit();
                workSheet.Protection.IsProtected = true; // Protect whole sheet
                foreach (var colHIS in dctWhonetColumn.Values)
                {
                    workSheet.Column(colHIS).Style.Locked = false;//unlock workSheet
                    workSheet.Column(1).AutoFit();
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
            var idxColErrorDesc = 3;
            var strSheetName = string.Format("{0}_{1}"
                                        , Path.GetFileNameWithoutExtension(HISFileUploadData.hfu_file_name)
                                        , "Summary");
            var colorDarkOliveGreen = Color.FromArgb(189, 215, 238);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var workSheet = package.Workbook.Worksheets.Add(strSheetName);

                // Header Column            
                workSheet.Cells[idxRowCurrent, idxColErrorRowNo].Value = "Row No.";
                workSheet.Cells[idxRowCurrent, idxColErrorColumn].Value = "Field";
                workSheet.Cells[idxRowCurrent, idxColErrorDesc].Value = "Description";

                using (ExcelRange h = workSheet.Cells[1, idxColErrorRowNo, 1, idxColErrorDesc])
                {
                    h.Style.Font.Bold = true;
                    h.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    h.Style.Fill.BackgroundColor.SetColor(colorDarkOliveGreen);
                    h.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    h.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                foreach (var obj in HISFileSummary)
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
                                        , "Summary.xlsx");
                iJSRuntime.InvokeAsync<HISFileUploadService>(
                    "saveAsFile",
                    strExportFileName,
                    Convert.ToBase64String(fileContents)
                    );

            }
            catch (Exception ex)
            {

            }
        }

        public async Task<bool> GenerateExportWithError(IJSRuntime iJSRuntime, List<HISFileUploadSummaryDTO> HISFileSummary, HISUploadDataDTO HISFileUpload)
        {
            var blnError = false;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            string strPath = "";
            List<ParameterDTO> objParamList = new List<ParameterDTO>();
            var searchModel = new ParameterDTO() { prm_code_major = "UPLOAD_PATH" };

            objParamList = await _apiHelper.GetDataListByModelAsync<ParameterDTO, ParameterDTO>("dropdownlist_api/GetParameterList", searchModel);

            if (objParamList.FirstOrDefault(x => x.prm_code_minor == "PATH") != null)
            {
                strPath = objParamList.FirstOrDefault(x => x.prm_code_minor == "PATH").prm_value;
            }
            else
            {
                await iJSRuntime.InvokeAsync<object>("ShowAlert", "ไม่พบ Path กรุณาติดต่อผู้ดูแลระบบ");
                blnError = true;
                return blnError;
            }

            //Read Original File
            string strUploadDate = HISFileUpload.hfu_createdate.Value.ToString("yyyyMMdd");
            var strFilePath = Path.Combine(strPath, strUploadDate, HISFileUpload.hfu_hos_code);
            var strFullPath = Path.Combine(strFilePath, HISFileUpload.hfu_file_name);
            if (!File.Exists(strFullPath))
            {
                await iJSRuntime.InvokeAsync<object>("ShowAlert", "ไม่พบไฟล์ต้นฉบับ");
                blnError = true;
                return blnError;
            }

            var extension = Path.GetExtension(HISFileUpload.hfu_file_name);
            var copypath = strFullPath.Replace(extension, "_ExportWarning" + extension);

            if (File.Exists(copypath))
            {
                File.Delete(copypath);
            }

            File.Copy(strFullPath, copypath);

            if (!File.Exists(copypath))
            {
                await iJSRuntime.InvokeAsync<object>("ShowAlert", "การเรียกไฟล์ต้นฉบับไม่สมบูรณ์");
                blnError = true;
                return blnError;
            }

            var colorTomatoRed = Color.FromArgb(255, 99, 71);
            var colorSalmon = Color.FromArgb(255, 160 ,122) ;
            var colorDarkRed = Color.FromArgb(192, 0, 0);           
            FileInfo fileInfo = new FileInfo(copypath);
          
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                int dataRows = worksheet.Dimension.Rows; 
                int dataColumns = worksheet.Dimension.Columns;
                int colDescription = dataColumns + 1;
           

                if (HISFileSummary != null)
                {
                    worksheet.Cells[1, colDescription].Value = "Remarks";
                    using (ExcelRange h = worksheet.Cells[1, colDescription])
                    {
                        h.Style.Font.Bold = true;                                        
                        h.Style.Font.Color.SetColor(colorDarkRed);
                        h.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    for (int iRow = 2; iRow <= dataRows; iRow++)
                    {                    
                        var objFile = HISFileSummary.Where(w => w.hus_error_fieldrecord.Trim() == iRow.ToString()).ToList();
                        if (objFile.Count() > 0) 
                        {
                            var drWarningRow = objFile.FirstOrDefault();
                            worksheet.Cells[iRow, colDescription].Value = drWarningRow.hus_error_fieldname + " : " + drWarningRow.hus_error_fielddescr;
                            //worksheet.Cells[iRow,1,iRow, colDescription].Style.Fill.BackgroundColor.SetColor(colorSalmon);
                        }
                        //string content = worksheet.Cells[iRow, iCol].Value.ToString();
                    }
                    package.Save();
                }
            //fileContents = package.GetAsByteArray();  
            }            
            
            var strExportFileName = Path.GetFileName(copypath);          
            byte[] bytes = File.ReadAllBytes(copypath);
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            using (MemoryStream oMemoryStream = new MemoryStream())
            {
                using (var stream = new FileStream(copypath, FileMode.Open))
                {
                    await stream.CopyToAsync(oMemoryStream);
                }
                await iJSRuntime.InvokeVoidAsync("BlazorFileSaver.saveAsBase64", strExportFileName, oMemoryStream.ToArray(), contentType);
            }
            return blnError;
        }
    }
}
