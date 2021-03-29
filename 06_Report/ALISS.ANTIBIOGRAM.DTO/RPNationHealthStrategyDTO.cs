using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.ANTIBIOGRAM.DTO
{
    public class RPNationHealthStrategyDTO
    {
        public int id { get; set; }
        public int? year { get; set; }
        public string org_code { get; set; }
        public string anti_code { get; set; }
        public string created_by { get; set; }
        public DateTime? created_datetime { get; set; }

        public string created_datetime_str
        {
            get
            {
                return (created_datetime != null) ? created_datetime.Value.ToString("dd MMM yyyy") : "";
            }
        }
    }
}
