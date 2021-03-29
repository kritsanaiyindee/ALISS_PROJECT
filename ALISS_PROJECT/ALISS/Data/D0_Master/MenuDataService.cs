using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ALISS.Data.Client;
using ALISS.LoginManagement.DTO;
using System.Text.Json;

namespace ALISS.Data.D0_Master
{
    public class MenuDataService
    {
        private IConfiguration Configuration { get; }

        private ApiHelper _apiHelper;

        public MenuDataService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }

        public async Task<List<MenuDataDTO>> GetDataList_Menu_Async()
        {
            List<MenuDataDTO> objList = new List<MenuDataDTO>();

            objList = await _apiHelper.GetDataListAsync<MenuDataDTO>("menudata_api/GetDataList");

            foreach (var _menu in objList)
            {
                if (string.IsNullOrEmpty(_menu.strSubMenuDataDTO) == false)
                {
                    _menu.SubMenuDataDTO = JsonSerializer.Deserialize<List<MenuDataDTO>>(_menu.strSubMenuDataDTO);
                }
            }

            return objList;
        }

        public async Task<List<MenuDataDTO>> GetDataList_Menu_Async(string rol_code)
        {
            List<MenuDataDTO> objList = new List<MenuDataDTO>();

            objList = await _apiHelper.GetDataListByParamsAsync<MenuDataDTO>("menudata_api/GetDataList", rol_code);

            foreach (var _menu in objList)
            {
                if (string.IsNullOrEmpty(_menu.strSubMenuDataDTO) == false)
                {
                    _menu.SubMenuDataDTO = JsonSerializer.Deserialize<List<MenuDataDTO>>(_menu.strSubMenuDataDTO);
                }
            }

            return objList;
        }
    }
}
