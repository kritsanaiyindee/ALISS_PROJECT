using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Log4NetLibrary;
using ALISS.Mapping.Library.DataAccess;
using ALISS.Mapping.DTO;
using ALISS.MasterManagement.Library.Models;
using ALISS.Mapping.Library.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ALISS.Mapping.Library
{
    public class MappingDataService : IMappingDataService
    {

        private static readonly ILogService log = new LogService(typeof(MappingDataService));

        private readonly MappingContext _db;
        private readonly IMapper _mapper;

        public MappingDataService(MappingContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }


        #region Mapping
        public List<MappingListsDTO> GetMappingList(string Param)
        {
            log.MethodStart();


            List<MappingListsDTO> objList = new List<MappingListsDTO>();

            var searchModel = JsonSerializer.Deserialize<MappingSearchDTO>(Param);

            using (var trans = _db.Database.BeginTransaction())
            {

                try
                {                    
                    objList = _db.MappingListDTOs.FromSqlRaw<MappingListsDTO>("sp_GET_TRMappingList {0} ,{1} ,{2}", searchModel.mps_Hos, searchModel.mps_Province, searchModel.mps_Area).ToList(); 
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
        
        public MappingDataDTO GetMappingDataById(string mp_id)
        {
            log.MethodStart();
            MappingDataDTO objModel = new MappingDataDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objModel = _db.MappingDataDTOs.FromSqlRaw<MappingDataDTO>("sp_GET_TRMappingByID {0}", mp_id).AsEnumerable().FirstOrDefault();
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

        public MappingDataDTO SaveMappingData(MappingDataDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            MappingDataDTO objReturn = new MappingDataDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TRMapping();

                    if (model.mp_status == 'N')
                    {
                        objModel = _mapper.Map<TRMapping>(model);

                        objModel.mp_createdate = currentDateTime;
                        objModel.mp_updatedate = currentDateTime;

                        _db.TRMappings.Add(objModel);
                    }
                    else if(model.mp_status == 'E')
                    {
                        objModel = _db.TRMappings.FirstOrDefault(x => x.mp_id == model.mp_id);
                        objModel.mp_status = model.mp_status;
                        objModel.mp_version = model.mp_version;
                        objModel.mp_startdate = model.mp_startdate;
                        objModel.mp_enddate = model.mp_enddate;
                        objModel.mp_program = model.mp_program;
                        objModel.mp_filetype = model.mp_filetype;
                        objModel.mp_dateformat = model.mp_dateformat;
                        objModel.mp_AntibioticIsolateOneRec = model.mp_AntibioticIsolateOneRec;
                        objModel.mp_firstlineisheader = model.mp_firstlineisheader;
                        objModel.mp_updateuser = model.mp_updateuser;
                        objModel.mp_updatedate = currentDateTime;
                    }
                    else if(model.mp_status == 'A')
                    {
                        objModel = _db.TRMappings.FirstOrDefault(x => x.mp_id == model.mp_id);
                        objModel.mp_status = model.mp_status;
                        objModel.mp_version = objModel.mp_version + 0.01M;
                        objModel.mp_approveduser = model.mp_approveduser;
                        objModel.mp_approveddate = currentDateTime;
                        objModel.mp_updateuser = model.mp_updateuser;
                        objModel.mp_updatedate = currentDateTime;

                        var oldVersion = _db.TRMappings.OrderByDescending(x => x.mp_version).FirstOrDefault(x => x.mp_lab == model.mp_lab
                                                           && x.mp_hos_code == model.mp_hos_code
                                                           && x.mp_status == 'A'
                                                           && x.mp_program == model.mp_program
                                                           && x.mp_filetype == model.mp_filetype
                                                           && x.mp_startdate < model.mp_startdate
                                                           && x.mp_enddate == null
                                                           && x.mp_flagdelete == false);
                        if (oldVersion != null)
                        { 
                            oldVersion.mp_enddate = model.mp_startdate.Value.AddDays(-1);                         
                        }
                    }

                  
                    #region Save Log Process ...
                    _db.LogProcesss.Add(new LogProcess()
                    {
                        log_usr_id = (objModel.mp_updateuser ?? objModel.mp_createuser),
                        log_mnu_id = "",
                        log_mnu_name = "Mapping",
                        log_tran_id = objModel.mp_id.ToString(),
                        log_action = (objModel.mp_status == 'N' ? "New" : "Update"),
                        log_desc = "Update Mapping ",
                        log_createuser = "SYSTEM",
                        log_createdate = currentDateTime
                    });
                    #endregion

                    _db.SaveChanges();
                    
                    trans.Commit();

                    objReturn = _mapper.Map<MappingDataDTO>(objModel);
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

        public TRMapping PrepareMappingUpdateVersion(TRMapping objTRMapping,String UpdateUser,DateTime UpdateDate)
        {
            //objTRMapping.mp_version = objTRMapping.mp_version + 0.01M;
            objTRMapping.mp_status = 'E';
            objTRMapping.mp_updateuser = UpdateUser;
            objTRMapping.mp_updatedate = UpdateDate;

            return objTRMapping;
        }

        public MappingDataDTO CopyMappingData(MappingDataDTO objParam)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            MappingDataDTO objReturn = new MappingDataDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                #region Create Mapping...
                var Mapping =  _db.TRMappings.FirstOrDefault(x => x.mp_id == objParam.mp_id);
                Mapping.mp_id = Guid.NewGuid();
                Mapping.mp_mst_code = objParam.mp_mst_code;
                Mapping.mp_status = 'E';
                Mapping.mp_version = Math.Floor(objParam.mp_version);
                Mapping.mp_hos_code = objParam.mp_hos_code;
                Mapping.mp_lab = objParam.mp_lab;                
                Mapping.mp_startdate = objParam.mp_startdate;
                Mapping.mp_enddate = null;
                Mapping.mp_createuser = objParam.mp_createuser;
                Mapping.mp_createdate = currentDateTime;
                Mapping.mp_updateuser = objParam.mp_createuser;
                Mapping.mp_updatedate = currentDateTime;
                _db.TRMappings.Add(Mapping);
                #endregion

                #region Create WHONetMapping...

                var WHONetMapping = _db.TRWHONetMappings.Where(w => w.wnm_mappingid == objParam.mp_id
                                                               && w.wnm_flagdelete == false);


                if (WHONetMapping != null)
                {
                    foreach (var w in WHONetMapping)
                    {
                        w.wnm_id = Guid.NewGuid();
                        w.wnm_mappingid = Mapping.mp_id;
                        w.wnm_status = 'N';
                        w.wnm_createuser = objParam.mp_createuser;
                        w.wnm_createdate = currentDateTime;
                        w.wnm_updateuser = objParam.mp_createuser;
                        w.wnm_updatedate = null;
                    }

                    _db.TRWHONetMappings.AddRange(WHONetMapping);
                }
                #endregion

                #region Create SpecimenMapping...
                var SpecimenMapping = _db.TRSpecimenMappings.Where(s => s.spm_mappingid == objParam.mp_id
                                                               && s.spm_flagdelete == false);
                
                if (SpecimenMapping != null)
                {
                    foreach (var s in SpecimenMapping)
                    {
                        s.spm_id = Guid.NewGuid();
                        s.spm_mappingid = Mapping.mp_id;
                        s.spm_status = 'N';
                        s.spm_createuser = objParam.mp_createuser;
                        s.spm_createdate = currentDateTime;
                        s.spm_updateuser = objParam.mp_createuser;
                        s.spm_updatedate = currentDateTime;
                    }

                    _db.TRSpecimenMappings.AddRange(SpecimenMapping);
                }
                #endregion

                #region Create OrganismMapping...
                var OrganismMapping = _db.TROrganismMappings.Where(o => o.ogm_mappingid == objParam.mp_id
                                                               && o.ogm_flagdelete == false);

                if (OrganismMapping != null)
                {
                    foreach (var o in OrganismMapping)
                    {
                        o.ogm_id = Guid.NewGuid();
                        o.ogm_mappingid = Mapping.mp_id;
                        o.ogm_status = 'N';
                        o.ogm_createuser = objParam.mp_createuser;
                        o.ogm_createdate = currentDateTime;
                        o.ogm_updateuser = objParam.mp_createuser;
                        o.ogm_updatedate = currentDateTime;
                    }

                    _db.TROrganismMappings.AddRange(OrganismMapping);
                }
                #endregion

                #region Create WardTypeMapping...
                var WardTypeMapping = _db.TRWardTypeMappings.Where(wd => wd.wdm_mappingid == objParam.mp_id
                                                               && wd.wdm_flagdelete == false);

                if (WardTypeMapping != null)
                {
                    foreach (var wd in WardTypeMapping)
                    {
                        wd.wdm_id = Guid.NewGuid();
                        wd.wdm_mappingid = Mapping.mp_id;
                        wd.wdm_status = 'N';
                        wd.wdm_createuser = objParam.mp_createuser;
                        wd.wdm_createdate = currentDateTime;
                        wd.wdm_updateuser = objParam.mp_createuser;
                        wd.wdm_updatedate = currentDateTime;
                    }

                    _db.TRWardTypeMappings.AddRange(WardTypeMapping);
                }
                #endregion

                #region Save Log Process ...
                _db.LogProcesss.Add(new LogProcess()
                {
                    log_usr_id = (Mapping.mp_updateuser ?? Mapping.mp_createuser),
                    log_mnu_id = "",
                    log_mnu_name = "Mapping",
                    log_tran_id = Mapping.mp_id.ToString(),                   
                    log_action = (Mapping.mp_status == 'N' ? "New" : "Update"),
                    log_desc = "Copy Mapping " ,
                    log_createuser = "SYSTEM",
                    log_createdate = currentDateTime
                });
                #endregion

                _db.SaveChanges();

                trans.Commit();

                objReturn = _mapper.Map<MappingDataDTO>(Mapping);
            }
            log.MethodFinish();
                return objReturn;
        }

        public MappingDataDTO GetMappingDataWithModel(MappingDataDTO model)
        {
            log.MethodStart();
            MappingDataDTO objModel = new MappingDataDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TRMappings.OrderByDescending(x => x.mp_version).FirstOrDefault(x => x.mp_lab == model.mp_lab
                                                             && x.mp_program == model.mp_program
                                                             && x.mp_filetype == model.mp_filetype
                                                             && x.mp_enddate == null
                                                             && x.mp_flagdelete == false);


                    if (objReturn1 != null)
                        objModel = _mapper.Map<MappingDataDTO>(objReturn1);

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

        public MappingDataDTO chkDuplicateMappingApproved(MappingDataDTO model)
        {
            log.MethodStart();
            MappingDataDTO objModel = new MappingDataDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TRMappings.OrderByDescending(x => x.mp_version).FirstOrDefault(x => x.mp_lab == model.mp_lab
                                                             && x.mp_hos_code == model.mp_hos_code
                                                             && x.mp_program == model.mp_program
                                                             && x.mp_filetype == model.mp_filetype
                                                             && x.mp_startdate > model.mp_startdate
                                                             && x.mp_status == 'A'
                                                             && x.mp_flagdelete == false);


                    if (objReturn1 != null)
                        objModel = _mapper.Map<MappingDataDTO>(objReturn1);

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
        
        public MappingDataDTO GetMappingDataActiveWithModel(MappingDataDTO model)
        {
            log.MethodStart();
            MappingDataDTO objModel = new MappingDataDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TRMappings.FromSqlRaw<TRMapping>("sp_GET_TRMapping_Approve_Active {0} ,{1} ,{2} ,{3}", model.mp_hos_code, model.mp_lab, model.mp_program, model.mp_filetype).ToList();

                    var objListMapping = _mapper.Map<List<MappingDataDTO>>(objDataList);

                    if (objListMapping.Count > 0)
                    {
                        objModel = objListMapping.FirstOrDefault();
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
        
        #endregion

        #region WHONetMapping

        public WHONetMappingDataDTO SaveWHONetMappingData(WHONetMappingDataDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            WHONetMappingDataDTO objReturn = new WHONetMappingDataDTO();
            bool chkUpdate = false;

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TRWHONetMapping();
                    objModel = _db.TRWHONetMappings.FirstOrDefault(x => x.wnm_id == model.wnm_id);
                    if(objModel == null)
                    {
                        objModel = _mapper.Map<TRWHONetMapping>(model);

                        objModel.wnm_createdate = currentDateTime;
                        objModel.wnm_updatedate = currentDateTime;

                        _db.TRWHONetMappings.Add(objModel);
                        chkUpdate = true;
                    }
                    else
                    {
                        if (
                            objModel.wnm_flagdelete != model.wnm_flagdelete ||
                            objModel.wnm_whonetfield != model.wnm_whonetfield ||
                            objModel.wnm_originalfield != model.wnm_originalfield ||
                            objModel.wnm_type != model.wnm_type ||
                            objModel.wnm_fieldlength != model.wnm_fieldlength ||
                            objModel.wnm_fieldformat != model.wnm_fieldformat ||
                            objModel.wnm_encrypt != model.wnm_encrypt ||
                            objModel.wnm_mandatory != model.wnm_mandatory ||
                            objModel.wnm_antibioticcolumn != model.wnm_antibioticcolumn ||
                            objModel.wnm_antibiotic != model.wnm_antibiotic)
                        {
                            objModel.wnm_status = model.wnm_status;
                            objModel.wnm_flagdelete = model.wnm_flagdelete;
                            objModel.wnm_whonetfield = model.wnm_whonetfield;
                            objModel.wnm_originalfield = model.wnm_originalfield;
                            objModel.wnm_type = model.wnm_type;
                            objModel.wnm_fieldlength = model.wnm_fieldlength;
                            objModel.wnm_fieldformat = model.wnm_fieldformat;
                            objModel.wnm_encrypt = model.wnm_encrypt;
                            objModel.wnm_mandatory = model.wnm_mandatory;
                            objModel.wnm_antibioticcolumn = model.wnm_antibioticcolumn;
                            objModel.wnm_antibiotic = model.wnm_antibiotic;
                            objModel.wnm_updateuser = model.wnm_updateuser;
                            objModel.wnm_updatedate = currentDateTime;
                            chkUpdate = true;
                        }
                    }
                    if (chkUpdate == true)
                    {
                        #region Update Mapping Version..

                        var objMapping = _db.TRMappings.FirstOrDefault(x => x.mp_id == model.wnm_mappingid);
                        if (objMapping != null)
                        {
                            objMapping = PrepareMappingUpdateVersion(objMapping, model.wnm_updateuser, currentDateTime);
                        }

                        #endregion

                        #region Save Log Process ...
                        _db.LogProcesss.Add(new LogProcess()
                        {
                            log_usr_id = (objModel.wnm_updateuser ?? objModel.wnm_createuser),
                            log_mnu_id = "",
                            log_mnu_name = "WHONetMapping",
                            log_tran_id = objModel.wnm_id.ToString(),
                            log_action = (objModel.wnm_status == 'N' ? "New" : "Update"),
                            log_desc = "Update WHONetMapping ",
                            log_createuser = "SYSTEM",
                            log_createdate = currentDateTime
                        });
                        #endregion
                    }
                    _db.SaveChanges();

                    trans.Commit();

                    objReturn = _mapper.Map<WHONetMappingDataDTO>(objModel);
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
        
        public List<WHONetMappingListsDTO> GetWHONetMappingListWithModel(WHONetMappingSearch searchModel)
        {
            log.MethodStart();
            List<WHONetMappingListsDTO> objList = new List<WHONetMappingListsDTO>();
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.WHONetMappingListsDTOs.FromSqlRaw<WHONetMappingListsDTO>("sp_GET_TRWHONetMappingList {0}, {1}", searchModel.wnm_mappingid,searchModel.wnm_mst_code).ToList();
                    
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


        public WHONetMappingDataDTO GetWHONetMappingData(string wnm_id)
        {
            log.MethodStart();
            WHONetMappingDataDTO objModel = new WHONetMappingDataDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TRWHONetMappings.FirstOrDefault(x => x.wnm_id.ToString() == wnm_id);

                    objModel = _mapper.Map<WHONetMappingDataDTO>(objReturn1);

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


        public WHONetMappingDataDTO GetWHONetMappingDataWithModel(WHONetMappingDataDTO model)
        {
            log.MethodStart();
            WHONetMappingDataDTO objModel = new WHONetMappingDataDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TRWHONetMappings.FirstOrDefault(x => x.wnm_mappingid == model.wnm_mappingid
                                                             && (x.wnm_whonetfield == model.wnm_whonetfield
                                                             || x.wnm_originalfield == model.wnm_originalfield)                                                          
                                                             && x.wnm_flagdelete == false);

                    if (objReturn1 != null)
                        objModel = _mapper.Map<WHONetMappingDataDTO>(objReturn1);

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

        #endregion

        #region SpecimenMapping
        public SpecimenMappingDataDTO SaveSpecimenMappingData(SpecimenMappingDataDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            SpecimenMappingDataDTO objReturn = new SpecimenMappingDataDTO();
            bool chkUpdate = false;

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TRSpecimenMapping();
                    objModel = _db.TRSpecimenMappings.FirstOrDefault(x => x.spm_id == model.spm_id);
                    if (objModel == null)
                    {
                        objModel = _mapper.Map<TRSpecimenMapping>(model);

                        objModel.spm_createdate = currentDateTime;
                        objModel.spm_updatedate = currentDateTime;
                        chkUpdate = true;
                        _db.TRSpecimenMappings.Add(objModel);
                    }
                    else
                    {
                        if (
                        objModel.spm_flagdelete != model.spm_flagdelete ||
                        objModel.spm_whonetcode != model.spm_whonetcode ||
                        objModel.spm_localspecimencode != model.spm_localspecimencode ||
                        objModel.spm_localspecimendesc != model.spm_localspecimendesc)
                        {
                            objModel.spm_status = model.spm_status;
                            objModel.spm_flagdelete = model.spm_flagdelete;
                            objModel.spm_whonetcode = model.spm_whonetcode;
                            objModel.spm_localspecimencode = model.spm_localspecimencode;
                            objModel.spm_localspecimendesc = model.spm_localspecimendesc;
                            objModel.spm_updateuser = model.spm_updateuser;
                            objModel.spm_updatedate = currentDateTime;
                            chkUpdate = true;
                        }
                    }

                    if (chkUpdate == true)
                    {
                        #region Update Mapping Version..

                        var objMapping = _db.TRMappings.FirstOrDefault(x => x.mp_id == model.spm_mappingid);
                        if (objMapping != null)
                        {
                            objMapping = PrepareMappingUpdateVersion(objMapping, model.spm_updateuser, currentDateTime);
                        }
                        #endregion


                        #region Save Log Process ...
                        _db.LogProcesss.Add(new LogProcess()
                        {
                            log_usr_id = (objModel.spm_updateuser ?? objModel.spm_createuser),
                            log_mnu_id = "",
                            log_mnu_name = "SpecimenMapping",
                            log_tran_id = objModel.spm_id.ToString(),                            
                            log_action = (objModel.spm_status == 'N' ? "New" : "Update"),
                            log_desc = "Update SpecimenMapping ",
                            log_createuser = "SYSTEM",
                            log_createdate = currentDateTime
                        });
                        #endregion
                    }

                    _db.SaveChanges();

                    trans.Commit();

                    objReturn = _mapper.Map<SpecimenMappingDataDTO>(objModel);
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

        public List<SpecimenMappingListsDTO> GetSpecimenMappingListWithModel(SpecimenMappingSearch searchModel)
        {
            log.MethodStart();
            List<SpecimenMappingListsDTO> objList = new List<SpecimenMappingListsDTO>();
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.SpecimenMappingListsDTOs.FromSqlRaw<SpecimenMappingListsDTO>("sp_GET_TRSpecimenMappingList {0}, {1}", searchModel.spm_mappingid,searchModel.spm_mst_code).ToList();

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

        public SpecimenMappingDataDTO GetSpecimenMappingData(string spm_id)
        {
            log.MethodStart();
            SpecimenMappingDataDTO objModel = new SpecimenMappingDataDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TRSpecimenMappings.FirstOrDefault(x => x.spm_id.ToString() == spm_id);

                    objModel = _mapper.Map<SpecimenMappingDataDTO>(objReturn1);

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

        public SpecimenMappingDataDTO GetSpecimenMappingDataWithModel(SpecimenMappingDataDTO model)
        {
            log.MethodStart();
            SpecimenMappingDataDTO objModel = new SpecimenMappingDataDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TRSpecimenMappings.FirstOrDefault(x => x.spm_mappingid == model.spm_mappingid
                                                             //&& x.spm_whonetcode == model.spm_whonetcode
                                                             && x.spm_localspecimencode.ToUpper() == model.spm_localspecimencode.ToUpper()
                                                             //&& x.spm_localspecimendesc == model.spm_localspecimendesc
                                                             && x.spm_flagdelete == false);

                    if(objReturn1 != null)
                    objModel = _mapper.Map<SpecimenMappingDataDTO>(objReturn1);

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
        #endregion

        #region OrganismMapping

        public OrganismMappingDataDTO SaveOrganismMappingData(OrganismMappingDataDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            OrganismMappingDataDTO objReturn = new OrganismMappingDataDTO();
            bool chkUpdate = false;

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TROrganismMapping();
                    objModel = _db.TROrganismMappings.FirstOrDefault(x => x.ogm_id == model.ogm_id);
                    if (objModel == null)
                    {
                        objModel = _mapper.Map<TROrganismMapping>(model);

                        objModel.ogm_createdate = currentDateTime;
                        objModel.ogm_updatedate = currentDateTime;
                        chkUpdate = true;
                        _db.TROrganismMappings.Add(objModel);
                    }
                    else
                    {
                        if (
                        objModel.ogm_flagdelete != model.ogm_flagdelete ||
                        objModel.ogm_whonetcode != model.ogm_whonetcode ||
                        objModel.ogm_whonetdesc != model.ogm_whonetdesc ||
                        objModel.ogm_localorganismcode != model.ogm_localorganismcode ||
                        objModel.ogm_localorganismdesc != model.ogm_localorganismdesc)
                        {
                            objModel.ogm_status = model.ogm_status;
                            objModel.ogm_flagdelete = model.ogm_flagdelete;
                            objModel.ogm_whonetcode = model.ogm_whonetcode;
                            objModel.ogm_whonetdesc = model.ogm_whonetdesc;
                            objModel.ogm_localorganismcode = model.ogm_localorganismcode;
                            objModel.ogm_localorganismdesc = model.ogm_localorganismdesc;
                            objModel.ogm_updateuser = model.ogm_updateuser;
                            objModel.ogm_updatedate = currentDateTime;
                            chkUpdate = true;
                        }
                    }

                    if (chkUpdate == true)
                    {
                        #region Update Mapping Version..

                        var objMapping = _db.TRMappings.FirstOrDefault(x => x.mp_id == model.ogm_mappingid);
                        if (objMapping != null)
                        {
                            objMapping = PrepareMappingUpdateVersion(objMapping, model.ogm_updateuser, currentDateTime);
                        }
                        #endregion

                        #region Save Log Process ...
                        _db.LogProcesss.Add(new LogProcess()
                        {
                            log_usr_id = (objModel.ogm_updateuser ?? objModel.ogm_createuser),
                            log_mnu_id = "",
                            log_mnu_name = "OrganismMapping",
                            log_tran_id = objModel.ogm_id.ToString(),                            
                            log_action = (objModel.ogm_status == 'N' ? "New" : "Update"),
                            log_desc = "Update OrganismMapping ",
                            log_createuser = "SYSTEM",
                            log_createdate = currentDateTime
                        });
                        #endregion
                    }
                    _db.SaveChanges();

                    trans.Commit();

                    objReturn = _mapper.Map<OrganismMappingDataDTO>(objModel);
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


        public List<OrganismMappingListsDTO> GetOrganismMappingListWithModel(OrganismMappingSearch searchModel)
        {
            log.MethodStart();
            List<OrganismMappingListsDTO> objList = new List<OrganismMappingListsDTO>();
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.OrganismMappingListsDTOs.FromSqlRaw<OrganismMappingListsDTO>("sp_GET_TROrganismMappingList {0}, {1}", searchModel.ogm_mappingid,searchModel.ogm_mst_code).ToList();

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

        public OrganismMappingDataDTO GetOrganismMappingData(string ogm_id)
        {
            log.MethodStart();
            OrganismMappingDataDTO objModel = new OrganismMappingDataDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TROrganismMappings.FirstOrDefault(x => x.ogm_id.ToString() == ogm_id);

                    objModel = _mapper.Map<OrganismMappingDataDTO>(objReturn1);

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

        public OrganismMappingDataDTO GetOrganismMappingDataWithModel(OrganismMappingDataDTO model)
        {
            log.MethodStart();
            OrganismMappingDataDTO objModel = new OrganismMappingDataDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TROrganismMappings.FirstOrDefault(x => x.ogm_mappingid == model.ogm_mappingid
                                                             //&& x.ogm_whonetcode == model.ogm_whonetcode
                                                             && x.ogm_localorganismcode.ToUpper() == model.ogm_localorganismcode.ToUpper()
                                                             //&& x.ogm_localorganismdesc == model.ogm_localorganismdesc
                                                             && x.ogm_flagdelete == false);

                    if (objReturn1 != null)
                        objModel = _mapper.Map<OrganismMappingDataDTO>(objReturn1);

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

        #endregion

        #region WardTypeMapping

        public WardTypeMappingDataDTO SaveWardTypeMappingData(WardTypeMappingDataDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            WardTypeMappingDataDTO objReturn = new WardTypeMappingDataDTO();
            bool chkUpdate = false;

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TRWardTypeMapping();
                    objModel = _db.TRWardTypeMappings.FirstOrDefault(x => x.wdm_id == model.wdm_id);
                    if (objModel == null)
                    {
                        objModel = _mapper.Map<TRWardTypeMapping>(model);

                        objModel.wdm_createdate = currentDateTime;
                        objModel.wdm_updatedate = currentDateTime;
                        chkUpdate = true;
                        _db.TRWardTypeMappings.Add(objModel);
                    }
                    else
                    {
                        if(
                        objModel.wdm_flagdelete != model.wdm_flagdelete ||
                        objModel.wdm_wardtype != model.wdm_wardtype ||
                        objModel.wdm_warddesc != model.wdm_warddesc ||
                        objModel.wdm_localwardname != model.wdm_localwardname)
                        {
                            objModel.wdm_status = model.wdm_status;
                            objModel.wdm_flagdelete = model.wdm_flagdelete;
                            objModel.wdm_wardtype = model.wdm_wardtype;
                            objModel.wdm_warddesc = model.wdm_warddesc;
                            objModel.wdm_localwardname = model.wdm_localwardname;
                            objModel.wdm_updateuser = model.wdm_updateuser;
                            objModel.wdm_updatedate = currentDateTime;
                            chkUpdate = true;
                        }                        
                    }

                    if (chkUpdate == true)
                    {
                        #region Update Mapping Version..

                        var objMapping = _db.TRMappings.FirstOrDefault(x => x.mp_id == model.wdm_mappingid);
                        if (objMapping != null)
                        {
                            objMapping = PrepareMappingUpdateVersion(objMapping, model.wdm_updateuser, currentDateTime);
                        }

                        #endregion

                        #region Save Log Process ...
                        _db.LogProcesss.Add(new LogProcess()
                        {
                            log_usr_id = (objModel.wdm_updateuser ?? objModel.wdm_createuser),
                            log_mnu_id = "",
                            log_mnu_name = "WardTypeMapping",
                            log_tran_id = objModel.wdm_id.ToString(),                           
                            log_action = (objModel.wdm_status == 'N' ? "New" : "Update"),
                            log_desc = "Update WardTypeMapping ",
                            log_createuser = "SYSTEM",
                            log_createdate = currentDateTime
                        });
                        #endregion
                    }

                    _db.SaveChanges();

                    trans.Commit();

                    objReturn = _mapper.Map<WardTypeMappingDataDTO>(objModel);
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


        public List<WardTypeMappingListsDTO> GetWardTypeMappingListWithModel(WardTypeMappingSearch searchModel)
        {
            log.MethodStart();
            List<WardTypeMappingListsDTO> objList = new List<WardTypeMappingListsDTO>();
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.WardTypeMappingListsDTOs.FromSqlRaw<WardTypeMappingListsDTO>("sp_GET_TRWardTypeMappingList {0}, {1}", searchModel.wdm_mappingid,searchModel.wdm_mst_code).ToList();

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

        public WardTypeMappingDataDTO GetWardTypeMappingData(string wdm_id)
        {
            log.MethodStart();
            WardTypeMappingDataDTO objModel = new WardTypeMappingDataDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TRWardTypeMappings.FirstOrDefault(x => x.wdm_id.ToString() == wdm_id);

                    objModel = _mapper.Map<WardTypeMappingDataDTO>(objReturn1);

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

        public WardTypeMappingDataDTO GetWardTypeMappingDataWithModel(WardTypeMappingDataDTO model)
        {
            log.MethodStart();
            WardTypeMappingDataDTO objModel = new WardTypeMappingDataDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TRWardTypeMappings.FirstOrDefault(x => x.wdm_mappingid == model.wdm_mappingid
                                                             //&& x.wdm_wardtype == model.wdm_wardtype
                                                             && x.wdm_localwardname == model.wdm_localwardname
                                                             && x.wdm_flagdelete == false);

                    if (objReturn1 != null)
                        objModel = _mapper.Map<WardTypeMappingDataDTO>(objReturn1);

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

        #endregion
    }
}


