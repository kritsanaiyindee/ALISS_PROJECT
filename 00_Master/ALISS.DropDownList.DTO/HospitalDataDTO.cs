using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.DropDownList.DTO
{
    public class HospitalDataDTO
    {
        public string hos_id { get; set; }
        public string arh_code { get; set; }
        public string arh_name { get; set; }
        public string prv_code { get; set; }
        public string prv_name { get; set; }
        public string hos_code { get; set; }
        public string hos_name { get; set; }
    }


    public class HospitalLabDataDTO
    {
        public int hos_id { get; set; }
        public string arh_code { get; set; }
        public string arh_name { get; set; }
        public string prv_code { get; set; }
        public string prv_name { get; set; }
        public string hos_code { get; set; }
        public string hos_name { get; set; }
        public string lab_code { get; set; }
        public string lab_name { get; set; }
    }
}
