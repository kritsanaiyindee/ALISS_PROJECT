using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ALISS.AUTH.DTO;
using ALISS.AUTH.Library;
using ALISS.AUTH.Library.DataAccess;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ALISS.AUTH.Api.Controllers
{
    //[Route("api/[controller]")]
    public class ColumnConfigController : Controller
    {
        private readonly IColumnConfigService _service;

        public ColumnConfigController(AuthContext db, IMapper mapper)
        {
            _service = new ColumnConfigService(db, mapper);
        }

        [HttpGet]
        [Route("api/ColumnConfig/Get_List")]
        public IEnumerable<ColumnConfigDTO> Get_List()
        {
            var objReturn = _service.GetList();

            return objReturn;
        }

        [HttpGet]
        [Route("api/ColumnConfig/Get_List/{param}")]
        public IEnumerable<ColumnConfigDTO> Get_List(string param)
        {
            var objReturn = _service.GetListWithParam(param);

            return objReturn;
        }

        [HttpPost]
        [Route("api/ColumnConfig/Get_ListByModel")]
        public IEnumerable<ColumnConfigDTO> Get_ListByModel([FromBody]ColumnConfigSearchDTO searchModel)
        {
            var objReturn = _service.GetListWithModel(searchModel);

            return objReturn;
        }

        [HttpGet]
        [Route("api/ColumnConfig/Get_Data/{mnu_Id}")]
        public ColumnConfigDTO Get_Data(string mnu_Id)
        {
            var objReturn = _service.GetData(mnu_Id);

            return objReturn;
        }

        [HttpPost]
        [Route("api/ColumnConfig/Post_SaveData")]
        public ColumnConfigDTO Post_SaveData([FromBody]ColumnConfigDTO model)
        {
            var objReturn = _service.SaveData(model);

            return objReturn;
        }

    }
}
