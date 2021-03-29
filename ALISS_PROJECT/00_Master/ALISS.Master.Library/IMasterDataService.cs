using ALISS.Master.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.Master.Library
{
    public interface IMasterDataService
    {
        List<MasterDataDTO> GetAreaHealth();
        List<MasterDataDTO> GetPorvince(string arh_Id);
        List<MasterDataDTO> GetHospital(string arh_Id = null, string prv_id = null);
        List<LogProcessDTO> GetLogProcess(string arh_Id = null);
    }
}
