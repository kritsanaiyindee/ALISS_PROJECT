using ALISS.Data.Client;
using ALISS.DropDownList.DTO;
using ALISS.HISUpload.DTO;
using BlazorInputFile;
using ExcelDataReader;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ALISS.Data.D5_HISData
{
    public class FileUploadManageService
    {
        private IConfiguration configuration { get; }

        private ApiHelper _apiHelper;
        private string _Path;
        public FileUploadManageService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }
        public async Task<List<HISUploadErrorMessageDTO>> ValidateSPFileAsync(IFileListEntry fileEntry, HISUploadDataDTO HISfileOwner)
        {
            List<HISUploadErrorMessageDTO> ErrorMessage = new List<HISUploadErrorMessageDTO>();                 
            try
            {
                string path = "";
                const bool FIRSTROW_IS_HEADER = true;
                List<ParameterDTO> objParamList = new List<ParameterDTO>();
                var searchModel = new ParameterDTO() { prm_code_major = "UPLOAD_PATH" };

                objParamList = await _apiHelper.GetDataListByModelAsync<ParameterDTO, ParameterDTO>("dropdownlist_api/GetParameterList", searchModel);

                if (objParamList.FirstOrDefault(x => x.prm_code_minor == "PATH") != null)
                {
                    path = objParamList.FirstOrDefault(x => x.prm_code_minor == "PATH").prm_value;
                }
                else
                {
                    ErrorMessage.Add(new HISUploadErrorMessageDTO
                    {
                        hfu_status = 'E',
                        hfu_Err_type = "E",
                        hfu_Err_no = 1,
                        hfu_Err_Column = "",
                        hfu_Err_Message = "ไม่พบ Config PATH กรุณาติดต่อผู้ดูแลระบบ "
                    });

                    return ErrorMessage;
                }

                string str_CurrentDate = DateTime.Now.ToString("yyyyMMdd");               
                path = Path.Combine(path, str_CurrentDate, HISfileOwner.hfu_hos_code);
                bool exists = System.IO.Directory.Exists(path);

                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                path = Path.Combine(path, fileEntry.Name);

                using (FileStream file = new FileStream(path, FileMode.Create))
                {
                    try
                    {
                        await fileEntry.Data.CopyToAsync(file);
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        file.Flush();
                    }

                }

                //        WHONetMappingSearch searchWHONet = new WHONetMappingSearch();
                //        searchWHONet.wnm_mappingid = MappingTemplate.mp_id;
                //        searchWHONet.wnm_mst_code = MappingTemplate.mp_mst_code;

                //        List<WHONetMappingListsDTO> WHONetColumn = await _apiHelper.GetDataListByModelAsync<WHONetMappingListsDTO, WHONetMappingSearch>("mapping_api/Get_WHONetMappingListByModel", searchWHONet);
                //        var WHONetColumnMandatory = WHONetColumn.Where(x => x.wnm_mandatory == true);
                #region ReadExcel
                if (Path.GetExtension(fileEntry.Name) == ".xls" || Path.GetExtension(fileEntry.Name) == ".xlsx")
                {
                    ExcelDataSetConfiguration option = new ExcelDataSetConfiguration();
                    DataSet result = new DataSet();
                    
                    using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {       
                           
                            if (FIRSTROW_IS_HEADER == true)
                            {                              
                                result = reader.AsDataSet(new ExcelDataSetConfiguration()
                                {
                                    ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                                    {
                                        UseHeaderRow = true
                                    }
                                }
                                );
                            }
                            else
                            {
                                result = reader.AsDataSet();
                            }
                  
                            ErrorMessage.Add(new HISUploadErrorMessageDTO
                            {
                                hfu_status = 'I',
                                hfu_Err_type = "I",
                                hfu_Err_no = 1,
                                hfu_Err_Column = "Total",
                                hfu_Err_Message = result.Tables[0].Rows.Count.ToString()
                            });
                        }
                    }
                    #endregion

                    //----------  Validate Data in File -------------
                    var dataTable = result.Tables[0];
                    const int colRefNo = 0;
                    const int colHNNo = 1;
                    const int colLabNo = 2;
                    const int colSpecDate = 3;

                    const string COL_REF_NO = "Ref No";
                    const string COL_HN_NO = "HN";
                    const string COL_LAB_NO = "Lab";
                    const string COL_DATE = "Date";

                    // Check column Exist
                    Boolean columnExists = result.Tables[0].Columns.Contains(COL_REF_NO)
                                        && result.Tables[0].Columns.Contains(COL_HN_NO)
                                        && result.Tables[0].Columns.Contains(COL_LAB_NO)
                                        && result.Tables[0].Columns.Contains(COL_DATE);
                    if (columnExists == false)
                    {
                        ErrorMessage.Add(new HISUploadErrorMessageDTO
                        {
                            hfu_status = 'E',
                            hfu_Err_type = "C",
                            hfu_Err_no = 1,
                            hfu_Err_Column = COL_REF_NO,
                            hfu_Err_Message = "ไม่พบ Column " + COL_REF_NO
                        });
                    }

                    for (var i = 0; i < dataTable.Rows.Count; i++)
                    {
                        for (var j = 0; j < dataTable.Columns.Count; j++)
                        {
                            //HISWithSP
                            if(HISfileOwner.hfu_file_type == "HIS" && j == 4)
                            {
                                break;
                            }

                            var data = dataTable.Rows[i][j];
                                                     
                            // Check column not null
                            if (string.IsNullOrEmpty(data.ToString()))
                            {
                                string columnError = "";
                                if(j == colRefNo) { columnError = "Ref No."; }
                                else if(j == colHNNo) { columnError = "HN No.";}
                                else if (j == colLabNo) { columnError = "Lab No.";}
                                else if(j == colSpecDate) { columnError = "Date"; }

                                


                            }




                            // Check Date format incorrect
                            if (j == colSpecDate)
                            {
                                //string dateString = data.ToString();
                                //DateTime dtValue;
                                //DateTime dt;
                                //string[] formateDate = { "dd/MM/yyyy" };
                                //if (DateTime.TryParse(dateString, out dtValue))
                                //{
                                //    var dateonly = dtValue.Date;
                                //    if (!DateTime.TryParseExact(dateonly.ToString(), formateDate,
                                //                 new CultureInfo("en-US"),
                                //                 DateTimeStyles.None, out dt))
                                //    {
                                //        //your condition fail code goes here
                                //        ErrorMessage.Add(new HISUploadErrorMessageDTO
                                //        {
                                //            hfu_status = 'E',
                                //            hfu_Err_type = "E",
                                //            hfu_Err_no = 1,
                                //            hfu_Err_Column = "",
                                //            hfu_Err_Message = "Column Date ต้องอยู่ในรูปแบบ " + formateDate[0]
                                //        }); ;
                                //        return ErrorMessage;
                                //    }

                                //}
                                //else
                                //{
                                //    ErrorMessage.Add(new HISUploadErrorMessageDTO
                                //    {
                                //        hfu_status = 'E',
                                //        hfu_Err_type = "E",
                                //        hfu_Err_no = 1,
                                //        hfu_Err_Column = "",
                                //        hfu_Err_Message = "Column Date ต้องอยู่ในรูปแบบ " + formateDate[0]
                                //    }); ;
                                //    return ErrorMessage;
                                //}                                                       
                            }

                        }
                    }                 

                    var chkError = ErrorMessage.FirstOrDefault(x => x.hfu_status == 'E');
                    if (chkError != null)
                    {
                        File.Delete(path);
                    }
                    else
                    {
                        ErrorMessage.Add(new HISUploadErrorMessageDTO
                        {
                            hfu_status = 'I',
                            hfu_Err_type = "P",
                            hfu_Err_no = 1,
                            hfu_Err_Column = "path",
                            hfu_Err_Message = path
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                //create log
            }
            return ErrorMessage;
        }

        public async Task<HISUploadDataDTO> SaveFileUploadAsync(IFileListEntry fileEntry, HISUploadDataDTO model)
        {
            HISUploadDataDTO objReturn = new HISUploadDataDTO();

            if (model.hfu_id == 0)
            {              
                model.hfu_status = 'N';
                model.hfu_delete_flag = false;
            }
            else
            {
                model.hfu_status = 'E';
            }

            //model.hfu_updatedate = DateTime.Now;
            objReturn = await _apiHelper.PostDataAsync<HISUploadDataDTO>("his_api/Post_SaveSPFileUploadData", model);

            return objReturn;
        }

        public async Task<HISFileUploadSummaryDTO> SaveFileUploadSumaryAsync(List<HISFileUploadSummaryDTO> models)
        {
            HISFileUploadSummaryDTO objReturn = new HISFileUploadSummaryDTO();

            objReturn = await _apiHelper.PostListofDataAsync<HISFileUploadSummaryDTO>("his_api/Post_SaveFileUploadSummary", models);

            return objReturn;
        }
    }
}
