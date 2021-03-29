using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ALISS.LabFileUpload.DTO
{
    public class LabFileUploadDataDTO
    {
        public Guid lfu_id { get; set; }
        public char lfu_status { get; set; }
        public int lfu_version { get; set; }
        public bool? lfu_flagdelete { get; set; }
        public Guid lfu_mp_id { get; set; }
        public decimal lfu_mp_version { get; set; }
        public string lfu_hos_code { get; set; }
        public string lfu_lab { get; set; }
        public string lfu_FileName { get; set; }

        [Required(ErrorMessage = "Program is required")]
        public string lfu_Program { get; set; }
        public int lfu_DataYear { get; set; }
        public string lfu_Path { get; set; }

        [Required(ErrorMessage = "File Type is required")]
        public string lfu_FileType { get; set; }
        public int lfu_TotalRecord { get; set; }
        public int lfu_AerobicCulture { get; set; }
        public int lfu_ErrorRecord { get; set; }
        public DateTime? lfu_StartDatePeriod { get; set; }
        public DateTime? lfu_EndDatePeriod { get; set; }
        public string lfu_createuser { get; set; }
        public DateTime? lfu_createdate { get; set; } 
        public string lfu_approveduser { get; set; }
        public DateTime? lfu_approveddate { get; set; }
        public string lfu_updateuser { get; set; }
        public DateTime? lfu_updatedate { get; set; }

        public string lfu_StartDatePeriod_str
        {
            get
            {
                return (lfu_StartDatePeriod != null) ? lfu_StartDatePeriod.Value.ToString("dd/MM/yyyy") : "";
            }
        }
        public string lfu_EndDatePeriod_str
        {
            get
            {
                return (lfu_EndDatePeriod != null) ? lfu_EndDatePeriod.Value.ToString("dd/MM/yyyy") : "";
            }
        }

        public string lfu_DatePeriod
        {
            get
            {
                return (!string.IsNullOrEmpty(lfu_StartDatePeriod_str) ? lfu_StartDatePeriod_str + " - " : "") + (!string.IsNullOrEmpty(lfu_EndDatePeriod_str) ? lfu_EndDatePeriod_str : "") ;
            }
        }

        public string lfu_createdate_str
        {
            get
            {
                return (lfu_createdate != null) ? lfu_createdate.Value.ToString("dd/MM/yyyy") : "";
            }
        }

        public string lfu_status_str
        {
            get
            {
                string objReturn = "";

                if (lfu_status == 'N') objReturn = "New";
                else if (lfu_status == 'R') objReturn = "Waiting Re-Process";
                else if (lfu_status == 'E') objReturn = "Error";
                else if (lfu_status == 'P' || lfu_status == 'M') objReturn = "Processing";
                else if (lfu_status == 'F') objReturn = "Finished";
                else if (lfu_status == 'D') objReturn = "Cancel";
                else if (lfu_status == 'W') objReturn = "WHONET Processed";
                return objReturn;
            }
        }
    }

    public class LabFileUploadSearchDTO
    {
        public string lfu_Hos { get; set; }
        public string lfu_Province { get; set; }
        public string lfu_Area { get; set; }
        public Guid lfu_id { get; set; }
        public string lfu_lab { get; set; }
        public string lfu_program { get; set; }
        public string lfu_filetype { get; set; }

    }
}
