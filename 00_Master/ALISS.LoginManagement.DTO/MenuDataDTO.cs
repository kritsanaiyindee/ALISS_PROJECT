using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.LoginManagement.DTO
{
    public class MenuDataDTO
    {
        public int mnu_id { get; set; }
        public string mnu_code { get; set; }
        public int mnu_order { get; set; }
        public int mnu_order_sub { get; set; }
        public string mnu_name { get; set; }
        public string mnu_icon { get; set; }
        public string mnu_area { get; set; }
        public string mnu_controller { get; set; }
        public string mnu_page { get; set; }
        public string mnu_group
        {
            get
            {
                return mnu_code.Substring(0, 6);
            }
        }
        public string mnu_path
        {
            get
            {
                return (!string.IsNullOrEmpty(mnu_area) ? mnu_area + "/" : "") + (!string.IsNullOrEmpty(mnu_controller) ? mnu_controller + "/" : "") + (!string.IsNullOrEmpty(mnu_page) ? mnu_page + "/" : "");
            }
        }

        public string strSubMenuDataDTO { get; set; }
        public List<MenuDataDTO> SubMenuDataDTO { get; set; } = new List<MenuDataDTO>();
    }

}
