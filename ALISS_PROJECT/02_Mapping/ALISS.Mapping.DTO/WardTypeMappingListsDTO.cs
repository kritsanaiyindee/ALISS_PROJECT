using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.Mapping.DTO
{
    public class WardTypeMappingListsDTO
    {
        public Guid wdm_id { get; set; }
        public Guid wdm_mappingid { get; set; }
        public string wdm_wardtype { get; set; }
        public string wdm_warddesc { get; set; }
        public string wdm_localwardname { get; set; }

    }

    public class WardTypeMappingSearch
    {
        public Guid wdm_mappingid { get; set; }
        public string wdm_mst_code { get; set; }
    }
}
