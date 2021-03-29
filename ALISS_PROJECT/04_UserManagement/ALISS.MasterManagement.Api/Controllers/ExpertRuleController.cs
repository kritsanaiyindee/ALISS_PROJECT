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
    public class ExpertRuleController : Controller
    {
        private readonly IExpertRuleService _service;

        public ExpertRuleController(MasterManagementContext db, IMapper mapper)
        {
            _service = new ExpertRuleService(db, mapper);
        }

        [HttpGet]
        [Route("api/ExpertRule/Get_List")]
        public IEnumerable<ExpertRuleDTO> Get_List()
        {
            var objReturn = _service.GetList();

            return objReturn;
        }

        [HttpGet]
        [Route("api/ExpertRule/Get_List/{param}")]
        public IEnumerable<ExpertRuleDTO> Get_List(string param)
        {
            var objReturn = _service.GetListWithParam(param);

            return objReturn;
        }

        [HttpPost]
        [Route("api/ExpertRule/Get_ListByModel")]
        public IEnumerable<ExpertRuleDTO> Get_ListByModel([FromBody]ExpertRuleDTO searchModel)
        {
            var objReturn = _service.GetListWithModel(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/ExpertRule/Get_List_Active_ByModel")]
        public IEnumerable<ExpertRuleDTO> Get_List_Active_ByModel([FromBody]ExpertRuleDTO searchModel)
        {
            var objReturn = _service.GetList_Active_WithModel(searchModel);

            return objReturn;
        }

        [HttpGet]
        [Route("api/ExpertRule/Get_Data/{hos_code}")]
        public ExpertRuleDTO Get_Data(string hos_code)
        {
            var objReturn = _service.GetData(hos_code);

            return objReturn;
        }

        [HttpPost]
        [Route("api/ExpertRule/Post_SaveData")]
        public ExpertRuleDTO Post_SaveData([FromBody]ExpertRuleDTO model)
        {
            var objReturn = _service.SaveData(model);

            //var objReturn = _service.GetList();

            return objReturn;
        }

    }
}
