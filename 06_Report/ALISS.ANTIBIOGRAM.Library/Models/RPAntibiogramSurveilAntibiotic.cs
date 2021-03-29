using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.ANTIBIOGRAM.Library.Models
{
    public class RPAntibiogramSurveilAntibiotic
    {
        public int id { get; set; }
        public string ant_code { get; set; }
        public string spc_code { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public bool active_flag { get; set; }
        public string remarks { get; set; }
        public string createuser { get; set; }
        public DateTime? createdate { get; set; }
        public string updateuser { get; set; }
        public DateTime? updatedate { get; set; }
    }
}
