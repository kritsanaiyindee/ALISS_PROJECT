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
    public class MenuController : Controller
    {
        private readonly IMenuService _service;

        public MenuController(AuthContext db, IMapper mapper)
        {
            _service = new MenuService(db, mapper);
        }

        [HttpGet]
        [Route("api/Menu/Get_List")]
        public IEnumerable<MenuDTO> Get_List()
        {
            var objReturn = _service.GetList();

            return objReturn;
        }

        [HttpGet]
        [Route("api/Menu/Get_List/{param}")]
        public IEnumerable<MenuDTO> Get_List(string param)
        {
            var objReturn = _service.GetListWithParam(param);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Menu/Get_ListByModel")]
        public IEnumerable<MenuDTO> Get_ListByModel([FromBody]MenuSearchDTO searchModel)
        {
            var objReturn = _service.GetListWithModel(searchModel);

            return objReturn;
        }

        [HttpGet]
        [Route("api/Menu/Get_Data/{mnu_Id}")]
        public MenuDTO Get_Data(string mnu_Id)
        {
            var objReturn = _service.GetData(mnu_Id);

            return objReturn;
        }

        [HttpPost]
        [Route("api/Menu/Post_SaveData")]
        public MenuDTO Post_SaveData([FromBody]MenuDTO model)
        {
            var objReturn = _service.SaveData(model);

            return objReturn;
        }

    }
}
