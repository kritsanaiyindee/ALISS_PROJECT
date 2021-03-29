using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.AUTH.Library.Models
{
    public class TCRole
    {
        public int rol_id { get; set; }
        public string rol_code { get; set; }
        public string rol_name { get; set; }
        public string rol_desc { get; set; }
        public string rol_status { get; set; }
        public bool rol_active { get; set; }
        public string rol_createuser { get; set; }
        public DateTime? rol_createdate { get; set; }
        public string rol_updateuser { get; set; }
        public DateTime? rol_updatedate { get; set; }
    }
}
