using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ALISS.AUTH.DTO;
using ALISS.AUTH.Library;
using ALISS.AUTH.Library.DataAccess;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ALISS.AUTH.Api.Controllers
{
    //[Route("api/[controller]")]
    public class LogUserLoginController : Controller
    {
        private readonly ILogUserLoginService _service;

        public LogUserLoginController(AuthContext db, IMapper mapper)
        {
            _service = new LogUserLoginService(db, mapper);
        }

        [HttpPost]
        [Route("api/LogUserLogin/Get_ListByModel")]
        public IEnumerable<LogUserLoginDTO> Get_ListByModel([FromBody]LogUserLoginDTO searchModel)
        {
            var objReturn = _service.GetListWithModel(searchModel);

            return objReturn;
        }
    }
}
