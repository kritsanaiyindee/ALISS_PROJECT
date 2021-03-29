using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ALISS.AUTH.DTO
{
    public class RoleDTO
    {
        public int rol_id { get; set; }
        [Required(ErrorMessage = "* Please enter role code")]
        [RegularExpression("([a-zA-Z0-9_-]+)", ErrorMessage = "Wrong format.")]
        public string rol_code { get; set; }
        [Required(ErrorMessage = "* Please enter role name")]
        public string rol_name { get; set; }
        public string rol_desc { get; set; }
        public string rol_status { get; set; }
        public bool rol_active { get; set; }
        public string rol_createuser { get; set; }
        public DateTime? rol_createdate { get; set; }
        public string rol_updateuser { get; set; }
        public DateTime? rol_updatedate { get; set; }

        //public List<RolePermissionDTO> RoleDetails { get; set; } = new List<RolePermissionDTO>();
    }

    public class RolePermissionDTO
    {
        public int mnu_id { get; set; }
        public string mnu_code { get; set; }
        public string mnu_name { get; set; }
        public string mnu_status { get; set; }
        public bool? mnu_active { get; set; }
        public int? rop_id { get; set; }
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
        public string rop_status { get; set; }
        public bool rop_active { get; set; }
        public string rop_createuser { get; set; }
        public DateTime? rop_createdate { get; set; }
        public string rop_updateuser { get; set; }
        public DateTime? rop_updatedate { get; set; }
    }

    public class RoleSearchDTO
    {
        public string sch_rol_code { get; set; }
        public string sch_rol_name { get; set; }
    }

}
