using System;
using System.Collections.Generic;
using System.Text;
using ALISS.DropDownList.DTO;

namespace ALISS.DropDownList.Library
{
    public interface IHospitalDataService
    {
        List<HospitalDataDTO> Get_Hospital_List(HospitalDataDTO searchModel);
        List<HospitalDataDTO> Get_TCHospital_List(HospitalDataDTO searchModel);
    }
}
