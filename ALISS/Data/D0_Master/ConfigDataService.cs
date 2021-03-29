using ALISS.Data.Client;
using ALISS.Master.DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALISS.Data.D0_Master
{
    public class ConfigDataService
    {
        private IConfiguration Configuration { get; }

        private ApiHelper _apiHelper;

        public ConfigDataService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }

        public async Task<List<TBConfigDTO>> Get_TBConfig_DataList_Async(TBConfigDTO param)
        {
            List<TBConfigDTO> objList = new List<TBConfigDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<TBConfigDTO, TBConfigDTO>("tbconfig_api/Get_DataList", param);

            return objList;
        }
    }
}
