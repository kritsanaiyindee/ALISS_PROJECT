using System;
using System.Globalization;

namespace ALISS.ANTIBIOGRAM.DTO
{
    public class AntibiogramDataDTO
    {
        public string prv_code { get; set; }
        public string prv_name { get; set; }
        public string arh_code { get; set; }
        public string arh_name { get; set; }
        public string hos_code { get; set; }
        public string hos_name { get; set; }
        public string gram { get; set; }
        public string spc_code { get; set; }
        public string spc_name { get; set; }
        public int total_isolate { get; set; }
        public int total_org { get; set; }
        public string org_code { get; set; }
        public string org_name { get; set; }
        public string anti_name { get; set; }
        public int total_drug_test { get; set; }
        public decimal percent_s { get; set; }

    }

    public class AntiHospitalSearchDTO
    {
        public string hos_code { get; set; }
        public string prv_code { get; set; }
        public string arh_code { get; set; }
        public string gram { get; set; }
        public string spc_code { get; set; }
        public string org_code { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }

        public string start_date_str
        {
            get
            {
                return (start_date != null) ? start_date.Value.ToString("yyyy/MM/dd", new CultureInfo("en-US")) : null;
            }
        }
        public string end_date_str
        {
            get
            {
                return (end_date != null) ? end_date.Value.ToString("yyyy/MM/dd", new CultureInfo("en-US")) : null;
            }
        }
    }

    public class AntiAreaHealthSearchDTO
    {
        public string arh_code { get; set; }
        public string spc_code { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public string start_date_str
        {
            get
            {
                return (start_date != null) ? start_date.Value.ToString("yyyy/MM/dd", new CultureInfo("en-US")) : null;
            }
        }
        public string end_date_str
        {
            get
            {
                return (end_date != null) ? end_date.Value.ToString("yyyy/MM/dd", new CultureInfo("en-US")) : null;
            }
        }
    }

    public class AntiNationSearchDTO
    {      
        public string spc_code { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }

        public string start_date_str
        {
            get
            {
                return (start_date != null) ? start_date.Value.ToString("yyyy/MM/dd", new CultureInfo("en-US")) : null;
            }
        }
        public string end_date_str
        {
            get
            {
                return (end_date != null) ? end_date.Value.ToString("yyyy/MM/dd", new CultureInfo("en-US")) : null;
            }
        }
    }
}
