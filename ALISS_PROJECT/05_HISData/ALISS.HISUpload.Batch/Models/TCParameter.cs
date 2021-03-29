using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.HISUpload.Batch.Models
{
    public class TCParameter
    {
        public int prm_id { get; set; }
        public string prm_code_major { get; set; }
        public string prm_code_minor { get; set; }
        public string prm_value { get; set; }
        public string prm_desc1 { get; set; }
        public string prm_desc2 { get; set; }
        public string prm_desc3 { get; set; }
        public string prm_desc4 { get; set; }
        public string prm_status { get; set; }
        public bool prm_active { get; set; }
    }
}
