using ALISS.Process.DTO;
using ALISS.Process.Library.DataAccess;
using ALISS.Process.Library.Models;
using AutoMapper;
using Log4NetLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ALISS.Process.Library
{
    public class ProcessRequestService : IProcessRequestService
    {
        private static readonly ILogService log = new LogService(typeof(ProcessRequestService));

        private readonly ProcessContext _db;
        private readonly IMapper _mapper;

        public ProcessRequestService(ProcessContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<ProcessRequestDTO> GetList()
        {
            log.MethodStart();

            List<ProcessRequestDTO> objList = new List<ProcessRequestDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TRProcessRequests.ToList();

                    objList = _mapper.Map<List<ProcessRequestDTO>>(objReturn1);

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

        public List<ProcessRequestDTO> GetListWithParam(string param)
        {
            log.MethodStart();

            List<ProcessRequestDTO> objList = new List<ProcessRequestDTO>();

            var searchModel = JsonSerializer.Deserialize<ProcessRequestDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TRProcessRequests.ToList();

                    objList = _mapper.Map<List<ProcessRequestDTO>>(objReturn1);

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

        public List<ProcessRequestDTO> GetListWithModel(ProcessRequestDTO searchModel)
        {
            log.MethodStart();

            List<ProcessRequestDTO> objList = new List<ProcessRequestDTO>();

            //var searchModel = JsonSerializer.Deserialize<HospitalDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.ProcessRequestDTOs.FromSqlRaw<ProcessRequestDTO>("sp_GET_TRProcessRequest_DTO {0}, {1}, {2}, {3}, {4}, {5}, {6}", searchModel.pcr_code, searchModel.pcr_arh_code, searchModel.pcr_prv_code, searchModel.pcr_hos_code, searchModel.pcr_month_start, searchModel.pcr_month_end, searchModel.pcr_year).ToList();

                    //objList = _mapper.Map<List<ProcessRequestDTO>>(objReturn1);

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

        public List<ProcessRequestCheckDetailDTO> GetDetailListWithModel(ProcessRequestDTO searchModel)
        {
            log.MethodStart();

            List<ProcessRequestCheckDetailDTO> objList = new List<ProcessRequestCheckDetailDTO>();

            //var searchModel = JsonSerializer.Deserialize<HospitalDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var date_start = new DateTime(Convert.ToInt32(searchModel.pcr_year), Convert.ToInt32(searchModel.pcr_month_start), 1);
                    var date_end = new DateTime(Convert.ToInt32(searchModel.pcr_year), Convert.ToInt32(searchModel.pcr_month_end), 1).AddMonths(1);

                    var objReturnList = _db.ProcessRequestDetailDTOs.FromSqlRaw<ProcessRequestDetailDTO>("sp_GET_TRProcessRequestDetail_DTO {0}, {1}, {2}, {3}, {4}, {5}, {6}", "E", searchModel.pcr_code, searchModel.pcr_arh_code, searchModel.pcr_prv_code, searchModel.pcr_hos_code, date_start, date_start).ToList();

                    objList = _mapper.Map<List<ProcessRequestCheckDetailDTO>>(objReturnList);

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

        public List<ProcessRequestCheckDetailDTO> GetCheckDetailListWithModel(ProcessRequestDTO searchModel)
        {
            log.MethodStart();

            List<ProcessRequestCheckDetailDTO> objList = new List<ProcessRequestCheckDetailDTO>();

            //var searchModel = JsonSerializer.Deserialize<HospitalDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var date_start = new DateTime(Convert.ToInt32(searchModel.pcr_year), Convert.ToInt32(searchModel.pcr_month_start), 1);
                    var date_end = new DateTime(Convert.ToInt32(searchModel.pcr_year), Convert.ToInt32(searchModel.pcr_month_end), 1).AddMonths(1);

                    var objReturnList = _db.ProcessRequestDetailDTOs.FromSqlRaw<ProcessRequestDetailDTO>("sp_GET_TRProcessRequestDetail_DTO {0}, {1}, {2}, {3}, {4}, {5}, {6}", "C", null, searchModel.pcr_arh_code, searchModel.pcr_prv_code, searchModel.pcr_hos_code, date_start, date_end).ToList();

                    objList = _mapper.Map<List<ProcessRequestCheckDetailDTO>>(objReturnList);

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

        public ProcessRequestDTO GetData(string pcr_code)
        {
            log.MethodStart();

            ProcessRequestDTO objModel = new ProcessRequestDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    //var objReturn1 = _db.TRProcessRequests.FirstOrDefault(x => x.pcr_code == pcr_code);
                    var objReturn1 = _db.TRProcessRequests.FromSqlRaw<TRProcessRequest>("sp_GET_TRProcessRequest {0}, {1}, {2}, {3}, {4}, {5}, {6}", pcr_code, null, null, null, null, null, null).ToList();

                    if (objReturn1 != null)
                    {
                        objModel = _mapper.Map<ProcessRequestDTO>(objReturn1.FirstOrDefault());
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

        public ProcessRequestDTO GetDataWithModel(ProcessRequestDTO searchModel)
        {
            log.MethodStart();

            ProcessRequestDTO objModel = new ProcessRequestDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    //var objReturn1 = _db.TRProcessRequests.FirstOrDefault(x => x.pcr_code == pcr_code);
                    var objReturn1 = _db.TRProcessRequests.Where(x => x.pcr_hos_code == searchModel.pcr_hos_code
                        && x.pcr_prv_code == searchModel.pcr_prv_code
                        && x.pcr_arh_code == searchModel.pcr_arh_code
                        && x.pcr_month_start == searchModel.pcr_month_start
                        && x.pcr_month_end == searchModel.pcr_month_end
                        && x.pcr_year == searchModel.pcr_year
                        && x.pcr_type == searchModel.pcr_type
                        && x.pcr_spc_code == searchModel.pcr_spc_code
                    ).ToList();

                    if (objReturn1 != null && objReturn1.Count > 0)
                    {
                        objModel = _mapper.Map<ProcessRequestDTO>(objReturn1.FirstOrDefault());
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

        public ProcessRequestDTO SaveData(ProcessRequestDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            var currentUser = "";
            var new_pcr_code = "";
            ProcessRequestDTO objReturn = new ProcessRequestDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TRProcessRequest();

                    objModel = _mapper.Map<TRProcessRequest>(model);

                    var currentYearCode = $"PCR_{DateTime.Now.ToString("yyyyMMdd")}";
                    var objDataList = _db.TRProcessRequests.Where(x => x.pcr_code.Contains(currentYearCode)).ToList();

                    if (objDataList.Count > 0)
                    {
                        var lastData = objDataList.LastOrDefault();
                        var lastCode = lastData.pcr_code.Replace(currentYearCode, "");
                        var newCodeInt = (Convert.ToInt32(lastCode) + 1);
                        new_pcr_code = currentYearCode + newCodeInt.ToString("0000");
                    }
                    else
                    {
                        new_pcr_code = currentYearCode + (1).ToString("0000");
                    }

                    objModel.pcr_id = 0;
                    objModel.pcr_code = new_pcr_code;
                    objModel.pcr_status = "W";
                    objModel.pcr_active = true;
                    objModel.pcr_status = objModel.pcr_active == true ? "A" : "I";
                    objModel.pcr_createdate = currentDateTime;
                    objModel.pcr_updatedate = currentDateTime;

                    currentUser = objModel.pcr_updateuser;

                    _db.TRProcessRequests.Add(objModel);

                    var objData = _db.TRProcessRequests.FirstOrDefault(x => x.pcr_code == model.pcr_prev_code);
                    if(objData != null)
                    {
                        objData.pcr_status = "I";
                        objData.pcr_active = false;
                        objData.pcr_updateuser = model.pcr_updateuser;
                        objData.pcr_updatedate = currentDateTime;
                    }

                    #region Save Log Process ...
                    _db.LogProcesss.Add(new LogProcess()
                    {
                        log_usr_id = currentUser,
                        log_mnu_id = "",
                        log_mnu_name = "ProcessRequest",
                        log_tran_id = objModel.pcr_id.ToString(),
                        log_action = (model.pcr_status == "N" ? "New" : "Update"),
                        log_desc = "Update ProcessRequest " + objModel.pcr_code,
                        log_createuser = currentUser,
                        log_createdate = currentDateTime
                    });
                    #endregion

                    _db.SaveChanges();

                    trans.Commit();

                    objReturn = _mapper.Map<ProcessRequestDTO>(objModel);
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

        public ProcessRequestDTO SaveDataToPublic(string pcr_code)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            var currentUser = "";
            ProcessRequestDTO objReturn = new ProcessRequestDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TRProcessRequest();

                    objModel = _db.TRProcessRequests.FirstOrDefault(x => x.pcr_code == pcr_code);

                    if (objModel != null)
                    {
                        objModel.pcr_status = "P";
                        objModel.pcr_updateuser = currentUser;
                        objModel.pcr_updatedate = currentDateTime;

                        #region Save Log Process ...
                        _db.LogProcesss.Add(new LogProcess()
                        {
                            log_usr_id = currentUser,
                            log_mnu_id = "",
                            log_mnu_name = "ProcessRequest",
                            log_tran_id = objModel.pcr_id.ToString(),
                            log_action = (objModel.pcr_status == "N" ? "New" : "Update"),
                            log_desc = "Update ProcessRequest " + objModel.pcr_code,
                            log_createuser = currentUser,
                            log_createdate = currentDateTime
                        });
                        #endregion

                        _db.SaveChanges();

                        trans.Commit();
                    }

                    objReturn = _mapper.Map<ProcessRequestDTO>(objModel);
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

        public List<ProcessRequestDetailDTO> SaveDetailData(List<ProcessRequestDetailDTO> modelList)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            var currentUser = "";

            List<ProcessRequestDetailDTO> objReturn = new List<ProcessRequestDetailDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in modelList)
                    {
                        var objModel = new TRProcessRequestDetail();

                        objModel = _mapper.Map<TRProcessRequestDetail>(item);

                        objModel.pcd_createdate = currentDateTime;
                        objModel.pcd_updatedate = currentDateTime;
                        objModel.pcd_status = objModel.pcd_active == true ? "A" : "I";

                        _db.TRProcessRequestDetails.Add(objModel);

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

    }
}
