using ALISS.AUTH.DTO;
using ALISS.LoginManagement.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace ALISS.Data.Account
{
    public class LoginUserData
    {
        public LoginUser CurrentLoginUser { get; set; } = new LoginUser();

        public event Action OnAdded;

        public void AddLoginUser(LoginUser loginUser)
        {
            CurrentLoginUser = loginUser;
            StateChanged();
        }

        public void UpdateTimeStamp()
        {
            CurrentLoginUser.SessionTimeStamp = DateTime.Now;
            StateChanged();
        }

        private void StateChanged() => OnAdded?.Invoke();
    }

    public class LoginUserDataList
    {
        public List<LoginUser> LoginUserList { get; set; } = new List<LoginUser>();
    }

    public class LoginUser
    {
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public string rol_code { get; set; }
        public string rol_name { get; set; }
        public string arh_code { get; set; }
        public string arh_name { get; set; }
        public string prv_code { get; set; }
        public string prv_name { get; set; }
        public string hos_code { get; set; }
        public string hos_name { get; set; }
        public string lab_code { get; set; }
        public string lab_name { get; set; }

        public List<RolePermissionDTO> rol_permission_List { get; set; } = new List<RolePermissionDTO>();

        public List<LoginUserPermissionDTO> LoginUserPermissionList { get; set; } = new List<LoginUserPermissionDTO>();

        public List<LoginUserRolePermissionDTO> LoginUserRolePermissionList { get; set; } = new List<LoginUserRolePermissionDTO>();

        public LoginUserRolePermissionDTO PagePermission { get; set; } = new LoginUserRolePermissionDTO();

        public string ClientIp { get; set; }
        public string SessionId { get; set; }
        public string CurrentSessionId { get; set; }
        public DateTime SessionTimeStamp { get; set; }
        public int SessionTimeout { get; set; }
        public string Fullname
        {
            get
            {
                return Firstname + " " + Lastname;
            }
        }

        public string FullRole
        {
            get
            {
                return rol_name + "(" + (hos_name ?? arh_name ?? "กรมวิทย์ฯ") + ")";
            }
        }

        public bool CheckPagePermission(string currentPage)
        {
            PagePermission = LoginUserRolePermissionList.FirstOrDefault(x => x.rop_mnu_code == currentPage);
            if (PagePermission == null || PagePermission.rop_view == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
