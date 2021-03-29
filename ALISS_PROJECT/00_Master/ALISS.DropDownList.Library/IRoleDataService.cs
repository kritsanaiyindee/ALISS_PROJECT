using System;
using System.Collections.Generic;
using System.Text;
using ALISS.DropDownList.DTO;

namespace ALISS.DropDownList.Library
{
    public interface IRoleDataService
    {
        List<DropDownListDTO> Get_Role_List(DropDownListDTO searchModel);
    }
}
