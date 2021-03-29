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
    public class WardTypeService : IWardTypeService
    {
        private static readonly ILogService log = new LogService(typeof(WardTypeService));

        private readonly MasterManagementContext _db;
        private readonly IMapper _mapper;

        public WardTypeService(MasterManagementContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<WardTypeDTO> GetList()
        {
            log.MethodStart();

            List<WardTypeDTO> objList = new List<WardTypeDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCWardTypes.ToList();

                    objList = _mapper.Map<List<WardTypeDTO>>(objReturn1);

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

        public List<WardTypeDTO> GetListWithParam(string param)
        {
            log.MethodStart();

            List<WardTypeDTO> objList = new List<WardTypeDTO>();

            var searchModel = JsonSerializer.Deserialize<WardTypeDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TCWardTypes.FromSqlRaw<TCWardType>("sp_GET_TCWard {0}", searchModel.wrd_name).ToList();

                    objList = _mapper.Map<List<WardTypeDTO>>(objDataList);

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

        public List<WardTypeDTO> GetListWithModel(WardTypeDTO searchModel)
        {
            log.MethodStart();

            List<WardTypeDTO> objList = new List<WardTypeDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TCWardTypes.FromSqlRaw<TCWardType>("sp_GET_TCWardType {0}", searchModel.wrd_mst_code).ToList();

                    objList = _mapper.Map<List<WardTypeDTO>>(objDataList);

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

        public List<WardTypeDTO> GetList_Active_WithModel(WardTypeDTO searchModel)
        {
            log.MethodStart();

            List<WardTypeDTO> objList = new List<WardTypeDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TCWardTypes.FromSqlRaw<TCWardType>("sp_GET_TCWardType_Active {0}", searchModel.wrd_mst_code).ToList();

                    objList = _mapper.Map<List<WardTypeDTO>>(objDataList);

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

        public WardTypeDTO GetData(string wrd_code)
        {
            log.MethodStart();

            WardTypeDTO objModel = new WardTypeDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCWardTypes.FirstOrDefault(x => x.wrd_code == wrd_code);

                    objModel = _mapper.Map<WardTypeDTO>(objReturn1);

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

        public WardTypeDTO SaveData(WardTypeDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            var currentUser = "";
            WardTypeDTO objReturn = new WardTypeDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TCWardType();

                    if (model.wrd_status == "E")
                    {
                        objModel = _db.TCWardTypes.FirstOrDefault(x => x.wrd_id == model.wrd_id);
                    }

                    if (model.wrd_status == "N")
                    {
                        objModel = _mapper.Map<TCWardType>(model);

                        objModel.wrd_status = objModel.wrd_active == true ? "A" : "I";
                        objModel.wrd_createdate = currentDateTime;

                        currentUser = objModel.wrd_createuser;

                        _db.TCWardTypes.Add(objModel);
                    }
                    else if (model.wrd_status == "E")
                    {
                        objModel.wrd_name = model.wrd_name;
                        objModel.wrd_desc = model.wrd_desc;
                        objModel.wrd_active = model.wrd_active;
                        objModel.wrd_status = objModel.wrd_active == true ? "A" : "I";
                        objModel.wrd_updateuser = model.wrd_updateuser;
                        objModel.wrd_updatedate = currentDateTime;

                        currentUser = objModel.wrd_updateuser;

                        //_db.TCHospitals.Update(objModel);
                    }

                    #region Save Log Process ...
                    _db.LogProcesss.Add(new LogProcess()
                    {
                        log_usr_id = currentUser,
                        log_mnu_id = "",
                        log_mnu_name = "Ward",
                        log_tran_id = $"{objModel.wrd_mst_code}:{objModel.wrd_code}",
                        log_action = (model.wrd_status == "N" ? "New" : "Update"),
                        log_desc = (model.wrd_status == "N" ? "New" : "Update") + " Ward " + objModel.wrd_name,
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

        public string CheckDuplicate(WardTypeDTO model)
        {
            log.MethodStart();

            string objReturn = "";

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objResult = _db.TCWardTypes.Any(x => x.wrd_code == model.wrd_code);

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
