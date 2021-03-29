using System;
using System.Collections.Generic;
using System.Text;
using ALISS.MasterManagement.DTO;

namespace ALISS.MasterManagement.Library
{
    public interface IMasterHospitalService
    {
        List<MasterHospitalDTO> GetList();
        List<MasterHospitalDTO> GetListWithParam(string param);
        List<MasterHospitalDTO> GetListWithModel(MasterHospitalSearchDTO searchModel);
        MasterHospitalDTO GetData(string hos_code);
        MasterHospitalDTO SaveData(MasterHospitalDTO model);
    }
}
