using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using AutoMapper;
using Log4NetLibrary;
using ALISS.AUTH.Library.DataAccess;
using ALISS.AUTH.Library.Models;
using ALISS.AUTH.DTO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ALISS.AUTH.Library
{
    public class RoleService : IRoleService
    {
        private static readonly ILogService log = new LogService(typeof(RoleService));

        private readonly AuthContext _db;
        private readonly IMapper _mapper;

        public RoleService(AuthContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<RoleDTO> GetList()
        {
            log.MethodStart();

            List<RoleDTO> objList = new List<RoleDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCRoles.ToList();

                    objList = _mapper.Map<List<RoleDTO>>(objReturn1);

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

        public List<RoleDTO> GetListWithParam(string param)
        {
            log.MethodStart();

            List<RoleDTO> objList = new List<RoleDTO>();

            //var searchModel = JsonSerializer.Deserialize<RoleSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TCRoles.FromSqlRaw<TCRole>("sp_GET_TCRole {0}, {1}", param, param).ToList();

                    objList = _mapper.Map<List<RoleDTO>>(objDataList);

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

        public List<RoleDTO> GetListWithModel(RoleSearchDTO searchModel)
        {
            log.MethodStart();

            List<RoleDTO> objList = new List<RoleDTO>();

            //var searchModel = JsonSerializer.Deserialize<RoleSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.RoleDTOs.FromSqlRaw<RoleDTO>("sp_GET_TCRole {0}, {1}", searchModel.sch_rol_code, searchModel.sch_rol_name).ToList();

                    objList = _mapper.Map<List<RoleDTO>>(objDataList);

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

        public RoleDTO GetData(string rol_code)
        {
            log.MethodStart();

            RoleDTO objModel = new RoleDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objData = _db.TCRoles.FirstOrDefault(x => x.rol_code == rol_code);

                    if(objData != null)
                    {
                        objModel = _mapper.Map<RoleDTO>(objData);
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

        public List<RolePermissionDTO> GetPermissionListWithModel(RoleSearchDTO searchModel)
        {
            log.MethodStart();

            List<RolePermissionDTO> objList = new List<RolePermissionDTO>();

            //var searchModel = JsonSerializer.Deserialize<RoleSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var menuList = _db.TCMenus.ToList();

                    var rolePermissionList = _db.TCRolePermissions.Where(x => x.rop_rol_code == searchModel.sch_rol_code).ToList();

                    foreach (var item in menuList)
                    {
                        var rolPermissionDTO = new RolePermissionDTO();
                        rolPermissionDTO.mnu_id = item.mnu_id;
                        rolPermissionDTO.mnu_code = item.mnu_code;
                        rolPermissionDTO.mnu_name = item.mnu_name;
                        rolPermissionDTO.mnu_status = item.mnu_status;
                        rolPermissionDTO.mnu_active = item.mnu_active;

                        var rolePermission = rolePermissionList.FirstOrDefault(x => x.rop_mnu_code == item.mnu_code);
                        if (rolePermission != null)
                        {
                            rolPermissionDTO.rop_id = rolePermission.rop_id;
                            rolPermissionDTO.rop_rol_code = rolePermission.rop_rol_code;
                            rolPermissionDTO.rop_mnu_code = rolePermission.rop_mnu_code;
                            rolPermissionDTO.rop_view = rolePermission.rop_view;
                            rolPermissionDTO.rop_create = rolePermission.rop_create;
                            rolPermissionDTO.rop_edit = rolePermission.rop_edit;
                            rolPermissionDTO.rop_approve = rolePermission.rop_approve;
                            rolPermissionDTO.rop_print = rolePermission.rop_print;
                            rolPermissionDTO.rop_reject = rolePermission.rop_reject;
                            rolPermissionDTO.rop_cancel = rolePermission.rop_cancel;
                            rolPermissionDTO.rop_return = rolePermission.rop_return;
                            rolPermissionDTO.rop_complete = rolePermission.rop_complete;
                            rolPermissionDTO.rop_implement = rolePermission.rop_implement;
                            rolPermissionDTO.rop_status = rolePermission.rop_status;
                            rolPermissionDTO.rop_active = rolePermission.rop_active;
                            rolPermissionDTO.rop_createuser = rolePermission.rop_createuser;
                            rolPermissionDTO.rop_createdate = rolePermission.rop_createdate;
                            rolPermissionDTO.rop_updateuser = rolePermission.rop_updateuser;
                            rolPermissionDTO.rop_updatedate = rolePermission.rop_updatedate;
                        }
                        else
                        {
                            rolPermissionDTO.rop_rol_code = searchModel.sch_rol_code;
                            rolPermissionDTO.rop_mnu_code = item.mnu_code;
                        }

                        objList.Add(rolPermissionDTO);
                    }

                    //objList = _db.RolePermissionDTOs.FromSqlRaw<RolePermissionDTO>("sp_GET_TCRolePermission {0}", searchModel.rol_code).ToList();

                    //objList = _mapper.Map<List<RoleDTO>>(objDataList);

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

        public RoleDTO SaveData(RoleDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            var currentUser = "";
            RoleDTO objReturn = new RoleDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TCRole();

                    if (model.rol_status == "E")
                    {
                        objModel = _db.TCRoles.FirstOrDefault(x => x.rol_id == model.rol_id);
                    }

                    if (model.rol_status == "N")
                    {
                        objModel = _mapper.Map<TCRole>(model);

                        objModel.rol_status = objModel.rol_active == true ? "A" : "I";
                        objModel.rol_createdate = currentDateTime;

                        currentUser = objModel.rol_createuser;

                        _db.TCRoles.Add(objModel);
                    }
                    else if (model.rol_status == "E")
                    {
                        objModel.rol_name = model.rol_name;
                        objModel.rol_desc = model.rol_desc;
                        objModel.rol_active = model.rol_active;
                        objModel.rol_status = objModel.rol_active == true ? "A" : "I";
                        objModel.rol_updateuser = model.rol_updateuser;
                        objModel.rol_updatedate = currentDateTime;

                        currentUser = objModel.rol_updateuser;

                        //_db.TCRoles.Update(objModel);
                    }

                    #region Save Log Process ...
                    _db.LogProcesss.Add(new LogProcess()
                    {
                        log_usr_id = currentUser,
                        log_mnu_id = "",
                        log_mnu_name = "Role",
                        log_tran_id = objModel.rol_code.ToString(),
                        log_action = (model.rol_status == "N" ? "New" : "Update"),
                        log_desc = (model.rol_status == "N" ? "New" : "Update") + " Role " + objModel.rol_name,
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

        public List<RolePermissionDTO> SaveListData(List<RolePermissionDTO> modelList)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var model in modelList)
                    {
                        var objModel = new TCRolePermission();

                        if (model.rop_id == null)
                        {
                            objModel = new TCRolePermission()
                            {
                                rop_rol_code = model.rop_rol_code,
                                rop_mnu_code = model.rop_mnu_code,
                                rop_view = model.rop_view,
                                rop_create = model.rop_create,
                                rop_edit = model.rop_edit,
                                rop_approve = model.rop_approve,
                                rop_print = model.rop_print,
                                rop_reject = model.rop_reject,
                                rop_cancel = model.rop_cancel,
                                rop_return = model.rop_return,
                                rop_complete = model.rop_complete,
                                rop_implement = model.rop_implement,
                                rop_status = "A",
                                rop_active = model.rop_active,
                                rop_createuser = model.rop_createuser,
                                rop_createdate = currentDateTime
                            };

                            _db.TCRolePermissions.Add(objModel);
                        }
                        else
                        {
                            objModel = _db.TCRolePermissions.FirstOrDefault(x => x.rop_id == model.rop_id);

                            objModel.rop_view = model.rop_view;
                            objModel.rop_create = model.rop_create;
                            objModel.rop_edit = model.rop_edit;
                            objModel.rop_approve = model.rop_approve;
                            objModel.rop_print = model.rop_print;
                            objModel.rop_reject = model.rop_reject;
                            objModel.rop_cancel = model.rop_cancel;
                            objModel.rop_return = model.rop_return;
                            objModel.rop_complete = model.rop_complete;
                            objModel.rop_implement = model.rop_implement;
                            objModel.rop_updateuser = model.rop_updateuser;
                            objModel.rop_updatedate = currentDateTime;

                            //_db.TCRoles.Update(objModel);
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

            return modelList;
        }

    }
}
