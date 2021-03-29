using ALISS.UserManagement.DTO;
using System.Collections.Generic;

namespace ALISS.UserManagement.Library
{
    public interface IUserLoginDataService
    {
        List<UserLoginDTO> GetList();
        //List<UserLoginDTO> GetListWithParam(string param);
        List<UserLoginPermissionDTO> GetListWithModel(UserLoginSearchDTO searchModel);
        List<UserLoginDTO> GetList_Active_WithModel(UserLoginSearchDTO searchModel);
        UserLoginDTO GetData(string usr_userName);
        UserLoginDTO SaveData(UserLoginDTO model);
        UserLoginDTO SavePassword(UserLoginDTO model);
    }
}