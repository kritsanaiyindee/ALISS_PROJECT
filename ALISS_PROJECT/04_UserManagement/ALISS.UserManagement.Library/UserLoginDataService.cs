using AutoMapper;
using Log4NetLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using ALISS.UserManagement.DTO;
using ALISS.UserManagement.Library.DataAccess;
using ALISS.UserManagement.Library.Models;
using System.Linq;

namespace ALISS.UserManagement.Library
{
    public class UserLoginDataService : IUserLoginDataService
    {
        private static readonly ILogService log = new LogService(typeof(UserLoginDataService));

        private readonly UserManagementAuthContext _db;
        private readonly IMapper _mapper;

        public UserLoginDataService(UserManagementAuthContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<UserLoginDTO> GetList()
        {
            log.MethodStart();

            List<UserLoginDTO> objList = new List<UserLoginDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCUserLogins.ToList();

                    objList = _mapper.Map<List<UserLoginDTO>>(objReturn1);

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

            return objList;
        }

        //public List<UserLoginDTO> GetListWithParam(string param)
        //{
        //    log.MethodStart();

        //    List<UserLoginDTO> objList = new List<UserLoginDTO>();

        //    var searchModel = JsonSerializer.Deserialize<UserLoginDTO>(param);

        //    using (var trans = _db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var objDataList = _db.TCUsers.FromSqlRaw<TCUserLogin>("sp_GET_TCUser {0}", searchModel.usr_userName).ToList();

        //            objList = _mapper.Map<List<UserLoginDTO>>(objDataList);

        //            trans.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            // TODO: Handle failure
        //            trans.Rollback();
        //        }
        //        finally
        //        {
        //            trans.Dispose();
        //        }
        //    }

        //    log.MethodFinish();

        //    return objList;
        //}

        public List<UserLoginPermissionDTO> GetListWithModel(UserLoginSearchDTO searchModel)
        {
            log.MethodStart();

            List<UserLoginPermissionDTO> objList = new List<UserLoginPermissionDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.UserLoginPermissionDTOs.FromSqlRaw<UserLoginPermissionDTO>
                        ("sp_GET_TCUserLogin {0}, {1}, {2}, {3}, {4}, {5}, {6}", 
                            searchModel.usp_arh_code,
                            searchModel.usp_prv_code,
                            searchModel.usp_hos_code,
                            searchModel.usp_lab_code,
                            searchModel.usp_rol_code,
                            searchModel.usr_active,
                            searchModel.usr_email
                        ).ToList();

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

            return objList;
        }

        public List<UserLoginDTO> GetList_Active_WithModel(UserLoginSearchDTO searchModel)
        {
            log.MethodStart();

            List<UserLoginDTO> objList = new List<UserLoginDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TCUserLogins.FromSqlRaw<TCUserLogin>("sp_GET_TCUser_Active {0}", searchModel.usr_username).ToList();

                    objList = _mapper.Map<List<UserLoginDTO>>(objDataList);

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

            return objList;
        }

        public UserLoginDTO GetData(string usr_userName)
        {
            log.MethodStart();

            UserLoginDTO objModel = new UserLoginDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCUserLogins.FirstOrDefault(x => x.usr_username == usr_userName);

                    if (objReturn1 != null)
                    {
                        objModel = _mapper.Map<UserLoginDTO>(objReturn1);
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

            return objModel;
        }

        public UserLoginDTO SaveData(UserLoginDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            var currentUser = "";
            UserLoginDTO objReturn = new UserLoginDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TCUserLogin();

                    if (model.usr_status == "E")
                    {
                        objModel = _db.TCUserLogins.FirstOrDefault(x => x.usr_username == model.usr_username);
                    }

                    if (model.usr_status == "N")
                    {
                        objModel = _mapper.Map<TCUserLogin>(model);

                        objModel.usr_status = objModel.usr_active == true ? "A" : "I";
                        objModel.usr_createuser = model.usr_createuser;
                        objModel.usr_createdate = currentDateTime;

                        currentUser = objModel.usr_createuser;

                        objModel.usr_lockoutEndDateUtc = currentDateTime;

                        _db.TCUserLogins.Add(objModel);
                    }
                    else if (model.usr_status == "E")
                    {
                        objModel.usr_firstname = model.usr_firstname;
                        objModel.usr_lastname = model.usr_lastname;
                        objModel.usr_email = model.usr_email;
                        if (string.IsNullOrEmpty(model.usr_password) == false && model.usr_password != objModel.usr_password)
                        {
                            objModel.usr_password = model.usr_password;

                            objModel.usr_accessFailedCount = 0;
                            objModel.usr_lockoutEndDateUtc = currentDateTime;

                            _db.TCUserPasswordHistorys.Add(new TCUserPasswordHistory()
                            {
                                uph_username = model.usr_username,
                                uph_password = model.usr_password,
                                uph_status = "A",
                                uph_active = true,
                                uph_createuser = model.usr_username,
                                uph_createdate = currentDateTime
                            });
                        }
                        objModel.usr_active = model.usr_active;
                        objModel.usr_status = objModel.usr_active == true ? "A" : "I";
                        objModel.usr_updateuser = model.usr_updateuser;
                        objModel.usr_updatedate = currentDateTime;

                        currentUser = objModel.usr_updateuser;

                        //_db.TCHospitals.Update(objModel);
                    }

                    #region Save Log Process ...
                    _db.LogProcesss.Add(new LogProcess()
                    {
                        log_usr_id = currentUser,
                        log_mnu_id = "",
                        log_mnu_name = "UserLogin",
                        log_tran_id = objModel.usr_username,
                        log_action = (model.usr_status == "N" ? "New" : "Update"),
                        log_desc = (model.usr_status == "N" ? "New" : "Update") + " UserLogin " + objModel.usr_username,
                        log_createuser = currentUser,
                        log_createdate = currentDateTime
                    });
                    #endregion

                    _db.SaveChanges();

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

        public UserLoginDTO SavePassword(UserLoginDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            UserLoginDTO objReturn = new UserLoginDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TCUserLogin();

                    if (model.usr_status == "E")
                    {
                        objModel = _db.TCUserLogins.FirstOrDefault(x => x.usr_username == model.usr_username);

                        objModel.usr_password = model.usr_password;
                        objModel.usr_accessFailedCount = 0;
                        objModel.usr_updateuser = model.usr_updateuser;
                        objModel.usr_updatedate = currentDateTime;

                        objModel.usr_lockoutEndDateUtc = currentDateTime;

                        //_db.TCHospitals.Update(objModel);

                        _db.TCUserPasswordHistorys.Add(new TCUserPasswordHistory()
                        {
                            uph_username = model.usr_username,
                            uph_password = model.usr_password,
                            uph_status = "A",
                            uph_active = true,
                            uph_createuser = model.usr_username,
                            uph_createdate = currentDateTime
                        });
                    }

                    #region Save Log Process ...
                    _db.LogProcesss.Add(new LogProcess()
                    {
                        log_usr_id = (model.usr_updateuser ?? model.usr_createuser),
                        log_mnu_id = "",
                        log_mnu_name = "UserLogin",
                        log_tran_id = objModel.usr_username,
                        log_action = (model.usr_status == "N" ? "New" : "Update"),
                        log_desc = "Update password " + objModel.usr_username,
                        log_createuser = (model.usr_updateuser ?? model.usr_createuser),
                        log_createdate = currentDateTime
                    });
                    #endregion

                    _db.SaveChanges();

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

        public string CheckDuplicate(UserLoginDTO model)
        {
            log.MethodStart();

            string objReturn = "";

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objResult = _db.TCUserLogins.Any(x => x.usr_username == model.usr_username);

                    if (objResult == true)
                    {
                        objReturn = "Dup";
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    // TODO: Handle failure
                    log.Error(ex.Message);
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
