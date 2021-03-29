using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ALISS.Mapping.DTO
{
    public class OrganismMappingDataDTO
    {
        public Guid ogm_id { get; set; }
        public char ogm_status { get; set; }
        public bool? ogm_flagdelete { get; set; }
        public Guid ogm_mappingid { get; set; }

        [Required(ErrorMessage = "WHONet Code is required")]
        [StringLength(50, ErrorMessage = "WHONet Code is too long.")]
        public string ogm_whonetcode { get; set; }

        [Required(ErrorMessage = "WHONet Description is required")]
        [StringLength(200, ErrorMessage = "WHONet Description is too long.")]
        public string ogm_whonetdesc { get; set; }

        [Required(ErrorMessage = "Local Code is required")]
        [StringLength(50, ErrorMessage = "Local Code is too long.")]
        public string ogm_localorganismcode { get; set; }

        [Required(ErrorMessage = "Local Description is required")]
        [StringLength(200, ErrorMessage = "Local Description is too long.")]
        public string ogm_localorganismdesc { get; set; }
        public string ogm_createuser { get; set; }
        public DateTime? ogm_createdate { get; set; }
        public string ogm_updateuser { get; set; }
        public DateTime? ogm_updatedate { get; set; }
    }
}
