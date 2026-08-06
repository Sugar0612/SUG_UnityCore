using System.Runtime.CompilerServices;
using UnityEngine;
namespace SUG.Essentials
{
    public class UnityLogger : ILogger
    {
        public void LogInfo(string mess) => Debug.Log($"{mess}");
        public void LogWarning(string mess) => Debug.LogWarning($"{mess}");
        public void LogError(string mess) => Debug.LogError($"{mess}");
        public void LogDetails(string mess, [CallerMemberName] string member = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            Debug.Log($"[{System.IO.Path.GetFileName(file)}:{line} - {member}] {mess}");
        }
    }
}