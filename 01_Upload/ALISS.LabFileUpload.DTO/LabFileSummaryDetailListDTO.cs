using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.LabFileUpload.DTO
{
    public class LabFileSummaryDetailListDTO
    {
        public Guid fsh_id { get; set; }
        public Guid fsd_id { get; set; }
        public string fsd_organismcode { get; set; }
        public string fsd_organismdesc { get; set; }
        public int fsd_total { get; set; }
        public LabFileSummaryHeaderListDTO LabFileSummaryHeaderList { get; set; }

    }
}
