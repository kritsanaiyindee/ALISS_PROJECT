using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ALISS.AUTH.DTO
{
    public class ColumnConfigDTO
    {
        public int tbc_id { get; set; }
        [Required(ErrorMessage = "* Please enter menu code")]
        public string tbc_mnu_code { get; set; }
        [Required(ErrorMessage = "* Please enter column name")]
        public string tbc_column_name { get; set; }
        public string tbc_column_label { get; set; }
        public string tbc_column_placeholder { get; set; }
        public bool tbc_required_field { get; set; }
        public bool tbc_edit { get; set; }
        public string tbc_status { get; set; }
        public bool tbc_active { get; set; }
        public string tbc_createuser { get; set; }
        public string tbc_updateuser { get; set; }
    }

    public class ColumnConfigSearchDTO
    {
        public string sch_mnu_code { get; set; }
    }
}
