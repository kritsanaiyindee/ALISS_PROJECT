using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Log4NetLibrary
{
    public interface ILogService
    {
        void Fatal(string message);
        void Error(string message);
        void Error(Exception message);
        void Warn(string message);
        void Info(string message);
        void Debug(string message);
        void MethodStart([CallerMemberName] string callerName = "");
        void MethodFinish([CallerMemberName] string callerName = "");
    }
}
