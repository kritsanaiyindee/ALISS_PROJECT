using System;
using System.ComponentModel.DataAnnotations;

namespace ALISS.MasterManagement.DTO
{
    public class MasterHospitalDTO
    {
        public int hos_id { get; set; }
        [Required(ErrorMessage = "* Please enter hospital code")]
        [RegularExpression("([a-zA-Z0-9_-]+)", ErrorMessage = "Wrong format.")]
        public string hos_code { get; set; }
        public string hos_code_5 { get; set; }
        [Required(ErrorMessage = "* Please enter hospital name")]
        public string hos_name { get; set; }
        public string hos_type_code { get; set; }
        public string hos_type_name { get; set; }
        public string hos_ministry_code { get; set; }
        public string hos_ministry_name { get; set; }
        public string hos_department_code { get; set; }
        public string hos_department_name { get; set; }
        public string hos_status_code { get; set; }
        public string hos_status_name { get; set; }
        [Required(ErrorMessage = "* Please select area health")]
        public string hos_arh_code { get; set; }
        public string hos_arh_name { get; set; }
        [Required(ErrorMessage = "* Please select province")]
        public string hos_province_code { get; set; }
        public string hos_province_name { get; set; }
        public string hos_amphoe_code { get; set; }
        public string hos_amphoe_name { get; set; }
        public string hos_tambon_code { get; set; }
        public string hos_tambon_name { get; set; }
        public string hos_address { get; set; }
        public string hos_moo { get; set; }
        public string hos_zipcode { get; set; }
        public string hos_tel { get; set; }
        public string hos_fax { get; set; }
        public string hos_parent_code { get; set; }
        public string hos_level_code { get; set; }
        public string hos_level_name { get; set; }
        public string hos_service_type { get; set; }
        public string hos_desc { get; set; }
        public string hos_open_date { get; set; }
        public string hos_close_date { get; set; }
        public string hos_record_date { get; set; }
        public string hos_code_create_date { get; set; }
        public string hos_code_cancel_date { get; set; }
        public string hos_limit { get; set; }
        public string hos_bed { get; set; }
        public string hos_status { get; set; }
        public bool hos_active { get; set; }
        public string hos_createuser { get; set; }
        public DateTime? hos_createdate { get; set; }
        public string hos_updateuser { get; set; }
        public DateTime? hos_updatedate { get; set; }
    }

    public class MasterHospitalSearchDTO
    {
        public string hos_id { get; set; }
        public string hos_code { get; set; }
        public string hos_code_5 { get; set; }
        public string hos_name { get; set; }
        public string hos_type_code { get; set; }
        public string hos_type_name { get; set; }
        public string hos_ministry_code { get; set; }
        public string hos_ministry_name { get; set; }
        public string hos_department_code { get; set; }
        public string hos_department_name { get; set; }
        public string hos_status_code { get; set; }
        public string hos_status_name { get; set; }
        public string hos_arh_code { get; set; }
        public string hos_province_code { get; set; }
        public string hos_province_name { get; set; }
        public string hos_amphoe_code { get; set; }
        public string hos_amphoe_name { get; set; }
        public string hos_tambon_code { get; set; }
        public string hos_tambon_name { get; set; }
        public string hos_address { get; set; }
        public string hos_moo { get; set; }
        public string hos_zipcode { get; set; }
        public string hos_tel { get; set; }
        public string hos_fax { get; set; }
        public string hos_parent_code { get; set; }
        public string hos_level_code { get; set; }
        public string hos_level_name { get; set; }
        public string hos_service_type { get; set; }
        public string hos_desc { get; set; }
        public string hos_open_date { get; set; }
        public string hos_close_date { get; set; }
        public string hos_record_date { get; set; }
        public string hos_code_create_date { get; set; }
        public string hos_code_cancel_date { get; set; }
        public string hos_limit { get; set; }
        public string hos_bed { get; set; }
    }
}
