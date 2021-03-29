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
    public class UserLoginPermissionDataService : IUserLoginPermissionDataService
    {
        private static readonly ILogService log = new LogService(typeof(UserLoginPermissionDataService));

        private readonly UserManagementAuthContext _db;
        private readonly IMapper _mapper;

        public UserLoginPermissionDataService(UserManagementAuthContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<UserLoginPermissionDTO> GetList()
        {
            log.MethodStart();

            List<UserLoginPermissionDTO> objList = new List<UserLoginPermissionDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCUserLoginPermissions.ToList();

                    objList = _mapper.Map<List<UserLoginPermissionDTO>>(objReturn1);

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

        public List<UserLoginPermissionDTO> GetListWithParam(string param)
        {
            log.MethodStart();

            List<UserLoginPermissionDTO> objList = new List<UserLoginPermissionDTO>();

            var searchModel = JsonSerializer.Deserialize<UserLoginPermissionDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TCUserLoginPermissions.FromSqlRaw<TCUserLoginPermission>("sp_GET_TCUserPermission {0}", searchModel.usp_usr_userName).ToList();

                    objList = _mapper.Map<List<UserLoginPermissionDTO>>(objDataList);

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

        public List<UserLoginPermissionDTO> GetListWithModel(UserLoginPermissionDTO searchModel)
        {
            log.MethodStart();

            List<UserLoginPermissionDTO> objList = new List<UserLoginPermissionDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.UserLoginPermissionDTOs.FromSqlRaw<UserLoginPermissionDTO>("sp_GET_TCUserPermission {0}", searchModel.usp_usr_userName).ToList();

                    //objList = _mapper.Map<List<UserLoginPermissionDTO>>(objDataList);

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

        public List<UserLoginPermissionDTO> GetList_Active_WithModel(UserLoginPermissionDTO searchModel)
        {
            log.MethodStart();

            List<UserLoginPermissionDTO> objList = new List<UserLoginPermissionDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TCUserLoginPermissions.FromSqlRaw<TCUserLoginPermission>("sp_GET_TCUserPermission_Active {0}", searchModel.usp_usr_userName).ToList();

                    objList = _mapper.Map<List<UserLoginPermissionDTO>>(objDataList);

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

        public UserLoginPermissionDTO GetData(string usp_usr_userName)
        {
            log.MethodStart();

            UserLoginPermissionDTO objModel = new UserLoginPermissionDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCUserLoginPermissions.FirstOrDefault(x => x.usp_usr_userName == usp_usr_userName);

                    objModel = _mapper.Map<UserLoginPermissionDTO>(objReturn1);

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

        public UserLoginPermissionDTO SaveData(UserLoginPermissionDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            var currentUser = "";
            UserLoginPermissionDTO objReturn = new UserLoginPermissionDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TCUserLoginPermission();

                    if (model.usp_status == "E")
                    {
                        objModel = _db.TCUserLoginPermissions.FirstOrDefault(x => x.usp_id == model.usp_id);
                    }

                    if (model.usp_status == "N")
                    {
                        objModel = _mapper.Map<TCUserLoginPermission>(model);

                        objModel.usp_createdate = currentDateTime;
                        objModel.usp_status = objModel.usp_active == true ? "A" : "I";

                        currentUser = objModel.usp_createuser;

                        _db.TCUserLoginPermissions.Add(objModel);
                    }
                    else if (model.usp_status == "E")
                    {
                        objModel.usp_rol_code = model.usp_rol_code;
                        objModel.usp_arh_code = model.usp_arh_code;
                        objModel.usp_hos_code = model.usp_hos_code;
                        objModel.usp_active = model.usp_active;
                        objModel.usp_status = objModel.usp_active == true ? "A" : "I";
                        objModel.usp_updateuser = model.usp_updateuser;
                        objModel.usp_updatedate = currentDateTime;

                        currentUser = objModel.usp_updateuser;

                        //_db.TCHospitals.Update(objModel);
                    }

                    #region Save Log Process ...
                    _db.LogProcesss.Add(new LogProcess()
                    {
                        log_usr_id = currentUser,
                        log_mnu_id = "",
                        log_mnu_name = "UserPermission",
                        log_tran_id = objModel.usp_id.ToString(),
                        log_action = (model.usp_status == "N" ? "New" : "Update"),
                        log_desc = "Update UserPermission " + objModel.usp_usr_userName,
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

        public List<UserLoginPermissionDTO> SaveListData(List<UserLoginPermissionDTO> model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            List<UserLoginPermissionDTO> objReturn = new List<UserLoginPermissionDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in model)
                    {
                        var objModel = new TCUserLoginPermission();

                        if (item.usp_status == "E")
                        {
                            objModel = _db.TCUserLoginPermissions.FirstOrDefault(x => x.usp_id == item.usp_id);
                        }

                        if (item.usp_status == "N")
                        {
                            objModel = _mapper.Map<TCUserLoginPermission>(item);

                            objModel.usp_createdate = currentDateTime;
                            objModel.usp_status = objModel.usp_active == true ? "A" : "I";

                            _db.TCUserLoginPermissions.Add(objModel);
                        }
                        else if (item.usp_status == "E")
                        {
                            objModel.usp_arh_code = item.usp_arh_code;
                            objModel.usp_hos_code = item.usp_hos_code;
                            objModel.usp_rol_code = item.usp_rol_code;

                            objModel.usp_active = item.usp_active;
                            objModel.usp_status = objModel.usp_active == true ? "A" : "I";
                            objModel.usp_updateuser = item.usp_updateuser;
                            objModel.usp_updatedate = currentDateTime;

                            //_db.TCHospitals.Update(objModel);
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

        public string CheckDuplicate(UserLoginPermissionDTO model)
        {
            log.MethodStart();

            string objReturn = "";

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objResult = _db.TCUserLoginPermissions.Any(x => x.usp_usr_userName == model.usp_usr_userName);

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
