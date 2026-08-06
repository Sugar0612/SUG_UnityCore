using System.Runtime.CompilerServices;
using UnityEngine;
namespace SUG.Essentials
{
    public class FileLogger : ILogger
    {
        private IFileWriter _writer = new FileWriter();
        public void LogInfo(string mess) => _writer.Write($"[INFO] {mess}");
        public void LogWarning(string mess) => _writer.Write($"[WARNING] {mess}");
        public void LogError(string mess) => _writer.Write($"[ERROR] {mess}");
        public void LogDetails(string mess, [CallerMemberName] string member = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            string details = $"[{System.IO.Path.GetFileName(file)}:{line} - {member}] {mess}";
            _writer.Write($"[STACK] {mess}");
        }
    }
}