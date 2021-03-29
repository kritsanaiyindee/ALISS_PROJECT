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
    public class QCRangeController : Controller
    {
        private readonly IQCRangeService _service;

        public QCRangeController(MasterManagementContext db, IMapper mapper)
        {
            _service = new QCRangeService(db, mapper);
        }

        [HttpGet]
        [Route("api/QCRange/Get_List")]
        public IEnumerable<QCRangeDTO> Get_List()
        {
            var objReturn = _service.GetList();

            return objReturn;
        }

        [HttpGet]
        [Route("api/QCRange/Get_List/{param}")]
        public IEnumerable<QCRangeDTO> Get_List(string param)
        {
            var objReturn = _service.GetListWithParam(param);

            return objReturn;
        }

        [HttpPost]
        [Route("api/QCRange/Get_ListByModel")]
        public IEnumerable<QCRangeDTO> Get_ListByModel([FromBody]QCRangeDTO searchModel)
        {
            var objReturn = _service.GetListWithModel(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/QCRange/Get_List_Active_ByModel")]
        public IEnumerable<QCRangeDTO> Get_List_Active_ByModel([FromBody]QCRangeDTO searchModel)
        {
            var objReturn = _service.GetList_Active_WithModel(searchModel);

            return objReturn;
        }

        [HttpGet]
        [Route("api/QCRange/Get_Data/{hos_code}")]
        public QCRangeDTO Get_Data(string hos_code)
        {
            var objReturn = _service.GetData(hos_code);

            return objReturn;
        }

        [HttpPost]
        [Route("api/QCRange/Post_SaveData")]
        public QCRangeDTO Post_SaveData([FromBody]QCRangeDTO model)
        {
            var objReturn = _service.SaveData(model);

            //var objReturn = _service.GetList();

            return objReturn;
        }

    }
}
