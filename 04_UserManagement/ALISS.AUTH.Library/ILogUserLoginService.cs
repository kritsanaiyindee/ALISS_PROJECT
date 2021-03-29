using ALISS.AUTH.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.AUTH.Library
{
    public interface ILogUserLoginService
    {
        List<LogUserLoginDTO> GetListWithModel(LogUserLoginDTO searchModel);
    }
}
