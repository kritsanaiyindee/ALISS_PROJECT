using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ALISS.MasterManagement.DTO
{
    public class OrganismDTO
    {
        public int org_id { get; set; }
        public string org_mst_code { get; set; }
        [Required(ErrorMessage = "* Please enter organism code")]
        [RegularExpression("([a-zA-Z0-9_-]+)", ErrorMessage = "Wrong format.")]
        public string org_code { get; set; }
        public string org_mst_ORG { get; set; }
        public string org_mst_GRAM { get; set; }
        [Required(ErrorMessage = "* Please enter organism name")]
        public string org_mst_ORGANISM { get; set; }
        public string org_mst_ORG_CLEAN { get; set; }
        public string org_mst_STATUS { get; set; }
        public string org_mst_COMMON { get; set; }
        public string org_mst_ORG_GROUP { get; set; }
        public string org_mst_SUB_GROUP { get; set; }
        public string org_mst_GENUS { get; set; }
        public string org_mst_GENUS_CODE { get; set; }
        public string org_mst_SCT_CODE { get; set; }
        public string org_mst_SCT_TEXT { get; set; }
        public string org_status { get; set; }
        public bool org_active { get; set; }
        public string org_createuser { get; set; }
        public DateTime? org_createdate { get; set; }
        public string org_updateuser { get; set; }
        public DateTime? org_updatedate { get; set; }
    }
}
