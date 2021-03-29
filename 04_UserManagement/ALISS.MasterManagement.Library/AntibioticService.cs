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
    public class AntibioticService : IAntibioticService
    {
        private static readonly ILogService log = new LogService(typeof(AntibioticService));

        private readonly MasterManagementContext _db;
        private readonly IMapper _mapper;

        public AntibioticService(MasterManagementContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<AntibioticDTO> GetList()
        {
            log.MethodStart();

            List<AntibioticDTO> objList = new List<AntibioticDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCAntibiotics.ToList();

                    objList = _mapper.Map<List<AntibioticDTO>>(objReturn1);

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

        public List<AntibioticDTO> GetListWithParam(string param)
        {
            log.MethodStart();

            List<AntibioticDTO> objList = new List<AntibioticDTO>();

            var searchModel = JsonSerializer.Deserialize<AntibioticDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TCAntibiotics.FromSqlRaw<TCAntibiotic>("sp_GET_TCAntibiotic {0}", searchModel.ant_code).ToList();

                    objList = _mapper.Map<List<AntibioticDTO>>(objDataList);

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

        public List<AntibioticDTO> GetListWithModel(AntibioticDTO searchModel)
        {
            log.MethodStart();

            List<AntibioticDTO> objList = new List<AntibioticDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.AntibioticDTOs.FromSqlRaw<AntibioticDTO>("sp_GET_TCAntibiotic {0}", searchModel.ant_mst_code).ToList();

                    //objList = _mapper.Map<List<AntibioticDTO>>(objDataList);

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

        public List<AntibioticDTO> GetList_Active_WithModel(AntibioticDTO searchModel)
        {
            log.MethodStart();

            List<AntibioticDTO> objList = new List<AntibioticDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.AntibioticDTOs.FromSqlRaw<AntibioticDTO>("sp_GET_TCAntibiotic_Active {0}", searchModel.ant_mst_code).ToList();

                    //objList = _mapper.Map<List<AntibioticDTO>>(objDataList);

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

        public AntibioticDTO GetData(string ant_code)
        {
            log.MethodStart();

            AntibioticDTO objModel = new AntibioticDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCAntibiotics.FirstOrDefault(x => x.ant_code == ant_code);

                    objModel = _mapper.Map<AntibioticDTO>(objReturn1);

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

        public AntibioticDTO GetDataById(int ant_code)
        {
            log.MethodStart();

            AntibioticDTO objModel = new AntibioticDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCAntibiotics.FirstOrDefault(x => x.ant_id == ant_code);

                    objModel = _mapper.Map<AntibioticDTO>(objReturn1);

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

        public AntibioticDTO SaveData(AntibioticDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            var currentUser = "";
            AntibioticDTO objReturn = new AntibioticDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TCAntibiotic();

                    if (model.ant_status == "E")
                    {
                        objModel = _db.TCAntibiotics.FirstOrDefault(x => x.ant_id == model.ant_id);
                    }

                    if (model.ant_status == "N")
                    {
                        objModel = _mapper.Map<TCAntibiotic>(model);

                        objModel.ant_status = objModel.ant_active == true ? "A" : "I";
                        objModel.ant_createdate = currentDateTime;

                        currentUser = objModel.ant_createuser;

                        _db.TCAntibiotics.Add(objModel);
                    }
                    else if (model.ant_status == "E")
                    {
                        objModel.ant_code = model.ant_code;
                        objModel.ant_name = model.ant_name;
                        objModel.GUIDELINES = model.ant_mst_GUIDELINES;
                        objModel.ABX_NUMBER = model.ant_mst_ABX_NUMBER;
                        objModel.POTENCY = model.ant_mst_POTENCY;
                        objModel.ant_active = model.ant_active;
                        objModel.ant_status = objModel.ant_active == true ? "A" : "I";
                        objModel.ant_updateuser = model.ant_updateuser;
                        objModel.ant_updatedate = currentDateTime;

                        currentUser = objModel.ant_updateuser;

                        //_db.TCHospitals.Update(objModel);
                    }

                    #region Save Log Process ...
                    _db.LogProcesss.Add(new LogProcess()
                    {
                        log_usr_id = currentUser,
                        log_mnu_id = "",
                        log_mnu_name = "Antibiotic",
                        log_tran_id = $"{objModel.ant_mst_code}:{objModel.ant_code}",
                        log_action = (model.ant_status == "N" ? "New" : "Update"),
                        log_desc = (model.ant_status == "N" ? "New" : "Update") + " Antibiotic " + objModel.ant_code,
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

            return model;
        }

        public string CheckDuplicate(AntibioticDTO model)
        {
            log.MethodStart();

            string objReturn = "";

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objResult = _db.TCAntibiotics.Any(x => x.ant_code == model.ant_code);

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
