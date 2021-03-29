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
    public class AntibioticController : Controller
    {
        private readonly IAntibioticService _service;

        public AntibioticController(MasterManagementContext db, IMapper mapper)
        {
            _service = new AntibioticService(db, mapper);
        }

        [HttpGet]
        [Route("api/Antibiotic/Get_List")]
        public IEnumerable<AntibioticDTO> Get_List()
        {
            var objReturn = _service.GetList();

            return objReturn;
        }

        [HttpGet]
        [Route("api/Antibiotic/Get_List/{param}")]
        public IEnumerable<AntibioticDTO> Get_List(string param)
        {
            var objReturn = _service.GetListWithParam(param);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Antibiotic/Get_ListByModel")]
        public IEnumerable<AntibioticDTO> Get_ListByModel([FromBody]AntibioticDTO searchModel)
        {
            var objReturn = _service.GetListWithModel(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Antibiotic/Get_List_Active_ByModel")]
        public IEnumerable<AntibioticDTO> Get_List_Active_ByModel([FromBody]AntibioticDTO searchModel)
        {
            var objReturn = _service.GetList_Active_WithModel(searchModel);

            return objReturn;
        }

        [HttpGet]
        [Route("api/Antibiotic/Get_Data/{hos_code}")]
        public AntibioticDTO Get_Data(string hos_code)
        {
            var objReturn = _service.GetData(hos_code);

            return objReturn;
        }

        [HttpGet]
        [Route("api/Antibiotic/Get_DataById/{hos_code}")]
        public AntibioticDTO Get_DataById(int hos_code)
        {
            var objReturn = _service.GetDataById(hos_code);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Antibiotic/Post_SaveData")]
        public AntibioticDTO Post_SaveData([FromBody]AntibioticDTO model)
        {
            var objReturn = _service.SaveData(model);

            return objReturn;
        }

    }
}
