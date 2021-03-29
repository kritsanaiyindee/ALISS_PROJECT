using ALISS.Data.Account;
using ALISS.Data.Client;
using ALISS.LoginManagement.DTO;
using Log4NetLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using ALISS.AUTH.DTO;

namespace ALISS.Data.D0_Master
{
    public class LoginUserService
    {
        private static readonly ILogService log = new LogService(typeof(LoginUserService));

        private IConfiguration Configuration { get; }
        private IHttpContextAccessor _httpContextAccessor;
        private int _timeoutDuration;

        private ApiHelper _apiHelper;

        public LoginUserService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
            _timeoutDuration = Convert.ToInt32(configuration["TimeoutDuration"]);
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LoginUser> Check_LoginUser_DataAsync(LoginUserSearchDTO searchModel)
        {
            log.MethodStart();

            LoginUser loginUser = new LoginUser();

            try
            {
                //searchModel.usr_clientIp = _httpContextAccessor.HttpContext.Connection.LocalIpAddress.ToString();
                searchModel.usr_sessionId = Guid.NewGuid().ToString().ToUpper();

                LoginUserDTO objModel = new LoginUserDTO();
                List<LoginUserPermissionDTO> objList = new List<LoginUserPermissionDTO>();

                //log.Info(JsonSerializer.Serialize(searchModel));
                objModel = await _apiHelper.GetDataByModelAsync<LoginUserDTO, LoginUserSearchDTO>("loginuserdata_api/GetLoginUserData", searchModel);
                //log.Info(JsonSerializer.Serialize(objModel));
                if (objModel?.usr_id != 0)
                {
                    loginUser.Username = objModel.usr_username;
                    loginUser.Firstname = objModel.usr_firstname;
                    loginUser.Lastname = objModel.usr_lastname;

                    loginUser.ClientIp = searchModel.usr_clientIp;
                    loginUser.SessionId = searchModel.usr_sessionId;
                    loginUser.SessionTimeStamp = DateTime.Now;
                    loginUser.SessionTimeout = _timeoutDuration;

                    loginUser.LoginUserPermissionList = JsonSerializer.Deserialize<List<LoginUserPermissionDTO>>(objModel.str_LoginUserPermission_List);

                    loginUser.LoginUserRolePermissionList = JsonSerializer.Deserialize<List<LoginUserRolePermissionDTO>>(objModel.str_LoginUserRolePermission_List);

                    //objList = await _apiHelper.GetDataListByModelAsync<LoginUserPermissionDTO, LoginUserSearchDTO>("loginuserdata_api/GetLoginUserPermissionData", searchModel);
                    //log.Info(JsonSerializer.Serialize(objList));
                    if (loginUser.LoginUserPermissionList.Count > 0)
                    {
                        var objListFirst = loginUser.LoginUserPermissionList.FirstOrDefault();

                        loginUser.rol_code = objListFirst.usp_rol_code;
                        loginUser.rol_name = objListFirst.usp_rol_name;
                        loginUser.arh_code = objListFirst.usp_arh_code;
                        loginUser.arh_name = objListFirst.usp_arh_name;
                        loginUser.prv_code = objListFirst.usp_prv_code;
                        loginUser.prv_name = objListFirst.usp_prv_name;
                        loginUser.hos_code = objListFirst.usp_hos_code;
                        loginUser.hos_name = objListFirst.usp_hos_name;
                        loginUser.lab_code = objListFirst.usp_lab_code;
                        loginUser.lab_name = objListFirst.usp_lab_name;

                        RoleSearchDTO searchData = new RoleSearchDTO() { sch_rol_code = loginUser.rol_code };
                        loginUser.rol_permission_List = await _apiHelper.GetDataListByModelAsync<RolePermissionDTO, RoleSearchDTO>("role_api/Get_PermissionListByModel", searchData);
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: Handle failure
                //trans.Rollback();

                log.Error(ex.Message);
                log.Error(ex.InnerException.Message);
            }
            finally
            {
                //trans.Dispose();
            }

            log.MethodFinish();

            return loginUser;
        }

        public async Task<LoginUserDTO> Set_WrongPassword_DataAsync(LoginUserSearchDTO searchModel)
        {
            LoginUserDTO objModel = new LoginUserDTO();
            List<LoginUserPermissionDTO> objList = new List<LoginUserPermissionDTO>();

            objModel = await _apiHelper.GetDataByModelAsync<LoginUserDTO, LoginUserSearchDTO>("loginuserdata_api/SetWrongPasswordData", searchModel);

            return objModel;
        }

        public async Task<LoginUserDTO> Set_Inactive_DataAsync(LoginUserSearchDTO searchModel)
        {
            LoginUserDTO objModel = new LoginUserDTO();
            List<LoginUserPermissionDTO> objList = new List<LoginUserPermissionDTO>();

            objModel = await _apiHelper.GetDataByModelAsync<LoginUserDTO, LoginUserSearchDTO>("loginuserdata_api/SetInactiveData", searchModel);

            return objModel;
        }

        public async Task<LoginUserDTO> Set_LogoutUser_DataAsync(LoginUserSearchDTO searchModel)
        {
            LoginUserDTO objModel = new LoginUserDTO();
            List<LoginUserPermissionDTO> objList = new List<LoginUserPermissionDTO>();

            objModel = await _apiHelper.GetDataByModelAsync<LoginUserDTO, LoginUserSearchDTO>("loginuserdata_api/SetLogoutUserData", searchModel);

            return objModel;
        }

        public async Task<LoginUserDTO> Set_TimeoutUser_DataAsync(LoginUserSearchDTO searchModel)
        {
            LoginUserDTO objModel = new LoginUserDTO();
            List<LoginUserPermissionDTO> objList = new List<LoginUserPermissionDTO>();

            objModel = await _apiHelper.GetDataByModelAsync<LoginUserDTO, LoginUserSearchDTO>("loginuserdata_api/SetTimeoutUserData", searchModel);

            return objModel;
        }

        public string Get_Session_Id()
        {
            var session_Id = _httpContextAccessor.HttpContext.Connection.Id;
            return session_Id;
        }
    }
}
