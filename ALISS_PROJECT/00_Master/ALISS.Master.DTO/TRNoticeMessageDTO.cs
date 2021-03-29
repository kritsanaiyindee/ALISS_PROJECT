﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.Master.DTO
{
    public class TRNoticeMessageDTO
    {
        public int noti_id { get; set; }
        public string noti_username { get; set; }
        public string noti_rol_code { get; set; }
        public string noti_mnu_code { get; set; }
        public string mnu_name { get; set; }
        public string mnu_area { get; set; }
        public string mnu_controller { get; set; }
        public string mnu_page { get; set; }
        public string noti_param_code { get; set; }
        public string noti_message { get; set; }
        public string noti_status { get; set; }
        public bool noti_active { get; set; }
        public string noti_createuser { get; set; }
        public DateTime? noti_createdate { get; set; }
        public string mnu_path
        {
            get
            {
                return (!string.IsNullOrEmpty(mnu_area) ? mnu_area + "/" : "") + (!string.IsNullOrEmpty(mnu_controller) ? mnu_controller + "/" : "") + (!string.IsNullOrEmpty(mnu_page) ? mnu_page + "/" : "");
            }
        }
    }
}
