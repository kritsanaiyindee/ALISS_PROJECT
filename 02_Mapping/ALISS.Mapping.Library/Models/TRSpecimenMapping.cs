using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.Mapping.Library.Models
{
    public class TRSpecimenMapping
    {
        public Guid spm_id { get; set; }
        public char spm_status { get; set; }
        public bool? spm_flagdelete { get; set; }
        public Guid spm_mappingid { get; set; }
        public string spm_whonetcode { get; set; }
        public string spm_localspecimencode { get; set; }
        public string spm_localspecimendesc { get; set; }        
        public string spm_createuser { get; set; }
        public DateTime? spm_createdate { get; set; }
        public string spm_updateuser { get; set; }
        public DateTime? spm_updatedate { get; set; }
    }
}
