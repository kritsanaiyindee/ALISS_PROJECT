using ALISS.Master.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALISS.Data.D0_Master
{
    public class ConfigData
    {
        public List<TBConfigDTO> ConfigDTOList { get; set; } = new List<TBConfigDTO>();

        public bool Get_Edit_Status(string column_name)
        {
            return (ConfigDTOList.FirstOrDefault(x => x.tbc_column_name == column_name) ?? new TBConfigDTO()).tbc_edit;
        }

        public string Get_Label(string column_name)
        {
            return (ConfigDTOList.FirstOrDefault(x => x.tbc_column_name == column_name) ?? new TBConfigDTO() { tbc_column_label = "???" }).tbc_column_label;
        }

        public string Get_PlaceHolder(string column_name)
        {
            return (ConfigDTOList.FirstOrDefault(x => x.tbc_column_name == column_name) ?? new TBConfigDTO()).tbc_column_placeholder;
        }

        public TBConfigDTO Get_ConfigRow(string column_name)
        {
            return (ConfigDTOList.FirstOrDefault(x => x.tbc_column_name == column_name) ?? new TBConfigDTO());
        }
    }
}
