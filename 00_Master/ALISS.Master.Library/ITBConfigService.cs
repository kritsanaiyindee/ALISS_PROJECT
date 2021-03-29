using ALISS.Master.DTO;
using System.Collections.Generic;

namespace ALISS.Master.Library
{
    public interface ITBConfigService
    {
        List<TBConfigDTO> GetTBConfig(TBConfigDTO searchModel);
    }
}