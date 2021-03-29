using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.LabFileUpload.DTO
{
    public class LabFileSummaryHeaderListDTO
    {
        public Guid fsh_id { get; set; }
        public Guid fsh_lfu_id { get; set; }
        public string fsh_code { get; set; }
        public string fsh_desc { get; set; }
        public int fsh_total { get; set; }
        public ICollection<LabFileSummaryDetailListDTO> LabFileSummaryDetailLists { get; set; }
    }
}
