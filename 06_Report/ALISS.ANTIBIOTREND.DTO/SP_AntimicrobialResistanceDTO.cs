using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.ANTIBIOTREND.DTO
{
    public class SP_AntimicrobialResistanceDTO
    {
        public int year { get; set; }
        public string spc_code { get; set; }
        public string ward_type { get; set; }
        public string org_code { get; set; }
        public string org_name { get; set; }
        public string anti_code { get; set; }
        public string anti_name { get; set; }
        public double percent_s { get; set; }
        public double percent_r { get; set; }
        public double percent_i { get; set; }
    }

    public class SP_AntimicrobialResistanceSearchDTO
    {
        public string org_codes { get; set; }
        public string anti_codes { get; set; }
        public int start_year { get; set; }
        public int end_year { get; set; }
    }
}
