using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.UserManagement.Library.Models
{
    public class TCUserLoginPermission
    {
        public int usp_id { get; set; }
        public string usp_usr_userName { get; set; }
        public string usp_rol_code { get; set; }
        public string usp_arh_code { get; set; }
        public string usp_hos_code { get; set; }
        public string usp_status { get; set; }
        public bool usp_active { get; set; }
        public string usp_createuser { get; set; }
        public DateTime? usp_createdate { get; set; }
        public string usp_updateuser { get; set; }
        public DateTime? usp_updatedate { get; set; }
    }
}
