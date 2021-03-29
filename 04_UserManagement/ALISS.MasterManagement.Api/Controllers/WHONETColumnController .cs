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
    public class WHONETColumnController : Controller
    {
        private readonly IWHONETColumnService _service;

        public WHONETColumnController(MasterManagementContext db, IMapper mapper)
        {
            _service = new WHONETColumnService(db, mapper);
        }

        [HttpGet]
        [Route("api/WHONETColumn/Get_List")]
        public IEnumerable<WHONETColumnDTO> Get_List()
        {
            var objReturn = _service.GetList();

            return objReturn;
        }

        [HttpGet]
        [Route("api/WHONETColumn/Get_List/{param}")]
        public IEnumerable<WHONETColumnDTO> Get_List(string param)
        {
            var objReturn = _service.GetListWithParam(param);

            return objReturn;
        }

        [HttpPost]
        [Route("api/WHONETColumn/Get_ListByModel")]
        public IEnumerable<WHONETColumnDTO> Get_ListByModel([FromBody]WHONETColumnDTO searchModel)
        {
            var objReturn = _service.GetListWithModel(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/WHONETColumn/Get_List_Active_ByModel")]
        public IEnumerable<WHONETColumnDTO> Get_List_Active_ByModel([FromBody]WHONETColumnDTO searchModel)
        {
            var objReturn = _service.GetList_Active_WithModel(searchModel);

            return objReturn;
        }

        [HttpGet]
        [Route("api/WHONETColumn/Get_Data/{hos_code}")]
        public WHONETColumnDTO Get_Data(string hos_code)
        {
            var objReturn = _service.GetData(hos_code);

            return objReturn;
        }

        [HttpPost]
        [Route("api/WHONETColumn/Post_SaveData")]
        public WHONETColumnDTO Post_SaveData([FromBody]WHONETColumnDTO model)
        {
            var objReturn = _service.SaveData(model);

            //var objReturn = _service.GetList();

            return objReturn;
        }

    }
}
