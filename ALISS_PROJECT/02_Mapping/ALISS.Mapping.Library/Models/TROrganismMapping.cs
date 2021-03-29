using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.Mapping.Library.Models
{
    public class TROrganismMapping
    {
        public Guid ogm_id { get; set; }
        public char ogm_status { get; set; }
        public bool? ogm_flagdelete { get; set; }
        public Guid ogm_mappingid { get; set; }
        public string ogm_whonetcode { get; set; }
        public string ogm_whonetdesc { get; set; }
        public string ogm_localorganismcode { get; set; }
        public string ogm_localorganismdesc { get; set; }
        public string ogm_createuser { get; set; }
        public DateTime? ogm_createdate { get; set; }
        public string ogm_updateuser { get; set; }
        public DateTime? ogm_updatedate { get; set; }

    }
}
