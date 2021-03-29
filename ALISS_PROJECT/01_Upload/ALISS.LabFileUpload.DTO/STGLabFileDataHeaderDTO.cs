using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.LabFileUpload.DTO
{
    public class STGLabFileDataHeaderDTO
    {
        public Guid ldh_id { get; set; }
        public char ldh_status { get; set; }
        public int ldh_sequence { get; set; }
        public bool? ldh_flagdelete { get; set; }
        public string ldh_hos_code { get; set; }
        public string ldh_lab { get; set; }
        public string ldh_labno { get; set; }
        public string ldh_date { get; set; }
        public DateTime? ldh_cdate { get; set; }
        public string ldh_organism { get; set; }
        public string ldh_specimen { get; set; }
        public Guid ldh_lfu_id { get; set; }
        public string ldh_createuser { get; set; }
        public DateTime? ldh_createdate { get; set; }
        public string ldh_updateuser { get; set; }
        public DateTime? ldh_updatedate { get; set; }


    }
}
