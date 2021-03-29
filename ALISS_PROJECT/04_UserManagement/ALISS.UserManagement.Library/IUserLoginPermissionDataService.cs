using ALISS.UserManagement.DTO;
using System.Collections.Generic;

namespace ALISS.UserManagement.Library
{
    public interface IUserLoginPermissionDataService
    {
        List<UserLoginPermissionDTO> GetList();
        List<UserLoginPermissionDTO> GetListWithParam(string param);
        List<UserLoginPermissionDTO> GetListWithModel(UserLoginPermissionDTO searchModel);
        List<UserLoginPermissionDTO> GetList_Active_WithModel(UserLoginPermissionDTO searchModel);
        UserLoginPermissionDTO GetData(string usp_usr_userName);
        UserLoginPermissionDTO SaveData(UserLoginPermissionDTO model);
        string CheckDuplicate(UserLoginPermissionDTO model);
        List<UserLoginPermissionDTO> SaveListData(List<UserLoginPermissionDTO> model);
    }
}