using ALISS.Master.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.Master.NoticeMessage.Library
{
    public interface IWHONETService
    {
        List<TCWHONET_AntibioticsDTO> Get_TCWHONET_Antibiotics_Acitve_List();
    }
}
