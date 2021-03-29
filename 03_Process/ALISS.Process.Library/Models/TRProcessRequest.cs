using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.Process.Library.Models
{
    public class TRProcessRequest
    {
        public int pcr_id { get; set; }
        public string pcr_code { get; set; }
        public string pcr_arh_code { get; set; }
        public string pcr_prv_code { get; set; }
        public string pcr_hos_code { get; set; }
        public string pcr_type { get; set; }
        public string pcr_spc_code { get; set; }
        public string pcr_month_start { get; set; }
        public string pcr_month_end { get; set; }
        public string pcr_year { get; set; }
        public string pcr_prev_code { get; set; }
        public string pcr_status { get; set; }
        public bool pcr_active { get; set; }
        public string pcr_createuser { get; set; }
        public DateTime? pcr_createdate { get; set; }
        public string pcr_updateuser { get; set; }
        public DateTime? pcr_updatedate { get; set; }
    }
}
