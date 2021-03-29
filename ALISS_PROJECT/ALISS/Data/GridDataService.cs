using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALISS.Data
{
    public class GridDataService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Task<GridData[]> GetData1Async()
        {
            var rng = new Random();
            var objReturn = new List<GridData>(){
                new GridData { Column_01 = "Data0101", Column_02 = "Data0102", Column_03 = "Data0103", Column_04 = "Data0104", Column_05 = "Data0105" },
                new GridData { Column_01 = "Data0201", Column_02 = "Data0202", Column_03 = "Data0203", Column_04 = "Data0204", Column_05 = "Data0205" },
                new GridData { Column_01 = "Data0301", Column_02 = "Data0302", Column_03 = "Data0303", Column_04 = "Data0304", Column_05 = "Data0305" },
                new GridData { Column_01 = "Data0401", Column_02 = "Data0402", Column_03 = "Data0403", Column_04 = "Data0404", Column_05 = "Data0405" },
                new GridData { Column_01 = "Data0501", Column_02 = "Data0502", Column_03 = "Data0503", Column_04 = "Data0504", Column_05 = "Data0505" }
            };
            return Task.FromResult(objReturn.ToArray());
        }

        public Task<GridData[]> GetHistory1Async()
        {
            var rng = new Random();
            var objReturn = new List<GridData>(){
                new GridData { Column_01 = "(A0000)Super admin", Column_02 = "Active role", Column_03 = "Lorem", Column_04 = "00/00/00 00:00", Column_05 = "" },
                new GridData { Column_01 = "(A0000)Super admin", Column_02 = "Inactive role", Column_03 = "Lorem", Column_04 = "00/00/00 00:00", Column_05 = "" },
            };
            return Task.FromResult(objReturn.ToArray());
        }
    }
}
