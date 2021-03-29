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
    public class SpecimenService : ISpecimenService
    {
        private static readonly ILogService log = new LogService(typeof(SpecimenService));

        private readonly MasterManagementContext _db;
        private readonly IMapper _mapper;

        public SpecimenService(MasterManagementContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<SpecimenDTO> GetList()
        {
            log.MethodStart();

            List<SpecimenDTO> objList = new List<SpecimenDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCSpecimens.ToList();

                    objList = _mapper.Map<List<SpecimenDTO>>(objReturn1);

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

        public List<SpecimenDTO> GetListWithParam(string param)
        {
            log.MethodStart();

            List<SpecimenDTO> objList = new List<SpecimenDTO>();

            var searchModel = JsonSerializer.Deserialize<SpecimenDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TCSpecimens.FromSqlRaw<TCSpecimen>("sp_GET_TCSpecimen {0}", searchModel.spc_name).ToList();

                    objList = _mapper.Map<List<SpecimenDTO>>(objDataList);

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

        public List<SpecimenDTO> GetListWithModel(SpecimenDTO searchModel)
        {
            log.MethodStart();

            List<SpecimenDTO> objList = new List<SpecimenDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TCSpecimens.FromSqlRaw<TCSpecimen>("sp_GET_TCSpecimen {0}", searchModel.spc_mst_code).ToList();

                    objList = _mapper.Map<List<SpecimenDTO>>(objDataList);

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

        public List<SpecimenDTO> GetList_Active_WithModel(SpecimenDTO searchModel)
        {
            log.MethodStart();

            List<SpecimenDTO> objList = new List<SpecimenDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TCSpecimens.FromSqlRaw<TCSpecimen>("sp_GET_TCSpecimen_Active {0}", searchModel.spc_mst_code).ToList();

                    objList = _mapper.Map<List<SpecimenDTO>>(objDataList);

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

        public SpecimenDTO GetData(string spc_code)
        {
            log.MethodStart();

            SpecimenDTO objModel = new SpecimenDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCSpecimens.FirstOrDefault(x => x.spc_code == spc_code);

                    objModel = _mapper.Map<SpecimenDTO>(objReturn1);

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

        public SpecimenDTO SaveData(SpecimenDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            var currentUser = "";
            SpecimenDTO objReturn = new SpecimenDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TCSpecimen();

                    if (model.spc_status == "E")
                    {
                        objModel = _db.TCSpecimens.FirstOrDefault(x => x.spc_id == model.spc_id);
                    }

                    if (model.spc_status == "N")
                    {
                        objModel = _mapper.Map<TCSpecimen>(model);

                        objModel.spc_status = objModel.spc_active == true ? "A" : "I";
                        objModel.spc_createdate = currentDateTime;

                        currentUser = objModel.spc_createuser;

                        _db.TCSpecimens.Add(objModel);
                    }
                    else if (model.spc_status == "E")
                    {
                        objModel.spc_name = model.spc_name;
                        objModel.spc_desc = model.spc_desc;
                        objModel.spc_active = model.spc_active;
                        objModel.spc_status = objModel.spc_active == true ? "A" : "I";
                        objModel.spc_updateuser = model.spc_updateuser;
                        objModel.spc_updatedate = currentDateTime;

                        currentUser = objModel.spc_updateuser;

                        //_db.TCHospitals.Update(objModel);
                    }

                    #region Save Log Process ...
                    _db.LogProcesss.Add(new LogProcess()
                    {
                        log_usr_id = currentUser,
                        log_mnu_id = "",
                        log_mnu_name = "Specimen",
                        log_tran_id = $"{objModel.spc_mst_code}:{objModel.spc_code}",
                        log_action = (model.spc_status == "N" ? "New" : "Update"),
                        log_desc = (model.spc_status == "N" ? "New" : "Update") + " Specimen " + objModel.spc_name,
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

        public string CheckDuplicate(SpecimenDTO model)
        {
            log.MethodStart();

            string objReturn = "";

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objResult = _db.TCSpecimens.Any(x => x.spc_code == model.spc_code);

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
