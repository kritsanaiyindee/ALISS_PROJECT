using ALISS.TC.WHONET_Antibiotics.DataAccess;
using ALISS.TC.WHONET_Antibiotics.Models;
using AutoMapper;
using Log4NetLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALISS.TR.NoticeMessage
{
    public class TCWHONET_AntibioticsDAC : ITCWHONET_AntibioticsDAC
    {
        private static readonly ILogService log = new LogService(typeof(TCWHONET_AntibioticsDAC));

        private readonly ALISSContext _db;

        public TCWHONET_AntibioticsDAC()
        {
            _db = new ALISSContext();
        }

        public void Insert(TCWHONET_Antibiotics model)
        {
            log.MethodStart();

            var objData = new TCWHONET_Antibiotics();
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = _db.TCWHONET_AntibioticsModel.Add(model);

                    _db.SaveChanges();

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    // TODO: Handle failure
                    log.Error(ex);

                    trans.Rollback();
                }
                finally
                {
                    trans.Dispose();
                }
            }

            log.MethodFinish();
        }

        public void Update(TCWHONET_Antibiotics model)
        {
            log.MethodStart();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objData = _db.TCWHONET_AntibioticsModel.FirstOrDefault(x => x.who_ant_id == model.who_ant_id);

                    //if (objData != null)
                    //{
                    //    objData = _mapper.Map<TCWHONET_Antibiotics>(model);
                    //}

                    _db.SaveChanges();

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    // TODO: Handle failure
                    log.Error(ex);

                    trans.Rollback();
                }
                finally
                {
                    trans.Dispose();
                }
            }

            log.MethodFinish();
        }

        public void Inactive(TCWHONET_Antibiotics model)
        {
            log.MethodStart();

            model.who_ant_status = "I";
            model.who_ant_active = false;

            try
            {
                Update(model);
            }
            catch (Exception ex)
            {
                // TODO: Handle failure
                log.Error(ex);
            }
            finally
            {
            }

            log.MethodFinish();
        }

        public TCWHONET_Antibiotics GetData(TCWHONET_Antibiotics searchModel)
        {
            log.MethodStart();

            var objData = new TCWHONET_Antibiotics();
            try
            {
                objData = GetList(searchModel)?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                // TODO: Handle failure
                log.Error(ex);
            }
            finally
            {
            }

            log.MethodFinish();

            return objData;
        }

        public List<TCWHONET_Antibiotics> GetList(TCWHONET_Antibiotics searchModel)
        {
            log.MethodStart();

            var objList = new List<TCWHONET_Antibiotics>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.TCWHONET_AntibioticsModel.FromSqlRaw<TCWHONET_Antibiotics>("sp_GET_TCWHONET_Antibiotics {0}", searchModel).ToList();

                    _db.SaveChanges();

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    // TODO: Handle failure
                    log.Error(ex);

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

        public List<TCWHONET_Antibiotics> GetList_Active()
        {
            log.MethodStart();

            var objList = new List<TCWHONET_Antibiotics>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.TCWHONET_AntibioticsModel.FromSqlRaw<TCWHONET_Antibiotics>("sp_GET_TCWHONET_Antibiotics_Active").ToList();

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    // TODO: Handle failure
                    log.Error(ex);

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
