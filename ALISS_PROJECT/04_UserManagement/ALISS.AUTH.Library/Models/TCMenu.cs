using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.AUTH.Library.Models
{
    public class TCMenu
    {
        public int mnu_id { get; set; }
        public string mnu_code { get; set; }
        public int mnu_order { get; set; }
        public int mnu_order_sub { get; set; }
        public string mnu_name { get; set; }
        public string mnu_icon { get; set; }
        public string mnu_area { get; set; }
        public string mnu_controller { get; set; }
        public string mnu_page { get; set; }
        public string mnu_status { get; set; }
        public bool mnu_active { get; set; }
        public string mnu_createuser { get; set; }
        public DateTime? mnu_createdate { get; set; }
        public string mnu_updateuser { get; set; }
        public DateTime? mnu_updatedate { get; set; }
    }
}
