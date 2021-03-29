using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ALISS.MasterManagement.DTO;
using ALISS.MasterManagement.Library;
using ALISS.MasterManagement.Library.DataAccess;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ALISS.MasterManagement.Api.Controllers
{
    //[Route("api/[controller]")]
    public class MasterHospitalController : Controller
    {
        private readonly IMasterHospitalService _service;

        public MasterHospitalController(MasterManagementContext db, IMapper mapper)
        {
            _service = new MasterHospitalService(db, mapper);
        }

        [HttpGet]
        [Route("api/MasterHospital/Get_List")]
        public IEnumerable<MasterHospitalDTO> Get_List()
        {
            var objReturn = _service.GetList();

            return objReturn;
        }

        [HttpGet]
        [Route("api/MasterHospital/Get_List/{param}")]
        public IEnumerable<MasterHospitalDTO> Get_List(string param)
        {
            var objReturn = _service.GetListWithParam(param);

            return objReturn;
        }

        [HttpPost]
        [Route("api/MasterHospital/Get_ListByModel")]
        public IEnumerable<MasterHospitalDTO> Get_ListByModel([FromBody]MasterHospitalSearchDTO searchModel)
        {
            var objReturn = _service.GetListWithModel(searchModel);

            return objReturn;
        }

        [HttpGet]
        [Route("api/MasterHospital/Get_Data/{hos_code}")]
        public MasterHospitalDTO Get_Data(string hos_code)
        {
            var objReturn = _service.GetData(hos_code);

            return objReturn;
        }

        [HttpPost]
        [Route("api/MasterHospital/Post_SaveData")]
        public MasterHospitalDTO Post_SaveData([FromBody]MasterHospitalDTO model)
        {
            var objReturn = _service.SaveData(model);

            return objReturn;
        }

    }
}
