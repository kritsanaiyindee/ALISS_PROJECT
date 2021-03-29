using System;
using AutoMapper;
using Log4NetLibrary;
using ALISS.LabFileUpload.Library.DataAccess;
using ALISS.LabFileUpload.DTO;
using ALISS.LabFileUpload.Library.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ALISS.LabFileUpload.Library
{
    public class LabFileUploadService : ILabFileUploadService
    {
        private static readonly ILogService log = new LogService(typeof(LabFileUploadService));
       
        private readonly LabFileUploadContext _db;
        private readonly IMapper _mapper;

        public LabFileUploadService(LabFileUploadContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }


        public LabFileUploadDataDTO SaveLabFileUploadData(LabFileUploadDataDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            LabFileUploadDataDTO objReturn = new LabFileUploadDataDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TRLabFileUpload();

                    if (model.lfu_status == 'N')
                    {
                        objModel = _mapper.Map<TRLabFileUpload>(model);
                        
                        objModel.lfu_createdate = currentDateTime;
                        objModel.lfu_updatedate = currentDateTime;

                        _db.TRLabFileUploads.Add(objModel);
                    }
                    else  
                    {
                        objModel = _db.TRLabFileUploads.FirstOrDefault(x => x.lfu_id == model.lfu_id);
                        objModel.lfu_status = model.lfu_status;
                        objModel.lfu_version = model.lfu_version;
                        objModel.lfu_flagdelete = model.lfu_flagdelete;
                        objModel.lfu_lab = model.lfu_lab;
                        objModel.lfu_FileName = model.lfu_FileName;
                        objModel.lfu_Program = model.lfu_Program;
                        objModel.lfu_Path = model.lfu_Path;
                        objModel.lfu_FileType = model.lfu_FileType;
                        objModel.lfu_TotalRecord = model.lfu_TotalRecord;
                        objModel.lfu_AerobicCulture = model.lfu_AerobicCulture;
                        objModel.lfu_ErrorRecord = model.lfu_ErrorRecord;
                        objModel.lfu_StartDatePeriod = model.lfu_StartDatePeriod;
                        objModel.lfu_EndDatePeriod = model.lfu_EndDatePeriod;
                        objModel.lfu_approveduser = model.lfu_approveduser;
                        objModel.lfu_approveddate = model.lfu_approveddate;
                        objModel.lfu_updateuser = model.lfu_updateuser;
                        objModel.lfu_updatedate = currentDateTime;
                    }


                    _db.SaveChanges();

                    trans.Commit();

                    objReturn = _mapper.Map<LabFileUploadDataDTO>(objModel);
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

        public LabFileUploadDataDTO GetLabFileUploadDataById(string lfu_id)
        {
            log.MethodStart();
            LabFileUploadDataDTO objModel = new LabFileUploadDataDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TRLabFileUploads.FirstOrDefault(x => x.lfu_id.ToString() == lfu_id);

                    objModel = _mapper.Map<LabFileUploadDataDTO>(objReturn1);

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

        public List<LabFileUploadDataDTO> GetLabFileUploadListWithModel(LabFileUploadSearchDTO searchModel)
        {
            log.MethodStart();


            List<LabFileUploadDataDTO> objList = new List<LabFileUploadDataDTO>();
                     
            using (var trans = _db.Database.BeginTransaction())
            {

                try
                {
                    objList = _db.LabFileUploadDataDTOs.FromSqlRaw<LabFileUploadDataDTO>("sp_GET_TRLabFileUploadList {0} ,{1} ,{2} ,{3}", searchModel.lfu_Hos, searchModel.lfu_Province, searchModel.lfu_Area,null).ToList();
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

        public List<LabFileErrorHeaderListDTO> GetLabFileErrorHeaderListBylfuId(string lfu_id)
        {
            log.MethodStart();
            List<LabFileErrorHeaderListDTO> objList = new List<LabFileErrorHeaderListDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {

                try
                {
                    objList = _db.LabFileErrorHeaderListDTOs.FromSqlRaw<LabFileErrorHeaderListDTO>("sp_GET_TRLabFileErrorHeaderList {0}", lfu_id).ToList();
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

        public List<LabFileErrorDetailListDTO> GetLabFileErrorDetailListBylfuId(string lfu_id)
        {
            log.MethodStart();
            List<LabFileErrorDetailListDTO> objList = new List<LabFileErrorDetailListDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {

                try
                {
                    objList = _db.LabFileErrorDetailListDTOs.FromSqlRaw<LabFileErrorDetailListDTO>("sp_GET_TRLabFileErrorDetailList {0}", lfu_id).ToList();
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


        public List<LabFileSummaryHeaderListDTO> GetLabFileSummaryHeaderBylfuId(string lfu_id)
        {
            log.MethodStart();
            List<LabFileSummaryHeaderListDTO> objList = new List<LabFileSummaryHeaderListDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {

                try
                {
                    objList = _db.LabFileSummaryHeaderListDTOs.FromSqlRaw<LabFileSummaryHeaderListDTO>("sp_GET_TRLabFileSummaryHeaderList {0}", lfu_id).ToList();
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

        public List<LabFileSummaryDetailListDTO> GetLabFileSummaryDetailBylfuId(string fsh_id)
        {
            log.MethodStart();
            List<LabFileSummaryDetailListDTO> objList = new List<LabFileSummaryDetailListDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {

                try
                {
                    objList = _db.LabFileSummaryDetailListDTOs.FromSqlRaw<LabFileSummaryDetailListDTO>("sp_GET_TRLabFileSummaryDetailList {0}",fsh_id).ToList();
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
    }
}
