using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace SUG.Essentials
{
    [Service(lifetime = ServiceLifetime.Global)]
    public sealed class SceneManager : MonoBehaviour, ISceneService
    {
        // Event SceneManager.sceneLoaded和SceneManager.sceneUnloaded的上层接口
        public event Action<Scene, LoadSceneMode> sceneLoaded;
        public event Action<Scene> sceneUnloaded;

        // 当前场景
        public SceneInstance currSc;

        // 上一个场景
        public SceneInstance lastSc;

        // 加载场景
        public AssetReference loadingSc;

        #region 声明周期
        private void Awake()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (s, l) => sceneLoaded?.Invoke(s,l);
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded += (s) => sceneUnloaded?.Invoke(s);
        }

        #endregion

        /// <summary>
        /// 异步切换场景
        /// </summary>
        /// <param name="scene"> 新场景 </param>
        /// <param name="useLoading"> 是否显示【加载场景】？ </param>
        public void LoadSceneAsync(AssetReference newScene, bool useLoading = false, LoadSceneMode mode = LoadSceneMode.Single)
        {
            StartCoroutine(LoadSceneDelayed(newScene, useLoading, mode));
        }

        private IEnumerator LoadSceneDelayed(AssetReference newScene, bool useLoading, LoadSceneMode mode)
        {
            // 当前场景变为老场景
            lastSc = currSc;

            // 加载场景界面显示
            AsyncOperationHandle<SceneInstance> loadingHandle = default; // 初始化，避免未赋值使用
            if (useLoading && loadingSc != null)
            {
                loadingHandle = Addressables.LoadSceneAsync(loadingSc, LoadSceneMode.Additive, false);
                yield return loadingHandle;
                loadingHandle.Result.ActivateAsync();
            }

            // 如果不是Additive，那就卸载lastSc
            if (mode != LoadSceneMode.Additive)
            {
                AsyncOperationHandle<SceneInstance> unload = UnloadSceneAsync(lastSc);
                yield return unload;
            }

            // 加载新场景并激活
            AsyncOperationHandle<SceneInstance> newLoad =
                Addressables.LoadSceneAsync(newScene, LoadSceneMode.Additive, false);

            // 等待加载完成
            while (!newLoad.IsValid() && !newLoad.IsDone)
            {
                yield return null;
            }

            // 准备切换场景清空场景DI容器
            ServiceRegistry.ClearScene();
            yield return new WaitForSeconds(1.0f);

            // 激活场景
            newLoad.Result.ActivateAsync();
            while (!newLoad.Result.Scene.isLoaded)
            {
                yield return null;
            }

            // 设置新的当前场景
            currSc = newLoad.Result;

            // 仅当 loadingHandle 有效时才卸载 loading 场景
            if (useLoading && loadingSc != null && loadingHandle.IsValid())
            {
                var unload = UnloadSceneAsync(loadingHandle.Result);
                yield return unload;
            }
        }

        public AsyncOperationHandle<SceneInstance> UnloadSceneAsync(SceneInstance scene)
        {
            return Addressables.UnloadSceneAsync(scene);
        }
    }
}
