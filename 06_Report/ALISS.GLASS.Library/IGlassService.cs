using ALISS.DropDownList.DTO;
using ALISS.GLASS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.GLASS.Library
{
    public interface IGlassService
    {
        List<GlassFileListDTO> GetGlassPublicFileListData();
        List<GlassFileListDTO> GetGlassPublicFileListDataModel(GlassFileListNationSearchDTO searchModel);
        List<GlassFileListDTO> GetGlassPublicRegHealthFileListDataModel(GlassFileListNationSearchDTO searchModel);
        List<GlassInfectOriginOverviewDTO> GetGlassInfectOverviewModel(GlassFileListDTO searchModel);
        List<GlassPathogenNSDTO> GetGlassPathogenNSModel(GlassFileListDTO searchModel);
        List<GlassInfectSpecimenDTO> GlassInfectSpecimenModel(GlassFileListDTO searchModel);
        List<GlassInfectPathAntiCombineDTO> GetGlassInfectPathAntiCombineModel(GlassFileListDTO searchModel);
        List<ParameterDTO> GetGlassReportPath(ParameterDTO searchModel);
    }
}
