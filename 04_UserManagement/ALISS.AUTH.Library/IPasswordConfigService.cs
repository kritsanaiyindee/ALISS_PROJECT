using ALISS.AUTH.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALISS.AUTH.Library
{
    public interface IPasswordConfigService
    {
        PasswordConfigDTO GetData(string pwc_Id);
        PasswordConfigDTO SaveData(PasswordConfigDTO model);
    }
}
