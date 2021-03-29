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
    public class TBConfigController : Controller
    {
        private readonly ITBConfigService _service;

        public TBConfigController(AuthContext db, IMapper mapper)
        {
            _service = new TBConfigService(db, mapper);
        }

        [HttpPost]
        [Route("api/TBConfig/Get_DataList/")]
        public IEnumerable<TBConfigDTO> Get_DataList([FromBody]TBConfigDTO searchModel)
        {
            var objReturn = _service.GetTBConfig(searchModel);

            return objReturn;
        }
    }
}
