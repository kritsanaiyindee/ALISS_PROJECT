using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.MasterManagement.Library.Models
{
    public class LogProcess
    {
        public int log_id { get; set; }
        public string log_app_id { get; set; }
        public string log_usr_id { get; set; }
        public string log_usr_name { get; set; }
        public string log_rol_id { get; set; }
        public string log_mnu_id { get; set; }
        public string log_mnu_name { get; set; }
        public string log_tran_id { get; set; }
        public string log_status { get; set; }
        public string log_action { get; set; }
        public string log_desc { get; set; }
        public string log_createuser { get; set; }
        public DateTime? log_createdate { get; set; }
        public string log_updateuser { get; set; }
        public DateTime? log_updatedate { get; set; }
    }
}
