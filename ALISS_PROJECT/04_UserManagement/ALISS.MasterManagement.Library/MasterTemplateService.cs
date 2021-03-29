using AutoMapper;
using Log4NetLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALISS.MasterManagement.DTO;
using ALISS.MasterManagement.Library.DataAccess;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ALISS.MasterManagement.Library.Models;

namespace ALISS.MasterManagement.Library
{
    public class MasterTemplateService : IMasterTemplateService
    {
        private static readonly ILogService log = new LogService(typeof(MasterTemplateService));

        private readonly MasterManagementContext _db;
        private readonly IMapper _mapper;

        public MasterTemplateService(MasterManagementContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<MasterTemplateDTO> GetList()
        {
            log.MethodStart();

            List<MasterTemplateDTO> objList = new List<MasterTemplateDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCMasterTemplates.ToList();

                    objList = _mapper.Map<List<MasterTemplateDTO>>(objReturn1);

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

        public List<MasterTemplateDTO> GetListWithParam(string param)
        {
            log.MethodStart();

            List<MasterTemplateDTO> objList = new List<MasterTemplateDTO>();

            var searchModel = JsonSerializer.Deserialize<MasterTemplateSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TCMasterTemplates.FromSqlRaw<TCMasterTemplate>("sp_GET_TCMasterTemplate {0}", searchModel.mst_code).ToList();

                    objList = _mapper.Map<List<MasterTemplateDTO>>(objDataList);

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

        public List<MasterTemplateDTO> GetListWithModel(MasterTemplateSearchDTO searchModel)
        {
            log.MethodStart();

            List<MasterTemplateDTO> objList = new List<MasterTemplateDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TCMasterTemplates.FromSqlRaw<TCMasterTemplate>("sp_GET_TCMasterTemplate {0}, {1}, {2}, {3}", searchModel.mst_code, searchModel.mst_version, searchModel.mst_date_from, searchModel.mst_date_to).ToList();

                    objList = _mapper.Map<List<MasterTemplateDTO>>(objDataList);

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

        public MasterTemplateDTO GetList_Active_WithModel(MasterTemplateSearchDTO searchModel)
        {
            log.MethodStart();

            MasterTemplateDTO objReturn = new MasterTemplateDTO();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TCMasterTemplates.FromSqlRaw<TCMasterTemplate>("sp_GET_TCMasterTemplate_Active {0}, {1}, {2}", searchModel.mst_code, searchModel.mst_date_from, searchModel.mst_date_to).ToList();

                    var objListMapping = _mapper.Map<List<MasterTemplateDTO>>(objDataList);

                    if (objListMapping.Count > 0)
                    {
                        objReturn = objListMapping.FirstOrDefault();
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

        public MasterTemplateDTO GetData(string mst_code)
        {
            log.MethodStart();

            MasterTemplateDTO objModel = new MasterTemplateDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCMasterTemplates.FirstOrDefault(x => x.mst_code == mst_code);

                    if (objReturn1 != null)
                    {
                        objModel = _mapper.Map<MasterTemplateDTO>(objReturn1);
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

        public MasterTemplateDTO SaveData(MasterTemplateDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            var currentUser = "";
            MasterTemplateDTO objReturn = new MasterTemplateDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TCMasterTemplate();

                    if (model.mst_status == "E")
                    {
                        objModel = _db.TCMasterTemplates.FirstOrDefault(x => x.mst_code == model.mst_code);
                    }

                    if (model.mst_status == "N")
                    {
                        var new_mst_code = "";
                        var currentYearCode = $"MST_{DateTime.Now.ToString("yyyy")}";
                        MasterTemplateSearchDTO searchModel = new MasterTemplateSearchDTO();

                        var objDataList = _db.TCMasterTemplates.FromSqlRaw<TCMasterTemplate>("sp_GET_TCMasterTemplate {0}, {1}, {2}", currentYearCode, searchModel.mst_date_from, searchModel.mst_date_to).ToList();

                        if (objDataList.Count > 0)
                        {
                            var lastData = objDataList.LastOrDefault();
                            var lastCode = lastData.mst_code.Substring(7);
                            var newCodeInt = (Convert.ToInt32(lastCode) + 1);
                            new_mst_code = currentYearCode + newCodeInt.ToString("0000");
                        }
                        else
                        {
                            new_mst_code = currentYearCode + (1).ToString("0000");
                        }

                        model.mst_code = new_mst_code;
                        objReturn.mst_code = new_mst_code;

                        objModel = _mapper.Map<TCMasterTemplate>(model);

                        objModel.mst_status = objModel.mst_active == true ? "A" : "I";
                        objModel.mst_createdate = currentDateTime;

                        currentUser = objModel.mst_createuser;

                        _db.TCMasterTemplates.Add(objModel);
                    }
                    else if (model.mst_status == "E")
                    {
                        //objModel.mst_name = model.mst_name;
                        objModel.mst_version = model.mst_version;
                        objModel.mst_date_from = model.mst_date_from;
                        objModel.mst_active = model.mst_active;
                        objModel.mst_status = objModel.mst_active == true ? "A" : "I";
                        objModel.mst_updateuser = model.mst_updateuser;
                        objModel.mst_updatedate = currentDateTime;

                        currentUser = objModel.mst_updateuser;

                        //_db.TCTemplates.Update(objModel);
                    }

                    #region Save Log Process ...
                    _db.LogProcesss.Add(new LogProcess()
                    {
                        log_usr_id = currentUser,
                        log_mnu_id = "",
                        log_mnu_name = "MasterTemplate",
                        log_tran_id = model.mst_code.ToString(),
                        log_action = (model.mst_status == "N" ? "New" : "Update"),
                        log_desc = (model.mst_status == "N" ? "New" : "Update") + " MasterTemplate " + objModel.mst_code,
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

        public MasterTemplateDTO SaveCopyData(MasterTemplateDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            var currentUser = "";
            var new_mst_code = "";
            MasterTemplateDTO objReturn = new MasterTemplateDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    MasterTemplateSearchDTO searchModel = new MasterTemplateSearchDTO();

                    var currentYearCode = $"MST_{DateTime.Now.ToString("yyyy")}";
                    var objDataList = _db.TCMasterTemplates.FromSqlRaw<TCMasterTemplate>("sp_GET_TCMasterTemplate {0}, {1}, {2}", currentYearCode, searchModel.mst_date_from, searchModel.mst_date_to).ToList();

                    if (objDataList.Count > 0)
                    {
                        var lastData = objDataList.LastOrDefault();
                        var lastCode = lastData.mst_code.Substring(7);
                        var newCodeInt = (Convert.ToInt32(lastCode) + 1);
                        new_mst_code = currentYearCode + newCodeInt.ToString("0000");
                    }
                    else
                    {
                        new_mst_code = currentYearCode + (1).ToString("0000");
                    }

                    var objCopyDataList = _db.TCMasterTemplates.FromSqlRaw<TCMasterTemplate>("sp_Copy_TCMasterTemplate {0}, {1}, {2}", model.mst_code, new_mst_code, model.mst_createuser).ToList();

                    if (objCopyDataList.Count > 0)
                    {
                        var objCopyData =  objCopyDataList.FirstOrDefault();
                        objReturn = _mapper.Map<MasterTemplateDTO>(objCopyData);
                    }

                    #region Save Log Process ...
                    _db.LogProcesss.Add(new LogProcess()
                    {
                        log_usr_id = currentUser,
                        log_mnu_id = "",
                        log_mnu_name = "MasterTemplate",
                        log_tran_id = model.mst_code,
                        log_action = "New",
                        log_desc = "Copy MasterTemplate " + model.mst_code + " From " + new_mst_code,
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

        public string CheckDuplicate(MasterTemplateDTO model)
        {
            log.MethodStart();

            string objReturn = "";

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objResult = _db.TCMasterTemplates.Any(x => x.mst_code == model.mst_code);

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
