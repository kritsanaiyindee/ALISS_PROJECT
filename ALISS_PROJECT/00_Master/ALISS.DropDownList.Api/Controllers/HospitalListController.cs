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
    public class HospitalListController : Controller
    {
        private readonly IHospitalDataService _service;

        public HospitalListController(DropDownListContext db, IMapper mapper)
        {
            _service = new HospitalDataService(db, mapper);
        }

        [HttpPost]
        [Route("api/DropDownList/GetHospitalList/")]
        public List<HospitalDataDTO> GetHospitalList([FromBody]HospitalDataDTO searchModel)
        {
            var objReturn = _service.Get_Hospital_List(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/DropDownList/GetTCHospitalList/")]
        public List<HospitalDataDTO> GetTCHospitalList([FromBody]HospitalDataDTO searchModel)
        {
            var objReturn = _service.Get_TCHospital_List(searchModel);

            return objReturn;
        }

    }
}
