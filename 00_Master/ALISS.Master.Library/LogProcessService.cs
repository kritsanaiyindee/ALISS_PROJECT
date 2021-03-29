using ALISS.Master.DTO;
using ALISS.Master.Library.DataAccess;
using AutoMapper;
using Log4NetLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ALISS.Master.Library
{
    public class LogProcessService : ILogProcessService
    {
        private static readonly ILogService log = new LogService(typeof(LogProcessService));

        private readonly AuthContext _dbAuth;
        private readonly MasterContext _db;
        private readonly IMapper _mapper;

        public LogProcessService(AuthContext dbAuth, MasterContext db, IMapper mapper)
        {
            _dbAuth = dbAuth;
            _db = db;
            _mapper = mapper;
        }

        public List<LogProcessDTO> GetLogProcessListWithModel(LogProcessSearchDTO searchModel)
        {
            log.MethodStart();

            List<LogProcessDTO> objList = new List<LogProcessDTO>();

            //var searchModel = JsonSerializer.Deserialize<LogProcessSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.LogProcessDTOs.FromSqlRaw<LogProcessDTO>("sp_GET_LogProcess {0}, {1}", searchModel.log_mnu_name, searchModel.log_tran_id).ToList();

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

        public List<LogProcessDTO> GetLogProcessAuthListWithModel(LogProcessSearchDTO searchModel)
        {
            log.MethodStart();

            List<LogProcessDTO> objList = new List<LogProcessDTO>();

            //var searchModel = JsonSerializer.Deserialize<LogProcessSearchDTO>(param);

            using (var trans = _dbAuth.Database.BeginTransaction())
            {
                try
                {
                    objList = _dbAuth.LogProcessDTOs.FromSqlRaw<LogProcessDTO>("sp_GET_LogProcess {0}, {1}", searchModel.log_mnu_name, searchModel.log_tran_id).ToList();

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
