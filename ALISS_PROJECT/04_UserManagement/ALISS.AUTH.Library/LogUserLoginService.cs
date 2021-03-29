using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Log4NetLibrary;
using ALISS.AUTH.DTO;
using ALISS.AUTH.Library.DataAccess;

namespace ALISS.AUTH.Library
{
    public class LogUserLoginService : ILogUserLoginService
    {
        private static readonly ILogService log = new LogService(typeof(LogUserLoginService));

        private readonly AuthContext _db;
        private readonly IMapper _mapper;

        public LogUserLoginService(AuthContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<LogUserLoginDTO> GetListWithModel(LogUserLoginDTO searchModel)
        {
            log.MethodStart();

            List<LogUserLoginDTO> objList = new List<LogUserLoginDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.LogUserLoginDTOs.FromSqlRaw<LogUserLoginDTO>("sp_GET_LogUserLogin {0}, {1}, {2}", searchModel.log_usr_id, searchModel.log_login_timestamp, (searchModel.log_logout_timestamp?.AddDays(1).AddMilliseconds(-1))).ToList();

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
