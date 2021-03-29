using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.Mapping.Library.Models
{
    public class TRMapping
    {
        public Guid mp_id { get; set; }
        public string mp_mst_code { get; set; }
        public char mp_status { get; set; }
        public decimal mp_version { get; set; }
        public bool? mp_flagdelete { get; set; }
        public string mp_hos_code { get; set; }
        public string mp_lab { get; set; }
        public string mp_program { get; set; }
        public string mp_filetype { get; set; }
        public bool? mp_AntibioticIsolateOneRec { get; set; }
        public bool? mp_firstlineisheader { get; set; }
        public string mp_dateformat { get; set; }
        public DateTime? mp_startdate { get; set; }
        public DateTime? mp_enddate { get; set; }
        public string mp_createuser { get; set; }
        public DateTime? mp_createdate { get; set; }
        public string mp_approveduser { get; set; }
        public DateTime? mp_approveddate { get; set; }
        public string mp_updateuser { get; set; }
        public DateTime? mp_updatedate { get; set; }
    }
}
