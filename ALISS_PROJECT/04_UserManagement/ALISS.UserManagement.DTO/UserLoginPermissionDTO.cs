using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ALISS.UserManagement.DTO
{
    public class UserLoginPermissionDTO
    {
        public int usp_id { get; set; }
        public string usp_usr_userName { get; set; }
        public string usp_usr_email { get; set; }
        public string usp_rol_code { get; set; }
        public string usp_rol_name { get; set; }
        public string usp_arh_code { get; set; }
        public string usp_arh_name { get; set; }
        public string usp_hos_code { get; set; }
        public string usp_hos_name { get; set; }
        public string usp_lab_code { get; set; }
        public string usp_lab_name { get; set; }
        public string usp_status { get; set; }
        public bool usp_active { get; set; }
        public string usp_createuser { get; set; }
        public DateTime? usp_createdate { get; set; }
        public string usp_updateuser { get; set; }
        public DateTime? usp_updatedate { get; set; }
    }
}
