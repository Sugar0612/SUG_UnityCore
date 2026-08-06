using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using UnityEngine;

namespace SUG.Essentials
{
    public class FileWriter : IFileWriter
    {
        private readonly string _logPath;
        private readonly ConcurrentQueue<string> _writeQueue = new();
        private readonly Thread _writeThread;
        private bool _isRunning = true;

        public FileWriter()
        {
            _logPath = Path.Combine(Application.persistentDataPath, "Logs", $"log_{DateTime.Now:yyyyMMdd}.txt");

        #if !UNITY_WEBGL
            _writeThread = new Thread(WriteLoop);
            _writeThread.IsBackground = true;
            _writeThread.Start();
        #endif
        }

        public void Write(string message)
        {
            string final = $"[{DateTime.Now:HH:mm:ss}] {message}\n";
        #if UNITY_WEBGL
            string dir = Path.GetDirectoryName(_logPath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            File.AppendAllText(_logPath, final);
        #else
            _writeQueue.Enqueue(final);
        #endif
        }

        private void WriteLoop()
        {
            while (_isRunning)
            {
                if (_writeQueue.TryDequeue(out var msg))
                {
                    string dir = Path.GetDirectoryName(_logPath);
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    File.AppendAllText(_logPath, msg);
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
        }

        public void Dispose()
        {
        #if !UNITY_WEBGL
            _isRunning = false;
            _writeThread.Join();
        #endif
        }
    }
}