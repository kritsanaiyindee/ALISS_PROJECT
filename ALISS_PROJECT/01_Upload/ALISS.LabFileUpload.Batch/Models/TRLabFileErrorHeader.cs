using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.LabFileUpload.Batch.Models
{
    public class TRLabFileErrorHeader
    {
        public Guid feh_id { get; set; }
        public char feh_status { get; set; }
        public bool? feh_flagdelete { get; set; }
        public string feh_type { get; set; }
        public string feh_field { get; set; }
        public string feh_message { get; set; }
        public int feh_errorrecord { get; set; }
        public Guid feh_lfu_id { get; set; }
        public string feh_createuser { get; set; }
        public DateTime? feh_createdate { get; set; }
        public string feh_updateuser { get; set; }
        public DateTime? feh_updatedate { get; set; }
    }
}
