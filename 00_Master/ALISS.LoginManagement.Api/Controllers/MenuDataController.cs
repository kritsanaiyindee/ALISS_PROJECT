using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ALISS.LoginManagement.DTO;
using ALISS.LoginManagement.Library;
using ALISS.LoginManagement.Library.DataAccess;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ALISS.LoginManagement.Api.Controllers
{
    //[Route("api/[controller]")]
    public class MenuDataController : Controller
    {
        private readonly IMenuDataService _service;

        public MenuDataController(AuthContext db, IMapper mapper)
        {
            _service = new MenuDataService(db, mapper);
        }

        // GET: api/<controller>
        [Route("api/MenuData/GetDataList")]
        [HttpGet]
        public IEnumerable<MenuDataDTO> GetDataList()
        {
            var objReturn = _service.Get_Menu_List();

            return objReturn;
        }

        // GET: api/<controller>
        [Route("api/MenuData/GetDataList/{rol_code}")]
        [HttpGet]
        public IEnumerable<MenuDataDTO> GetDataList(string rol_code)
        {
            var objReturn = _service.Get_Menu_List(rol_code);

            return objReturn;
        }
    }
}
