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
        Normal,
        Warning,
        Error,
        Details
    }

    public static class LogHelper
    {
        // —— ILogger ——
        private static UnityLogger _unityLogger;
        private static FileLogger _fileLogger;

        public static void Init()
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
            if ((channel & LogChannel.Unity) != 0) _unityLogger?.LogInfo(mess);
            if ((channel & LogChannel.File) != 0)  _fileLogger?.LogInfo(mess);
        }

        private static void LogWarning(string mess, LogChannel channel = LogChannel.Unity)
        {
            if ((channel & LogChannel.Unity) != 0) _unityLogger?.LogWarning(mess);
            if ((channel & LogChannel.File) != 0)  _fileLogger?.LogWarning(mess);
        }

        private static void LogError(string mess, LogChannel channel = LogChannel.Unity)
        {
            if ((channel & LogChannel.Unity) != 0) _unityLogger?.LogError(mess);
            if ((channel & LogChannel.File) != 0)  _fileLogger?.LogError(mess);
        }

        private static void LogDetails(string mess, LogChannel channel = LogChannel.Unity)
        {
            if ((channel & LogChannel.Unity) != 0) _unityLogger?.LogDetails(mess);
            if ((channel & LogChannel.File) != 0)  _fileLogger?.LogDetails(mess);
        }

        public static void Log(string mess, LogChannel channel = LogChannel.Unity, LogStatus status = LogStatus.Normal)
        {
            Init();
            switch (status)
            {
                case LogStatus.Normal:
                    LogInfo(mess, channel);
                    break;
                case LogStatus.Warning:
                    LogWarning(mess, channel);
                    break;
                case LogStatus.Error:
                    LogError(mess, channel);
                    break;
                case LogStatus.Details:
                    LogDetails(mess, channel);
                    break;
            }
        }
    }
}