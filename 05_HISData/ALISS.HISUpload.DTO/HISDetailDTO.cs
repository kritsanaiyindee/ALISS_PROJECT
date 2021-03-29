using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.HISUpload.DTO
{
    public class HISDetailDTO
    {
        public int hud_id { get; set; }
        public int hud_huh_id { get; set; }
        public int hud_seq_no { get; set; }
        public string hud_field_name { get; set; }
        public string hud_field_value { get; set; }
        public char hud_status { get; set; }
        public string hud_remarks { get; set; }
        public string lab_no { get; set; }
        public DateTime? spec_date { get; set; }
        public string hn_no { get; set; }
        public string ref_no { get; set; }
    }
}
