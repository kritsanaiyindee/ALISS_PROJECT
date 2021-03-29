using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.HISUpload.Library.Models
{
    public class TRHISFileUpload
    {
        public int hfu_id { get; set; }
        public string hfu_template_id { get; set; }
        public int hfu_version { get; set; }
        public string hfu_hos_code { get; set; }
        public string hfu_lab { get; set; }
        public string hfu_file_name { get; set; }
        public string hfu_file_path { get; set; }
        public string hfu_file_type { get; set; }
        public int hfu_total_records { get; set; }
        public int hfu_error_records { get; set; }
        public int hfu_duplicate_records { get; set; }
        public int hfu_matching_records { get; set; }
        public char hfu_status { get; set; }
        public bool? hfu_delete_flag { get; set; }
        public string hfu_remarks { get; set; }
        public string hfu_createuser { get; set; }
        public DateTime? hfu_createdate { get; set; }
        public string hfu_approveduser { get; set; }
        public DateTime? hfu_approveddate { get; set; }
        public string hfu_updateuser { get; set; }
        public DateTime? hfu_updatedate { get; set; }
    }
}
