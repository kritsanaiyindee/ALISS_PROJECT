using ALISS.DropDownList.DTO;
using System.Collections.Generic;

namespace ALISS.DropDownList.Library
{
    public interface ILabDataService
    {
        List<DropDownListDTO> Get_Lab_List(DropDownListDTO searchModel);
        List<HospitalLabDataDTO> Get_AllLab_List(HospitalLabDataDTO searchModel);
    }
}