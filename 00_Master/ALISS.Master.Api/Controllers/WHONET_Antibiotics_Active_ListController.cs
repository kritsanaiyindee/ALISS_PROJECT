using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ALISS.Master.DTO;
using ALISS.Master.Library.DataAccess;
using ALISS.Master.NoticeMessage.Library;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ALISS.Master.Api.Controllers
{
    //[Route("api/[controller]")]
    public class WHONET_Antibiotics_Active_ListController : Controller
    {
        private readonly IWHONETService _service;

        public WHONET_Antibiotics_Active_ListController(IMapper mapper)
        {
            _service = new WHONETService(mapper);
        }

        // GET: api/<controller>

        [HttpGet]
        [Route("api/WHONET/WHONET_Antibiotics_Active_List/")]
        public IEnumerable<TCWHONET_AntibioticsDTO> Get()
        {
            var objList = _service.Get_TCWHONET_Antibiotics_Acitve_List();
            return objList;
        }

    }
}
