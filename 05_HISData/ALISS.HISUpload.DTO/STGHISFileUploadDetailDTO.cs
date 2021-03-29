using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.HISUpload.DTO
{
    public class STGHISFileUploadDetailDTO
    {
        public int hud_id { get; set; }
        public int hud_huh_id { get; set; }
        public int hud_seq_no { get; set; }
        public string hud_field_name { get; set; }
        public string hud_field_value { get; set; }
        public char hud_status { get; set; }
        public string hud_remarks { get; set; }
        public string hud_createuser { get; set; }
        public DateTime? hud_createdate { get; set; }
        public string hud_updateuser { get; set; }
        public DateTime? hud_updatedate { get; set; }
    }
}
