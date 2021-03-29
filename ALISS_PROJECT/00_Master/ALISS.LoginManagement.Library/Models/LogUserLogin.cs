using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.LoginManagement.Library.Models
{
    public class LogUserLogin
    {
        public int log_id { get; set; }
        public string log_app_id { get; set; }
        public string log_usr_id { get; set; }
        public string log_usr_password { get; set; }
        public string log_access_ip { get; set; }
        public string log_session_id { get; set; }
        public DateTime? log_login_timestamp { get; set; }
        public string log_idPass { get; set; }
        public DateTime? log_logout_timestamp { get; set; }
        public string log_status { get; set; }
        public string log_remark { get; set; }
    }
}
