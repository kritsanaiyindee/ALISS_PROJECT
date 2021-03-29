using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.ANTIBIOGRAM.DTO
{
    public class AntibiogramTemplateDTO
    {
    }

    public class AntibiogramAreaHealthTemplateDTO
    {
        public string arh_code { get; set; }
        public string arh_name { get; set; }
        public string spc_code { get; set; }
        public string spc_name { get; set; }
        public string file_name { get; set; }
        public string file_path { get; set; }

    }

    public class AntibiogramHospitalTemplateDTO
    {
        public string hos_code { get; set; }
        public string hos_name { get; set; }
        public string prv_code { get; set; }
        public string prv_name { get; set; }
        public string arh_code { get; set; }
        public string arh_name { get; set; }
        public string spc_code { get; set; }
        public string spc_name { get; set; }
        public string file_name { get; set; }
        public string file_path { get; set; }
       
    }

    public class AntibiogramNationTemplateDTO
    {
        public string spc_code { get; set; }
        public string spc_name { get; set; }
        public string file_name { get; set; }
        public string file_path { get; set; }
       
    }
  
    public class RPAntibiogramSurveilAntibioticDTO
    {
        public int id { get; set; }
        public string ant_code { get; set; }
        public string ant_name { get; set; }
        public string ant_potency { get; set; }
        public string ant_class { get; set; }
        public string ant_subclass { get; set; }
        public string spc_code { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public bool active_flag { get; set; }
        public string remarks { get; set; }
        public string createuser { get; set; }
        public DateTime? createdate { get; set; }
        public string updateuser { get; set; }
        public DateTime? updatedate { get; set; }
    }

    public class RPAntibiogramSurveilOrganismDTO
    {
        public int id { get; set; }
        public string org_code { get; set; }
        public string org_name { get; set; }
        public string org_gram { get; set; }
        public string org_genus { get; set; }
        public string spc_code { get; set; }
        public int? sub_group_id { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public bool active_flag { get; set; }
        public string remarks { get; set; }
        public string createuser { get; set; }
        public DateTime? createdate { get; set; }
        public string updateuser { get; set; }
        public DateTime? updatedate { get; set; }
    }
}
