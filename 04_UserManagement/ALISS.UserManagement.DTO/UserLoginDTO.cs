using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ALISS.UserManagement.DTO
{
    public class UserLoginDTO
    {
        public int usr_id { get; set; }
        [Required(ErrorMessage ="* Please enter username")]
        [RegularExpression("([a-zA-Z0-9_.@-]+)", ErrorMessage = "Wrong format.")]
        public string usr_username { get; set; }
        [Required(ErrorMessage = "* Please enter first name")]
        public string usr_firstname { get; set; }
        [Required(ErrorMessage = "* Please enter last name")]
        public string usr_lastname { get; set; }
        [Required(ErrorMessage ="* Please enter email")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
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

    public class UserLoginSearchDTO
    {
        public int usr_id { get; set; }
        public string usr_username { get; set; }
        public string usr_firstname { get; set; }
        public string usr_lastname { get; set; }
        public string usr_email { get; set; }
        public bool usr_active { get; set; }
        public string usp_rol_code { get; set; }
        public string usp_arh_code { get; set; }
        public string usp_prv_code { get; set; }
        public string usp_hos_code { get; set; }
        public string usp_lab_code { get; set; }
    }

}
