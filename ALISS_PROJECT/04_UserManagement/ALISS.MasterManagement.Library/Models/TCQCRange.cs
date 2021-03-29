using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.MasterManagement.Library.Models
{
    public class TCQCRange
    {
        public int qcr_id { get; set; }
        public string qcr_mst_code { get; set; }
        public string qcr_code { get; set; }
        public string GUIDELINE { get; set; }
        public string STRAIN { get; set; }
        public string REF_TABLE { get; set; }
        public string ORGANISM { get; set; }
        public string ANTIBIOTIC { get; set; }
        public string ABX_TEST { get; set; }
        public string WHON5_CODE { get; set; }
        public string METHOD { get; set; }
        public string GUIDELINES { get; set; }
        public string MEDIUM { get; set; }
        public string QC_RANGE { get; set; }
        public string qcr_status { get; set; }
        public bool qcr_active { get; set; }
        public string qcr_createuser { get; set; }
        public DateTime? qcr_createdate { get; set; }
        public string qcr_updateuser { get; set; }
        public DateTime? qcr_updatedate { get; set; }
    }
}
