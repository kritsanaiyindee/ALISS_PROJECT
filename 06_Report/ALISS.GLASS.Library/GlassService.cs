using ALISS.DropDownList.DTO;
using ALISS.GLASS.DTO;
using ALISS.GLASS.Library.DataAccess;
using AutoMapper;
using Log4NetLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ALISS.GLASS.Library
{
    public class GlassService : IGlassService
    {
        private static readonly ILogService log = new LogService(typeof(GlassService));

        private readonly GlassContext _db;
        private readonly IMapper _mapper;
        public GlassService(GlassContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<GlassFileListDTO> GetGlassPublicFileListData()
        {
            log.MethodStart();

            List<GlassFileListDTO> objList = new List<GlassFileListDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.DropdownGlassListDTOs.FromSqlRaw<GlassFileListDTO>("sp_GET_RPGlassPublicFileList").ToList();

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

        public List<GlassFileListDTO> GetGlassPublicFileListDataModel(GlassFileListNationSearchDTO searchModel)
        {
            log.MethodStart();

            List<GlassFileListDTO> objList = new List<GlassFileListDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.DropdownGlassListDTOs.FromSqlRaw<GlassFileListDTO>("sp_GET_RPGlassPublicFileList {0},{1}"
                                                                                                    , searchModel.start_year
                                                                                                    , searchModel.end_year
                                                                                                    ).ToList();

                    objList = _mapper.Map<List<GlassFileListDTO>>(objDataList);

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

        public List<GlassFileListDTO> GetGlassPublicRegHealthFileListDataModel(GlassFileListNationSearchDTO searchModel)
        {
            log.MethodStart();

            List<GlassFileListDTO> objList = new List<GlassFileListDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.DropdownGlassListDTOs.FromSqlRaw<GlassFileListDTO>("sp_GET_RPGlassPublicFileList {0},{1},{2}"
                                                                                                    , searchModel.start_year
                                                                                                    , searchModel.end_year
                                                                                                    , searchModel.arh_code
                                                                                                    ).ToList();

                    objList = _mapper.Map<List<GlassFileListDTO>>(objDataList);

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

        public List<GlassInfectOriginOverviewDTO> GetGlassInfectOverviewModel(GlassFileListDTO searchModel)
        {
            log.MethodStart();

            List<GlassInfectOriginOverviewDTO> objList = new List<GlassInfectOriginOverviewDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.DropdownGlassOverviewListDTOs.FromSqlRaw<GlassInfectOriginOverviewDTO>("sp_GET_RPGlassInfectOriginOverview {0},{1}"
                                                                                                    , searchModel.arh_code
                                                                                                    , searchModel.year
                                                                                                    ).ToList();

                    objList = _mapper.Map<List<GlassInfectOriginOverviewDTO>>(objDataList);

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

        public List<GlassPathogenNSDTO> GetGlassPathogenNSModel(GlassFileListDTO searchModel)
        {
            log.MethodStart();

            List<GlassPathogenNSDTO> objList = new List<GlassPathogenNSDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.DropdownGlassPathogenNSListDTOs.FromSqlRaw<GlassPathogenNSDTO>("sp_GET_RPGlassPathogenNS {0},{1}"
                                                                                                    , searchModel.arh_code
                                                                                                    , searchModel.year
                                                                                                    ).ToList();

                    objList = _mapper.Map<List<GlassPathogenNSDTO>>(objDataList);

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

        public List<GlassInfectSpecimenDTO> GlassInfectSpecimenModel(GlassFileListDTO searchModel)
        {
            log.MethodStart();

            List<GlassInfectSpecimenDTO> objList = new List<GlassInfectSpecimenDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.DropdownGlassInfectSpecimenDTOs.FromSqlRaw<GlassInfectSpecimenDTO>("sp_GET_RPGlassInfectSpecimen {0},{1}"
                                                                                                    , searchModel.arh_code
                                                                                                    , searchModel.year
                                                                                                    ).ToList();

                    objList = _mapper.Map<List<GlassInfectSpecimenDTO>>(objDataList);

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

        public List<GlassInfectPathAntiCombineDTO> GetGlassInfectPathAntiCombineModel(GlassFileListDTO searchModel)
        {
            log.MethodStart();

            List<GlassInfectPathAntiCombineDTO> objList = new List<GlassInfectPathAntiCombineDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.DropdownGlassInfectPathAntiCombineDTOs.FromSqlRaw<GlassInfectPathAntiCombineDTO>("sp_GET_RPGlassInfectPathAntiCombine {0},{1}"
                                                                                                    , searchModel.arh_code
                                                                                                    , searchModel.year
                                                                                                    ).ToList();

                    objList = _mapper.Map<List<GlassInfectPathAntiCombineDTO>>(objDataList);

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

        public List<ParameterDTO> GetGlassReportPath(ParameterDTO searchModel)
        {
            log.MethodStart();

            List<ParameterDTO> objList = new List<ParameterDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.ParameterDTOs.FromSqlRaw<ParameterDTO>("sp_GET_TCParameter {0}", searchModel.prm_code_major).ToList();

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
