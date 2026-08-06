using SUG.Essentials;
using System.IO;
using UnityEngine;

namespace SUG.Essentials
{
    using System;
    using System.Collections;
    using System.IO;
    using UnityEngine;
    using UnityEngine.Networking;

    namespace SUG.Essentials
    {
        /// <summary>
        /// 文件服务。
        /// </summary>
        [Service(ServiceLifetime.Global)] public sealed class FileManager : MonoBehaviour, IFileService
        {
            #region Public

            public bool Exists(string path)
            {
#if UNITY_WEBGL && !UNITY_EDITOR
            return !path.StartsWith(Application.streamingAssetsPath) && File.Exists(path);
#else
                return File.Exists(path);
#endif
            }

            /// <summary>
            /// 读取文本文件
            /// </summary>
            /// <param name="path"></param>
            /// <param name="onCompleted"></param>
            public void ReadText(string path, Action<string> onCompleted)
            {
                StartCoroutine(ReadTextCoroutine(path, onCompleted));
            }

            /// <summary>
            /// 读取二进制文件
            /// </summary>
            /// <param name="path"></param>
            /// <param name="onCompleted"></param>
            public void ReadBytes(string path, Action<byte[]> onCompleted)
            {
                StartCoroutine(ReadBytesCoroutine(path, onCompleted));
            }

            /// <summary>
            /// 文本文件写入
            /// </summary>
            /// <param name="path"></param>
            /// <param name="text"></param>
            /// <param name="onCompleted"></param>
            public void WriteText(string path, string text, Action<bool> onCompleted = null)
            {
                StartCoroutine(WriteTextCoroutine(path, text, onCompleted));
            }

            /// <summary>
            /// 二进制文件写入
            /// </summary>
            /// <param name="path"></param>
            /// <param name="bytes"></param>
            /// <param name="onCompleted"></param>
            public void WriteBytes(string path, byte[] bytes, Action<bool> onCompleted = null)
            {
                StartCoroutine(WriteBytesCoroutine(path, bytes, onCompleted));
            }

            /// <summary>
            /// 删除文件
            /// </summary>
            /// <param name="path"></param>
            public void Delete(string path)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }

            /// <summary>
            /// 创建目录
            /// </summary>
            /// <param name="path"></param>
            public void CreateDirectory(string path)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }

            #endregion

            #region Coroutine

            /// <summary>
            /// 文本文件读取协程
            /// </summary>
            /// <param name="path"></param>
            /// <param name="callback"></param>
            /// <returns></returns>
            public IEnumerator ReadTextCoroutine(string path, Action<string> callback)
            {
#if UNITY_WEBGL && !UNITY_EDITOR

            using UnityWebRequest request = UnityWebRequest.Get(path);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
                callback?.Invoke(string.Empty);
            }
            else
            {
                callback?.Invoke(request.downloadHandler.text);
            }

#else

                if (!File.Exists(path))
                {
                    Debug.LogError($"[FileManager] File not found : {path}");
                    callback?.Invoke(string.Empty);
                    yield break;
                }

                callback?.Invoke(File.ReadAllText(path));

                yield break;

#endif
            }

            /// <summary>
            /// 读取二进制文件协程
            /// </summary>
            /// <param name="path"></param>
            /// <param name="callback"></param>
            /// <returns></returns>
            public IEnumerator ReadBytesCoroutine(string path, Action<byte[]> callback)
            {
#if UNITY_WEBGL && !UNITY_EDITOR

            using UnityWebRequest request = UnityWebRequest.Get(path);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
                callback?.Invoke(null);
            }
            else
            {
                callback?.Invoke(request.downloadHandler.data);
            }

#else

                if (!File.Exists(path))
                {
                    Debug.LogError($"[FileManager] File not found : {path}");
                    callback?.Invoke(null);
                    yield break;
                }

                callback?.Invoke(File.ReadAllBytes(path));

                yield break;

#endif
            }

            /// <summary>
            /// 写入文本文件协程
            /// </summary>
            /// <param name="path"></param>
            /// <param name="text"></param>
            /// <param name="callback"></param>
            /// <returns></returns>
            public IEnumerator WriteTextCoroutine(string path, string text, Action<bool> callback)
            {
                bool success = true;

                try
                {
                    EnsureDirectory(path);
                    File.WriteAllText(path, text);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    success = false;
                }

                callback?.Invoke(success);

                yield break;
            }

            /// <summary>
            /// 二进制文件写入协程
            /// </summary>
            /// <param name="path"></param>
            /// <param name="bytes"></param>
            /// <param name="callback"></param>
            /// <returns></returns>
            public IEnumerator WriteBytesCoroutine(string path, byte[] bytes, Action<bool> callback)
            {
                bool success = true;

                try
                {
                    EnsureDirectory(path);
                    File.WriteAllBytes(path, bytes);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    success = false;
                }

                callback?.Invoke(success);

                yield break;
            }

            #endregion

            #region Utility

            /// <summary>
            /// 创建父目录
            /// </summary>
            /// <param name="filePath"></param>
            private void EnsureDirectory(string filePath)
            {
                string directory = Path.GetDirectoryName(filePath);

                if (!string.IsNullOrEmpty(directory) &&
                    !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }

            #endregion
        }
    }
}
