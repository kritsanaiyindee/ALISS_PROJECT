using ALISS.ANTIBIOGRAM.DTO;
using ALISS.ANTIBIOGRAM.Library.DataAccess;
using ALISS.ANTIBIOGRAM.Library.Models;
using AutoMapper;
using Log4NetLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Globalization;

namespace ALISS.ANTIBIOGRAM.Library
{
   public class ReportService : IReportService
    {
        private static readonly ILogService log = new LogService(typeof(ReportService));

        private readonly ReportContext _db;
        private readonly IMapper _mapper;

        public ReportService(ReportContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
   
        public List<AntibiogramDataDTO> GetAntibiogramHospitalData()
        {
            log.MethodStart();

            List<AntibiogramDataDTO> objList = new List<AntibiogramDataDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.DropdownListDTOs.FromSqlRaw<AntibiogramDataDTO>("sp_GET_RPAntibiogramHospital").ToList();

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

        public List<AntibiogramDataDTO> GetAntibiogramHospitalDataWithParam(string param)
        {
            log.MethodStart();

            List<AntibiogramDataDTO> objList = new List<AntibiogramDataDTO>();

            var searchModel = JsonSerializer.Deserialize<AntibiogramDataDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.DropdownListDTOs.FromSqlRaw<AntibiogramDataDTO>("sp_GET_RPAntibiogramHospital {0},{1},{2},{3},{4}" 
                                                                                    , searchModel.hos_code
                                                                                    , searchModel.prv_code
                                                                                    , searchModel.arh_code
                                                                                    , searchModel.gram
                                                                                    , searchModel.spc_code ).ToList();
                                                                                    //, searchModel.org_code
                                                                                    //, searchModel.month_from
                                                                                    //, searchModel.year_from
                                                                                    //, searchModel.month_to
                                                                                    //, searchModel.year_to
                                                                                   

                    objList = _mapper.Map<List<AntibiogramDataDTO>>(objReturn1);

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

        public List<AntibiogramDataDTO> GetAntibiogramHospitalDataWithModel(AntiHospitalSearchDTO searchModel)
        {
            log.MethodStart();

            List<AntibiogramDataDTO> objList = new List<AntibiogramDataDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.DropdownListDTOs.FromSqlRaw<AntibiogramDataDTO>("sp_GET_RPAntibiogramHospital {0},{1},{2},{3},{4},{5},{6},{7}"
                                                                                                , searchModel.hos_code
                                                                                                , searchModel.prv_code
                                                                                                , searchModel.arh_code
                                                                                                , searchModel.gram
                                                                                                , searchModel.spc_code
                                                                                                , searchModel.org_code
                                                                                                , searchModel.start_date_str
                                                                                                , searchModel.end_date_str
                                                                                                ).ToList();

                    objList = _mapper.Map<List<AntibiogramDataDTO>>(objDataList);

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

        public List<AntibiogramDataDTO> GetAntibiogramAreaHealthData()
        {
            log.MethodStart();

            List<AntibiogramDataDTO> objList = new List<AntibiogramDataDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.DropdownListDTOs.FromSqlRaw<AntibiogramDataDTO>("sp_GET_RPAntibiogramAreaHealth").ToList();

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

        public List<AntibiogramDataDTO> GetAntibiogramAreaHealthDataWithModel(AntiAreaHealthSearchDTO searchModel)
        {
            log.MethodStart();

            List<AntibiogramDataDTO> objList = new List<AntibiogramDataDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.DropdownListDTOs.FromSqlRaw<AntibiogramDataDTO>("sp_GET_RPAntibiogramAreaHealth {0},{1},{2},{3}"
                                                                                                , searchModel.arh_code
                                                                                                , searchModel.spc_code
                                                                                                , searchModel.start_date_str
                                                                                                , searchModel.end_date_str
                                                                                                ).ToList();

                    objList = _mapper.Map<List<AntibiogramDataDTO>>(objDataList);

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

        public List<AntibiogramDataDTO> GetAntibiogramAreaHealthDataWithParam(string param)
        {
            log.MethodStart();

            List<AntibiogramDataDTO> objList = new List<AntibiogramDataDTO>();

            var searchModel = JsonSerializer.Deserialize<AntibiogramDataDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.DropdownListDTOs.FromSqlRaw<AntibiogramDataDTO>("sp_GET_RPAntibiogramAreaHealth {0},{1}", searchModel.arh_code,searchModel.spc_code).ToList();

                    objList = _mapper.Map<List<AntibiogramDataDTO>>(objReturn1);

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

        public List<AntibiogramDataDTO> GetAntibiogramNationData()
        {
            log.MethodStart();

            List<AntibiogramDataDTO> objList = new List<AntibiogramDataDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.DropdownListDTOs.FromSqlRaw<AntibiogramDataDTO>("sp_GET_RPAntibiogramNation").ToList();

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

        public List<AntibiogramDataDTO> GetAntibiogramNationDataWithModel(AntiNationSearchDTO searchModel)
        {
            log.MethodStart();

            List<AntibiogramDataDTO> objList = new List<AntibiogramDataDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.DropdownListDTOs.FromSqlRaw<AntibiogramDataDTO>("sp_GET_RPAntibiogramNation {0},{1},{2}"
                                                                                                , searchModel.spc_code
                                                                                                , searchModel.start_date_str
                                                                                                , searchModel.end_date_str
                                                                                                ).ToList();

                    objList = _mapper.Map<List<AntibiogramDataDTO>>(objDataList);

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

        public List<AntibiogramDataDTO> GetAntibiogramNationDataWithParam(string param)
        {
            log.MethodStart();

            List<AntibiogramDataDTO> objList = new List<AntibiogramDataDTO>();

            var searchModel = JsonSerializer.Deserialize<AntibiogramDataDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.DropdownListDTOs.FromSqlRaw<AntibiogramDataDTO>("sp_GET_RPAntibiogramNation {0}", searchModel.spc_code).ToList();

                    objList = _mapper.Map<List<AntibiogramDataDTO>>(objReturn1);

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

        public List<RPYearlyIsolateListingRISDTO> GetYearlyIsolateListingRIS()
        {
            log.MethodStart();

            List<RPYearlyIsolateListingRISDTO> objList = new List<RPYearlyIsolateListingRISDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.RPYearlyIsolateListingRISDTOs.ToList();

                    objList = _mapper.Map<List<RPYearlyIsolateListingRISDTO>>(objReturn1);

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

        public List<RPYearlyIsolateListingRISDetailDTO> GetYearlyIsolateListingRISDetail()
        {
            log.MethodStart();

            List<RPYearlyIsolateListingRISDetailDTO> objList = new List<RPYearlyIsolateListingRISDetailDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.RPYearlyIsolateListingRISDetailDTOs.ToList();

                    objList = _mapper.Map<List<RPYearlyIsolateListingRISDetailDTO>>(objReturn1);

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
