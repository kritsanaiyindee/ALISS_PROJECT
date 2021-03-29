using ALISS.TC.WHONET_Antibiotics.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.TR.NoticeMessage
{
    public interface ITCWHONET_AntibioticsDAC
    {
        void Insert(TCWHONET_Antibiotics model);
        void Update(TCWHONET_Antibiotics model);
        void Inactive(TCWHONET_Antibiotics model);
        TCWHONET_Antibiotics GetData(TCWHONET_Antibiotics searchModel);
        List<TCWHONET_Antibiotics> GetList(TCWHONET_Antibiotics searchModel);
        List<TCWHONET_Antibiotics> GetList_Active();
    }
}
