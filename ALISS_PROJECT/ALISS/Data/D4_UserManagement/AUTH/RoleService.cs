using ALISS.AUTH.DTO;
using ALISS.Data.Client;
using ALISS.Master.DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALISS.Data.D4_UserManagement.AUTH
{
    public class RoleService
    {
        private IConfiguration Configuration { get; }

        private ApiHelper _apiHelper;

        public RoleService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }

        public async Task<List<RoleDTO>> GetRoleListAsync()
        {
            List<RoleDTO> objList = new List<RoleDTO>();

            objList = await _apiHelper.GetDataListAsync<RoleDTO>("role_api/Get_List");

            return objList;
        }

        public async Task<List<RoleDTO>> GetRoleListAsync(string rol_text)
        {
            List<RoleDTO> objList = new List<RoleDTO>();

            objList = await _apiHelper.GetDataListByParamsAsync<RoleDTO>("role_api/Get_List", rol_text);

            return objList;
        }

        public async Task<List<RoleDTO>> GetListByModelAsync(RoleSearchDTO searchData)
        {
            List<RoleDTO> objList = new List<RoleDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<RoleDTO, RoleSearchDTO>("role_api/Get_ListByModel", searchData);

            return objList;
        }

        public async Task<RoleDTO> GetRoleDataAsync(string rol_code)
        {
            RoleDTO objData = new RoleDTO();

            objData = await _apiHelper.GetDataByIdAsync<RoleDTO>("role_api/Get_Data", rol_code);

            return objData;
        }

        public async Task<RoleDTO> SaveDataAsync(RoleDTO model)
        {
            //MenuDTO menu = new MenuDTO()
            //{
            //    mnu_id = "2",
            //    mnu_name = "TEST_name",
            //    mnu_area = "TEST_area",
            //    mnu_controller = "TEST_controller"
            //};

            var menua = await _apiHelper.PostDataAsync<RoleDTO>("role_api/Post_SaveData", model);

            return menua;
        }

        public async Task<List<RolePermissionDTO>> GetRolePermissionListByModelAsync(string role_code)
        {
            List<RolePermissionDTO> objList = new List<RolePermissionDTO>();

            RoleSearchDTO searchData = new RoleSearchDTO() { sch_rol_code = role_code };

            objList = await _apiHelper.GetDataListByModelAsync<RolePermissionDTO, RoleSearchDTO>("role_api/Get_PermissionListByModel", searchData);

            return objList;
        }

        public async Task<List<RolePermissionDTO>> SaveListDataAsync(List<RolePermissionDTO> modelList)
        {
            //MenuDTO menu = new MenuDTO()
            //{
            //    mnu_id = "2",
            //    mnu_name = "TEST_name",
            //    mnu_area = "TEST_area",
            //    mnu_controller = "TEST_controller"
            //};

            var menua = await _apiHelper.PostDataAsync<List<RolePermissionDTO>>("role_api/Post_SaveListData", modelList);

            return menua;
        }

        public async Task<List<LogProcessDTO>> GetHistoryAsync(string tran_id)
        {
            List<LogProcessDTO> objList = new List<LogProcessDTO>();

            LogProcessSearchDTO searchData = new LogProcessSearchDTO() { log_mnu_name = "Role", log_tran_id = tran_id };

            objList = await _apiHelper.GetDataListByModelAsync<LogProcessDTO, LogProcessSearchDTO>("logprocess_api/GetAuth_List", searchData);

            return objList;
        }

        public Task<GridData[]> GetHistoryAsyncTmp()
        {
            var rng = new Random();
            var objReturn = new List<GridData>(){
                new GridData { Column_01 = "(A0000)Super admin", Column_02 = "Active role", Column_03 = "Lorem", Column_04 = "00/00/00 00:00", Column_05 = "" },
                new GridData { Column_01 = "(A0000)Super admin", Column_02 = "Inactive role", Column_03 = "Lorem", Column_04 = "00/00/00 00:00", Column_05 = "" },
            };
            return Task.FromResult(objReturn.ToArray());
        }
    }
}
