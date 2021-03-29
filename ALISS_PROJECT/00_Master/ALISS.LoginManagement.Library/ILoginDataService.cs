using ALISS.LoginManagement.DTO;
using System.Collections.Generic;

namespace ALISS.LoginManagement.Library
{
    public interface ILoginDataService
    {
        LoginUserDTO Get_LoginUser_Data(LoginUserSearchDTO searchModel);
        string Get_LoginUserPermission_Data(string usr_username);
        string Get_LoginUserRolePermission_Data(string param);
        List<LoginUserPermissionDTO> Get_LoginUserPermission_List(LoginUserSearchDTO searchModel);
        LoginUserDTO Set_WrongPassword_Data(LoginUserSearchDTO searchModel);
        LoginUserDTO Set_Inactive_Data(LoginUserSearchDTO searchModel);
        LoginUserDTO Set_LogoutUser_Data(LoginUserSearchDTO searchModel);
        LoginUserDTO Set_TimeoutUser_Data(LoginUserSearchDTO searchModel);
    }
}