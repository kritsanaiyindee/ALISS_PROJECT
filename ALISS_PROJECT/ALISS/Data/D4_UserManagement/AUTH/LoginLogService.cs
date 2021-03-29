using ALISS.AUTH.DTO;
using ALISS.Data.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALISS.Data.D4_UserManagement.AUTH
{
    public class LoginLogService
    {
        private IConfiguration Configuration { get; }

        private ApiHelper _apiHelper;

        public LoginLogService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }

        public async Task<List<LogUserLoginDTO>> GetListByModelAsync(LogUserLoginDTO searchData)
        {
            List<LogUserLoginDTO> objList = new List<LogUserLoginDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<LogUserLoginDTO, LogUserLoginDTO>("loginlog_api/Get_ListByModel", searchData);

            return objList;
        }
    }
}
