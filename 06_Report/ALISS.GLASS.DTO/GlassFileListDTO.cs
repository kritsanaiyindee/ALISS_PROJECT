using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.GLASS.DTO
{
    public class GlassFileListDTO
    {
        public int year { get; set; }
        public string arh_code { get; set; }
        public string ris_file_name { get; set; }
        public string ris_file_path { get; set; }
        public string ris_file_type { get; set; }
        public string sample_file_name { get; set; }
        public string sample_file_path { get; set; }
        public string sample_file_type { get; set; }
        public string analyze_file_name { get; set; }
        public string analyze_file_path { get; set; }
        public string analyze_file_type { get; set; }
        public bool? who_flag { get; set; }
        public string createuser { get; set; }
        public DateTime? createdate { get; set; }
        public string updateuser { get; set; }
        public DateTime? updatedate { get; set; }
        public string createdate_str
        {
            get
            {
                return (createdate != null) ? createdate.Value.ToString("dd/MM/yyyy") : "";
            }
        }
        public string updatedate_str
        {
            get
            {
                return (updatedate != null) ? updatedate.Value.ToString("dd/MM/yyyy") : "";
            }
        }

    }

    public class GlassFileListNationSearchDTO
    {
        public string arh_code { get; set; }
        public int start_year { get; set; }
        public int end_year { get; set; }
    }

    public class GlassInfectOriginOverviewDTO
    {
        public string spc_code { get; set; }
        public string spc_name { get; set; }
        public int spc_co_origin { get; set; }
        public int spc_hos_origin { get; set; }
        public int spc_unk_origin { get; set; }
        public int spc_total_origin { get; set; }
        public string org_code { get; set; }
        public string org_name { get; set; }
        public int org_co_origin { get; set; }
        public int org_hos_origin { get; set; }
        public int org_unk_origin { get; set; }
        public int org_total_origin { get; set; }
        public string lab_no { get; set; }
        public string arh_code { get; set; }
        public string arh_name { get; set; }
    }

    public class GlassPathogenNSDTO
    {
        public string arh_code { get; set; }
        public string arh_name { get; set; }
        public string spc_code { get; set; }
        public string spc_name { get; set; }
        public string org_code { get; set; }
        public string org_name { get; set; }
        public string anti_code { get; set; }
        public string anti_name { get; set; }
        public int total_drug_test { get; set; }
        public decimal percent_s { get; set; }
        public decimal percent_i { get; set; }
        public decimal percent_r { get; set; }
        public decimal percent_ns { get { return percent_i + percent_r; } }
    }

    public class GlassInfectSpecimenDTO
    {
        public string spc_code { get; set; }
        public string spc_name { get; set; }
        public int spc_co_origin { get; set; }
        public int spc_hos_origin { get; set; }
        public int spc_unk_origin { get; set; }
        public int spc_total_origin { get; set; }
        public string lab_no { get; set; }
        public string arh_code { get; set; }
        public string arh_name { get; set; }
    }

    public class GlassInfectPathAntiCombineDTO
    {
        public string spc_code { get; set; }
        public string spc_name { get; set; }
        public string org_code { get; set; }
        public string org_name { get; set; }
        public string anti_code { get; set; }
        public string anti_name { get; set; }
        public int freq_co_org { get; set; }
        public int freq_hos_org { get; set; }
        public int freq_unk_org { get; set; }
        public int freq_co_anti { get; set; }
        public int freq_hos_anti { get; set; }
        public int freq_unk_anti { get; set; }
        public string lab_no { get; set; }
        public string arh_code { get; set; }
        public string arh_name { get; set; }
        public int num_org { get; set; }
    }

    public class GlassParameterPathDTOss
    {
        public int prm_id { get; set; }
        public string prm_code_major { get; set; }
        public string prm_code_minor { get; set; }
        public string prm_value { get; set; }
        public string prm_desc1 { get; set; }
        public string prm_desc2 { get; set; }
        public string prm_desc3 { get; set; }
        public string prm_desc4 { get; set; }
        public string prm_status { get; set; }
        public bool prm_active { get; set; }
    }
}
