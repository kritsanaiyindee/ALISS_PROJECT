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
    public class WardTypeController : Controller
    {
        private readonly IWardTypeService _service;

        public WardTypeController(MasterManagementContext db, IMapper mapper)
        {
            _service = new WardTypeService(db, mapper);
        }

        [HttpGet]
        [Route("api/WardType/Get_List")]
        public IEnumerable<WardTypeDTO> Get_List()
        {
            var objReturn = _service.GetList();

            return objReturn;
        }

        [HttpGet]
        [Route("api/WardType/Get_List/{param}")]
        public IEnumerable<WardTypeDTO> Get_List(string param)
        {
            var objReturn = _service.GetListWithParam(param);

            return objReturn;
        }

        [HttpPost]
        [Route("api/WardType/Get_ListByModel")]
        public IEnumerable<WardTypeDTO> Get_ListByModel([FromBody]WardTypeDTO searchModel)
        {
            var objReturn = _service.GetListWithModel(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/WardType/Get_List_Active_ByModel")]
        public IEnumerable<WardTypeDTO> Get_List_Active_ByModel([FromBody]WardTypeDTO searchModel)
        {
            var objReturn = _service.GetList_Active_WithModel(searchModel);

            return objReturn;
        }

        [HttpGet]
        [Route("api/WardType/Get_Data/{hos_code}")]
        public WardTypeDTO Get_Data(string hos_code)
        {
            var objReturn = _service.GetData(hos_code);

            return objReturn;
        }

        [HttpPost]
        [Route("api/WardType/Post_SaveData")]
        public WardTypeDTO Post_SaveData([FromBody]WardTypeDTO model)
        {
            var objReturn = _service.SaveData(model);

            //var objReturn = _service.GetList();

            return objReturn;
        }

    }
}
