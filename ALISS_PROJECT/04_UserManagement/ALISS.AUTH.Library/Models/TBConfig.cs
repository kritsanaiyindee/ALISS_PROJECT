using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.AUTH.Library.Models
{
    public class TBConfig
    {
        public int tbc_id { get; set; }
        public string tbc_mnu_code { get; set; }
        public string tbc_column_name { get; set; }
        public string tbc_column_label { get; set; }
        public string tbc_column_placeholder { get; set; }
        public bool tbc_required_field { get; set; }
        public bool tbc_edit { get; set; }
        public string tbc_status { get; set; }
        public bool tbc_active { get; set; }
        public string tbc_createuser { get; set; }
        public DateTime? tbc_createdate { get; set; }
        public string tbc_updateuser { get; set; }
        public DateTime? tbc_updatedate { get; set; }
    }
}
