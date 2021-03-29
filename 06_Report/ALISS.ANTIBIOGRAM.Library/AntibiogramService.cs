using ALISS.ANTIBIOGRAM.Library.DataAccess;
using ALISS.ANTIBIOGRAM.DTO;
using AutoMapper;
using Log4NetLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json;
//using ALISS.ANTIBIOGRAM.Library.Models;

namespace ALISS.ANTIBIOGRAM.Library
{
    public class AntibiogramService : IAntibiogramService
    {
        private static readonly ILogService log = new LogService(typeof(AntibiogramService));

        private readonly ReportContext _db;
        private readonly IMapper _mapper;

        public AntibiogramService(ReportContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<AntibiogramHospitalTemplateDTO> GetAntibiogramHospitalTemplateWithModel(AntiHospitalSearchDTO searchModel)
        {
            log.MethodStart();

            List<AntibiogramHospitalTemplateDTO> objList = new List<AntibiogramHospitalTemplateDTO>();
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.DropdownHospitalTemplateListDTOs.FromSqlRaw<AntibiogramHospitalTemplateDTO>("sp_GET_RPAntibiogramHospitalTemplateFile {0},{1},{2},{3},{4},{5}"
                                                                                                , searchModel.hos_code
                                                                                                , searchModel.prv_code
                                                                                                , searchModel.arh_code
                                                                                                , searchModel.spc_code
                                                                                                , searchModel.start_date
                                                                                                , searchModel.end_date
                                                                                                ).ToList();

                    objList = _mapper.Map<List<AntibiogramHospitalTemplateDTO>>(objDataList);

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
        public List<AntibiogramAreaHealthTemplateDTO> GetAntibiogramAreaHealthTemplateWithModel(AntiAreaHealthSearchDTO searchModel)
        {
            log.MethodStart();

            List<AntibiogramAreaHealthTemplateDTO> objList = new List<AntibiogramAreaHealthTemplateDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.DropdownAreaHealthTemplateListDTOs.FromSqlRaw<AntibiogramAreaHealthTemplateDTO>("sp_GET_RPAntibiogramAreaHealthTemplateFile {0},{1},{2},{3}"
                                                                                                , searchModel.arh_code
                                                                                                , searchModel.spc_code
                                                                                                , searchModel.start_date
                                                                                                , searchModel.end_date
                                                                                                ).ToList();

                    objList = _mapper.Map<List<AntibiogramAreaHealthTemplateDTO>>(objDataList);

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

        public List<AntibiogramNationTemplateDTO> GetAntibiogramNationTemplateWithModel(AntiNationSearchDTO searchModel)
        {
            log.MethodStart();

            List<AntibiogramNationTemplateDTO> objList = new List<AntibiogramNationTemplateDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.DropdownNationTemplateListDTOs.FromSqlRaw<AntibiogramNationTemplateDTO>("sp_GET_RPAntibiogramNationTemplateFile {0},{1},{2}"
                                                                                                , searchModel.spc_code
                                                                                                , searchModel.start_date
                                                                                                , searchModel.end_date
                                                                                                ).ToList();

                    objList = _mapper.Map<List<AntibiogramNationTemplateDTO>>(objDataList);

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

        public List<RPAntibiogramSurveilAntibioticDTO> GetAntibiogramSurveilAntibioticList_Active_WithModel(RPAntibiogramSurveilAntibioticDTO searchModel)
        {
            log.MethodStart();

            List<RPAntibiogramSurveilAntibioticDTO> objList = new List<RPAntibiogramSurveilAntibioticDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.RPAntibiogramSurveilAntibioticDTOs.FromSqlRaw<RPAntibiogramSurveilAntibioticDTO>("sp_GET_RPAntibiogramSurveilAntibiotic_Active {0}", searchModel.ant_code).ToList();

                    objList = _mapper.Map<List<RPAntibiogramSurveilAntibioticDTO>>(objDataList);

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

        public List<RPAntibiogramSurveilOrganismDTO> GetAntibiogramSurveilOrganismList_Active_WithModel(RPAntibiogramSurveilOrganismDTO searchModel)
        {
            log.MethodStart();

            List<RPAntibiogramSurveilOrganismDTO> objList = new List<RPAntibiogramSurveilOrganismDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.RPAntibiogramSurveilOrganismDTOs.FromSqlRaw<RPAntibiogramSurveilOrganismDTO>("sp_GET_RPAntibiogramSurveilOrganism_Active {0}", searchModel.org_code).ToList();

                    objList = _mapper.Map<List<RPAntibiogramSurveilOrganismDTO>>(objDataList);

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

        //public List<AntibioticDTO> GetList()
        //{
        //    log.MethodStart();

        //    List<AntibioticDTO> objList = new List<AntibioticDTO>();

        //    using (var trans = _db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var objReturn1 = _db.TCAntibiotics.ToList();

        //            objList = _mapper.Map<List<AntibioticDTO>>(objReturn1);

        //            trans.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            // TODO: Handle failure
        //            trans.Rollback();
        //        }
        //        finally
        //        {
        //            trans.Dispose();
        //        }
        //    }

        //    log.MethodFinish();

        //    return objList;
        //}

    }// end class
}
