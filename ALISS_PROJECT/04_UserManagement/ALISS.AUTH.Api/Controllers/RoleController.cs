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
    public class RoleController : Controller
    {
        private readonly IRoleService _service;

        public RoleController(AuthContext db, IMapper mapper)
        {
            _service = new RoleService(db, mapper);
        }

        [HttpGet]
        [Route("api/Role/Get_List")]
        public IEnumerable<RoleDTO> Get_List()
        {
            //var objReturn = _service.GetList();

            RoleSearchDTO searchModel = new RoleSearchDTO();

            var objReturn = _service.GetListWithModel(searchModel);


            return objReturn;
        }

        [HttpGet]
        [Route("api/Role/Get_List/{param}")]
        public IEnumerable<RoleDTO> Get_List(string param)
        {
            var objReturn = _service.GetListWithParam(param);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Role/Get_ListByModel")]
        public IEnumerable<RoleDTO> Get_ListByModel([FromBody]RoleSearchDTO searchModel)
        {
            var objReturn = _service.GetListWithModel(searchModel);

            return objReturn;
        }

        [HttpGet]
        [Route("api/Role/Get_Data/{rol_code}")]
        public RoleDTO Get_Data(string rol_code)
        {
            var objReturn = _service.GetData(rol_code);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Role/Get_PermissionListByModel")]
        public IEnumerable<RolePermissionDTO> Get_PermissionListByModel([FromBody]RoleSearchDTO searchModel)
        {
            var objReturn = _service.GetPermissionListWithModel(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Role/Post_SaveData")]
        public RoleDTO Post_SaveData([FromBody]RoleDTO model)
        {
            var objReturn = _service.SaveData(model);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Role/Post_SaveListData")]
        public IEnumerable<RolePermissionDTO> Post_SaveListData([FromBody]List<RolePermissionDTO> model)
        {
            var objReturn = _service.SaveListData(model);

            return objReturn;
        }
    }
}
