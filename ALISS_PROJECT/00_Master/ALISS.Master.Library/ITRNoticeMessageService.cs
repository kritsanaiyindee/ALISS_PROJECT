using ALISS.Master.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.Master.Library
{
    public interface ITRNoticeMessageService
    {
        List<TRNoticeMessageDTO> GetListWithModel(TRNoticeMessageDTO searchModel);
    }
}
