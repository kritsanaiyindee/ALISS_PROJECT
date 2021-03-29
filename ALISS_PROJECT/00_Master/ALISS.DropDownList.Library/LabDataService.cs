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
    public class LabDataService : ILabDataService
    {
        private static readonly ILogService log = new LogService(typeof(LabDataService));

        private readonly DropDownListContext _db;
        private readonly IMapper _mapper;

        public LabDataService(DropDownListContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<DropDownListDTO> Get_Lab_List(DropDownListDTO searchModel)
        {
            log.MethodStart();

            List<DropDownListDTO> objList = new List<DropDownListDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.DropDownListDTOs.FromSqlRaw<DropDownListDTO>("sp_GET_DDL_Lab {0}", searchModel.data_Value).ToList();

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

        public List<HospitalLabDataDTO> Get_AllLab_List(HospitalLabDataDTO searchModel)
        {
            log.MethodStart();

            List<HospitalLabDataDTO> objList = new List<HospitalLabDataDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    objList = _db.HospitalLabDataDTOs.FromSqlRaw<HospitalLabDataDTO>("sp_GET_DDL_AllLab {0}, {1}, {2}, {3}", searchModel.arh_code, searchModel.prv_code, searchModel.hos_code, searchModel.lab_code).ToList();

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
