using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ALISS.DropDownList.DTO;
using ALISS.DropDownList.Library;
using ALISS.DropDownList.Library.DataAccess;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ALISS.DropDownList.Api.Controllers
{
    //[Route("api/[controller]")]
    public class RoleListController : Controller
    {
        private readonly IRoleDataService _service;

        public RoleListController(DropDownListAuthContext db, IMapper mapper)
        {
            _service = new RoleDataService(db, mapper);
        }

        [HttpPost]
        [Route("api/DropDownList/GetRoleList/")]
        public List<DropDownListDTO> GetRoleList([FromBody]DropDownListDTO searchModel)
        {
            var objReturn = _service.Get_Role_List(searchModel);

            return objReturn;
        }

    }
}
