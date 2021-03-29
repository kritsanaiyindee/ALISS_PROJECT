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
    public class MasterTemplateController : Controller
    {
        private readonly IMasterTemplateService _service;

        public MasterTemplateController(MasterManagementContext db, IMapper mapper)
        {
            _service = new MasterTemplateService(db, mapper);
        }

        [HttpGet]
        [Route("api/MasterTemplate/Get_List")]
        public IEnumerable<MasterTemplateDTO> Get_List()
        {
            var objReturn = _service.GetList();

            return objReturn;
        }

        [HttpGet]
        [Route("api/MasterTemplate/Get_List/{param}")]
        public IEnumerable<MasterTemplateDTO> Get_List(string param)
        {
            var objReturn = _service.GetListWithParam(param);

            return objReturn;
        }

        [HttpPost]
        [Route("api/MasterTemplate/Get_ListByModel")]
        public IEnumerable<MasterTemplateDTO> Get_ListByModel([FromBody]MasterTemplateSearchDTO searchModel)
        {
            var objReturn = _service.GetListWithModel(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/MasterTemplate/Get_List_Active_ByModel")]
        public MasterTemplateDTO Get_List_Active_ByModel([FromBody]MasterTemplateSearchDTO searchModel)
        {
            var objReturn = _service.GetList_Active_WithModel(searchModel);

            return objReturn;
        }

        [HttpGet]
        [Route("api/MasterTemplate/Get_Data/{hos_code}")]
        public MasterTemplateDTO Get_Data(string hos_code)
        {
            var objReturn = _service.GetData(hos_code);

            return objReturn;
        }

        [HttpPost]
        [Route("api/MasterTemplate/Post_SaveData")]
        public MasterTemplateDTO Post_SaveData([FromBody]MasterTemplateDTO model)
        {
            var objReturn = _service.SaveData(model);

            return objReturn;
        }

        [HttpPost]
        [Route("api/MasterTemplate/Post_SaveCopyData")]
        public MasterTemplateDTO Post_SaveCopyData([FromBody]MasterTemplateDTO model)
        {
            var objReturn = _service.SaveCopyData(model);

            return objReturn;
        }

    }
}
