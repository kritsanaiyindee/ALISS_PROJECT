using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.MasterManagement.Library.Models
{
    public class TCMasterTemplate
    {
        public int mst_id { get; set; }
        public string mst_code { get; set; }
        public string mst_version { get; set; }
        public DateTime? mst_date_from { get; set; }
        public DateTime? mst_date_to { get; set; }
        public string mst_status { get; set; }
        public bool? mst_active { get; set; }
        public string mst_createuser { get; set; }
        public DateTime? mst_createdate { get; set; }
        public string mst_updateuser { get; set; }
        public DateTime? mst_updatedate { get; set; }
    }
}
