using System;
using System.Collections.Generic;
using System.Text;
using ALISS.DropDownList.DTO;

namespace ALISS.DropDownList.Library
{
    public interface IAreaHealthDataService
    {
        List<HospitalDataDTO> Get_AreaHealth_List(HospitalDataDTO searchModel);
    }
}
