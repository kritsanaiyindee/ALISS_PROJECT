using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ALISS.Mapping.DTO
{
    public class MappingDataDTO
    {
        public Guid mp_id { get; set; }
        public string mp_mst_code { get; set; }
        public char mp_status { get; set; }
        public decimal mp_version { get; set; }
        public bool? mp_flagdelete { get; set; }

        [Required(ErrorMessage = "ชื่่อโรงพยาบาล is required")]
        public string mp_hos_code { get; set; }
        [Required(ErrorMessage = "Lab Code is required")]
        [StringLength(10, ErrorMessage = "Lab Code is too long.")]
        public string mp_lab { get; set; }

        [Required(ErrorMessage = "Program is required")]
        [StringLength(50, ErrorMessage = "Program is too long.")]
        public string mp_program { get; set; }
        public string mp_filetype { get; set; }

        [Required(ErrorMessage = "The antibiotics of one isolate require how  many row of data? is required")]
        public bool? mp_AntibioticIsolateOneRec { get; set; }

        [Required(ErrorMessage = "Does the first row of the data file have the names of the data fields? is required")]
        public bool? mp_firstlineisheader { get; set; }

        [Required(ErrorMessage = "Date Format is required")]
        public string mp_dateformat { get; set; }

        [Required(ErrorMessage = "วันที่เริ่มใช้งาน is required")]
        public DateTime? mp_startdate { get; set; }
        public DateTime? mp_enddate { get; set; }
        public string mp_createuser { get; set; }
        public DateTime? mp_createdate { get; set; }
        public string mp_approveduser { get; set; }
        public DateTime? mp_approveddate { get; set; }
        public string mp_updateuser { get; set; }
        public DateTime? mp_updatedate { get; set; }
        public string mp_startdate_str
        {
            get
            {
                return (mp_startdate != null) ? mp_startdate.Value.ToString("dd/MM/yyyy") : "";
            }
        }
        public string mp_enddate_str
        {
            get
            {
                return (mp_enddate != null) ? mp_enddate.Value.ToString("dd/MM/yyyy") : "";
            }
        }
        public string mp_createdate_str
        {
            get
            {
                return (mp_createdate != null) ? mp_createdate.Value.ToString("dd/MM/yyyy") : "";
            }
        }
        public string mp_approveddate_str
        {
            get
            {
                return (mp_approveddate != null) ? mp_approveddate.Value.ToString("dd/MM/yyyy") : "";
            }
        }
        public string mp_updatedate_str
        {
            get
            {
                return (mp_updatedate != null) ? mp_updatedate.Value.ToString("dd/MM/yyyy") : "";
            }
        }

        public string mp_status_str
        {
            get
            {
                string objReturn = "";

                if (mp_status == 'N') objReturn = "New";
                else if (mp_status == 'E') objReturn = "Draft";
                else if (mp_status == 'A') objReturn = "Approved";


                return objReturn;
            }
        }

    }
}
