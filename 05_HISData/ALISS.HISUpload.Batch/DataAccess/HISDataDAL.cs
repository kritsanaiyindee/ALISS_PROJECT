using ALISS.HISUpload.Batch.Models;
using ALISS.HISUpload.DTO;
using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALISS.HISUpload.Batch.DataAccess
{
    public class HISDataDAL
    {
        private readonly IMapper _mapper;
        #region TRLabFileUpload
        public List<HISUploadDataDTO> Get_NewHISFileUpload(char status)
        {
            List<HISUploadDataDTO> objList = new List<HISUploadDataDTO>();

            using (var _db = new HISDataContext())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        objList = _db.HISUploadDataDTOs.FromSqlRaw<HISUploadDataDTO>("sp_GET_TRHISFileUploadList {0} ,{1} ,{2} ,{3}, {4}"
                                                                                        , null, null, null, status, 1).ToList();

                       


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
        public List<HISFileTemplateDTO> GetHISFileTemplate_Active()
        {
            List<HISFileTemplateDTO> objList = new List<HISFileTemplateDTO>();
            using (var _db = new HISDataContext())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        objList = _db.HISFileTemplateDTOs.FromSqlRaw<HISFileTemplateDTO>("sp_GET_TCHISFileTemplate_Active").ToList();
                      
                        trans.Commit();
                    }
                    catch
                    {
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
        public int Update_HISFileUploadStatus(int hfu_id, char hfu_status, int hfu_matching_records, int hfu_duplicate_records, string hfu_updateuser)
        {
            int rowsAffected = 0;
            using (var _db = new HISDataContext())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        rowsAffected = _db.Database.ExecuteSqlCommand("exec sp_UPDATE_TRHISFileUpload_Status {0},{1},{2},{3},{4}" 
                                                                                                            , hfu_id 
                                                                                                            , hfu_status 
                                                                                                            , hfu_matching_records
                                                                                                            , hfu_duplicate_records
                                                                                                            , hfu_updateuser);
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

        #region TRSTGHSFileDataHeader
        public int Save_HISFileDataHeader(TRSTGHISFileUploadHeader model)
        {
            var currentDateTime = DateTime.Now;
            int objReturn = 0;
            using (var _db = new HISDataContext())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        _db.TRSTGHISFileUploadHeaders.Add(model);
                        _db.SaveChanges();
                        trans.Commit();
                        objReturn = model.huh_id;


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
        public List<TRSTGHISFileUploadHeader> Save_HISFileDataHeaderList(List<TRSTGHISFileUploadHeader> model)
        {
            var currentDateTime = DateTime.Now;
            List<TRSTGHISFileUploadHeader> objReturn = new List<TRSTGHISFileUploadHeader>();
           
            using (var _db = new HISDataContext())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        var hfu_id = model.FirstOrDefault().huh_hfu_id;
                        //_db.TRSTGHISFileUploadDetails.AddRange(model);
                        _db.BulkInsert(model);
                        _db.SaveChanges();
                        trans.Commit();

                        objReturn = _db.TRSTGHISFileUploadHeaders.FromSqlRaw<TRSTGHISFileUploadHeader>("sp_GET_TRSTGHISFileUploadHeader {0} "
                                                                                                    , hfu_id
                                                                                                    ).ToList(); ;

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
        public Tuple<int, Boolean> Get_CheckExistingHeader(TRSTGHISFileUploadHeader model,  string mp_filetype)
        {
            int iNextSeqNo = 1;
            bool blnDup = false;
            var currentDateTime = DateTime.Now;
            using (var _db = new HISDataContext())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                                            
                        var objResult = _db.TRSTGHISFileUploadHeaders.FromSqlRaw<TRSTGHISFileUploadHeader>("sp_UPLOAD_HIS_CheckExisting_SP_Header {0},{1},{2},{3},{4},{5},{6}",
                                                                                                    model.huh_hos_code,
                                                                                                    model.huh_ref_no,
                                                                                                    model.huh_lab_no,
                                                                                                    model.huh_hn_no,
                                                                                                    model.huh_date,
                                                                                                    model.huh_hfu_id,
                                                                                                    mp_filetype
                                                                                                    ).AsEnumerable().FirstOrDefault();


                            if (objResult != null)
                            {
                                iNextSeqNo = objResult.huh_seq_no + 1;
                                blnDup = true;
                                objResult.huh_status = 'D';
                                objResult.huh_delete_flag = true;
                                objResult.huh_updateuser = "BATCH";
                                objResult.huh_updatedate = currentDateTime;
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

            return Tuple.Create(iNextSeqNo, blnDup);
        }
        public string Get_CheckExistingHeaders(TRSTGHISFileUploadHeader model, string mp_filetype, ref int refNextSeq, ref bool refIsDup)
        {
            string returnvalue = "";
            //int iNextSeqNo = 1;
            //bool blnDup = false;
            var currentDateTime = DateTime.Now;
            using (var _db = new HISDataContext())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {

                        var objResult = _db.TRSTGHISFileUploadHeaders.FromSqlRaw<TRSTGHISFileUploadHeader>("sp_UPLOAD_HIS_CheckExisting_SP_Header {0},{1},{2},{3},{4},{5},{6}",
                                                                                                    model.huh_hos_code,
                                                                                                    model.huh_ref_no,
                                                                                                    model.huh_lab_no,
                                                                                                    model.huh_hn_no,
                                                                                                    model.huh_date,
                                                                                                    model.huh_hfu_id,
                                                                                                    mp_filetype
                                                                                                    ).AsEnumerable().FirstOrDefault();


                        if (objResult != null)
                        {
                            refNextSeq = objResult.huh_seq_no + 1;
                            refIsDup = true;
                            objResult.huh_status = 'D';
                            objResult.huh_delete_flag = true;
                            objResult.huh_updateuser = "BATCH";
                            objResult.huh_updatedate = currentDateTime;
                            _db.SaveChanges();
                        }

                        trans.Commit();
                        returnvalue = "OK";
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
            return returnvalue;
        }
        public Tuple<int, Boolean> Get_CheckExistingDetail(TRSTGHISFileUploadHeader modelHeader, TRSTGHISFileUploadDetail modelDetail)
        {
            int iNextSeqNo = 1;
            bool blnDup = false;
            var currentDateTime = DateTime.Now;
            using (var _db = new HISDataContext())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {                      
                        var objResult = _db.TRSTGHISFileUploadDetails.FromSqlRaw<TRSTGHISFileUploadDetail>("sp_UPLOAD_HIS_CheckExisting_Detail {0},{1},{2},{3},{4},{5},{6},{7}",
                                                                                                            modelHeader.huh_hos_code
                                                                                                            , modelHeader.huh_ref_no
                                                                                                            , modelHeader.huh_lab_no
                                                                                                            , modelHeader.huh_hn_no
                                                                                                            , modelHeader.huh_date
                                                                                                            , modelHeader.huh_hfu_id
                                                                                                            , modelDetail.hud_field_name
                                                                                                            , modelDetail.hud_field_value
                                                                                                            ).AsEnumerable();
                        if (objResult != null)
                        {
                            foreach (var obj in objResult)
                            {
                                iNextSeqNo = obj.hud_seq_no + 1;
                                blnDup = true;
                                obj.hud_status = 'D';
                                obj.hud_updateuser = "BATCH";
                                obj.hud_updatedate = currentDateTime;
                            }

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

            return Tuple.Create(iNextSeqNo, blnDup);
        }
        public int Get_HeaderIdForMultipleline(TRSTGHISFileUploadHeader model)
        {
            int objReturn = 0;
            using (var _db = new HISDataContext())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        var objResult = _db.TRSTGHISFileUploadHeaders.FirstOrDefault(x => x.huh_hfu_id == model.huh_hfu_id
                                                                          && x.huh_hos_code == model.huh_hos_code
                                                                          && x.huh_ref_no == model.huh_ref_no
                                                                          && x.huh_lab_no == model.huh_lab_no
                                                                          && x.huh_hn_no == model.huh_hn_no
                                                                          && x.huh_date == model.huh_date
                                                                          && x.huh_delete_flag == false);

                        if (objResult != null)
                        {
                            objReturn = objResult.huh_id;
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

        #region TRSTGHSFileDataDetail
        public STGHISFileUploadDetailDTO Save_HISFileDataDetail(List<TRSTGHISFileUploadDetail> model)
        {
            var currentDateTime = DateTime.Now;
            STGHISFileUploadDetailDTO objReturn = new STGHISFileUploadDetailDTO();
            using (var _db = new HISDataContext())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        //_db.TRSTGHISFileUploadDetails.AddRange(model);
                        _db.BulkInsert(model);
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

        #region
        public bool Save_HISFileUploadSummary(TRHISFileUploadSummary model)
        {
            var currentDateTime = DateTime.Now;
            bool objComplete = false;

            using (var _db = new HISDataContext())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        _db.TRHISFileUploadSummarys.Add(model);
                        _db.SaveChanges();
                        trans.Commit();

                        objComplete = true;
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

            return objComplete;
        }
        #endregion

        #region TCParameter
        public List<TCParameter> GetParameter()
        {
            List<TCParameter> objReturn = new List<TCParameter>();

            using (var _db = new HISDataContext())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        objReturn = _db.TCParameters.FromSqlRaw<TCParameter>("sp_GET_TCParameter {0}", "SP_UPLOAD_KEY").ToList();
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

        public int GetLabDataWithHISMatching(string hos_code, int hfu_id)
        {
            
            List<LabDataWithHISMatchingDTO> objList = new List<LabDataWithHISMatchingDTO>();
            int iMatchingRecord = 0;

            using (var _db = new HISDataContext())
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {

                        var objDataList = _db.LabDataWithHISMatchingDTOs.FromSqlRaw<LabDataWithHISMatchingDTO>("sp_GET_LabWithHISMatching {0} ,{1} "
                                                                                                    , hos_code
                                                                                                    , hfu_id).ToList();
                        trans.Commit();

                        iMatchingRecord = objDataList.Count();
                    }
                    catch
                    {
                        trans.Rollback();
                    }
                    finally
                    {
                        trans.Dispose();
                    }
                }
            }
     
            return iMatchingRecord;
        }
    }
}
