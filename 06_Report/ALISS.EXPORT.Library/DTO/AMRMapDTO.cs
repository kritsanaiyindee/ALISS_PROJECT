using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALISS.EXPORT.Library.DTO
{
    public class AMRMapDTO
    {
    }

    public class AMRSearchMapDTO
    {
        public DateTime? month_start { get; set; }
        public DateTime? month_end { get; set; }
        public string month_start_str
        {
            get
            {
                return (month_start != null) ? month_start.Value.ToString("yyyy/MM/dd", new CultureInfo("en-US")) : null;
            }
        }
        public string month_end_str
        {
            get
            {
                return (month_end != null) ? month_end.Value.ToString("yyyy/MM/dd", new CultureInfo("en-US")) : null;
            }
        }
    }
}
