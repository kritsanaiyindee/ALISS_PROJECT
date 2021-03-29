using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ALISS.HISUpload.DTO
{
    public class HISUploadDataDTO
    {
        public int hfu_id { get; set; }
        public string hfu_template_id { get; set; }
        public int hfu_version { get; set; }
        public string hfu_hos_code { get; set; }
        public string hfu_lab { get; set; }
        public string hfu_file_name { get; set; }
        public string hfu_file_path { get; set; }
        public string hfu_file_type { get; set; }
        public int hfu_total_records { get; set; }
        public int hfu_error_records { get; set; }
        public int hfu_duplicate_records { get; set; }
        public int hfu_matching_records { get; set; }
        public char hfu_status { get; set; }
        public bool? hfu_delete_flag { get; set; }
        public string hfu_remarks { get; set; }
        public string hfu_createuser { get; set; }
        public DateTime? hfu_createdate { get; set; }
        public string hfu_approveduser { get; set; }
        public DateTime? hfu_approveddate { get; set; }
        public string hfu_updateuser { get; set; }
        public DateTime? hfu_updatedate { get; set; }
        public string hos_name { get; set; }
        public string prv_name { get; set; }
        public string arh_name { get; set; }
        public string lab_name { get; set; }
        public string hfu_createdate_str
        {
            get
            {
                return (hfu_createdate != null) ? hfu_createdate.Value.ToString("dd/MM/yyyy") : "";
            }
        }
        public string hfu_status_str
        {
            get
            {
                string objReturn = "";

                if (hfu_status == 'N') objReturn = "New";
                else if (hfu_status == 'R') objReturn = "Waiting Re-Process";
                else if (hfu_status == 'E') objReturn = "Error";
                else if (hfu_status == 'P' || hfu_status == 'M') objReturn = "Processing";
                else if (hfu_status == 'F') objReturn = "Finished";
                else if (hfu_status == 'D') objReturn = "Cancel";
                else if (hfu_status == 'A') objReturn = "Approved";

                return objReturn;
            }
        }
    }
    public class HISUploadDataSearchDTO
    {
        public string hfu_hos_code { get; set; }
        public string hfu_prv_code { get; set; }
        public string hfu_arh_code { get; set; }
        public int hfu_id { get; set; }
        public string hfu_lab { get; set; }
        public string hfu_file_type { get; set; }
        public DateTime? hfu_upload_date_from { get; set; }
        public DateTime? hfu_upload_date_to { get; set; }
        public string hfu_upload_date_from_str
        {
            get
            {
                return (hfu_upload_date_from != null) ? hfu_upload_date_from.Value.ToString("yyyy/MM/dd", new CultureInfo("en-US")) : null;
            }
        }
        public string hfu_upload_date_to_str
        {
            get
            {
                return (hfu_upload_date_to != null) ? hfu_upload_date_to.Value.ToString("yyyy/MM/dd", new CultureInfo("en-US")) : null;
            }
        }
    }
}
