using ALISS.Master.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.Master.Library
{
    public interface ILogProcessService
    {
        List<LogProcessDTO> GetLogProcessListWithModel(LogProcessSearchDTO searchModel);
        List<LogProcessDTO> GetLogProcessAuthListWithModel(LogProcessSearchDTO searchModel);
    }
}
