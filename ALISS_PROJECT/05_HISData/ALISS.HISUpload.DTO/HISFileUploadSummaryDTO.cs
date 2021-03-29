using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.HISUpload.DTO
{
    public class HISFileUploadSummaryDTO
    {
        public int hus_id { get; set; }
        public int hus_hfu_id { get; set; }
        public string hus_error_fieldname { get; set; }
        public string hus_error_fielddescr { get; set; }
        public string hus_error_fieldrecord { get; set; }
        public bool hus_delete_flag { get; set; }
        public string hus_remarks { get; set; }
        public string hus_createuser { get; set; }
        public DateTime? hus_createdate { get; set; }
        public string hus_updateuser { get; set; }
        public DateTime? hus_updatedate { get; set; }
    }
}
