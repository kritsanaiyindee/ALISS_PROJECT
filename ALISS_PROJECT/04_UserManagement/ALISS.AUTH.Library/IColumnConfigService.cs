using ALISS.AUTH.DTO;
using System.Collections.Generic;

namespace ALISS.AUTH.Library
{
    public interface IColumnConfigService
    {
        List<ColumnConfigDTO> GetList();
        List<ColumnConfigDTO> GetListWithParam(string param);
        List<ColumnConfigDTO> GetListWithModel(ColumnConfigSearchDTO searchModel);
        ColumnConfigDTO GetData(string tbc_id);
        ColumnConfigDTO SaveData(ColumnConfigDTO model);
    }
}