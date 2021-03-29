using ALISS.MasterManagement.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.MasterManagement.Library
{
    public interface IWHONETColumnService
    {
        List<WHONETColumnDTO> GetList();
        List<WHONETColumnDTO> GetListWithParam(string param);
        List<WHONETColumnDTO> GetListWithModel(WHONETColumnDTO searchModel);
        List<WHONETColumnDTO> GetList_Active_WithModel(WHONETColumnDTO searchModel);
        WHONETColumnDTO GetData(string hos_code);
        WHONETColumnDTO SaveData(WHONETColumnDTO model);
        string CheckDuplicate(WHONETColumnDTO model);
    }
}
