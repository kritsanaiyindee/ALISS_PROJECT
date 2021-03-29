using ALISS.DropDownList.DTO;
using ALISS.DropDownList.Library.DataAccess;
using AutoMapper;
using Log4NetLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALISS.DropDownList.Library
{
    public class ParameterService : IParameterService
    {
        private static readonly ILogService log = new LogService(typeof(ParameterService));

        private readonly DropDownListContext _db;
        private readonly IMapper _mapper;

        public ParameterService(DropDownListContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<ParameterDTO> Get_Parameter_List(ParameterDTO searchModel)
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
