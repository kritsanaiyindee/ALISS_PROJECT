using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.LabFileUpload.DTO
{
    public class LabFileErrorHeaderListDTO
    {
        public Guid feh_id { get; set; }
        public string feh_type { get; set; }
        public string feh_field { get; set; }
        public string feh_message { get; set; }
        public int feh_errorrecord { get; set; }
    }
}
