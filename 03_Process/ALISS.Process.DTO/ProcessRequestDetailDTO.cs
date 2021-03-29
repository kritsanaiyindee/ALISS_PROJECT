using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.Process.DTO
{
    public class ProcessRequestDetailDTO
    {
        public int pcd_id { get; set; }
        public string pcd_pcr_code { get; set; }
        public string pcd_hos_code { get; set; }
        public string pcd_hos_name { get; set; }
        public string pcd_lab_code { get; set; }
        public int? pcd_M01_qty { get; set; }
        public int? pcd_M02_qty { get; set; }
        public int? pcd_M03_qty { get; set; }
        public int? pcd_M04_qty { get; set; }
        public int? pcd_M05_qty { get; set; }
        public int? pcd_M06_qty { get; set; }
        public int? pcd_M07_qty { get; set; }
        public int? pcd_M08_qty { get; set; }
        public int? pcd_M09_qty { get; set; }
        public int? pcd_M10_qty { get; set; }
        public int? pcd_M11_qty { get; set; }
        public int? pcd_M12_qty { get; set; }
        public int? pcd_Total_qty { get; set; }
        public string pcd_status { get; set; }
        public bool pcd_active { get; set; }
        public string pcd_createuser { get; set; }
        public DateTime? pcd_createdate { get; set; }
        public string pcd_updateuser { get; set; }
        public DateTime? pcd_updatedate { get; set; }
    }

    public class ProcessRequestCheckDetailDTO
    {
        public int pcd_id { get; set; }
        public string pcd_pcr_code { get; set; }
        public string pcd_hos_code { get; set; }
        public string pcd_hos_name { get; set; }
        public string pcd_lab_code { get; set; }
        public int? pcd_M01_qty { get; set; }
        public int? pcd_M02_qty { get; set; }
        public int? pcd_M03_qty { get; set; }
        public int? pcd_M04_qty { get; set; }
        public int? pcd_M05_qty { get; set; }
        public int? pcd_M06_qty { get; set; }
        public int? pcd_M07_qty { get; set; }
        public int? pcd_M08_qty { get; set; }
        public int? pcd_M09_qty { get; set; }
        public int? pcd_M10_qty { get; set; }
        public int? pcd_M11_qty { get; set; }
        public int? pcd_M12_qty { get; set; }
        public int? pcd_Total_qty { get; set; }
        public int? pcd_M01_qty_new { get; set; }
        public int? pcd_M02_qty_new { get; set; }
        public int? pcd_M03_qty_new { get; set; }
        public int? pcd_M04_qty_new { get; set; }
        public int? pcd_M05_qty_new { get; set; }
        public int? pcd_M06_qty_new { get; set; }
        public int? pcd_M07_qty_new { get; set; }
        public int? pcd_M08_qty_new { get; set; }
        public int? pcd_M09_qty_new { get; set; }
        public int? pcd_M10_qty_new { get; set; }
        public int? pcd_M11_qty_new { get; set; }
        public int? pcd_M12_qty_new { get; set; }
        public int? pcd_Total_qty_new { get; set; }
        public string pcd_status { get; set; }
        public bool pcd_active { get; set; }
    }
}
