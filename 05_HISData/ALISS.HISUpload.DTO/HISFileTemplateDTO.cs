using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.HISUpload.DTO
{
    public class HISFileTemplateDTO
    {
        public int hft_id { get; set; }
        public string hft_template_id { get; set; }
        public string hft_field1 { get; set; }
        public string hft_field2 { get; set; }
        public string hft_field3 { get; set; }
        public string hft_field4 { get; set; }
        public string hft_date_format { get; set; }
        public bool hft_active_flag { get; set; }
        public string hft_remarks { get; set; }
        public string hft_createuser { get; set; }
        public DateTime? hft_createdate { get; set; }
        public string hft_updateuser { get; set; }
        public DateTime? hft_updatedate { get; set; }
    }
}
