using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ALISS.MasterManagement.DTO
{
    public class WHONETColumnDTO
    {
        public int wnc_id { get; set; }
        public string wnc_mst_code { get; set; }
        [Required(ErrorMessage = "* Please enter WHONET Column code")]
        [RegularExpression("([a-zA-Z0-9_-]+)", ErrorMessage = "Wrong format.")]
        public string wnc_code { get; set; }
        [Required(ErrorMessage = "* Please enter WHONET Column name")]
        public string wnc_name { get; set; }
        public string wnc_data_type { get; set; }
        public string wnc_date_format { get; set; }
        public bool wnc_mendatory { get; set; }
        public bool wnc_encrypt { get; set; }
        public string wnc_status { get; set; }
        public bool wnc_active { get; set; }
        public string wnc_createuser { get; set; }
        public DateTime? wnc_createdate { get; set; }
        public string wnc_updateuser { get; set; }
        public DateTime? wnc_updatedate { get; set; }
    }
}
