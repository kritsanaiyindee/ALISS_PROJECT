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
    public class OrganismService : IOrganismService
    {
        private static readonly ILogService log = new LogService(typeof(OrganismService));

        private readonly MasterManagementContext _db;
        private readonly IMapper _mapper;

        public OrganismService(MasterManagementContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<OrganismDTO> GetList()
        {
            log.MethodStart();

            List<OrganismDTO> objList = new List<OrganismDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCOrganisms.ToList();

                    objList = _mapper.Map<List<OrganismDTO>>(objReturn1);

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

        public List<OrganismDTO> GetListWithParam(string param)
        {
            log.MethodStart();

            List<OrganismDTO> objList = new List<OrganismDTO>();

            var searchModel = JsonSerializer.Deserialize<OrganismDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TCOrganisms.FromSqlRaw<TCOrganism>("sp_GET_TCOrganism {0}", searchModel.org_code).ToList();

                    objList = _mapper.Map<List<OrganismDTO>>(objDataList);

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

        public List<OrganismDTO> GetListWithModel(OrganismDTO searchModel)
        {
            log.MethodStart();

            List<OrganismDTO> objList = new List<OrganismDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.OrganismDTOs.FromSqlRaw<OrganismDTO>("sp_GET_TCOrganism {0}", searchModel.org_mst_code).ToList();

                    //objList = _mapper.Map<List<OrganismDTO>>(objDataList);

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

        public List<OrganismDTO> GetList_Active_WithModel(OrganismDTO searchModel)
        {
            log.MethodStart();

            List<OrganismDTO> objList = new List<OrganismDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.OrganismDTOs.FromSqlRaw<OrganismDTO>("sp_GET_TCOrganism_Active {0}", searchModel.org_mst_code).ToList();

                    //objList = _mapper.Map<List<OrganismDTO>>(objDataList);

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

        public OrganismDTO GetData(string org_code)
        {
            log.MethodStart();

            OrganismDTO objModel = new OrganismDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCOrganisms.FirstOrDefault(x => x.org_code == org_code);

                    objModel = _mapper.Map<OrganismDTO>(objReturn1);

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

        public OrganismDTO SaveData(OrganismDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            var currentUser = "";
            OrganismDTO objReturn = new OrganismDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TCOrganism();

                    if (model.org_status == "E")
                    {
                        objModel = _db.TCOrganisms.FirstOrDefault(x => x.org_id == model.org_id);
                    }

                    if (model.org_status == "N")
                    {
                        objModel = _mapper.Map<TCOrganism>(model);

                        objModel.ORG = model.org_mst_ORG;
                        objModel.ORG = model.org_mst_ORG;
                        objModel.GRAM = model.org_mst_GRAM;
                        objModel.ORGANISM = model.org_mst_ORGANISM;
                        objModel.STATUS = model.org_mst_STATUS;
                        objModel.org_status = objModel.org_active == true ? "A" : "I";
                        objModel.org_createdate = currentDateTime;

                        currentUser = objModel.org_createuser;

                        _db.TCOrganisms.Add(objModel);
                    }
                    else if (model.org_status == "E")
                    {
                        objModel.org_code = model.org_code;
                        objModel.ORG = model.org_mst_ORG;
                        objModel.GRAM = model.org_mst_GRAM;
                        objModel.ORGANISM = model.org_mst_ORGANISM;
                        objModel.STATUS = model.org_mst_STATUS;
                        objModel.org_active = model.org_active;
                        objModel.org_status = objModel.org_active == true ? "A" : "I";
                        objModel.org_updateuser = model.org_updateuser;
                        objModel.org_updatedate = currentDateTime;

                        currentUser = objModel.org_createuser;

                        //_db.TCHospitals.Update(objModel);
                    }

                    #region Save Log Process ...
                    _db.LogProcesss.Add(new LogProcess()
                    {
                        log_usr_id = currentUser,
                        log_mnu_id = "",
                        log_mnu_name = "Organism",
                        log_tran_id = $"{objModel.org_mst_code}:{objModel.org_code}",
                        log_action = (model.org_status == "N" ? "New" : "Update"),
                        log_desc = (model.org_status == "N" ? "New" : "Update") + " Organism " + objModel.org_code,
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

        public string CheckDuplicate(OrganismDTO model)
        {
            log.MethodStart();

            string objReturn = "";

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objResult = _db.TCOrganisms.Any(x => x.org_code == model.org_code);

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
