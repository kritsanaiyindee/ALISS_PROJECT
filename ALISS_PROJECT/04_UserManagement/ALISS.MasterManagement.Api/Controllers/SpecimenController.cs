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
    public class SpecimenController : Controller
    {
        private readonly ISpecimenService _service;

        public SpecimenController(MasterManagementContext db, IMapper mapper)
        {
            _service = new SpecimenService(db, mapper);
        }

        [HttpGet]
        [Route("api/Specimen/Get_List")]
        public IEnumerable<SpecimenDTO> Get_List()
        {
            var objReturn = _service.GetList();

            return objReturn;
        }

        [HttpGet]
        [Route("api/Specimen/Get_List/{param}")]
        public IEnumerable<SpecimenDTO> Get_List(string param)
        {
            var objReturn = _service.GetListWithParam(param);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Specimen/Get_ListByModel")]
        public IEnumerable<SpecimenDTO> Get_ListByModel([FromBody]SpecimenDTO searchModel)
        {
            var objReturn = _service.GetListWithModel(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Specimen/Get_List_Active_ByModel")]
        public IEnumerable<SpecimenDTO> Get_List_Active_ByModel([FromBody]SpecimenDTO searchModel)
        {
            var objReturn = _service.GetList_Active_WithModel(searchModel);

            return objReturn;
        }

        [HttpGet]
        [Route("api/Specimen/Get_Data/{hos_code}")]
        public SpecimenDTO Get_Data(string hos_code)
        {
            var objReturn = _service.GetData(hos_code);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Specimen/Post_SaveData")]
        public SpecimenDTO Post_SaveData([FromBody]SpecimenDTO model)
        {
            var objReturn = _service.SaveData(model);

            //var objReturn = _service.GetList();

            return objReturn;
        }

    }
}
