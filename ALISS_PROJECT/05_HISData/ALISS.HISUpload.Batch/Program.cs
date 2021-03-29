using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;
using System.Runtime.InteropServices.ComTypes;
using ExcelDataReader;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Globalization;
using ALISS.HISUpload.Batch.Models;
using ALISS.HISUpload.Batch.DataAccess;
using ALISS.HISUpload.DTO;

namespace ALISS.HISUpload.Batch
{
    class Program
    {
        public IConfiguration Configuration { get; }
        static void Main(string[] args)
        {
            var HISDataDAL = new HISDataDAL();
            var HISFileUpload = HISDataDAL.Get_NewHISFileUpload('N');
            var HISFileUploadReprocess = HISDataDAL.Get_NewHISFileUpload('R');

            if (HISFileUploadReprocess.Count > 0)
            {
                HISFileUpload.AddRange(HISFileUploadReprocess);
            }
            string Param_RefNo = "";
            string Param_HNNo = "";
            string Param_LabNo = "";
            string Param_Date = "";
            int iDupRecord = 0;
            int iMatchRecord = 0;
            var strFileType = "SP";  
            bool FIRST_ROW_IS_COLUMN = true;

            try
            {
                if (HISFileUpload != null)
                {
                    //Update status = P (Processing)
                    foreach (HISUploadDataDTO HISFile in HISFileUpload)
                    {
                        var rowsAffected = HISDataDAL.Update_HISFileUploadStatus(HISFile.hfu_id, 'P', 0, 0, "BATCH");
                    }

                    List<TCParameter> ParameterList = HISDataDAL.GetParameter();

                    if (ParameterList.Count != 0)
                    {
                        Param_RefNo = ParameterList.FirstOrDefault(x => x.prm_code_minor == "REF_NO").prm_value;
                        Param_HNNo = ParameterList.FirstOrDefault(x => x.prm_code_minor == "HN").prm_value;
                        Param_LabNo = ParameterList.FirstOrDefault(x => x.prm_code_minor == "LAB_NO").prm_value;
                        Param_Date = ParameterList.FirstOrDefault(x => x.prm_code_minor == "DATE").prm_value;
                    }

                    foreach (HISUploadDataDTO HISFile in HISFileUpload)
                    {
                        iMatchRecord = 0;
                        iDupRecord = 0;
                        var iFileUploadID = HISFile.hfu_id;
                        var strFileName = HISFile.hfu_file_name;
                        var strFilePath = HISFile.hfu_file_path;

                        if (Path.GetExtension(strFileName) == ".xls" || Path.GetExtension(strFileName) == ".xlsx")
                        {
                          
                            List<TRSTGHISFileUploadDetail> HISFileDataDetailList = new List<TRSTGHISFileUploadDetail>();

                            using (var stream = File.Open(strFilePath, FileMode.Open, FileAccess.Read))
                            {
                                DataSet result = new DataSet();

                                if (Path.GetExtension(strFileName) == ".xls" || Path.GetExtension(strFileName) == ".xlsx")
                                {
                                    ExcelDataSetConfiguration option = new ExcelDataSetConfiguration();
                                    //var reader = ExcelReaderFactory.CreateReader(stream);
                                    var reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                                    if (FIRST_ROW_IS_COLUMN == true)
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
                                }

                                var iRow = 0;


                                string[] columnNames = (from dc in result.Tables[0].Columns.Cast<DataColumn>()
                                                        select dc.ColumnName).ToArray();

                                foreach (DataRow drRow in result.Tables[0].Rows)
                                {
                                  
                                    var strRefNo = drRow[Param_RefNo].ToString();
                                    var strHNNo = drRow[Param_HNNo].ToString();
                                    var strLabNo = drRow[Param_LabNo].ToString();
                                    DateTime? dtDate = null;
                                    var strDate = drRow[Param_Date].ToString();
                                    if (!string.IsNullOrEmpty(strDate))
                                    {
                                        dtDate = (DateTime)drRow[Param_Date];
                                    }
                                   
                                    #region InsertHISFileDataHeader
                                    TRSTGHISFileUploadHeader objHeader = new TRSTGHISFileUploadHeader();

                                    objHeader.huh_hfu_id = iFileUploadID;
                                    objHeader.huh_template_id = HISFile.hfu_template_id;
                                    objHeader.huh_status = 'N';
                                    objHeader.huh_delete_flag = false;
                                    objHeader.huh_hos_code = HISFile.hfu_hos_code;
                                    
                                    objHeader.huh_ref_no = strRefNo;
                                    objHeader.huh_hn_no = strHNNo;
                                    objHeader.huh_lab_no = strLabNo;
                                    objHeader.huh_date = dtDate;
                                    objHeader.huh_createuser = "BATCH";

                                    int seq = HISDataDAL.Get_CheckExistingHeader(objHeader, strFileType); 
                                    objHeader.huh_seq_no = seq;
                                    objHeader.huh_createdate = DateTime.Now;
                                    var huh_id = HISDataDAL.Save_HISFileDataHeader(objHeader);


                                    //    else if (FieldDateType == "System.DateTime")
                                    //    {
                                    //        objModel.ldh_cdate = (DateTime)row[DATE];
                                    //        int year = objModel.ldh_cdate.Value.Year;
                                    //        if (year > DateTime.Now.Year)
                                    //        {
                                    //            objModel.ldh_cdate = objModel.ldh_cdate.Value.AddYears(-543);
                                    //        }
                                    //    }

                                    #endregion

                                    #region TRHISFileUploadSummary

                                    //////string strErrColumn = "";
                                    //////string strErrMessage = "";
                                    //////int iErrorRow = 0;
                                    //////bool blnError = false;

                                    //////if (string.IsNullOrEmpty(strRefNo))
                                    //////{
                                    //////    strErrColumn = Param_RefNo;
                                    //////    strErrMessage = "Field is required";
                                    //////    iErrorRow = iRow +1 ;
                                    //////    blnError = true;
                                    //////}

                                    //////if (string.IsNullOrEmpty(strHNNo))
                                    //////{
                                    //////    strErrColumn = Param_HNNo;
                                    //////    strErrMessage = "Field is required";
                                    //////    iErrorRow = iRow + 1;
                                    //////    blnError = true;
                                    //////}

                                    //////if (string.IsNullOrEmpty(strLabNo))
                                    //////{
                                    //////    strErrColumn = Param_LabNo;
                                    //////    strErrMessage = "Field is required";
                                    //////    iErrorRow = iRow + 1;
                                    //////    blnError = true;
                                    //////}

                                    //////if (!dtDate.HasValue)
                                    //////{
                                    //////    strErrColumn = Param_Date;
                                    //////    strErrMessage = "Field is required";
                                    //////    iErrorRow = iRow + 1;
                                    //////    blnError = true;
                                    //////}

                                    //////if (blnError)
                                    //////{
                                    //////    TRHISFileUploadSummary objSummary = new TRHISFileUploadSummary();

                                    //////    objSummary.hus_hfu_id = iFileUploadID;
                                    //////    objSummary.hus_error_fieldname = strErrColumn;
                                    //////    objSummary.hus_error_fielddescr = strErrMessage;
                                    //////    objSummary.hus_error_fieldrecord = iErrorRow.ToString();
                                    //////    objSummary.hus_delete_flag = false;
                                    //////    objSummary.hus_createuser = "BATCH";
                                    //////    objSummary.hus_createdate = DateTime.Now;

                                    //////    var complete = HISDataDAL.Save_HISFileUploadSummary(objSummary);
                                    //////    if (!complete)
                                    //////    {
                                    //////        Console.WriteLine("Error Save HISFileUploadSummary");
                                    //////    }
                                    //////}
                                    #endregion

                                    #region InsertHISFileDataDetail

                                    if (HISFile.hfu_file_type == "HIS")
                                    {
                                        for(int c = 4; c <= (columnNames.Length-1);c++)
                                        {
                                            string value = "";
                                            if (!drRow[columnNames[c].ToString()].Equals(null))
                                            {
                                                value = drRow[columnNames[c].ToString()].ToString();
                                            }

                                            HISFileDataDetailList.Add(new TRSTGHISFileUploadDetail
                                            {

                                                hud_status = 'N',
                                                hud_huh_id = huh_id,
                                                hud_field_name = columnNames[c].ToString(),
                                                hud_field_value = value,
                                                hud_createuser = "BATCH",
                                                hud_createdate = DateTime.Now
                                            });
                                        }
                                        

                                    }

                                  
                                    //// Save แยก header/detail ไม่ได้
                                    //var ldd = HISDataDAL.Save_HISFileDataDetail(HISFileDataDetailList);

                                    #endregion
                                    iRow++;
                                }

                                if(HISFile.hfu_file_type == "HIS" && HISFileDataDetailList.Count!=0)
                                {
                                    var ldd = HISDataDAL.Save_HISFileDataDetail(HISFileDataDetailList);
                                }
                            }
                        } 
                        else
                        {
                            // support only .xls .xlsx file
                        }


                        //char status;
                        //if (intDupRecord == 0)
                        //{
                        //    status = 'D';
                        //}
                        //else
                        //{
                        //    status = 'E';
                        //}

                        //Update status = F (Finish)
                        var rowsAffected2 = HISDataDAL.Update_HISFileUploadStatus(HISFile.hfu_id, 'F', iMatchRecord, iDupRecord, "BATCH");
                        }
                    }
                Console.WriteLine("Batch Complete");
            }

            catch (Exception ex)
            {
                //var rowsAffected = HISDataDAL.Update_HISFileUploadStatus(HISFile.hfu_id, 'N', 0, 0, "BATCH");
                var d = ex.Message;       
                Console.WriteLine(d);
            }
        }

    }
}
