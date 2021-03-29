using System;
using System.Collections.Generic;

namespace ALISS.LoginManagement.DTO
{
    public class LoginUserDTO
    {
        public int usr_id { get; set; }
        public string usr_username { get; set; }
        public string usr_firstname { get; set; }
        public string usr_lastname { get; set; }

        public string str_LoginUserPermission_List { get; set; }

        public string str_LoginUserRolePermission_List { get; set; }
    }

    public class LoginUserPermissionDTO
    {
        public int usp_id { get; set; }
        public string usp_usr_username { get; set; }
        public string usp_rol_code { get; set; }
        public string usp_rol_name { get; set; }
        public string usp_arh_code { get; set; }
        public string usp_arh_name { get; set; }
        public string usp_prv_code { get; set; }
        public string usp_prv_name { get; set; }
        public string usp_hos_code { get; set; }
        public string usp_hos_name { get; set; }
        public string usp_lab_code { get; set; }
        public string usp_lab_name { get; set; }
    }

    public class LoginUserRolePermissionDTO
    {
        public int rop_id { get; set; }
        public string rop_rol_code { get; set; }
        public string rop_mnu_code { get; set; }
        public int mnu_order { get; set; }
        public int mnu_order_sub { get; set; }
        public string mnu_name { get; set; }
        public string mnu_icon { get; set; }
        public string mnu_area { get; set; }
        public string mnu_controller { get; set; }
        public string mnu_page { get; set; }
        public string mnu_status { get; set; }
        public bool mnu_active { get; set; }
        public bool rop_view { get; set; }
        public bool rop_create { get; set; }
        public bool rop_edit { get; set; }
        public bool rop_approve { get; set; }
        public bool rop_print { get; set; }
        public bool rop_reject { get; set; }
        public bool rop_cancel { get; set; }
        public bool rop_return { get; set; }
        public bool rop_complete { get; set; }
        public bool rop_implement { get; set; }
        public string rop_status { get; set; }
        public bool rop_active { get; set; }

        public string mnu_group
        {
            get
            {
                return rop_mnu_code?.Substring(0, 6);
            }
        }
        public string mnu_path
        {
            get
            {
                return (!string.IsNullOrEmpty(mnu_area) ? mnu_area + "/" : "") + (!string.IsNullOrEmpty(mnu_controller) ? mnu_controller + "/" : "") + (!string.IsNullOrEmpty(mnu_page) ? mnu_page + "/" : "");
            }
        }
    }

    public class LoginUserSearchDTO
    {
        public string usr_username { get; set; }
        public string usr_password { get; set; }
        public string usr_clientIp { get; set; }
        public string usr_sessionId { get; set; }

    }
}
