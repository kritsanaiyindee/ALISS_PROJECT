using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ALISS.ANTIBIOTREND.DTO
{
    public class NationHealthStrategyDTO
    {
        public DateTime? month_start { get; set; }
        public DateTime? month_end { get; set; }
        public string org_code { get; set; }
        public string org_name { get; set; }
        public string anti_code { get; set; }
        public string anti_name { get; set; }
        public string measure_type { get; set; }
        public string org_label_name { get; set; }
    }

    public class AMRStrategySearchDTO
    {
        public DateTime? month_start { get; set; }
        public DateTime? month_end { get; set; }

        public string month_start_str
        {
            get
            {
                return (month_start != null) ? month_start.Value.ToString("yyyy/MM/dd", new CultureInfo("en-US")) : null;
            }
        }
        public string month_end_str
        {
            get
            {
                return (month_end != null) ? month_end.Value.ToString("yyyy/MM/dd", new CultureInfo("en-US")) : null;
            }
        }
    }

    public class AntibiotrendAMRStrategyDTO
    {
        public DateTime? month_start { get; set; }
        public DateTime? month_end { get; set; }
        //public string hos_code { get; set; }
        public string hos_arh_code { get; set; }
        public string org_code { get; set; }
        public string org_name { get; set; }
        public string anti_code { get; set; }
        public string anti_name { get; set; }
        public string measure_type { get; set; }
        public string org_label_name { get; set; }
        //public decimal percent_s { get; set; }
        //public decimal percent_i { get; set; }
        //public decimal percent_r { get; set; }
        //public decimal percent_ns { get; set; }
        public decimal arh_01 { get; set; }
        public decimal arh_02 { get; set; }
        public decimal arh_03 { get; set; }
        public decimal arh_04 { get; set; }
        public decimal arh_05 { get; set; }
        public decimal arh_06 { get; set; }
        public decimal arh_07 { get; set; }
        public decimal arh_08 { get; set; }
        public decimal arh_09 { get; set; }
        public decimal arh_10 { get; set; }
        public decimal arh_11 { get; set; }
        public decimal arh_12 { get; set; }
        public decimal arh_13 { get; set; }
        public int rank_arh_01 { get; set; }
        public int rank_arh_02 { get; set; }
        public int rank_arh_03 { get; set; }
        public int rank_arh_04 { get; set; }
        public int rank_arh_05 { get; set; }
        public int rank_arh_06 { get; set; }
        public int rank_arh_07 { get; set; }
        public int rank_arh_08 { get; set; }
        public int rank_arh_09 { get; set; }
        public int rank_arh_10 { get; set; }
        public int rank_arh_11 { get; set; }
        public int rank_arh_12 { get; set; }
        public int rank_arh_13 { get; set; }
    }
   
}
