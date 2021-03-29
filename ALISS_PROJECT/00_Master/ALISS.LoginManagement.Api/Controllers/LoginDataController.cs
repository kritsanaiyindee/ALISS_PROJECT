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
    public class LoginDataController : Controller
    {
        private readonly ILoginDataService _service;

        public LoginDataController(AuthContext db, IMapper mapper)
        {
            _service = new LoginDataService(db, mapper);
        }

        [HttpPost]
        [Route("api/LoginUser/GetLoginUserData/")]
        public LoginUserDTO GetLoginUserData([FromBody]LoginUserSearchDTO searchModel)
        {
            var objReturn = _service.Get_LoginUser_Data(searchModel);

            if(objReturn != new LoginUserDTO())
            {
                objReturn.str_LoginUserPermission_List = _service.Get_LoginUserPermission_Data(searchModel.usr_username);
                objReturn.str_LoginUserRolePermission_List = _service.Get_LoginUserRolePermission_Data(objReturn.str_LoginUserPermission_List);
            }

            return objReturn;
        }

        [HttpPost]
        [Route("api/LoginUser/GetLoginUserPermissionData/")]
        public List<LoginUserPermissionDTO> GetLoginUserPermissionData([FromBody]LoginUserSearchDTO searchModel)
        {
            var objReturn = _service.Get_LoginUserPermission_List(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/LoginUser/SetWrongPasswordData/")]
        public LoginUserDTO SetWrongPasswordData([FromBody]LoginUserSearchDTO searchModel)
        {
            var objReturn = _service.Set_WrongPassword_Data(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/LoginUser/SetInactiveData/")]
        public LoginUserDTO SetInactiveData([FromBody]LoginUserSearchDTO searchModel)
        {
            var objReturn = _service.Set_Inactive_Data(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/LoginUser/SetLogoutUserData/")]
        public LoginUserDTO SetLogoutUserData([FromBody]LoginUserSearchDTO searchModel)
        {
            var objReturn = _service.Set_LogoutUser_Data(searchModel);

            return objReturn;
        }

        [HttpPost]
        [Route("api/LoginUser/SetTimeoutUserData/")]
        public LoginUserDTO SetTimeoutUserData([FromBody]LoginUserSearchDTO searchModel)
        {
            var objReturn = _service.Set_TimeoutUser_Data(searchModel);

            return objReturn;
        }

    }
}
