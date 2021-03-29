using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.MasterManagement.Library.Models
{
    public class TCExpertRule
    {
        public int exr_id { get; set; }
        public string exr_mst_code { get; set; }
        public string exr_code { get; set; }
        public string RuleSet { get; set; }
        public string EXPERTTYPE { get; set; }
        public string CATEGORY { get; set; }
        public string ACTIVE { get; set; }
        public string PRIORITY { get; set; }
        public string ORG_GROUP { get; set; }
        public string ORGANISMS { get; set; }
        public string ORG_CODES { get; set; }
        public string DESCRIPTION { get; set; }
        public string ABX_RESULT { get; set; }
        public string OTH_RESULT { get; set; }
        public string INTERP { get; set; }
        public string MICROBIOL { get; set; }
        public string CLINICAL { get; set; }
        public string QUALITY { get; set; }
        public string QUAL_TYPE { get; set; }
        public string IMP_SPECIE { get; set; }
        public string IMP_RESIST { get; set; }
        public string SAVE_ISOL { get; set; }
        public string SEND_REF { get; set; }
        public string INF_CONT { get; set; }
        public string RX_COMMENT { get; set; }
        public string exr_status { get; set; }
        public bool exr_active { get; set; }
        public string exr_createuser { get; set; }
        public DateTime? exr_createdate { get; set; }
        public string exr_updateuser { get; set; }
        public DateTime? exr_updatedate { get; set; }
    }
}
