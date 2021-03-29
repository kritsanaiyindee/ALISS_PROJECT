using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ALISS.AUTH.DTO;

namespace ALISS.AUTH.Library
{
    public interface IMenuService
    {
        List<MenuDTO> GetList();
        List<MenuDTO> GetListWithParam(string param);
        List<MenuDTO> GetListWithModel(MenuSearchDTO searchModel);
        MenuDTO GetData(string mnu_id);
        MenuDTO SaveData(MenuDTO model);
    }
}
