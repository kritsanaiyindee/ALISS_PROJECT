using ALISS.Data.Client;
using ALISS.DropDownList.DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALISS.Data.D0_Master
{
    public class DropDownListDataService
    {
        private IConfiguration Configuration { get; }

        private ApiHelper _apiHelper;

        public DropDownListDataService(IConfiguration configuration)
        {
            _apiHelper = new ApiHelper(configuration["ApiClient:ApiUrl"]);
        }

        public async Task<List<HospitalDataDTO>> GetAreaHealthListByModelAsync(HospitalDataDTO searchModel)
        {
            List<HospitalDataDTO> objList = new List<HospitalDataDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<HospitalDataDTO, HospitalDataDTO>("dropdownlist_api/GetAreaHealthList", searchModel);

            return objList;
        }

        public async Task<List<HospitalDataDTO>> GetProvinceListByModelAsync(HospitalDataDTO searchModel)
        {
            List<HospitalDataDTO> objList = new List<HospitalDataDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<HospitalDataDTO, HospitalDataDTO>("dropdownlist_api/GetProvinceList", searchModel);

            return objList;
        }

        public async Task<List<HospitalDataDTO>> GetHospitalListByModelAsync(HospitalDataDTO searchModel)
        {
            List<HospitalDataDTO> objList = new List<HospitalDataDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<HospitalDataDTO, HospitalDataDTO>("dropdownlist_api/GetHospitalList", searchModel);

            return objList;
        }

        public async Task<List<HospitalDataDTO>> GetTCHospitalListByModelAsync(HospitalDataDTO searchModel)
        {
            List<HospitalDataDTO> objList = new List<HospitalDataDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<HospitalDataDTO, HospitalDataDTO>("dropdownlist_api/GetTCHospitalList", searchModel);

            return objList;
        }

        public async Task<List<DropDownListDTO>> GetRoleListByModelAsync()
        {
            List<DropDownListDTO> objList = new List<DropDownListDTO>();
            var searchModel = new DropDownListDTO();

            objList = await _apiHelper.GetDataListByModelAsync<DropDownListDTO, DropDownListDTO>("dropdownlist_api/GetRoleList", searchModel);

            return objList;
        }

        public async Task<List<ParameterDTO>> GetParameterListByModelAsync(string prm_code_major)
        {
            List<ParameterDTO> objList = new List<ParameterDTO>();
            var searchModel = new ParameterDTO() { prm_code_major = prm_code_major };

            objList = await _apiHelper.GetDataListByModelAsync<ParameterDTO, ParameterDTO>("dropdownlist_api/GetParameterList", searchModel);

            return objList;
        }

        public async Task<List<DropDownListDTO>> GetLabListByModelAsync(DropDownListDTO searchModel)
        {
            List<DropDownListDTO> objList = new List<DropDownListDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<DropDownListDTO, DropDownListDTO>("dropdownlist_api/GetLabList", searchModel);

            return objList;
        }

        public async Task<List<HospitalLabDataDTO>> GetAllLabListByModelAsync(HospitalLabDataDTO searchModel)
        {
            List<HospitalLabDataDTO> objList = new List<HospitalLabDataDTO>();

            objList = await _apiHelper.GetDataListByModelAsync<HospitalLabDataDTO, HospitalLabDataDTO>("dropdownlist_api/GetAllLabList", searchModel);

            return objList;
        }

        public List<DropDownListDTO> GetMonthList()
        {
            List<DropDownListDTO> objList = new List<DropDownListDTO>();

            objList.Add(new DropDownListDTO() { data_id = "01", data_Value = "01", data_Text = "มกราคม" });
            objList.Add(new DropDownListDTO() { data_id = "02", data_Value = "02", data_Text = "กุมภาพันธ์" });
            objList.Add(new DropDownListDTO() { data_id = "03", data_Value = "03", data_Text = "มีนาคม" });
            objList.Add(new DropDownListDTO() { data_id = "04", data_Value = "04", data_Text = "เมษายน" });
            objList.Add(new DropDownListDTO() { data_id = "05", data_Value = "05", data_Text = "พฤษภาคม" });
            objList.Add(new DropDownListDTO() { data_id = "06", data_Value = "06", data_Text = "มิถุนายน" });
            objList.Add(new DropDownListDTO() { data_id = "07", data_Value = "07", data_Text = "กรกฎาคม" });
            objList.Add(new DropDownListDTO() { data_id = "08", data_Value = "08", data_Text = "สิงหาคม" });
            objList.Add(new DropDownListDTO() { data_id = "09", data_Value = "09", data_Text = "กันยายน" });
            objList.Add(new DropDownListDTO() { data_id = "10", data_Value = "10", data_Text = "ตุลาคม" });
            objList.Add(new DropDownListDTO() { data_id = "11", data_Value = "11", data_Text = "พฤศจิกายน" });
            objList.Add(new DropDownListDTO() { data_id = "12", data_Value = "12", data_Text = "ธันวาคม" });

            return objList;
        }

        public List<DropDownListDTO> GetYearList(string year = "0")
        {
            List<DropDownListDTO> objList = new List<DropDownListDTO>();

            var startYear = Convert.ToInt32(year);

            if ((startYear == 0) || (startYear + 10 > DateTime.Now.Year)) startYear = DateTime.Now.Year;
            else startYear = startYear + 10;

            for (var i = startYear; i >= startYear - 20; i--)
            {
                objList.Add(new DropDownListDTO() { data_id = i.ToString(), data_Value = i.ToString(), data_Text = i.ToString() });
            }

            return objList;
        }
    }
}
