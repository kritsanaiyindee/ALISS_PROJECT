using ALISS.Data.Client;
using ALISS.Master.DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ALISS.Data.D0_Master
{
    public class HomeService
    {
        private IConfiguration Configuration { get; }

        private ApiHelper _apiHelper;

        public HomeService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }

        public async Task<List<TRNoticeMessageDTO>> GetListByModelAsync(TRNoticeMessageDTO searchData)
        {
            List<TRNoticeMessageDTO> objList = new List<TRNoticeMessageDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<TRNoticeMessageDTO, TRNoticeMessageDTO>("noticemessage_api/Get_ListByModel", searchData);

            return objList;
        }

    }
}
