using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ALISS.UserManagement.DTO;
using ALISS.UserManagement.Library;
using ALISS.UserManagement.Library.DataAccess;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ALISS.UserManagement.Api.Controllers
{
    //[Route("api/[controller]")]
    public class HospitalController : Controller
    {
        private readonly IHospitalService _service;

        public HospitalController(UserManagementContext db, IMapper mapper)
        {
            _service = new HospitalService(db, mapper);
        }

        [HttpGet]
        [Route("api/Hospital/Get_List")]
        public IEnumerable<HospitalDTO> Get_List()
        {
            var objReturn = _service.GetList();

            return objReturn;
        }

        [HttpGet]
        [Route("api/Hospital/Get_List/{param}")]
        public IEnumerable<HospitalDTO> Get_List(string param)
        {
            var objReturn = _service.GetListWithParam(param);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Hospital/Get_ListByModel")]
        public IEnumerable<HospitalDTO> Get_ListByModel([FromBody]HospitalSearchDTO searchModel)
        {
            var objReturn = _service.GetListWithModel(searchModel);

            return objReturn;
        }

        [HttpGet]
        [Route("api/Hospital/Get_Data/{hos_Id}")]
        public HospitalDTO Get_Data(string hos_Id)
        {
            var objReturn = _service.GetData(hos_Id);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Hospital/Get_LabListByModel")]
        public IEnumerable<HospitalLabDTO> Get_LabListByModel([FromBody]HospitalSearchDTO searchModel)
        {
            var objReturn = _service.GetLabListWithModel(searchModel);

            return objReturn;
        }

        [HttpGet]
        [Route("api/Hospital/Get_LabData/{hol_code}")]
        public HospitalLabDTO Get_LabData(string hol_code)
        {
            var objReturn = _service.GetLabData(hol_code);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Hospital/Post_SaveData")]
        public HospitalDTO Post_SaveData([FromBody]HospitalDTO model)
        {
            var objReturn = _service.SaveData(model);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Hospital/Post_SaveLabData")]
        public IEnumerable<HospitalLabDTO> Post_SaveLabData([FromBody]List<HospitalLabDTO> model)
        {
            var objReturn = _service.SaveLabData(model);

            return objReturn;
        }

    }
}
