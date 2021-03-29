using ALISS.ANTIBIOTREND.DTO;
using ALISS.ANTIBIOTREND.Library.DataAccess;
using AutoMapper;
using Log4NetLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALISS.ANTIBIOTREND.Library
{
    public class AntibiotrendService : IAntibiotrendService
    {
        private static readonly ILogService log = new LogService(typeof(AntibiotrendService));

        private readonly AntibiotrendContext _db;
        private readonly IMapper _mapper;

        public AntibiotrendService(AntibiotrendContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<SP_AntimicrobialResistanceDTO> GetAMRWithModel(SP_AntimicrobialResistanceSearchDTO searchModel)
        {
            log.MethodStart();

            List<SP_AntimicrobialResistanceDTO> objList = new List<SP_AntimicrobialResistanceDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.DropdownAMRListDTOs.FromSqlRaw<SP_AntimicrobialResistanceDTO>("sp_GET_RPAntibicromialResistance {0},{1},{2},{3}"
                                                                                                    , searchModel.org_codes
                                                                                                    , searchModel.anti_codes
                                                                                                    , searchModel.start_year
                                                                                                    , searchModel.end_year
                                                                                                    ).ToList();

                    objList = _mapper.Map<List<SP_AntimicrobialResistanceDTO>>(objDataList);

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

        public List<NationHealthStrategyDTO> GetAMRNationStrategyWithModel(AMRStrategySearchDTO searchModel)
        {
            log.MethodStart();

            List<NationHealthStrategyDTO> objList = new List<NationHealthStrategyDTO>();

       
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.AMRNationHealthStrategyListDTOs.FromSqlRaw<NationHealthStrategyDTO>("sp_GET_RPNationHealthStrategy {0},{1}"
                                                                                                    , searchModel.month_start_str
                                                                                                    , searchModel.month_end_str
                                                                                                    ).ToList();

                    objList = _mapper.Map<List<NationHealthStrategyDTO>>(objDataList);

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

        public List<AntibiotrendAMRStrategyDTO> GetAntibiotrendAMRStrategyWithModel(AMRStrategySearchDTO searchModel)
        {
            log.MethodStart();

            List<AntibiotrendAMRStrategyDTO> objList = new List<AntibiotrendAMRStrategyDTO>();


            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.AntibiotrendAMRStrategyListDTOs.FromSqlRaw<AntibiotrendAMRStrategyDTO>("sp_GET_RPAntibiotrendAMRStrategy {0},{1}"
                                                                                                    , searchModel.month_start_str
                                                                                                    , searchModel.month_end_str
                                                                                                    ).ToList();

                    objList = _mapper.Map<List<AntibiotrendAMRStrategyDTO>>(objDataList);

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

        public List<SP_AntimicrobialResistanceDTO> GetAMRByOverallWithModel(SP_AntimicrobialResistanceSearchDTO searchModel)
        {
            log.MethodStart();

            List<SP_AntimicrobialResistanceDTO> objList = new List<SP_AntimicrobialResistanceDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.DropdownAMRListDTOs.FromSqlRaw<SP_AntimicrobialResistanceDTO>("sp_GET_RPAntibicromialResstAll {0},{1},{2},{3}"
                                                                                                    , searchModel.org_codes
                                                                                                    , searchModel.anti_codes
                                                                                                    , searchModel.start_year
                                                                                                    , searchModel.end_year
                                                                                                    ).ToList();

                    objList = _mapper.Map<List<SP_AntimicrobialResistanceDTO>>(objDataList);

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
        public List<SP_AntimicrobialResistanceDTO> GetAMRBySpecimenWithModel(SP_AntimicrobialResistanceSearchDTO searchModel)
        {
            log.MethodStart();

            List<SP_AntimicrobialResistanceDTO> objList = new List<SP_AntimicrobialResistanceDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.DropdownAMRListDTOs.FromSqlRaw<SP_AntimicrobialResistanceDTO>("sp_GET_RPAntibicromialResstSpecimen {0},{1},{2},{3}"
                                                                                                    , searchModel.org_codes
                                                                                                    , searchModel.anti_codes
                                                                                                    , searchModel.start_year
                                                                                                    , searchModel.end_year
                                                                                                    ).ToList();

                    objList = _mapper.Map<List<SP_AntimicrobialResistanceDTO>>(objDataList);

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
        public List<SP_AntimicrobialResistanceDTO> GetAMRByWardWithModel(SP_AntimicrobialResistanceSearchDTO searchModel)
        {
            log.MethodStart();

            List<SP_AntimicrobialResistanceDTO> objList = new List<SP_AntimicrobialResistanceDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.DropdownAMRListDTOs.FromSqlRaw<SP_AntimicrobialResistanceDTO>("sp_GET_RPAntibicromialResstWard {0},{1},{2},{3}"
                                                                                                    , searchModel.org_codes
                                                                                                    , searchModel.anti_codes
                                                                                                    , searchModel.start_year
                                                                                                    , searchModel.end_year
                                                                                                    ).ToList();

                    objList = _mapper.Map<List<SP_AntimicrobialResistanceDTO>>(objDataList);

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
