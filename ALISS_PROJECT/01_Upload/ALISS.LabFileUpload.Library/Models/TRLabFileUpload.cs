using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.LabFileUpload.Library.Models
{
    public class TRLabFileUpload
    {
        public Guid lfu_id { get; set; }
        public char lfu_status { get; set; }
        public int lfu_version { get; set; }
        public bool? lfu_flagdelete { get; set; }
        public Guid lfu_mp_id { get; set; }
        public decimal lfu_mp_version { get; set; }
        public string lfu_hos_code { get; set; }
        public string lfu_lab { get; set; }
        public string lfu_FileName { get; set; }
        public string lfu_Program { get; set; }
        public string lfu_Path { get; set; }
        public int lfu_DataYear { get; set; }
        public string lfu_FileType { get; set; }
        public int lfu_TotalRecord { get; set; }
        public int lfu_AerobicCulture { get; set; }
        public int lfu_ErrorRecord { get; set; }
        public DateTime? lfu_StartDatePeriod { get; set; }
        public DateTime? lfu_EndDatePeriod { get; set; }
        public string lfu_createuser { get; set; }
        public DateTime? lfu_createdate { get; set; }
        public string lfu_approveduser { get; set; }
        public DateTime? lfu_approveddate { get; set; }
        public string lfu_updateuser { get; set; }
        public DateTime? lfu_updatedate { get; set; }
    }
}
