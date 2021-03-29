using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.Master.DTO
{
    public class TBConfigDTO
    {
        public int tbc_id { get; set; }
        public string tbc_mnu_code { get; set; }
        public string tbc_column_name { get; set; }
        public string tbc_column_label { get; set; }
        public string tbc_column_placeholder { get; set; }
        public bool tbc_required_field { get; set; }
        public bool tbc_edit { get; set; }

    }
}
