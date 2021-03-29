using ALISS.MasterManagement.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.MasterManagement.Library
{
    public interface IQCRangeService
    {
        List<QCRangeDTO> GetList();
        List<QCRangeDTO> GetListWithParam(string param);
        List<QCRangeDTO> GetListWithModel(QCRangeDTO searchModel);
        List<QCRangeDTO> GetList_Active_WithModel(QCRangeDTO searchModel);
        QCRangeDTO GetData(string hos_code);
        QCRangeDTO SaveData(QCRangeDTO model);
        string CheckDuplicate(QCRangeDTO model);
    }
}
