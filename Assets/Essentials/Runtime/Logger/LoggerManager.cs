using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SUG.Essentials
{
    [Flags]
    public enum LogChannel
    {
        None = 0,
        Unity = 1 << 0,
        File = 1 << 1,
    }

    public enum LogStatus
    {
        Info,
        Warning,
        Error
    }

    public static class EssLog
    {
        // —— ILogger ——
        private static UnityLogger _unityLogger;
        private static FileLogger _fileLogger;

        public static void Initialization()
        {
            if (_unityLogger == null) _unityLogger = new UnityLogger();

#if !UNITY_WEBGL
            if (_fileLogger == null) _fileLogger = new FileLogger();
#endif
        }

        // =====================
        // 对外日志方法
        // =====================
        private static void LogInfo(string mess, LogChannel channel = LogChannel.Unity)
        {
            Initialization();
            if ((channel & LogChannel.Unity) != 0) _unityLogger?.LogInfo(mess);
            if ((channel & LogChannel.File) != 0)  _fileLogger?.LogInfo(mess);
        }

        private static void LogWarning(string mess, LogChannel channel = LogChannel.Unity)
        {
            Initialization();
            if ((channel & LogChannel.Unity) != 0) _unityLogger?.LogWarning(mess);
            if ((channel & LogChannel.File) != 0)  _fileLogger?.LogWarning(mess);
        }

        private static void LogError(string mess, LogChannel channel = LogChannel.Unity)
        {
            Initialization();
            if ((channel & LogChannel.Unity) != 0) _unityLogger?.LogError(mess);
            if ((channel & LogChannel.File) != 0)  _fileLogger?.LogError(mess);
        }

        #region Public interface

        public static void Info(string mess, LogChannel channel = LogChannel.Unity) => LogInfo(mess, channel);
        public static void Warning(string mess, LogChannel channel = LogChannel.Unity) => LogWarning(mess, channel);
        public static void Error(string mess, LogChannel channel = LogChannel.Unity) => LogError(mess, channel);

        /// <summary> 特殊方法，可以知道这个LOG是那个文件，哪一行的LOG </summary>
        public static void Details(string ms, [CallerMemberName] string m = "", [CallerFilePath] string f = "", [CallerLineNumber] int l = 0)
        {
            Debug.Log($"[{System.IO.Path.GetFileName(f)}:{l} - {m}] {ms}");
        }

        #endregion
    }
}