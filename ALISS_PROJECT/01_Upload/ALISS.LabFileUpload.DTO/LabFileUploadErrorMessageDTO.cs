using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.LabFileUpload.DTO
{
    public class LabFileUploadErrorMessageDTO
    {
        public char lfu_status { get; set; }
        public char lfu_Err_type { get; set; }
        public int lfu_Err_no { get; set; }
        public string lfu_Err_Column { get; set; }
        public string lfu_Err_Message { get; set; }
    }
}
