using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.HISUpload.DTO
{
    public class STGHISFileUploadHeaderDTO
    {
        public int huh_id { get; set; }
        public int huh_template_id { get; set; }
        public int huh_seq_no { get; set; }
        public string huh_ref_no { get; set; }
        public string huh_lab_no { get; set; }
        public string huh_hn_no { get; set; }
        public DateTime? huh_date { get; set; }
        public bool huh_delete_flag { get; set; }
        public string huh_remarks { get; set; }
        public string huh_createuser { get; set; }
        public DateTime? huh_createdate { get; set; }
        public string huh_updateuser { get; set; }
        public DateTime? huh_updatedate { get; set; }
    }
}
