using DbfDataReader;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;
using ALISS.LabFileUpload.DTO;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ALISS.LabFileUpload.Batch.DataAccess;
using AutoMapper;
using System.Runtime.InteropServices.ComTypes;
using ALISS.Mapping.DTO;
using ExcelDataReader;
using System.Collections.Generic;
using ALISS.LabFileUpload.Batch.Models;
using System.Net.Sockets;
using System.Globalization;

namespace ALISS.LabFileUpload.Batch
{
    public class Program
    {

        public IConfiguration Configuration { get; }

        static void Main(string[] args)
        {
            var LabDataDAL = new LabDataDAL();
            var LabFileUpload = LabDataDAL.Get_NewLabFileUpload('N');
            var LabFileUploadReprocess = LabDataDAL.Get_NewLabFileUpload('R');

            if (LabFileUploadReprocess.Count > 0)
            {
                LabFileUpload.AddRange(LabFileUploadReprocess);
            }
            string Param_labno = "";
            string Param_organism = "";
            string Param_specimen = "";
            string Param_date = "";
            int chkError = 0;
            int chkDetailError = 0;

            try
            {
                if (LabFileUpload != null)
                {
                    //Update status = P (Processing)
                    foreach (LabFileUploadDataDTO LabFile in LabFileUpload)
                    {
                        var rowsAffected = LabDataDAL.Update_LabFileUploadStatus(LabFile.lfu_id.ToString(), 'P', 0, "BATCH");
                    }


                    List<TCParameter> ParameterList = LabDataDAL.GetParameter();

                    if (ParameterList.Count != 0)
                    {
                        Param_labno = ParameterList.FirstOrDefault(x => x.prm_code_minor == "LAB_NO").prm_value;
                        Param_organism = ParameterList.FirstOrDefault(x => x.prm_code_minor == "ORGANISM").prm_value;
                        Param_specimen = ParameterList.FirstOrDefault(x => x.prm_code_minor == "SPECIMEN").prm_value;
                        Param_date = ParameterList.FirstOrDefault(x => x.prm_code_minor == "DATE").prm_value;
                    }

                    foreach (LabFileUploadDataDTO LabFile in LabFileUpload)
                    {
                        chkError = 0;
                        chkDetailError = 0;
                        var MappingTemplate = LabDataDAL.GetMappingData(LabFile.lfu_mp_id.ToString());
                        var whonetmapping = LabDataDAL.GetWHONetMappingList(LabFile.lfu_mp_id.ToString(), MappingTemplate.mp_mst_code);

                        if (Path.GetExtension(LabFile.lfu_FileName) == ".xls" || Path.GetExtension(LabFile.lfu_FileName) == ".xlsx" || Path.GetExtension(LabFile.lfu_FileName) == ".csv" || Path.GetExtension(LabFile.lfu_FileName) == ".txt")
                        {
                            Guid feh_id_cdate = Guid.Empty;
                            char feh_status_cdate = 'N';
                            int cDateError = 0;
                            string FieldDateType = "", DateFormat = "";
                            string LAB_NO = whonetmapping.FirstOrDefault(x => x.wnm_whonetfield == Param_labno).wnm_originalfield;
                            string ORGANISM = whonetmapping.FirstOrDefault(x => x.wnm_whonetfield == Param_organism).wnm_originalfield;
                            string DATE = whonetmapping.FirstOrDefault(x => x.wnm_whonetfield == Param_date).wnm_originalfield;
                            DateFormat = whonetmapping.FirstOrDefault(x => x.wnm_whonetfield == Param_date).wnm_fieldformat;


                            string SOURCE = "";
                            if (whonetmapping.FirstOrDefault(x => x.wnm_whonetfield == Param_specimen) != null)
                                SOURCE = whonetmapping.FirstOrDefault(x => x.wnm_whonetfield == Param_specimen).wnm_originalfield;

                            if (MappingTemplate.mp_firstlineisheader == false)
                            {
                                LAB_NO = convertColumn(LAB_NO);
                                ORGANISM = convertColumn(ORGANISM);

                                if (SOURCE != "")
                                    SOURCE = convertColumn(SOURCE);

                                DATE = convertColumn(DATE);
                            }

                            List<TRSTGLabFileDataDetail> LabFileDataDetailList = new List<TRSTGLabFileDataDetail>();

                            using (var stream = File.Open(LabFile.lfu_Path, FileMode.Open, FileAccess.Read))
                            {
                                DataSet result = new DataSet();

                                if (Path.GetExtension(LabFile.lfu_FileName) == ".xls" || Path.GetExtension(LabFile.lfu_FileName) == ".xlsx")
                                {
                                    ExcelDataSetConfiguration option = new ExcelDataSetConfiguration();
                                    var reader = ExcelReaderFactory.CreateReader(stream);
                                    if (MappingTemplate.mp_firstlineisheader == true)
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
                                else if (Path.GetExtension(LabFile.lfu_FileName) == ".csv")
                                {
                                    var reader = ExcelReaderFactory.CreateCsvReader(stream, new ExcelReaderConfiguration()
                                    {
                                        FallbackEncoding = Encoding.GetEncoding(1252),
                                        AutodetectSeparators = new char[] { ',', ';', '\t', '|', '#' },
                                        LeaveOpen = false,
                                        AnalyzeInitialCsvRows = 0,
                                    });

                                    if (MappingTemplate.mp_firstlineisheader == true)
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
                                else if (Path.GetExtension(LabFile.lfu_FileName) == ".txt")
                                {
                                    string line;
                                    DataTable dt = new DataTable();

                                    using (TextReader tr = File.OpenText(LabFile.lfu_Path))
                                    {
                                        while ((line = tr.ReadLine()) != null)
                                        {
                                            string[] items = line.Split('\t');
                                            if (dt.Columns.Count == 0)
                                            {
                                                if (MappingTemplate.mp_firstlineisheader == false)
                                                {
                                                    for (int i = 0; i < items.Length; i++)
                                                        dt.Columns.Add(new DataColumn("Column" + i, typeof(string)));
                                                }
                                                else
                                                {
                                                    for (int i = 0; i < items.Length; i++)
                                                        dt.Columns.Add(new DataColumn(items[i].ToString(), typeof(string)));
                                                }
                                            }
                                            dt.Rows.Add(items);
                                        }
                                    }

                                    result.Tables.Add(dt);


                                }

                                if (result.Tables[0].Columns.Contains(DATE) == true)
                                {
                                    FieldDateType = result.Tables[0].Columns[DATE].DataType.ToString();
                                }


                                if (MappingTemplate.mp_AntibioticIsolateOneRec == false)
                                {
                                    var MappingAnt = whonetmapping.Where(x => x.wnm_antibiotic != null);
                                    string Antibioticcolumn = MappingAnt.FirstOrDefault().wnm_antibioticcolumn;

                                    if (MappingTemplate.mp_firstlineisheader == false)
                                    {

                                        Antibioticcolumn = convertColumn(Antibioticcolumn);
                                    }

                                    var listexcludeAntibiotic = new List<string>();

                                    foreach (WHONetMappingListsDTO t in MappingAnt)
                                    {
                                        listexcludeAntibiotic.Add(t.wnm_antibiotic);
                                    }



                                    var AntNotMatch = result.Tables[0].AsEnumerable().Where(row => !listexcludeAntibiotic.Contains(row.Field<string>(Antibioticcolumn)));
                                    int ErrorAnt = AntNotMatch.Count();
                                    var AntNotMatchDistinct = AntNotMatch.AsEnumerable().Select(s => new { antibiotic = s.Field<string>(Antibioticcolumn), }).Distinct().ToList();   //e

                                    if (ErrorAnt > 0)
                                    {
                                        chkError = ErrorAnt;
                                        TRLabFileErrorHeader objErrorH = new TRLabFileErrorHeader();
                                        objErrorH.feh_id = Guid.NewGuid();
                                        objErrorH.feh_status = 'N';
                                        objErrorH.feh_flagdelete = false;
                                        objErrorH.feh_type = "NOT_MATCH";
                                        objErrorH.feh_field = "Antibiotic";
                                        objErrorH.feh_message = "Antibiotic not match";
                                        objErrorH.feh_errorrecord = ErrorAnt;
                                        objErrorH.feh_createuser = "BATCH";
                                        objErrorH.feh_createdate = DateTime.Now;
                                        objErrorH.feh_lfu_id = LabFile.lfu_id;
                                        var objTRLabFileErrorHeader = LabDataDAL.Save_TRLabFileErrorHeader(objErrorH);

                                        foreach (var item in AntNotMatchDistinct)
                                        {
                                            TRLabFileErrorDetail objErrorD = new TRLabFileErrorDetail();
                                            objErrorD.fed_id = Guid.NewGuid();
                                            objErrorD.fed_status = 'N';
                                            objErrorD.fed_localvalue = item.antibiotic;
                                            objErrorD.fed_feh_id = objErrorH.feh_id;
                                            objErrorD.fed_createuser = "BATCH";
                                            objErrorD.fed_createdate = DateTime.Now;

                                            var objTRLabFileErrorDetail = LabDataDAL.Save_TRLabFileErrorDetail(objErrorD);
                                        }

                                        goto Finish;
                                    }
                                }



                                foreach (DataRow row in result.Tables[0].Rows)
                                {
                                    string cmethod = "";
                                    if (result.Tables[0].Columns.Contains("CMETHOD"))
                                    {
                                        cmethod = row["CMETHOD"].ToString();
                                    }

                                    if ((cmethod == "Aerobic Culture" && LabFile.lfu_Program == "MLAB")
                                   || LabFile.lfu_Program != "MLAB"
                                   || (LabFile.lfu_Program == "MLAB" && MappingTemplate.mp_filetype == "ETEST"))
                                    {

                                        #region InsertLabFileDataHeader
                                        TRSTGLabFileDataHeader objModel = new TRSTGLabFileDataHeader();
                                        Guid ldh_id = Guid.NewGuid();

                                        objModel.ldh_id = ldh_id;
                                        objModel.ldh_status = 'N';
                                        objModel.ldh_flagdelete = false;
                                        objModel.ldh_hos_code = LabFile.lfu_hos_code;
                                        objModel.ldh_lab = LabFile.lfu_lab;
                                        objModel.ldh_lfu_id = LabFile.lfu_id;


                                        objModel.ldh_labno = row[LAB_NO].ToString();
                                        objModel.ldh_organism = row[ORGANISM].ToString();

                                        if (SOURCE != "")
                                            objModel.ldh_specimen = row[SOURCE].ToString();

                                        objModel.ldh_date = row[DATE].ToString();

                                        if (FieldDateType == "System.String")
                                        {
                                            try
                                            {
                                                objModel.ldh_cdate = DateTime.ParseExact(objModel.ldh_date, DateFormat, CultureInfo.InvariantCulture);
                                                int year = objModel.ldh_cdate.Value.Year;
                                                if (year > DateTime.Now.Year)
                                                {
                                                    objModel.ldh_cdate = objModel.ldh_cdate.Value.AddYears(-543);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                chkError++;

                                                if (feh_id_cdate == Guid.Empty)
                                                {
                                                    feh_id_cdate = Guid.NewGuid();
                                                }
                                                else
                                                {
                                                    feh_status_cdate = 'E';
                                                }
                                                TRLabFileErrorHeader objErrorH = new TRLabFileErrorHeader();
                                                objErrorH.feh_id = feh_id_cdate;
                                                objErrorH.feh_status = feh_status_cdate;
                                                objErrorH.feh_flagdelete = false;
                                                objErrorH.feh_type = "CONVERT_ERROR";
                                                objErrorH.feh_field = DATE;
                                                objErrorH.feh_message = "Cannot convert date.";
                                                cDateError = cDateError + 1;
                                                objErrorH.feh_errorrecord = cDateError;
                                                objErrorH.feh_createuser = "BATCH";
                                                objErrorH.feh_createdate = DateTime.Now;
                                                objErrorH.feh_lfu_id = LabFile.lfu_id;

                                                var objTRLabFileErrorHeader = LabDataDAL.Save_TRLabFileErrorHeader(objErrorH);

                                                TRLabFileErrorDetail objErrorD = new TRLabFileErrorDetail();
                                                objErrorD.fed_id = Guid.NewGuid();
                                                objErrorD.fed_status = 'N';
                                                objErrorD.fed_localvalue = ex.Message;
                                                objErrorD.fed_feh_id = feh_id_cdate;
                                                objErrorD.fed_createuser = "BATCH";
                                                objErrorD.fed_createdate = DateTime.Now;

                                                var objTRLabFileErrorDetail = LabDataDAL.Save_TRLabFileErrorDetail(objErrorD);
                                                continue;
                                            }
                                        }
                                        else if (FieldDateType == "System.DateTime")
                                        {
                                            objModel.ldh_cdate = (DateTime)row[DATE];
                                            int year = objModel.ldh_cdate.Value.Year;
                                            if (year > DateTime.Now.Year)
                                            {
                                                objModel.ldh_cdate = objModel.ldh_cdate.Value.AddYears(-543);
                                            }
                                        }

                                        if (LabFile.lfu_Program == "MLAB" && MappingTemplate.mp_filetype == "ETEST" && objModel.ldh_cdate.Value.Year != LabFile.lfu_DataYear)
                                        {
                                            continue;
                                        }

                                        objModel.ldh_createuser = "BATCH";


                                        int seq = LabDataDAL.Get_CheckExistingHeader(objModel, MappingTemplate.mp_program, MappingTemplate.mp_filetype);
                                        objModel.ldh_sequence = seq;
                                        objModel.ldh_createdate = DateTime.Now;
                                        Guid idh_id_related = Guid.Empty;


                                        bool isNotFirstline = false;
                                        if (MappingTemplate.mp_AntibioticIsolateOneRec == false)
                                        {
                                            idh_id_related = LabDataDAL.Get_HeaderIdForMultipleline(objModel);
                                            if (idh_id_related != Guid.Empty)
                                            {
                                                isNotFirstline = true;
                                                ldh_id = idh_id_related;
                                            }
                                            else
                                            {                                                
                                                var x = LabDataDAL.Save_LabFileDataHeader(objModel);
                                            }
                                        }
                                        else
                                        {
                                            var x = LabDataDAL.Save_LabFileDataHeader(objModel);
                                        }


                                        #endregion

                                        #region InsertLabFileDataDetail


                                        if (whonetmapping != null)
                                        {
                                            foreach (WHONetMappingListsDTO item in whonetmapping)
                                            {


                                                String wnm_originalfield;
                                                var Encoding = "";

                                                if (MappingTemplate.mp_firstlineisheader == false)
                                                {

                                                    wnm_originalfield = convertColumn(item.wnm_originalfield);
                                                }
                                                else
                                                {
                                                    wnm_originalfield = item.wnm_originalfield;
                                                }

                                                if (isNotFirstline == true && MappingTemplate.mp_filetype == "ETEST" && (wnm_originalfield == LAB_NO || wnm_originalfield == DATE || wnm_originalfield == ORGANISM))
                                                {
                                                    continue;
                                                }

                                                if (!result.Tables[0].Columns.Contains(wnm_originalfield))
                                                {
                                                    continue;
                                                }

                                                if (MappingTemplate.mp_AntibioticIsolateOneRec == false)
                                                {
                                                    if (item.wnm_antibioticcolumn != null)
                                                    {
                                                        var antibioticcolumn = "";


                                                        if (MappingTemplate.mp_firstlineisheader == false)
                                                        {
                                                            antibioticcolumn = convertColumn(item.wnm_antibioticcolumn);
                                                        }
                                                        else
                                                        {
                                                            antibioticcolumn = item.wnm_antibioticcolumn;
                                                        }

                                                        if (row.IsNull(antibioticcolumn))
                                                        {
                                                            continue;
                                                        }

                                                        if (row[antibioticcolumn].ToString() != item.wnm_antibiotic)
                                                        {
                                                            continue;
                                                        }
                                                    }
                                                }



                                                if (row.IsNull(wnm_originalfield))
                                                {
                                                    continue;
                                                }

                                                string tempvalue = "";
                                                var xx = row[wnm_originalfield].GetType().ToString();
                                                if (row[wnm_originalfield].GetType().ToString() != "System.DateTime" && item.wnm_type == "Date")
                                                {
                                                    try
                                                    {
                                                        var tempfielddate = DateTime.ParseExact(row[wnm_originalfield].ToString(), item.wnm_fieldformat, CultureInfo.InvariantCulture);
                                                        tempvalue = tempfielddate.ToString();
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        chkDetailError++;

                                                        if (feh_id_cdate == Guid.Empty)
                                                        {
                                                            feh_id_cdate = Guid.NewGuid();
                                                        }
                                                        else
                                                        {
                                                            feh_status_cdate = 'E';
                                                        }
                                                        TRLabFileErrorHeader objErrorH = new TRLabFileErrorHeader();
                                                        objErrorH.feh_id = feh_id_cdate;
                                                        objErrorH.feh_status = feh_status_cdate;
                                                        objErrorH.feh_flagdelete = false;
                                                        objErrorH.feh_type = "CONVERT_ERROR";
                                                        objErrorH.feh_field = item.wnm_originalfield;
                                                        objErrorH.feh_message = "Cannot convert date.";
                                                        cDateError = cDateError + 1;
                                                        objErrorH.feh_errorrecord = cDateError;
                                                        objErrorH.feh_createuser = "BATCH";
                                                        objErrorH.feh_createdate = DateTime.Now;
                                                        objErrorH.feh_lfu_id = LabFile.lfu_id;

                                                        var objTRLabFileErrorHeader = LabDataDAL.Save_TRLabFileErrorHeader(objErrorH);

                                                        TRLabFileErrorDetail objErrorD = new TRLabFileErrorDetail();
                                                        objErrorD.fed_id = Guid.NewGuid();
                                                        objErrorD.fed_status = 'N';
                                                        objErrorD.fed_localvalue = ex.Message;
                                                        objErrorD.fed_feh_id = feh_id_cdate;
                                                        objErrorD.fed_createuser = "BATCH";
                                                        objErrorD.fed_createdate = DateTime.Now;

                                                        var objTRLabFileErrorDetail = LabDataDAL.Save_TRLabFileErrorDetail(objErrorD);
                                                        continue;
                                                    }
                                                }
                                                else if (row[wnm_originalfield].GetType().ToString() == "System.DateTime" && item.wnm_type == "Date")
                                                {
                                                    var tempfielddate = (DateTime)row[wnm_originalfield];
                                                    tempvalue = tempfielddate.ToString();
                                                }
                                                else
                                                {
                                                    tempvalue = row[wnm_originalfield].ToString();
                                                }

                                                if (item.wnm_encrypt == true)
                                                {
                                                    Encoding = CryptoHelper.UnicodeEncoding(tempvalue);
                                                }
                                                else
                                                {
                                                    Encoding = tempvalue;
                                                }

                                                LabFileDataDetailList.Add(new TRSTGLabFileDataDetail
                                                {
                                                    ldd_id = Guid.NewGuid(),
                                                    ldd_status = 'N',
                                                    ldd_whonetfield = item.wnm_whonetfield,
                                                    ldd_originalfield = item.wnm_originalfield,
                                                    ldd_originalvalue = Encoding,
                                                    ldd_ldh_id = ldh_id,
                                                    ldd_createuser = "BATCH",
                                                    ldd_createdate = DateTime.Now
                                                }
                                                );

                                            }

                                        }
                                        #endregion
                                    }
                                }




                                if (LabFileDataDetailList.Count != 0)
                                {
                                    var ldd = LabDataDAL.Save_LabFileDataDetail(LabFileDataDetailList);
                                }
                            }

                        }
                        #region comment
                        //else if (Path.GetExtension(LabFile.lfu_FileName) == ".csv")
                        //{
                        //    Guid feh_id_cdate = Guid.Empty;
                        //    char feh_status_cdate = 'N';
                        //    int cDateError = 0;
                        //    string FieldDateType = "", DateFormat = "";

                        //    string LAB_NO = whonetmapping.FirstOrDefault(x => x.wnm_whonetfield == Param_labno).wnm_originalfield;
                        //    string ORGANISM = whonetmapping.FirstOrDefault(x => x.wnm_whonetfield == Param_organism).wnm_originalfield;
                        //    //string SOURCE = whonetmapping.FirstOrDefault(x => x.wnm_whonetfield == Param_specimen).wnm_originalfield;
                        //    string DATE = whonetmapping.FirstOrDefault(x => x.wnm_whonetfield == Param_date).wnm_originalfield;
                        //    DateFormat = whonetmapping.FirstOrDefault(x => x.wnm_whonetfield == Param_date).wnm_fieldformat;


                        //    string SOURCE = "";
                        //    if (whonetmapping.FirstOrDefault(x => x.wnm_whonetfield == Param_specimen) != null)
                        //        SOURCE = whonetmapping.FirstOrDefault(x => x.wnm_whonetfield == Param_specimen).wnm_originalfield;

                        //    if (MappingTemplate.mp_firstlineisheader == false)
                        //    {
                        //        LAB_NO = convertColumn(LAB_NO);
                        //        ORGANISM = convertColumn(ORGANISM);
                        //        if (SOURCE != "")
                        //            SOURCE = convertColumn(SOURCE);
                        //        DATE = convertColumn(DATE);
                        //    }

                        //    List<TRSTGLabFileDataDetail> LabFileDataDetailList = new List<TRSTGLabFileDataDetail>();

                        //    using (var stream = File.Open(LabFile.lfu_Path, FileMode.Open, FileAccess.Read))
                        //    {

                        //        var reader = ExcelReaderFactory.CreateCsvReader(stream, new ExcelReaderConfiguration()
                        //        {
                        //            FallbackEncoding = Encoding.GetEncoding(1252),
                        //            AutodetectSeparators = new char[] { ',', ';', '\t', '|', '#' },
                        //            LeaveOpen = false,
                        //            AnalyzeInitialCsvRows = 0,
                        //        });

                        //        DataSet result = new DataSet();

                        //        if (MappingTemplate.mp_firstlineisheader == true)
                        //        {
                        //            result = reader.AsDataSet(new ExcelDataSetConfiguration()
                        //            {
                        //                ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                        //                {
                        //                    UseHeaderRow = true
                        //                }
                        //            }
                        //            );
                        //        }
                        //        else
                        //        {
                        //            result = reader.AsDataSet();
                        //        }

                        //        if (result.Tables[0].Columns.Contains(DATE) == true)
                        //        {
                        //            FieldDateType = result.Tables[0].Columns[DATE].DataType.ToString();
                        //        }


                        //        foreach (DataRow row in result.Tables[0].Rows)
                        //        {
                        //            #region InsertLabFileDataHeader
                        //            TRSTGLabFileDataHeader objModel = new TRSTGLabFileDataHeader();
                        //            Guid ldh_id = Guid.NewGuid();

                        //            objModel.ldh_id = ldh_id;
                        //            objModel.ldh_status = 'N';
                        //            objModel.ldh_flagdelete = false;
                        //            objModel.ldh_hos_code = LabFile.lfu_hos_code;
                        //            objModel.ldh_lab = LabFile.lfu_lab;
                        //            objModel.ldh_lfu_id = LabFile.lfu_id;


                        //            objModel.ldh_labno = row[LAB_NO].ToString();
                        //            objModel.ldh_organism = row[ORGANISM].ToString();
                        //            if (SOURCE != "")
                        //                objModel.ldh_specimen = row[SOURCE].ToString();
                        //            objModel.ldh_date = row[DATE].ToString();

                        //            if (FieldDateType == "System.String")
                        //            {
                        //                try
                        //                {
                        //                    objModel.ldh_cdate = DateTime.ParseExact(objModel.ldh_date, DateFormat, CultureInfo.InvariantCulture);
                        //                }
                        //                catch (Exception ex)
                        //                {
                        //                    chkError++;

                        //                    if (feh_id_cdate == Guid.Empty)
                        //                    {
                        //                        feh_id_cdate = Guid.NewGuid();
                        //                    }
                        //                    else
                        //                    {
                        //                        feh_status_cdate = 'E';
                        //                    }
                        //                    TRLabFileErrorHeader objErrorH = new TRLabFileErrorHeader();
                        //                    objErrorH.feh_id = feh_id_cdate;
                        //                    objErrorH.feh_status = feh_status_cdate;
                        //                    objErrorH.feh_flagdelete = false;
                        //                    objErrorH.feh_type = "CONVERT_ERROR";
                        //                    objErrorH.feh_field = DATE;
                        //                    objErrorH.feh_message = "Cannot convert date.";
                        //                    cDateError = cDateError + 1;
                        //                    objErrorH.feh_errorrecord = cDateError;
                        //                    objErrorH.feh_createuser = "BATCH";
                        //                    objErrorH.feh_createdate = DateTime.Now;
                        //                    objErrorH.feh_lfu_id = LabFile.lfu_id;

                        //                    var objTRLabFileErrorHeader = LabDataDAL.Save_TRLabFileErrorHeader(objErrorH);

                        //                    TRLabFileErrorDetail objErrorD = new TRLabFileErrorDetail();
                        //                    objErrorD.fed_id = Guid.NewGuid();
                        //                    objErrorD.fed_status = 'N';
                        //                    objErrorD.fed_localvalue = ex.Message;
                        //                    objErrorD.fed_feh_id = feh_id_cdate;
                        //                    objErrorD.fed_createuser = "BATCH";
                        //                    objErrorD.fed_createdate = DateTime.Now;

                        //                    var objTRLabFileErrorDetail = LabDataDAL.Save_TRLabFileErrorDetail(objErrorD);
                        //                    continue;
                        //                }
                        //            }
                        //            else if (FieldDateType == "System.DateTime")
                        //            {
                        //                objModel.ldh_cdate = (DateTime)row[DATE];
                        //            }

                        //            objModel.ldh_createuser = "BATCH";


                        //            int seq = LabDataDAL.Get_CheckExistingHeader(objModel, MappingTemplate.mp_program, MappingTemplate.mp_filetype);
                        //            objModel.ldh_sequence = seq;
                        //            objModel.ldh_createdate = DateTime.Now;
                        //            Guid idh_id_related = Guid.Empty;



                        //            if (MappingTemplate.mp_AntibioticIsolateOneRec == false)
                        //            {
                        //                idh_id_related = LabDataDAL.Get_HeaderIdForMultipleline(objModel);
                        //                if (idh_id_related != Guid.Empty)
                        //                {
                        //                    ldh_id = idh_id_related;
                        //                }
                        //                else
                        //                {
                        //                    var x = LabDataDAL.Save_LabFileDataHeader(objModel);
                        //                }
                        //            }
                        //            else
                        //            {
                        //                var x = LabDataDAL.Save_LabFileDataHeader(objModel);
                        //            }


                        //            #endregion

                        //            #region InsertLabFileDataDetail


                        //            if (whonetmapping != null)
                        //            {
                        //                foreach (WHONetMappingListsDTO item in whonetmapping)
                        //                {
                        //                    String wnm_originalfield;
                        //                    var Encoding = "";

                        //                    if (MappingTemplate.mp_firstlineisheader == false)
                        //                    {

                        //                        wnm_originalfield = convertColumn(item.wnm_originalfield);
                        //                    }
                        //                    else
                        //                    {
                        //                        wnm_originalfield = item.wnm_originalfield;
                        //                    }

                        //                    if (MappingTemplate.mp_AntibioticIsolateOneRec == false)
                        //                    {
                        //                        if (item.wnm_antibioticcolumn != null)
                        //                        {
                        //                            var antibioticcolumn = "";
                        //                            antibioticcolumn = convertColumn(item.wnm_antibioticcolumn);

                        //                            if (row.IsNull(antibioticcolumn))
                        //                            {
                        //                                continue;
                        //                            }

                        //                            if (row[antibioticcolumn].ToString() != item.wnm_antibiotic)
                        //                            {
                        //                                continue;
                        //                            }
                        //                        }
                        //                    }

                        //                    if (row.IsNull(wnm_originalfield))
                        //                    {
                        //                        continue;
                        //                    }


                        //                    if (item.wnm_encrypt == true)
                        //                    {
                        //                        Encoding = CryptoHelper.UnicodeEncoding(row[wnm_originalfield].ToString());
                        //                    }
                        //                    else
                        //                    {
                        //                        Encoding = row[wnm_originalfield].ToString();
                        //                    }

                        //                    LabFileDataDetailList.Add(new TRSTGLabFileDataDetail
                        //                    {
                        //                        ldd_id = Guid.NewGuid(),
                        //                        ldd_status = 'N',
                        //                        ldd_whonetfield = item.wnm_whonetfield,
                        //                        ldd_originalfield = item.wnm_originalfield,
                        //                        ldd_originalvalue = Encoding,
                        //                        ldd_ldh_id = ldh_id,
                        //                        ldd_createuser = "BATCH",
                        //                        ldd_createdate = DateTime.Now
                        //                    }
                        //                    );

                        //                }                                    

                        //            }

                        //            #endregion

                        //        }


                        //    }

                        //    if (LabFileDataDetailList.Count != 0)
                        //    {
                        //        var ldd = LabDataDAL.Save_LabFileDataDetail(LabFileDataDetailList);
                        //    }

                        //}
                        //else if (Path.GetExtension(LabFile.lfu_FileName) == ".txt")
                        //{
                        //    string line;
                        //    DataTable dt = new DataTable();

                        //    string LAB_NO = whonetmapping.FirstOrDefault(x => x.wnm_whonetfield == Param_labno).wnm_originalfield;
                        //    string ORGANISM = whonetmapping.FirstOrDefault(x => x.wnm_whonetfield == Param_organism).wnm_originalfield;
                        //    string SOURCE = whonetmapping.FirstOrDefault(x => x.wnm_whonetfield == Param_specimen).wnm_originalfield;
                        //    string DATE = whonetmapping.FirstOrDefault(x => x.wnm_whonetfield == Param_date).wnm_originalfield;


                        //    if (MappingTemplate.mp_firstlineisheader == false)
                        //    {
                        //        LAB_NO = convertColumn(LAB_NO);
                        //        ORGANISM = convertColumn(ORGANISM);
                        //        SOURCE = convertColumn(SOURCE);
                        //        DATE = convertColumn(DATE);
                        //    }

                        //    List<TRSTGLabFileDataDetail> LabFileDataDetailList = new List<TRSTGLabFileDataDetail>();

                        //    using (TextReader tr = File.OpenText(LabFile.lfu_Path))
                        //    {
                        //        while ((line = tr.ReadLine()) != null)
                        //        {
                        //            string[] items = line.Split('\t');
                        //            if (dt.Columns.Count == 0)
                        //            {
                        //                if (MappingTemplate.mp_firstlineisheader == false)
                        //                {
                        //                    for (int i = 0; i < items.Length; i++)
                        //                        dt.Columns.Add(new DataColumn("Column" + i, typeof(string)));
                        //                }
                        //                else
                        //                {
                        //                    for (int i = 0; i < items.Length; i++)
                        //                        dt.Columns.Add(new DataColumn(items[i].ToString(), typeof(string)));
                        //                }
                        //            }
                        //            dt.Rows.Add(items);
                        //        }

                        //        foreach (DataRow dr in dt.Rows)
                        //        {
                        //            #region InsertLabFileDataHeader
                        //            TRSTGLabFileDataHeader objModel = new TRSTGLabFileDataHeader();
                        //            Guid ldh_id = Guid.NewGuid();

                        //            objModel.ldh_id = ldh_id;
                        //            objModel.ldh_status = 'N';
                        //            objModel.ldh_flagdelete = false;
                        //            objModel.ldh_hos_code = LabFile.lfu_hos_code;
                        //            objModel.ldh_lab = LabFile.lfu_lab;
                        //            objModel.ldh_lfu_id = LabFile.lfu_id;


                        //            objModel.ldh_labno = dr[LAB_NO].ToString();
                        //            objModel.ldh_organism = dr[ORGANISM].ToString();
                        //            objModel.ldh_specimen = dr[SOURCE].ToString();
                        //            objModel.ldh_date = dr[DATE].ToString();
                        //            objModel.ldh_createuser = "BATCH";


                        //            int seq = LabDataDAL.Get_CheckExistingHeader(objModel, MappingTemplate.mp_program, MappingTemplate.mp_filetype);
                        //            objModel.ldh_sequence = seq;
                        //            objModel.ldh_createdate = DateTime.Now;
                        //            Guid idh_id_related = Guid.Empty;

                        //            if (MappingTemplate.mp_AntibioticIsolateOneRec == false)
                        //            {
                        //                idh_id_related = LabDataDAL.Get_HeaderIdForMultipleline(objModel);
                        //                if (idh_id_related != Guid.Empty)
                        //                {
                        //                    ldh_id = idh_id_related;
                        //                }
                        //                else
                        //                {
                        //                    var x = LabDataDAL.Save_LabFileDataHeader(objModel);
                        //                }
                        //            }
                        //            else
                        //            {
                        //                var x = LabDataDAL.Save_LabFileDataHeader(objModel);
                        //            }
                        //            #endregion


                        //            #region InsertLabFileDataDetail

                        //            if (whonetmapping != null)
                        //            {
                        //                foreach (WHONetMappingListsDTO item in whonetmapping)
                        //                {
                        //                   // var DataType = dr[item.wnm_originalfield].GetType().Name;

                        //                    var Encoding = "";

                        //                    if(dr.IsNull(item.wnm_originalfield))
                        //                    {
                        //                        continue;
                        //                    }

                        //                    if (item.wnm_encrypt == true)
                        //                    {
                        //                        Encoding = CryptoHelper.UnicodeEncoding(dr[item.wnm_originalfield].ToString());
                        //                    }
                        //                    else
                        //                    {
                        //                        Encoding = dr[item.wnm_originalfield].ToString();
                        //                    }

                        //                    LabFileDataDetailList.Add(new TRSTGLabFileDataDetail
                        //                    {

                        //                        ldd_id = Guid.NewGuid(),
                        //                        ldd_status = 'N',
                        //                        ldd_whonetfield = item.wnm_whonetfield,
                        //                        ldd_originalfield = item.wnm_originalfield,
                        //                        ldd_originalvalue = Encoding,
                        //                        ldd_ldh_id = ldh_id,
                        //                        ldd_createuser = "BATCH",
                        //                        ldd_createdate = DateTime.Now
                        //                    }
                        //                    );

                        //                }
                        //            }
                        //            #endregion
                        //        }
                        //    }


                        //    if (LabFileDataDetailList.Count != 0)
                        //    {
                        //        var ldd = LabDataDAL.Save_LabFileDataDetail(LabFileDataDetailList);
                        //    }

                        //}

                        #endregion
                        else
                        {
                            var options = new DbfDataReaderOptions
                            {
                                Encoding = Encoding.GetEncoding(874)
                            };

                            using (var dbfDataReader = new DbfDataReader.DbfDataReader(LabFile.lfu_Path, options))
                            {
                                var x = dbfDataReader.DbfTable.Header.RecordCount;

                                string LAB_NO = whonetmapping.FirstOrDefault(x => x.wnm_whonetfield == Param_labno).wnm_originalfield;
                                string ORGANISM = whonetmapping.FirstOrDefault(x => x.wnm_whonetfield == Param_organism).wnm_originalfield;
                                string DATE = whonetmapping.FirstOrDefault(x => x.wnm_whonetfield == Param_date).wnm_originalfield;

                                string SOURCE = "";
                                if (whonetmapping.FirstOrDefault(x => x.wnm_whonetfield == Param_specimen) != null)
                                    SOURCE = whonetmapping.FirstOrDefault(x => x.wnm_whonetfield == Param_specimen).wnm_originalfield;


                                List<TRSTGLabFileDataDetail> LabFileDataDetailList = new List<TRSTGLabFileDataDetail>();

                                string Antibioticcolumn = "";
                                List<WHONetMappingListsDTO> MappingAnt = new List<WHONetMappingListsDTO>();
                                if (MappingTemplate.mp_AntibioticIsolateOneRec == false)
                                {
                                    MappingAnt = whonetmapping.Where(x => x.wnm_antibiotic != null).ToList();
                                    Antibioticcolumn = MappingAnt.FirstOrDefault().wnm_antibioticcolumn;


                                    //foreach (WHONetMappingListsDTO t in MappingAnt)
                                    //{
                                    //    excludeAntibiotic.Append(t.wnm_antibiotic);
                                    //}                                 

                                }

                                List<TRLabFileErrorDetail> AntibioticErrorD = new List<TRLabFileErrorDetail>();

                                int ErrorAnt = 0;
                                //int test = 0;
                                while (dbfDataReader.Read())
                                {
                                    //test++;
                                    //if(test == 200)
                                    //{
                                    //    goto EndWhile;
                                    //}

                                    if (MappingTemplate.mp_AntibioticIsolateOneRec == false)
                                    {
                                        if (dbfDataReader[Antibioticcolumn] == "" || dbfDataReader[Antibioticcolumn] == null)
                                        {
                                            continue;
                                        }
                                        string antibiotic = dbfDataReader[Antibioticcolumn].ToString();
                                        if (!MappingAnt.Any(x => x.wnm_antibiotic == antibiotic))
                                        {
                                            if (!AntibioticErrorD.Any(x => x.fed_localvalue == antibiotic))
                                            {
                                                AntibioticErrorD.Add(new TRLabFileErrorDetail
                                                {
                                                    fed_id = Guid.NewGuid(),
                                                    fed_status = 'N',
                                                    fed_localvalue = antibiotic,
                                                    fed_createuser = "BATCH",
                                                    fed_createdate = DateTime.Now
                                                }
                                                );
                                            }
                                            ErrorAnt++;
                                            continue;
                                        }
                                    }

                                    string cmethod = "";
                                    if (dbfDataReader.DbfTable.Columns.FirstOrDefault(x => x.Name == "CMETHOD") != null)
                                        cmethod = dbfDataReader.GetString("CMETHOD");

                                    if ((cmethod == "Aerobic Culture" && LabFile.lfu_Program == "MLAB")
                                    || LabFile.lfu_Program != "MLAB"
                                    || (LabFile.lfu_Program == "MLAB" && MappingTemplate.mp_filetype == "ETEST"))
                                    {
                                        #region InsertLabFileDataHeader
                                        TRSTGLabFileDataHeader objModel = new TRSTGLabFileDataHeader();
                                        Guid ldh_id = Guid.NewGuid();

                                        objModel.ldh_id = ldh_id;
                                        objModel.ldh_status = 'N';
                                        objModel.ldh_flagdelete = false;
                                        objModel.ldh_hos_code = LabFile.lfu_hos_code;
                                        objModel.ldh_lab = LabFile.lfu_lab;
                                        objModel.ldh_lfu_id = LabFile.lfu_id;

                                        objModel.ldh_labno = dbfDataReader[LAB_NO].ToString();
                                        objModel.ldh_organism = dbfDataReader.GetString(ORGANISM);

                                        if (dbfDataReader.DbfTable.Columns.FirstOrDefault(x => x.Name == SOURCE) != null)
                                            objModel.ldh_specimen = dbfDataReader.GetString(SOURCE);

                                        objModel.ldh_date = dbfDataReader.GetDateTime(DATE).ToString();
                                        try
                                        {
                                            objModel.ldh_cdate = dbfDataReader.GetDateTime(DATE);
                                        }
                                        catch (Exception ex)
                                        {
                                            var dd = ex.Message;
                                        }

                                        if (LabFile.lfu_Program == "MLAB" && MappingTemplate.mp_filetype == "ETEST" && objModel.ldh_cdate.Value.Year != LabFile.lfu_DataYear)
                                        {
                                            continue;
                                        }


                                        objModel.ldh_createuser = "BATCH";

                                        int seq = LabDataDAL.Get_CheckExistingHeader(objModel, MappingTemplate.mp_program, MappingTemplate.mp_filetype);
                                        objModel.ldh_sequence = seq;
                                        objModel.ldh_createdate = DateTime.Now;

                                        Guid idh_id_related = Guid.Empty;
                                        bool isNotFirstline = false;

                                        if (MappingTemplate.mp_AntibioticIsolateOneRec == false)
                                        {
                                            idh_id_related = LabDataDAL.Get_HeaderIdForMultipleline(objModel);
                                            if (idh_id_related != Guid.Empty)
                                            {
                                                isNotFirstline = true;
                                                ldh_id = idh_id_related;
                                            }
                                            else
                                            {
                                                var c = LabDataDAL.Save_LabFileDataHeader(objModel);
                                            }
                                        }
                                        else
                                        {
                                            var c = LabDataDAL.Save_LabFileDataHeader(objModel);
                                        }
                                        #endregion

                                        #region InsertLabFileDataDetail
                                        try
                                        {
                                            if (whonetmapping != null)
                                            {
                                                foreach (WHONetMappingListsDTO item in whonetmapping)
                                                {
                                                    if (isNotFirstline == true && MappingTemplate.mp_filetype == "ETEST" && (item.wnm_originalfield == LAB_NO || item.wnm_originalfield == DATE || item.wnm_originalfield == ORGANISM))
                                                    {
                                                        continue;
                                                    }
                                                    //var DataType = dbfDataReader[item.wnm_originalfield].GetType().Name;
                                                    //if (dbfDataReader[item.wnm_originalfield] == "" || dbfDataReader[item.wnm_originalfield] == null)
                                                    if (dbfDataReader[item.wnm_originalfield] == null)
                                                    {
                                                        continue;
                                                    }

                                                    var Encoding = "";

                                                    if (item.wnm_encrypt == true)
                                                    {
                                                        Encoding = CryptoHelper.UnicodeEncoding(dbfDataReader[item.wnm_originalfield].ToString());
                                                    }
                                                    else
                                                    {
                                                        Encoding = dbfDataReader[item.wnm_originalfield].ToString();
                                                    }

                                                    LabFileDataDetailList.Add(new TRSTGLabFileDataDetail
                                                    {

                                                        ldd_id = Guid.NewGuid(),
                                                        ldd_status = 'N',
                                                        ldd_whonetfield = item.wnm_whonetfield,
                                                        ldd_originalfield = item.wnm_originalfield,
                                                        ldd_originalvalue = Encoding,
                                                        ldd_ldh_id = ldh_id,
                                                        ldd_createuser = "BATCH",
                                                        ldd_createdate = DateTime.Now
                                                    }
                                                    );

                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        #endregion
                                        //Test++;

                                    }
                                }

                                //EndWhile:


                                if (AntibioticErrorD.Count > 0)
                                {
                                    chkError = chkError + ErrorAnt;

                                    TRLabFileErrorHeader objErrorH = new TRLabFileErrorHeader();
                                    objErrorH.feh_id = Guid.NewGuid();
                                    objErrorH.feh_status = 'N';
                                    objErrorH.feh_flagdelete = false;
                                    objErrorH.feh_type = "NOT_MATCH";
                                    objErrorH.feh_field = "Antibiotic";
                                    objErrorH.feh_message = "Antibiotic not match";
                                    objErrorH.feh_errorrecord = ErrorAnt;
                                    objErrorH.feh_createuser = "BATCH";
                                    objErrorH.feh_createdate = DateTime.Now;
                                    objErrorH.feh_lfu_id = LabFile.lfu_id;
                                    var objTRLabFileErrorHeader = LabDataDAL.Save_TRLabFileErrorHeader(objErrorH);

                                    //Collection.All(c => { c.needsChange = value; return true; });
                                    AntibioticErrorD = AntibioticErrorD.Select(a => { a.fed_feh_id = objErrorH.feh_id; return a; }).ToList();
                                    var objTRLabFileErrorDetail = LabDataDAL.Save_TRLabFileErrorDetailList(AntibioticErrorD);

                                }
                                if (LabFileDataDetailList.Count != 0)
                                {
                                    var ldd = LabDataDAL.Save_LabFileDataDetail(LabFileDataDetailList);
                                }
                            }
                        }


                    Finish:
                        char str;
                        if (chkError == 0)
                        {
                            str = 'M';
                        }
                        else
                        {
                            str = 'E';
                        }
                        var rowsAffected2 = LabDataDAL.Update_LabFileUploadStatus(LabFile.lfu_id.ToString(), str, chkError + chkDetailError, "BATCH");
                    }
                }

            }
            catch (Exception ex)


            {
                var d = ex.Message;
            }
        }


        public static string convertColumn(string originalColumn)
        {
            string objReturn = "";
            int index = 0;
            Int32.TryParse(originalColumn.Replace("Column", ""), out index);

            objReturn = "Column" + (index - 1);

            return objReturn;
        }

    }
}
