using System;

namespace ALISS.ANTIBIOGRAM.DTO
{
    public class RPYearlyIsolateListingRISDTO
    {
        public int id { get; set; }
        public DateTime? tran_date { get; set; }
        public int year { get; set; }
        public string spc_code { get; set; }
        public string ward_type { get; set; }
        public string org_code { get; set; }
        public string org_name { get; set; }
        public string createuser { get; set; }
        public DateTime? createdate { get; set; }
    }

    public class RPYearlyIsolateListingRISDetailDTO
    {
        public int id { get; set; }
        public int header_id { get; set; }
        public string anti_code { get; set; }
        public string anti_name { get; set; }
        public double percent_s { get; set; }
        public double percent_r { get; set; }
        public double percent_i { get; set; }
        public string createduser { get; set; }
        public DateTime? createdate { get; set; }

    }

}