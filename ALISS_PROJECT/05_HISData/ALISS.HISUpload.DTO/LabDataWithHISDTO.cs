using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ALISS.HISUpload.DTO
{
    public class LabDataWithHISDTO
    {
        public string hos_code { get; set; }
        public string lab_no { get; set; }
        public DateTime? spec_date { get; set; }
        public string hn_no { get; set; }
        public string ref_no { get; set; }
        public DateTime? admission_date { get; set; }
        public string cini { get; set; }

    }
    public class LabDataWithHISSearchDTO
    {
        public string hos_code { get; set; }
        public string prv_code { get; set; }
        public string arh_code { get; set; }
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
