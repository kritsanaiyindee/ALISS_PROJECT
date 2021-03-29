using AutoMapper;
using Log4NetLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ALISS.DropDownList.DTO;
using ALISS.DropDownList.Library.DataAccess;

namespace ALISS.DropDownList.Library
{
    public class RoleDataService : IRoleDataService
    {
        private static readonly ILogService log = new LogService(typeof(RoleDataService));

        private readonly DropDownListAuthContext _db;
        private readonly IMapper _mapper;

        public RoleDataService(DropDownListAuthContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<DropDownListDTO> Get_Role_List(DropDownListDTO searchModel)
        {
            log.MethodStart();

            List<DropDownListDTO> objList = new List<DropDownListDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.DropDownListDTOs.FromSqlRaw<DropDownListDTO>("sp_GET_DDL_Role").ToList();

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
