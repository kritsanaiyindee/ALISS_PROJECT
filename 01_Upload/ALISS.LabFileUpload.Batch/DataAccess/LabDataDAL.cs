using System;
using System.Collections.Generic;
using System.Text;
using ALISS.LabFileUpload.DTO;
using ALISS.Mapping.DTO;
using Microsoft.EntityFrameworkCore;
using Log4NetLibrary;
using System.Linq;
using ALISS.LabFileUpload.Batch.Models;
using AutoMapper;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using EFCore.BulkExtensions;

namespace ALISS.LabFileUpload.Batch.DataAccess
{
    public class LabDataDAL
    {
        //private static readonly ILogService log = new LogService(typeof(LabDataDAL));
        private readonly IMapper _mapper;
        //private readonly LabDataContext _db;

        //public LabDataDAL(LabDataContext db, IMapper mapper)
        //{
        //    _db = db;
        //    _mapper = mapper;
        //}

        #region TRLabFileUpload
        public List<LabFileUploadDataDTO> Get_NewLabFileUpload(char status)
        {

            List<LabFileUploadDataDTO> objList = new List<LabFileUploadDataDTO>();
            using (var _db = new LabDataContext())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        objList = _db.LabFileUploadDataDTOs.FromSqlRaw<LabFileUploadDataDTO>("sp_GET_TRLabFileUploadList {0} ,{1} ,{2} ,{3}", null, null, null, status).ToList();
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

            }


            return objList;

        }


        public int Update_LabFileUploadStatus(string lfu_id, char lfu_status, int lfu_error, string lfu_updateuser)
        {
            int rowsAffected = 0;
            using (var _db = new LabDataContext())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        rowsAffected = _db.Database.ExecuteSqlCommand("exec sp_UPDATE_TRLabFileUpload_Status {0},{1},{2},{3}", lfu_id, lfu_status, lfu_error, lfu_updateuser);
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
            }

            return rowsAffected;
        }
        #endregion

        #region TRMapping
        public MappingDataDTO GetMappingData(string mp_id)
        {

            MappingDataDTO objModel = new MappingDataDTO();

            using (var _db = new LabDataContext())
            {
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

            }
            return objModel;
        }
        #endregion

        #region TRWHONetMapping
        public List<WHONetMappingListsDTO> GetWHONetMappingList(string wnm_mappingid, string mst_code)
        {
            List<WHONetMappingListsDTO> objList = new List<WHONetMappingListsDTO>();
            using (var _db = new LabDataContext())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        objList = _db.WHONetMappingListsDTOs.FromSqlRaw<WHONetMappingListsDTO>("sp_GET_TRWHONetMappingList {0}, {1}", wnm_mappingid, mst_code).ToList();

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

            }
            return objList;
        }
        #endregion

        #region TRSTGLabFileDataHeader
        public Guid Save_LabFileDataHeader(TRSTGLabFileDataHeader model)
        {
            var currentDateTime = DateTime.Now;
            Guid objReturn = Guid.Empty;

            using (var _db = new LabDataContext())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {

                        _db.TRSTGLabFileDataHeaders.Add(model);

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

            }

            return objReturn;
        }

        public int Get_CheckExistingHeader(TRSTGLabFileDataHeader model, string mp_program, string mp_filetype)
        {
            int objReturn = 1;
            var currentDateTime = DateTime.Now;
            using (var _db = new LabDataContext())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        //var objResult2 = _db.TRSTGLabFileDataHeaders.FirstOrDefault(x => x.ldh_hos_code == model.ldh_hos_code
                        //                                                  && x.ldh_lab == model.ldh_lab
                        //                                                  && x.ldh_labno == model.ldh_labno
                        //                                                  && x.ldh_organism == model.ldh_organism
                        //                                                  //&& x.ldh_specimen == model.ldh_specimen
                        //                                                  && x.ldh_date == model.ldh_date
                        //                                                  && x.ldh_lfu_id != model.ldh_lfu_id
                        //                                                  && x.ldh_flagdelete == false);

                        var objResult = _db.TRSTGLabFileDataHeaders.FromSqlRaw<TRSTGLabFileDataHeader>("sp_UPLOAD_CheckExistingHeader {0},{1},{2},{3},{4},{5},{6},{7}",
                            model.ldh_hos_code,
                            model.ldh_lab,
                            model.ldh_labno,
                            model.ldh_organism,
                            model.ldh_date,
                            model.ldh_lfu_id,
                            mp_program,
                            mp_filetype
                            ).AsEnumerable().FirstOrDefault();


                        if (objResult != null)
                        {
                            objReturn = objResult.ldh_sequence + 1;

                            objResult.ldh_status = 'D';
                            objResult.ldh_flagdelete = true;
                            objResult.ldh_updateuser = "BATCH";
                            objResult.ldh_updatedate = currentDateTime;
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

            }

            return objReturn;
        }


        public Guid Get_HeaderIdForMultipleline(TRSTGLabFileDataHeader model)
        {
            Guid objReturn = Guid.Empty;
            using (var _db = new LabDataContext())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        var objResult = _db.TRSTGLabFileDataHeaders.FirstOrDefault(x => x.ldh_lfu_id == model.ldh_lfu_id
                                                                          && x.ldh_hos_code == model.ldh_hos_code
                                                                          && x.ldh_lab == model.ldh_lab
                                                                          && x.ldh_labno == model.ldh_labno
                                                                          && x.ldh_organism == model.ldh_organism
                                                                          //&& x.ldh_specimen == model.ldh_specimen
                                                                          && x.ldh_date == model.ldh_date
                                                                          && x.ldh_flagdelete == false);

                        if (objResult != null)
                        {
                            objReturn = objResult.ldh_id;
                        }


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

            }

            return objReturn;
        }




        #endregion

        #region TRSTGLabFileDataDetail
        public STGLabFileDataDetailDTO Save_LabFileDataDetail(List<TRSTGLabFileDataDetail> model)
        {
            var currentDateTime = DateTime.Now;
            STGLabFileDataDetailDTO objReturn = new STGLabFileDataDetailDTO();
            using (var _db = new LabDataContext())
            {
                _db.BulkInsert(model);
                //using (var trans = _db.Database.BeginTransaction())
                //{
                //    try
                //    {
                       
                //        //_db.TRSTGLabFileDataDetails.
                //       _db.TRSTGLabFileDataDetails.AddRange(model);

                //        _db.SaveChanges();

                //        trans.Commit();

                //        //objReturn = _mapper.Map<STGLabFileDataDetailDTO>(objModel);
                //    }
                //    catch (Exception ex)
                //    {
                //        // TODO: Handle failure
                //        trans.Rollback();
                //    }
                //    finally
                //    {
                //        trans.Dispose();
                //    }
                //}

            }

            return objReturn;
        }

        #endregion

        #region TCParameter
        public List<TCParameter> GetParameter()
        {
            List<TCParameter> objReturn = new List<TCParameter>();

            using (var _db = new LabDataContext())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        objReturn = _db.TCParameters.FromSqlRaw<TCParameter>("sp_GET_TCParameter {0}", "UPLOAD_KEY").ToList();
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
            }

            return objReturn;
        }
        #endregion

        #region TRLabFileErrorHeader
        public Guid Save_TRLabFileErrorHeader(TRLabFileErrorHeader model)
        {
            var currentDateTime = DateTime.Now;
            Guid objReturn = Guid.Empty;

            using (var _db = new LabDataContext())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        if (model.feh_status == 'N')
                        {
                            _db.TRLabFileErrorHeaders.Add(model);
                        }
                        else if (model.feh_status == 'E')
                        {
                            var objModel = new TRLabFileErrorHeader();
                            objModel = _db.TRLabFileErrorHeaders.FirstOrDefault(x => x.feh_id == model.feh_id);
                            objModel.feh_errorrecord = model.feh_errorrecord;

                        }

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

            }

            return objReturn;
        }
        #endregion

        #region TRLabFileErrorDetail
        public Guid Save_TRLabFileErrorDetail(TRLabFileErrorDetail model)
        {
            var currentDateTime = DateTime.Now;
            Guid objReturn = Guid.Empty;

            using (var _db = new LabDataContext())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {

                        _db.TRLabFileErrorDetails.Add(model);

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

            }

            return objReturn;
        }

        public TRLabFileErrorDetail Save_TRLabFileErrorDetailList(List<TRLabFileErrorDetail> model)
        {
            var currentDateTime = DateTime.Now;
            TRLabFileErrorDetail objReturn = new TRLabFileErrorDetail();
            using (var _db = new LabDataContext())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {

                        _db.TRLabFileErrorDetails.AddRange(model);

                        _db.SaveChanges();

                        trans.Commit();

                        //objReturn = _mapper.Map<STGLabFileDataDetailDTO>(objModel);
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

            }

            return objReturn;
        }


        #endregion




    }
}
