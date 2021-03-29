using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.LoginManagement.Library.Models
{
    public class TCUserLogin
    {
        public int usr_id { get; set; }
        public string usr_username { get; set; }
        public string usr_firstname { get; set; }
        public string usr_lastname { get; set; }
        public string usr_email { get; set; }
        public bool usr_emailConfirmed { get; set; }
        public string usr_password { get; set; }
        public string usr_passwordHash { get; set; }
        public string usr_securityStamp { get; set; }
        public string usr_phoneNumber { get; set; }
        public bool usr_phoneNumberConfirmed { get; set; }
        public bool usr_twoFactorEnabled { get; set; }
        public DateTime? usr_lockoutEndDateUtc { get; set; }
        public bool usr_lockoutEnabled { get; set; }
        public int? usr_accessFailedCount { get; set; }
        public string usr_status { get; set; }
        public bool usr_active { get; set; }
        public string usr_createuser { get; set; }
        public DateTime? usr_createdate { get; set; }
        public string usr_updateuser { get; set; }
        public DateTime? usr_updatedate { get; set; }
    }
}
