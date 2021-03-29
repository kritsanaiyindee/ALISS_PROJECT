using ALISS.AUTH.DTO;
using ALISS.AUTH.Library.DataAccess;
using ALISS.AUTH.Library.Models;
using AutoMapper;
using Log4NetLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ALISS.AUTH.Library
{
    public class ColumnConfigService : IColumnConfigService
    {
        private static readonly ILogService log = new LogService(typeof(ColumnConfigService));

        private readonly AuthContext _db;
        private readonly IMapper _mapper;

        public ColumnConfigService(AuthContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<ColumnConfigDTO> GetList()
        {
            log.MethodStart();

            List<ColumnConfigDTO> objList = new List<ColumnConfigDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TBConfigs.ToList();

                    objList = _mapper.Map<List<ColumnConfigDTO>>(objReturn1);

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

        public List<ColumnConfigDTO> GetListWithParam(string param)
        {
            log.MethodStart();

            List<ColumnConfigDTO> objList = new List<ColumnConfigDTO>();

            var searchModel = JsonSerializer.Deserialize<ColumnConfigDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TBConfigs.FromSqlRaw<TBConfig>("sp_GET_TBConfig {0}", searchModel.tbc_id).ToList();

                    objList = _mapper.Map<List<ColumnConfigDTO>>(objDataList);

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

        public List<ColumnConfigDTO> GetListWithModel(ColumnConfigSearchDTO searchModel)
        {
            log.MethodStart();

            List<ColumnConfigDTO> objList = new List<ColumnConfigDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.ColumnConfigDTOs.FromSqlRaw<ColumnConfigDTO>("sp_GET_ColumnConfigDTO {0}", searchModel.sch_mnu_code).ToList();

                    //objList = _mapper.Map<List<ColumnConfigDTO>>(objDataList);

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    // TODO: Handle failure
                    log.Error(ex);

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

        public ColumnConfigDTO GetData(string tbc_code)
        {
            log.MethodStart();

            ColumnConfigDTO objModel = new ColumnConfigDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TBConfigs.FirstOrDefault(x => x.tbc_mnu_code == tbc_code);

                    if (objReturn1 != null)
                    {
                        objModel = _mapper.Map<ColumnConfigDTO>(objReturn1);
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

        public ColumnConfigDTO SaveData(ColumnConfigDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            var currentUser = "";
            ColumnConfigDTO objReturn = new ColumnConfigDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TBConfig();

                    if (model.tbc_status == "E")
                    {
                        objModel = _db.TBConfigs.FirstOrDefault(x => x.tbc_id == model.tbc_id);
                    }

                    if (model.tbc_status == "N")
                    {
                        objModel = _mapper.Map<TBConfig>(model);

                        objModel.tbc_status = objModel.tbc_active == true ? "A" : "I";
                        objModel.tbc_createdate = currentDateTime;

                        currentUser = objModel.tbc_createuser;

                        _db.TBConfigs.Add(objModel);
                    }
                    else if (model.tbc_status == "E")
                    {
                        objModel.tbc_column_name = model.tbc_column_name;
                        objModel.tbc_column_label = model.tbc_column_label;
                        objModel.tbc_column_placeholder = model.tbc_column_placeholder;
                        objModel.tbc_required_field = model.tbc_required_field;
                        objModel.tbc_edit = model.tbc_edit;
                        objModel.tbc_active = model.tbc_active;
                        objModel.tbc_status = objModel.tbc_active == true ? "A" : "I";
                        objModel.tbc_updateuser = model.tbc_updateuser;
                        objModel.tbc_updatedate = currentDateTime;

                        currentUser = objModel.tbc_updateuser;

                        //_db.TCMenus.Update(objModel);
                    }


                    #region Save Log Process ...
                    _db.LogProcesss.Add(new LogProcess()
                    {
                        log_usr_id = currentUser,
                        log_mnu_id = "",
                        log_mnu_name = "ColumnConfig",
                        log_tran_id = objModel.tbc_mnu_code,
                        log_action = (model.tbc_status == "N" ? "New" : "Update"),
                        log_desc = (model.tbc_status == "N" ? "New" : "Update") + " ColumnConfig " + objModel.tbc_mnu_code + ":" + objModel.tbc_column_name,
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
                    log.Error(ex);
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
