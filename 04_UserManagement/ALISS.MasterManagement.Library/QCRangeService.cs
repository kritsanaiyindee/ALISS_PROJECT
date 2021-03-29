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
    public class QCRangeService : IQCRangeService
    {
        private static readonly ILogService log = new LogService(typeof(QCRangeService));

        private readonly MasterManagementContext _db;
        private readonly IMapper _mapper;

        public QCRangeService(MasterManagementContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<QCRangeDTO> GetList()
        {
            log.MethodStart();

            List<QCRangeDTO> objList = new List<QCRangeDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCQCRanges.ToList();

                    objList = _mapper.Map<List<QCRangeDTO>>(objReturn1);

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

        public List<QCRangeDTO> GetListWithParam(string param)
        {
            log.MethodStart();

            List<QCRangeDTO> objList = new List<QCRangeDTO>();

            var searchModel = JsonSerializer.Deserialize<QCRangeDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TCQCRanges.FromSqlRaw<TCQCRange>("sp_GET_TCQCRange {0}", searchModel.qcr_code).ToList();

                    objList = _mapper.Map<List<QCRangeDTO>>(objDataList);

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

        public List<QCRangeDTO> GetListWithModel(QCRangeDTO searchModel)
        {
            log.MethodStart();

            List<QCRangeDTO> objList = new List<QCRangeDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.QCRangeDTOs.FromSqlRaw<QCRangeDTO>("sp_GET_TCQCRange {0}", searchModel.qcr_mst_code).ToList();

                    //objList = _mapper.Map<List<QCRangeDTO>>(objDataList);

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

        public List<QCRangeDTO> GetList_Active_WithModel(QCRangeDTO searchModel)
        {
            log.MethodStart();

            List<QCRangeDTO> objList = new List<QCRangeDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.QCRangeDTOs.FromSqlRaw<QCRangeDTO>("sp_GET_TCQCRange_Active {0}", searchModel.qcr_mst_code).ToList();

                    //objList = _mapper.Map<List<QCRangeDTO>>(objDataList);

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

        public QCRangeDTO GetData(string qcr_code)
        {
            log.MethodStart();

            QCRangeDTO objModel = new QCRangeDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCQCRanges.FirstOrDefault(x => x.qcr_code == qcr_code);

                    objModel = _mapper.Map<QCRangeDTO>(objReturn1);

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

        public QCRangeDTO SaveData(QCRangeDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            var currentUser = "";
            QCRangeDTO objReturn = new QCRangeDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TCQCRange();

                    if (model.qcr_status == "E")
                    {
                        objModel = _db.TCQCRanges.FirstOrDefault(x => x.qcr_id == model.qcr_id);
                    }

                    if (model.qcr_status == "N")
                    {
                        objModel = _mapper.Map<TCQCRange>(model);

                        objModel.GUIDELINE = model.qcr_mst_GUIDELINE;
                        objModel.ORGANISM = model.qcr_mst_ORGANISM;
                        objModel.ANTIBIOTIC = model.qcr_mst_ANTIBIOTIC;
                        objModel.QC_RANGE = model.qcr_mst_QC_RANGE;

                        objModel.qcr_status = objModel.qcr_active == true ? "A" : "I";
                        objModel.qcr_createdate = currentDateTime;

                        currentUser = objModel.qcr_createuser;

                        _db.TCQCRanges.Add(objModel);
                    }
                    else if (model.qcr_status == "E")
                    {
                        objModel.qcr_code = model.qcr_code;
                        objModel.GUIDELINE = model.qcr_mst_GUIDELINE;
                        objModel.ORGANISM = model.qcr_mst_ORGANISM;
                        objModel.ANTIBIOTIC = model.qcr_mst_ANTIBIOTIC;
                        objModel.QC_RANGE = model.qcr_mst_QC_RANGE;
                        objModel.qcr_active = model.qcr_active;
                        objModel.qcr_status = objModel.qcr_active == true ? "A" : "I";
                        objModel.qcr_updateuser = model.qcr_updateuser;
                        objModel.qcr_updatedate = currentDateTime;

                        currentUser = objModel.qcr_updateuser;

                        //_db.TCHospitals.Update(objModel);
                    }

                    #region Save Log Process ...
                    _db.LogProcesss.Add(new LogProcess()
                    {
                        log_usr_id = currentUser,
                        log_mnu_id = "",
                        log_mnu_name = "QCRange",
                        log_tran_id = $"{objModel.qcr_mst_code}:{objModel.qcr_code}",
                        log_action = (model.qcr_status == "N" ? "New" : "Update"),
                        log_desc = (model.qcr_status == "N" ? "New" : "Update") + " QCRange " + objModel.qcr_code,
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

        public string CheckDuplicate(QCRangeDTO model)
        {
            log.MethodStart();

            string objReturn = "";

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objResult = _db.TCQCRanges.Any(x => x.qcr_code == model.qcr_code);

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
