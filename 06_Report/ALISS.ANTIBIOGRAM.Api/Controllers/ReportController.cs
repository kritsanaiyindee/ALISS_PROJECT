using ALISS.ANTIBIOGRAM.DTO;
using ALISS.ANTIBIOGRAM.Library;
using ALISS.ANTIBIOGRAM.Library.DataAccess;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ALISS.ANTIBIOGRAM.Api.Controllers
{
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _service;

        public ReportController(ReportContext db, IMapper mapper)
        {
            _service = new ReportService(db, mapper);
        }

        [HttpGet]
        [Route("api/GetAntiHosp")]
        public IEnumerable<AntibiogramDataDTO> GetAntibiogramHospital()
        {
            var objReturn = _service.GetAntibiogramHospitalData();          
            return objReturn;
        }

        [HttpGet]
        [Route("api/GetAntiHosp/{param}")]
        public IEnumerable<AntibiogramDataDTO> GetAntibiogramHospitalParam(string param)
        {
            var objReturn = _service.GetAntibiogramHospitalDataWithParam(param);          
            return objReturn;
        }

        [HttpPost]
        [Route("api/GetAntiHospModel")]
        public IEnumerable<AntibiogramDataDTO> GetAntibiogramHospModel([FromBody]AntiHospitalSearchDTO searchModel)
        {
            var objReturn = _service.GetAntibiogramHospitalDataWithModel(searchModel);

            return objReturn;
        }

        [HttpGet]
        [Route("api/GetAntiAreaHealth")]
        public IEnumerable<AntibiogramDataDTO> GetAntibiogramAreaHealth()
        {
            var objReturn = _service.GetAntibiogramAreaHealthData();
            return objReturn;
        }

        [HttpGet]
        [Route("api/GetAntiAreaHealth/{param}")]
        public IEnumerable<AntibiogramDataDTO> GetAntibiogramAreaHealthParam(string param)
        {
            var objReturn = _service.GetAntibiogramAreaHealthDataWithParam(param);
            return objReturn;
        }

        [HttpPost]
        [Route("api/GetAntiAreaHealthModel")]
        public IEnumerable<AntibiogramDataDTO> GetAntibiogramAreaHealthModel([FromBody]AntiAreaHealthSearchDTO searchModel)
        {
            var objReturn = _service.GetAntibiogramAreaHealthDataWithModel(searchModel);

            return objReturn;
        }

        [HttpGet]
        [Route("api/GetAntiNation")]
        public IEnumerable<AntibiogramDataDTO> GetAntibiogramNation()
        {
            var objReturn = _service.GetAntibiogramNationData();
            return objReturn;
        }

        [HttpGet]
        [Route("api/GetAntiNation/{param}")]
        public IEnumerable<AntibiogramDataDTO> GetAntibiogramNationParam(string param)
        {
            var objReturn = _service.GetAntibiogramNationDataWithParam(param);
            return objReturn;
        }

        [HttpPost]
        [Route("api/GetAntiNationModel")]
        public IEnumerable<AntibiogramDataDTO> GetAntibiogramNationhModel([FromBody]AntiNationSearchDTO searchModel)
        {
            var objReturn = _service.GetAntibiogramNationDataWithModel(searchModel);

            return objReturn;
        }
          
    }
}