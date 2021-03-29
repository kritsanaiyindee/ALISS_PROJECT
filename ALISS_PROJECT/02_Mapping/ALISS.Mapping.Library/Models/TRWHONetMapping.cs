using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.Mapping.Library.Models
{
    public class TRWHONetMapping
    {
        public Guid wnm_id { get; set; }
        public char wnm_status { get; set; }
        public bool? wnm_flagdelete { get; set; }
        public Guid wnm_mappingid { get; set; }
        public string wnm_whonetfield { get; set; }
        public string wnm_originalfield { get; set; }
        public string wnm_type { get; set; }
        public int wnm_fieldlength { get; set; }
        public string wnm_fieldformat { get; set; }
        public bool? wnm_encrypt { get; set; }
        public bool? wnm_mandatory { get; set; }
        public string wnm_antibioticcolumn { get; set; }
        public string wnm_antibiotic { get; set; }
        public string wnm_createuser { get; set; }
        public DateTime? wnm_createdate { get; set; }
        public string wnm_updateuser { get; set; }
        public DateTime? wnm_updatedate { get; set; }
    }
}
