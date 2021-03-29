using ALISS.LoginManagement.DTO;
using System.Collections.Generic;

namespace ALISS.LoginManagement.Library
{
    public interface IMenuDataService
    {
        List<MenuDataDTO> Get_Menu_List();
        List<MenuDataDTO> Get_Menu_List(string rol_code);
    }
}