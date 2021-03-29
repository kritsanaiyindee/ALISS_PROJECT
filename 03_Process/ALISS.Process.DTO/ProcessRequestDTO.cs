using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ALISS.Process.DTO
{
    public class ProcessRequestDTO
    {
        public int pcr_id { get; set; }
        public string pcr_code { get; set; }
        public string pcr_arh_code { get; set; }
        public string pcr_prv_code { get; set; }
        public string pcr_hos_code { get; set; }
        public string pcr_hos_name { get; set; }
        public string pcr_lab_code { get; set; }
        [Required(ErrorMessage = "* Please select type")]
        public string pcr_type { get; set; }
        public string pcr_spc_code { get; set; }
        [Required(ErrorMessage = "* Please select start month")]
        public string pcr_month_start { get; set; }
        [Required(ErrorMessage = "* Please select end month")]
        public string pcr_month_end { get; set; }
        [Required(ErrorMessage = "* Please select year")]
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
