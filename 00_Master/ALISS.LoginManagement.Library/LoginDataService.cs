using AutoMapper;
using Log4NetLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ALISS.LoginManagement.Library.DataAccess;
using ALISS.LoginManagement.DTO;
using ALISS.LoginManagement.Library.Models;
using System.Text.Json;

namespace ALISS.LoginManagement.Library
{
    public class LoginDataService : ILoginDataService
    {
        private static readonly ILogService log = new LogService(typeof(LoginDataService));

        private readonly AuthContext _db;
        private readonly IMapper _mapper;

        public LoginDataService(AuthContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public LoginUserDTO Get_LoginUser_Data(LoginUserSearchDTO searchModel)
        {
            log.MethodStart();

            LoginUserDTO objReturn = new LoginUserDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objResult = _db.LoginUserDTOs.FromSqlRaw<LoginUserDTO>("sp_Check_UserLogin {0}, {1}", searchModel.usr_username, searchModel.usr_password).ToList();
                    if (objResult.Count > 0)
                    {
                        objReturn = objResult.FirstOrDefault();

                        _db.LogUserLogins.Add(new LogUserLogin()
                        {
                            log_usr_id = objReturn.usr_username,
                            log_access_ip = searchModel.usr_clientIp,
                            log_session_id = searchModel.usr_sessionId,
                            log_login_timestamp = DateTime.Now,
                            log_status = "Login"
                        });

                        var objUserLogin = _db.TCUserLogins.FirstOrDefault(x => x.usr_username == searchModel.usr_username);
                        if (objUserLogin != null)
                        {
                            objUserLogin.usr_accessFailedCount = 0;
                        }

                        _db.SaveChanges();
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    // TODO: Handle failure
                    trans.Rollback();
                }
                finally
                {
                    trans.Dispose();
                }
            }

            log.MethodFinish();

            return objReturn;
        }

        public string Get_LoginUserPermission_Data(string usr_username)
        {
            log.MethodStart();

            string objReturn = "";

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var userPermissionList = _db.LoginUserPermissionDTOs.FromSqlRaw<LoginUserPermissionDTO>("sp_Check_UserLoginPermission {0}", usr_username).ToList();

                    if(userPermissionList != new List<LoginUserPermissionDTO>())
                    {
                        objReturn = JsonSerializer.Serialize(userPermissionList);
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    // TODO: Handle failure
                    trans.Rollback();
                }
                finally
                {
                    trans.Dispose();
                }
            }

            log.MethodFinish();

            return objReturn;
        }

        public string Get_LoginUserRolePermission_Data(string param)
        {
            log.MethodStart();

            string objReturn = "";

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var userRolePermissionList = new List<LoginUserRolePermissionDTO>();

                    var userPermissionList = JsonSerializer.Deserialize<List<LoginUserPermissionDTO>>(param);

                    foreach (var item in userPermissionList)
                    {
                        var userRolePermissionListResult = _db.LoginUserRolePermissionDTOs.FromSqlRaw<LoginUserRolePermissionDTO>("sp_Check_UserLoginRolePermission {0}", item.usp_rol_code).ToList();

                        if (userRolePermissionListResult.Count > 0)
                        {
                            userRolePermissionList.AddRange(userRolePermissionListResult);
                        }
                    }

                    if (userRolePermissionList != new List<LoginUserRolePermissionDTO>())
                    {
                        objReturn = JsonSerializer.Serialize(userRolePermissionList);
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    // TODO: Handle failure
                    trans.Rollback();
                }
                finally
                {
                    trans.Dispose();
                }
            }

            log.MethodFinish();

            return objReturn;
        }

        public List<LoginUserPermissionDTO> Get_LoginUserPermission_List(LoginUserSearchDTO searchModel)
        {
            log.MethodStart();

            List<LoginUserPermissionDTO> objReturn = new List<LoginUserPermissionDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objReturn = _db.LoginUserPermissionDTOs.FromSqlRaw<LoginUserPermissionDTO>("sp_Check_UserLoginPermission {0}", searchModel.usr_username).ToList();

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    // TODO: Handle failure
                    trans.Rollback();
                }
                finally
                {
                    trans.Dispose();
                }
            }

            log.MethodFinish();

            return objReturn;
        }

        public LoginUserDTO Set_WrongPassword_Data(LoginUserSearchDTO searchModel)
        {
            log.MethodStart();

            LoginUserDTO objReturn = new LoginUserDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objUserLogin = _db.TCUserLogins.FirstOrDefault(x => x.usr_username == searchModel.usr_username);
                    if (objUserLogin != null)
                    {
                        if (objUserLogin.usr_accessFailedCount == null) objUserLogin.usr_accessFailedCount = 0;
                        objUserLogin.usr_accessFailedCount += 1;
                        _db.SaveChanges();
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    // TODO: Handle failure
                    trans.Rollback();
                }
                finally
                {
                    trans.Dispose();
                }
            }

            log.MethodFinish();

            return objReturn;
        }

        public LoginUserDTO Set_Inactive_Data(LoginUserSearchDTO searchModel)
        {
            log.MethodStart();

            LoginUserDTO objReturn = new LoginUserDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objUserLogin = _db.TCUserLogins.FirstOrDefault(x => x.usr_username == searchModel.usr_username);
                    if (objUserLogin != null)
                    {
                        objUserLogin.usr_status = "I";
                        objUserLogin.usr_active = false;
                        _db.SaveChanges();
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    // TODO: Handle failure
                    trans.Rollback();
                }
                finally
                {
                    trans.Dispose();
                }
            }

            log.MethodFinish();

            return objReturn;
        }

        public LoginUserDTO Set_LogoutUser_Data(LoginUserSearchDTO searchModel)
        {
            log.MethodStart();

            LoginUserDTO objReturn = new LoginUserDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objResult = _db.LogUserLogins.FirstOrDefault(x => x.log_usr_id == searchModel.usr_username && x.log_session_id == searchModel.usr_sessionId);

                    if (objResult != null)
                    {
                        objResult.log_logout_timestamp = DateTime.Now;
                        objResult.log_status = "Logout";
                        objResult.log_remark = "Normal";

                        _db.SaveChanges();

                        trans.Commit();

                        objReturn = new LoginUserDTO()
                        {
                            usr_username = objResult.log_usr_id
                        };
                    }
                }
                catch (Exception ex)
                {
                    // TODO: Handle failure
                    trans.Rollback();
                }
                finally
                {
                    trans.Dispose();
                }
            }

            log.MethodFinish();

            return objReturn;
        }

        public LoginUserDTO Set_TimeoutUser_Data(LoginUserSearchDTO searchModel)
        {
            log.MethodStart();

            LoginUserDTO objReturn = new LoginUserDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objResult = _db.LogUserLogins.FirstOrDefault(x => x.log_usr_id == searchModel.usr_username && x.log_session_id == searchModel.usr_sessionId);

                    if (objResult != null)
                    {
                        objResult.log_logout_timestamp = DateTime.Now;
                        objResult.log_status = "Logout";
                        objResult.log_remark = "Timeout";

                        _db.SaveChanges();

                        trans.Commit();

                        objReturn = new LoginUserDTO()
                        {
                            usr_username = objResult.log_usr_id
                        };
                    }
                }
                catch (Exception ex)
                {
                    // TODO: Handle failure
                    trans.Rollback();
                }
                finally
                {
                    trans.Dispose();
                }
            }

            log.MethodFinish();

            return objReturn;
        }

    }
}
