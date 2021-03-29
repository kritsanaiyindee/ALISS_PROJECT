using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ALISS.DropDownList.DTO;
using ALISS.DropDownList.Library;
using ALISS.DropDownList.Library.DataAccess;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ALISS.DropDownList.Api.Controllers
{
    //[Route("api/[controller]")]
    public class AreaHealthListController : Controller
    {
        private readonly IAreaHealthDataService _service;

        public AreaHealthListController(DropDownListContext db, IMapper mapper)
        {
            _service = new AreaHealthDataService(db, mapper);
        }

        [HttpPost]
        [Route("api/DropDownList/GetAreaHealthList/")]
        public List<HospitalDataDTO> GetAreaHealthList([FromBody]HospitalDataDTO searchModel)
        {
            var objReturn = _service.Get_AreaHealth_List(searchModel);

            return objReturn;
        }

    }
}
