using ALISS.Master.DTO;
using ALISS.TC.WHONET_Antibiotics.Models;
using ALISS.TR.NoticeMessage;
using AutoMapper;
using Log4NetLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALISS.Master.NoticeMessage.Library
{
    public class WHONETService : IWHONETService
    {
        private static readonly ILogService log = new LogService(typeof(WHONETService));

        private readonly IMapper _mapper;
        private readonly ITCWHONET_AntibioticsDAC _dac;

        public WHONETService(IMapper mapper)
        {
            _mapper = mapper;
            _dac = new TCWHONET_AntibioticsDAC();
        }

        public List<TCWHONET_AntibioticsDTO> Get_TCWHONET_Antibiotics_Acitve_List()
        {
            log.MethodStart();

            var objList = new List<TCWHONET_AntibioticsDTO>();

            try
            {
                var objGetList = _dac.GetList_Active();
                objList = objGetList.Select(x => new TCWHONET_AntibioticsDTO()
                {
                    who_ant_id = x.who_ant_id,
                    who_ant_mst_code = x.who_ant_mst_code,
                    who_ant_source = x.who_ant_source,
                    who_ant_code = x.who_ant_code,
                    who_ant_name = x.who_ant_name,
                    who_ant_type = x.who_ant_type,
                    who_ant_lab = x.who_ant_lab,
                    who_ant_size =x.who_ant_size,
                    who_ant_S = x.who_ant_S,
                    who_ant_I = x.who_ant_I,
                    who_ant_R = x.who_ant_R,
                    who_ant_status = x.who_ant_status,
                    who_ant_active = x.who_ant_active,
                    who_ant_createuser = x.who_ant_createuser,
                    who_ant_createdate = x.who_ant_createdate,
                    who_ant_updateuser = x.who_ant_updateuser,
                    who_ant_updatedate = x.who_ant_updatedate
                }).ToList();
            }
            catch (Exception ex)
            {
                // TODO: Handle failure
                log.Error(ex);
            }
            finally
            {

            }

            log.MethodFinish();

            return objList;
        }

    }
}
