using ALISS.AUTH.DTO;
using ALISS.AUTH.Library.DataAccess;
using AutoMapper;
using Log4NetLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ALISS.AUTH.Library.Models;

namespace ALISS.AUTH.Library
{
    public class PasswordConfigService : IPasswordConfigService
    {
        private static readonly ILogService log = new LogService(typeof(PasswordConfigService));

        private readonly AuthContext _db;
        private readonly IMapper _mapper;

        private readonly string _MenuCode = "MNU_0102";

        public PasswordConfigService(AuthContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public PasswordConfigDTO GetData(string pwc_Id)
        {
            log.MethodStart();

            PasswordConfigDTO objModel = new PasswordConfigDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objReturn1 = _db.TCPasswordConfigs.FirstOrDefault(x => x.pwc_id == 1);

                    objModel = _mapper.Map<PasswordConfigDTO>(objReturn1);

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

        public PasswordConfigDTO SaveData(PasswordConfigDTO model)
        {
            log.MethodStart();

            var currentDateTime = DateTime.Now;
            var currentUser = "";
            PasswordConfigDTO objReturn = new PasswordConfigDTO();

            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    var objModel = new TCPasswordConfig();

                    objModel = _db.TCPasswordConfigs.FirstOrDefault();

                    objModel.pwc_user_min_char = model.pwc_user_min_char;
                    objModel.pwc_user_max_char = model.pwc_user_max_char;
                    objModel.pwc_min_char = model.pwc_min_char;
                    objModel.pwc_max_char = model.pwc_max_char;
                    objModel.pwc_lowwer_letter = model.pwc_lowwer_letter;
                    objModel.pwc_upper_letter = model.pwc_upper_letter;
                    objModel.pwc_number = model.pwc_number;
                    objModel.pwc_special_char = model.pwc_special_char;
                    objModel.pwc_max_invalid = model.pwc_max_invalid;
                    objModel.pwc_force_reset = model.pwc_force_reset;
                    objModel.pwc_session_timeout = model.pwc_session_timeout;
                    objModel.pwc_default_password = model.pwc_default_password;
                    objModel.pwc_updateuser = model.pwc_updateuser;
                    objModel.pwc_updatedate = currentDateTime;

                    currentUser = objModel.pwc_updateuser;

                    //_db.TCMenus.Update(objModel);

                    #region Save Log Process ...
                    _db.LogProcesss.Add(new LogProcess()
                    {
                        log_usr_id = currentUser,
                        log_mnu_id = _MenuCode,
                        log_mnu_name = "PasswordConfig",
                        log_tran_id = "",
                        log_action = "Update",
                        log_desc = "Update PasswordConfig",
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
