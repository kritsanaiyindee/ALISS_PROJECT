using ALISS.HISUpload.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.HISUpload.Library
{
    public interface IHISFileUploadService
    {
        HISUploadDataDTO GetHISFileUploadDataById(int hfu_id);
        List<HISUploadDataDTO> GetHISFileUploadListWithModel(HISUploadDataSearchDTO searchModel);
        List<HISFileTemplateDTO> GetHISFileTemplate_Actice_WithModel(HISFileTemplateDTO searchModel);
        HISUploadDataDTO SaveLabFileUploadData(HISUploadDataDTO model);
        HISFileUploadSummaryDTO SaveFileUploadSummary(List<HISFileUploadSummaryDTO> models);
        List<HISFileUploadSummaryDTO> GetHISFileUploadSummaryByUploadId(int HISUploadFileid);
        List<LabDataWithHISDTO> GetLabDataWithHIS(LabDataWithHISSearchDTO searchModel);
    }
}
