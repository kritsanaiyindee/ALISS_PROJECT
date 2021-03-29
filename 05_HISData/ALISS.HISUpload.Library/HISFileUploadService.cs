using ALISS.HISUpload.DTO;
using ALISS.HISUpload.Library.DataAccess;
using ALISS.HISUpload.Library.Models;
using AutoMapper;
using Log4NetLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALISS.HISUpload.Library
{
    public class HISFileUploadService : IHISFileUploadService
    {
        private static readonly ILogService log = new LogService(typeof(HISFileUploadService));
        private readonly HISFileUploadContext _db;
        private readonly IMapper _mapper;
        public HISFileUploadService(HISFileUploadContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public HISUploadDataDTO GetHISFileUploadDataById(int hfu_id)
        {
            log.MethodStart();
            HISUploadDataDTO objModel = new HISUploadDataDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    //var objReturn1 = _db.TRHISFileUploads.FirstOrDefault(x => x.hfu_id == hfu_id);

                    var objDataList = _db.HISFileUploadDataDTOs.FromSqlRaw<HISUploadDataDTO>("sp_GET_TRHISFileUploadList  {0} ,{1} ,{2} ,{3}, {4}, {5}, {6} , {7}"
                                                                                                    , null , null, null , null , 0 , null , null
                                                                                                    , hfu_id
                                                                                                    ).ToList();

                    var objData = objDataList.FirstOrDefault();
                    objModel = _mapper.Map<HISUploadDataDTO>(objData);

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

        public List<HISUploadDataDTO> GetHISFileUploadListWithModel(HISUploadDataSearchDTO searchModel)
        {
            log.MethodStart();

            List<HISUploadDataDTO> objList = new List<HISUploadDataDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.HISFileUploadDataDTOs.FromSqlRaw<HISUploadDataDTO>("sp_GET_TRHISFileUploadList  {0} ,{1} ,{2} ,{3}, {4}, {5}, {6}"
                                                                                                    , searchModel.hfu_hos_code
                                                                                                    , searchModel.hfu_prv_code
                                                                                                    , searchModel.hfu_arh_code
                                                                                                    , null
                                                                                                    , 0
                                                                                                    , searchModel.hfu_upload_date_from_str
                                                                                                    , searchModel.hfu_upload_date_to_str
                                                                                                    ).ToList();

                    objList = _mapper.Map<List<HISUploadDataDTO>>(objDataList);

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
        public List<HISFileTemplateDTO> GetHISFileTemplate_Actice_WithModel(HISFileTemplateDTO searchModel)
        {
            log.MethodStart();

            List<HISFileTemplateDTO> objList = new List<HISFileTemplateDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.HISFileTemplateDTOs.FromSqlRaw<HISFileTemplateDTO>("sp_GET_TCHISFileTemplate_Active").ToList();

                    objList = _mapper.Map<List<HISFileTemplateDTO>>(objDataList);

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
        public HISUploadDataDTO SaveLabFileUploadData(HISUploadDataDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            HISUploadDataDTO objReturn = new HISUploadDataDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TRHISFileUpload();

                    if (model.hfu_status == 'N')
                    {
                        objModel = _mapper.Map<TRHISFileUpload>(model);

                        objModel.hfu_createdate = currentDateTime;
                        //objModel.hfu_updatedate = currentDateTime;

                        _db.TRHISFileUploads.Add(objModel);
                    }
                    else
                    {
                        objModel = _db.TRHISFileUploads.FirstOrDefault(x => x.hfu_id == model.hfu_id);
                        objModel.hfu_status = model.hfu_status;
                        //objModel.hfu_version = model.hfu_version;
                        //objModel.hfu_delete_flag = model.hfu_delete_flag;
                        //objModel.hfu_lab = model.hfu_lab;
                        //objModel.hfu_file_name = model.hfu_file_name;
                        //objModel.hfu_file_path = model.hfu_file_path;
                        //objModel.hfu_file_type = model.hfu_file_type;
                        //objModel.hfu_total_records = model.hfu_total_records;
                        //objModel.hfu_error_records = model.hfu_error_records;
                        objModel.hfu_approveduser = model.hfu_approveduser;
                        objModel.hfu_approveddate = model.hfu_approveddate;
                        objModel.hfu_updateuser = model.hfu_approveduser;
                        objModel.hfu_updatedate = currentDateTime;
                    }

                    _db.SaveChanges();

                    trans.Commit();

                    objReturn = _mapper.Map<HISUploadDataDTO>(objModel);
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

            return objReturn;
        }

        public HISFileUploadSummaryDTO SaveFileUploadSummary(List<HISFileUploadSummaryDTO> models)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            HISFileUploadSummaryDTO objReturn = new HISFileUploadSummaryDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    foreach(var objModel in models)
                    {
                        var obj = new TRHISFileUploadSummary();

                        obj.hus_hfu_id = objModel.hus_hfu_id;
                        obj.hus_error_fieldname = objModel.hus_error_fieldname;
                        obj.hus_error_fielddescr = objModel.hus_error_fielddescr;
                        obj.hus_error_fieldrecord = objModel.hus_error_fieldrecord;
                        obj.hus_createuser = objModel.hus_createuser;
                        obj.hus_createdate = currentDateTime;

                        _db.TRHISFileUploadSummarys.Add(obj);
                    }
                
                    _db.SaveChanges();

                    trans.Commit();

                    var objM = _db.TRHISFileUploadSummarys.FirstOrDefault();
                    objReturn = _mapper.Map<HISFileUploadSummaryDTO>(objM);
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

            return objReturn;
        }
        public List<HISFileUploadSummaryDTO> GetHISFileUploadSummaryByUploadId(int HISUploadFileid)
        {
            log.MethodStart();
            List<HISFileUploadSummaryDTO> objList = new List<HISFileUploadSummaryDTO> ();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.HISFileUploadSummaryDTOs.FromSqlRaw<HISFileUploadSummaryDTO>("sp_GET_TRHISFileUploadSummary {0}"
                                                                                                        , HISUploadFileid).ToList();

                    objList = _mapper.Map<List<HISFileUploadSummaryDTO>>(objDataList);

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

        public List<LabDataWithHISDTO> GetLabDataWithHIS(LabDataWithHISSearchDTO searchModel)
        {
            log.MethodStart();

            List<LabDataWithHISDTO> objList = new List<LabDataWithHISDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.LabDataWithHISDTOs.FromSqlRaw<LabDataWithHISDTO>("sp_GET_LabDataWithHIS {0} ,{1} ,{2}"
                                                                                            , searchModel.hos_code
                                                                                            , searchModel.start_date_str
                                                                                            , searchModel.end_date_str).ToList();

                    objList = _mapper.Map<List<LabDataWithHISDTO>>(objDataList);

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

        public List<HISDetailDTO> GetSTGHISUploadDetail(LabDataWithHISSearchDTO searchModel)
        {
            log.MethodStart();

            List<HISDetailDTO> objList = new List<HISDetailDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.HISDetailDTOs.FromSqlRaw<HISDetailDTO>("sp_GET_TRSTGHISFileUploadDetail {0} ,{1} ,{2}"
                                                                                            , searchModel.hos_code
                                                                                            , searchModel.start_date_str
                                                                                            , searchModel.end_date_str).ToList();

                    objList = _mapper.Map<List<HISDetailDTO>>(objDataList);

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

        public List<STGLabFileDataDetailDTO> GetLabDataWithHISDetail(LabDataWithHISSearchDTO searchModel)
        {
            log.MethodStart();
            
            List<STGLabFileDataDetailDTO> objList = new List<STGLabFileDataDetailDTO>();
            
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.STGLabFileDataDetailDTOs.FromSqlRaw<STGLabFileDataDetailDTO>("sp_GET_LabDataWithHISDetail {0}, {1}, {2} "
                                                                                                , searchModel.hos_code
                                                                                                , searchModel.start_date_str
                                                                                                , searchModel.end_date_str).ToList();

                    objList = _mapper.Map<List<STGLabFileDataDetailDTO>>(objDataList);

                    trans.Commit();
                }
                catch (Exception ex)
                {
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
    }
}
