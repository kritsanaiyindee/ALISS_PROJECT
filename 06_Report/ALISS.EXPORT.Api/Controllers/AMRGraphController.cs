//using ALISS.EXPORT.Api.DTO;
//using ALISS.EXPORT.Api.Models;
using ALISS.EXPORT.Library.Model;
using ALISS.EXPORT.Library.DTO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace ALISS.EXPORT.Api.Controllers
{
    public class AMRGraphController : ApiController
    {
        ALISSEntities db = new ALISSEntities(true);

        [HttpGet]
        [Route("api/AMRGraph/Get")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("api/AMRGraph/GetDB")]
        public IEnumerable<string> GetDB()
        {
            var organismMaster = new List<sp_GET_TCOrganism_Active_Result>();
            organismMaster = db.sp_GET_TCOrganism_Active(null).ToList();
            return new string[] { "Connection DB", "OK" };
        }

        [HttpGet]
        [Route("api/AMRGraph/GetFile")]
        public IHttpActionResult GetFile()
        {
            var modelYearFrom = 2015; 
            var modelYearTo = 2020; 
            var modelSelectedOrg = new List<string> { "alc", "bde" }; 
            var modelSelectedAnti = new List<string> { "AMX", "CEC" };
            var modelStrSelectedOrg = string.Join(",", modelSelectedOrg);
            var modelStrSelectedAnti = string.Join(",", modelSelectedAnti);
            var modelSelectedSIR = "S"; 
            var modelGraphFormat = 1; //  1 = Line , 2 = Bar ;
            var modelSubGraph = 2;  // 0 = none , 1 = Specimen , 2 = Ward//model.sub_graph; 
            var strReportTitle = "";

            // Get Specimen Master
            var SpecimenMasters = db.sp_GET_TCSpecimen_Active(null);
            var SpecimenMaster = SpecimenMasters.Select(w => w.spc_code).Distinct().ToList();
            // Get Ward Type Master
            var WardTypeMasters = db.sp_GET_TCWardType_Active(null);
            var WardTypeMaster = WardTypeMasters.Select(w => w.wrd_name).Distinct().ToList();

            if (modelSelectedSIR == "S") { strReportTitle = "Susceptibility of "; }
            else if (modelSelectedSIR == "I") { strReportTitle = "Intermediate of "; }
            else { strReportTitle = "Resistance of "; }

            //var queryRaw = new List<sp_GET_RPAntibicromialResistance_Result>();
            //var query = new List<sp_GET_RPAntibicromialResistance_Result>();
            var queryAll = new List<sp_GET_RPAntibicromialResstAll_Result>();
            var queryWard = new List<sp_GET_RPAntibicromialResstWard_Result>();
            var querySpcimen = new List<sp_GET_RPAntibicromialResstSpecimen_Result>();
            var queryRawAll = new List<sp_GET_RPAntibicromialResstAll_Result>();
            var queryRawWard = new List<sp_GET_RPAntibicromialResstWard_Result>();
            var queryRawSpcimen = new List<sp_GET_RPAntibicromialResstSpecimen_Result>();

            var organismMaster = new List<sp_GET_TCOrganism_Active_Result>();
            var antibioticMaster = new List<sp_GET_TCAntibiotic_Active_Result>();

            organismMaster = db.sp_GET_TCOrganism_Active(null).ToList();
            antibioticMaster = db.sp_GET_TCAntibiotic_Active(null).ToList();

            ReportDocument rpt = new ReportDocument();
            var strReportName = "";
            object dsReport = new object();

            //query = db.sp_GET_RPAntibicromialResistance(modelStrSelectedOrg, modelStrSelectedAnti, modelYearFrom, modelYearTo).ToList();
            //queryRaw = query.ToList();
            if (modelSubGraph == 0)
            {
                queryAll = db.sp_GET_RPAntibicromialResstAll(modelStrSelectedOrg, modelStrSelectedAnti, modelYearFrom, modelYearTo).ToList();
                queryRawAll = queryAll.ToList();
                dsReport = queryAll;
            }
            else if (modelSubGraph == 1)
            {
                querySpcimen = db.sp_GET_RPAntibicromialResstSpecimen(modelStrSelectedOrg, modelStrSelectedAnti, modelYearFrom, modelYearTo).ToList();
                queryRawSpcimen = querySpcimen.ToList();
                dsReport = querySpcimen;
            }
            else if (modelSubGraph == 2)
            {
                queryWard = db.sp_GET_RPAntibicromialResstWard(modelStrSelectedOrg, modelStrSelectedAnti, modelYearFrom, modelYearTo).ToList();
                queryRawWard = queryWard.ToList();
                dsReport = queryWard;
            }

                 
            // add empty 
            for (var iYear = modelYearFrom; iYear <= modelYearTo; iYear++)
            {
                foreach (var orgn in modelSelectedOrg)
                {
                    foreach (var drug in modelSelectedAnti)
                    {

                        if (modelSubGraph == 0)
                        {
                            var check = queryRawAll.Where(w => w.year == iYear && w.anti_code == drug && w.org_code == orgn).ToList();
                            if (check.Count() == 0)
                            {
                                var emptData = new sp_GET_RPAntibicromialResstAll_Result();
                                emptData.year = iYear;
                                emptData.anti_code = drug;
                                //ToDo : Find Anti Name From Anti Code
                                //emptData.anti_name = ((drug == "AMX") ? "Amoxicillin" : "Cefaclor");
                                emptData.anti_name = (antibioticMaster.Find(t => t.ant_mst_WHON5_CODE == drug) != null) ? antibioticMaster.Find(t => t.ant_mst_WHON5_CODE == drug).ant_name : drug;
                                //emptData.anti_name =  antibioticMaster.Find(t => t.ant_code == drug).ant_name;
                                emptData.org_code = orgn;
                                //ToDo : Find Org Name From Org Code
                                //emptData.org_name = ((orgn == "alc" )? "Alcaligenes sp." : "Bacteroides denticola"); 
                                emptData.org_name = (organismMaster.Find(t => t.org_mst_ORG == orgn) != null) ? organismMaster.Find(t => t.org_mst_ORG == orgn).org_mst_ORGANISM : orgn;
                                emptData.spc_code = "";
                                emptData.ward_type = "";
                                emptData.percent_s = 0;
                                emptData.percent_i = 0;
                                emptData.percent_r = 0;

                                queryAll.Add(emptData);
                            }
                        }

                        else if (modelSubGraph == 1)
                        {
                            foreach (var spc in SpecimenMaster)
                            {
                                var checkSpec = queryRawSpcimen.Where(w => w.year == iYear && w.anti_code == drug
                                                         && w.org_code == orgn && w.spc_code == spc).ToList();
                                if (checkSpec.Count() == 0)
                                {
                                    var emptData = new sp_GET_RPAntibicromialResstSpecimen_Result();
                                    emptData.year = iYear;
                                    emptData.anti_code = drug;
                                    //ToDo : Find Anti Name From Anti Code
                                    emptData.anti_name = (antibioticMaster.Find(t => t.ant_mst_WHON5_CODE == drug) != null) ? antibioticMaster.Find(t => t.ant_mst_WHON5_CODE == drug).ant_name : drug;
                                    emptData.org_code = orgn;
                                    //ToDo : Find Org Name From Org Code
                                    emptData.org_name = (organismMaster.Find(t => t.org_mst_ORG == orgn) != null) ? organismMaster.Find(t => t.org_mst_ORG == orgn).org_mst_ORGANISM : orgn;
                                    emptData.spc_code = spc;
                                    emptData.ward_type = "";
                                    emptData.percent_s = 0;
                                    emptData.percent_i = 0;
                                    emptData.percent_r = 0;

                                    querySpcimen.Add(emptData);
                                }
                            }
                        }

                        else if (modelSubGraph == 2)
                        {
                            foreach (var ward in WardTypeMaster)
                            {
                                var checkWard = queryRawWard.Where(w => w.year == iYear && w.anti_code == drug
                                                         && w.org_code == orgn && w.ward_type == ward).ToList();
                                if (checkWard.Count() == 0)
                                {
                                    var emptData = new sp_GET_RPAntibicromialResstWard_Result();
                                    emptData.year = iYear;
                                    emptData.anti_code = drug;
                                    //ToDo : Find Anti Name From Anti Code
                                    //emptData.anti_name = ((drug == "AMX") ? "Amoxicillin" : "Cefaclor");
                                    emptData.anti_name = (antibioticMaster.Find(t => t.ant_mst_WHON5_CODE == drug) != null) ? antibioticMaster.Find(t => t.ant_mst_WHON5_CODE == drug).ant_name : drug;
                                    emptData.org_code = orgn;
                                    //ToDo : Find Org Name From Org Code
                                    //emptData.org_name = ((orgn == "alc") ? "Alcaligenes sp." : "Bacteroides denticola");
                                    emptData.org_name = (organismMaster.Find(t => t.org_mst_ORG == orgn) != null) ? organismMaster.Find(t => t.org_mst_ORG == orgn).org_mst_ORGANISM : orgn;
                                    emptData.spc_code = "";
                                    emptData.ward_type = ward;
                                    emptData.percent_s = 0;
                                    emptData.percent_i = 0;
                                    emptData.percent_r = 0;

                                    queryWard.Add(emptData);
                                }
                            }
                        }
                    }
                }
            }

            //case test : หลายเชื้อ 1 ยา หรือ 1:1 (no-sub)
            //Note : where ยา (Data ทั้งหมดต้องเป็นยา 1 ตัวเท่านั้น)

            if (modelSelectedOrg.Count() > 0 && modelSelectedAnti.Count() == 1)
            {
                if (modelSubGraph == 0) // No SubGraph
                {
                    strReportName = "rptAMRMultiAntiOrg.rpt";
                    strReportTitle += modelStrSelectedAnti;
                }
                else if (modelSubGraph == 1) // Sub Specimen
                {
                    strReportName = "rptAMRAntiSubSpc.rpt";
                    strReportTitle += string.Format("{0} by {1}", modelStrSelectedAnti, "Specimen");
                }
                else // Sub Ward Type
                {
                    strReportName = "rptAMRAntiSubWard.rpt";
                    strReportTitle += string.Format("{0} by {1}", modelStrSelectedAnti, "Ward Type");
                }
            }


            //case test : 1 เชื้อ หลายยา 
            //Note : where เชื้อ (Data ทั้งหมดต้องเป็นเชื้อ 1 ตัวเท่านั้น)
            if (modelSelectedOrg.Count() == 1 && modelSelectedAnti.Count() > 1)
            {
                //query = db.sp_GET_RPAntibicromialResistance(modelStrSelectedOrg, modelStrSelectedAnti, modelYearFrom, modelYearTo).ToList();
                if (modelSubGraph == 0) // No SubGraph
                {
                    strReportName = "rptAMRMultiAnti.rpt";
                    strReportTitle += modelStrSelectedOrg;
                }
                else if (modelSubGraph == 1) // Sub Specimen
                {
                    strReportName = "rptAMROrgSubSpec.rpt";
                    strReportTitle += string.Format("{0} by {1}", modelStrSelectedOrg, "Specimen");
                }
                else // Sub Ward Type
                {
                    strReportName = "rptAMROrgSubWard.rpt";
                    strReportTitle += string.Format("{0} by {1}", modelStrSelectedOrg, "Ward Type");
                }

            }

            //case test : หลายเชื้อ หลายยา (no-sub)
            //Note จำนวนกราฟ = จำนวนยา , จำนวน series = จำนวนเชื้อ
            if (modelSelectedOrg.Count() > 1 && modelSelectedAnti.Count() > 1)
            {
                if (modelSubGraph == 0) // No SubGraph
                {
                    strReportName = "rptAMRMultiAntiOrg.rpt";
                    strReportTitle += modelStrSelectedOrg;
                }
                else if (modelSubGraph == 1) // Sub Specimen
                {
                    strReportName = "rptAMRAntiSubSpc.rpt";
                    strReportTitle += string.Format("{0} by {1}", modelStrSelectedAnti, "Specimen");
                }
                else // Sub Ward Type
                {
                    strReportName = "rptAMRAntiSubWard.rpt";
                    strReportTitle += string.Format("{0} by {1}", modelStrSelectedAnti, "Ward Type");
                }
            }

            
            var Targetpath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Report"), strReportName);
            rpt.Load(Targetpath);
            rpt.SetDataSource(dsReport);
            rpt.SetParameterValue("paramStrTitle", strReportTitle);
            rpt.SetParameterValue("paramStrSelectedSIR", modelSelectedSIR);
            rpt.SetParameterValue("paramIntChartType", modelGraphFormat);

            // --------------- Export to PDF -------------------------------

            var stream = rpt.ExportToStream(ExportFormatType.PortableDocFormat);
            // processing the stream.
            var sm = ReadFully(stream);
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(sm.ToArray())
            };
            result.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    //FileName = "AMR.xls"
                    FileName = "AMR.pdf"
                };
            result.Content.Headers.ContentType =
                 new MediaTypeHeaderValue("application/pdf");

            rpt.Dispose();

            var response = ResponseMessage(result);
            return response;
        }

        [HttpPost]
        [Route("api/AMRGraph/ExportGraph")]
        public IHttpActionResult ExportGraph(AMRGraphSearchDTO model)
        //[HttpGet]
        //[Route("api/AMRGraph/ExportGraph")]
        //public IHttpActionResult ExportGraph()
        {
            var modelYearFrom = model.start_year; //2015; 
            var modelYearTo =   model.end_year; //2020; 
            var modelSelectedOrg =  model.organism; //new List<string> { "alc", "bde" }; 
            var modelSelectedAnti =  model.antibiotic; // new List<string> { "AMX", "CEC" };
            var modelStrSelectedOrg = string.Join(",", modelSelectedOrg);
            var modelStrSelectedAnti = string.Join(",", modelSelectedAnti);
            var modelSelectedSIR =  model.sir; //"S"; 
            var modelGraphFormat = model.graph_format ; //  1 = Line , 2 = Bar ;
            var modelSubGraph = model.sub_graph;  // 0 = none , 1 = Specimen , 2 = Ward//model.sub_graph; 
            var strReportTitle = "";

            // Get Specimen Master
            var SpecimenMasters = db.sp_GET_TCSpecimen_Active(null);
            var SpecimenMaster = SpecimenMasters.Select(w => w.spc_code).Distinct().ToList();
            // Get Ward Type Master
            var WardTypeMasters = db.sp_GET_TCWardType_Active(null);
            var WardTypeMaster = WardTypeMasters.Select(w => w.wrd_name).Distinct().ToList();

            if (modelSelectedSIR == "S") { strReportTitle =  "Susceptibility of ";}
            else if (modelSelectedSIR == "I") { strReportTitle = "Intermediate of "; }
            else { strReportTitle = "Resistance of "; }

            //var queryRaw = new List<sp_GET_RPAntibicromialResistance_Result>();
            //var query = new List<sp_GET_RPAntibicromialResistance_Result>();
            var queryAll = new List<sp_GET_RPAntibicromialResstAll_Result>();
            var queryWard = new List<sp_GET_RPAntibicromialResstWard_Result>();
            var querySpcimen = new List<sp_GET_RPAntibicromialResstSpecimen_Result>();
            var queryRawAll = new List<sp_GET_RPAntibicromialResstAll_Result>();
            var queryRawWard = new List<sp_GET_RPAntibicromialResstWard_Result>();
            var queryRawSpcimen = new List<sp_GET_RPAntibicromialResstSpecimen_Result>();

            var organismMaster = new List<sp_GET_TCOrganism_Active_Result>();
            var antibioticMaster = new List<sp_GET_TCAntibiotic_Active_Result>();

            organismMaster = db.sp_GET_TCOrganism_Active(null).ToList();
            antibioticMaster = db.sp_GET_TCAntibiotic_Active(null).ToList();

            ReportDocument rpt = new ReportDocument();
            var strReportName = "";
            object dsReport = new object();

            //query = db.sp_GET_RPAntibicromialResistance(modelStrSelectedOrg, modelStrSelectedAnti, modelYearFrom, modelYearTo).ToList();
            //queryRaw = query.ToList();

            if (modelSubGraph == 0)
            {
                queryAll = db.sp_GET_RPAntibicromialResstAll(modelStrSelectedOrg, modelStrSelectedAnti, modelYearFrom, modelYearTo).ToList();
                queryRawAll = queryAll.ToList();
                dsReport = queryAll;
            }
            else if (modelSubGraph == 1)
            {
                querySpcimen = db.sp_GET_RPAntibicromialResstSpecimen(modelStrSelectedOrg, modelStrSelectedAnti, modelYearFrom, modelYearTo).ToList();
                queryRawSpcimen = querySpcimen.ToList();
                dsReport = querySpcimen;
            }
            else if (modelSubGraph == 2)
            {
                queryWard = db.sp_GET_RPAntibicromialResstWard(modelStrSelectedOrg, modelStrSelectedAnti, modelYearFrom, modelYearTo).ToList();
                queryRawWard = queryWard.ToList();
                dsReport = queryWard;
            }

            // add empty 
            for (var iYear = modelYearFrom; iYear <= modelYearTo; iYear++)
            {   
                foreach(var orgn in modelSelectedOrg)
                {               
                    foreach (var drug in modelSelectedAnti)
                    {
                       
                        if(modelSubGraph == 0)
                        {
                            var check = queryRawAll.Where(w => w.year == iYear && w.anti_code == drug && w.org_code == orgn).ToList();
                            if (check.Count() == 0)
                            {
                                var emptData = new sp_GET_RPAntibicromialResstAll_Result();
                                emptData.year = iYear;
                                emptData.anti_code = drug;
                                //ToDo : Find Anti Name From Anti Code
                                //emptData.anti_name = ((drug == "AMX") ? "Amoxicillin" : "Cefaclor");
                                emptData.anti_name = (antibioticMaster.Find(t => t.ant_mst_WHON5_CODE == drug) != null)? antibioticMaster.Find(t => t.ant_mst_WHON5_CODE == drug).ant_name : drug;
                                //emptData.anti_name =  antibioticMaster.Find(t => t.ant_code == drug).ant_name;
                                emptData.org_code = orgn;
                                //ToDo : Find Org Name From Org Code
                                //emptData.org_name = ((orgn == "alc" )? "Alcaligenes sp." : "Bacteroides denticola"); 
                                emptData.org_name = (organismMaster.Find(t => t.org_mst_ORG == orgn) != null)? organismMaster.Find(t => t.org_mst_ORG == orgn).org_mst_ORGANISM : orgn;
                                emptData.spc_code = "";
                                emptData.ward_type = "";
                                emptData.percent_s = 0;
                                emptData.percent_i = 0;
                                emptData.percent_r = 0;

                                queryAll.Add(emptData);
                            }
                        }                      
                        
                        else if(modelSubGraph == 1)
                        {
                            foreach (var spc in SpecimenMaster)
                            {
                                var checkSpec = queryRawSpcimen.Where(w => w.year == iYear && w.anti_code == drug
                                                         && w.org_code == orgn && w.spc_code == spc).ToList();
                                if (checkSpec.Count() == 0)
                                {
                                    var emptData = new sp_GET_RPAntibicromialResstSpecimen_Result();
                                    emptData.year = iYear;
                                    emptData.anti_code = drug;
                                    //ToDo : Find Anti Name From Anti Code
                                    emptData.anti_name = (antibioticMaster.Find(t => t.ant_mst_WHON5_CODE == drug) != null) ? antibioticMaster.Find(t => t.ant_mst_WHON5_CODE == drug).ant_name : drug;
                                    emptData.org_code = orgn;
                                    //ToDo : Find Org Name From Org Code
                                    emptData.org_name = (organismMaster.Find(t => t.org_mst_ORG == orgn) != null) ? organismMaster.Find(t => t.org_mst_ORG == orgn).org_mst_ORGANISM : orgn;
                                    emptData.spc_code = spc;
                                    emptData.ward_type = "";
                                    emptData.percent_s = 0;
                                    emptData.percent_i = 0;
                                    emptData.percent_r = 0;

                                    querySpcimen.Add(emptData);
                                }
                            }
                        }
                        
                        else if(modelSubGraph == 2)
                        {
                            foreach (var ward in WardTypeMaster)
                            {
                                var checkWard = queryRawWard.Where(w => w.year == iYear && w.anti_code == drug
                                                         && w.org_code == orgn && w.ward_type == ward).ToList();
                                if (checkWard.Count() == 0)
                                {
                                    var emptData = new sp_GET_RPAntibicromialResstWard_Result();
                                    emptData.year = iYear;
                                    emptData.anti_code = drug;
                                    //ToDo : Find Anti Name From Anti Code
                                    //emptData.anti_name = ((drug == "AMX") ? "Amoxicillin" : "Cefaclor");
                                    emptData.anti_name = (antibioticMaster.Find(t => t.ant_mst_WHON5_CODE == drug) != null) ? antibioticMaster.Find(t => t.ant_mst_WHON5_CODE == drug).ant_name : drug;
                                    emptData.org_code = orgn;
                                    //ToDo : Find Org Name From Org Code
                                    //emptData.org_name = ((orgn == "alc") ? "Alcaligenes sp." : "Bacteroides denticola");
                                    emptData.org_name = (organismMaster.Find(t => t.org_mst_ORG == orgn) != null) ? organismMaster.Find(t => t.org_mst_ORG == orgn).org_mst_ORGANISM : orgn;
                                    emptData.spc_code = "";
                                    emptData.ward_type = ward;
                                    emptData.percent_s = 0;
                                    emptData.percent_i = 0;
                                    emptData.percent_r = 0;

                                    queryWard.Add(emptData);
                                }
                            }
                        }           
                    }          
                }
            }

            //case test : หลายเชื้อ 1 ยา หรือ 1:1 (no-sub)
            //Note : where ยา (Data ทั้งหมดต้องเป็นยา 1 ตัวเท่านั้น)
            if (modelSelectedOrg.Count() > 0 && modelSelectedAnti.Count() == 1)
            {
                if (modelSubGraph == 0) // No SubGraph
                {
                    strReportName = "rptAMRMultiAntiOrg.rpt";
                    strReportTitle += modelStrSelectedAnti;
                }
                else if (modelSubGraph == 1) // Sub Specimen
                {
                    strReportName = "rptAMRAntiSubSpc.rpt";
                    strReportTitle += string.Format("{0} by {1}", modelStrSelectedAnti, "Specimen");
                }
                else // Sub Ward Type
                {
                    strReportName = "rptAMRAntiSubWard.rpt";
                    strReportTitle += string.Format("{0} by {1}", modelStrSelectedAnti, "Ward Type");
                }
            }
            

            //case test : 1 เชื้อ หลายยา 
            //Note : where เชื้อ (Data ทั้งหมดต้องเป็นเชื้อ 1 ตัวเท่านั้น)
            if (modelSelectedOrg.Count() == 1 && modelSelectedAnti.Count() > 1)
            {
                //query = db.sp_GET_RPAntibicromialResistance(modelStrSelectedOrg, modelStrSelectedAnti, modelYearFrom, modelYearTo).ToList();
                if (modelSubGraph == 0) // No SubGraph
                {
                    strReportName = "rptAMRMultiAnti.rpt";
                    strReportTitle += modelStrSelectedOrg;
                }
                else if (modelSubGraph == 1) // Sub Specimen
                {
                    strReportName = "rptAMROrgSubSpec.rpt";
                    strReportTitle += string.Format("{0} by {1}", modelStrSelectedOrg,"Specimen");
                }
                else // Sub Ward Type
                {
                    strReportName = "rptAMROrgSubWard.rpt";
                    strReportTitle += string.Format("{0} by {1}", modelStrSelectedOrg, "Ward Type");
                }
               
            }

            //case test : หลายเชื้อ หลายยา (no-sub)
            //Note จำนวนกราฟ = จำนวนยา , จำนวน series = จำนวนเชื้อ
            if (modelSelectedOrg.Count() > 1 && modelSelectedAnti.Count() > 1)
            {
                if (modelSubGraph == 0) // No SubGraph
                {
                    strReportName = "rptAMRMultiAntiOrg.rpt";
                    strReportTitle += modelStrSelectedOrg;
                }
                else if (modelSubGraph == 1) // Sub Specimen
                {
                    strReportName = "rptAMRAntiSubSpc.rpt";
                    strReportTitle += string.Format("{0} by {1}", modelStrSelectedAnti, "Specimen");
                }
                else // Sub Ward Type
                {
                    strReportName = "rptAMRAntiSubWard.rpt";
                    strReportTitle += string.Format("{0} by {1}", modelStrSelectedAnti, "Ward Type");
                }
            }


            var Targetpath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Report"), strReportName);
            rpt.Load(Targetpath);
            rpt.SetDataSource(dsReport);
            rpt.SetParameterValue("paramStrTitle", strReportTitle);
            rpt.SetParameterValue("paramStrSelectedSIR", modelSelectedSIR);
            rpt.SetParameterValue("paramIntChartType", modelGraphFormat);

            // --------------- Export to PDF -------------------------------

            var stream = rpt.ExportToStream(ExportFormatType.PortableDocFormat);
            // processing the stream.
            var sm = ReadFully(stream);
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(sm.ToArray())
            };
            result.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    //FileName = "AMR.xls"
                    FileName = "AMR.pdf"
                };
            result.Content.Headers.ContentType =
                 new MediaTypeHeaderValue("application/pdf");

            rpt.Dispose();

            var response = ResponseMessage(result);
            return response;
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

    } // end ApiController
}
