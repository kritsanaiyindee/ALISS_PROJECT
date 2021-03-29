using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.AUTH.Library.Models
{
    public class TCPasswordConfig
    {
        public int pwc_id { get; set; }
        public int pwc_user_min_char { get; set; }
        public int pwc_user_max_char { get; set; }
        public int pwc_min_char { get; set; }
        public int pwc_max_char { get; set; }
        public bool pwc_lowwer_letter { get; set; }
        public bool pwc_upper_letter { get; set; }
        public bool pwc_number { get; set; }
        public bool pwc_special_char { get; set; }
        public int pwc_max_invalid { get; set; }
        public int pwc_force_reset { get; set; }
        public int pwc_session_timeout { get; set; }
        public string pwc_default_password { get; set; }
        public string pwc_createuser { get; set; }
        public DateTime? pwc_createdate { get; set; }
        public string pwc_updateuser { get; set; }
        public DateTime? pwc_updatedate { get; set; }
    }
}
