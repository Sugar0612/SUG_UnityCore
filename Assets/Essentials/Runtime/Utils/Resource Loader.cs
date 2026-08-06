using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 资源加载工具
/// </summary>
namespace SUG.Essentials
{
    public class ResourceLoader : Singleton<ResourceLoader, SingletonGlobal>
    {
        // —— 资源加载 ——
        public T TryLoadObject<T>(string path) where T : UnityEngine.Object
        {
            T obj = Resources.Load<T>(path);
            return obj;
        }

        public void TryLoadObjectAsync<T>(string path, Action<T> onLoad) where T : UnityEngine.Object
        {
            StartCoroutine(LoadObjectAsyncSquence<T>(path, onLoad));
        }

        private IEnumerator LoadObjectAsyncSquence<T>(string path, Action<T> onLoad) where T : UnityEngine.Object
        {
            ResourceRequest request = Resources.LoadAsync<T>(path);
            yield return request;

            if (request.asset != null)
            {
                onLoad?.Invoke(request.asset as T);
            }
        }

        // —— 实例化 ——
        public T InstantiateInTheScene<T>(GameObject prefab, Transform parent,
            Vector3 position, Vector3 eulerAngles, Vector3 scale) where T : MonoBehaviour
        {
            if (!prefab) return null;
            GameObject go = GameObject.Instantiate(prefab, parent);
            go.transform.localPosition = position;
            go.transform.localEulerAngles = eulerAngles;
            go.transform.localScale = scale;
            return go?.GetComponent<T>();
        }
    }
}
