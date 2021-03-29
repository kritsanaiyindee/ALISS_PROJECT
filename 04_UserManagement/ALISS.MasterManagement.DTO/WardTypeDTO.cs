using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ALISS.MasterManagement.DTO
{
    public class WardTypeDTO
    {
        public int wrd_id { get; set; }
        public string wrd_mst_code { get; set; }
        [Required(ErrorMessage = "* Please enter ward type code")]
        [RegularExpression("([a-zA-Z0-9_-]+)", ErrorMessage = "Wrong format.")]
        public string wrd_code { get; set; }
        [Required(ErrorMessage = "* Please enter ward type name")]
        public string wrd_name { get; set; }
        public string wrd_desc { get; set; }
        public string wrd_status { get; set; }
        public bool wrd_active { get; set; }
        public string wrd_createuser { get; set; }
        public DateTime? wrd_createdate { get; set; }
        public string wrd_updateuser { get; set; }
        public DateTime? wrd_updatedate { get; set; }
    }
}
