using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ALISS.DropDownList.DTO;
using ALISS.GLASS.DTO;
using ALISS.GLASS.Library;
using ALISS.GLASS.Library.DataAccess;
using AutoMapper;
using Log4NetLibrary;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;


namespace ALISS.GLASS.Api.Controllers
{  
    [ApiController]
    public class GlassController : ControllerBase
    {
        private readonly IGlassService _service;        
        private readonly IWebHostEnvironment _host;
        private static readonly ILogService log = new LogService(typeof(GlassController));
        public GlassController(GlassContext db, IMapper mapper, IWebHostEnvironment host)
        {
            _service = new GlassService(db, mapper);
            _host = host;
        }

        [HttpGet]
        [Route("api/Glass/Get")]
        public IEnumerable<string> Get()
        {
            return new string[] { "Glass", "Glass" };
        }

        [HttpGet]
        [Route("api/Glass/GetGlassPublicFileList")]
        public IEnumerable<GlassFileListDTO> GetGlassPublicFileList()
        {
            var objReturn = _service.GetGlassPublicFileListData();
            return objReturn;
        }


        [HttpPost]
        [Route("api/Glass/GetGlassPublicFileListModel")]
        public IEnumerable<GlassFileListDTO> GetGlassPublicFileListModel([FromBody]GlassFileListNationSearchDTO searchModel)
        {
            var objReturn = _service.GetGlassPublicFileListDataModel(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Glass/GetGlassPublicRegHealthFileListModel")]
        public IEnumerable<GlassFileListDTO> GetGlassPublicRegHealthFileListModel([FromBody]GlassFileListNationSearchDTO searchModel)
        {
            var objReturn = _service.GetGlassPublicRegHealthFileListDataModel(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Glass/DownloadRISFile/{param}")]
        public async Task<IActionResult> DownloadFile([FromBody]GlassFileListDTO model, string param)
        {
            string contentType = null;
            string filename = null;
            string filepath = null;

            if (!string.IsNullOrEmpty(param) && param == "r")
            {
                contentType = model.ris_file_type;
                filename = model.ris_file_name;
                filepath = model.ris_file_path.Remove(0, 1);
            }

            if (!string.IsNullOrEmpty(param) && param == "s")
            {
                contentType = model.sample_file_type;
                filename = model.sample_file_name;
                filepath = model.sample_file_path.Remove(0, 1);
            }

            //string contentType = "text/plain";
            //string filename = "/GLASS-THA-2020-DS1-RIS.txt";
            //string filepath = "GlassProcessFile";


            if (filename == null)
            {
                return Content("filename not present");
            }

            //var OutputFilePath = Path.Combine(Server.MapPath("~/" + "FormTemp"), fid + ".pdf");
            string Targetpath = _host.ContentRootPath; //Directory.GetCurrentDirectory(); 
            //string Targetpath = @"D:\Project\ALISS_PROJECT\06_Report\ALISS.ANTIBIOGRAM.Api";
            var ServerFilePath = Path.Combine(Targetpath, filepath, filename);
            //var OutputFilePath = @"D:\Project\ALISS_PROJECT\06_Report\ALISS.ANTIBIOGRAM.Api\GlassProcessFile\bb.txt";
            var memory = new MemoryStream();
            bool FileExists = System.IO.File.Exists(ServerFilePath);

            if (FileExists)
            {
                using (var stream = new FileStream(ServerFilePath, FileMode.Open))
                {
                    try
                    {
                        await stream.CopyToAsync(memory);
                    }
                    finally
                    {
                        //memory.Flush();
                    }
                   
                }
            }
            else
            {
                Response.StatusCode = 404;
            }

            memory.Position = 0;
            return File(memory, contentType, Path.GetFileName(ServerFilePath));
        }

        [HttpGet]
        [Route("api/Glass/TestGenFile")]
        public async Task<IActionResult> TestGenFile()
        {
            try
            {
                log.MethodStart();
                GlassFileListDTO model = _service.GetGlassPublicFileListData().Where(w => w.who_flag == false).FirstOrDefault();

                int ParamYear = model.year;
                string ParamAreaHealth = string.Format("Area Health No. {0}", model.arh_code);

                //string sWebRootFolder = @"D:\Project\ALISS_PROJECT\06_Report\ALISS.ANTIBIOGRAM.Api\" + model.analyze_file_path.Remove(0, 1);

                var sFileName = model.analyze_file_name;
                var sFilePdfName = model.analyze_file_name.Replace(".xlsx", "pdf");
                var strReportPath = "";

                // Find ALISS Process Path
                List<ParameterDTO> objParamList = new List<ParameterDTO>();
                var searchModel = new ParameterDTO() { prm_code_major = "RPT_PROCESS_PATH" };

                objParamList = _service.GetGlassReportPath(searchModel);

                if (objParamList.FirstOrDefault(x => x.prm_code_minor == "PATH") != null)
                {
                    strReportPath = objParamList.FirstOrDefault(x => x.prm_code_minor == "PATH").prm_value;
                }
                else
                {
                    Response.StatusCode = 404;
                    return new EmptyResult();
                }
                // Find ALISS Process Path

                string sWebRootFolder = Path.Combine(strReportPath, model.analyze_file_path.Remove(0, 1));
                //sWebRootFolder = @"D:\ALISS_ProcessFile\GLASS\20200730_01_GLASS";
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                FileInfo filePdf = new FileInfo(Path.Combine(sWebRootFolder, sFilePdfName));

                if (file.Exists)
                {
                    file.Delete();
                    file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                }

                var objGlass = _service.GetGlassInfectOverviewModel(model);
                string spc_code = "";
                int startRowidx = 8;
                int idxRowSpc = startRowidx;
                const int colOvvSpcName = 1;
                const int colOvvSpcCO = 2;
                const int colOvvSpcHO = 3;
                const int colOvvSpcUO = 4;
                const int colOvvSpcTotal = 5;

                const int colOvvPathName = 6;
                const int colOvvPathCO = 7;
                const int colOvvPathHO = 8;
                const int colOvvPathUO = 9;
                const int colOvvPathTotal = 10;

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelWorksheet worksheets;
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ////------------------------   Sheet 1 : Data Glass overview -----------------------------------------------
                    ///
                    worksheets = package.Workbook.Worksheets.Add("Data GLASS overview");

                    worksheets.Cells[1, 1].Value = "Data GLASS overview";
                    using (ExcelRange h = worksheets.Cells[(1), 1, (1), (3)])
                    {
                        h.Style.Font.Bold = true;
                        h.Style.Font.Size = 20;
                    }
                    worksheets.Cells[2, 1].Value = string.Format("January-December {0} - {1}", ParamYear, ParamAreaHealth);
                    using (ExcelRange h = worksheets.Cells[(2), 1, (2), (5)])
                    {
                        h.Style.Font.Bold = true;
                        h.Style.Font.Size = 16;

                    }

                    using (ExcelRange h = worksheets.Cells[(6), colOvvSpcName, (8), (colOvvPathTotal)])
                    {
                        h.Style.Font.Bold = true;
                        h.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        h.Style.Fill.BackgroundColor.SetColor(1, 220, 230, 241);
                        h.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    worksheets.Cells[6, colOvvSpcName].Value = "Specimen";
                    worksheets.Cells[6, colOvvSpcName, 8, colOvvSpcName].Merge = true;

                    worksheets.Cells[6, colOvvSpcCO].Value = "Number of test pateints";
                    worksheets.Cells[6, colOvvSpcCO, 6, colOvvSpcTotal].Merge = true;

                    worksheets.Cells[7, colOvvSpcCO].Value = "Community Origin";
                    worksheets.Cells[7, colOvvSpcCO, 8, colOvvSpcCO].Merge = true;
                    worksheets.Cells[7, colOvvSpcHO].Value = "Hospital Origin";
                    worksheets.Cells[7, colOvvSpcHO, 8, colOvvSpcHO].Merge = true;
                    worksheets.Cells[7, colOvvSpcUO].Value = "Unknow Origin";
                    worksheets.Cells[7, colOvvSpcUO, 8, colOvvSpcUO].Merge = true;
                    worksheets.Cells[7, colOvvSpcTotal].Value = "Total";
                    worksheets.Cells[7, colOvvSpcTotal, 8, colOvvSpcTotal].Merge = true;

                    worksheets.Cells[6, colOvvPathName].Value = "Pathogens";
                    worksheets.Cells[6, colOvvPathName, 8, colOvvPathName].Merge = true;

                    worksheets.Cells[6, colOvvPathCO].Value = "Number of patients with positive samples";
                    worksheets.Cells[6, colOvvPathCO, 6, colOvvPathTotal].Merge = true;

                    worksheets.Cells[7, colOvvPathCO].Value = "Community Origin";
                    worksheets.Cells[7, colOvvPathCO, 8, colOvvPathCO].Merge = true;
                    worksheets.Cells[7, colOvvPathHO].Value = "Hospital Origin";
                    worksheets.Cells[7, colOvvPathHO, 8, colOvvPathHO].Merge = true;
                    worksheets.Cells[7, colOvvPathUO].Value = "Unknow Origin";
                    worksheets.Cells[7, colOvvPathUO, 8, colOvvPathUO].Merge = true;
                    worksheets.Cells[7, colOvvPathTotal].Value = "Total";
                    worksheets.Cells[7, colOvvPathTotal, 8, colOvvPathTotal].Merge = true;

                    var cntSpcRow = 0;
                    foreach (var obj in objGlass)
                    {
                        idxRowSpc += 1;

                        if (spc_code != "" && spc_code != obj.spc_code)
                        {
                            worksheets.Cells[idxRowSpc - cntSpcRow, colOvvSpcName, idxRowSpc - 1, colOvvSpcName].Merge = true;
                            worksheets.Cells[idxRowSpc - cntSpcRow, colOvvSpcCO, idxRowSpc - 1, colOvvSpcCO].Merge = true;
                            worksheets.Cells[idxRowSpc - cntSpcRow, colOvvSpcHO, idxRowSpc - 1, colOvvSpcHO].Merge = true;
                            worksheets.Cells[idxRowSpc - cntSpcRow, colOvvSpcUO, idxRowSpc - 1, colOvvSpcUO].Merge = true;
                            worksheets.Cells[idxRowSpc - cntSpcRow, colOvvSpcTotal, idxRowSpc - 1, colOvvSpcTotal].Merge = true;

                            cntSpcRow = 0;
                        }

                        worksheets.Cells[idxRowSpc, colOvvSpcName].Value = obj.spc_name;
                        worksheets.Cells[idxRowSpc, colOvvSpcCO].Value = obj.spc_co_origin;
                        worksheets.Cells[idxRowSpc, colOvvSpcHO].Value = obj.spc_hos_origin;
                        worksheets.Cells[idxRowSpc, colOvvSpcUO].Value = obj.spc_unk_origin;
                        worksheets.Cells[idxRowSpc, colOvvSpcTotal].Value = obj.spc_total_origin;

                        worksheets.Cells[idxRowSpc, 6].Value = obj.org_name;
                        worksheets.Cells[idxRowSpc, 7].Value = obj.org_co_origin;
                        worksheets.Cells[idxRowSpc, 8].Value = obj.org_hos_origin;
                        worksheets.Cells[idxRowSpc, 9].Value = obj.org_unk_origin;
                        worksheets.Cells[idxRowSpc, 10].Value = obj.org_total_origin;

                        spc_code = obj.spc_code;
                        cntSpcRow += 1;
                    }

                    worksheets.Cells[idxRowSpc - cntSpcRow + 1, colOvvSpcName, idxRowSpc, colOvvSpcName].Merge = true;
                    worksheets.Cells[idxRowSpc - cntSpcRow + 1, colOvvSpcCO, idxRowSpc, colOvvSpcCO].Merge = true;
                    worksheets.Cells[idxRowSpc - cntSpcRow + 1, colOvvSpcHO, idxRowSpc, colOvvSpcHO].Merge = true;
                    worksheets.Cells[idxRowSpc - cntSpcRow + 1, colOvvSpcUO, idxRowSpc, colOvvSpcUO].Merge = true;
                    worksheets.Cells[idxRowSpc - cntSpcRow + 1, colOvvSpcTotal, idxRowSpc, colOvvSpcTotal].Merge = true;

                    using (ExcelRange h = worksheets.Cells[6, colOvvSpcName, (idxRowSpc), colOvvPathTotal])
                    {
                        h.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        h.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        h.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        h.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        h.Style.Font.Size = 14;
                        h.AutoFitColumns();
                    }
                    worksheets.Column(colOvvSpcName).Width = 15;
                    worksheets.Column(colOvvSpcCO).Width = 14;
                    worksheets.Column(colOvvSpcHO).Width = 14;
                    worksheets.Column(colOvvSpcUO).Width = 14;
                    worksheets.Column(colOvvSpcTotal).Width = 14;

                    worksheets.Column(colOvvPathName).Width = 35;
                    worksheets.Column(colOvvPathCO).Width = 14;
                    worksheets.Column(colOvvPathHO).Width = 14;
                    worksheets.Column(colOvvPathUO).Width = 14;
                    worksheets.Column(colOvvPathTotal).Width = 14;

                    worksheets.Row(6).Height = 25;
                    worksheets.Row(8).Height = 30;

                    worksheets.Column(colOvvSpcCO).Style.WrapText = true;
                    worksheets.Column(colOvvSpcHO).Style.WrapText = true;
                    worksheets.Column(colOvvSpcUO).Style.WrapText = true;
                    worksheets.Column(colOvvSpcTotal).Style.WrapText = true;

                    worksheets.Column(colOvvPathCO).Style.WrapText = true;
                    worksheets.Column(colOvvPathHO).Style.WrapText = true;
                    worksheets.Column(colOvvPathUO).Style.WrapText = true;
                    worksheets.Column(colOvvPathTotal).Style.WrapText = true;


                    worksheets.Cells[6, colOvvSpcName, (idxRowSpc), colOvvSpcName].Style.Font.Bold = true;
                    worksheets.Cells[6, 6, (idxRowSpc), 6].Style.Font.Bold = true;

                    using (ExcelRange h = worksheets.Cells[6, colOvvSpcName, (idxRowSpc), colOvvSpcTotal])
                    {
                        h.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    //------------------------   Sheet 2 : Pathogen NS ---------------------------------------
                    worksheets = package.Workbook.Worksheets.Add("Pathogen NS");

                    var objGlassPathogen = _service.GetGlassPathogenNSModel(model);

                    // Heder
                    worksheets.Cells[1, 1].Value = "Pathogen non-susceptibility";
                    using (ExcelRange h = worksheets.Cells[(1), 1, (1), (3)])
                    {
                        h.Style.Font.Bold = true;
                        h.Style.Font.Size = 14;
                    }

                    startRowidx = 1;
                    spc_code = "";
                    string org_code = "";
                    string spc_name = "";

                    ExcelBarChart chartPathogenNS;
                    ExcelRange xlsRangeName;
                    ExcelRange xlsRangeValue;
                    const int colPathPSAntiName = 1;
                    const int colPathPSNum = 2;
                    const int colPathPSPercentR = 3;
                    const int colPathPSPercentI = 4;
                    const int colPathPSPercentS = 5;
                    const int colPathPSPercentNS = 6;
                    const int colPathPSPercentR95CI = 7;
                    const int colPathPSPercentNS95CI = 8;

                    var idxRowStart = 1;
                    var idxend = 1;
                    var colorPink = Color.FromArgb(255, 165, 0);
                    var colorBlue = Color.FromArgb(108, 166, 205);
                    var colorLightBlue = Color.FromArgb(189, 215, 238);
                    var colorGray = Color.FromArgb(237, 237, 237);
                    foreach (var obj in objGlassPathogen)
                    {
                        if (spc_code != obj.spc_code || org_code != obj.org_code)
                        {
                            if (spc_code != "")
                            {
                                idxend = startRowidx;

                                chartPathogenNS = (ExcelBarChart)worksheets.Drawings.AddChart(spc_code + startRowidx.ToString(), OfficeOpenXml.Drawing.Chart.eChartType.BarClustered);
                                chartPathogenNS.Title.Text = obj.spc_name;
                                chartPathogenNS.Title.Font.Size = 14;
                                chartPathogenNS.Title.Font.Bold = true;

                                // X axis
                                //chartPathogenNS.Legend.Position = eLegendPosition.Left;
                                chartPathogenNS.Legend.Remove();
                                chartPathogenNS.XAxis.Title.Text = "Antibiotic";
                                chartPathogenNS.XAxis.Title.Rotation = 270;
                                chartPathogenNS.XAxis.Deleted = true;


                                // Y axis
                                chartPathogenNS.YAxis.Font.Size = 9;
                                chartPathogenNS.YAxis.Border.Fill.Style = eFillStyle.NoFill;
                                chartPathogenNS.YAxis.Title.Text = "Proportion of non-susceptible isolates (%)";
                                chartPathogenNS.YAxis.Font.Size = 10;
                                chartPathogenNS.YAxis.MaxValue = 100;

                                // Chart Area
                                chartPathogenNS.SetPosition(idxRowStart - 2, 0, 9, 0);
                                chartPathogenNS.SetSize(500, 250);
                                chartPathogenNS.DataLabel.ShowCategory = true;
                                chartPathogenNS.GapWidth = 50;
                                //chartPathogenNS.Border.LineStyle = eLineStyle.LongDashDot;
                                //chartPathogenNS.DataLabel.Position = eLabelPosition.Left;


                                xlsRangeValue = worksheets.Cells[idxRowStart + 1, colPathPSPercentNS, idxend, colPathPSPercentNS];
                                xlsRangeName = worksheets.Cells[idxRowStart + 1, colPathPSAntiName, idxend, colPathPSAntiName];

                                var ser1 = (ExcelChartSerie)(chartPathogenNS.Series.Add(xlsRangeValue, xlsRangeName));
                                ser1.Fill.Color = colorBlue;
                                //chartPathogenNS.VaryColors = true;

                                using (ExcelRange h = worksheets.Cells[idxRowStart, colPathPSPercentNS, idxend, colPathPSPercentNS])
                                {
                                    //h.Style.Font.Bold = true;
                                    h.Style.Font.Size = 11;
                                    h.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    h.Style.Fill.BackgroundColor.SetColor(colorLightBlue);
                                }
                            }


                            if (string.IsNullOrEmpty(spc_code)) startRowidx += 3; else startRowidx += 12;

                            var ovv = objGlass.Where(w => w.spc_code == obj.spc_code && w.org_code == obj.org_code).FirstOrDefault();
                            worksheets.Cells[startRowidx, colPathPSAntiName].Value = obj.org_name + '(' + obj.org_code + ')';
                            worksheets.Cells[startRowidx, colPathPSPercentR].Value = String.Format("{0}={1}", obj.spc_name, (ovv != null) ? ovv.org_total_origin : 0);
                            using (ExcelRange h = worksheets.Cells[startRowidx, colPathPSAntiName, startRowidx, colPathPSPercentR])
                            {
                                //h.Style.Font.Bold = true;
                                h.Style.Font.Size = 11;
                                h.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                h.Style.Fill.BackgroundColor.SetColor(colorPink);
                            }


                            startRowidx += 2;
                            worksheets.Cells[startRowidx, colPathPSAntiName].Value = "Antibiotic name";
                            worksheets.Cells[startRowidx, colPathPSNum].Value = "Number";
                            worksheets.Cells[startRowidx, colPathPSPercentR].Value = "%R";
                            worksheets.Cells[startRowidx, colPathPSPercentI].Value = "%I";
                            worksheets.Cells[startRowidx, colPathPSPercentS].Value = "%S";
                            worksheets.Cells[startRowidx, colPathPSPercentNS].Value = "Proportion of non-susceptible isolates (%)";
                            worksheets.Cells[startRowidx, colPathPSPercentR95CI].Value = "%R 95%C.I.";
                            worksheets.Cells[startRowidx, colPathPSPercentNS95CI].Value = "%NS 95%C.I.";
                            idxRowStart = startRowidx;
                        }

                        startRowidx += 1;
                        worksheets.Cells[startRowidx, colPathPSAntiName].Value = (string.IsNullOrEmpty(obj.anti_name)) ? obj.anti_code : obj.anti_name;
                        worksheets.Cells[startRowidx, colPathPSNum].Value = obj.total_drug_test;
                        worksheets.Cells[startRowidx, colPathPSPercentR].Value = obj.percent_r;
                        worksheets.Cells[startRowidx, colPathPSPercentI].Value = obj.percent_i;
                        worksheets.Cells[startRowidx, colPathPSPercentS].Value = obj.percent_s;
                        worksheets.Cells[startRowidx, colPathPSPercentNS].Value = obj.percent_ns;
                        worksheets.Cells[startRowidx, colPathPSPercentR95CI].Value = 0;
                        if (obj.total_drug_test != 0)
                        {
                            worksheets.Cells[startRowidx, colPathPSPercentNS95CI].Formula = string.Format("=1.96*(SQRT({0}*{1}/{2}))"
                                                                                                  , worksheets.Cells[startRowidx, colPathPSPercentS].Address
                                                                                                  , worksheets.Cells[startRowidx, colPathPSPercentNS].Address
                                                                                                  , worksheets.Cells[startRowidx, colPathPSNum].Address);
                        }


                        spc_code = obj.spc_code;
                        org_code = obj.org_code;
                        spc_name = obj.spc_name;
                    }

                    idxend = startRowidx;

                    chartPathogenNS = (ExcelBarChart)worksheets.Drawings.AddChart(spc_code + startRowidx.ToString(), OfficeOpenXml.Drawing.Chart.eChartType.BarClustered);
                    chartPathogenNS.Title.Text = spc_name;
                    chartPathogenNS.Title.Font.Size = 14;
                    chartPathogenNS.Title.Font.Bold = true;

                    // X axis
                    //chartPathogenNS.Legend.Position = eLegendPosition.Left;
                    chartPathogenNS.Legend.Remove();
                    chartPathogenNS.XAxis.Title.Text = "Antibiotic";
                    chartPathogenNS.XAxis.Title.Rotation = 270;
                    chartPathogenNS.XAxis.Deleted = true;

                    // Y axis
                    chartPathogenNS.YAxis.Font.Size = 9;
                    chartPathogenNS.YAxis.Border.Fill.Style = eFillStyle.NoFill;
                    chartPathogenNS.YAxis.Title.Text = "Proportion of non-susceptible isolates (%)";
                    chartPathogenNS.YAxis.Font.Size = 10;
                    chartPathogenNS.YAxis.MaxValue = 100;

                    // Chart Area
                    chartPathogenNS.SetPosition(idxRowStart - 2, -4, 9, 0);
                    //chartPathogenNS.SetSize(350, 150);
                    chartPathogenNS.SetSize(500, 250);
                    chartPathogenNS.DataLabel.ShowCategory = true;
                    chartPathogenNS.GapWidth = 50;
                    //chartPathogenNS.Fill.SolidFill();

                    xlsRangeValue = worksheets.Cells[idxRowStart + 1, 6, idxend, 6];
                    xlsRangeName = worksheets.Cells[idxRowStart + 1, 1, idxend, 1];

                    var seriesNS = (ExcelChartSerie)(chartPathogenNS.Series.Add(xlsRangeValue, xlsRangeName));
                    //chartPathogenNS.VaryColors = true;
                    seriesNS.Fill.Color = colorBlue;


                    using (ExcelRange h = worksheets.Cells[(idxRowStart), 6, (idxend), (6)])
                    {
                        h.Style.Font.Size = 11;
                        h.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        h.Style.Fill.BackgroundColor.SetColor(colorLightBlue);
                    }

                    worksheets.Column(colPathPSAntiName).Width = 25;

                    //------------------------   Sheet 3 : COHO ---------------------------------------
                    ExcelBarChart chartAntibiotic;
                    worksheets = package.Workbook.Worksheets.Add("COHO");

                    var objInfectSpcimen = _service.GlassInfectSpecimenModel(model);
                    var objInfectPathoAntiCombine = _service.GetGlassInfectPathAntiCombineModel(model);


                    worksheets.Cells[1, 1].Value = "Non-susceptibility pathogen-antimicrobial combination freqency";
                    using (ExcelRange h = worksheets.Cells[(1), 1, (1), (10)])
                    {
                        h.Style.Font.Bold = true;
                        h.Style.Font.Size = 14;
                    }

                    startRowidx = 1;
                    var idxRowPathogen = 1;
                    int idxRowAnti = 1;
                    spc_code = "";
                    var lstOrigin = new List<string>() { "CO", "HO" };

                    int colOrganism = 1;
                    const int colFreqOrganism = 4;
                    const int colAnti = 14;
                    const int colFreqAnti = 17;
                    var idxEndAnti = 1;
                    var idxStartAnti = 1;
                    var iRowChartStart = 1;
                    string strPathogen = "";
                    var rand = new System.Random();
                    var dicColor = new Dictionary<string, Color>();
                    var strTemp = "";
                    foreach (var obj in objInfectSpcimen)
                    {

                        var objPathogen = objInfectPathoAntiCombine.Where(s => s.spc_code == obj.spc_code)
                                                                   .OrderBy(o => o.org_code).ToList();
                        var maxGroup = objInfectPathoAntiCombine.Max(m => m.num_org);
                        maxGroup = maxGroup + 2;
                        var objFullFill = objPathogen.Where(w => w.num_org != (maxGroup)).ToList();

                        foreach (var off in objFullFill)
                        {
                            if (strTemp == off.org_code) { continue; }
                            //off.tempId = off.spc_code + 
                            var diff = (maxGroup) - off.num_org;
                            for (var i = 0; i < diff; i++)
                            {
                                GlassInfectPathAntiCombineDTO objEmpty = new GlassInfectPathAntiCombineDTO();
                                objEmpty.org_code = off.org_code;
                                if (!string.IsNullOrEmpty(off.org_name))
                                {
                                    objEmpty.org_name = off.org_name;
                                }
                                objEmpty.anti_code = "";
                                objEmpty.spc_code = off.spc_code;
                                objPathogen.Add(objEmpty);
                            }

                            strTemp = off.org_code;
                        }

                        objPathogen = objPathogen.Where(w => 0 == 0).OrderBy(o => o.org_code).ToList();
                        foreach (var origin in lstOrigin)
                        {
                            strPathogen = "";
                            //dicColor.Clear();
                            if (objPathogen.Count() == 0) { continue; }

                            //header group
                            idxRowPathogen += 2;
                            var strOriginName = (origin == "CO") ? "Community origin" : "Hospital origin";
                            worksheets.Cells[idxRowPathogen, colOrganism].Value = obj.spc_name + " - " + strOriginName;
                            using (ExcelRange h = worksheets.Cells[(idxRowPathogen), 1, (idxRowPathogen), (3)])
                            {
                                h.Style.Font.Bold = true;
                                h.Style.Font.Size = 11;
                                h.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                h.Style.Fill.BackgroundColor.SetColor(colorPink);
                            }

                            //header column
                            idxRowPathogen += 2;
                            worksheets.Cells[idxRowPathogen, colOrganism].Value = "Pathogen";
                            worksheets.Cells[idxRowPathogen, colFreqOrganism].Value = "Frequency of infection (per 100,000 tested patients)";

                            worksheets.Cells[idxRowPathogen, colAnti].Value = "Antibiotic";
                            worksheets.Cells[idxRowPathogen, colFreqAnti].Value = "Frequency of antibiotic resistant infection (per 100,000 tested patients)";

                            idxRowStart = idxRowPathogen + 1;
                            idxStartAnti = idxRowPathogen + 1;
                            idxRowAnti = idxRowPathogen;

                            // Detail                   
                            foreach (var objPath in objPathogen)
                            {
                                if (strPathogen == objPath.org_code)
                                {

                                    idxRowAnti += 1;
                                    worksheets.Cells[idxRowAnti, colAnti].Value = (string.IsNullOrEmpty(objPath.anti_name)) ? objPath.anti_code : objPath.anti_name;
                                    if (objPath.num_org != 0)
                                    {
                                        worksheets.Cells[idxRowAnti, colFreqAnti].Value = (origin == "CO") ? objPath.freq_co_anti : objPath.freq_hos_anti;
                                    }
                                    idxEndAnti = idxRowAnti;
                                    continue;
                                }

                                //detail Pathogen (Left)
                                idxRowPathogen += 1;
                                worksheets.Cells[idxRowPathogen, colOrganism].Value = (string.IsNullOrEmpty(objPath.org_name)) ? objPath.org_code : objPath.org_name;
                                worksheets.Cells[idxRowPathogen, colFreqOrganism].Value = (origin == "CO") ? objPath.freq_co_org : objPath.freq_hos_org;
                                idxend = idxRowPathogen;

                                //detail Anti (Right)
                                idxRowAnti += 1;
                                worksheets.Cells[idxRowAnti, colAnti].Value = (string.IsNullOrEmpty(objPath.anti_name)) ? objPath.anti_code : objPath.anti_name;
                                if (objPath.num_org != 0)
                                {
                                    worksheets.Cells[idxRowAnti, colFreqAnti].Value = (origin == "CO") ? objPath.freq_co_anti : objPath.freq_hos_anti;
                                }

                                idxEndAnti = idxRowAnti;
                                strPathogen = objPath.org_code;
                            }

                            //if (idxend > idxEndAnti) iRowChartStart = idxend; else iRowChartStart = idxEndAnti;
                            iRowChartStart = (idxend > idxEndAnti) ? idxend : idxEndAnti;
                            // Create Chart after last data of Pathogen or Organism shift 3 rows
                            iRowChartStart += 3;
                            // chart Pathogen
                            chartPathogenNS = (ExcelBarChart)worksheets.Drawings.AddChart(string.Format("{0}_Pathogen_{1}", obj.spc_code, origin), OfficeOpenXml.Drawing.Chart.eChartType.BarClustered);
                            chartPathogenNS.SetPosition(iRowChartStart, 0, 2, 0);
                            chartPathogenNS.SetSize(600, 450);
                            chartPathogenNS.GapWidth = 10;
                            chartPathogenNS.YAxis.Title.Text = "Frequency of infection (per 100,000 tested patients)";
                            chartPathogenNS.XAxis.RemoveGridlines();
                            chartPathogenNS.YAxis.RemoveGridlines();
                            chartPathogenNS.XAxis.Deleted = true;
                            chartPathogenNS.YAxis.Font.Size = 10;
                            chartPathogenNS.DataLabel.ShowValue = true;
                            xlsRangeValue = worksheets.Cells[idxRowStart, colFreqOrganism, idxend, colFreqOrganism];
                            xlsRangeName = worksheets.Cells[idxRowStart, colOrganism, idxend, colOrganism];
                            var seriesPathogen = (ExcelChartSerie)(chartPathogenNS.Series.Add(xlsRangeValue, xlsRangeName));
                            //chartPathogenNS.VaryColors = true;


                            // chart Antibiotic                      
                            chartAntibiotic = (ExcelBarChart)worksheets.Drawings.AddChart(string.Format("{0}_Antibiotic_{1}", obj.spc_code, origin), OfficeOpenXml.Drawing.Chart.eChartType.BarClustered);
                            chartAntibiotic.SetPosition(iRowChartStart, 0, colAnti - 2, 0);
                            chartAntibiotic.SetSize(600, 450);
                            chartAntibiotic.GapWidth = 30;
                            chartAntibiotic.XAxis.Deleted = true;

                            chartAntibiotic.YAxis.Title.Text = "Frequency of antibiotic resistant infection (per 100,000 tested patients)";
                            chartAntibiotic.YAxis.Font.Size = 10;
                            //chartAntibiotic.XAxis.RemoveGridlines();
                            //chartAntibiotic.YAxis.RemoveGridlines();
                            var xlsRangeXValue = worksheets.Cells[idxStartAnti, colFreqAnti, idxEndAnti, colFreqAnti];
                            var xlsRangeYName = worksheets.Cells[idxStartAnti, colAnti, idxEndAnti, colAnti];
                            var seriesAnti = (ExcelChartSerie)(chartAntibiotic.Series.Add(xlsRangeXValue, xlsRangeYName));
                            //chartAntibiotic.VaryColors = true;


                            // ------------------------------ XML -----------------------------------
                            var wsBar = chartPathogenNS.WorkSheet;
                            var wsAnti = chartAntibiotic.WorkSheet;
                            var xdocBar = chartPathogenNS.ChartXml;
                            var xdocAnti = chartAntibiotic.ChartXml;
                            var nsmBar = new System.Xml.XmlNamespaceManager(xdocBar.NameTable);
                            var nsmAnti = new System.Xml.XmlNamespaceManager(xdocAnti.NameTable);

                            string schemaDrawings = "http://schemas.openxmlformats.org/drawingml/2006/main";
                            string schemaChart = "http://schemas.openxmlformats.org/drawingml/2006/chart";

                            nsmBar.AddNamespace("c", schemaChart);
                            nsmBar.AddNamespace("a", schemaDrawings);
                            nsmAnti.AddNamespace("c", schemaChart);
                            nsmAnti.AddNamespace("a", schemaDrawings);

                            const string COLUMN_PATH = "c:chartSpace/c:chart/c:plotArea/c:barChart/c:ser";

                            var nodeBar = chartPathogenNS.ChartXml.SelectSingleNode(COLUMN_PATH, nsmBar);
                            var nodeAnti = chartAntibiotic.ChartXml.SelectSingleNode(COLUMN_PATH, nsmAnti);
                            Color colorRand;
                            strPathogen = "";

                            var objDistPathogen = objPathogen.Select(w => w.org_code).Distinct().ToList();

                            for (var i = 0; i < objDistPathogen.Count(); i++)
                            {


                                //Create the data point node
                                var dPt = xdocBar.CreateElement("dPt", schemaChart);

                                var idx = dPt.AppendChild(xdocBar.CreateElement("idx", schemaChart));
                                var valattrib = idx.Attributes.Append(xdocBar.CreateAttribute("val"));
                                valattrib.Value = i.ToString(CultureInfo.InvariantCulture);
                                nodeBar.AppendChild(dPt);

                                //Add the solid fill node
                                var spPr = xdocBar.CreateElement("spPr", schemaChart);
                                var solidFill = spPr.AppendChild(xdocBar.CreateElement("solidFill", schemaDrawings));
                                var srgbClr = solidFill.AppendChild(xdocBar.CreateElement("srgbClr", schemaDrawings));
                                valattrib = srgbClr.Attributes.Append(xdocBar.CreateAttribute("val"));

                                ////Set the color
                                colorRand = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                                //colorRand = Color.FromArgb(0, 0, 0);
                                if (!dicColor.ContainsKey(objDistPathogen[i]))
                                {
                                    dicColor.Add(objDistPathogen[i], colorRand);
                                }
                                else
                                {
                                    colorRand = dicColor[objDistPathogen[i]];
                                }

                                valattrib.Value = ColorTranslator.ToHtml(colorRand).Replace("#", String.Empty);
                                dPt.AppendChild(spPr);
                                //}
                            }

                            // ------- Anti Chart------

                            for (var i = 0; i < objPathogen.Count(); i++)
                            {
                                //Create the data point node
                                var dPtAnti = xdocAnti.CreateElement("dPt", schemaChart);

                                var idxAnti = dPtAnti.AppendChild(xdocAnti.CreateElement("idx", schemaChart));
                                var valattribAnti = idxAnti.Attributes.Append(xdocAnti.CreateAttribute("val"));
                                valattribAnti.Value = i.ToString(CultureInfo.InvariantCulture);
                                nodeAnti.AppendChild(dPtAnti);

                                //Add the solid fill node
                                var spPrAnti = xdocAnti.CreateElement("spPr", schemaChart);
                                var solidFillAnti = spPrAnti.AppendChild(xdocAnti.CreateElement("solidFill", schemaDrawings));
                                var srgbClrAnti = solidFillAnti.AppendChild(xdocAnti.CreateElement("srgbClr", schemaDrawings));
                                valattribAnti = srgbClrAnti.Attributes.Append(xdocAnti.CreateAttribute("val"));


                                if (dicColor.ContainsKey(objPathogen[i].org_code))
                                {
                                    colorRand = dicColor[objPathogen[i].org_code];
                                }
                                else
                                {
                                    colorRand = Color.FromArgb(0, 0, 0);
                                }
                                //colorRand = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                                valattribAnti.Value = ColorTranslator.ToHtml(colorRand).Replace("#", String.Empty);
                                dPtAnti.AppendChild(spPrAnti);

                            }
                            // shift 28 rows untill chart start position row
                            iRowChartStart += 28;
                            idxRowPathogen = iRowChartStart;
                            idxRowAnti = iRowChartStart;
                        }
                    }

                    //--------------------------- End Sheet3 -----------------------------------

                    package.Save(); //Save the workbook.

                }

                var ServerFilePath = Path.Combine(sWebRootFolder, sFileName);
                var memory = new MemoryStream();
                string contentType = model.analyze_file_type;
                if (System.IO.File.Exists(ServerFilePath))
                {
                    using (var stream = new FileStream(ServerFilePath, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }
                }
                else
                {
                    Response.StatusCode = 404;
                }

                memory.Position = 0;
                return File(memory, contentType, Path.GetFileName(ServerFilePath));
                log.MethodFinish();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return new EmptyResult();
        }

        [HttpPost]
        [Route("api/Glass/GenerateAnalyzeFile")]
        public async Task<IActionResult> GenerateAnalyzeFile([FromBody]GlassFileListDTO model)
        {
            try
            {
                log.MethodStart();
                //GlassFileListDTO model = _service.GetGlassPublicFileListData().Where(w => w.who_flag == false).FirstOrDefault();
                
                int ParamYear = model.year;
                string ParamAreaHealth = string.Format("Area Health No. {0}" , model.arh_code); 
               
                //string sWebRootFolder = @"D:\Project\ALISS_PROJECT\06_Report\ALISS.ANTIBIOGRAM.Api\" + model.analyze_file_path.Remove(0, 1);
           
                var sFileName = model.analyze_file_name;
                var sFilePdfName = model.analyze_file_name.Replace(".xlsx", "pdf");
                var strReportPath = "";
        
                // Find ALISS Process Path
                List<ParameterDTO> objParamList = new List<ParameterDTO>();
                var searchModel = new ParameterDTO() { prm_code_major = "RPT_PROCESS_PATH" };

                objParamList = _service.GetGlassReportPath(searchModel);

                if (objParamList.FirstOrDefault(x => x.prm_code_minor == "PATH") != null)
                {
                    strReportPath = objParamList.FirstOrDefault(x => x.prm_code_minor == "PATH").prm_value;
                }
                else
                {
                    Response.StatusCode = 404;
                    return new EmptyResult();
                }
                // Find ALISS Process Path

                string sWebRootFolder = Path.Combine(strReportPath, model.analyze_file_path.Remove(0, 1));
                //sWebRootFolder = @"D:\ALISS_ProcessFile\GLASS\20200730_01_GLASS";
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                FileInfo filePdf = new FileInfo(Path.Combine(sWebRootFolder, sFilePdfName));

                if (file.Exists)
                {
                    file.Delete();
                    file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                }

                var objGlass = _service.GetGlassInfectOverviewModel(model);
                string spc_code = "";
                int startRowidx = 8;
                int idxRowSpc = startRowidx;
                const int colOvvSpcName = 1;
                const int colOvvSpcCO = 2;
                const int colOvvSpcHO = 3;
                const int colOvvSpcUO = 4;
                const int colOvvSpcTotal = 5;

                const int colOvvPathName = 6;
                const int colOvvPathCO = 7;
                const int colOvvPathHO = 8;
                const int colOvvPathUO = 9;
                const int colOvvPathTotal = 10;

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelWorksheet worksheets;
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ////------------------------   Sheet 1 : Data Glass overview -----------------------------------------------
                    ///
                    worksheets = package.Workbook.Worksheets.Add("Data GLASS overview");

                    worksheets.Cells[1, 1].Value = "Data GLASS overview";
                    using (ExcelRange h = worksheets.Cells[(1), 1, (1), (3)])
                    {
                        h.Style.Font.Bold = true;
                        h.Style.Font.Size = 20;
                    }
                    worksheets.Cells[2, 1].Value = string.Format("January-December {0} - {1}", ParamYear, ParamAreaHealth);
                    using (ExcelRange h = worksheets.Cells[(2), 1, (2), (5)])
                    {
                        h.Style.Font.Bold = true;
                        h.Style.Font.Size = 16;

                    }

                    using (ExcelRange h = worksheets.Cells[(6), colOvvSpcName, (8), (colOvvPathTotal)])
                    {
                        h.Style.Font.Bold = true;
                        h.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        h.Style.Fill.BackgroundColor.SetColor(1, 220, 230, 241);
                        h.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    worksheets.Cells[6, colOvvSpcName].Value = "Specimen";
                    worksheets.Cells[6, colOvvSpcName, 8, colOvvSpcName].Merge = true;

                    worksheets.Cells[6, colOvvSpcCO].Value = "Number of test pateints";
                    worksheets.Cells[6, colOvvSpcCO, 6, colOvvSpcTotal].Merge = true;

                    worksheets.Cells[7, colOvvSpcCO].Value = "Community Origin";
                    worksheets.Cells[7, colOvvSpcCO, 8, colOvvSpcCO].Merge = true;
                    worksheets.Cells[7, colOvvSpcHO].Value = "Hospital Origin";
                    worksheets.Cells[7, colOvvSpcHO, 8, colOvvSpcHO].Merge = true;
                    worksheets.Cells[7, colOvvSpcUO].Value = "Unknow Origin";
                    worksheets.Cells[7, colOvvSpcUO, 8, colOvvSpcUO].Merge = true;
                    worksheets.Cells[7, colOvvSpcTotal].Value = "Total";
                    worksheets.Cells[7, colOvvSpcTotal, 8, colOvvSpcTotal].Merge = true;

                    worksheets.Cells[6, colOvvPathName].Value = "Pathogens";
                    worksheets.Cells[6, colOvvPathName, 8, colOvvPathName].Merge = true;

                    worksheets.Cells[6, colOvvPathCO].Value = "Number of patients with positive samples";
                    worksheets.Cells[6, colOvvPathCO, 6, colOvvPathTotal].Merge = true;

                    worksheets.Cells[7, colOvvPathCO].Value = "Community Origin";
                    worksheets.Cells[7, colOvvPathCO, 8, colOvvPathCO].Merge = true;
                    worksheets.Cells[7, colOvvPathHO].Value = "Hospital Origin";
                    worksheets.Cells[7, colOvvPathHO, 8, colOvvPathHO].Merge = true;
                    worksheets.Cells[7, colOvvPathUO].Value = "Unknow Origin";
                    worksheets.Cells[7, colOvvPathUO, 8, colOvvPathUO].Merge = true;
                    worksheets.Cells[7, colOvvPathTotal].Value = "Total";
                    worksheets.Cells[7, colOvvPathTotal, 8, colOvvPathTotal].Merge = true;

                    var cntSpcRow = 0;
                    foreach (var obj in objGlass)
                    {
                        idxRowSpc += 1;

                        if (spc_code != "" && spc_code != obj.spc_code)
                        {
                            worksheets.Cells[idxRowSpc - cntSpcRow, colOvvSpcName, idxRowSpc - 1, colOvvSpcName].Merge = true;
                            worksheets.Cells[idxRowSpc - cntSpcRow, colOvvSpcCO, idxRowSpc - 1, colOvvSpcCO].Merge = true;
                            worksheets.Cells[idxRowSpc - cntSpcRow, colOvvSpcHO, idxRowSpc - 1, colOvvSpcHO].Merge = true;
                            worksheets.Cells[idxRowSpc - cntSpcRow, colOvvSpcUO, idxRowSpc - 1, colOvvSpcUO].Merge = true;
                            worksheets.Cells[idxRowSpc - cntSpcRow, colOvvSpcTotal, idxRowSpc - 1, colOvvSpcTotal].Merge = true;

                            cntSpcRow = 0;
                        }

                        worksheets.Cells[idxRowSpc, colOvvSpcName].Value = obj.spc_name;
                        worksheets.Cells[idxRowSpc, colOvvSpcCO].Value = obj.spc_co_origin;
                        worksheets.Cells[idxRowSpc, colOvvSpcHO].Value = obj.spc_hos_origin;
                        worksheets.Cells[idxRowSpc, colOvvSpcUO].Value = obj.spc_unk_origin;
                        worksheets.Cells[idxRowSpc, colOvvSpcTotal].Value = obj.spc_total_origin;

                        worksheets.Cells[idxRowSpc, 6].Value = obj.org_name;
                        worksheets.Cells[idxRowSpc, 7].Value = obj.org_co_origin;
                        worksheets.Cells[idxRowSpc, 8].Value = obj.org_hos_origin;
                        worksheets.Cells[idxRowSpc, 9].Value = obj.org_unk_origin;
                        worksheets.Cells[idxRowSpc, 10].Value = obj.org_total_origin;

                        spc_code = obj.spc_code;
                        cntSpcRow += 1;
                    }

                    worksheets.Cells[idxRowSpc - cntSpcRow + 1, colOvvSpcName, idxRowSpc, colOvvSpcName].Merge = true;
                    worksheets.Cells[idxRowSpc - cntSpcRow + 1, colOvvSpcCO, idxRowSpc, colOvvSpcCO].Merge = true;
                    worksheets.Cells[idxRowSpc - cntSpcRow + 1, colOvvSpcHO, idxRowSpc, colOvvSpcHO].Merge = true;
                    worksheets.Cells[idxRowSpc - cntSpcRow + 1, colOvvSpcUO, idxRowSpc, colOvvSpcUO].Merge = true;
                    worksheets.Cells[idxRowSpc - cntSpcRow + 1, colOvvSpcTotal, idxRowSpc, colOvvSpcTotal].Merge = true;

                    using (ExcelRange h = worksheets.Cells[6, colOvvSpcName, (idxRowSpc), colOvvPathTotal])
                    {
                        h.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        h.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        h.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        h.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        h.Style.Font.Size = 14;
                        h.AutoFitColumns();
                    }
                    worksheets.Column(colOvvSpcName).Width = 15;
                    worksheets.Column(colOvvSpcCO).Width = 14;
                    worksheets.Column(colOvvSpcHO).Width = 14;
                    worksheets.Column(colOvvSpcUO).Width = 14;
                    worksheets.Column(colOvvSpcTotal).Width = 14;

                    worksheets.Column(colOvvPathName).Width = 35;
                    worksheets.Column(colOvvPathCO).Width = 14;
                    worksheets.Column(colOvvPathHO).Width = 14;
                    worksheets.Column(colOvvPathUO).Width = 14;
                    worksheets.Column(colOvvPathTotal).Width = 14;

                    worksheets.Row(6).Height = 25;
                    worksheets.Row(8).Height = 30;

                    worksheets.Column(colOvvSpcCO).Style.WrapText = true;
                    worksheets.Column(colOvvSpcHO).Style.WrapText = true;
                    worksheets.Column(colOvvSpcUO).Style.WrapText = true;
                    worksheets.Column(colOvvSpcTotal).Style.WrapText = true;

                    worksheets.Column(colOvvPathCO).Style.WrapText = true;
                    worksheets.Column(colOvvPathHO).Style.WrapText = true;
                    worksheets.Column(colOvvPathUO).Style.WrapText = true;
                    worksheets.Column(colOvvPathTotal).Style.WrapText = true;


                    worksheets.Cells[6, colOvvSpcName, (idxRowSpc), colOvvSpcName].Style.Font.Bold = true;
                    worksheets.Cells[6, 6, (idxRowSpc), 6].Style.Font.Bold = true;

                    using (ExcelRange h = worksheets.Cells[6, colOvvSpcName, (idxRowSpc), colOvvSpcTotal])
                    {
                        h.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        h.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    //------------------------   Sheet 2 : Pathogen NS ---------------------------------------
                    worksheets = package.Workbook.Worksheets.Add("Pathogen NS");

                    var objGlassPathogen = _service.GetGlassPathogenNSModel(model);

                    // Heder
                    worksheets.Cells[1, 1].Value = "Pathogen non-susceptibility";
                    using (ExcelRange h = worksheets.Cells[(1), 1, (1), (3)])
                    {
                        h.Style.Font.Bold = true;
                        h.Style.Font.Size = 14;
                    }

                    startRowidx = 1;
                    spc_code = "";
                    string org_code = "";
                    string spc_name = "";

                    ExcelBarChart chartPathogenNS;
                    ExcelRange xlsRangeName;
                    ExcelRange xlsRangeValue;
                    const int colPathPSAntiName = 1;
                    const int colPathPSNum = 2;
                    const int colPathPSPercentR = 3;
                    const int colPathPSPercentI = 4;
                    const int colPathPSPercentS = 5;
                    const int colPathPSPercentNS = 6;
                    const int colPathPSPercentR95CI = 7;
                    const int colPathPSPercentNS95CI = 8;

                    var idxRowStart = 1;
                    var idxend = 1;
                    var colorPink = Color.FromArgb(255, 165, 0);
                    var colorBlue = Color.FromArgb(108, 166, 205);
                    var colorLightBlue = Color.FromArgb(189, 215, 238);
                    var colorGray = Color.FromArgb(237, 237, 237);
                    foreach (var obj in objGlassPathogen)
                    {
                        if (spc_code != obj.spc_code || org_code != obj.org_code)
                        {
                            if (spc_code != "")
                            {
                                idxend = startRowidx;

                                chartPathogenNS = (ExcelBarChart)worksheets.Drawings.AddChart(spc_code + startRowidx.ToString(), OfficeOpenXml.Drawing.Chart.eChartType.BarClustered);
                                chartPathogenNS.Title.Text = obj.spc_name;
                                chartPathogenNS.Title.Font.Size = 14;
                                chartPathogenNS.Title.Font.Bold = true;

                                // X axis
                                //chartPathogenNS.Legend.Position = eLegendPosition.Left;
                                chartPathogenNS.Legend.Remove();
                                chartPathogenNS.XAxis.Title.Text = "Antibiotic";
                                chartPathogenNS.XAxis.Title.Rotation = 270;
                                chartPathogenNS.XAxis.Deleted = true;


                                // Y axis
                                chartPathogenNS.YAxis.Font.Size = 9;
                                chartPathogenNS.YAxis.Border.Fill.Style = eFillStyle.NoFill;
                                chartPathogenNS.YAxis.Title.Text = "Proportion of non-susceptible isolates (%)";
                                chartPathogenNS.YAxis.Font.Size = 10;
                                chartPathogenNS.YAxis.MaxValue = 100;

                                // Chart Area
                                chartPathogenNS.SetPosition(idxRowStart - 2, 0, 9, 0);
                                chartPathogenNS.SetSize(500, 250);
                                chartPathogenNS.DataLabel.ShowCategory = true;
                                chartPathogenNS.GapWidth = 50;
                                //chartPathogenNS.Border.LineStyle = eLineStyle.LongDashDot;
                                //chartPathogenNS.DataLabel.Position = eLabelPosition.Left;


                                xlsRangeValue = worksheets.Cells[idxRowStart + 1, colPathPSPercentNS, idxend, colPathPSPercentNS];
                                xlsRangeName = worksheets.Cells[idxRowStart + 1, colPathPSAntiName, idxend, colPathPSAntiName];

                                var ser1 = (ExcelChartSerie)(chartPathogenNS.Series.Add(xlsRangeValue, xlsRangeName));
                                ser1.Fill.Color = colorBlue;
                                //chartPathogenNS.VaryColors = true;

                                using (ExcelRange h = worksheets.Cells[idxRowStart, colPathPSPercentNS, idxend, colPathPSPercentNS])
                                {
                                    //h.Style.Font.Bold = true;
                                    h.Style.Font.Size = 11;
                                    h.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    h.Style.Fill.BackgroundColor.SetColor(colorLightBlue);
                                }
                            }


                            if (string.IsNullOrEmpty(spc_code)) startRowidx += 3; else startRowidx += 12;

                            var ovv = objGlass.Where(w => w.spc_code == obj.spc_code && w.org_code == obj.org_code).FirstOrDefault();
                            worksheets.Cells[startRowidx, colPathPSAntiName].Value = obj.org_name + '(' + obj.org_code + ')';
                            worksheets.Cells[startRowidx, colPathPSPercentR].Value = String.Format("{0}={1}", obj.spc_name, (ovv != null)? ovv.org_total_origin : 0);
                            using (ExcelRange h = worksheets.Cells[startRowidx, colPathPSAntiName, startRowidx, colPathPSPercentR])
                            {
                                //h.Style.Font.Bold = true;
                                h.Style.Font.Size = 11;
                                h.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                h.Style.Fill.BackgroundColor.SetColor(colorPink);
                            }


                            startRowidx += 2;
                            worksheets.Cells[startRowidx, colPathPSAntiName].Value = "Antibiotic name";
                            worksheets.Cells[startRowidx, colPathPSNum].Value = "Number";
                            worksheets.Cells[startRowidx, colPathPSPercentR].Value = "%R";
                            worksheets.Cells[startRowidx, colPathPSPercentI].Value = "%I";
                            worksheets.Cells[startRowidx, colPathPSPercentS].Value = "%S";
                            worksheets.Cells[startRowidx, colPathPSPercentNS].Value = "Proportion of non-susceptible isolates (%)";
                            worksheets.Cells[startRowidx, colPathPSPercentR95CI].Value = "%R 95%C.I.";
                            worksheets.Cells[startRowidx, colPathPSPercentNS95CI].Value = "%NS 95%C.I.";
                            idxRowStart = startRowidx;
                        }

                        startRowidx += 1;
                        worksheets.Cells[startRowidx, colPathPSAntiName].Value = (string.IsNullOrEmpty(obj.anti_name)) ? obj.anti_code : obj.anti_name;
                        worksheets.Cells[startRowidx, colPathPSNum].Value = obj.total_drug_test;
                        worksheets.Cells[startRowidx, colPathPSPercentR].Value = obj.percent_r;
                        worksheets.Cells[startRowidx, colPathPSPercentI].Value = obj.percent_i;
                        worksheets.Cells[startRowidx, colPathPSPercentS].Value = obj.percent_s;
                        worksheets.Cells[startRowidx, colPathPSPercentNS].Value = obj.percent_ns;
                        worksheets.Cells[startRowidx, colPathPSPercentR95CI].Value = 0;
                        if (obj.total_drug_test != 0)
                        {
                            worksheets.Cells[startRowidx, colPathPSPercentNS95CI].Formula = string.Format("=1.96*(SQRT({0}*{1}/{2}))"
                                                                                                  , worksheets.Cells[startRowidx, colPathPSPercentS].Address
                                                                                                  , worksheets.Cells[startRowidx, colPathPSPercentNS].Address
                                                                                                  , worksheets.Cells[startRowidx, colPathPSNum].Address);
                        }


                        spc_code = obj.spc_code;
                        org_code = obj.org_code;
                        spc_name = obj.spc_name;
                    }

                    idxend = startRowidx;

                    chartPathogenNS = (ExcelBarChart)worksheets.Drawings.AddChart(spc_code + startRowidx.ToString(), OfficeOpenXml.Drawing.Chart.eChartType.BarClustered);
                    chartPathogenNS.Title.Text = spc_name;
                    chartPathogenNS.Title.Font.Size = 14;
                    chartPathogenNS.Title.Font.Bold = true;

                    // X axis
                    //chartPathogenNS.Legend.Position = eLegendPosition.Left;
                    chartPathogenNS.Legend.Remove();
                    chartPathogenNS.XAxis.Title.Text = "Antibiotic";
                    chartPathogenNS.XAxis.Title.Rotation = 270;
                    chartPathogenNS.XAxis.Deleted = true;

                    // Y axis
                    chartPathogenNS.YAxis.Font.Size = 9;
                    chartPathogenNS.YAxis.Border.Fill.Style = eFillStyle.NoFill;
                    chartPathogenNS.YAxis.Title.Text = "Proportion of non-susceptible isolates (%)";
                    chartPathogenNS.YAxis.Font.Size = 10;
                    chartPathogenNS.YAxis.MaxValue = 100;

                    // Chart Area
                    chartPathogenNS.SetPosition(idxRowStart - 2, -4, 9, 0);
                    //chartPathogenNS.SetSize(350, 150);
                    chartPathogenNS.SetSize(500, 250);
                    chartPathogenNS.DataLabel.ShowCategory = true;
                    chartPathogenNS.GapWidth = 50;
                    //chartPathogenNS.Fill.SolidFill();

                    xlsRangeValue = worksheets.Cells[idxRowStart + 1, 6, idxend, 6];
                    xlsRangeName = worksheets.Cells[idxRowStart + 1, 1, idxend, 1];

                    var seriesNS = (ExcelChartSerie)(chartPathogenNS.Series.Add(xlsRangeValue, xlsRangeName));
                    //chartPathogenNS.VaryColors = true;
                    seriesNS.Fill.Color = colorBlue;


                    using (ExcelRange h = worksheets.Cells[(idxRowStart), 6, (idxend), (6)])
                    {
                        h.Style.Font.Size = 11;
                        h.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        h.Style.Fill.BackgroundColor.SetColor(colorLightBlue);
                    }

                    worksheets.Column(colPathPSAntiName).Width = 25;

                    //------------------------   Sheet 3 : COHO ---------------------------------------
                    ExcelBarChart chartAntibiotic;
                    worksheets = package.Workbook.Worksheets.Add("COHO");

                    var objInfectSpcimen = _service.GlassInfectSpecimenModel(model);
                    var objInfectPathoAntiCombine = _service.GetGlassInfectPathAntiCombineModel(model);


                    worksheets.Cells[1, 1].Value = "Non-susceptibility pathogen-antimicrobial combination freqency";
                    using (ExcelRange h = worksheets.Cells[(1), 1, (1), (10)])
                    {
                        h.Style.Font.Bold = true;
                        h.Style.Font.Size = 14;
                    }

                    startRowidx = 1;
                    var idxRowPathogen = 1;
                    int idxRowAnti = 1;
                    spc_code = "";
                    var lstOrigin = new List<string>() { "CO", "HO" };

                    int colOrganism = 1;
                    const int colFreqOrganism = 4;
                    const int colAnti = 14;
                    const int colFreqAnti = 17;
                    var idxEndAnti = 1;
                    var idxStartAnti = 1;
                    var iRowChartStart = 1;
                    string strPathogen = "";
                    var rand = new System.Random();
                    var dicColor = new Dictionary<string, Color>();
                    var strTemp = "";
                    foreach (var obj in objInfectSpcimen)
                    {

                        var objPathogen = objInfectPathoAntiCombine.Where(s => s.spc_code == obj.spc_code)
                                                                   .OrderBy(o => o.org_code).ToList();
                        var maxGroup = objInfectPathoAntiCombine.Max(m => m.num_org);
                        maxGroup = maxGroup + 2;
                        var objFullFill = objPathogen.Where(w => w.num_org != (maxGroup)).ToList();

                        foreach (var off in objFullFill)
                        {
                            if (strTemp == off.org_code) { continue; }
                            //off.tempId = off.spc_code + 
                            var diff = (maxGroup) - off.num_org;
                            for (var i = 0; i < diff; i++)
                            {
                                GlassInfectPathAntiCombineDTO objEmpty = new GlassInfectPathAntiCombineDTO();
                                objEmpty.org_code = off.org_code;
                                if (!string.IsNullOrEmpty(off.org_name))
                                {
                                    objEmpty.org_name = off.org_name;
                                }
                                objEmpty.anti_code = "";
                                objEmpty.spc_code = off.spc_code;
                                objPathogen.Add(objEmpty);
                            }

                            strTemp = off.org_code;
                        }

                        objPathogen = objPathogen.Where(w => 0 == 0).OrderBy(o => o.org_code).ToList();
                        foreach (var origin in lstOrigin)
                        {
                            strPathogen = "";
                            //dicColor.Clear();
                            if (objPathogen.Count() == 0) { continue; }

                            //header group
                            idxRowPathogen += 2;
                            var strOriginName = (origin == "CO") ? "Community origin" : "Hospital origin";
                            worksheets.Cells[idxRowPathogen, colOrganism].Value = obj.spc_name + " - " + strOriginName;
                            using (ExcelRange h = worksheets.Cells[(idxRowPathogen), 1, (idxRowPathogen), (3)])
                            {
                                h.Style.Font.Bold = true;
                                h.Style.Font.Size = 11;
                                h.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                h.Style.Fill.BackgroundColor.SetColor(colorPink);
                            }

                            //header column
                            idxRowPathogen += 2;
                            worksheets.Cells[idxRowPathogen, colOrganism].Value = "Pathogen";
                            worksheets.Cells[idxRowPathogen, colFreqOrganism].Value = "Frequency of infection (per 100,000 tested patients)";

                            worksheets.Cells[idxRowPathogen, colAnti].Value = "Antibiotic";
                            worksheets.Cells[idxRowPathogen, colFreqAnti].Value = "Frequency of antibiotic resistant infection (per 100,000 tested patients)";

                            idxRowStart = idxRowPathogen + 1;
                            idxStartAnti = idxRowPathogen + 1;
                            idxRowAnti = idxRowPathogen;

                            // Detail                   
                            foreach (var objPath in objPathogen)
                            {
                                if (strPathogen == objPath.org_code)
                                {

                                    idxRowAnti += 1;
                                    worksheets.Cells[idxRowAnti, colAnti].Value = (string.IsNullOrEmpty(objPath.anti_name)) ? objPath.anti_code : objPath.anti_name;
                                    if (objPath.num_org != 0)
                                    {
                                        worksheets.Cells[idxRowAnti, colFreqAnti].Value = (origin == "CO") ? objPath.freq_co_anti : objPath.freq_hos_anti;
                                    }
                                    idxEndAnti = idxRowAnti;
                                    continue;
                                }

                                //detail Pathogen (Left)
                                idxRowPathogen += 1;
                                worksheets.Cells[idxRowPathogen, colOrganism].Value = (string.IsNullOrEmpty(objPath.org_name)) ? objPath.org_code : objPath.org_name;
                                worksheets.Cells[idxRowPathogen, colFreqOrganism].Value = (origin == "CO") ? objPath.freq_co_org : objPath.freq_hos_org;
                                idxend = idxRowPathogen;

                                //detail Anti (Right)
                                idxRowAnti += 1;
                                worksheets.Cells[idxRowAnti, colAnti].Value = (string.IsNullOrEmpty(objPath.anti_name)) ? objPath.anti_code : objPath.anti_name;
                                if (objPath.num_org != 0)
                                {
                                    worksheets.Cells[idxRowAnti, colFreqAnti].Value = (origin == "CO") ? objPath.freq_co_anti : objPath.freq_hos_anti;
                                }

                                idxEndAnti = idxRowAnti;
                                strPathogen = objPath.org_code;
                            }

                            //if (idxend > idxEndAnti) iRowChartStart = idxend; else iRowChartStart = idxEndAnti;
                            iRowChartStart = (idxend > idxEndAnti) ? idxend : idxEndAnti;
                            // Create Chart after last data of Pathogen or Organism shift 3 rows
                            iRowChartStart += 3;
                            // chart Pathogen
                            chartPathogenNS = (ExcelBarChart)worksheets.Drawings.AddChart(string.Format("{0}_Pathogen_{1}", obj.spc_code, origin), OfficeOpenXml.Drawing.Chart.eChartType.BarClustered);
                            chartPathogenNS.SetPosition(iRowChartStart, 0, 2, 0);
                            chartPathogenNS.SetSize(600, 450);
                            chartPathogenNS.GapWidth = 10;
                            chartPathogenNS.YAxis.Title.Text = "Frequency of infection (per 100,000 tested patients)";
                            chartPathogenNS.XAxis.RemoveGridlines();
                            chartPathogenNS.YAxis.RemoveGridlines();
                            chartPathogenNS.XAxis.Deleted = true;
                            chartPathogenNS.YAxis.Font.Size = 10;
                            chartPathogenNS.DataLabel.ShowValue = true;
                            xlsRangeValue = worksheets.Cells[idxRowStart, colFreqOrganism, idxend, colFreqOrganism];
                            xlsRangeName = worksheets.Cells[idxRowStart, colOrganism, idxend, colOrganism];
                            var seriesPathogen = (ExcelChartSerie)(chartPathogenNS.Series.Add(xlsRangeValue, xlsRangeName));
                            //chartPathogenNS.VaryColors = true;


                            // chart Antibiotic                      
                            chartAntibiotic = (ExcelBarChart)worksheets.Drawings.AddChart(string.Format("{0}_Antibiotic_{1}", obj.spc_code, origin), OfficeOpenXml.Drawing.Chart.eChartType.BarClustered);
                            chartAntibiotic.SetPosition(iRowChartStart, 0, colAnti - 2, 0);
                            chartAntibiotic.SetSize(600, 450);
                            chartAntibiotic.GapWidth = 30;
                            chartAntibiotic.XAxis.Deleted = true;

                            chartAntibiotic.YAxis.Title.Text = "Frequency of antibiotic resistant infection (per 100,000 tested patients)";
                            chartAntibiotic.YAxis.Font.Size = 10;
                            //chartAntibiotic.XAxis.RemoveGridlines();
                            //chartAntibiotic.YAxis.RemoveGridlines();
                            var xlsRangeXValue = worksheets.Cells[idxStartAnti, colFreqAnti, idxEndAnti, colFreqAnti];
                            var xlsRangeYName = worksheets.Cells[idxStartAnti, colAnti, idxEndAnti, colAnti];
                            var seriesAnti = (ExcelChartSerie)(chartAntibiotic.Series.Add(xlsRangeXValue, xlsRangeYName));
                            //chartAntibiotic.VaryColors = true;


                            // ------------------------------ XML -----------------------------------
                            var wsBar = chartPathogenNS.WorkSheet;
                            var wsAnti = chartAntibiotic.WorkSheet;
                            var xdocBar = chartPathogenNS.ChartXml;
                            var xdocAnti = chartAntibiotic.ChartXml;
                            var nsmBar = new System.Xml.XmlNamespaceManager(xdocBar.NameTable);
                            var nsmAnti = new System.Xml.XmlNamespaceManager(xdocAnti.NameTable);

                            string schemaDrawings = "http://schemas.openxmlformats.org/drawingml/2006/main";
                            string schemaChart = "http://schemas.openxmlformats.org/drawingml/2006/chart";

                            nsmBar.AddNamespace("c", schemaChart);
                            nsmBar.AddNamespace("a", schemaDrawings);
                            nsmAnti.AddNamespace("c", schemaChart);
                            nsmAnti.AddNamespace("a", schemaDrawings);

                            const string COLUMN_PATH = "c:chartSpace/c:chart/c:plotArea/c:barChart/c:ser";

                            var nodeBar = chartPathogenNS.ChartXml.SelectSingleNode(COLUMN_PATH, nsmBar);
                            var nodeAnti = chartAntibiotic.ChartXml.SelectSingleNode(COLUMN_PATH, nsmAnti);
                            Color colorRand;
                            strPathogen = "";

                            var objDistPathogen = objPathogen.Select(w => w.org_code).Distinct().ToList();

                            for (var i = 0; i < objDistPathogen.Count(); i++)
                            {


                                //Create the data point node
                                var dPt = xdocBar.CreateElement("dPt", schemaChart);

                                var idx = dPt.AppendChild(xdocBar.CreateElement("idx", schemaChart));
                                var valattrib = idx.Attributes.Append(xdocBar.CreateAttribute("val"));
                                valattrib.Value = i.ToString(CultureInfo.InvariantCulture);
                                nodeBar.AppendChild(dPt);

                                //Add the solid fill node
                                var spPr = xdocBar.CreateElement("spPr", schemaChart);
                                var solidFill = spPr.AppendChild(xdocBar.CreateElement("solidFill", schemaDrawings));
                                var srgbClr = solidFill.AppendChild(xdocBar.CreateElement("srgbClr", schemaDrawings));
                                valattrib = srgbClr.Attributes.Append(xdocBar.CreateAttribute("val"));

                                ////Set the color
                                colorRand = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                                //colorRand = Color.FromArgb(0, 0, 0);
                                if (!dicColor.ContainsKey(objDistPathogen[i]))
                                {
                                    dicColor.Add(objDistPathogen[i], colorRand);
                                }
                                else
                                {
                                    colorRand = dicColor[objDistPathogen[i]];
                                }

                                valattrib.Value = ColorTranslator.ToHtml(colorRand).Replace("#", String.Empty);
                                dPt.AppendChild(spPr);
                                //}
                            }

                            // ------- Anti Chart------

                            for (var i = 0; i < objPathogen.Count(); i++)
                            {
                                //Create the data point node
                                var dPtAnti = xdocAnti.CreateElement("dPt", schemaChart);

                                var idxAnti = dPtAnti.AppendChild(xdocAnti.CreateElement("idx", schemaChart));
                                var valattribAnti = idxAnti.Attributes.Append(xdocAnti.CreateAttribute("val"));
                                valattribAnti.Value = i.ToString(CultureInfo.InvariantCulture);
                                nodeAnti.AppendChild(dPtAnti);

                                //Add the solid fill node
                                var spPrAnti = xdocAnti.CreateElement("spPr", schemaChart);
                                var solidFillAnti = spPrAnti.AppendChild(xdocAnti.CreateElement("solidFill", schemaDrawings));
                                var srgbClrAnti = solidFillAnti.AppendChild(xdocAnti.CreateElement("srgbClr", schemaDrawings));
                                valattribAnti = srgbClrAnti.Attributes.Append(xdocAnti.CreateAttribute("val"));


                                if (dicColor.ContainsKey(objPathogen[i].org_code))
                                {
                                    colorRand = dicColor[objPathogen[i].org_code];
                                }
                                else
                                {
                                    colorRand = Color.FromArgb(0, 0, 0);
                                }
                                //colorRand = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                                valattribAnti.Value = ColorTranslator.ToHtml(colorRand).Replace("#", String.Empty);
                                dPtAnti.AppendChild(spPrAnti);

                            }
                            // shift 28 rows untill chart start position row
                            iRowChartStart += 28;
                            idxRowPathogen = iRowChartStart;
                            idxRowAnti = iRowChartStart;
                        }
                    }

                    //--------------------------- End Sheet3 -----------------------------------

                    package.Save(); //Save the workbook.
           
                }
      
                var ServerFilePath = Path.Combine(sWebRootFolder, sFileName);
                var memory = new MemoryStream();
                string contentType = model.analyze_file_type;
                if (System.IO.File.Exists(ServerFilePath))
                {
                    using (var stream = new FileStream(ServerFilePath, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }
                }
                else
                {
                    Response.StatusCode = 404;
                }

                memory.Position = 0;
                return File(memory, contentType, Path.GetFileName(ServerFilePath));
                log.MethodFinish();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return new EmptyResult();
        }    
    }
}