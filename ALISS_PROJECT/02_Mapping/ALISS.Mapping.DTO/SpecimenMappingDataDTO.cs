using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ALISS.Mapping.DTO
{
    public class SpecimenMappingDataDTO
    {
        public Guid spm_id { get; set; }
        public char spm_status { get; set; }
        public bool? spm_flagdelete { get; set; }
        public Guid spm_mappingid { get; set; }

        [Required (ErrorMessage = "WHONet Code is required")]
        [StringLength(50, ErrorMessage = "WHONet Code is too long.")]
        public string spm_whonetcode { get; set; }

        [Required(ErrorMessage = "Source is required")]
        [StringLength(50, ErrorMessage = "Source is too long.")]
        public string spm_localspecimencode { get; set; }

        [Required(ErrorMessage = "CSource is required")]
        [StringLength(200, ErrorMessage = "CSource is too long.")]
        public string spm_localspecimendesc { get; set; }
        public string spm_createuser { get; set; }
        public DateTime? spm_createdate { get; set; }
        public string spm_updateuser { get; set; }
        public DateTime? spm_updatedate { get; set; }
    }
}
