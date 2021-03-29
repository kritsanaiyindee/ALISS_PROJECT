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
    public class WHONETColumnService : IWHONETColumnService
    {
        private static readonly ILogService log = new LogService(typeof(WHONETColumnService));

        private readonly MasterManagementContext _db;
        private readonly IMapper _mapper;

        public WHONETColumnService(MasterManagementContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<WHONETColumnDTO> GetList()
        {
            log.MethodStart();

            List<WHONETColumnDTO> objList = new List<WHONETColumnDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCWHONETColumns.ToList();

                    objList = _mapper.Map<List<WHONETColumnDTO>>(objReturn1);

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

        public List<WHONETColumnDTO> GetListWithParam(string param)
        {
            log.MethodStart();

            List<WHONETColumnDTO> objList = new List<WHONETColumnDTO>();

            var searchModel = JsonSerializer.Deserialize<WHONETColumnDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TCWHONETColumns.FromSqlRaw<TCWHONETColumn>("sp_GET_TCWHONETColumn {0}", searchModel.wnc_name).ToList();

                    objList = _mapper.Map<List<WHONETColumnDTO>>(objDataList);

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

        public List<WHONETColumnDTO> GetListWithModel(WHONETColumnDTO searchModel)
        {
            log.MethodStart();

            List<WHONETColumnDTO> objList = new List<WHONETColumnDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TCWHONETColumns.FromSqlRaw<TCWHONETColumn>("sp_GET_TCWHONETColumn {0}", searchModel.wnc_mst_code).ToList();

                    objList = _mapper.Map<List<WHONETColumnDTO>>(objDataList);

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

        public List<WHONETColumnDTO> GetList_Active_WithModel(WHONETColumnDTO searchModel)
        {
            log.MethodStart();

            List<WHONETColumnDTO> objList = new List<WHONETColumnDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TCWHONETColumns.FromSqlRaw<TCWHONETColumn>("sp_GET_TCWHONETColumn_Active {0}", searchModel.wnc_mst_code).ToList();

                    objList = _mapper.Map<List<WHONETColumnDTO>>(objDataList);

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

        public WHONETColumnDTO GetData(string wnc_code)
        {
            log.MethodStart();

            WHONETColumnDTO objModel = new WHONETColumnDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCWHONETColumns.FirstOrDefault(x => x.wnc_code == wnc_code);

                    objModel = _mapper.Map<WHONETColumnDTO>(objReturn1);

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

        public WHONETColumnDTO SaveData(WHONETColumnDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            var currentUser = "";
            WHONETColumnDTO objReturn = new WHONETColumnDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TCWHONETColumn();

                    if (model.wnc_status == "E")
                    {
                        objModel = _db.TCWHONETColumns.FirstOrDefault(x => x.wnc_id == model.wnc_id);
                    }

                    if (model.wnc_status == "N")
                    {
                        objModel = _mapper.Map<TCWHONETColumn>(model);

                        objModel.wnc_status = objModel.wnc_active == true ? "A" : "I";
                        objModel.wnc_createdate = currentDateTime;

                        currentUser = objModel.wnc_createuser;

                        _db.TCWHONETColumns.Add(objModel);
                    }
                    else if (model.wnc_status == "E")
                    {
                        objModel.wnc_name = model.wnc_name;
                        objModel.wnc_data_type = model.wnc_data_type;
                        objModel.wnc_encrypt = model.wnc_encrypt;
                        objModel.wnc_active = model.wnc_active;
                        objModel.wnc_status = objModel.wnc_active == true ? "A" : "I";
                        objModel.wnc_updateuser = model.wnc_updateuser;
                        objModel.wnc_updatedate = currentDateTime;

                        currentUser = objModel.wnc_updateuser;

                        //_db.TCHospitals.Update(objModel);
                    }

                    #region Save Log Process ...
                    _db.LogProcesss.Add(new LogProcess()
                    {
                        log_usr_id = currentUser,
                        log_mnu_id = "",
                        log_mnu_name = "WHONETColumn",
                        log_tran_id =  $"{objModel.wnc_mst_code}:{objModel.wnc_code}",
                        log_action = (model.wnc_status == "N" ? "New" : "Update"),
                        log_desc = (model.wnc_status == "N" ? "New" : "Update") + " WHONETColumn " + objModel.wnc_name,
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

        public string CheckDuplicate(WHONETColumnDTO model)
        {
            log.MethodStart();

            string objReturn = "";

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objResult = _db.TCWHONETColumns.Any(x => x.wnc_code == model.wnc_code);

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
