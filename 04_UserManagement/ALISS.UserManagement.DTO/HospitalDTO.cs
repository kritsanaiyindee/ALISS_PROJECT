using System;
using System.ComponentModel.DataAnnotations;

namespace ALISS.UserManagement.DTO
{
    public class HospitalDTO
    {
        public int hos_id { get; set; }
        [Required(ErrorMessage = "* Please choose hospital")]
        public string hos_code { get; set; }
        public string hos_name { get; set; }
        [Required(ErrorMessage = "* Please choose area health")]
        public string hos_arh_code { get; set; }
        public string arh_name { get; set; }
        [Required(ErrorMessage = "* Please choose province")]
        public string hos_prv_code { get; set; }
        public string prv_name { get; set; }
        public string hos_status { get; set; }
        public bool hos_active { get; set; }
        public string hos_createuser { get; set; }
        public DateTime? hos_createdate { get; set; }
        public string hos_updateuser { get; set; }
        public DateTime? hos_updatedate { get; set; }
        public string hos_createdate_str { 
            get { 
                return (hos_createdate != null) ? hos_createdate.Value.ToString("dd/MM/yyyy") : ""; 
            } 
        }
        public string hos_updatedate_str
        {
            get
            {
                return (hos_updatedate != null) ? hos_updatedate.Value.ToString("dd/MM/yyyy") : "";
            }
        }
    }

    public class HospitalLabDTO
    {
        public int lab_id { get; set; }
        public string lab_hos_code { get; set; }
        [Required(ErrorMessage = "* Please enter lab code")]
        public string lab_code { get; set; }
        [Required(ErrorMessage = "* Please enter lab name")]
        public string lab_name { get; set; }
        public string lab_type { get; set; }
        public string lab_prg_type { get; set; }
        public string lab_status { get; set; }
        public bool lab_active { get; set; }
        public string lab_createuser { get; set; }
        public DateTime? lab_createdate { get; set; }
        public string lab_updateuser { get; set; }
        public DateTime? lab_updatedate { get; set; }
    }

    public class HospitalSearchDTO
    {
        public string hos_id { get; set; }
        public string hos_code { get; set; }
        public string hos_name { get; set; }
        public string hos_arh_code { get; set; }
        public string arh_name { get; set; }
        public string hos_prv_code { get; set; }
        public string prv_name { get; set; }
        public string hos_status { get; set; }
        public bool hos_active { get; set; }
    }
}
