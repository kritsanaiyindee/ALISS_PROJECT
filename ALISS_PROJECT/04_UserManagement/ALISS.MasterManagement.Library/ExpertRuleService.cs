using AutoMapper;
using Log4NetLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using ALISS.MasterManagement.DTO;
using ALISS.MasterManagement.Library.DataAccess;
using ALISS.MasterManagement.Library.Models;

namespace ALISS.MasterManagement.Library
{
    public class ExpertRuleService : IExpertRuleService
    {
        private static readonly ILogService log = new LogService(typeof(ExpertRuleService));

        private readonly MasterManagementContext _db;
        private readonly IMapper _mapper;

        public ExpertRuleService(MasterManagementContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<ExpertRuleDTO> GetList()
        {
            log.MethodStart();

            List<ExpertRuleDTO> objList = new List<ExpertRuleDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCExpertRoles.ToList();

                    objList = _mapper.Map<List<ExpertRuleDTO>>(objReturn1);

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

        public List<ExpertRuleDTO> GetListWithParam(string param)
        {
            log.MethodStart();

            List<ExpertRuleDTO> objList = new List<ExpertRuleDTO>();

            var searchModel = JsonSerializer.Deserialize<ExpertRuleDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TCExpertRoles.FromSqlRaw<TCExpertRule>("sp_GET_TCExpertRule {0}", searchModel.exr_mst_code).ToList();

                    objList = _mapper.Map<List<ExpertRuleDTO>>(objDataList);

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

        public List<ExpertRuleDTO> GetListWithModel(ExpertRuleDTO searchModel)
        {
            log.MethodStart();

            List<ExpertRuleDTO> objList = new List<ExpertRuleDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.ExpertRuleDTOs.FromSqlRaw<ExpertRuleDTO>("sp_GET_TCExpertRule {0}", searchModel.exr_mst_code).ToList();

                    //objList = _mapper.Map<List<ExpertRuleDTO>>(objDataList);

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

        public List<ExpertRuleDTO> GetList_Active_WithModel(ExpertRuleDTO searchModel)
        {
            log.MethodStart();

            List<ExpertRuleDTO> objList = new List<ExpertRuleDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.ExpertRuleDTOs.FromSqlRaw<ExpertRuleDTO>("sp_GET_TCExpertRule_Active {0}", searchModel.exr_mst_code).ToList();

                    //objList = _mapper.Map<List<ExpertRuleDTO>>(objDataList);

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

        public ExpertRuleDTO GetData(string exr_code)
        {
            log.MethodStart();

            ExpertRuleDTO objModel = new ExpertRuleDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCExpertRoles.FirstOrDefault(x => x.exr_code == exr_code);

                    objModel = _mapper.Map<ExpertRuleDTO>(objReturn1);

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

        public ExpertRuleDTO SaveData(ExpertRuleDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            var currentUser = "";
            ExpertRuleDTO objReturn = new ExpertRuleDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TCExpertRule();

                    if (model.exr_status == "E")
                    {
                        objModel = _db.TCExpertRoles.FirstOrDefault(x => x.exr_id == model.exr_id);
                    }

                    if (model.exr_status == "N")
                    {
                        objModel = _mapper.Map<TCExpertRule>(model);

                        objModel.CATEGORY = model.exr_mst_CATEGORY;
                        objModel.PRIORITY = model.exr_mst_PRIORITY;
                        objModel.ORGANISMS = model.exr_mst_ORGANISMS;
                        objModel.DESCRIPTION = model.exr_mst_DESCRIPTION;
                        objModel.exr_status = objModel.exr_active == true ? "A" : "I";
                        objModel.exr_createdate = currentDateTime;

                        currentUser = objModel.exr_createuser;

                        _db.TCExpertRoles.Add(objModel);
                    }
                    else if (model.exr_status == "E")
                    {
                        objModel.exr_code = model.exr_code;
                        objModel.CATEGORY = model.exr_mst_CATEGORY;
                        objModel.PRIORITY = model.exr_mst_PRIORITY;
                        objModel.ORGANISMS = model.exr_mst_ORGANISMS;
                        objModel.DESCRIPTION = model.exr_mst_DESCRIPTION;
                        objModel.exr_active = model.exr_active;
                        objModel.exr_status = objModel.exr_active == true ? "A" : "I";
                        objModel.exr_updateuser = model.exr_updateuser;
                        objModel.exr_updatedate = currentDateTime;

                        currentUser = objModel.exr_updateuser;

                        //_db.TCHospitals.Update(objModel);
                    }

                    #region Save Log Process ...
                    _db.LogProcesss.Add(new LogProcess()
                    {
                        log_usr_id = currentUser,
                        log_mnu_id = "",
                        log_mnu_name = "ExpertRole",
                        log_tran_id = $"{objModel.exr_mst_code}:{objModel.exr_code}",
                        log_action = (model.exr_status == "N" ? "New" : "Update"),
                        log_desc = (model.exr_status == "N" ? "New" : "Update") + " ExpertRole " + objModel.exr_code,
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

        public string CheckDuplicate(ExpertRuleDTO model)
        {
            log.MethodStart();

            string objReturn = "";

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objResult = _db.TCExpertRoles.Any(x => x.exr_code == model.exr_code);

                    if(objResult == true)
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
