using System;
using System.Collections.Generic;
using System.Text;
using ALISS.UserManagement.DTO;

namespace ALISS.UserManagement.Library
{
    public interface IHospitalService
    {
        List<HospitalDTO> GetList();
        List<HospitalDTO> GetListWithParam(string param);
        List<HospitalDTO> GetListWithModel(HospitalSearchDTO searchModel);
        HospitalDTO GetData(string mnu_id);
        List<HospitalLabDTO> GetLabListWithModel(HospitalSearchDTO searchModel);
        HospitalLabDTO GetLabData(string hol_code);
        HospitalDTO SaveData(HospitalDTO model);
        List<HospitalLabDTO> SaveLabData(List<HospitalLabDTO> modelList);
    }
}
