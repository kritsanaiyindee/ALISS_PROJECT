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
    public class ProvinceDataService : IProvinceDataService
    {
        private static readonly ILogService log = new LogService(typeof(ProvinceDataService));

        private readonly DropDownListContext _db;
        private readonly IMapper _mapper;

        public ProvinceDataService(DropDownListContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<HospitalDataDTO> Get_Province_List(HospitalDataDTO searchModel)
        {
            log.MethodStart();

            List<HospitalDataDTO> objList = new List<HospitalDataDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.HospitalDataDTOs.FromSqlRaw<HospitalDataDTO>("sp_GET_DDL_Province {0}, {1}, {2}", searchModel.arh_code, searchModel.prv_code, searchModel.hos_code).ToList();

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
