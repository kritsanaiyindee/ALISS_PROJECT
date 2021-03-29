using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using ALISS.EXPORT.Library.Model;
using ALISS.EXPORT.Library.DTO;
using System.Globalization;

namespace ALISS.EXPORT.Api.Controllers
{
    public class AMRMapController : ApiController
    {
        ALISSEntities db = new ALISSEntities(true);

        [HttpGet]
        [Route("api/AMRMap/Get")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //[HttpGet]
        //[Route("api/AMRGraph/ExportMap")]
        //public IHttpActionResult ExportMap()
        [HttpPost]
        [Route("api/AMRMap/ExportMap")]
        public IHttpActionResult ExportMap(AMRSearchMapDTO model)
        {
            //DateTime modelMonthFrom = Convert.ToDateTime(model.month_start_str);  //Convert.ToDateTime("01/01/2020");// Convert.ToDateTime(model.month_start_str); 
            //DateTime modelMonthTo = Convert.ToDateTime(model.month_end_str);  //Convert.ToDateTime("01/06/2020");//Convert.ToDateTime(model.month_end_str); 

            DateTime modelMonthFrom = DateTime.ParseExact(model.month_start_str, "yyyy/MM/dd", new CultureInfo("en-US"));
            DateTime modelMonthTo = DateTime.ParseExact(model.month_end_str, "yyyy/MM/dd", new CultureInfo("en-US"));

            ReportDocument rpt = new ReportDocument();
            var query = new List<sp_GET_RPAntibiotrendAMRStrategy_Result>();
            var strReportName = "rptAntibiotrendMap.rpt";

            query = db.sp_GET_RPAntibiotrendAMRStrategy(modelMonthFrom, modelMonthTo).ToList();

            if (query.Count() > 0)
            {
                var strHeaderDateFromTo = string.Format("{0} - {1} {2}"
                                        , modelMonthFrom.ToString("MMMM", new CultureInfo("en-US"))
                                        , modelMonthTo.ToString("MMMM", new CultureInfo("en-US"))
                                        , modelMonthTo.ToString("yyyy")
                                        );

                var Targetpath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Report"), strReportName);
                rpt.Load(Targetpath);
                rpt.SetDataSource(query);
                rpt.SetParameterValue("paramStrHeader", strHeaderDateFromTo);

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
                    //FileName = "AMR_MAP.xls"
                    FileName = "AMR_MAP.pdf"
                    };
                result.Content.Headers.ContentType =
                     new MediaTypeHeaderValue("application/pdf");

                rpt.Dispose();

                var response = ResponseMessage(result);
                return response;
            }
            else
            {
                var result = new HttpResponseMessage(HttpStatusCode.NotFound);
                var response = ResponseMessage(result);
                return response;
            }
           
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
     
    }
}
