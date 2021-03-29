using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using AutoMapper;
using Log4NetLibrary;
using ALISS.MasterManagement.Library.DataAccess;
using ALISS.MasterManagement.Library.Models;
using ALISS.MasterManagement.DTO;
using Microsoft.EntityFrameworkCore;

namespace ALISS.MasterManagement.Library
{
    public class MasterHospitalService : IMasterHospitalService
    {
        private static readonly ILogService log = new LogService(typeof(MasterHospitalService));
        
        private readonly MasterManagementContext _db;
        private readonly IMapper _mapper;

        public MasterHospitalService(MasterManagementContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<MasterHospitalDTO> GetList()
        {
            log.MethodStart();

            List<MasterHospitalDTO> objList = new List<MasterHospitalDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCHospitals.ToList();

                    objList = _mapper.Map<List<MasterHospitalDTO>>(objReturn1);

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

        public List<MasterHospitalDTO> GetListWithParam(string param)
        {
            log.MethodStart();

            List<MasterHospitalDTO> objList = new List<MasterHospitalDTO>();

            var searchModel = JsonSerializer.Deserialize<MasterHospitalSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TCHospitals.FromSqlRaw<TCHospital>("sp_GET_TCHospital {0}", searchModel.hos_name).ToList();

                    objList = _mapper.Map<List<MasterHospitalDTO>>(objDataList);

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

        public List<MasterHospitalDTO> GetListWithModel(MasterHospitalSearchDTO searchModel)
        {
            log.MethodStart();

            List<MasterHospitalDTO> objList = new List<MasterHospitalDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.MasterHospitalDTOs.FromSqlRaw<MasterHospitalDTO>("sp_GET_MasterHospital {0}, {1}, {2}", searchModel.hos_name, searchModel.hos_province_code, searchModel.hos_arh_code).ToList();

                    //objList = _mapper.Map<List<MasterHospitalDTO>>(objDataList);

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

        public MasterHospitalDTO GetData(string hos_code)
        {
            log.MethodStart();

            MasterHospitalDTO objModel = new MasterHospitalDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCHospitals.FirstOrDefault(x => x.hos_code == hos_code);

                    if (objReturn1 != null)
                    {
                        objModel = _mapper.Map<MasterHospitalDTO>(objReturn1);
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

        public MasterHospitalDTO SaveData(MasterHospitalDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            var currentUser = "";
            MasterHospitalDTO objReturn = new MasterHospitalDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TCHospital();
                    var objModelTR = new TRHospital();

                    if (model.hos_status == "E")
                    {
                        objModel = _db.TCHospitals.FirstOrDefault(x => x.hos_id == model.hos_id);
                        objModelTR = _db.TRHospitals.FirstOrDefault(x => x.hos_code == objModel.hos_code);
                    }

                    if (model.hos_status == "N")
                    {
                        objModel = _mapper.Map<TCHospital>(model);

                        objModel.hos_status = objModel.hos_active == true ? "A" : "I";
                        objModel.hos_createdate = currentDateTime;

                        currentUser = objModel.hos_createuser;

                        _db.TCHospitals.Add(objModel);
                    }
                    else if (model.hos_status == "E")
                    {
                        objModel.hos_name = model.hos_name;
                        objModel.hos_arh_code = model.hos_arh_code;
                        objModel.hos_province_code = model.hos_province_code;
                        objModel.hos_active = model.hos_active;
                        objModel.hos_status = objModel.hos_active == true ? "A" : "I";
                        objModel.hos_updatedate = currentDateTime;

                        currentUser = objModel.hos_updateuser;

                        if (objModelTR != null)
                        {
                            objModelTR.hos_name = model.hos_name;
                            objModelTR.hos_arh_code = model.hos_arh_code;
                            objModelTR.hos_prv_code = model.hos_province_code;
                            objModelTR.hos_active = model.hos_active;
                            objModelTR.hos_status = objModel.hos_active == true ? "A" : "I";
                            objModelTR.hos_updateuser = currentUser;
                            objModelTR.hos_updatedate = currentDateTime;
                        }
                        //_db.TCHospitals.Update(objModel);
                    }

                    #region Save Log Process ...
                    _db.LogProcesss.Add(new LogProcess()
                    {
                        log_usr_id = currentUser,
                        log_mnu_id = "",
                        log_mnu_name = "MasterHospital",
                        log_tran_id = objModel.hos_code,
                        log_action = (model.hos_status == "N" ? "New" : "Update"),
                        log_desc = (model.hos_status == "N" ? "New" : "Update") + " MasterHospital " + objModel.hos_name,
                        log_createuser = currentUser,
                        log_createdate = currentDateTime
                    });

                    if (model.hos_status == "E" && objModelTR != null)
                    {
                        _db.LogProcesss.Add(new LogProcess()
                        {
                            log_usr_id = currentUser,
                            log_mnu_id = "",
                            log_mnu_name = "Hospital",
                            log_tran_id = objModel.hos_code.ToString(),
                            log_action = "Update",
                            log_desc = "Update Hospital " + objModel.hos_name,
                            log_createuser = currentUser,
                            log_createdate = currentDateTime
                        });
                    }
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

    }
}
