using System;
using System.Collections.Generic;
using System.Text;
using ALISS.LabFileUpload.DTO;

namespace ALISS.LabFileUpload.Library
{
    public interface ILabFileUploadService
    {
        LabFileUploadDataDTO SaveLabFileUploadData(LabFileUploadDataDTO model);

        LabFileUploadDataDTO GetLabFileUploadDataById(string lfu_id);

        List<LabFileUploadDataDTO> GetLabFileUploadListWithModel(LabFileUploadSearchDTO model);

        List<LabFileErrorHeaderListDTO> GetLabFileErrorHeaderListBylfuId(string lfu_id);
        List<LabFileErrorDetailListDTO> GetLabFileErrorDetailListBylfuId(string lfu_id);
        List<LabFileSummaryHeaderListDTO> GetLabFileSummaryHeaderBylfuId(string lfu_id);
        List<LabFileSummaryDetailListDTO> GetLabFileSummaryDetailBylfuId(string fsh_id);


    }
}
