using ALISS.ANTIBIOGRAM.DTO;
using ALISS.ANTIBIOGRAM.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.ANTIBIOGRAM.Library
{
    public interface IReportService
    {    
        List<AntibiogramDataDTO> GetAntibiogramHospitalData();
        List<AntibiogramDataDTO> GetAntibiogramHospitalDataWithParam(string param);
        List<AntibiogramDataDTO> GetAntibiogramHospitalDataWithModel(AntiHospitalSearchDTO searchModel);
        List<AntibiogramDataDTO> GetAntibiogramAreaHealthData();
        List<AntibiogramDataDTO> GetAntibiogramAreaHealthDataWithParam(string param);
        List<AntibiogramDataDTO> GetAntibiogramAreaHealthDataWithModel(AntiAreaHealthSearchDTO searchModel);
        List<AntibiogramDataDTO> GetAntibiogramNationData();
        List<AntibiogramDataDTO> GetAntibiogramNationDataWithParam(string param);
        List<AntibiogramDataDTO> GetAntibiogramNationDataWithModel(AntiNationSearchDTO searchModel);
        List<RPYearlyIsolateListingRISDTO> GetYearlyIsolateListingRIS();
        List<RPYearlyIsolateListingRISDetailDTO> GetYearlyIsolateListingRISDetail();
    }
}
