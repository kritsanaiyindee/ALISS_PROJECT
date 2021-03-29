using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.Mapping.Library.Models
{
    public class TRWardTypeMapping
    {
        public Guid wdm_id { get; set; }
        public char wdm_status { get; set; }
        public bool? wdm_flagdelete { get; set; }
        public Guid wdm_mappingid { get; set; }
        public string wdm_wardtype { get; set; }
        public string wdm_warddesc { get; set; }
        public string wdm_localwardname { get; set; }
        public string wdm_createuser { get; set; }
        public DateTime? wdm_createdate { get; set; }
        public string wdm_updateuser { get; set; }
        public DateTime? wdm_updatedate { get; set; }
    }
}
