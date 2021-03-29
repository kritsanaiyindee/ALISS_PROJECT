using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ALISS.Mapping.DTO
{
    public class WHONetMappingDataDTO
    {
        public Guid wnm_id { get; set; }
        public char wnm_status{ get; set; }
        public bool? wnm_flagdelete { get; set; }
        public Guid wnm_mappingid { get; set; }

        [Required(ErrorMessage = "WHONet Field is required")]
        [StringLength(50, ErrorMessage = "WHONet Field is too long.")]
        public string wnm_whonetfield { get; set; }

        [Required(ErrorMessage = "Original Field is required")]
        [StringLength(50, ErrorMessage = "Original Field is too long.")]
        public string wnm_originalfield { get; set; }
        public string wnm_type { get; set; }
        public int wnm_fieldlength { get; set; }
        public string wnm_fieldformat { get; set; }
        public bool? wnm_encrypt { get; set; }
        public bool? wnm_mandatory { get; set; }
        public string wnm_antibioticcolumn { get; set; }
        public string wnm_antibiotic { get; set; }
        public string wnm_createuser { get; set; }
        public DateTime? wnm_createdate { get; set; }
        public string wnm_updateuser { get; set; }
        public DateTime? wnm_updatedate { get; set; }

    }
}
