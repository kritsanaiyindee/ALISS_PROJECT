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
    public class PasswordConfigController : Controller
    {
        private readonly IPasswordConfigService _service;

        public PasswordConfigController(AuthContext db, IMapper mapper)
        {
            _service = new PasswordConfigService(db, mapper);
        }

        [HttpGet]
        [Route("api/PasswordConfig/Get_Data/{pwc_Id}")]
        public PasswordConfigDTO Get_Data(string pwc_Id)
        {
            var objReturn = _service.GetData(pwc_Id);

            return objReturn;
        }

        [HttpPost]
        [Route("api/PasswordConfig/Post_SaveData")]
        public PasswordConfigDTO Post_SaveData([FromBody]PasswordConfigDTO model)
        {
            var objReturn1 = _service.SaveData(model);

            var objReturn = _service.GetData("1");

            return objReturn;
        }
    }
}
