using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ALISS.ANTIBIOTREND.DTO;
using ALISS.ANTIBIOTREND.Library;
using ALISS.ANTIBIOTREND.Library.DataAccess;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ALISS.ANTIBIOTREND.Api.Controllers
{
    
    [ApiController]
    public class AntibiotrendController : ControllerBase
    {
        private readonly IAntibiotrendService _service;
        private readonly IWebHostEnvironment _host;

        public AntibiotrendController(AntibiotrendContext db, IMapper mapper, IWebHostEnvironment host)
        {
            _service = new AntibiotrendService(db, mapper);
            _host = host;
        }

        [HttpGet]
        [Route("api/Antibiotrend/Get")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("api/Antibiotrend/GetAMRModel")]
        public IEnumerable<SP_AntimicrobialResistanceDTO> GetAMR([FromBody]SP_AntimicrobialResistanceSearchDTO searchModel)
        {
            var objReturn = _service.GetAMRWithModel(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Antibiotrend/GetNationHealthStrategyModel")]
        public IEnumerable<NationHealthStrategyDTO> GetAMRNationHealthStrategy([FromBody]AMRStrategySearchDTO searchModel)
        {
            var objReturn = _service.GetAMRNationStrategyWithModel(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Antibiotrend/GetAMRStrategyModel")]
        public IEnumerable<AntibiotrendAMRStrategyDTO> GetAMRStrategy([FromBody]AMRStrategySearchDTO searchModel)
        {
            var objReturn = _service.GetAntibiotrendAMRStrategyWithModel(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Antibiotrend/GetAMROverallModel")]
        public IEnumerable<SP_AntimicrobialResistanceDTO> GetAMROverall([FromBody]SP_AntimicrobialResistanceSearchDTO searchModel)
        {
            var objReturn = _service.GetAMRByOverallWithModel(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Antibiotrend/GetAMRSpecimenModel")]
        public IEnumerable<SP_AntimicrobialResistanceDTO> GetAMRSpecimen([FromBody]SP_AntimicrobialResistanceSearchDTO searchModel)
        {
            var objReturn = _service.GetAMRBySpecimenWithModel(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Antibiotrend/GetAMRWardTypeModel")]
        public IEnumerable<SP_AntimicrobialResistanceDTO> GetAMRWardType([FromBody]SP_AntimicrobialResistanceSearchDTO searchModel)
        {
            var objReturn = _service.GetAMRByWardWithModel(searchModel);

            return objReturn;
        }

    }
}