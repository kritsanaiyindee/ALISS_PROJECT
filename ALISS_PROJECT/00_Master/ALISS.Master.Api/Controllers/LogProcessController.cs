using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ALISS.Master.DTO;
using ALISS.Master.Library;
using ALISS.Master.Library.DataAccess;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ALISS.Master.Api.Controllers
{
    //[Route("api/[controller]")]
    public class LogProcessController : Controller
    {
        private readonly ILogProcessService _service;

        public LogProcessController(AuthContext dbAuth, MasterContext db, IMapper mapper)
        {
            _service = new LogProcessService(dbAuth, db, mapper);
        }

        [HttpPost]
        [Route("api/LogProcess/Get_List/")]
        public IEnumerable<LogProcessDTO> Get_List([FromBody]LogProcessSearchDTO searchModel)
        {
            var objReturn = _service.GetLogProcessListWithModel(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/LogProcess/GetAuth_List/")]
        public IEnumerable<LogProcessDTO> GetAuth_List([FromBody]LogProcessSearchDTO searchModel)
        {
            var objReturn = _service.GetLogProcessAuthListWithModel(searchModel);
            
            return objReturn;
        }
    }
}
