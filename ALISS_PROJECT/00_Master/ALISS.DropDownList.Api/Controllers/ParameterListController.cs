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
    public class ParameterListController : Controller
    {
        private readonly IParameterService _service;

        public ParameterListController(DropDownListContext db, IMapper mapper)
        {
            _service = new ParameterService(db, mapper);
        }

        [HttpPost]
        [Route("api/DropDownList/GetParameterList/")]
        public List<ParameterDTO> GetParameterList([FromBody]ParameterDTO searchModel)
        {
            var objReturn = _service.Get_Parameter_List(searchModel);

            return objReturn;
        }

    }
}
