using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ALISS.MasterManagement.DTO
{
    public class MasterTemplateDTO
    {
        public int mst_id { get; set; }
        [Required(ErrorMessage = "* Please enter master code")]
        [RegularExpression("([a-zA-Z0-9_-]+)", ErrorMessage = "Wrong format.")]
        public string mst_code { get; set; }
        [Required(ErrorMessage = "* Please enter master version")]
        [RegularExpression("([a-zA-Z0-9_.-]+)", ErrorMessage = "Wrong format.")]
        public string mst_version { get; set; }
        [Required(ErrorMessage = "* Please enter eff. date")]
        public DateTime? mst_date_from { get; set; }
        public DateTime? mst_date_to { get; set; }
        public string mst_status { get; set; }
        public bool mst_active { get; set; }
        public string mst_createuser { get; set; }
        public DateTime? mst_createdate { get; set; }
        public string mst_updateuser { get; set; }
        public DateTime? mst_updatedate { get; set; }
    }

    public class MasterTemplateSearchDTO
    {
        public int mst_id { get; set; }
        public string mst_code { get; set; }
        public string mst_version { get; set; }
        public DateTime? mst_date_from { get; set; }
        public DateTime? mst_date_to { get; set; }
    }

}
