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
    public class TRNoticeMessageService : ITRNoticeMessageService
    {
        private static readonly ILogService log = new LogService(typeof(TRNoticeMessageService));

        private readonly MasterContext _db;
        private readonly IMapper _mapper;

        public TRNoticeMessageService(MasterContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<TRNoticeMessageDTO> GetListWithModel(TRNoticeMessageDTO searchModel)
        {
            log.MethodStart();

            List<TRNoticeMessageDTO> objList = new List<TRNoticeMessageDTO>();

            //var searchModel = JsonSerializer.Deserialize<LogProcessSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.TRNoticeMessageDTOs.FromSqlRaw<TRNoticeMessageDTO>("sp_GET_TRNoticeMessage {0}", searchModel.noti_username).ToList();

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
