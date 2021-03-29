using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ALISS.ANTIBIOGRAM.DTO;
using ALISS.ANTIBIOGRAM.Library;
using ALISS.ANTIBIOGRAM.Library.DataAccess;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ALISS.ANTIBIOGRAM.Api.Controllers
{
    [ApiController]
    public class AntibiogramController : Controller
    {
        private readonly IAntibiogramService _service;
        private readonly IWebHostEnvironment _host;

        public AntibiogramController(ReportContext db, IMapper mapper, IWebHostEnvironment host)
        {
            _service = new AntibiogramService(db, mapper);
            _host = host;
        }

        [HttpGet]
        [Route("api/Antibiogram/Get")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("api/Antibiogram/GetAntiHospitalTemplateModel")]
        public IEnumerable<AntibiogramHospitalTemplateDTO> GetAntibiogramHospitalTemplateModel([FromBody]AntiHospitalSearchDTO searchModel)
        {
            var objReturn = _service.GetAntibiogramHospitalTemplateWithModel(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Antibiogram/GetAntiAreaHealthTemplateModel")]
        public IEnumerable<AntibiogramAreaHealthTemplateDTO> GetAntibiogramAreaHealthTemplateModel([FromBody]AntiAreaHealthSearchDTO searchModel)
        {
            var objReturn = _service.GetAntibiogramAreaHealthTemplateWithModel(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Antibiogram/GetAntiNationTemplateModel")]
        public IEnumerable<AntibiogramNationTemplateDTO> GetAntibiogramNationTemplateModel([FromBody]AntiNationSearchDTO searchModel)
        {
            var objReturn = _service.GetAntibiogramNationTemplateWithModel(searchModel);

            return objReturn;
        }

        //[HttpPost]
        //[Route("api/Antibiogram/DownloadAntibiogramAreaHealthTemplate")]
        //public async Task<IActionResult> DownloadAntibiogramAreaHealthTemplate([FromBody]AntibiogramAreaHealthTemplateDTO model)
        //{
        //    string contentType = "application/pdf";
        //    string filename = model.file_name.Replace(".xls",".pdf");
        //    //string filepath = model.file_path;

        //    string Serverfilepath = @"AssumeServerFolder\Antibiogram\AreaHealth";

        //    if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(Serverfilepath))
        //    {
        //        return Content("filename not present");
        //    }

        //    //var OutputFilePath = Path.Combine(Server.MapPath("~/" + "FormTemp"), fid + ".pdf");
        //    string Targetpath = _host.ContentRootPath;  // CurrentDirectoty       
        //    var ServerFilePath = Path.Combine(Targetpath, Serverfilepath, filename);           
        //    var memory = new MemoryStream();

        //    if (System.IO.File.Exists(ServerFilePath))
        //    {
        //        using (var stream = new FileStream(ServerFilePath, FileMode.Open))
        //        {
        //            await stream.CopyToAsync(memory);
        //        }
        //    }
        //    else
        //    {
        //        Response.StatusCode = 404;
        //    }

        //    memory.Position = 0;
        //    return File(memory, contentType, Path.GetFileName(ServerFilePath));
        //}

        //[HttpPost]
        //[Route("api/Antibiogram/DownloadAntibiogramNationTemplate")]
        //public async Task<IActionResult> DownloadAntibiogramNationTemplate([FromBody]AntibiogramNationTemplateDTO model)
        //{
        //    string contentType = "application/pdf";
        //    string filename = model.file_name.Replace(".xls", ".pdf");
        //    //string filepath = model.file_path;

        //    string Serverfilepath = @"AssumeServerFolder\Antibiogram\Nation";

        //    if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(Serverfilepath))
        //    {
        //        return Content("filename not present");
        //    }

        //    //var OutputFilePath = Path.Combine(Server.MapPath("~/" + "FormTemp"), fid + ".pdf");
        //    string Targetpath = _host.ContentRootPath; // CurrentDirectoty      
        //    var ServerFilePath = Path.Combine(Targetpath, Serverfilepath, filename);
        //    var memory = new MemoryStream();

        //    if (System.IO.File.Exists(ServerFilePath))
        //    {
        //        using (var stream = new FileStream(ServerFilePath, FileMode.Open))
        //        {
        //            await stream.CopyToAsync(memory);
        //        }
        //    }
        //    else
        //    {
        //        Response.StatusCode = 404;
        //    }

        //    memory.Position = 0;
        //    return File(memory, contentType, Path.GetFileName(ServerFilePath));
        //}

    }
}