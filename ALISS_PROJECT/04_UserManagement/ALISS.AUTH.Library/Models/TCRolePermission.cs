using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ALISS.AUTH.Library.Models
{
    public class TCRolePermission
    {
        [Key]
        public int rop_id { get; set; }
        public string rop_rol_code { get; set; }
        public string rop_mnu_code { get; set; }
        public bool rop_view { get; set; }
        public bool rop_create { get; set; }
        public bool rop_edit { get; set; }
        public bool rop_approve { get; set; }
        public bool rop_print { get; set; }
        public bool rop_reject { get; set; }
        public bool rop_cancel { get; set; }
        public bool rop_return { get; set; }
        public bool rop_complete { get; set; }
        public bool rop_implement { get; set; }
        public string rop_status{ get; set; }
        public bool rop_active { get; set; }
        public string rop_createuser { get; set; }
        public DateTime? rop_createdate { get; set; }
        public string rop_updateuser { get; set; }
        public DateTime? rop_updatedate { get; set; }
    }
}
