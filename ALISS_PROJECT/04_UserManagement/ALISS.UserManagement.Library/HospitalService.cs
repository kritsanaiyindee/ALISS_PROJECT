using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using AutoMapper;
using Log4NetLibrary;
using ALISS.UserManagement.Library.DataAccess;
using ALISS.UserManagement.Library.Models;
using ALISS.UserManagement.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ALISS.UserManagement.Library
{
    public class HospitalService : IHospitalService
    {
        private static readonly ILogService log = new LogService(typeof(HospitalService));
        
        private readonly UserManagementContext _db;
        private readonly IMapper _mapper;

        public HospitalService(UserManagementContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<HospitalDTO> GetList()
        {
            log.MethodStart();

            List<HospitalDTO> objList = new List<HospitalDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TRHospitals.ToList();

                    objList = _mapper.Map<List<HospitalDTO>>(objReturn1);

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

        public List<HospitalDTO> GetListWithParam(string param)
        {
            log.MethodStart();

            List<HospitalDTO> objList = new List<HospitalDTO>();

            var searchModel = JsonSerializer.Deserialize<HospitalDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TRHospitals.ToList();

                    objList = _mapper.Map<List<HospitalDTO>>(objReturn1);

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

        public List<HospitalDTO> GetListWithModel(HospitalSearchDTO searchModel)
        {
            log.MethodStart();

            List<HospitalDTO> objList = new List<HospitalDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.HospitalDTOs.FromSqlRaw<HospitalDTO>("sp_GET_TRHospital {0}, {1}, {2}", searchModel.hos_name, searchModel.hos_prv_code, searchModel.hos_arh_code).ToList();

                    //objList = _mapper.Map<List<HospitalDTO>>(objDataList);

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

        public HospitalDTO GetData(string hos_code)
        {
            log.MethodStart();

            HospitalDTO objModel = new HospitalDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TRHospitals.FirstOrDefault(x => x.hos_code == hos_code);

                    objModel = _mapper.Map<HospitalDTO>(objReturn1);

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

        public List<HospitalLabDTO> GetLabListWithModel(HospitalSearchDTO searchModel)
        {
            log.MethodStart();

            List<HospitalLabDTO> objList = new List<HospitalLabDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TRHospitalLabs.Where(x => x.lab_hos_code == searchModel.hos_code).ToList();

                    objList = _mapper.Map<List<HospitalLabDTO>>(objDataList);

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

        public HospitalLabDTO GetLabData(string hol_code)
        {
            log.MethodStart();

            HospitalLabDTO objModel = new HospitalLabDTO();

            var paramSplit = hol_code.Split(':');

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TRHospitalLabs.FirstOrDefault(x => x.lab_hos_code == paramSplit[0] && x.lab_code == paramSplit[1]);

                    objModel = _mapper.Map<HospitalLabDTO>(objReturn1);

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

        public HospitalDTO SaveData(HospitalDTO model)
        {
            log.MethodStart();

            DateTime currentDateTime = DateTime.Now;
            var currentUser = "";
            HospitalDTO objReturn = new HospitalDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TRHospital();

                    if (model.hos_status == "E")
                    {
                        objModel = _db.TRHospitals.FirstOrDefault(x => x.hos_id == model.hos_id);
                    }

                    if (model.hos_status == "N")
                    {
                        objModel = _mapper.Map<TRHospital>(model);

                        objModel.hos_status = objModel.hos_active == true ? "A" : "I";
                        objModel.hos_createdate = currentDateTime;

                        currentUser = objModel.hos_createuser;

                        _db.TRHospitals.Add(objModel);
                    }
                    else if (model.hos_status == "E")
                    {
                        objModel.hos_active = model.hos_active;
                        objModel.hos_status = objModel.hos_active == true ? "A" : "I";
                        objModel.hos_updateuser = model.hos_updateuser;
                        objModel.hos_updatedate = currentDateTime;

                        currentUser = objModel.hos_updateuser;

                        //_db.TCMenus.Update(objModel);
                    }

                    #region Save Log Process ...
                    _db.LogProcesss.Add(new LogProcess()
                    {
                        log_usr_id = currentUser,
                        log_mnu_id = "",
                        log_mnu_name = "Hospital",
                        log_tran_id = objModel.hos_code.ToString(),
                        log_action = (model.hos_status == "N" ? "New" : "Update"),
                        log_desc = (model.hos_status == "N" ? "New" : "Update") + " Hospital " + objModel.hos_name,
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

        public List<HospitalLabDTO> SaveLabData(List<HospitalLabDTO> modelList)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            List<HospitalLabDTO> objReturn = new List<HospitalLabDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in modelList)
                    {
                        //var objModel = _db.TRHospitalLabs.FirstOrDefault(x => x.lab_id == item.lab_id);
                        var objModel = new TRHospitalLab();

                        if (item.lab_status == "E")
                        {
                            objModel = _db.TRHospitalLabs.FirstOrDefault(x => x.lab_id == item.lab_id);
                        }

                        if (item.lab_status == "N")
                        {
                            objModel = _mapper.Map<TRHospitalLab>(item);

                            objModel.lab_createdate = currentDateTime;

                            _db.TRHospitalLabs.Add(objModel);
                        }
                        else if (item.lab_status == "E")
                        {
                            objModel.lab_name = item.lab_name;
                            objModel.lab_type = item.lab_type;
                            objModel.lab_prg_type = item.lab_prg_type;

                            objModel.lab_updateuser = item.lab_updateuser;
                            objModel.lab_updatedate = currentDateTime;

                            //_db.TCHospitals.Update(objModel);
                        }

                        objModel.lab_status = objModel.lab_active == true ? "A" : "I";

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
