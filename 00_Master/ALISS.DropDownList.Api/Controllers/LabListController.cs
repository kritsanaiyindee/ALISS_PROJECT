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
    public class LabListController : Controller
    {
        private readonly ILabDataService _service;

        public LabListController(DropDownListContext db, IMapper mapper)
        {
            _service = new LabDataService(db, mapper);
        }

        [HttpPost]
        [Route("api/DropDownList/GetLabList/")]
        public List<DropDownListDTO> GetLabList([FromBody]DropDownListDTO searchModel)
        {
            var objReturn = _service.Get_Lab_List(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/DropDownList/GetAllLabList/")]
        public List<HospitalLabDataDTO> GetAllLabList([FromBody]HospitalLabDataDTO searchModel)
        {
            var objReturn = _service.Get_AllLab_List(searchModel);

            return objReturn;
        }

    }
}
