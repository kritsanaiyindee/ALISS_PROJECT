using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ALISS.MasterManagement.DTO
{
    public class SpecimenDTO
    {
        public int spc_id { get; set; }
        public string spc_mst_code { get; set; }
        [Required(ErrorMessage = "* Please enter specimen code")]
        [RegularExpression("([a-zA-Z0-9_-]+)", ErrorMessage = "Wrong format.")]
        public string spc_code { get; set; }
        [Required(ErrorMessage = "* Please enter specimen name")]
        public string spc_name { get; set; }
        public string spc_desc { get; set; }
        public string spc_status { get; set; }
        public bool spc_active { get; set; }
        public string spc_createuser { get; set; }
        public DateTime? spc_createdate { get; set; }
        public string spc_updateuser { get; set; }
        public DateTime? spc_updatedate { get; set; }
    }
}
