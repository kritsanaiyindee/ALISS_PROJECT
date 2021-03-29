using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.LabFileUpload.Batch.Models
{
    public class TRLabFileErrorDetail
    {
        public Guid fed_id { get; set; }
        public char fed_status { get; set; }
        public string fed_localvalue { get; set; }
        public string fed_whonetvalue { get; set; }
        public Guid fed_feh_id { get; set; }
        public string fed_createuser { get; set; }
        public DateTime? fed_createdate { get; set; }
        public string fed_updateuser { get; set; }
        public DateTime? fed_updatedate { get; set; }

    }
}
