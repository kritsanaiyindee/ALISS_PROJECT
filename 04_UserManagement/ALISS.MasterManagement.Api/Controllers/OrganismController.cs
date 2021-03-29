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
    public class OrganismController : Controller
    {
        private readonly IOrganismService _service;

        public OrganismController(MasterManagementContext db, IMapper mapper)
        {
            _service = new OrganismService(db, mapper);
        }

        [HttpGet]
        [Route("api/Organism/Get_List")]
        public IEnumerable<OrganismDTO> Get_List()
        {
            var objReturn = _service.GetList();

            return objReturn;
        }

        [HttpGet]
        [Route("api/Organism/Get_List/{param}")]
        public IEnumerable<OrganismDTO> Get_List(string param)
        {
            var objReturn = _service.GetListWithParam(param);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Organism/Get_ListByModel")]
        public IEnumerable<OrganismDTO> Get_ListByModel([FromBody]OrganismDTO searchModel)
        {
            var objReturn = _service.GetListWithModel(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Organism/Get_List_Active_ByModel")]
        public IEnumerable<OrganismDTO> Get_List_Active_ByModel([FromBody]OrganismDTO searchModel)
        {
            var objReturn = _service.GetList_Active_WithModel(searchModel);

            return objReturn;
        }

        [HttpGet]
        [Route("api/Organism/Get_Data/{hos_code}")]
        public OrganismDTO Get_Data(string hos_code)
        {
            var objReturn = _service.GetData(hos_code);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Organism/Post_SaveData")]
        public OrganismDTO Post_SaveData([FromBody]OrganismDTO model)
        {
            var objReturn = _service.SaveData(model);

            //var objReturn = _service.GetList();

            return objReturn;
        }

    }
}
