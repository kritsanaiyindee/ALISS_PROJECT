using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.HISUpload.DTO
{
    public class LabDataWithHISMatchingDTO
    {
        public string hos_code { get; set; }
        public string lab_no { get; set; }
        public DateTime? spec_date { get; set; }
        public string hn_no { get; set; }
        public string ref_no { get; set; }
    }

    public class LabDataWithHISMatchingSearchDTO
    {
        public string hos_code { get; set; }
        public int hfu_id { get; set; }
    }
}
