using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.Master.DTO
{
    public class TCWHONET_AntibioticsDTO
    {
        public int who_ant_id { get; set; }
        public string who_ant_mst_code { get; set; }
        public string who_ant_source { get; set; }
        public string who_ant_code { get; set; }
        public string who_ant_name { get; set; }
        public string who_ant_type { get; set; }
        public string who_ant_lab { get; set; }
        public string who_ant_size { get; set; }
        public string who_ant_S { get; set; }
        public string who_ant_I { get; set; }
        public string who_ant_R { get; set; }
        public string who_ant_status { get; set; }
        public bool who_ant_active { get; set; }
        public string who_ant_createuser { get; set; }
        public DateTime? who_ant_createdate { get; set; }
        public string who_ant_updateuser { get; set; }
        public DateTime? who_ant_updatedate { get; set; }
    }
}
