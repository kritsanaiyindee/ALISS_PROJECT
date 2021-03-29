using ALISS.Data.Client;
using ALISS.Master.DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALISS.Data.D0_Master
{
    public class WHONETService
    {
        private IConfiguration Configuration { get; }

        private ApiHelper _apiHelper;

        public WHONETService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }

        public async Task<List<TCWHONET_AntibioticsDTO>> GetDataList_TCWHONET_Antibiotics_Async()
        {
            List<TCWHONET_AntibioticsDTO> objList = new List<TCWHONET_AntibioticsDTO>();

            objList = await _apiHelper.GetDataListAsync<TCWHONET_AntibioticsDTO>("whonetdata_api/WHONET_Antibiotics_Active_List");

            return objList;
        }
    }
}
