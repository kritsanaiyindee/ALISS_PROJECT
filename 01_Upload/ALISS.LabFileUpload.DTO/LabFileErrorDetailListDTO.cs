using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ALISS.LabFileUpload.DTO
{
    public class LabFileErrorDetailListDTO
    {
        public Guid fed_id { get; set; }
        public Guid feh_id { get; set; }
        public string feh_type { get; set; }
        public string feh_field { get; set; }
        public string feh_message { get; set; }
        public string fed_localvalue { get; set; }

    }
}
