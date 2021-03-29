using AutoMapper;
using Log4NetLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ALISS.LoginManagement.DTO;
using ALISS.LoginManagement.Library.DataAccess;
using ALISS.LoginManagement.Library.Models;

namespace ALISS.LoginManagement.Library
{
    public class MenuDataService : IMenuDataService
    {
        private static readonly ILogService log = new LogService(typeof(MenuDataService));

        private readonly AuthContext _db;
        private readonly IMapper _mapper;

        public MenuDataService(AuthContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<MenuDataDTO> Get_Menu_List()
        {
            log.MethodStart();

            List<MenuDataDTO> objReturn = new List<MenuDataDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    List<MenuData> objResult = _db.MenuDatas.FromSqlRaw<MenuData>("sp_Get_MenuList").ToList();

                    var objResultDTO = _mapper.Map<List<MenuDataDTO>>(objResult);

                    objReturn = objResultDTO.Where(x => x.mnu_order == 0).ToList();

                    foreach(var item in objReturn)
                    {
                        item.strSubMenuDataDTO = JsonSerializer.Serialize(objResultDTO.Where(x => x.mnu_group == item.mnu_group && x.mnu_order != 0).ToList());
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

        public List<MenuDataDTO> Get_Menu_List(string rol_code)
        {
            log.MethodStart();

            List<MenuDataDTO> objReturn = new List<MenuDataDTO>();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    List<MenuData> objResult = _db.MenuDatas.FromSqlRaw<MenuData>("sp_Get_MenuList {0}", rol_code).ToList();

                    var objResultDTO = _mapper.Map<List<MenuDataDTO>>(objResult);

                    objReturn = objResultDTO.Where(x => x.mnu_order == 0).ToList();

                    foreach(var item in objReturn)
                    {
                        item.strSubMenuDataDTO = JsonSerializer.Serialize(objResultDTO.Where(x => x.mnu_group == item.mnu_group && x.mnu_order != 0).ToList());
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
