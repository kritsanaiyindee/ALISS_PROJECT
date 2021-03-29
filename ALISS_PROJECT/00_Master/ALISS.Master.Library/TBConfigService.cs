using ALISS.Master.DTO;
using ALISS.Master.Library.DataAccess;
using AutoMapper;
using Log4NetLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALISS.Master.Library
{
    public class TBConfigService : ITBConfigService
    {
        private static readonly ILogService log = new LogService(typeof(TBConfigService));

        private readonly AuthContext _db;
        private readonly IMapper _mapper;

        public TBConfigService(AuthContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<TBConfigDTO> GetTBConfig(TBConfigDTO searchModel)
        {
            log.MethodStart();

            List<TBConfigDTO> objList = new List<TBConfigDTO>();

            //var searchModel = JsonSerializer.Deserialize<LogProcessSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.TBConfigDTOs.FromSqlRaw<TBConfigDTO>("sp_GET_TBConfig_New {0}", searchModel.tbc_mnu_code).ToList();

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
