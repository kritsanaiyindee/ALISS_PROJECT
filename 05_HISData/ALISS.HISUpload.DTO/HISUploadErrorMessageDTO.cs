using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.HISUpload.DTO
{
    public class HISUploadErrorMessageDTO
    {
        public char hfu_status { get; set; }
        public string hfu_Err_type { get; set; }
        public int hfu_Err_no { get; set; }
        public string hfu_Err_Column { get; set; }
        public string hfu_Err_Message { get; set; }
    }
}
