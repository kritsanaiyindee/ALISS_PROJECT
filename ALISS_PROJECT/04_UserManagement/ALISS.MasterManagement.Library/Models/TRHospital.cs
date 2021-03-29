using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.MasterManagement.Library.Models
{
    public class TRHospital
    {
        public int hos_id { get; set; }
        public string hos_code { get; set; }
        public string hos_name { get; set; }
        public string hos_arh_code { get; set; }
        public string hos_prv_code { get; set; }
        public string hos_status { get; set; }
        public bool hos_active { get; set; }
        public string hos_createuser { get; set; }
        public DateTime? hos_createdate { get; set; }
        public string hos_updateuser { get; set; }
        public DateTime? hos_updatedate { get; set; }
    }
}
