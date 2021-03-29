using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ALISS.MasterManagement.DTO
{
    public class QCRangeDTO
    {
        public int qcr_id { get; set; }
        public string qcr_mst_code { get; set; }
        [Required(ErrorMessage = "* Please enter qcRange code")]
        [RegularExpression("([a-zA-Z0-9_'-]+)", ErrorMessage = "Wrong format.")]
        public string qcr_code { get; set; }
        public string qcr_mst_GUIDELINE { get; set; }
        public string qcr_mst_STRAIN { get; set; }
        public string qcr_mst_REF_TABLE { get; set; }
        public string qcr_mst_ORGANISM { get; set; }
        public string qcr_mst_ANTIBIOTIC { get; set; }
        public string qcr_mst_ABX_TEST { get; set; }
        public string qcr_mst_WHON5_CODE { get; set; }
        public string qcr_mst_METHOD { get; set; }
        public string qcr_mst_GUIDELINES { get; set; }
        public string qcr_mst_MEDIUM { get; set; }
        public string qcr_mst_QC_RANGE { get; set; }
        public string qcr_status { get; set; }
        public bool qcr_active { get; set; }
        public string qcr_createuser { get; set; }
        public DateTime? qcr_createdate { get; set; }
        public string qcr_updateuser { get; set; }
        public DateTime? qcr_updatedate { get; set; }
    }
}
