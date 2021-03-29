using ALISS.AUTH.DTO;
using ALISS.AUTH.Library.DataAccess;
using ALISS.AUTH.Library.Models;
using AutoMapper;
using Log4NetLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.Json;

namespace ALISS.AUTH.Library
{
    public class MasterService : IMasterService
    {
        private static readonly ILogService log = new LogService(typeof(MenuService));

        private readonly AuthContext _db;
        private readonly IMapper _mapper;

        public MasterService(AuthContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        //public List<LogProcessDTO> GetLogProcessListWithParam(string param)
        //{
        //    log.MethodStart();

        //    List<LogProcessDTO> objList = new List<LogProcessDTO>();

        //    var searchModel = JsonSerializer.Deserialize<LogProcessSearchDTO>(param);

        //    using (var trans = _db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            objList = _db.LogProcessDTOs.FromSqlRaw<LogProcessDTO>("sp_GET_LogProcess {0}, {1}", searchModel.log_mnu_name, searchModel.log_tran_id).ToList();

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

    }
}
