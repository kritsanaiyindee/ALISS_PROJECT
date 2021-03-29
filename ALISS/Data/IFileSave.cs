using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALISS.Data
{
    public interface IFileSave
    {
        Task DownloadFile(string ServerFileName, string OutputFileName, string contentType);       
    }
}
