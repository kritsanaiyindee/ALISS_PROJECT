using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.Mapping.DTO
{
    public class WHONetMappingListsDTO
    {
        public Guid wnm_id { get; set; }
        public Guid wnm_mappingid { get; set; }
        public string wnm_whonetfield { get; set; }
        public string wnm_originalfield { get; set; }
        public string wnm_type { get; set; }
        public string wnm_fieldformat { get; set; }
        public int wnm_fieldlength { get; set; }
        public bool? wnm_encrypt { get; set; }
        public bool? wnm_mandatory { get; set; }
        public string wnm_antibioticcolumn { get; set; }
        public string wnm_antibiotic { get; set; }
    }

    public class WHONetMappingSearch
    {
        public Guid wnm_mappingid { get; set; }

        public string wnm_mst_code { get; set; }
    }
}
