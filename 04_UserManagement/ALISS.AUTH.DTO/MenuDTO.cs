using System;
using System.ComponentModel.DataAnnotations;

namespace ALISS.AUTH.DTO
{
    public class MenuDTO
    {
        public int mnu_id { get; set; }
        [Required(ErrorMessage = "* Please enter menu code")]
        [RegularExpression("([a-zA-Z0-9_-]+)", ErrorMessage = "Wrong format.")]
        public string mnu_code { get; set; }
        [Range(0, 99, ErrorMessage = "* Please enter number between {1} to {2}")]
        public int mnu_order { get; set; }
        [Range(0, 99, ErrorMessage = "* Please enter number between {1} to {2}")]
        public int mnu_order_sub { get; set; }
        [Required(ErrorMessage = "* Please enter menu name")]
        public string mnu_name { get; set; }
        public string mnu_icon { get; set; }
        public string mnu_area { get; set; }
        public string mnu_controller { get; set; }
        public string mnu_page { get; set; }
        public string mnu_status { get; set; }
        public bool mnu_active { get; set; }
        public string mnu_createuser { get; set; }
        public DateTime? mnu_createdate { get; set; }
        public string mnu_updateuser { get; set; }
        public DateTime? mnu_updatedate { get; set; }
        public string mnu_group { get
            {
                return mnu_code.Substring(0, 6);
            } }
        public string mnu_path
        {
            get
            {
                return (!string.IsNullOrEmpty(mnu_area) ? mnu_area + "/" : "") + (!string.IsNullOrEmpty(mnu_controller) ? mnu_controller + "/" : "") + (!string.IsNullOrEmpty(mnu_page) ? mnu_page + "/" : "");
            }
        }
        public string mnu_createdate_str
        {
            get
            { 
                return (mnu_createdate != null) ? mnu_createdate.Value.ToString("dd/MM/yyyy") : ""; 
            } 
        }
        public string mnu_updatedate_str
        {
            get
            {
                return (mnu_updatedate != null) ? mnu_updatedate.Value.ToString("dd/MM/yyyy") : "";
            }
        }
    }

    public class MenuSearchDTO
    {
        public string sch_mnu_code { get; set; }
        public string sch_mnu_name { get; set; }
    }
}
