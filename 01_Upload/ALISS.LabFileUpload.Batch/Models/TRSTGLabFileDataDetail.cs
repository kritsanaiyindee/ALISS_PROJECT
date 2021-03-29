using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.LabFileUpload.Batch.Models
{
    public class TRSTGLabFileDataDetail
    {
        public Guid ldd_id { get; set; }
        public char ldd_status { get; set; }
        public string ldd_whonetfield { get; set; }
        public string ldd_originalfield { get; set; }
        public string ldd_originalvalue { get; set; }
        public string ldd_mappingvalue { get; set; }
        public Guid ldd_ldh_id { get; set; }
        public string ldd_createuser { get; set; }
        public DateTime? ldd_createdate { get; set; }
        public string ldd_updateuser { get; set; }
        public DateTime? ldd_updatedate { get; set; }

    }
}
