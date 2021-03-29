using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ALISS.MasterManagement.DTO
{
    public class ExpertRuleDTO
    {
        public int exr_id { get; set; }
        public string exr_mst_code { get; set; }
        [Required(ErrorMessage = "* Please enter expert rule code")]
        [RegularExpression("([a-zA-Z0-9_'-]+)", ErrorMessage = "Wrong format.")]
        public string exr_code { get; set; }
        public string exr_mst_RuleSet { get; set; }
        public string exr_mst_EXPERTTYPE { get; set; }
        public string exr_mst_CATEGORY { get; set; }
        public string exr_mst_ACTIVE { get; set; }
        public string exr_mst_PRIORITY { get; set; }
        public string exr_mst_ORG_GROUP { get; set; }
        public string exr_mst_ORGANISMS { get; set; }
        public string exr_mst_ORG_CODES { get; set; }
        public string exr_mst_DESCRIPTION { get; set; }
        public string exr_mst_ABX_RESULT { get; set; }
        public string exr_mst_OTH_RESULT { get; set; }
        public string exr_mst_INTERP { get; set; }
        public string exr_mst_MICROBIOL { get; set; }
        public string exr_mst_CLINICAL { get; set; }
        public string exr_mst_QUALITY { get; set; }
        public string exr_mst_QUAL_TYPE { get; set; }
        public string exr_mst_IMP_SPECIE { get; set; }
        public string exr_mst_IMP_RESIST { get; set; }
        public string exr_mst_SAVE_ISOL { get; set; }
        public string exr_mst_SEND_REF { get; set; }
        public string exr_mst_INF_CONT { get; set; }
        public string exr_mst_RX_COMMENT { get; set; }
        public string exr_status { get; set; }
        public bool exr_active { get; set; }
        public string exr_createuser { get; set; }
        public DateTime? exr_createdate { get; set; }
        public string exr_updateuser { get; set; }
        public DateTime? exr_updatedate { get; set; }
    }
}
