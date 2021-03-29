using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.UserManagement.Library.Models
{
    public class TCUserPasswordHistory
    {
        public int uph_id { get; set; }
        public string uph_username { get; set; }
        public string uph_password { get; set; }
        public string uph_status { get; set; }
        public bool uph_active { get; set; }
        public string uph_createuser { get; set; }
        public DateTime? uph_createdate { get; set; }
        public string uph_updateuser { get; set; }
        public DateTime? uph_updatedate { get; set; }
    }
}
