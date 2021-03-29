using ALISS.Process.DTO;
using ALISS.Process.Library.DataAccess;
using ALISS.Process.Library.Models;
using AutoMapper;
using Log4NetLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ALISS.Process.Library
{
    public class ProcessRequestService : IProcessRequestService
    {
        private static readonly ILogService log = new LogService(typeof(ProcessRequestService));

        private readonly ProcessContext _db;
        private readonly IMapper _mapper;

        public ProcessRequestService(ProcessContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<ProcessRequestDTO> GetList()
        {
            log.MethodStart();

            List<ProcessRequestDTO> objList = new List<ProcessRequestDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TRProcessRequests.ToList();

                    objList = _mapper.Map<List<ProcessRequestDTO>>(objReturn1);

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

        public List<ProcessRequestDTO> GetListWithParam(string param)
        {
            log.MethodStart();

            List<ProcessRequestDTO> objList = new List<ProcessRequestDTO>();

            var searchModel = JsonSerializer.Deserialize<ProcessRequestDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TRProcessRequests.ToList();

                    objList = _mapper.Map<List<ProcessRequestDTO>>(objReturn1);

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

        public List<ProcessRequestDTO> GetListWithModel(ProcessRequestDTO searchModel)
        {
            log.MethodStart();

            List<ProcessRequestDTO> objList = new List<ProcessRequestDTO>();

            //var searchModel = JsonSerializer.Deserialize<HospitalDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TRProcessRequests.ToList();

                    objList = _mapper.Map<List<ProcessRequestDTO>>(objReturn1);

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

        public List<ProcessRequestDetailDTO> GetDetailListWithModel(ProcessRequestDTO searchModel)
        {
            log.MethodStart();

            List<ProcessRequestDetailDTO> objList = new List<ProcessRequestDetailDTO>();

            //var searchModel = JsonSerializer.Deserialize<HospitalDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TRProcessRequestDetails.ToList();

                    objList = _mapper.Map<List<ProcessRequestDetailDTO>>(objReturn1);

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

        public ProcessRequestDTO GetData(string pcr_code)
        {
            log.MethodStart();

            ProcessRequestDTO objModel = new ProcessRequestDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TRProcessRequests.FirstOrDefault(x => x.pcr_code == pcr_code);

                    objModel = _mapper.Map<ProcessRequestDTO>(objReturn1);

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

            return objModel;
        }

        public ProcessRequestDTO SaveData(ProcessRequestDTO model)
        {
            log.MethodStart();

            ProcessRequestDTO objReturn = new ProcessRequestDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = _db.TRProcessRequests.FirstOrDefault(x => x.pcr_code == model.pcr_code);

                    objModel = _mapper.Map<TRProcessRequest>(model);

                    //objModel.hos_status = objModel.hos_active == true ? "A" : "I";

                    _db.SaveChanges();

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

            return objReturn;
        }

        public List<ProcessRequestDetailDTO> SaveDetailData(List<ProcessRequestDetailDTO> modelList)
        {
            log.MethodStart();

            List<ProcessRequestDetailDTO> objReturn = new List<ProcessRequestDetailDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in modelList)
                    {
                        var objModel = _db.TRProcessRequestDetails.FirstOrDefault(x => x.pcd_id == item.pcd_id);

                        objModel = _mapper.Map<TRProcessRequestDetail>(item);

                        //objModel.hos_status = objModel.hos_active == true ? "A" : "I";

                        _db.SaveChanges();
                    }

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

            return objReturn;
        }

    }
}
