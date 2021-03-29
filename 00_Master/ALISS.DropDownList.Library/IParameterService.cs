using ALISS.DropDownList.DTO;
using System.Collections.Generic;

namespace ALISS.DropDownList.Library
{
    public interface IParameterService
    {
        List<ParameterDTO> Get_Parameter_List(ParameterDTO searchModel);
    }
}