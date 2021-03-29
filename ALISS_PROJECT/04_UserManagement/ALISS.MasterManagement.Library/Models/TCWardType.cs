using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.MasterManagement.Library.Models
{
    public class TCWardType
    {
        public int wrd_id { get; set; }
        public string wrd_mst_code { get; set; }
        public string wrd_code { get; set; }
        public string wrd_name { get; set; }
        public string wrd_desc { get; set; }
        public string wrd_status { get; set; }
        public bool wrd_active { get; set; }
        public string wrd_createuser { get; set; }
        public DateTime? wrd_createdate { get; set; }
        public string wrd_updateuser { get; set; }
        public DateTime? wrd_updatedate { get; set; }
    }
}
