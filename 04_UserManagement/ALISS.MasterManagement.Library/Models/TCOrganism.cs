using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.MasterManagement.Library.Models
{
    public class TCOrganism
    {
        public int org_id { get; set; }
        public string org_mst_code { get; set; }
        public string org_code { get; set; }
        public string ORG { get; set; }
        public string GRAM { get; set; }
        public string ORGANISM { get; set; }
        public string ORG_CLEAN { get; set; }
        public string STATUS { get; set; }
        public string COMMON { get; set; }
        public string ORG_GROUP { get; set; }
        public string SUB_GROUP { get; set; }
        public string GENUS { get; set; }
        public string GENUS_CODE { get; set; }
        public string SCT_CODE { get; set; }
        public string SCT_TEXT { get; set; }
        public string org_status { get; set; }
        public bool org_active { get; set; }
        public string org_createuser { get; set; }
        public DateTime? org_createdate { get; set; }
        public string org_updateuser { get; set; }
        public DateTime? org_updatedate { get; set; }
    }
}
