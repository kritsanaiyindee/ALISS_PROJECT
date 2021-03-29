using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using AutoMapper;
using Log4NetLibrary;
using ALISS.AUTH.Library.DataAccess;
using ALISS.AUTH.Library.Models;
using ALISS.AUTH.DTO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ALISS.AUTH.Library
{
    public class MenuService : IMenuService
    {
        private static readonly ILogService log = new LogService(typeof(MenuService));
        
        private readonly AuthContext _db;
        private readonly IMapper _mapper;

        public MenuService(AuthContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<MenuDTO> GetList()
        {
            log.MethodStart();

            List<MenuDTO> objList = new List<MenuDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCMenus.ToList();

                    objList = _mapper.Map<List<MenuDTO>>(objReturn1);

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

        public List<MenuDTO> GetListWithParam(string param)
        {
            log.MethodStart();

            List<MenuDTO> objList = new List<MenuDTO>();

            var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TCMenus.FromSqlRaw<TCMenu>("sp_GET_TCMenu {0}", searchModel.sch_mnu_name).ToList();

                    objList = _mapper.Map<List<MenuDTO>>(objDataList);

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

        public List<MenuDTO> GetListWithModel(MenuSearchDTO searchModel)
        {
            log.MethodStart();

            List<MenuDTO> objList = new List<MenuDTO>();

            //var searchModel = JsonSerializer.Deserialize<MenuSearchDTO>(param);

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objDataList = _db.TCMenus.FromSqlRaw<TCMenu>("sp_GET_TCMenu {0}, {1}", searchModel.sch_mnu_code, searchModel.sch_mnu_name).ToList();

                    objList = _mapper.Map<List<MenuDTO>>(objDataList);

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

        public MenuDTO GetData(string mnu_code)
        {
            log.MethodStart();

            MenuDTO objModel = new MenuDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCMenus.FirstOrDefault(x => x.mnu_code == mnu_code);

                    if (objReturn1 != null)
                    {
                        objModel = _mapper.Map<MenuDTO>(objReturn1);
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

            return objModel;
        }

        public MenuDTO SaveData(MenuDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            var currentUser = "";
            MenuDTO objReturn = new MenuDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TCMenu();

                    if (model.mnu_status == "E")
                    {
                        objModel = _db.TCMenus.FirstOrDefault(x => x.mnu_id == model.mnu_id);
                    }

                    if (model.mnu_status == "N")
                    {
                        objModel = _mapper.Map<TCMenu>(model);

                        objModel.mnu_status = objModel.mnu_active == true ? "A" : "I";
                        objModel.mnu_createdate = currentDateTime;

                        currentUser = objModel.mnu_createuser;

                        _db.TCMenus.Add(objModel);
                    }
                    else if (model.mnu_status == "E")
                    {
                        objModel.mnu_order = model.mnu_order;
                        objModel.mnu_order_sub = model.mnu_order_sub;
                        objModel.mnu_name = model.mnu_name;
                        objModel.mnu_area = model.mnu_area;
                        objModel.mnu_controller = model.mnu_controller;
                        objModel.mnu_page = model.mnu_page;
                        objModel.mnu_active = model.mnu_active;
                        objModel.mnu_status = objModel.mnu_active == true ? "A" : "I";
                        objModel.mnu_updateuser = model.mnu_updateuser;
                        objModel.mnu_updatedate = currentDateTime;

                        currentUser = objModel.mnu_updateuser;

                        //_db.TCMenus.Update(objModel);
                    }


                    #region Save Log Process ...
                    _db.LogProcesss.Add(new LogProcess()
                    {
                        log_usr_id = currentUser,
                        log_mnu_id = "",
                        log_mnu_name = "Menu",
                        log_tran_id = objModel.mnu_code.ToString(),
                        log_action = (model.mnu_status == "N" ? "New" : "Update"),
                        log_desc = (model.mnu_status == "N" ? "New" : "Update") + " Menu " + objModel.mnu_name,
                        log_createuser = currentUser,
                        log_createdate = currentDateTime
                    });
                    #endregion

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

    }
}
