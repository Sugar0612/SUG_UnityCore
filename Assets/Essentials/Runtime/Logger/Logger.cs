using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SUG.Essentials
{
    public interface ILogger
    {
        void LogInfo(string mess);
        void LogWarning(string mess);
        void LogError(string mess);
        void LogDetails(string mess, [CallerMemberName] string member = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0);       
    }
}