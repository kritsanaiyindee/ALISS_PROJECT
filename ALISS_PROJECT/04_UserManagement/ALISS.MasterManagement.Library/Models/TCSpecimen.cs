using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.MasterManagement.Library.Models
{
    public class TCSpecimen
    {
        public int spc_id { get; set; }
        public string spc_mst_code { get; set; }
        public string spc_code { get; set; }
        public string spc_name { get; set; }
        public string spc_desc { get; set; }
        public string spc_status { get; set; }
        public bool spc_active { get; set; }
        public string spc_createuser { get; set; }
        public DateTime? spc_createdate { get; set; }
        public string spc_updateuser { get; set; }
        public DateTime? spc_updatedate { get; set; }
    }
}
