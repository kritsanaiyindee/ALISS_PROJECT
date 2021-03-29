using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ALISS.AUTH.DTO;

namespace ALISS.AUTH.Library
{
    public interface IRoleService
    {
        List<RoleDTO> GetList();
        List<RoleDTO> GetListWithParam(string param);
        List<RoleDTO> GetListWithModel(RoleSearchDTO searchModel);
        RoleDTO GetData(string rol_code);
        List<RolePermissionDTO> GetPermissionListWithModel(RoleSearchDTO searchModel);
        RoleDTO SaveData(RoleDTO model);
        List<RolePermissionDTO> SaveListData(List<RolePermissionDTO> modelList);
    }
}
