using System;
using System.Collections.Generic;
using System.Text;
using ALISS.DropDownList.DTO;

namespace ALISS.DropDownList.Library
{
    public interface IProvinceDataService
    {
        List<HospitalDataDTO> Get_Province_List(HospitalDataDTO searchModel);
    }
}
