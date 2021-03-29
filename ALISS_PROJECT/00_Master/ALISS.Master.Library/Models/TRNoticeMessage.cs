using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.Master.Library.Models
{
    public class TRNoticeMessage
    {
        public int noti_id { get; set; }
        public string noti_username { get; set; }
        public string noti_rol_code { get; set; }
        public string noti_mnu_code { get; set; }
        public string noti_param_code { get; set; }
        public string noti_message { get; set; }
        public string noti_status { get; set; }
        public bool noti_active { get; set; }
        public string noti_createuser { get; set; }
        public DateTime? noti_createdate { get; set; }
        public string noti_updateuser { get; set; }
        public DateTime? noti_updatedate { get; set; }
    }
}
