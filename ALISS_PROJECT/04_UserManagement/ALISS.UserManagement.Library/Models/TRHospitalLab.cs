using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.UserManagement.Library.Models
{
    public class TRHospitalLab
    {
        public int lab_id { get; set; }
        public string lab_hos_code { get; set; }
        public string lab_code { get; set; }
        public string lab_name { get; set; }
        public string lab_type { get; set; }
        public string lab_prg_type { get; set; }
        public string lab_status { get; set; }
        public bool lab_active { get; set; }
        public string lab_createuser { get; set; }
        public DateTime? lab_createdate { get; set; }
        public string lab_updateuser { get; set; }
        public DateTime? lab_updatedate { get; set; }
    }
}
