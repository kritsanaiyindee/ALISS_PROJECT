using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ALISS.UserManagement.DTO;
using ALISS.UserManagement.Library;
using ALISS.UserManagement.Library.DataAccess;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ALISS.UserManagement.Api.Controllers
{
    //[Route("api/[controller]")]
    public class UserLoginController : Controller
    {
        private readonly IUserLoginDataService _service;
        private readonly IUserLoginPermissionDataService _permissionService;

        public UserLoginController(UserManagementAuthContext db, IMapper mapper)
        {
            _service = new UserLoginDataService(db, mapper);
            _permissionService = new UserLoginPermissionDataService(db, mapper);
        }

        [HttpGet]
        [Route("api/UserLogin/Get_List")]
        public IEnumerable<UserLoginDTO> Get_List()
        {
            var objReturn = _service.GetList();

            return objReturn;
        }

        //[HttpGet]
        //[Route("api/UserLogin/Get_List/{param}")]
        //public IEnumerable<UserLoginDTO> Get_List(string param)
        //{
        //    var objReturn = _service.GetListWithParam(param);

        //    return objReturn;
        //}

        [HttpPost]
        [Route("api/UserLogin/Get_ListByModel")]
        public IEnumerable<UserLoginPermissionDTO> Get_ListByModel([FromBody]UserLoginSearchDTO searchModel)
        {
            var objReturn = _service.GetListWithModel(searchModel);

            return objReturn;
        }

        [HttpGet]
        [Route("api/UserLogin/Get_Data/{usr_userName}")]
        public UserLoginDTO Get_Data(string usr_userName)
        {
            var objReturn = _service.GetData(usr_userName);

            return objReturn;
        }

        [HttpPost]
        [Route("api/UserLogin/Get_PermissionListByModel")]
        public IEnumerable<UserLoginPermissionDTO> Get_PermissionListByModel([FromBody]UserLoginPermissionDTO searchModel)
        {
            var objReturn = _permissionService.GetListWithModel(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/UserLogin/Post_SaveData")]
        public UserLoginDTO Post_SaveData([FromBody]UserLoginDTO model)
        {
            var objReturn = _service.SaveData(model);

            return objReturn;
        }

        [HttpPost]
        [Route("api/UserLogin/Post_SavePermissionData")]
        public List<UserLoginPermissionDTO> Post_SavePermissionData([FromBody]List<UserLoginPermissionDTO> model)
        {
            var objReturn = _permissionService.SaveListData(model);

            return objReturn;
        }

        [HttpPost]
        [Route("api/UserLogin/Post_SavePasswordData")]
        public UserLoginDTO Post_SavePasswordData([FromBody]UserLoginDTO model)
        {
            var objReturn = _service.SavePassword(model);

            return objReturn;
        }

    }
}
