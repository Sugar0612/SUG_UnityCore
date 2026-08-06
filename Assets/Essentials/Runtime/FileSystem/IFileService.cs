using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUG.Essentials
{
    [Injectable] public interface IFileService
    {
        bool Exists(string path);

        void ReadText(string path, Action<string> onCompleted);

        void ReadBytes(string path, Action<byte[]> onCompleted);

        void WriteText(string path, string text, Action<bool> onCompleted = null);

        void WriteBytes(string path, byte[] bytes, Action<bool> onCompleted = null);

        void Delete(string path);

        void CreateDirectory(string path);

        // =====================
        // Coroutine
        // =====================
        public IEnumerator ReadTextCoroutine(string path, Action<string> callback);
        public IEnumerator ReadBytesCoroutine(string path, Action<byte[]> callback);
        public IEnumerator WriteTextCoroutine(string path, string text, Action<bool> callback);
        public IEnumerator WriteBytesCoroutine(string path, byte[] bytes, Action<bool> callback);
    }
}